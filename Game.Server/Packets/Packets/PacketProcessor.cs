using Game.Base.Events;
using Game.Server;
using Game.Server.Packets;
using Game.Server.Packets.Client;
using log4net;
using System;
using System.Configuration;
using System.Reflection;
using System.Threading;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;

namespace Game.Base.Packets
{
    public class PacketProcessor
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Thread-safe packet handler storage
        protected static readonly ConcurrentDictionary<int, IPacketHandler> m_packetHandlers =
            new ConcurrentDictionary<int, IPacketHandler>();

        // Maximum packet size to prevent memory exhaustion attacks
        private const int MAX_PACKET_SIZE = 1024 * 1024; // 1MB

        // Performance tracking
        private static readonly Stopwatch performanceTimer = new Stopwatch();
        private static long lastGCCollectionTime = 0;
        private const int GC_COLLECTION_INTERVAL = 300000; // 5 minutes in milliseconds

        // Rate limiting
        private static readonly ConcurrentDictionary<string, ClientRateInfo> clientRateLimiters =
            new ConcurrentDictionary<string, ClientRateInfo>();
        private const int MAX_PACKETS_PER_SECOND = 5000;
        private const int RATE_LIMIT_WINDOW_MS = 1000;

        // Client information
        protected GameClient m_client;
        protected int m_handlerThreadID;
        protected IPacketHandler m_activePacketHandler;

        // Rate limiting info structure
        private class ClientRateInfo
        {
            public int PacketCount { get; set; }
            public long LastResetTime { get; set; }
        }

        public PacketProcessor(GameClient client)
        {
            m_client = client ?? throw new ArgumentNullException(nameof(client));
        }

        /// <summary>
        /// Handles incoming packets with security checks and performance monitoring
        /// </summary>
        public void HandlePacket(GSPacketIn packet)
        {
            if (packet == null)
            {
                log.Error("Null packet received");
                return;
            }

            int code = packet.Code;

            // Update statistics
            Statistics.BytesIn += packet.Length;
            Statistics.PacketsIn++;

            // Security check: packet size validation
            if (packet.Length > MAX_PACKET_SIZE)
            {
                log.Warn($"Oversized packet received from {m_client.TcpEndpoint}. Size: {packet.Length}, Code: {code}");
                return;
            }

            // Security check: rate limiting
            if (!IsClientAllowedToSendPacket(m_client.TcpEndpoint))
            {
                log.Warn($"Oyuncu {m_client.TcpEndpoint} paket limitlerini aştı. Paket kodu: {code}. İyice bi kontrol et bakalım handlerlardan, nesi yanlış? nesi değil? not: yutikeyu");
                return;
            }

            // Get the appropriate packet handler
            if (!m_packetHandlers.TryGetValue(code, out IPacketHandler packetHandler))
            {
                if (log.IsErrorEnabled)
                {
                    log.ErrorFormat("Received packet code {0} has no registered handler! Client: {1}",
                        code, m_client.ToString());
                    log.Error(Marshal.ToHexDump(string.Format("===> <{2}> Packet 0x{0:X2} (0x{1:X2}) length: {3} (ThreadId={4})",
                        code, code ^ 0xA8, m_client.TcpEndpoint, packet.Length, Thread.CurrentThread.ManagedThreadId),
                        packet.Buffer));
                }
                return;
            }

            // Process the packet with performance monitoring
            ProcessPacketWithHandler(packet, packetHandler);

            // Periodic memory cleanup
            PerformPeriodicGCCollection();
        }

        /// <summary>
        /// Processes a packet using the specified handler with performance monitoring
        /// </summary>
        private void ProcessPacketWithHandler(GSPacketIn packet, IPacketHandler packetHandler)
        {
            m_activePacketHandler = packetHandler;
            performanceTimer.Restart();

            try
            {
                // Security check: validate client state
                if (!IsClientValidForPacketProcessing(packet))
                {
                    return;
                }

                // Debug logging for specific user
                if (m_client.Player?.PlayerCharacter.NickName == "tester5656")
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"[{packet.Code}]");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }

