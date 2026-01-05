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
    // Token: 0x02000019 RID: 25
    // ConsortiaApplyAllyList sınıfı, bir loncaya gelen müttefik başvurularını listelemek için kullanılan bir HTTP Handler'dır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ConsortiaApplyAllyList : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x06000064 RID: 100 RVA: 0x00005520 File Offset: 0x00003720
        // Gelen isteği karşılayan ve başvuru listesini hazırlayan metod
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
                int applyID = int.Parse(context.Request["applyID"]); // Belirli bir başvuru ID'si (Filtre için)
                int state = int.Parse(context.Request["state"]); // Durum (Başvuran/Reddedilen/Kabul Edilen vb.)

                // NOT: Önceki dosyadan (ConsortiaAllyList) fark olarak burada "name" (İsim) parametresi YOKTUR.

                // --- 2. VERİTABANI İŞLEMLERİ ---
                using (ConsortiaBussiness db = new ConsortiaBussiness())
                {
                    // Veritabanından sayfalama mantığıyla başvuru listesini çek
                    ConsortiaApplyAllyInfo[] applyList = db.GetConsortiaApplyAllyPage(page, pageSize, ref totalCount, order, consortiaID, applyID, state);

                    // Çekilen her başvuruyu döngüye al
                    foreach (ConsortiaApplyAllyInfo application in applyList)
                    {
                        // Başvuru bilgisini XML formatına çevirip sonuç listesine ekle
                        // FlashUtils, Flash istemcilerine uygun XML oluşturmak için kullanılan bir yardımcı sınıftır.
                        resultXml.Add(FlashUtils.CreateConsortiaApplyAllyInfo(application));
                    }

                    isSuccess = true;
                    message = "Başarılı!";
                }
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                ConsortiaApplyAllyList.log.Error("ConsortiaApplyAllyList yüklenirken hata:", ex);
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

        // Token: 0x17000015 RID: 21
        // (get) Token: 0x06000065 RID: 101 RVA: 0x00003828 File Offset: 0x00001A28
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