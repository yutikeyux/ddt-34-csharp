using System.Collections.Generic;
using System.Threading;
using System.Net;
using System;
using log4net;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace Game.Base
{
    public class HydroFilter
    {
        private const int TIMER_CHECK_INTERVAL = 20000;
        private const int TIMER_SCAN_INTERVAL = 10000;
        private const int MAX_CONNECTIONS_IN_20SEC = 200;
        private const int MAX_AVG_PACKETS_PER_CLIENT_IN_20SEC = 5000;
        private const int SCAN_MAX_CONNECTIONS_PER_IP = 10;
        private const int SCAN_MAX_PACKETS_PER_IP = 300;
        private const int TIMES_TO_DISABLE = 3;
        private const string BLOCK_LIST_FILENAME = "IPBlockList.txt";
        private const string WHITE_LIST_FILENAME = "IPWhiteList.txt"; // Yeni: Whitelist dosyası

        public static bool IsActive;
        public static bool IsStarted;

        public static Dictionary<IPAddress, int> ConnectionList = new();
        public static Dictionary<IPAddress, int> PacketList = new();
        public static List<IPAddress> BlockList = new();
        public static List<IPAddress> WhiteList = new(); // Yeni: Whitelist listesi

        public static int ConnectionCount;
        public static int BlockedConnections;
        public static int BlockedPackets;

        private static int Before20SecConnectionCount;
        private static long Before20SecPacketInCount;
        private static int RemainTimesToDisable = 0;

        protected static Timer timerCheck;
        protected static Timer timerScan;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static HydroFilter()
        {
            LoadWhiteList(); // Yeni: Whitelist'i yükle
        }

        // Yeni: Whitelist'e IP ekleme metodu
        public static void AddToWhiteList(IPAddress ipAddress)
        {
            try
            {
                if (!WhiteList.Contains(ipAddress))
                {
                    WhiteList.Add(ipAddress);
                    SaveWhiteList();
                    log.Info($"IP whitelist'e eklendi: {ipAddress}");

                    // Eğer IP engellenmişse, engellemeyi kaldır
                    if (BlockList.Contains(ipAddress))
                    {
                        RemoveBlockIP(ipAddress);
                        log.Info($"Whitelist'e eklenen IP engellemesi kaldırıldı: {ipAddress}");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Whitelist'e IP eklenirken hata: {ex.Message}");
            }
        }

        // Yeni: Whitelist'ten IP kaldırma metodu
        public static void RemoveFromWhiteList(IPAddress ipAddress)
        {
            try
            {
                if (WhiteList.Contains(ipAddress))
                {
                    WhiteList.Remove(ipAddress);
                    SaveWhiteList();
                    log.Info($"IP whitelist'ten kaldırıldı: {ipAddress}");
                }
            }
            catch (Exception ex)
            {
                log.Error($"Whitelist'ten IP kaldırılırken hata: {ex.Message}");
            }
        }

        // Yeni: Whitelist'i dosyaya kaydetme
        private static void SaveWhiteList()
        {
            try
            {
                File.WriteAllLines(WHITE_LIST_FILENAME, WhiteList.Select(ip => ip.ToString()));
            }
            catch (Exception ex)
            {
                log.Error($"Whitelist dosyasına yazılırken hata: {ex.Message}");
            }
        }

        // Yeni: Whitelist'i dosyadan yükleme
        private static void LoadWhiteList()
        {
            try
            {
                if (File.Exists(WHITE_LIST_FILENAME))
                {
                    string[] ips = File.ReadAllLines(WHITE_LIST_FILENAME);
                    foreach (string ip in ips)
                    {
                        if (!string.IsNullOrWhiteSpace(ip) && IPAddress.TryParse(ip, out IPAddress address))
                        {
                            WhiteList.Add(address);
                        }
                    }
                    log.Info($"{WhiteList.Count} adet whitelisted IP yüklendi.");
                }
            }
            catch (Exception ex)
            {
                log.Error($"Whitelist yüklenirken hata: {ex.Message}");
            }
        }

        // Yeni: IP'nin whitelist'te olup olmadığını kontrol et
        public static bool IsWhiteListed(IPAddress ipAddress)
        {
            return WhiteList.Contains(ipAddress);
        }

        // Tüm engellenmiş IP'leri kaldıran ana metot
        public static void UnblockAllIPs()
        {
            try
            {
                // 1. Bellekteki engellenen IP listesini temizle
                BlockList.Clear();
                log.Info("Bellekteki engellenen IP listesi temizlendi.");

                // 2. IPBlockList.txt dosyasını temizle
                if (File.Exists(BLOCK_LIST_FILENAME))
                {
                    File.WriteAllText(BLOCK_LIST_FILENAME, string.Empty);
                    log.Info($"{BLOCK_LIST_FILENAME} dosyası temizlendi.");
                }

                // 3. Güvenlik duvarından tüm HydroFilter kurallarını kaldır
                RemoveAllFirewallRules();
                log.Info("Güvenlik duvarından tüm engelleme kuralları kaldırıldı.");

                // 4. İstatistikleri sıfırla
                BlockedConnections = 0;
                BlockedPackets = 0;
                ConnectionCount = 0;
                Before20SecConnectionCount = 0;
                Before20SecPacketInCount = 0;
                RemainTimesToDisable = 0;

                log.Info("Tüm engellenen IP'ler başarıyla kaldırıldı!");
            }
            catch (Exception ex)
            {
                log.Error($"IP engellemeleri kaldırılırken hata: {ex.Message}");
            }
        }

        // Güvenlik duvarından tüm HydroFilter kurallarını kaldırır
        private static void RemoveAllFirewallRules()
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "netsh",
                    Arguments = "advfirewall firewall delete rule name=\"HydroFilter - \"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(psi))
                {
                    process.WaitForExit();
                    string output = process.StandardOutput.ReadToEnd();

                    if (!string.IsNullOrEmpty(output))
                    {
                        log.Debug($"Güvenlik duvarı temizleme çıktısı: {output}");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Güvenlik duvarı kuralları kaldırılırken hata: {ex.Message}");
            }
        }

        public static void Log(string logMessage)
        {
            try
            {
                File.AppendAllText(BLOCK_LIST_FILENAME, logMessage + Environment.NewLine);
            }
            catch (Exception ex)
            {
                log.Error($"Log yazılırken hata: {ex.Message}");
            }
        }

        public static void RemoveLog(string logMessage)
        {
            try
            {
                if (File.Exists(BLOCK_LIST_FILENAME))
                {
                    var lines = File.ReadAllLines(BLOCK_LIST_FILENAME).Where(l => l != logMessage);
                    File.WriteAllLines(BLOCK_LIST_FILENAME, lines);
                }
            }
            catch (Exception ex)
            {
                log.Error($"Log silinirken hata: {ex.Message}");
            }
        }

        public static void Start()
        {
            IsStarted = true;
            LoadBlockedIPs();

            if (timerCheck == null)
                timerCheck = new Timer(Check, null, TIMER_CHECK_INTERVAL, TIMER_CHECK_INTERVAL);

            log.Info("HydroFilter is started!");
        }

        public static void Stop()
        {
            IsStarted = false;
            Disable();

            if (timerCheck != null)
            {
                timerCheck.Change(-1, -1);
                timerCheck.Dispose();
                timerCheck = null;
            }

            // Program durduğunda tüm engellemeleri kaldır
            UnblockAllIPs();
            log.Info("HydroFilter is stopped!");
        }

        public static void Activate()
        {
            IsActive = true;
            RemainTimesToDisable = TIMES_TO_DISABLE;

            if (timerScan == null)
                timerScan = new Timer(Scan, null, TIMER_SCAN_INTERVAL, TIMER_SCAN_INTERVAL);

            log.Info("HydroFilter is activated!");
        }

        public static void Disable()
        {
            IsActive = false;

            if (timerScan != null)
            {
                timerScan.Change(-1, -1);
                timerScan.Dispose();
                timerScan = null;
            }

            ConnectionList.Clear();
            PacketList.Clear();
            log.Info("HydroFilter is disabled!");
        }

        protected static void Check(object sender)
        {
            if (ConnectionCount - Before20SecConnectionCount > MAX_CONNECTIONS_IN_20SEC ||
                (Statistics.ClientsCount != 0 &&
                 Statistics.PacketsIn - Before20SecPacketInCount > MAX_AVG_PACKETS_PER_CLIENT_IN_20SEC * Statistics.ClientsCount))
            {
                Activate();
            }
            else
            {
                RemainTimesToDisable--;
                if (IsActive && RemainTimesToDisable == 0)
                {
                    Disable();
                }
            }

            Before20SecConnectionCount = ConnectionCount;
            Before20SecPacketInCount = Statistics.PacketsIn;
        }

        public static void CheckAndUnblockQuestAddHandler(IPAddress ipAddress)
        {
            if (IsBlocked(new IPEndPoint(ipAddress, 0)))
            {
                RemoveBlockIP(ipAddress);
                log.Info($"HydroFilter - {ipAddress} adlı IP, filtre dışında bırakıldı (questaddhandler).");
            }
        }

        protected static void Scan(object sender)
        {
            lock (ConnectionList)
            {
                foreach (KeyValuePair<IPAddress, int> keyValuePair in ConnectionList)
                {
                    // Yeni: Whitelist kontrolü
                    if (IsWhiteListed(keyValuePair.Key))
                    {
                        log.Info($"IP whitelist'te, engelleme atlandı: {keyValuePair.Key}");
                        continue;
                    }

                    if (keyValuePair.Value > SCAN_MAX_CONNECTIONS_PER_IP)
                    {
                        log.Info($"HydroFilter - BAĞLANTI KAYNAKLI BANLANACAK BİRİ BULUNDU: {keyValuePair.Key}, Value: {keyValuePair.Value}, MAX: {SCAN_MAX_CONNECTIONS_PER_IP}");
                        BlockIP(keyValuePair.Key);
                    }
                }
                ConnectionList.Clear();
            }

            lock (PacketList)
            {
                foreach (KeyValuePair<IPAddress, int> keyValuePair in PacketList)
                {
                    // Yeni: Whitelist kontrolü
                    if (IsWhiteListed(keyValuePair.Key))
                    {
                        log.Info($"IP whitelist'te, engelleme atlandı: {keyValuePair.Key}");
                        continue;
                    }

                    if (keyValuePair.Value > SCAN_MAX_PACKETS_PER_IP)
                    {
                        log.Info($"HydroFilter - PAKET KAYNAKLI BANLANACAK BİRİ BULUNDU: {keyValuePair.Key}, Value: {keyValuePair.Value}, MAX: {SCAN_MAX_PACKETS_PER_IP}");
                        BlockIP(keyValuePair.Key);
                    }
                }
                PacketList.Clear();
            }
        }

        public static void LogNewConnection(EndPoint endPoint)
        {
            IPEndPoint remoteIpEndPoint = endPoint as IPEndPoint;
            if (remoteIpEndPoint != null)
            {
                lock (ConnectionList)
                {
                    if (!ConnectionList.ContainsKey(remoteIpEndPoint.Address))
                    {
                        ConnectionList.Add(remoteIpEndPoint.Address, 1);
                        CheckAndUnblockQuestAddHandler(remoteIpEndPoint.Address);
                    }
                    else
                        ConnectionList[remoteIpEndPoint.Address]++;
                }
            }
        }

        public static bool IsBlocked(EndPoint endPoint)
        {
            IPEndPoint ipEndPoint = endPoint as IPEndPoint;
            return !(ipEndPoint == null) && BlockList.Contains(ipEndPoint.Address);
        }

        public static void BlockIP(IPAddress ipAddress, bool Save = true)
        {
            // Yeni: Whitelist kontrolü
            if (IsWhiteListed(ipAddress))
            {
                log.Info($"IP whitelist'te, engelleme atlandı: {ipAddress}");
                return;
            }

            if (!BlockList.Contains(ipAddress))
            {
                BlockList.Add(ipAddress);
                if (Save)
                {
                    Log(ipAddress.ToString());
                    FirewallBlock(ipAddress.ToString());
                    BlockedConnections++;
                }
            }
        }

        private static void FirewallBlock(string ipAddress)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo("netsh",
                    $"advfirewall firewall add rule name=\"HydroFilter - {ipAddress}\" dir=in action=block remoteip={ipAddress}")
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(psi))
                {
                    process.WaitForExit();
                    string output = process.StandardOutput.ReadToEnd();

                    if (!string.IsNullOrEmpty(output))
                    {
                        log.Debug($"Güvenlik duvarı kuralı ekleme çıktısı: {output}");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Güvenlik duvarı kuralı eklenirken hata: {ex.Message}");
            }
        }

        private static void DeleteFirewallBlock(string ipAddress)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo("netsh",
                    $"advfirewall firewall delete rule name=\"HydroFilter - {ipAddress}\"")
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(psi))
                {
                    process.WaitForExit();
                    string output = process.StandardOutput.ReadToEnd();

                    if (!string.IsNullOrEmpty(output))
                    {
                        log.Debug($"Güvenlik duvarı kuralı silme çıktısı: {output}");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Güvenlik duvarı kuralı silinirken hata: {ex.Message}");
            }
        }

        public static bool RemoveBlockIP(IPAddress ipAddress, bool Save = true)
        {
            bool result = false;
            if (BlockList.Contains(ipAddress))
            {
                BlockList.Remove(ipAddress);
                if (Save)
                {
                    RemoveLog(ipAddress.ToString());
                    DeleteFirewallBlock(ipAddress.ToString());
                    BlockedConnections--;
                }
                result = true;
            }
            return result;
        }

        private static void LoadBlockedIPs()
        {
            try
            {
                if (File.Exists(BLOCK_LIST_FILENAME))
                {
                    string[] ips = File.ReadAllLines(BLOCK_LIST_FILENAME);
                    foreach (string ip in ips)
                    {
                        if (!string.IsNullOrWhiteSpace(ip) && IPAddress.TryParse(ip, out IPAddress address))
                        {
                            // Yeni: Whitelist kontrolü
                            if (!IsWhiteListed(address))
                            {
                                BlockIP(address, false);
                            }
                            else
                            {
                                log.Info($"Whitelist'teki IP engelleme listesinden atlandı: {address}");
                            }
                        }
                    }
                    log.Info($"{BlockList.Count} adet engellenmiş IP yüklendi.");
                }
            }
            catch (Exception ex)
            {
                log.Error($"Engellenmiş IP'ler yüklenirken hata: {ex.Message}");
            }
        }

        public static void LogNewPacket(EndPoint endPoint)
        {
            IPEndPoint remoteIpEndPoint = endPoint as IPEndPoint;
            if (remoteIpEndPoint != null)
            {
                lock (PacketList)
                {
                    if (!PacketList.ContainsKey(remoteIpEndPoint.Address))
                        PacketList.Add(remoteIpEndPoint.Address, 1);
                    else
                        PacketList[remoteIpEndPoint.Address]++;
                }
            }
        }
    }
}