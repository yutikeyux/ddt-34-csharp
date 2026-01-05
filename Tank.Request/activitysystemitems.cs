using System;
using System.Reflection;
using System.Web;
using System.Xml.Linq;
using Bussiness; // İş katmanı kütüphanesi
using log4net; // Loglama kütüphanesi
using Road.Flash; // Flash istemcisi yardımcı kütüphanesi
using SqlDataProvider.Data; // Veritabanı veri yapıları

namespace Tank.Request
{
    // Token: 0x0200000A RID: 10
    // activitysystemitems sınıfı, etkinlik sistemindeki öğeleri listelemek için kullanılan bir HTTP Handler'dır.
    // Not: Sınıf ismi küçük harfle başlıyor (activitysystemitems), bu genellikle proje standartları böyle tanımlandığından öyledir.
    public class activitysystemitems : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x06000024 RID: 36 RVA: 0x00003DFC File Offset: 0x00001FFC
        // Gelen isteği karşılayan metod
        public void ProcessRequest(HttpContext context)
        {
            // İstek yapanın IP adresi yetkili mi kontrol et
            if (csFunction.ValidAdminIP(context.Request.UserHostAddress))
            {
                // Yetkiliyse veriyi çekip yanıt olarak gönder
                context.Response.Write(activitysystemitems.Build(context));
            }
            else
            {
                // Yetkili değilse hata mesajı döndür
                context.Response.Write("Tabi Efendim!");
            }
        }

        // Token: 0x06000025 RID: 37 RVA: 0x00003E48 File Offset: 0x00002048
        // Veritabanından etkinlik öğelerini (ActivitySystemItem) çekip XML formatına çeviren metod.
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
                    // Tüm etkinlik sistemi öğelerini veritabanından çek
                    ActivitySystemItemInfo[] allItems = db.GetAllActivitySystemItem();

                    // Her bir öğeyi döngüye al
                    foreach (ActivitySystemItemInfo item in allItems)
                    {
                        // Öğe bilgilerini XML formatına çevirip sonuç listesine ekle
                        // FlashUtils, Flash istemcilerine uygun XML oluşturmak için kullanılan bir yardımcı sınıftır.
                        resultXml.Add(FlashUtils.CreateActivitySystemItems(item));
                    }

                    isSuccess = true;
                    message = "Başarılı!";
                }
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                activitysystemitems.log.Error("ActivitySystemItems listesi yüklenirken hata:", ex);
            }

            // XML'e genel durum bilgilerini (value ve message) ekle
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // --- NOT: Orijinal kodda burası iki kez çağrılıyor ---
            // İlk çağrı: Sıkıştırma kapalı (false), muhtemelen sunucu taraflı loglama veya dosyaya yazma için.
            csFunction.CreateCompressXml(context, resultXml, "activitysystemitems_out", false);

            // İkinci çağrı: Sıkıştırma açık (true), istemciye gönderilecek asıl sonuç.
            return csFunction.CreateCompressXml(context, resultXml, "activitysystemitems", true);
        }

        // Token: 0x17000007 RID: 7
        // (get) Token: 0x06000026 RID: 38 RVA: 0x00003828 File Offset: 0x00001A28
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