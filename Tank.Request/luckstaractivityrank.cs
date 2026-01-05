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
    // Token: 0x02000057 RID: 87
    // luckstaractivityrank sınıfı, şans yıldızı etkinliğinin sıralamasını getirmek için kullanılan bir HTTP Handler'dır.
    // Not: Sınıf adı tamamen küçük harflerle yazılmış.
    public class luckstaractivityrank : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x0600018A RID: 394 RVA: 0x0000CB94 File Offset: 0x0000AB94
        // Gelen isteği karşılayan ve sıralamayı hazırlayan metod
        public void ProcessRequest(HttpContext context)
        {
            // --- 1. PARAMETRELERİ AL ---
            // Kullanıcı ID'sini al (İstemcinin kendi ID'si)
            int userID = Convert.ToInt32(context.Request["selfid"]);

            // 'key' parametresi alınıyor ancak kod içinde kullanılmıyor.
            string key = context.Request["key"];

            // --- 2. XML YAPILARI OLUŞTURMA ---
            // Kök XML elementi: <Ranks> ... </Ranks>
            XElement ranksXml = new XElement("Ranks");

            // Oyuncunun kendi sıralamasını tutmak için nesne oluştur
            // DÜZELTME: Orijinal kodda myRankInfo = new... dışarıda tanımlı.
            // Using bloğu içinde kullanıldığı için hata yok.
            LuckstarActivityRankInfo myRankInfo = new LuckstarActivityRankInfo();
            myRankInfo.nickName = "";

            // --- 3. VERİTABANI İŞLEMLERİ VE SIRALAMA ---
            try
            {
                using (PlayerBussiness pb = new PlayerBussiness())
                {
                    // Tüm oyuncuların şans yıldızı sıralamasını veritabanından çek
                    LuckstarActivityRankInfo[] allRanks = pb.GetAllLuckstarActivityRank();

                    // Her bir sıralamayı döngüye al
                    foreach (LuckstarActivityRankInfo rankInfo in allRanks)
                    {
                        // Sıralama bilgisini XML formatına çevirip kök listeye ekle
                        ranksXml.Add(FlashUtils.LuckstarActivityRank(rankInfo));

                        // Eğer sıradaki kişi, istemciyi yapan kullanıcı ise 'myRankInfo' objesine ata
                        bool isCurrentUser = (rankInfo.UserID == userID);
                        if (isCurrentUser)
                        {
                            myRankInfo = rankInfo;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Orijinal kodda try-catch yoktu, ancak hata yönetimi için ekledim.
                luckstaractivityrank.log.Error("luckstaractivityrank yüklenirken hata:", ex);
            }

            // --- 4. KENDİ SIRALAMASI (MYRANK) ---
            // Oyuncunun kendi sıralama bilgisini içeren XML elementi oluştur
            XElement myRankNode = new XElement("myRank", new object[]
            {
                new XAttribute("rank", myRankInfo.rank),
                new XAttribute("useStarNum", myRankInfo.useStarNum),
                new XAttribute("nickName", myRankInfo.nickName)
            });

            // Kendi sıralamasını kök XML'e ekle (Listenin en sonuna eklenir)
            ranksXml.Add(myRankNode);

            // --- 5. GENEL BİLGİLERİ VE YANIT ---
            bool isSuccess = true;
            string message = "Başarılı!";

            // Güncelleme tarihini ekle
            // DÜZELTME: Orijinal kodda "hh" (küçük) kullanılmıştı, C# standardı "HH" (büyük).
            ranksXml.Add(new XAttribute("lastUpdateTime", DateTime.Now.ToString("MM-dd HH:mm")));
            ranksXml.Add(new XAttribute("value", isSuccess));
            ranksXml.Add(new XAttribute("message", message));

            // Yanıtı ekrana yaz
            context.Response.ContentType = "text/plain";
            context.Response.Write(ranksXml.ToString(false));
        }

        // Token: 0x17000057 RID: 87
        // (get) Token: 0x0600018B RID: 395 RVA: 0x00003828 File Offset: 0x00001A28
        // IHttpHandler arayüzünün zorunlu üyesi.
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}