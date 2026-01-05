using System;
using System.Configuration; // 'ConfigurationManager' yerine güncel sınıf
using System.Reflection;
using System.Web;
using System.Web.Services;
using log4net; // Loglama kütüphanesi

namespace Tank.Request
{
    // Token: 0x02000033 RID: 51
    // ExitGameTransit sınıfı, oyuncu çıkış yaptığı (Logout) zamanında kullanıcıyı doğru URL'e yönlendirmek için kullanılır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ExitGameTransit : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x0400003A RID: 58
        // Site bilgisi (Örnek: "web", "mobile" vb.)
        // Not: IHttpHandler'lar her istekte yeniden oluşturulduğu için instance field kullanılabilir.
        private string site = "";

        // Token: 0x17000033 RID: 51
        // (get) Token: 0x060000EE RID: 238 RVA: 0x000088F0 File Offset: 0x00006AF0
        // Belirli bir site için özel çıkış URL'ini Web.config'ten okuyan özellik.
        // Örnek: Site="test" ise key="ExitURL_test" aranır.
        public string LoginURL
        {
            get
            {
                string configKey = "ExitURL_" + this.site;
                // DÜZELTME: Orijinal kodda eski "ConfigurationManager" kullanılmıştı, güncel ile değiştirildi.
                return ConfigurationManager.AppSettings[configKey];
            }
        }

        // Token: 0x060000EF RID: 239 RVA: 0x00008920 File Offset: 0x00006B20
        // Gelen isteği karşılayan ve kullanıcıyı yönlendiren (Redirect) metod
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string username = "";
            string redirectUrl = string.Empty;

            try
            {
                // --- 1. PARAMETRELERİ AL ---
                bool isUsernameProvided = !string.IsNullOrEmpty(context.Request["username"]);
                if (isUsernameProvided)
                {
                    username = HttpUtility.UrlDecode(context.Request["username"]).Trim();
                }

                this.site = (context.Request["site"] == null) ? "" : HttpUtility.UrlDecode(context.Request["site"]).ToLower();

                // --- 2. URL BELİRLEME ---
                bool isSiteProvided = !string.IsNullOrEmpty(this.site);

                if (isSiteProvided)
                {
                    // Site bazlı URL'i bul (Örn: ExitURL_facebook)
                    redirectUrl = this.LoginURL;

                    // --- KULLANICI ADI TEMİZLİĞİ ---
                    // Oyuncu adı muhtemelen Site Prefix'li ile başlar (Örn: "S1_Ahmet").
                    // Bu kod "_" karakterinin sağındaki kısmı ( gerçek kullanıcı adını) alır.
                    int underscoreIndex = username.IndexOf('_');
                    bool hasUnderscore = underscoreIndex != -1;

                    if (hasUnderscore)
                    {
                        // '_' karakterinden bir sonraki karakterden itibaren al
                        username = username.Substring(underscoreIndex + 1);
                    }
                }

                // Eğer site bazlı URL bulunamadıysa veya site boşsa, varsayılan URL'i al
                bool isUrlEmpty = string.IsNullOrEmpty(redirectUrl);
                if (isUrlEmpty)
                {
                    // DÜZELTME: Orijinal kodda eski "ConfigurationManager" kullanılmıştı, güncel ile değiştirildi.
                    redirectUrl = ConfigurationManager.AppSettings["ExitURL"];
                }

                // --- 3. YÖNLENDİRME ---
                // URL string formatı muhtemelen "{0} {1}" şeklindedir. Burada {0}=username, {1}=site yerleşir.
                context.Response.Redirect(string.Format(redirectUrl, username, this.site), false);
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                ExitGameTransit.log.Error("ExitGameTransit hatası:", ex);
            }
        }

        // Token: 0x17000034 RID: 52
        // (get) Token: 0x060000F0 RID: 240 RVA: 0x00003828 File Offset: 0x00001A28
        // IHttpHandler arayüzünün zorunlu üyesi.
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}