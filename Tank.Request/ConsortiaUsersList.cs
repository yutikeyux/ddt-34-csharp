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
    // Token: 0x02000025 RID: 37
    // ConsortiaUsersList sınıfı, bir loncadaki üyeleri listelemek için kullanılan bir HTTP Handler'dır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ConsortiaUsersList : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x06000096 RID: 150 RVA: 0x00006898 File Offset: 0x00004A98
        // Gelen isteği karşılayan ve üye listesini hazırlayan metod
        public void ProcessRequest(HttpContext context)
        {
            bool isSuccess = false;
            string message = "Hata!";
            int totalCount = 0;

            // Kök XML elementini oluştur
            XElement resultXml = new XElement("Result");

            try
            {
                // --- 1. PARAMETRELERİ AL ---
                int page = int.Parse(context.Request["page"]); // Hangi sayfa
                int pageSize = int.Parse(context.Request["size"]); // Sayfa boyutu
                int order = int.Parse(context.Request["order"]); // Sıralama kriteri
                int consortiaID = int.Parse(context.Request["consortiaID"]); // Lonca ID'si
                int userID = int.Parse(context.Request["userID"]); // Filtre: Oyuncu ID'si
                int state = int.Parse(context.Request["state"]); // Filtre: Durumu (Çevrimiçi/Kapalı vb.)

                // --- 2. VERİTABANI İŞLEMLERİ ---
                using (ConsortiaBussiness db = new ConsortiaBussiness())
                {
                    // Veritabanından sayfalama mantığıyla üye listesini çek
                    ConsortiaUserInfo[] userList = db.GetConsortiaUsersPage(page, pageSize, ref totalCount, order, consortiaID, userID, state);

                    // Çekilen her üyeyi döngüye al
                    foreach (ConsortiaUserInfo user in userList)
                    {
                        // Üye bilgisini XML formatına çevirip sonuç listesine ekle
                        // FlashUtils, Flash istemcilerine uygun XML oluşturmak için kullanılan bir yardımcı sınıftır.
                        resultXml.Add(FlashUtils.CreateConsortiaUserInfo(user));
                    }

                    isSuccess = true;
                    message = "Başarılı!";
                }
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                ConsortiaUsersList.log.Error("ConsortiaUsersList yüklenirken hata:", ex);
            }

            // --- 3. YANITI HAZIRLAMA ---
            // Toplam kayıt sayısı, işlem durumu ve mesajı XML'e ekle
            resultXml.Add(new XAttribute("total", totalCount));
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // ÖZEL ÖZELLİK: Sunucunun şu anki tarih ve saatini XML'e ekle.
            // Bu muhtemelen istemcinin zaman senkronizasyonu veya listelenme zamanı için kullanılır.
            resultXml.Add(new XAttribute("currentDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));

            // Yanıtı ekrana yaz
            context.Response.ContentType = "text/plain";
            context.Response.Write(resultXml.ToString(false));
        }

        // Token: 0x17000021 RID: 33
        // (get) Token: 0x06000097 RID: 151 RVA: 0x00003828 File Offset: 0x00001A28
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