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
    // Token: 0x0200002C RID: 44
    // DailyLeagueLevelList sınıfı, günlük lig seviyeleriyle ilgili ödülleri listelemek için kullanılan bir HTTP Handler'dır.
    public class DailyLeagueLevelList : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x060000BF RID: 191 RVA: 0x00007AA8 File Offset: 0x00005CA8
        // Gelen isteği karşılayan metod
        public void ProcessRequest(HttpContext context)
        {
            // İstek yapanın IP adresi yetkili mi kontrol et
            if (csFunction.ValidAdminIP(context.Request.UserHostAddress))
            {
                // Yetkiliyse veriyi çekip yanıt olarak gönder
                context.Response.Write(DailyLeagueLevelList.Build(context));
            }
            else
            {
                // Yetkili değilse espirili hata mesajı döndür
                context.Response.Write("Tabi Efendim!");
            }
        }

        // Token: 0x17000028 RID: 40
        // (get) Token: 0x060000C0 RID: 192 RVA: 0x00003828 File Offset: 0x00001A28
        // IHttpHandler arayüzünün zorunlu üyesi.
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        // Token: 0x060000C1 RID: 193 RVA: 0x00007AF4 File Offset: 0x00005CF4
        // Veritabanından ödülleri çekip XML formatına çeviren metod
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
                    // DİKKAT: Sınıf adı "DailyLeagueLevel" iken, burada çekilen veri tipi "FairBattleRewardInfo" (Adil Savaş Ödülü) tir.
                    // Bu muhtemelen kodun kopyala-yapıştır ile hazırlandığını ve ismin güncellenmediğini gösterir.
                    FairBattleRewardInfo[] rewardList = db.GetAllFairBattleReward();

                    // Her bir ödülü döngüye al
                    foreach (FairBattleRewardInfo reward in rewardList)
                    {
                        // Ödül bilgisini XML formatına çevirip sonuç listesine ekle
                        // FlashUtils, Flash istemcilerine uygun XML oluşturmak için kullanılan bir yardımcı sınıftır.
                        resultXml.Add(FlashUtils.CreateFairBattleReward(reward));
                    }

                    isSuccess = true;
                    message = "Başarılı!";
                }
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                DailyLeagueLevelList.log.Error("DailyLeagueLevelList yüklenirken hata:", ex);
            }

            // XML'e genel durum bilgilerini (value ve message) ekle
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // --- NOT: Orijinal kodda burası iki kez çağrılıyor ---
            // İlk çağrı: Sıkıştırma kapalı (false), muhtemelen sunucu taraflı loglama veya dosyaya yazma için.
            csFunction.CreateCompressXml(context, resultXml, "dailyleaguelevel_out", false);

            // İkinci çağrı: Sıkıştırma açık (true), istemciye gönderilecek asıl sonuç.
            return csFunction.CreateCompressXml(context, resultXml, "dailyleaguelevel", true);
        }
    }
}