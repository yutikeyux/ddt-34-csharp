using System.Collections.Concurrent;
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
    /// <summary>
    /// Sunucuya gelen bağlantı ve paket trafiğini izleyerek,
    /// flood/DDoS benzeri durumlarda IP bazlı engelleme yapan filtre.
    /// Whitelist desteği, netsh advfirewall entegrasyonu ve istatistik tutar.
    /// </summary>
    public class HydroFilter
    {
        // Check timer: sunucu trafiğine göre HydroFilter'ı aktif/pasif yapar.
        private const int TIMER_CHECK_INTERVAL = 20000; // 20 saniye

        // Scan timer: her 10 saniyede bir IP'leri kontrol edip banlama yapar.
        private const int TIMER_SCAN_INTERVAL = 10000;  // 10 saniye

        // 20 saniye içinde izin verilen maksimum yeni bağlantı sayısı.
        // Bu sınırı aşan durumda HydroFilter aktif olur.
        private const int MAX_CONNECTIONS_IN_20SEC = 2000;

        // 20 saniye içinde client başına izin verilen maksimum paket sayısı.
        // Çok yüksek değerler flood/DoS'a açık olabilir; burada 10k kullanılıyor.
        private const int MAX_AVG_PACKETS_PER_CLIENT_IN_20SEC = 40000;

        // Scan aşamasında bir IP'nin 10 saniyede yapabileceği maksimum bağlantı sayısı.
        private const int SCAN_MAX_CONNECTIONS_PER_IP = 2000;

        // Scan aşamasında bir IP'nin 10 saniyede gönderebileceği maksimum paket sayısı.
        private const int SCAN_MAX_PACKETS_PER_IP = 15000;

        // Aktifken, trafiğin normal seviyeye düşmesi durumunda HydroFilter'in
        // kaç kez daha "normal" ölçüm gelirse devre dışı kalacağını belirtir.
        private const int TIMES_TO_DISABLE = 3;

        // Engellenmiş IP'lerin dosya yolu (hem log hem liste olarak kullanılıyor).
        private const string BLOCK_LIST_FILENAME = "IPBlockList.txt";

        // Whitelist dosyası: bu IP'ler asla engellenmez.
        private const string WHITE_LIST_FILENAME = "IPWhiteList.txt"; // Yeni: Whitelist dosyası not: yuti

        // HydroFilter'in şu an aktif olup olmadığını gösterir.
        public static bool IsActive;

        // HydroFilter'in başlatılıp başlatılmadığını gösterir.
        public static bool IsStarted;

        // 10 saniyelik pencere içinde her IP için gelen bağlantı sayısı.
        // Thread-safe olması için ConcurrentDictionary kullanılıyor.
        public static ConcurrentDictionary<IPAddress, int> ConnectionList =
            new ConcurrentDictionary<IPAddress, int>();

        // 10 saniyelik pencere içinde her IP için gelen paket sayısı.
        public static ConcurrentDictionary<IPAddress, int> PacketList =
            new ConcurrentDictionary<IPAddress, int>();

        // Şu an engellenmiş olan IP'lerin listesi (bellek).
        public static List<IPAddress> BlockList = new();

        // Whitelist'teki IP'lerin listesi (bellek).
        public static List<IPAddress> WhiteList = new(); // Yeni: Whitelist listesi not: yuti

        // Toplam yeni bağlantı sayısı (20 saniyelik pencere için).
        public static int ConnectionCount;

        // Toplam engellenen bağlantı sayısı.
        public static int BlockedConnections;

        // Toplam engellenen paket sayısı (log amaçlı).
        public static int BlockedPackets;

        // 20 saniye önceki bağlantı sayısı (fark hesabı için).
        private static int Before20SecConnectionCount;

        // 20 saniye önceki toplam gelen paket sayısı.
        private static long Before20SecPacketInCount;

        // Aktifken, trafiğin normal seviyeye düşmesi durumunda
        // kaç kez daha "normal" ölçüm gelirse devre dışı kalacağını tutar.
        private static int RemainTimesToDisable = 0;

        // 20 saniyede bir çalışan timer; trafiği izleyip Activate/Disable yapar.
        protected static Timer timerCheck;

        // 10 saniyede bir çalışan timer; IP'leri tarayıp banlama yapar.
        protected static Timer timerScan;

        // log4net ile HydroFilter log kategorisi.
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Static constructor: HydroFilter ilk kez kullanıldığında Whitelist'i yükle.
        /// </summary>
        static HydroFilter()
        {
            LoadWhiteList(); // Yeni: Whitelist'i yükle not: yuti
        }

        // Yeni: Whitelist'e IP ekleme metodu not: yuti
        /// <summary>
        /// Belirtilen IP'yi Whitelist'e ekler ve dosyaya kaydeder.
        /// Eğer bu IP önceden engellenmişse, engellemesi kaldırılır.
        /// </summary>
        /// <param name="ipAddress">Whitelist'e eklenecek IP adresi.</param>
        public static void AddToWhiteList(IPAddress ipAddress)
        {
            try
            {
                if (!WhiteList.Contains(ipAddress))
                {
                    WhiteList.Add(ipAddress);
                    SaveWhiteList();
                    log.Info($"IP whitelist'e eklendi: {ipAddress}");

                    // Eğer IP engellenmişse, engellemeyi kaldır not: yuti
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

        // Yeni: Whitelist'ten IP kaldırma metodu not: yuti
        /// <summary>
        /// Belirtilen IP'yi Whitelist'ten kaldırır ve dosyayı günceller.
        /// </summary>
        /// <param name="ipAddress">Whitelist'ten kaldırılacak IP adresi.</param>
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

        // Yeni: Whitelist'i dosyaya kaydetme not: yuti
        /// <summary>
        /// Whitelist'teki IP'leri dosyaya yazar.
        /// </summary>
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

        // Yeni: Whitelist'i dosyadan yükleme not: yuti
        /// <summary>
        /// Whitelist dosyasından IP'leri okur ve belleğe yükler.
        /// </summary>
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

        // Yeni: IP'nin whitelist'te olup olmadığını kontrol et not: yuti
        /// <summary>
        /// Belirtilen IP'nin Whitelist'te olup olmadığını döner.
        /// </summary>
        /// <param name="ipAddress">Kontrol edilecek IP adresi.</param>
        /// <returns>Whitelist'teyse true, değilse false.</returns>
        public static bool IsWhiteListed(IPAddress ipAddress)
        {
            return WhiteList.Contains(ipAddress);
        }

        // Tüm engellenmiş IP'leri kaldıran ana metot not: yuti
        /// <summary>
        /// Tüm engellenmiş IP'leri bellekten, dosyadan ve Windows güvenlik duvarından kaldırır.
        /// İstatistikleri de sıfırlar.
        /// </summary>
        public static void UnblockAllIPs()
        {
            try
            {
                // 1. Bellekteki engellenen IP listesini temizle not: yuti
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

        // Güvenlik duvarından tüm HydroFilter kurallarını kaldırır not: yuti
        /// <summary>
        /// Windows güvenlik duvarından "HydroFilter - " ile başlayan tüm kuralları siler.
        /// Dikkat: başka servisler aynı isimle kural açtıysa onları da siler.
        /// </summary>
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

        /// <summary>
        /// Belirli bir mesajı BLOCK_LIST_FILENAME dosyasına ekler.
        /// Şu an hem IP listesi hem log dosyası olarak kullanılıyor.
        /// </summary>
        /// <param name="logMessage">Dosyaya yazılacak mesaj.</param>
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

        /// <summary>
        /// BLOCK_LIST_FILENAME dosyasından belirli bir satırı kaldırır.
        /// Sadece tam eşleşen satır silinir.
        /// </summary>
        /// <param name="logMessage">Silinmesi istenen tam satır.</param>
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

        /// <summary>
        /// HydroFilter'i başlatır. BlockList'i dosyadan yükler ve Check timer'ı başlatır.
        /// </summary>
        public static void Start()
        {
            IsStarted = true;
            LoadBlockedIPs();

            if (timerCheck == null)
                timerCheck = new Timer(Check, null, TIMER_CHECK_INTERVAL, TIMER_CHECK_INTERVAL);

            log.Info("HydroFilter is started!");
        }

        /// <summary>
        /// HydroFilter'i durdurur. Timer'ları durdurur ve tüm engellemeleri kaldırır.
        /// </summary>
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

            // Program durduğunda tüm engellemeleri kaldır not: yuti
            UnblockAllIPs();
            log.Info("HydroFilter is stopped!");
        }

        /// <summary>
        /// HydroFilter'i aktif hale getirir. Scan timer'ı başlatır.
        /// </summary>
        public static void Activate()
        {
            IsActive = true;
            RemainTimesToDisable = TIMES_TO_DISABLE;

            if (timerScan == null)
                timerScan = new Timer(Scan, null, TIMER_SCAN_INTERVAL, TIMER_SCAN_INTERVAL);

            log.Info("HydroFilter is activated!");
        }

        /// <summary>
        /// HydroFilter'i pasif hale getirir. Timer'ı durdurur ve ConnectionList/PacketList'i temizler.
        /// </summary>
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

        /// <summary>
        /// 20 saniyede bir çalışan, sunucu trafiğini izleyen timer callback'i.
        /// Toplam bağlantı veya paket sayısı belirlenen limitleri aşıyorsa HydroFilter aktif olur.
        /// Aksi halde, belirli sayıda "normal" ölçüm sonunda devre dışı kalır.
        /// </summary>
        /// <param name="sender">Timer callback sender (kullanılmıyor).</param>
        protected static void Check(object sender)
        {
            // 20 saniye içindeki yeni bağlantı farkı limiti aşıyorsa aktif et.
            if (ConnectionCount - Before20SecConnectionCount > MAX_CONNECTIONS_IN_20SEC)
            {
                Activate();
            }
            // Paket sayısı da çok fazlaysa aktif et (client başına ortalama paket sayısı).
            else if (Statistics.ClientsCount != 0 &&
                     Statistics.PacketsIn - Before20SecPacketInCount > MAX_AVG_PACKETS_PER_CLIENT_IN_20SEC * Statistics.ClientsCount)
            {
                Activate();
            }
            else
            {
                // Trafik normal seviyede ise, devre dışı kalma sayacını azalt.
                RemainTimesToDisable--;

                // Aktifken ve sayac 0'a inmişse devre dışı bırak.
                if (IsActive && RemainTimesToDisable == 0)
                {
                    Disable();
                }
            }

            // 20 saniyelik pencereyi güncelle.
            Before20SecConnectionCount = ConnectionCount;
            Before20SecPacketInCount = Statistics.PacketsIn;
        }

        /// <summary>
        /// QuestAddHandler gibi özel durumlarda, bir IP'nin engellenmiş olup olmadığını kontrol eder
        /// ve engellenmişse engellemesini kaldırır.
        /// </summary>
        /// <param name="ipAddress">Kontrol edilecek IP adresi.</param>
        public static void CheckAndUnblockQuestAddHandler(IPAddress ipAddress)
        {
            if (IsBlocked(new IPEndPoint(ipAddress, 0)))
            {
                RemoveBlockIP(ipAddress);
                log.Info($"HydroFilter - {ipAddress} adlı IP, filtre dışında bırakıldı (questaddhandler).");
            }
        }

        /// <summary>
        /// 10 saniyede bir çalışan, her IP'nin bağlantı ve paket sayısını tarayan timer callback'i.
        /// Whitelist'teki IP'ler atlanır; limiti aşan IP'ler engellenir.
        /// </summary>
        /// <param name="sender">Timer callback sender (kullanılmıyor).</param>
        protected static void Scan(object sender)
        {
            // ConnectionList üzerinde snapshot alıp dolaşmak daha güvenli.
            var connectionsSnapshot = ConnectionList.ToArray();
            foreach (var keyValuePair in connectionsSnapshot)
            {
                // Yeni: Whitelist kontrolü not: yuti
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

            // 10 saniyelik pencereyi sıfırla.
            ConnectionList.Clear();

            // PacketList üzerinde snapshot alıp dolaş.
            var packetsSnapshot = PacketList.ToArray();
            foreach (var keyValuePair in packetsSnapshot)
            {
                // Yeni: Whitelist kontrolü not: yuti
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

            // 10 saniyelik pencereyi sıfırla.
            PacketList.Clear();
        }

        /// <summary>
        /// Yeni bir bağlantı geldiğinde çağrılır. IP'yi ConnectionList'e ekler veya sayısını artırır.
        /// </summary>
        /// <param name="endPoint">Yeni bağlantıya ait EndPoint.</param>
        public static void LogNewConnection(EndPoint endPoint)
        {
            IPEndPoint remoteIpEndPoint = endPoint as IPEndPoint;
            if (remoteIpEndPoint != null)
            {
                // ConcurrentDictionary.AddOrUpdate ile thread-safe artırma.
                ConnectionList.AddOrUpdate(
                    remoteIpEndPoint.Address,
                    1,
                    (ip, count) => count + 1);

                // QuestAddHandler özel durumu için kontrol.
                CheckAndUnblockQuestAddHandler(remoteIpEndPoint.Address);
            }
        }

        /// <summary>
        /// Belirli bir EndPoint'in engellenmiş olup olmadığını döner.
        /// </summary>
        /// <param name="endPoint">Kontrol edilecek EndPoint.</param>
        /// <returns>Engellenmişse true, değilse false.</returns>
        public static bool IsBlocked(EndPoint endPoint)
        {
            IPEndPoint ipEndPoint = endPoint as IPEndPoint;
            return !(ipEndPoint == null) && BlockList.Contains(ipEndPoint.Address);
        }

        /// <summary>
        /// Belirli bir IP'yi engeller. Whitelist'teyse işlem yapılmaz.
        /// </summary>
        /// <param name="ipAddress">Engellenecek IP adresi.</param>
        /// <param name="Save">Dosya ve güvenlik duvarına kaydetmek isteniyorsa true.</param>
        public static void BlockIP(IPAddress ipAddress, bool Save = true)
        {
            // Yeni: Whitelist kontrolü not: yuti
            if (IsWhiteListed(ipAddress))
            {
                log.Info($"IP whitelist'te, engelleme atlandı: {ipAddress}");
                return;
            }

            // Race condition önlemek için lock kullan.
            lock (BlockList)
            {
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
        }

        /// <summary>
        /// Windows güvenlik duvarına belirli bir IP için engelleme kuralı ekler.
        /// </summary>
        /// <param name="ipAddress">Engellenecek IP adresi (string).</param>
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

        /// <summary>
        /// Windows güvenlik duvarından belirli bir IP için engelleme kuralını siler.
        /// </summary>
        /// <param name="ipAddress">Engellemesi kaldırılacak IP adresi (string).</param>
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

        /// <summary>
        /// Belirli bir IP'nin engellemesini kaldırır.
        /// </summary>
        /// <param name="ipAddress">Engellemesi kaldırılacak IP adresi.</param>
        /// <param name="Save">Dosya ve güvenlik duvarından da kaldırılacaksa true.</param>
        /// <returns>Engellemesi kaldırıldıysa true, yoksa false.</returns>
        public static bool RemoveBlockIP(IPAddress ipAddress, bool Save = true)
        {
            bool result = false;
            lock (BlockList)
            {
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
            }
            return result;
        }

        /// <summary>
        /// Dosyadan engellenmiş IP'leri yükler ve belleğe ekler.
        /// Whitelist'teki IP'ler engellenmez.
        /// </summary>
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
                            // Yeni: Whitelist kontrolü not: yuti
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

        /// <summary>
        /// Yeni bir paket geldiğinde çağrılır. IP'yi PacketList'e ekler veya sayısını artırır.
        /// </summary>
        /// <param name="endPoint">Paketi gönderen EndPoint.</param>
        public static void LogNewPacket(EndPoint endPoint)
        {
            IPEndPoint remoteIpEndPoint = endPoint as IPEndPoint;
            if (remoteIpEndPoint != null)
            {
                // ConcurrentDictionary.AddOrUpdate ile thread-safe artırma.
                PacketList.AddOrUpdate(
                    remoteIpEndPoint.Address,
                    1,
                    (ip, count) => count + 1);
            }
        }
    }
}
