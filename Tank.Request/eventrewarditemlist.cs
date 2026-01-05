using System;
using System.Collections.Generic;
using System.Web;
using System.Xml.Linq;
using Bussiness; // İş katmanı kütüphanesi
using SqlDataProvider.Data; // Veritabanı veri yapıları

namespace Tank.Request
{
    // Token: 0x02000032 RID: 50
    // eventrewarditemlist sınıfı, oyunun etkinlik ödüllerini listelemek için kullanılan bir HTTP Handler'dır.
    public class eventrewarditemlist : IHttpHandler
    {
        // Token: 0x060000EA RID: 234 RVA: 0x000083AC File Offset: 0x000065AC
        // Gelen isteği karşılayan metod
        public void ProcessRequest(HttpContext context)
        {
            // İstek yapanın IP adresi yetkili mi kontrol et
            if (csFunction.ValidAdminIP(context.Request.UserHostAddress))
            {
                // Yetkiliyse veriyi çekip yanıt olarak gönder
                context.Response.Write(eventrewarditemlist.Build(context));
            }
            else
            {
                // Yetkili değilse espirili hata mesajı döndür
                context.Response.Write("Tabi Efendim!");
            }
        }

        // Token: 0x060000EB RID: 235 RVA: 0x000083F8 File Offset: 0x000065F8
        // Veritabanından etkinlik ödüllerini çekip, gruplayıp XML formatına çeviren metod
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
                    // --- 1. VERİLERİ ÇEKME ---
                    // Etkinlik ödül tanımlarını çek
                    EventRewardInfo[] allEventRewards = db.GetAllEventRewardInfo();
                    // Eşyaları (Items) çek
                    EventRewardGoodsInfo[] allRewardGoods = db.GetAllEventRewardGoods();

                    // --- 2. GRUPLANDIRMA (DICTIONARY OLUŞTURMA) ---
                    // Yapı: Dictionary<ActivityID, Dictionary<SubActivityID, EventRewardInfo>>
                    // Orijinal kodda bu değişken adı 'EventRewardInfo' (Sınıf ismiyle çakışıyor), biz 'rewardDictionary' yaptık.
                    Dictionary<int, Dictionary<int, EventRewardInfo>> rewardDictionary = new Dictionary<int, Dictionary<int, EventRewardInfo>>();

                    foreach (EventRewardInfo reward in allEventRewards)
                    {
                        // Her ödülün içinde eşya listesini tutacak liste oluştur
                        reward.AwardLists = new List<EventRewardGoodsInfo>();

                        // ActivityType var mı bak, yoksa ekle
                        if (!rewardDictionary.ContainsKey(reward.ActivityType))
                        {
                            Dictionary<int, EventRewardInfo> innerDict = new Dictionary<int, EventRewardInfo>();
                            innerDict.Add(reward.SubActivityType, reward);
                            rewardDictionary.Add(reward.ActivityType, innerDict);
                        }
                        else
                        {
                            // ActivityType varsa, SubActivityType var mı bak, yoksa ekle
                            if (!rewardDictionary[reward.ActivityType].ContainsKey(reward.SubActivityType))
                            {
                                rewardDictionary[reward.ActivityType].Add(reward.SubActivityType, reward);
                            }
                        }
                    }

                    // --- 3. EŞYALARI GRUPLARA EKLEME ---
                    foreach (EventRewardGoodsInfo rewardGood in allRewardGoods)
                    {
                        // İlgili grup (Activity -> SubActivity) varsa eşyayı listeye ekle
                        if (rewardDictionary.ContainsKey(rewardGood.ActivityType) && rewardDictionary[rewardGood.ActivityType].ContainsKey(rewardGood.SubActivityType))
                        {
                            rewardDictionary[rewardGood.ActivityType][rewardGood.SubActivityType].AwardLists.Add(rewardGood);
                        }
                    }

                    // --- 4. XML OLUŞTURMA ---
                    XElement activityTypeNode = null; // Geçerli ActivityType Node'u

                    // Hiyerarşiyi geziyoruz: Activity -> SubActivity -> Items
                    foreach (Dictionary<int, EventRewardInfo> outerDict in rewardDictionary.Values)
                    {
                        foreach (EventRewardInfo eventReward in outerDict.Values)
                        {
                            // Eğer yeni bir ActivityType başlıyorsa ana node'u oluştur
                            if (activityTypeNode == null)
                            {
                                activityTypeNode = new XElement("ActivityType", new XAttribute("value", eventReward.ActivityType));
                            }

                            // Alt başlık (Items) node'unu oluştur. SubActivityType ve Condition özellikleri burada.
                            XElement itemsNode = new XElement("Items", new object[]
                            {
                                new XAttribute("SubActivityType", eventReward.SubActivityType),
                                new XAttribute("Condition", eventReward.Condition)
                            });

                            // Bu başlık altındaki her eşyayı (Item) döngüye al
                            foreach (EventRewardGoodsInfo awardItem in eventReward.AwardLists)
                            {
                                // Eşyanın özellikleriyle birlikte XML'e ekle
                                XElement itemNode = new XElement("Item", new object[]
                                {
                                    new XAttribute("TemplateId", awardItem.TemplateId),
                                    new XAttribute("StrengthLevel", awardItem.StrengthLevel),
                                    new XAttribute("AttackCompose", awardItem.AttackCompose),
                                    new XAttribute("DefendCompose", awardItem.DefendCompose),
                                    new XAttribute("LuckCompose", awardItem.LuckCompose),
                                    new XAttribute("AgilityCompose", awardItem.AgilityCompose),
                                    new XAttribute("IsBind", awardItem.IsBind),
                                    new XAttribute("ValidDate", awardItem.ValidDate),
                                    new XAttribute("Count", awardItem.Count)
                                });
                                itemsNode.Add(itemNode);
                            }

                            // Items node'unu ActivityType node'una ekle
                            activityTypeNode.Add(itemsNode);
                        }

                        // Hazırlanan ActivityType node'unu ana XML'e ekle
                        resultXml.Add(activityTypeNode);
                        activityTypeNode = null; // Sonraki aktivite için sıfırla
                    }
                    isSuccess = true;
                    message = "Başarılı!";
                }
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla (Orijinal kodda boş catch bloğu vardı)
                // Hata loglama satırı orijinalde yoktu, ancak iyi bir uygulama olmalıdır.
                // Bu yüzden burayı boş bırakıyoruz, 'isSuccess' false kalacak.
            }

            // XML'e genel durum bilgilerini (value ve message) ekle
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // --- NOT: Orijinal kodda burası iki kez çağrılıyor ---
            // İlk çağrı: Sıkıştırma kapalı (false), muhtemelen sunucu taraflı loglama veya dosyaya yazma için.
            csFunction.CreateCompressXml(context, resultXml, "eventrewarditemlist_out", false);

            // İkinci çağrı: Sıkıştırma açık (true), istemciye gönderilecek asıl sonuç.
            return csFunction.CreateCompressXml(context, resultXml, "eventrewarditemlist", true);
        }

        // Token: 0x17000032 RID: 50
        // (get) Token: 0x060000EC RID: 236 RVA: 0x00003828 File Offset: 0x00001A28
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