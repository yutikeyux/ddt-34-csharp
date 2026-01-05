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
    // Token: 0x02000008 RID: 8
    // ActiveList sınıfı, oyunun aktif etkinliklerini listelemek için kullanılan bir HTTP Handler'dır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ActiveList : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x0600001B RID: 27 RVA: 0x00003AC8 File Offset: 0x00001CC8
        // Gelen isteği karşılayan metod
        public void ProcessRequest(HttpContext context)
        {
            // İstek yapanın IP adresi yetkili mi kontrol et
            if (csFunction.ValidAdminIP(context.Request.UserHostAddress))
            {
                // Yetkiliyse etkinlik listesini çekip yanıt olarak gönder
                context.Response.Write(ActiveList.Build(context));
            }
            else
            {
                // Yetkili değilse hata mesajı döndür
                context.Response.Write("Tabi Efendim!");
            }
        }

        // Token: 0x0600001C RID: 28 RVA: 0x00003B14 File Offset: 0x00001D14
        // Veritabanından etkinlikleri (Active) çekip XML formatına çeviren metod.
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
                using (ActiveBussiness db = new ActiveBussiness())
                {
                    // Tüm aktif etkinlikleri veritabanından çek
                    ActiveInfo[] activeList = db.GetAllActives();

                    // Her bir etkinliği döngüye al
                    foreach (ActiveInfo activeItem in activeList)
                    {
                        // Etkinlik bilgilerini XML formatına çevirip sonuç listesine ekle
                        // FlashUtils, Flash istemcilerine uygun XML oluşturmak için kullanılan bir yardımcı sınıftır.
                        resultXml.Add(FlashUtils.CreateActiveInfo(activeItem));
                    }

                    isSuccess = true;
                    message = "Başarılı!";
                }
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                ActiveList.log.Error("Aktiflik listesi yüklenirken hata:", ex);
            }

            // XML'e genel durum bilgilerini (value ve message) ekle
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // --- NOT: Orijinal kodda burası iki kez çağrılıyor ---
            // İlk çağrı: Sıkıştırma kapalı (false), muhtemelen dosyaya yazma veya loglama amaçlı.
            csFunction.CreateCompressXml(context, resultXml, "ActiveList_out", false);

            // İkinci çağrı: Sıkıştırma açık (true), istemciye gönderilecek asıl sonuç.
            return csFunction.CreateCompressXml(context, resultXml, "ActiveList", true);
        }

        // Token: 0x17000005 RID: 5
        // (get) Token: 0x0600001D RID: 29 RVA: 0x00003828 File Offset: 0x00001A28
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