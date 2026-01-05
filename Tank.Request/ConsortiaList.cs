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
    // Token: 0x02000023 RID: 35
    // ConsortiaList sınıfı, oyun içindeki loncaları listelemek için kullanılan bir HTTP Handler'dır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ConsortiaList : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x0600008E RID: 142 RVA: 0x00006514 File Offset: 0x00004714
        // Gelen isteği karşılayan ve lonca listesini hazırlayan metod
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
                int consortiaID = int.Parse(context.Request["consortiaID"]); // Filtre için Lonca ID'si

                // Güvenlik Önlemi: Orijinal kodda null kontrolü yoktu, ekleme yapıldı.
                // İsim araması yapılacaksa decode edilir ve SQL Injection temizliği (ConvertSql) yapılır.
                string searchName = csFunction.ConvertSql(HttpUtility.UrlDecode((context.Request["name"] == null) ? "" : context.Request["name"]));

                int level = int.Parse(context.Request["level"]); // Filtre için Seviye
                int openApply = int.Parse(context.Request["openApply"]); // Filtre: Başvuru Açık/Kapalı (1/0)

                // --- 2. VERİTABANI İŞLEMLERİ ---
                using (ConsortiaBussiness db = new ConsortiaBussiness())
                {
                    // Veritabanından sayfalama mantığıyla lonca listesini çek
                    ConsortiaInfo[] consortiaList = db.GetConsortiaPage(page, pageSize, ref totalCount, order, searchName, consortiaID, level, openApply);

                    // Çekilen her loncayı döngüye al
                    foreach (ConsortiaInfo consortia in consortiaList)
                    {
                        // Lonca bilgisini XML formatına çevirip sonuç listesine ekle
                        // FlashUtils, Flash istemcilerine uygun XML oluşturmak için kullanılan bir yardımcı sınıftır.
                        resultXml.Add(FlashUtils.CreateConsortiaInfo(consortia));
                    }

                    isSuccess = true;
                    message = "Başarılı!";
                }
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                ConsortiaList.log.Error("ConsortiaList yüklenirken hata:", ex);
            }

            // --- 3. YANITI HAZIRLAMA ---
            // Toplam kayıt sayısı, işlem durumu ve mesajı XML'e ekle
            resultXml.Add(new XAttribute("total", totalCount));
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            context.Response.ContentType = "text/plain";

            // --- 4. ÇIKTI (OUTPUT) ---
            // Diğer dosyalardan farklı olarak burada 'csFunction.CreateCompressXml' kullanılmamıştır.
            // Bunun yerine XML string'e çevrilip, manuel olarak sıkıştırılır (Compress) ve Binary olarak yazılır.
            context.Response.BinaryWrite(StaticFunction.Compress(resultXml.ToString(false)));
        }

        // Token: 0x1700001F RID: 31
        // (get) Token: 0x0600008F RID: 143 RVA: 0x00003828 File Offset: 0x00001A28
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