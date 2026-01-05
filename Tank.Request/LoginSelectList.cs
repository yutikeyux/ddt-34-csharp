using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness; // İş katmanı kütüphanesi
using log4net; // Loglama kütüphanesi
using Road.Flash; // Flash istemcisi yardımcı kütüphanesi
using SqlDataProvider.Data; // Veritabanı veri yapıları

namespace Tank.Request
{
    // Token: 0x02000054 RID: 84
    // LoginSelectList sınıfı, belirli bir kullanıcı adı ile ilişkili giriş listesini yüklemek için kullanılan bir HTTP Handler'dır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class LoginSelectList : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x17000055 RID: 85
        // (get) Token: 0x06000180 RID: 384 RVA: 0x0000215A File Offset: 0x0000035A
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06000181 RID: 385 RVA: 0x0000C7BC File Offset: 0x0000A9BC
        // Gelen isteği karşılayan ve giriş listesini hazırlayan metod
        public void ProcessRequest(HttpContext context)
        {
            bool isSuccess = false;
            string message = "Fail!";

            // Kök XML elementini oluştur
            XElement resultXml = new XElement("Result");

            try
            {
                // --- 1. PARAMETRELERİ AL ---
                // Kullanıcı adını al ve URL Decode et
                string username = HttpUtility.UrlDecode(context.Request["username"]);

                // Şifre al ve URL Decode et
                // NOT: Orijinal kodda şifre bu noktada parse edilir ancak veritabanı sorgusunda kullanılmaz.
                // Sorguda sadece 'username' parametresi kullanılır.
                string password = HttpUtility.UrlDecode(context.Request["password"]);

                // --- 2. VERİTABANI İŞLEMLERİ ---
                using (PlayerBussiness pb = new PlayerBussiness())
                {
                    // Kullanıcı adına göre giriş listesini çek (Alt hesaplar vb. olabilir)
                    PlayerInfo[] userLoginList = pb.GetUserLoginList(username);

                    // Liste dolu mu kontrol et
                    bool isListEmpty = (userLoginList.Length == 0);

                    if (!isListEmpty)
                    {
                        // Her bir kullanıcıyı döngüye al
                        foreach (PlayerInfo playerInfo in userLoginList)
                        {
                            // Rumuz (NickName) var mı kontrol et
                            bool hasNickname = !string.IsNullOrEmpty(playerInfo.NickName);

                            if (hasNickname)
                            {
                                // Kullanıcı bilgisini XML formatına çevirip sonuç listesine ekle
                                resultXml.Add(FlashUtils.CreateUserLoginList(playerInfo));
                            }
                        }

                        isSuccess = true;
                        message = "Success!";
                    }
                }
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                LoginSelectList.log.Error("LoginSelectList yüklenirken hata:", ex);
            }

            // --- 3. YANITI GÖNDER (FINALLY) ---
            // Hata olsa bile yanıtı göndermek için finally bloğu kullanılmış
            // Ancak XML'e değerleri en son ekleyip gönderiyoruz.
            finally
            {
                // XML'e genel durum bilgilerini (value ve message) ekle
                resultXml.Add(new XAttribute("value", isSuccess));
                resultXml.Add(new XAttribute("message", message));

                // Yanıtı ekrana yaz
                // DİKKAT: Sıkıştırma (Compress) yapılmaz. Doğrudan XML string'i gönderilir.
                context.Response.ContentType = "text/plain";
                context.Response.Write(resultXml.ToString(false));
            }
        }
    }
}