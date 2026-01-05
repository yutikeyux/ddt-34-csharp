using System;
using System.Configuration; // 'ConfigurationManager' yerine güncel sınıf
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Bussiness.CenterService; // Merkez servisi (WCF) kütüphanesi
using log4net; // Loglama kütüphanesi

namespace Tank.Request
{
    // Token: 0x02000034 RID: 52
    // ExperienceRate sınıfı, sunucu tecrübe oranlarını yönetmek için kullanılan bir Web Form sayfasıdır.
    public class ExperienceRate : Page
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Web Form sayfası için gerekli Form elemanı
        protected HtmlForm form1;

        // Token: 0x17000035 RID: 53
        // (get) Token: 0x060000F3 RID: 243 RVA: 0x000035C4 File Offset: 0x000017C4
        // Web.config dosyasında tanımlı "AdminIP" ayarını okuyan özellik.
        // DÜZELTME: Orijinal kodda eski "ConfigurationManager" kullanılıyordu, "ConfigurationManager" ile güncellendi.
        public static string GetAdminIP
        {
            get
            {
                return ConfigurationManager.AppSettings["AdminIP"];
            }
        }

        // Token: 0x060000F4 RID: 244 RVA: 0x00008A6C File Offset: 0x00006C6C
        // Gelen IP adresinin yetkili admin IP'leri arasında olup olmadığını kontrol eder
        public static bool ValidLoginIP(string ip)
        {
            string adminIPs = ExperienceRate.GetAdminIP;
            // Mantık:
            // 1. AdminIP listesi boşsa herkese izin ver (True).
            // 2. AdminIP doluysa, Gelen IP'nin listede olup olmadığına bakar.
            return string.IsNullOrEmpty(adminIPs) || adminIPs.Split(new char[] { '|' }).Contains(ip);
        }

        // Token: 0x060000F5 RID: 245 RVA: 0x00008AB0 File Offset: 0x00006CB0
        // Sayfa yüklendiğinde çalışır (Genellikle Admin Panelinden çağrılır)
        protected void Page_Load(object sender, EventArgs e)
        {
            // Varsayılan sonuç kodu (Hata veya Yetkisiz ise bu döner)
            int resultCode = 2;

            try
            {
                // İstekten Sunucu ID'sini al
                int serverId = int.Parse(this.Context.Request["serverId"]);

                // İstek yapanın IP adresi yetkili mi kontrol et
                if (ExperienceRate.ValidLoginIP(this.Context.Request.UserHostAddress))
                {
                    // Yetkiliyse WCF Servisini kullanarak güncelleme yap
                    using (CenterServiceClient serviceClient = new CenterServiceClient())
                    {
                        // Merkez sunucusuna (Game Server) tecrübe oranını güncelleme komutu gönderilir
                        resultCode = serviceClient.ExperienceRateUpdate(serverId);
                    }
                }
                // Else: IP yetkili değilse resultCode = 2 kalır
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                ExperienceRate.log.Error("ExperienceRateUpdate işlem hatası:", ex);
            }

            // Sonuç kodunu ekrana yaz
            base.Response.Write(resultCode);
        }
    }
}