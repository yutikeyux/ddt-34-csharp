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
    // Token: 0x02000020 RID: 32
    // ConsortiaIMList sınıfı, bir loncanın üye listesini ve temel bilgilerini getirmek için kullanılan bir HTTP Handler'dır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ConsortiaIMList : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x06000081 RID: 129 RVA: 0x000060CC File Offset: 0x000042CC
        // Gelen isteği karşılayan ve liste hazırlayan metod
        public void ProcessRequest(HttpContext context)
        {
            bool isSuccess = false;
            string message = "Hata!";
            int totalCount = 0;

            // Kök XML elementini oluştur
            XElement resultXml = new XElement("Result");

            try
            {
                // Lonca ID'sini al
                int consortiaID = int.Parse(context.Request["id"]);

                // --- 1. ADIM: LONCA BİLGİLERİNİ (ROOT ATTRIBUTES) ALMA ---
                using (ConsortiaBussiness dbConsortia = new ConsortiaBussiness())
                {
                    ConsortiaInfo consortiaInfo = dbConsortia.GetConsortiaSingle(consortiaID);

                    if (consortiaInfo != null)
                    {
                        // Lonca bilgisi varsa, XML'in kök elementine seviye ve itibar (repute) ekle
                        resultXml.Add(new XAttribute("Level", consortiaInfo.Level));
                        resultXml.Add(new XAttribute("Repute", consortiaInfo.Repute));
                    }
                }

                // --- 2. ADIM: LONCA ÜYELERİNİ LİSTELEME ---
                // NOT: Orijinal kodda ikinci bir using bloğu açılmış. Veritabanı bağlantısı kapatıp tekrar açmak verimsizdir 
                // ancak orijinal yapı bozulmamalıdır.
                using (ConsortiaBussiness dbUsers = new ConsortiaBussiness())
                {
                    // Sayfa: 1, Boyut: 1000. (İlk 1000 üyeyi getir)
                    ConsortiaUserInfo[] userList = dbUsers.GetConsortiaUsersPage(1, 1000, ref totalCount, -1, consortiaID, -1, -1);

                    // Çekilen her üyeyi döngüye al
                    foreach (ConsortiaUserInfo userInfo in userList)
                    {
                        // Üye bilgisini XML formatına çevirip sonuç listesine ekle
                        // FlashUtils, Flash istemcilerine uygun XML oluşturmak için kullanılan bir yardımcı sınıftır.
                        resultXml.Add(FlashUtils.CreateConsortiaIMInfo(userInfo));
                    }

                    isSuccess = true;
                    message = "Başarılı!";
                }
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                ConsortiaIMList.log.Error("ConsortiaIMList yüklenirken hata:", ex);
            }

            // --- 3. YANITI HAZIRLAMA ---
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // Yanıtı ekrana yaz
            context.Response.Write(resultXml.ToString(false));
        }

        // Token: 0x1700001C RID: 28
        // (get) Token: 0x06000082 RID: 130 RVA: 0x00003828 File Offset: 0x00001A28
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