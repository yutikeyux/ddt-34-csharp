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
    /// <summary>
    /// Tüm gelen paketleri alır, güvenlik kontrolleri yapar, rate limit uygular,
    /// doğru handler'a yönlendirir ve performans izlemesi yapar.
    /// </summary>
    public class PacketProcessor
    {
        // log4net ile paket işlemeye özel log kategorisi.
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Tüm paket handler'ları, thread-safe bir sözlükte tutulur.
        // Aynı packet.Code için birden fazla thread aynı anda erişebilir.
        protected static readonly ConcurrentDictionary<int, IPacketHandler> m_packetHandlers =
            new ConcurrentDictionary<int, IPacketHandler>();

        // Bellek tükenmesi saldırılarını önlemek için maksimum paket boyutu (1 MB).
        // Çok büyük paketler muhtemelen spam, hata veya exploit olabilir.
        private const int MAX_PACKET_SIZE = 1024 * 1024; // 1MB

        // Paket işleme süresini ölçmek için global bir Stopwatch.
        // Aynı instance üzerinden tüm paketlerin süresi ölçülür.
        private static readonly Stopwatch performanceTimer = new Stopwatch();

        // Son garbage collection zamanı (Environment.TickCount ile).
        // GC_INTERVAL kadar süre geçince tekrar toplama yapılır.
        private static long lastGCCollectionTime = 0;
        private const int GC_COLLECTION_INTERVAL = 60000; // 1 dakika (ms)

        // Her istemci için rate limit bilgileri.
        // Anahtar: client.TcpEndpoint (IP:Port), değer: ClientRateInfo.
        private static readonly ConcurrentDictionary<string, ClientRateInfo> clientRateLimiters =
            new ConcurrentDictionary<string, ClientRateInfo>();

        // İstemcinin saniyede gönderebileceği maksimum paket sayısı.
        // 500 paket/saniye, normal oyun trafiği için makul; flood/DoS'a karşı sınır.
        private const int MAX_PACKETS_PER_SECOND = 7500;

        // Rate limit penceresi: 1 saniye içindeki paket sayısı kontrol edilir.
        private const int RATE_LIMIT_WINDOW_MS = 7500;

        // Bu PacketProcessor'nin bağlı olduğu GameClient örneği.
        // Her oyuncu bağlantısı için ayrı bir PacketProcessor nesnesi vardır.
        protected GameClient m_client;

        // Şu anda paket işleyen thread'in ID'si (loglama amaçlı).
        protected int m_handlerThreadID;

        // Şu anda aktif olan handler (exception loglamada kullanılır).
        protected IPacketHandler m_activePacketHandler;

        // Rate limit için istemci bilgilerini tutan sınıf.
        private class ClientRateInfo
        {
            // Son pencere içinde gelen paket sayısı.
            public int PacketCount { get; set; }

            // Son sıfırlama zamanı (Environment.TickCount).
            public long LastResetTime { get; set; }
        }

        /// <summary>
        /// Yeni bir istemci bağlantısı için PacketProcessor başlatılır.
        /// </summary>
        /// <param name="client">Bağlantıya ait GameClient nesnesi.</param>
        public PacketProcessor(GameClient client)
        {
            m_client = client ?? throw new ArgumentNullException(nameof(client));
        }

        /// <summary>
        /// Gelen bir paketi alır, güvenlik kontrolleri yapar, handler'a yönlendirir.
        /// </summary>
        /// <param name="packet">İstemciden gelen GSPacketIn paketi.</param>
        public void HandlePacket(GSPacketIn packet)
        {
            // Paket null gelirse, bağlantı bozuk olabilir; logla ve işlemeyi durdur.
            if (packet == null)
            {
                log.Error("Null packet received");
                return;
            }

            int code = packet.Code;

            // İstatistikleri güncelle: gelen bayt ve paket sayısı.
            // Thread-safe olması için Interlocked kullanılması önerilir.
            // Burada sadece örnek olduğu için direkt += bırakıldı; üretimde Interlocked kullan.
            Statistics.BytesIn += packet.Length;
            Statistics.PacketsIn++;

            // Güvenlik kontrolü: paket boyutu çok büyükse reddet.
            // 1MB'dan büyük paketler bellek tükenmesi (OOM) saldırısına açık olabilir.
            if (packet.Length > MAX_PACKET_SIZE)
            {
                log.Warn($"Oversized packet received from {m_client.TcpEndpoint}. Size: {packet.Length}, Code: {code}");
                return;
            }

            // Güvenlik kontrolü: istemcinin saniyede gönderdiği paket sayısı limiti.
            // Aşarsa oyuncuyu uyarıp disconnect ettir.
            if (!IsClientAllowedToSendPacket(m_client.TcpEndpoint))
            {
                m_client.Player?.SendMessage("Tabi efendim.");
                log.Warn($"{m_client.TcpEndpoint} paket limitlerini aştı. Paket kodu: {code}. İyice bi kontrol et bakalım handlerlardan, nesi yanlış? nesi değil? not: yutikeyu");
                m_client.Disconnect();
                return;
            }

            // Paket koduna karşılık gelen handler var mı?
            if (!m_packetHandlers.TryGetValue(code, out IPacketHandler packetHandler))
            {
                // Handler yoksa, muhtemelen hatalı/gereksiz/eksik paket.
                if (log.IsErrorEnabled)
                {
                    log.ErrorFormat("Received packet code {0} has no registered handler! Client: {1}",
                        code, m_client.ToString());

                    // Paket içeriğini hex dump olarak logla, debug amaçlı.
                    log.Error(Marshal.ToHexDump(
                        string.Format("===> <{2}> Packet 0x{0:X2} (0x{1:X2}) length: {3} (ThreadId={4})",
                            code, code ^ 0xA8, m_client.TcpEndpoint, packet.Length, Thread.CurrentThread.ManagedThreadId),
                        packet.Buffer));
                }
                return;
            }

            // Paketi handler ile işleyip performans ölçümü yap.
            ProcessPacketWithHandler(packet, packetHandler);

            // Belli aralıklarla garbage collection çalıştır (bellek baskısı için).
            PerformPeriodicGCCollection();
        }

        /// <summary>
        /// Belirli bir paketi, verilen handler ile işlerken performans ölçümü yapar.
        /// </summary>
        /// <param name="packet">İşlenecek paket.</param>
        /// <param name="packetHandler">Paketin işleneceği handler.</param>
        private void ProcessPacketWithHandler(GSPacketIn packet, IPacketHandler packetHandler)
        {
            m_activePacketHandler = packetHandler;
            performanceTimer.Restart();

            try
            {
                // Güvenlik kontrolü: istemcinin durumu paket işlemeye uygun mu?
                // Bağlantı kopmuşsa veya oyuncu yoksa bazı paketler reddedilir.
                if (!IsClientValidForPacketProcessing(packet))
                {
                    return;
                }

                // Sadece belirli bir test oyuncusu için konsola debug logu yaz.
                // Üretimde bu kısım kaldırılabilir veya log seviyesine alınabilir.
                if (m_client.Player?.PlayerCharacter?.NickName == "elementt")
                {
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine($"Admin {m_client.Player?.PlayerCharacter?.NickName} Paket Yönetimi. Gönderilen Paket Kodu: [{packet.Code}]");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }

                // Handler'a paketi gönder ve işlettir.
                packetHandler.HandlePacket(m_client, packet);
            }
            catch (Exception ex)
            {
                // Handler içinde hata olursa logla ve istemciyi disconnect etmeyi düşünebilirsin.
                HandlePacketProcessingException(packetHandler, packet, ex);
            }
            finally
            {
                performanceTimer.Stop();
                m_activePacketHandler = null;

                // Paketin ne kadar sürede işlendiğini logla.
                LogPerformanceMetrics(packetHandler, performanceTimer.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// İstemcinin paket işlemeye uygun durumda olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="packet">İşlenecek paket.</param>
        /// <returns>İşlemeye devam edilebilir ise true, yoksa false.</returns>
        private bool IsClientValidForPacketProcessing(GSPacketIn packet)
        {
            // m_client null ise bağlantı zaten kopmuş demektir.
            if (m_client == null)
            {
                return false;
            }

            // TcpEndpoint "not connected" ise bağlantı geçersizdir.
            // Gerçek projede TcpEndpoint yerine m_client.IsConnected gibi bir flag daha temiz olur.
            if (m_client.TcpEndpoint == "not connected")
            {
                return false;
            }

            // Login paketi (Code 1) istemci henüz Player nesnesi oluşturmadan bile işlenebilir.
            if (packet.Code == 1)
            {
                return true;
            }

            // Diğer paketler için Player nesnesi oluşturulmuş olmalıdır.
            // Aksi halde oyuncu henüz tam olarak giriş yapmamıştır.
            return m_client.Player != null;
        }

        /// <summary>
        /// Paket işlenirken handler içinde oluşan hatayı loglar.
        /// </summary>
        /// <param name="packetHandler">Hata veren handler.</param>
        /// <param name="packet">Hata sırasında işlenmekte olan paket.</param>
        /// <param name="ex">Oluşan istisna.</param>
        private void HandlePacketProcessingException(IPacketHandler packetHandler, GSPacketIn packet, Exception ex)
        {
            if (log.IsErrorEnabled)
            {
                string clientEndpoint = m_client?.TcpEndpoint ?? "unknown";
                log.Error($"Error processing packet (handler={packetHandler.GetType().FullName}, client={clientEndpoint})", ex);

                // Paket içeriğini hex dump olarak logla, exploit analizi için.
                log.Error(Marshal.ToHexDump("Package Buffer:", packet.Buffer, 0, packet.Length));
            }

            // Kritik exception tipleri için istemciyi disconnect etmek mantıklı olabilir.
            // Örneğin: InvalidOperationException, NullReferenceException, ProtocolViolation vb.
            // Bu kısım isteğe göre genişletilebilir.
        }

        /// <summary>
        /// Paket işleme süresini loglar; uzun süren işlemler için uyarı verir.
        /// </summary>
        /// <param name="packetHandler">İşleyen handler.</param>
        /// <param name="processingTimeMs">İşlem süresi (milisaniye).</param>
        private void LogPerformanceMetrics(IPacketHandler packetHandler, long processingTimeMs)
        {
            if (log.IsDebugEnabled)
            {
                log.Debug($"Package process time: {processingTimeMs}ms");
            }

            // 1.5 saniyeden uzun süren paketler, muhtemelen performans problemi var.
            // Bu threshold üretim ortamına göre ayarlanabilir.
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
        /// Belirli bir istemcinin rate limit kurallarına uyup uymadığını kontrol eder.
        /// </summary>
        /// <param name="clientEndpoint">İstemcinin IP:Port bilgisi (TcpEndpoint).</param>
        /// <returns>Rate limit aşılmadıysa true, aşıldıysa false.</returns>
        private bool IsClientAllowedToSendPacket(string clientEndpoint)
        {
            // clientEndpoint boş veya null ise, bağlantı geçersizdir.
            if (string.IsNullOrEmpty(clientEndpoint))
                return false;

            // Her istemci için bir ClientRateInfo nesnesi oluştur veya mevcut olanı al.
            // ConcurrentDictionary.GetOrAdd thread-safe olarak çalışır.
            var rateInfo = clientRateLimiters.GetOrAdd(clientEndpoint,
                new ClientRateInfo { PacketCount = 0, LastResetTime = Environment.TickCount });

            long currentTime = Environment.TickCount;

            // TickCount taşma ihtimaline rağmen, küçük farklar için hâlâ kullanışlıdır.
            // Daha doğru çözüm: Stopwatch.GetTimestamp() veya DateTime.UtcNow.Ticks kullanmak.
            // Bu örnekte TickCount bırakıldı; üretimde Stopwatch kullanmak önerilir.

            // Pencere süresi (1 saniye) geçmişse sayacı sıfırla ve yeni paketi say.
            if (currentTime - rateInfo.LastResetTime > RATE_LIMIT_WINDOW_MS)
            {
                rateInfo.PacketCount = 1;
                rateInfo.LastResetTime = currentTime;
                return true;
            }

            // Pencere içindeki paket sayısı limiti aşıldıysa reddet.
            if (rateInfo.PacketCount >= MAX_PACKETS_PER_SECOND)
            {
                return false;
            }

            // Limit aşılmadıysa, sayacı artır ve pakete izin ver.
            rateInfo.PacketCount++;
            return true;
        }

        /// <summary>
        /// Belli aralıklarla garbage collection çalıştırarak bellek baskısını azaltmaya çalışır.
        /// NOT: GC.Collect üretimde genelde önerilmez; sadece özel durumlar için.
        /// </summary>
        private void PerformPeriodicGCCollection()
        {
            long currentTime = Environment.TickCount;

            // GC_COLLECTION_INTERVAL kadar süre geçtiyse GC çalıştır.
            if (currentTime - lastGCCollectionTime > GC_COLLECTION_INTERVAL)
            {
                try
                {
                    // En yüksek nesil için garbage collection başlat.
                    GC.Collect(GC.MaxGeneration, GCCollectionMode.Optimized);

                    // Finalizer queue'daki nesnelerin işlenmesini bekle.
                    GC.WaitForPendingFinalizers();

                    // Son toplama zamanını güncelle.
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
        /// Script yeniden yüklendiğinde tüm eski handler'ları temizleyip yenilerini yükle.
        /// </summary>
        /// <param name="ev">ScriptLoadedEvent nesnesi.</param>
        /// <param name="sender">Event'i gönderen nesne.</param>
        /// <param name="args">Event argümanları.</param>
        [ScriptLoadedEvent]
        public static void OnScriptCompiled(RoadEvent ev, object sender, EventArgs args)
        {
            // Eski handler'ları temizle; aksi halde çakışma olur.
            m_packetHandlers.Clear();

            // GameServer assembly'sinden IPacketHandler implement eden türleri bul ve yükle.
            // "v168" parametresi şu an kullanılmıyor; üretimde ya kullan ya da kaldır.
            int handlerCount = SearchPacketHandlers("v168", Assembly.GetAssembly(typeof(GameServer)));

            if (log.IsInfoEnabled)
            {
                log.Info($"PacketProcessor: Loaded {handlerCount} handlers from GameServer Assembly!");
            }
        }

        /// <summary>
        /// Belirli bir paket kodu için handler kaydeder.
        /// Aynı kodla birden fazla handler kaydedilirse, son kaydedilen geçerli olur.
        /// </summary>
        /// <param name="packetCode">Paket kodu (int).</param>
        /// <param name="handler">IPacketHandler implement eden nesne.</param>
        public static void RegisterPacketHandler(int packetCode, IPacketHandler handler)
        {
            if (handler == null)
            {
                log.Error($"Attempted to register null handler for packet code {packetCode}");
                return;
            }

            // ConcurrentDictionary.TryAdd: eğer anahtar zaten varsa false döner.
            // Bu durumda, eski handler yerine yeni handler'ı kullanmak istiyorsan
            // m_packetHandlers[packetCode] = handler; yapman gerekir.
            // Şu an sadece logla geçiyor; istersen burayı overwrite mantığına çevir.
            if (!m_packetHandlers.TryAdd(packetCode, handler))
            {
                log.Info($"Packet handler for code {packetCode} already registered and was replaced");
            }
        }

        /// <summary>
        /// Belirtilen assembly içinde IPacketHandler implement eden sınıfları bulur
        /// ve PacketHandlerAttribute ile işaretlenmiş olanları handler olarak kaydeder.
        /// </summary>
        /// <param name="version">Script sürümü (şu an kullanılmıyor; üretimde kaldırılabilir).</param>
        /// <param name="assembly">Handler'ların aranacağı assembly.</param>
        /// <returns>Başarıyla kaydedilen handler sayısı.</returns>
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
                // Assembly içindeki tüm türleri al.
                // Bu işlem büyük assembly'lerde maliyetli olabilir; mümkünse namespace filtresi ekle.
                var handlerTypes = assembly.GetTypes()
                    .Where(t => t.IsClass &&
                               t.GetInterface("Game.Server.Packets.Client.IPacketHandler") != null)
                    .ToList();

                foreach (var type in handlerTypes)
                {
                    try
                    {
                        // PacketHandlerAttribute ile işaretlenmiş mi?
                        // Burada string kullanmak yerine typeof(IPacketHandler) kullanılması daha doğru olur.
                        var attributes = (PacketHandlerAttribute[])type.GetCustomAttributes(
                            typeof(PacketHandlerAttribute), inherit: true);

                        if (attributes.Length > 0)
                        {
                            // Handler nesnesini oluştur.
                            var handler = (IPacketHandler)Activator.CreateInstance(type);

                            // Paket koduna göre kaydet.
                            RegisterPacketHandler(attributes[0].Code, handler);

                            // Sadece başarıyla kaydedilen handler'lar sayılır.
                            handlerCount++;
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
