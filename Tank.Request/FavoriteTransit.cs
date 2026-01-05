using System;
using System.Configuration; // 'ConfigurationManager' yerine güncel sınıf
using System.Reflection;
using System.Web;
using System.Web.Services;
using log4net; // Loglama kütüphanesi

namespace Tank.Request
{
    // Token: 0x02000037 RID: 55
    // FavoriteTransit sınıfı, oyuncuların favori sayfalarına veya çıkış sayfalarına yönlendirme işlemini yapar.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class FavoriteTransit : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x17000038 RID: 56
        // (get) Token: 0x06000102 RID: 258 RVA: 0x00008F18 File Offset: 0x00007118
        // Varsayılan yönlendirme URL'ini (Web.config'te tanımlı "FavoriteUrl") okuyan özellik.
        // DÜZELTME: Orijinal kodda eski "ConfigurationManager" kullanılıyordu, güncellendi.
        public static string GetFavoriteUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["FavoriteUrl"];
            }
        }

        // Token: 0x06000103 RID: 259 RVA: 0x00008F3C File Offset: 0x0000713C
        // Gelen isteği karşılayan ve yönlendirme işlemi yapan metod
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string redirectUrl = string.Empty;

            try
            {
                // --- 1. PARAMETRELERİ ALMA ---
                string username = (context.Request["username"] == null) ? "" : HttpUtility.UrlDecode(context.Request["username"]);
                string siteKey = (context.Request["site"] == null) ? "" : HttpUtility.UrlDecode(context.Request["site"]).ToLower();

                // --- 2. URL BELİRLEME ---
                bool isSiteProvided = !string.IsNullOrEmpty(siteKey);

                if (isSiteProvided)
                {
                    // Siteye özel bir URL var mı bak (Örn: FavoriteUrl_facebook)
                    string configKey = string.Format("FavoriteUrl_{0}", siteKey);
                    redirectUrl = ConfigurationManager.AppSettings[configKey];

                    // --- 3. KULLANICI ADI TEMİZLİĞİ ---
                    // Kullanıcı adında alt tire (_) varsa ve bu bir site prefix'i ise (Örn: S1_Ahmet)
                    // Site prefix'ini silip sadece kullanıcı adını (Ahmet) almak için kullanılır.
                    int underscoreIndex = username.IndexOf('_');
                    bool hasUnderscore = underscoreIndex != -1;

                    if (hasUnderscore)
                    {
                        // Alt çizgi sonrasındaki karakteri alır (Suffix)
                        username = username.Substring(underscoreIndex + 1);
                    }
                }

                // Eğer hala URL bulunamadıysa varsayılan URL'i al
                bool isUrlEmpty = string.IsNullOrEmpty(redirectUrl);
                if (isUrlEmpty)
                {
                    redirectUrl = FavoriteTransit.GetFavoriteUrl;
                }

                // --- 4. YÖNLENDİRME ---
                // URL formatında string.Format kullanılır. Muhtemelen URL şöyledir:
                // "http://site.com/login.aspx?user={0}&site={1}" -> {0}=username, {1}=site
                context.Response.Redirect(string.Format(redirectUrl, username, siteKey), false);
            }
            catch (Exception ex)
            {
                FavoriteTransit.log.Error("FavoriteTransit yönlendirme hatası:", ex);
            }
        }

        // Token: 0x17000039 RID: 57
        // (get) Token: 0x06000104 RID: 260 RVA: 0x00003828 File Offset: 0x00001A28
        // IHttpHandler arayüzünün zorunlu üyesi.
        // False döndürmek, bu sınıfın bir pool (havuz) içinde tekrar kullanılmayacağını belirtir.
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}