                // Process the packet
                packetHandler.HandlePacket(m_client, packet);
            }
            catch (Exception ex)
            {
                HandlePacketProcessingException(packetHandler, packet, ex);
            }
            finally
            {
                performanceTimer.Stop();
                m_activePacketHandler = null;

                // Log performance metrics
                LogPerformanceMetrics(packetHandler, performanceTimer.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// Validates if the client is in a valid state for packet processing
        /// </summary>
        private bool IsClientValidForPacketProcessing(GSPacketIn packet)
        {
            // Check if client is connected
            if (m_client == null || m_client.TcpEndpoint == "not connected")
            {
                return false;
            }

            // Allow login packets even if player is not initialized
            if (packet.Code == 1)
            {
                return true;
            }

            // For other packets, player must be initialized
            return m_client.Player != null;
        }

        /// <summary>
        /// Handles exceptions during packet processing
        /// </summary>
        private void HandlePacketProcessingException(IPacketHandler packetHandler, GSPacketIn packet, Exception ex)
        {
            if (log.IsErrorEnabled)
            {
                string clientEndpoint = m_client?.TcpEndpoint ?? "unknown";
                log.Error($"Error processing packet (handler={packetHandler.GetType().FullName}, client={clientEndpoint})", ex);
                log.Error(Marshal.ToHexDump("Package Buffer:", packet.Buffer, 0, packet.Length));
            }

            // Consider disconnecting the client for certain types of exceptions
            // This could be implemented based on specific exception types
        }

        /// <summary>
        /// Logs performance metrics for packet processing
        /// </summary>
        private void LogPerformanceMetrics(IPacketHandler packetHandler, long processingTimeMs)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug($"Package process time: {processingTimeMs}ms");
            }

            if (processingTimeMs > 1500)
            {
                string clientEndpoint = m_client?.TcpEndpoint ?? "unknown";
                if (log.IsWarnEnabled)
                {
                    log.Warn($"({clientEndpoint}) Handle packet Thread {Thread.CurrentThread.ManagedThreadId} " +
                             $"{packetHandler} took {processingTimeMs}ms!");
                }
            }
        }

        /// <summary>
        /// Checks if the client is allowed to send packets based on rate limiting
        /// </summary>
        private bool IsClientAllowedToSendPacket(string clientEndpoint)
        {
            if (string.IsNullOrEmpty(clientEndpoint))
                return false;

            var rateInfo = clientRateLimiters.GetOrAdd(clientEndpoint,
                new ClientRateInfo { PacketCount = 0, LastResetTime = Environment.TickCount });

            long currentTime = Environment.TickCount;

            // Reset counter if time window has passed
            if (currentTime - rateInfo.LastResetTime > RATE_LIMIT_WINDOW_MS)
            {
                rateInfo.PacketCount = 1;
                rateInfo.LastResetTime = currentTime;
                return true;
            }

            // Check if client exceeded rate limit
            if (rateInfo.PacketCount >= MAX_PACKETS_PER_SECOND)
            {
                return false;
            }

            // Increment packet count
            rateInfo.PacketCount++;
            return true;
        }

        /// <summary>
        /// Performs periodic garbage collection to manage memory
        /// </summary>
        private void PerformPeriodicGCCollection()
        {
            long currentTime = Environment.TickCount;

            // Only collect garbage at specified intervals
            if (currentTime - lastGCCollectionTime > GC_COLLECTION_INTERVAL)
            {
                try
                {
                    // Force garbage collection
                    GC.Collect(GC.MaxGeneration, GCCollectionMode.Optimized);
                    GC.WaitForPendingFinalizers();

                    // Update last collection time
                    Interlocked.Exchange(ref lastGCCollectionTime, currentTime);

                    if (log.IsDebugEnabled)
                    {
                        log.Debug("Performed periodic garbage collection");
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error during garbage collection", ex);
                }
            }
        }

        /// <summary>
        /// Initializes packet handlers when scripts are loaded
        /// </summary>
        [ScriptLoadedEvent]
        public static void OnScriptCompiled(RoadEvent ev, object sender, EventArgs args)
        {
            // Clear existing handlers
            m_packetHandlers.Clear();

            // Load packet handlers from assembly
            int handlerCount = SearchPacketHandlers("v168", Assembly.GetAssembly(typeof(GameServer)));

            if (log.IsInfoEnabled)
            {
                log.Info($"PacketProcessor: Loaded {handlerCount} handlers from GameServer Assembly!");
            }
        }

        /// <summary>
        /// Registers a packet handler for a specific packet code
        /// </summary>
        public static void RegisterPacketHandler(int packetCode, IPacketHandler handler)
        {
            if (handler == null)
            {
                log.Error($"Attempted to register null handler for packet code {packetCode}");
                return;
            }

            if (!m_packetHandlers.TryAdd(packetCode, handler))
            {
                log.Warn($"Packet handler for code {packetCode} already registered and was replaced");
            }
        }

        /// <summary>
        /// Searches for packet handlers in the specified assembly
        /// </summary>
        protected static int SearchPacketHandlers(string version, Assembly assembly)
        {
            if (assembly == null)
            {
                log.Error("Null assembly provided to SearchPacketHandlers");
                return 0;
            }

            int handlerCount = 0;

            try
            {
                // Get all types that implement IPacketHandler interface
                var handlerTypes = assembly.GetTypes()
                    .Where(t => t.IsClass &&
                               t.GetInterface("Game.Server.Packets.Client.IPacketHandler") != null)
                    .ToList();

                foreach (var type in handlerTypes)
                {
                    try
                    {
                        var attributes = (PacketHandlerAttribute[])type.GetCustomAttributes(
                            typeof(PacketHandlerAttribute), inherit: true);

                        if (attributes.Length > 0)
                        {
                            handlerCount++;
                            var handler = (IPacketHandler)Activator.CreateInstance(type);
                            RegisterPacketHandler(attributes[0].Code, handler);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error($"Error creating packet handler for type {type.FullName}", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error searching for packet handlers", ex);
            }

            return handlerCount;
        }
    }
}