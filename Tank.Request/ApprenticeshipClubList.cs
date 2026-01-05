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
    // Token: 0x0200000D RID: 13
    // ApprenticeshipClubList sınıfı, çıraklık kulübü oyuncularını listelemek için kullanılan bir HTTP Handler'dır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ApprenticeshipClubList : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x1700000A RID: 10
        // (get) Token: 0x06000031 RID: 49 RVA: 0x0000215A File Offset: 0x0000035A
        // IHttpHandler arayüzünün zorunlu üyesi.
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06000032 RID: 50 RVA: 0x00004124 File Offset: 0x00002324
        // Gelen isteği karşılayan ve listeyi oluşturan metod
        public void ProcessRequest(HttpContext context)
        {
            // --- DEĞİŞKEN TANIMLAMALARI ---
            bool isSuccess = false;
            string message = "true!"; // Varsayılan hata mesajı
            bool isPlayerRegistered = false; // XML'de kullanılacak ama mantıkta değeri değişmeyen bayrak
            bool isSelfPublishEquip = false; // XML'de kullanılacak ama mantıkta değeri değişmeyen bayrak

            // Veritabanı sorgusunda kullanılacak filtreleme ve sayfalama değişkenleri
            int queryType = 0;
            int queryFilter = 0;
            int searchUserID = -1;

            int totalCount = 0; // Toplam kayıt sayısı
            XElement resultXml = new XElement("Result");

            try
            {
                // --- 1. PARAMETRELERİ ALMA ---
                int page = int.Parse(context.Request["page"]); // Hangi sayfa isteniyor

                // Aşağıdaki parametreler orijinal kodda parse edilmiş ancak kullanılmamış.
                // Muhtemelen sadece parametre doğrulama (boş mu/dolu mu) amacıyla parse edilmiş olabilir.
                int.Parse(context.Request["selfid"]); // Kendi ID'si (Kullanılmıyor)
                bool.Parse(context.Request["isReturnSelf"]); // Kendine dönmek mi (Kullanılmıyor)

                // Aranan kullanıcı ismi (Ad filtresi)
                string searchName = (context.Request["name"] == null) ? "" : context.Request["name"];

                // Filtreleme bayrakları
                bool appshipStateType = bool.Parse(context.Request["appshipStateType"]);
                bool requestType = bool.Parse(context.Request["requestType"]);

                // --- 2. SORGU PARAMETRELERİNİ HESAPLAMA ---
                // Sayfa büyüklüğü: requestType true ise 9, değilse 3
                int pageSize = requestType ? 9 : 3;

                // Başlangıç filtre değerleri (requestType true varsayımı ile)
                queryType = 10;
                queryFilter = 2;

                if (!appshipStateType)
                {
                    queryType = 8;
                    queryFilter = 1;
                }

                // requestType false ise mantık değişiyor
                if (!requestType)
                {
                    if (!appshipStateType)
                    {
                        // !requestType && !appshipStateType durumu
                        queryFilter = 3;
                        queryType = 9;
                    }
                    else
                    {
                        // !requestType && appshipStateType durumu
                        queryFilter = 4;
                        queryType = 9;
                    }
                }

                // --- 3. VERİTABANI İŞLEMLERİ ---
                using (PlayerBussiness pb = new PlayerBussiness())
                {
                    // Eğer arama yapılacaksa (isim verildi), önce ID'sini bul
                    if (!string.IsNullOrEmpty(searchName))
                    {
                        PlayerInfo user = pb.GetUserSingleByNickName(searchName);
                        searchUserID = (user != null) ? user.ID : 0;
                    }

                    // Oyuncuları sayfalama (Paging) mantığı ile çekiyoruz
                    // Not: isSuccess değişkeni orijinal kodda 'ref' olarak buraya gönderiliyor.
                    // Ancak hemen ardından 'true' atanıyor, dolayısıyla fonksiyonun ne döndürdüğü
                    // orijinal kodda görmezden gelinmiş. Bu yüzden yine 'ref isSuccess' gönderiyoruz.
                    PlayerInfo[] playerList = pb.GetPlayerPage(page, pageSize, ref totalCount, queryType, queryFilter, searchUserID, ref isSuccess);

                    // Çekilen her oyuncuyu XML'e dönüştürüp listeye ekle
                    for (int i = 0; i < playerList.Length; i++)
                    {
                        XElement playerNode = FlashUtils.CreateApprenticeShipInfo(playerList[i]);
                        resultXml.Add(playerNode);
                    }

                    isSuccess = true;
                    message = "Başarılı!";
                }
            }
            catch (Exception ex)
            {
                // Hata logla
                ApprenticeshipClubList.log.Error(ex);
            }

            // --- 4. YANITI HAZIRLAMA ---
            resultXml.Add(new XAttribute("total", totalCount));
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // Not: Orijinal kodda bu iki değişken asla true yapılmıyor.
            // Flash istemcisi bu attribute'ları bekliyor olabilir bu yüzden ekliyoruz.
            resultXml.Add(new XAttribute("isPlayerRegeisted", isPlayerRegistered)); // Yazım hatası 'Regeisted' korundu
            resultXml.Add(new XAttribute("isSelfPublishEquip", isSelfPublishEquip));

            context.Response.ContentType = "text/plain";
            context.Response.Write(resultXml.ToString(false));
        }
    }
}