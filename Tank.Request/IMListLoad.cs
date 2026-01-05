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
    // Token: 0x02000042 RID: 66
    // IMListLoad sınıfı, kullanıcının anlık mesaj arkadaşlarını (Friend List) listelemek için kullanılan bir HTTP Handler'dır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class IMListLoad : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x0600012F RID: 303 RVA: 0x0000A064 File Offset: 0x00008264
        // Gelen isteği karşılayan ve arkadaş listesini hazırlayan metod
        public void ProcessRequest(HttpContext context)
        {
            bool isSuccess = false;
            string message = "Fail!";

            // Kök XML elementini oluştur
            XElement resultXml = new XElement("Result");

            try
            {
                // --- 1. PARAMETRELERİ AL ---
                int userID = int.Parse(context.Request["id"]);

                // --- 2. VERİTABANI İŞLEMLERİ ---
                using (PlayerBussiness pb = new PlayerBussiness())
                {
                    // Tüm arkadaşları (GetFriendsAll) veritabanından çek
                    FriendInfo[] allFriends = pb.GetFriendsAll(userID);

                    // --- 3. XML OLUŞTURMA (CUSTOM LIST) ---
                    // Özel bir kategori başlığı oluşturur ("Arkadaşlar")
                    XElement customListNode = new XElement("customList", new object[]
                    {
                        new XAttribute("ID", 0),
                        new XAttribute("Name", "Arkadaşlar")
                    });
                    resultXml.Add(customListNode);

                    // Her bir arkadaşı döngüye al
                    foreach (FriendInfo friend in allFriends)
                    {
                        // Arkadaşın tüm bilgilerini XML formatına çevirip sonuç listesine ekle
                        XElement friendNode = new XElement("Item", new object[]
                        {
                            new XAttribute("ID", friend.FriendID),
                            new XAttribute("NickName", friend.NickName),
                            // DİKKAT: Doğum günü veritabanından alınmıyor, şimdiki zaman atanıyor.
                            new XAttribute("Birthday", DateTime.Now),
                            new XAttribute("ApprenticeshipState", 0),
                            new XAttribute("LoginName", friend.UserName),
                            new XAttribute("Style", friend.Style),
                            new XAttribute("Sex", friend.Sex == 1), // Cinsiyet (1 = Bay, 0 = Bayan)
                            new XAttribute("Colors", friend.Colors),
                            new XAttribute("Grade", friend.Grade),
                            new XAttribute("Hide", friend.Hide),
                            new XAttribute("ConsortiaName", friend.ConsortiaName),
                            new XAttribute("TotalCount", friend.Total),
                            new XAttribute("EscapeCount", friend.Escape),
                            new XAttribute("WinCount", friend.Win),
                            new XAttribute("Offer", friend.Offer),
                            new XAttribute("Relation", friend.Relation),
                            new XAttribute("Repute", friend.Repute),
                            new XAttribute("State", (friend.State == 1) ? 1 : 0), // Çevrimiçi Durumu (1=Online)
                            new XAttribute("Nimbus", friend.Nimbus),
                            new XAttribute("DutyName", friend.DutyName)
                        });
                        resultXml.Add(friendNode);
                    }
                }
                isSuccess = true;
                message = "Success!";
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                IMListLoad.log.Error("IMListLoad yüklenirken hata:", ex);
            }

            // XML'e genel durum bilgilerini (value ve message) ekle
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // Yanıtı ekrana yaz
            context.Response.ContentType = "text/plain";
            context.Response.Write(resultXml.ToString(false));
        }

        // Token: 0x17000042 RID: 66
        // (get) Token: 0x06000130 RID: 304 RVA: 0x00003828 File Offset: 0x00001A28
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