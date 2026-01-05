using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness; // İş katmanı kütüphanesi
using log4net; // Loglama kütüphanesi

namespace Tank.Request
{
    // Token: 0x02000036 RID: 54
    // FarmGetUserFieldInfosSingle sınıfı, bir arkadaşın çiftlik alanının yemlenme durumunu kontrol eden bir HTTP Handler'dır.
    public class FarmGetUserFieldInfosSingle : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x060000FE RID: 254 RVA: 0x00008E08 File Offset: 0x00007008
        // Gelen isteği karşılayan metod
        public void ProcessRequest(HttpContext context)
        {
            bool isSuccess = false;
            string message = "Hata!";

            // Kök XML elementini oluştur
            XElement resultXml = new XElement("Result");

            try
            {
                // İstekten Arkadaş ID'sini al
                string friendID = context.Request["friendID"];

                // Arkadaşın bilgisini (Item) içeren XML elementi oluştur
                XElement itemNode = new XElement("Item", new object[]
                {
                    new XAttribute("UserID", friendID),
                    // NOT: 'isFeed' değeri burada SABİT 'false' (Yemlenmiyor) olarak ayarlanmış.
                    // Muhtemelen bu arkadaşın tarlasının şu anki durumu veya yetkisini temsil eder.
                    new XAttribute("isFeed", false)
                });

                // Hazırlanan item'ı sonuc listesine ekle
                resultXml.Add(itemNode);

                isSuccess = true;
                message = "Başarılı!";
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                FarmGetUserFieldInfosSingle.log.Error("FarmGetUserFieldInfosSingle yüklenirken hata:", ex);
            }

            // XML'e genel durum bilgilerini (value ve message) ekle
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // Yanıtı ekrana yaz
            context.Response.ContentType = "text/plain";
            context.Response.Write(resultXml.ToString(false));
        }

        // Token: 0x17000037 RID: 55
        // (get) Token: 0x060000FF RID: 255 RVA: 0x00003828 File Offset: 0x00001A28
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