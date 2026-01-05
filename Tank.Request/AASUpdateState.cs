using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using Bussiness.CenterService; // Merkez servisi iletişim kütüphanesi
using log4net;

namespace Tank.Request
{
    // AASUpdateState sınıfı, yöneticilerin sunucu durumunu (açık/kapalı vb.) güncellemesine izin verir.
    public class AASUpdateState : Page
    {
        // Log4net ile loglama nesnesi (Hataları takip için)
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x17000002 RID: 2
        // Web.config dosyasında tanımlı "AdminIP" ayarını okuyan özellik.
        // DÜZELTME: Orijinal kodda kullanılan eski "ConfigurationManager" sınıfı yerine,
        // .NET'in standart ve güncel "ConfigurationManager" sınıfı kullanıldı.
        public static string GetAdminIP
        {
            get
            {
                return ConfigurationManager.AppSettings["AdminIP"];
            }
        }

        // Token: 0x0600000B RID: 11 RVA: 0x000035E8 File Offset: 0x000017E8
        // Gelen IP adresinin yetkili olup olmadığını kontrol eden metod.
        public static bool ValidLoginIP(string ip)
        {
            string allowedIPs = AASUpdateState.GetAdminIP;

            // Mantık:
            // 1. Eğer allowedIPs listesi boşsa (string.IsNullOrEmpty) -> Herkese izin ver (True).
            // 2. Eğer doluysa -> Gelen IP'nin '|' ile ayrılmış listede olup olmadığına bakar.
            return string.IsNullOrEmpty(allowedIPs) || allowedIPs.Split(new char[] { '|' }).Contains(ip);
        }

        // Token: 0x0600000C RID: 12 RVA: 0x0000362C File Offset: 0x0000182C
        // Sayfa yüklendiğinde çalışır (Genellikle AJAX ile çağrılır)
        protected void Page_Load(object sender, EventArgs e)
        {
            // Varsayılan hata durumu (2)
            int result = 2;

            try
            {
                // 1. İstekten 'state' parametresini alıyoruz (Örn: ?state=true veya ?state=false)
                // Bu parametre sunucunun durumu (Online/Offline gibi) belirtir.
                bool newState = bool.Parse(base.Request["state"]);

                // 2. İstek yapan kişinin IP adresini alıyoruz
                string clientIP = this.Context.Request.UserHostAddress;

                // 3. IP yetkili mi kontrol ediyoruz
                bool isAuthorized = AASUpdateState.ValidLoginIP(clientIP);

                if (isAuthorized)
                {
                    // 4. WCF Servisine bağlanıp durumu güncelliyoruz
                    using (CenterServiceClient serviceClient = new CenterServiceClient())
                    {
                        // Servis metodu yeni durumu gönderiyor
                        bool updateSuccess = serviceClient.AASUpdateState(newState);

                        // 5. Servis sonucuna göre dönüş kodu belirle
                        if (updateSuccess)
                        {
                            // Başarılı işlem
                            result = 0;
                        }
                        else
                        {
                            // Servis çağrısı başarısız döndü
                            result = 1;
                        }
                    }
                }
                else
                {
                    // IP yetkili değilse result = 2 kalır (Yetkisiz Erişim/Hata)
                    // Burada isteğe bağlı özel bir log da eklenebilir.
                }
            }
            catch (Exception ex)
            {
                // Beklenmeyen bir hata olursa (Parametre eksik veya servis hatası vb.)
                AASUpdateState.log.Error("AASUpdateState İşlem Hatası:", ex);
                // result zaten hata kodu (2) olarak tanımlı, burada değiştirmeye gerek yok.
            }

            // Sonuç kodunu tarayıcıya/çağıran yere yazar
            base.Response.Write(result);
        }
    }
}