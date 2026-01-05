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
    // Token: 0x0200001F RID: 31
    // ConsortiaEventList sınıfı, bir loncanın etkinliklerini (Event) listelemek için kullanılan bir HTTP Handler'dır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ConsortiaEventList : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x0600007D RID: 125 RVA: 0x00005F38 File Offset: 0x00004138
        // Gelen isteği karşılayan ve etkinlik listesini hazırlayan metod
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
                int pageSize = int.Parse(context.Request["size"]); // Sayfa boyutu (Kaç tane gösterilecek)
                int order = int.Parse(context.Request["order"]); // Sıralama kriteri
                int consortiaID = int.Parse(context.Request["consortiaID"]); // Lonca ID'si

                // --- 2. VERİTABANI İŞLEMLERİ ---
                using (ConsortiaBussiness db = new ConsortiaBussiness())
                {
                    // Veritabanından sayfalama mantığıyla etkinlik listesini çek
                    ConsortiaEventInfo[] eventList = db.GetConsortiaEventPage(page, pageSize, ref totalCount, order, consortiaID);

                    // Çekilen her bir etkinliği döngüye al
                    foreach (ConsortiaEventInfo eventInfo in eventList)
                    {
                        // Etkinlik bilgisini XML formatına çevirip sonuç listesine ekle
                        // FlashUtils, Flash istemcilerine uygun XML oluşturmak için kullanılan bir yardımcı sınıftır.
                        resultXml.Add(FlashUtils.CreateConsortiaEventInfo(eventInfo));
                    }

                    isSuccess = true;
                    message = "Başarılı!";
                }
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                ConsortiaEventList.log.Error("ConsortiaEventList yüklenirken hata:", ex);
            }

            // --- 3. YANITI HAZIRLAMA ---
            // Toplam kayıt sayısı, işlem durumu ve mesajı XML'e ekle
            resultXml.Add(new XAttribute("total", totalCount));
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // Yanıtı ekrana yaz
            context.Response.ContentType = "text/plain";
            context.Response.Write(resultXml.ToString(false));
        }

        // Token: 0x1700001B RID: 27
        // (get) Token: 0x0600007E RID: 126 RVA: 0x00003828 File Offset: 0x00001A28
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