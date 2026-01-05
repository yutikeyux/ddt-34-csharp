using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using Bussiness.CenterService; // WCF Servis kütüphanesi
using log4net; // Loglama kütüphanesi

namespace Tank.Request
{
    // AASGetState sınıfı, Admin veya Sistem durumu kontrolü için kullanılan bir Page sınıfıdır.
    public class AASGetState : Page
    {
        // Log4net ile loglama nesnesi tanımlanır (Statik ve Read-Only)
        // Hataları takip etmek için kullanılır.
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x17000001 RID: 1
        // Web.config dosyasında tanımlı "AdminIP" ayarını okuyan özellik (Property)
        public static string GetAdminIP
        {
            get
            {
                // AppSettings içindeki "AdminIP" değerini döndürür.
                // Genellikle birden fazla IP "192.168.1.1|10.0.0.5" şeklinde '|' karakteri ile ayrılarak tanımlanır.
                return ConfigurationManager.AppSettings["AdminIP"];
            }
        }

        // Token: 0x06000006 RID: 6 RVA: 0x000034EC File Offset: 0x000016EC
        // Gelen IP adresinin, erişime izin verilen Admin IP'leri arasında olup olmadığını kontrol eder
        public static bool ValidLoginIP(string ip)
        {
            string adminIps = AASGetState.GetAdminIP;

            // Mantık:
            // 1. AdminIP listesi boşsa herkese izin verilebilir (veya güvenlik duvarına bırakılır).
            // 2. Eğer AdminIP doluysa ve gelen IP bu listede yoksa 'flag' true olur (Reddedilecek).
            // 3. Fonksiyon true dönerse erişime izin var, false dönerse yasak demektir.

            bool flag = !string.IsNullOrEmpty(adminIps) && !adminIps.Split(new char[]
            {
                '|'
            }).Contains(ip);

            // !flag ile ters mantığı uygular: Listedeyse True, değilse False döner.
            return !flag;
        }

        // Token: 0x06000007 RID: 7 RVA: 0x00003534 File Offset: 0x00001734
        // Sayfa yüklendiğinde çalışır (Genellikle AJAX ile çağrılır)
        protected void Page_Load(object sender, EventArgs e)
        {
            // Varsayılan durum değeri. "2" hata veya durum alınamadığı anlamına gelebilir.
            int serverState = 2;

            try
            {
                // İstek yapanın IP adresini alır
                // Not: Yüksek güvenlik gerektiren admin panellerinde Proxy arkası IP (X-Forwarded-For) kullanmak 
                // IP spoofing riski oluşturabilir, bu yüzden UserHostAddress daha güvenlidir.
                string clientIP = this.Context.Request.UserHostAddress;

                // IP adresi yetkili mi kontrol et
                bool isAuthorized = AASGetState.ValidLoginIP(clientIP);

                if (isAuthorized)
                {
                    // CenterServiceClient muhtemelen bir WCF servisinin istemcisidir (Client).
                    // 'using' bloğu, iş bitince servisin kaynaklarını (Connection) doğru şekilde serbest bırakır (Dispose).
                    using (CenterServiceClient centerServiceClient = new CenterServiceClient())
                    {
                        // Servis üzerinden AASGetState() metodunu çağırıp sunucu durumunu alır
                        serverState = centerServiceClient.AASGetState();
                    }
                }
                else
                {
                    // Yetkisiz erişim durumunda loglama yapılabilir
                    // log.Warn($"Yetkisiz erişim denemesi: {clientIP}");
                    serverState = 2; // Yetkisiz erişimde varsayılan hata kodu döner
                }
            }
            catch (Exception ex)
            {
                // Servis bağlantı hatası veya diğer beklenmeyen hatalar loglanır
                AASGetState.log.Error("AASGetState servisi hatası:", ex);
                serverState = 2; // Hata oluştuğunda hata kodu korunur
            }

            // Sonuç (sayısal değer) doğrudan HTTP Response'a yazılır.
            // Bu, JavaScript tarafında okunabilir (örn: alert(result);)
            base.Response.Write(serverState);
        }
    }
}