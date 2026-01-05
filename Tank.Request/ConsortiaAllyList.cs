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
    // Token: 0x02000018 RID: 24
    // ConsortiaAllyList sınıfı, bir loncanın müttefiklerini listelemek için kullanılan bir HTTP Handler'dır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ConsortiaAllyList : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x06000060 RID: 96 RVA: 0x00005324 File Offset: 0x00003524
        // Gelen isteği karşılayan ve müttefik listesini hazırlayan metod
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
                int consortiaID = int.Parse(context.Request["consortiaID"]); // Müttefikleri listelenecek Lonca ID'si
                int state = int.Parse(context.Request["state"]); // Durum filtresi (Başvurular vs.)

                // Arama yapılacak Lonca adı (SQL Injection koruması ve Decode işlemi uygulanıyor)
                string searchName = csFunction.ConvertSql(HttpUtility.UrlDecode((context.Request["name"] == null) ? "" : context.Request["name"]));

                // --- 2. VERİTABANI İŞLEMLERİ ---
                using (ConsortiaBussiness db = new ConsortiaBussiness())
                {
                    // Veritabanından sayfalama mantığıyla müttefik listesini çek
                    ConsortiaAllyInfo[] allianceList = db.GetConsortiaAllyPage(page, pageSize, ref totalCount, order, consortiaID, state, searchName);

                    // Çekilen her bir müttefik kaydını döngüye al
                    foreach (ConsortiaAllyInfo alliance in allianceList)
                    {
                        // Müttefik bilgisini XML formatına çevirip sonuç listesine ekle
                        // FlashUtils, Flash istemcilerine uygun XML oluşturmak için kullanılan bir yardımcı sınıftır.
                        resultXml.Add(FlashUtils.CreateConsortiaAllyInfo(alliance));
                    }

                    isSuccess = true;
                    message = "Başarılı!";
                }
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                ConsortiaAllyList.log.Error("ConsortiaAllyList yüklenirken hata:", ex);
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

        // Token: 0x17000014 RID: 20
        // (get) Token: 0x06000061 RID: 97 RVA: 0x00003828 File Offset: 0x00001A28
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