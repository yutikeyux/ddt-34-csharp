using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Linq;
using Bussiness; // İş katmanı kütüphanesi
using log4net; // Loglama kütüphanesi
using Road.Flash; // Flash istemcisi yardımcı kütüphanesi
using SqlDataProvider.Data; // Veritabanı veri yapıları

namespace Tank.Request
{
    // Token: 0x02000006 RID: 6
    // achievementlist sınıfı, oyunun başarımlarını listelemek için kullanılan bir HTTP Handler'dır.
    public class achievementlist : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x06000013 RID: 19 RVA: 0x0000383C File Offset: 0x00001A3C
        // Gelen isteği karşılayan metod
        public void ProcessRequest(HttpContext context)
        {
            // İstek yapanın IP adresi yetkili mi kontrol et
            if (csFunction.ValidAdminIP(context.Request.UserHostAddress))
            {
                // Yetkiliyse başarımları çekip yanıt olarak gönder
                context.Response.Write(achievementlist.Build(context));
            }
            else
            {
                // Yetkili değilse hata mesajı döndür
                context.Response.Write("Tabi Efendim!");
            }
        }

        // Token: 0x06000014 RID: 20 RVA: 0x00003888 File Offset: 0x00001A88
        // Veritabanından başarımları (achievement), koşulları ve ödülleri toplayıp XML formatına çeviren metod.
        // Not: Orijinal kodda metot adı "Build" (Build) olarak hatalı yazılmış, uyumluluk için böyle bırakıldı.
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
                    // Tüm başarımları, ödülleri ve koşulları veritabanından çek
                    AchievementInfo[] allAchievements = db.GetALlAchievement();
                    AchievementRewardInfo[] allRewards = db.GetALlAchievementReward();
                    AchievementConditionInfo[] allConditions = db.GetALlAchievementCondition();

                    // Her bir başarımla (Achievement) için döngü kur
                    foreach (var achievement in allAchievements)
                    {
                        // Mevcut başarım için ana XML elementini oluştur (Flash istemcisi için özel format)
                        XElement achievementElement = FlashUtils.CreateAchievement(achievement);

                        // Bu başarımın ID'sine uyan koşulları (Condition) filtrele
                        var matchingConditions = allConditions.Where(cond => cond.AchievementID == achievement.ID);
                        foreach (var condition in matchingConditions)
                        {
                            // Koşul elementini başarımın altına ekle
                            achievementElement.Add(FlashUtils.CreateAchievementCondition(condition));
                        }

                        // Bu başarımın ID'sine uyan ödülleri (Reward) filtrele
                        var matchingRewards = allRewards.Where(reward => reward.AchievementID == achievement.ID);
                        foreach (var reward in matchingRewards)
                        {
                            // Ödül elementini başarımın altına ekle
                            achievementElement.Add(FlashUtils.CreateAchievementReward(reward));
                        }

                        // Hazırlanan başarım elementini sonuç XML'ine ekle
                        resultXml.Add(achievementElement);
                    }

                    isSuccess = true;
                    message = "Başarılı!";
                }
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                achievementlist.log.Error("AchievementList oluşturma hatası:", ex);
            }

            // XML'e genel durum bilgilerini (value ve message) ekle
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // --- NOT: Orijinal kodda burası iki kez çağrılıyor ---
            // İlk çağrı muhtemelen loglama veya debug için 'false' (sıkıştırma kapalı) parametresi ile yapılıyor.
            csFunction.CreateCompressXml(context, resultXml, "achievementlist_out", false);

            // Geri dönüş değeri olarak, sıkıştırma açık ('true') olan sonucu döndür.
            return csFunction.CreateCompressXml(context, resultXml, "achievementlist", true);
        }

        // Token: 0x17000004 RID: 4
        // (get) Token: 0x06000015 RID: 21 RVA: 0x00003828 File Offset: 0x00001A28
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