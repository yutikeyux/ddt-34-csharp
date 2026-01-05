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
    // Token: 0x02000035 RID: 53
    // FarmGetUserFieldInfos sınıfı, oyuncu çiftlik (Farm) alanlarının zaman hesaplamalarını yapan bir HTTP Handler'dır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class FarmGetUserFieldInfos : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x060000F8 RID: 248 RVA: 0x00008B60 File Offset: 0x00006D60
        // Ekim zamanı ile şimdiki zaman arasındaki farkı (dakika cinsinden) hesaplayan metod.
        // Bu fark, tarlanın ne kadar büyümüş olduğunu veya ne kadar zaman aldığını gösterir.
        public static int AccelerateTimeFields(DateTime plantTime, int fieldValidDate)
        {
            DateTime now = DateTime.Now;

            // Saat farkı
            int hourDiff = now.Hour - plantTime.Hour;

            // Dakika farkı
            int minuteDiff = now.Minute - plantTime.Minute;

            // NEGATİF SAAT FARKI DÜZELTME (Gece yarısı veya gün farkı)
            // Eğer şimdiki saat daha küçükse (örn: dün 23, bugün 01), 24 saat ekle.
            if (hourDiff < 0)
            {
                hourDiff = 24 + hourDiff;
            }

            // NEGATİF DAKİKA FARKI DÜZELTME (Önceki saate göre dakika dönmüşse)
            if (minuteDiff < 0)
            {
                minuteDiff = 60 + minuteDiff;
            }

            // Toplam dakika farkı
            int totalElapsedMinutes = (hourDiff * 60) + minuteDiff;

            // SINIRLAMA (CLAMP)
            // Eğer geçen süre, alanın geçerlilik süresinden (Maksimum büyüme süresi) uzunsa,
            // farkı o süreye eşitler.
            bool isOverLimit = totalElapsedMinutes > fieldValidDate;
            if (isOverLimit)
            {
                totalElapsedMinutes = fieldValidDate;
            }

            return totalElapsedMinutes;
        }

        // Token: 0x060000F9 RID: 249 RVA: 0x00008BCC File Offset: 0x00006DCC
        // Ara metod: Tarla bilgisini alıp süre hesaplamasını yapar.
        public static int AccelerateTimeFields(UserFieldInfo fieldInfo)
        {
            int elapsedMinutes = 0;

            // Tarla bilgisi varsa ve ekim ID'si 0'dan büyükse (ekilmiş demektir)
            bool isValidField = fieldInfo != null && fieldInfo.SeedID > 0;

            if (isValidField)
            {
                // Ekim zamanı ve geçerlilik süresi ile hesabı yap
                elapsedMinutes = FarmGetUserFieldInfos.AccelerateTimeFields(fieldInfo.PlantTime, fieldInfo.FieldValidDate);
            }

            return elapsedMinutes;
        }

        // Token: 0x060000FA RID: 250 RVA: 0x00008C08 File Offset: 0x00006E08
        // Gelen isteği karşılayan ve arkadaşların tarlalarını listeyip sürelerini hesaplayan metod
        public void ProcessRequest(HttpContext context)
        {
            // Kullanıcı ID'sini al
            int userId = Convert.ToInt32(context.Request["selfid"]);

            // Not: Gelen key parametresi kullanılmamış.
            string key = context.Request["key"];

            bool isSuccess = true;
            string message = "Başarılı!";

            // Kök XML elementini oluştur
            XElement resultXml = new XElement("Result");

            try
            {
                using (PlayerBussiness pb = new PlayerBussiness())
                {
                    // Kullanıcının arkadaş listesini al
                    foreach (FriendInfo friend in pb.GetFriendsAll(userId))
                    {
                        // Her arkadaş için bir kök XML düğümü oluşturur
                        XElement friendNode = new XElement("Item");

                        // O arkadaşın tüm tarlalarını (Fields) al
                        foreach (UserFieldInfo field in pb.GetSingleFields(friend.FriendID))
                        {
                            // Tarlanın hızlanma/büyüme süresini hesapla
                            int accelerateDate = FarmGetUserFieldInfos.AccelerateTimeFields(field);

                            // Tarlanın bilgisini (Item) XML'e ekle
                            XElement fieldItemNode = new XElement("Item", new object[]
                            {
                                new XAttribute("SeedID", field.SeedID),
                                // Hesaplanan geçen süreyi ekle
                                new XAttribute("AccelerateDate", accelerateDate),
                                // Ekim zamanını formatlı olarak ekle
                                new XAttribute("GrowTime", field.PlantTime.ToString("yyyy-MM-ddTHH:mm:ss"))
                            });
                            friendNode.Add(fieldItemNode);
                        }

                        // Arkadaşın ID'sini düğüme ekle
                        friendNode.Add(new XAttribute("UserID", friend.FriendID));

                        // Hazırlanan arkadaş düğümünü ana listeye ekle
                        resultXml.Add(friendNode);
                    }
                }
            }
            catch (Exception ex)
            {
                FarmGetUserFieldInfos.log.Error("FarmGetUserFieldInfos yüklenirken hata:", ex);
                isSuccess = false; // Hata varsa flag false olur
            }

            // XML'e genel durum bilgilerini (value ve message) ekle
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            context.Response.ContentType = "text/plain";
            context.Response.Write(resultXml.ToString(false));
        }

        // Token: 0x17000036 RID: 54
        // (get) Token: 0x060000FB RID: 251 RVA: 0x00003828 File Offset: 0x00001A28
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