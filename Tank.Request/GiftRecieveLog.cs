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
    // Token: 0x0200003A RID: 58
    // GiftRecieveLog sınıfı, kullanıcının hediye alım kayıtlarını listelemek için kullanılan bir HTTP Handler'dır.
    // Not: Sınıf isminde "Recieve" (Receive) yerine yazım hatası ("Recieve") geçiyor.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GiftRecieveLog : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x1700003C RID: 60
        // (get) Token: 0x0600010F RID: 271 RVA: 0x0000215A File Offset: 0x0000035A
        // IHttpHandler arayüzünün zorunlu üyesi.
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06000110 RID: 272 RVA: 0x00009358 File Offset: 0x00007558
        // Gelen isteği karşılayan ve hediye loglarını hazırlayan metod
        public void ProcessRequest(HttpContext context)
        {
            bool isSuccess = false;
            string message = "Fail!";

            // Kök XML elementini oluştur
            XElement resultXml = new XElement("Result");

            try
            {
                // --- 1. PARAMETRELERİ AL ---
                // NOT: 'selfid' ve 'key' parametreleri parse ediliyor ancak kodun ilerleyen kısımlarında KULLANILMIYOR.
                // Bu muhtemelen kopyala-yapıştır sonrası temizlenmeyen kod kalıntısıdır.
                int unusedSelfID = int.Parse(context.Request["selfid"]);
                string unusedKey = context.Request["key"];

                // Burada kullanılan gerçek parametre "userID" (Alıcı ID'si)
                int userID = int.Parse(context.Request["userID"]);

                // --- 2. VERİTABANI İŞLEMLERİ ---
                using (PlayerBussiness pb = new PlayerBussiness())
                {
                    // Belirtilen kullanıcının hediyelerini (GetAllUserGifts) çekiyoruz.
                    // İkinci parametre 'true' muhtemelen "Silinmişleri getir" anlamına gelir.
                    UserGiftInfo[] giftList = pb.GetAllUserGifts(userID, true);

                    // Liste null değilse döngüye gir
                    if (giftList != null)
                    {
                        foreach (UserGiftInfo gift in giftList)
                        {
                            // Her hediyeyi XML formatına çevirip sonuç listesine ekle
                            XElement giftNode = new XElement("Item", new object[]
                            {
                                new XAttribute("playerID", gift.ReceiverID), // Hediye alan kişi ID'si
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
                // NOT: Orijinal log mesajında "giftrecievelog" (Recieve yazım hatası) var.
                GiftRecieveLog.log.Error("giftrecievelog yüklenirken hata:", ex);
            }

            // XML'e genel durum bilgilerini (value ve message) ekle
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // --- 3. YANITI GÖNDER ---
            // Yanıtı sıkıştırıp (Compress) binary olarak ekrana yazar
            context.Response.ContentType = "text/plain";
            context.Response.BinaryWrite(StaticFunction.Compress(resultXml.ToString(false)));
        }
    }
}