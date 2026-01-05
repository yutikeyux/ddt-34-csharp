using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness; // İş katmanı kütüphanesi
using log4net; // Loglama kütüphanesi
using SqlDataProvider.Data; // Veritabanı veri yapıları

namespace Tank.Request
{
    // Token: 0x0200003B RID: 59
    // giftsendlog sınıfı, kullanıcının hediye gönderme kayıtlarını listelemek için kullanılan bir HTTP Handler'dır.
    // Not: Sınıf adı tamamen küçük harflerle yazılmış.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class giftsendlog : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x1700003D RID: 61
        // (get) Token: 0x06000113 RID: 275 RVA: 0x0000215A File Offset: 0x0000035A
        // IHttpHandler arayüzünün zorunlu üyesi.
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06000114 RID: 276 RVA: 0x00009548 File Offset: 0x00007748
        // Gelen isteği karşılayan ve gönderim loglarını hazırlayan metod
        public void ProcessRequest(HttpContext context)
        {
            bool isSuccess = false;
            string message = "Fail!";

            // Kök XML elementini oluştur
            XElement resultXml = new XElement("Result");

            try
            {
                // --- 1. PARAMETRELERİ AL ---
                // NOT: 'selfid' ve 'key' parametreleri parse edilmiş ancak kodun ilerleyen kısımlarında KULLANILMIYOR.
                // Bu muhtemelen önceki bir versiyondan kopyala-yapıştır sonrası temizlenmemiş kod parçalarıdır.
                int unusedSelfID = int.Parse(context.Request["selfid"]);
                string unusedKey = context.Request["key"];

                // Burada kullanılan gerçek parametre: Kullanıcı ID'si
                int userID = int.Parse(context.Request["userID"]);

                // --- 2. VERİTABANI İŞLEMLERİ ---
                using (PlayerBussiness pb = new PlayerBussiness())
                {
                    // Önceki dosyadan (GiftRecieveLog) temel fark buradadır:
                    // Önceki dosya 'true' (GetAllUserGifts(userId, true)) gönderiyordu -> Gelen Hediyeler
                    // Bu dosya 'false' (GetAllUserGifts(userID, false)) gönderiyor -> Giden Hediyeler
                    UserGiftInfo[] giftList = pb.GetAllUserGifts(userID, false);

                    // Liste null değilse döngüye al
                    if (giftList != null)
                    {
                        foreach (UserGiftInfo gift in giftList)
                        {
                            // Her hediye gönderimini XML formatına çevirip sonuç listesine ekle
                            XElement giftNode = new XElement("Item", new object[]
                            {
                                new XAttribute("playerID", gift.ReceiverID), // Hediye alan kişinin ID'si
                                new XAttribute("TemplateID", gift.TemplateID), // Eşya ID'si
                                new XAttribute("count", gift.Count) // Adet
                            });
                            resultXml.Add(giftNode);
                        }
                    }
                }

                isSuccess = true;
                message = "Success!";
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                giftsendlog.log.Error("giftsendlog yüklenirken hata:", ex);
            }

            // XML'e genel durum bilgilerini (value ve message) ekle
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // Yanıtı sıkıştırıp (Compress) Binary olarak ekrana yaz
            context.Response.ContentType = "text/plain";
            context.Response.BinaryWrite(StaticFunction.Compress(resultXml.ToString(false)));
        }
    }
}