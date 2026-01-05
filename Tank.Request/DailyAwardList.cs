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
    // Token: 0x0200002A RID: 42
    // DailyAwardList sınıfı, oyun içindeki günlük ödüllerin listesini almak için kullanılan bir HTTP Handler'dır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class DailyAwardList : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x060000B5 RID: 181 RVA: 0x00002474 File Offset: 0x00000674
        // Gelen isteği karşılayan metod
        public void ProcessRequest(HttpContext context)
        {
            // Veriyi hazırlayan metodu çağır ve sonucu yaz
            context.Response.Write(DailyAwardList.Build(context));
        }

        // Token: 0x060000B6 RID: 182 RVA: 0x0000785C File Offset: 0x00005A5C
        // Veritabanından günlük ödülleri çekip XML formatına çeviren metod
        public static string Build(HttpContext context)
        {
            bool isSuccess = false;
            string message = "Hata!";

            // Kök XML elementini oluştur
            XElement resultXml = new XElement("Result");

            try
            {
                // Veritabanı işlemleri için bağlantı oluştur
                using (ProduceBussiness db = new ProduceBussiness())
                {
                    // Tüm günlük ödülleri veritabanından çek
                    DailyAwardInfo[] awardList = db.GetAllDailyAward();

                    // Her bir ödülü döngüye al
                    foreach (DailyAwardInfo award in awardList)
                    {
                        // NOT: Burada `FlashUtils.CreateActiveInfo` kullanılmış.
                        // Muhtemelen DailyAwardInfo yapısı ile ActiveInfo yapısı XML'de benzer düzende olduğu için bu kullanılmıştır.
                        resultXml.Add(FlashUtils.CreateActiveInfo(award));
                    }

                    isSuccess = true;
                    message = "Başarılı!";
                }
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                DailyAwardList.log.Error("DailyAwardList yüklenirken hata:", ex);
            }

            // XML'e genel durum bilgilerini (value ve message) ekle
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // Sonucu sıkıştırıp (Compress) döndür
            return csFunction.CreateCompressXml(context, resultXml, "DailyAwardList", true);
        }

        // Token: 0x17000026 RID: 38
        // (get) Token: 0x060000B7 RID: 183 RVA: 0x00003828 File Offset: 0x00001A28
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