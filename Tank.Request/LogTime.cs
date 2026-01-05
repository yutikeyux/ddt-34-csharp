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
    // Token: 0x02000056 RID: 86
    // LogTime sınıfı, lonca üyelerinin giriş/çıkış (Log) kayıtlarını listelemek için kullanılan bir HTTP Handler'dır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class LogTime : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x06000186 RID: 390 RVA: 0x0000CA20 File Offset: 0x0000AC20
        // Gelen isteği karşılayan ve sayfalı listeyi hazırlayan metod
        public void ProcessRequest(HttpContext context)
        {
            bool isSuccess = false;
            string message = "Fail!";
            int totalCount = 0;

            // Kök XML elementini oluştur
            XElement resultXml = new XElement("Result");

            try
            {
                // --- 1. PARAMETRELERİ AL ---
                int pageIndex = int.Parse(context.Request["page"]);
                int pageSize = int.Parse(context.Request["size"]);
                int order = int.Parse(context.Request["order"]); // Sıralama kriteri
                int consortiaID = int.Parse(context.Request["consortiaID"]); // Lonca ID
                int state = int.Parse(context.Request["state"]); // Durum (Örn: Çevrimiçi vs Tümü)

                // --- 2. İSİM GÜVENLİĞİ ---
                // Gelen 'name' parametresini UrlDecode ile çöz, daha sonra ConvertSql ile temizle (SQL Injection önlemi).
                string consortiumName = csFunction.ConvertSql(HttpUtility.UrlDecode((context.Request["name"] == null) ? "" : context.Request["name"]));

                // --- 3. VERİTABANI İŞLEMLERİ ---
                using (ConsortiaBussiness db = new ConsortiaBussiness())
                {
                    // Sayfalı üye listesini (GetConsortiaAllyPage) veritabanından çek
                    // Not: totalCount 'ref' parametresi ile toplam kayıt sayısını döner.
                    ConsortiaAllyInfo[] allyList = db.GetConsortiaAllyPage(pageIndex, pageSize, ref totalCount, order, consortiaID, state, consortiumName);

                    // Her bir üye bilgisini döngüye al
                    foreach (ConsortiaAllyInfo ally in allyList)
                    {
                        // Üye bilgisini XML formatına çevirip sonuç listesine ekle
                        resultXml.Add(FlashUtils.CreateConsortiaAllyInfo(ally));
                    }
                }

                isSuccess = true;
                message = "Success!";
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                LogTime.log.Error("LogTime yüklenirken hata:", ex);
            }

            // --- 4. YANITI OLUŞTURMA (ORİJİNALDE EKSİKTI) ---
            // XML'e genel durum bilgilerini (total, value ve message) ekle
            resultXml.Add(new XAttribute("total", totalCount));
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // Yanıtı ekrana yaz
            context.Response.ContentType = "text/plain";
            context.Response.Write(resultXml.ToString(false));
        }

        // Token: 0x17000056 RID: 86
        // (get) Token: 0x06000187 RID: 391 RVA: 0x00003828 File Offset: 0x00001A28
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