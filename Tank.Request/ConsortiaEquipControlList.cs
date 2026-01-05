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
    // Token: 0x0200001E RID: 30
    // ConsortiaEquipControlList sınıfı, lonca ekipman kontrolünü listelemek için kullanılan bir HTTP Handler'dır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ConsortiaEquipControlList : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x06000079 RID: 121 RVA: 0x00005DB0 File Offset: 0x00003FB0
        // Gelen isteği karşılayan ve ekipman kontrol listesini hazırlayan metod
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
                // NOT: 'page', 'size' ve 'order' değerleri burada SABİTLENMİŞTİR.
                // İstemciden bu parametreler alınmaz. Her zaman ilk sayfa ve 10 kayıt gelir.
                int page = 1;
                int pageSize = 10;
                int order = 1;

                // İstemciden gelen filtre parametreleri
                int consortiaID = int.Parse(context.Request["consortiaID"]);
                int level = int.Parse(context.Request["level"]);
                int type = int.Parse(context.Request["type"]);

                // --- 2. VERİTABANI İŞLEMLERİ ---
                using (ConsortiaBussiness db = new ConsortiaBussiness())
                {
                    // Veritabanından sayfalama mantığıyla ekipman kontrol listesini çek
                    ConsortiaEquipControlInfo[] equipList = db.GetConsortiaEquipControlPage(page, pageSize, ref totalCount, order, consortiaID, level, type);

                    // Çekilen her bir kaydı döngüye al
                    foreach (ConsortiaEquipControlInfo equip in equipList)
                    {
                        // Ekipman bilgisini XML formatına çevirip sonuç listesine ekle
                        // FlashUtils, Flash istemcilerine uygun XML oluşturmak için kullanılan bir yardımcı sınıftır.
                        resultXml.Add(FlashUtils.CreateConsortiaEquipControlInfo(equip));
                    }

                    isSuccess = true;
                    message = "Başarılı!";
                }
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                // DÜZELTME: Orijinal kodda "ConsortiaList" yazıyordu, doğru sınıf ismi ile düzeltildi.
                ConsortiaEquipControlList.log.Error("ConsortiaEquipControlList yüklenirken hata:", ex);
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

        // Token: 0x1700001A RID: 26
        // (get) Token: 0x0600007A RID: 122 RVA: 0x00003828 File Offset: 0x00001A28
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