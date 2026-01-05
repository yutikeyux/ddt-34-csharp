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
    // Token: 0x0200000E RID: 14
    // AuctionPageList sınıfı, Aksiyon (Ihale/Etiket) sayfasındaki ürünleri listeler.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class AuctionPageList : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x06000035 RID: 53 RVA: 0x000043DC File Offset: 0x000025DC
        // Gelen isteği karşılayan ve aksiyon listesini hazırlayan metod
        public void ProcessRequest(HttpContext context)
        {
            bool isSuccess = false;
            string message = "Hata!";
            int totalCount = 0;

            // Kök XML elementini oluştur
            XElement resultXml = new XElement("Result");

            try
            {
                // --- 1. PARAMETRELERİ AL VE DOĞRULA ---
                int page = int.Parse(context.Request["page"]);

                // Arama yapılacak isim (SQL Injection koruması ve Decode işlemi uygulanıyor)
                string searchName = csFunction.ConvertSql(HttpUtility.UrlDecode(context.Request["name"]));

                int type = int.Parse(context.Request["type"]); // Filtreleme tipi
                int pay = int.Parse(context.Request["pay"]);   // Ödeme tipi (Altın/Token vb.)
                int userID = int.Parse(context.Request["userID"]); // Satıcı ID
                int buyID = int.Parse(context.Request["buyID"]);   // Alıcı ID
                int order = int.Parse(context.Request["order"]);   // Sıralama kriteri
                bool sort = bool.Parse(context.Request["sort"]);   // Sıralama yönü (Artan/Azalan)

                // Özel ID listesi (Filtreleme için)
                string auctionIDs = csFunction.ConvertSql(HttpUtility.UrlDecode(context.Request["Auctions"]));
                auctionIDs = string.IsNullOrEmpty(auctionIDs) ? "0" : auctionIDs;

                // Sayfa boyutu sabitlenmiş (Her sayfada 50 ürün)
                int pageSize = 50;

                // --- 2. VERİTABANI İŞLEMLERİ ---
                using (PlayerBussiness db = new PlayerBussiness())
                {
                    // Veritabanından sayfalama mantığıyla aksiyon ürünlerini çek
                    AuctionInfo[] auctionList = db.GetAuctionPage(page, searchName, type, pay, ref totalCount, userID, buyID, order, sort, pageSize, auctionIDs);

                    // Çekilen her aksiyon için döngü kur
                    foreach (AuctionInfo auction in auctionList)
                    {
                        // Aksiyon bilgisini XML'e çevir
                        XElement auctionElement = FlashUtils.CreateAuctionInfo(auction);

                        // OPTİMİZASYON: Orijinal kodda burada 'using (PlayerBussiness pb = new PlayerBussiness())' yapılarak
                        // her ürün için yeni bir veritabanı bağlantısı açılıyordu. Bu büyük bir performans hatasıydı.
                        // Dışarıda zaten açık olan 'db' bağlantısını kullanarak aynı sorguyu daha verimli hale getirdik.
                        ItemInfo item = db.GetUserItemSingle(auction.ItemID);

                        if (item != null)
                        {
                            // Eğer ürün bilgisi varsa, aksiyonun içine ekle
                            auctionElement.Add(FlashUtils.CreateGoodsInfo(item));
                        }

                        // Hazırlanan XML elementini ana listeye ekle
                        resultXml.Add(auctionElement);
                    }

                    isSuccess = true;
                    message = "Başarılı!";
                }
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                AuctionPageList.log.Error("AuctionPageList Hatası:", ex);
            }

            // --- 3. YANITI HAZIRLAMA ---
            resultXml.Add(new XAttribute("total", totalCount));
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            context.Response.ContentType = "text/plain";
            context.Response.Write(resultXml.ToString(false));
        }

        // Token: 0x1700000B RID: 11
        // (get) Token: 0x06000036 RID: 54 RVA: 0x00003828 File Offset: 0x00001A28
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