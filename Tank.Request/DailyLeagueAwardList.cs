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
    // Token: 0x0200002B RID: 43
    // DailyLeagueAwardList sınıfı, günlük lig ödüllerini listelemek için kullanılan bir HTTP Handler'dır.
    public class DailyLeagueAwardList : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x060000BA RID: 186 RVA: 0x00007954 File Offset: 0x00005B54
        // Gelen isteği karşılayan metod
        public void ProcessRequest(HttpContext context)
        {
            // İstek yapanın IP adresi yetkili mi kontrol et
            if (csFunction.ValidAdminIP(context.Request.UserHostAddress))
            {
                // Yetkiliyse veriyi çekip yanıt olarak gönder
                context.Response.Write(DailyLeagueAwardList.Build(context));
            }
            else
            {
                // Yetkili değilse espirili hata mesajı döndür
                context.Response.Write("Tabi Efendim!");
            }
        }

        // Token: 0x17000027 RID: 39
        // (get) Token: 0x060000BB RID: 187 RVA: 0x00003828 File Offset: 0x00001A28
        // IHttpHandler arayüzünün zorunlu üyesi.
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        // Token: 0x060000BC RID: 188 RVA: 0x000079A0 File Offset: 0x00005BA0
        // Veritabanından günlük lig ödüllerini çekip XML formatına çeviren metod
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
                    // Tüm günlük lig ödüllerini veritabanından çek
                    DailyLeagueAwardInfo[] awardList = db.GetAllDailyLeagueAward();

                    // Her bir ödülü döngüye al
                    foreach (DailyLeagueAwardInfo award in awardList)
                    {
                        // Ödül bilgisini XML formatına çevirip sonuç listesine ekle
                        // FlashUtils, Flash istemcilerine uygun XML oluşturmak için kullanılan bir yardımcı sınıftır.
                        resultXml.Add(FlashUtils.CreateDailyLeagueAward(award));
                    }

                    isSuccess = true;
                    message = "Başarılı!";
                }
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                DailyLeagueAwardList.log.Error("DailyLeagueAwardList yüklenirken hata:", ex);
            }

            // XML'e genel durum bilgilerini (value ve message) ekle
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // --- NOT: Orijinal kodda burası iki kez çağrılıyor ---
            // İlk çağrı: Sıkıştırma kapalı (false), muhtemelen sunucu taraflı loglama veya dosyaya yazma için.
            // Dosya adı "dailyleagueaward_out" olarak kullanılmış.
            csFunction.CreateCompressXml(context, resultXml, "dailyleagueaward_out", false);

            // İkinci çağrı: Sıkıştırma açık (true), istemciye gönderilecek asıl sonuç.
            return csFunction.CreateCompressXml(context, resultXml, "dailyleagueaward", true);
        }
    }
}