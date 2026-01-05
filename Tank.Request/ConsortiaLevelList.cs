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
    // Token: 0x02000022 RID: 34
    // ConsortiaLevelList sınıfı, oyun içindeki lonca seviyelerini listelemek için kullanılan bir HTTP Handler'dır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ConsortiaLevelList : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x06000089 RID: 137 RVA: 0x0000239A File Offset: 0x0000059A
        // Gelen isteği karşılayan metod
        public void ProcessRequest(HttpContext context)
        {
            // Çıktı tipini belirle
            context.Response.ContentType = "text/plain";

            // Veriyi hazırlayan metodu çağır ve sonucu yaz
            context.Response.Write(ConsortiaLevelList.Build(context));
        }

        // Token: 0x0600008A RID: 138 RVA: 0x0000641C File Offset: 0x0000461C
        // Veritabanından lonca seviyelerini çekip XML formatına çeviren metod.
        // Not: Orijinal kodda metot adı "Build" (Build) olarak hatalı yazılmış, sistemin uyumluluğu için böyle bırakıldı.
        public static string Build(HttpContext context)
        {
            bool isSuccess = false;
            string message = "Hata!";

            // Kök XML elementini oluştur
            XElement resultXml = new XElement("Result");

            try
            {
                // Veritabanı işlemleri için bağlantı oluştur
                using (ConsortiaBussiness db = new ConsortiaBussiness())
                {
                    // Tüm lonca seviyelerini veritabanından çek
                    ConsortiaLevelInfo[] allLevels = db.GetAllConsortiaLevel();

                    // Her bir seviyeyi döngüye al
                    foreach (ConsortiaLevelInfo level in allLevels)
                    {
                        // Seviye bilgisini XML formatına çevirip sonuç listesine ekle
                        // FlashUtils, Flash istemcilerine uygun XML oluşturmak için kullanılan bir yardımcı sınıftır.
                        // Not: Yardımcı metod ismi 'CreateConsortiLevelInfo' olarak geçiyor (Typo muhtemelen).
                        resultXml.Add(FlashUtils.CreateConsortiLevelInfo(level));
                    }
                }

                isSuccess = true;
                message = "Başarılı!";
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                ConsortiaLevelList.log.Error("ConsortiaLevelList yüklenirken hata:", ex);
            }

            // XML'e genel durum bilgilerini (value ve message) ekle
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // Sonucu sıkıştırıp (Compress) döndür
            return csFunction.CreateCompressXml(context, resultXml, "ConsortiaLevelList", true);
        }

        // Token: 0x1700001E RID: 30
        // (get) Token: 0x0600008B RID: 139 RVA: 0x00003828 File Offset: 0x00001A28
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