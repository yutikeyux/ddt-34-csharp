using System;
using System.Collections;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness; // İş katmanı kütüphanesi
using log4net; // Loglama kütüphanesi
using SqlDataProvider.Data; // Veritabanı veri yapıları

namespace Tank.Request
{
    // Token: 0x02000041 RID: 65
    // IMFriendsGood sınıfı, bir kullanıcının "İyi Arkadaşlarını" listelemek için kullanılan bir HTTP Handler'dır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class IMFriendsGood : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x0600012B RID: 299 RVA: 0x00009F1C File Offset: 0x0000811C
        // Gelen isteği karşılayan metod
        public void ProcessRequest(HttpContext context)
        {
            bool isSuccess = false;
            string message = "Hata!";

            // Kök XML elementini oluştur
            XElement resultXml = new XElement("Result");

            try
            {
                // --- 1. PARAMETRELERİ AL ---
                string userName = context.Request["UserName"];

                // --- 2. VERİTABANI İŞLEMLERİ ---
                using (PlayerBussiness db = new PlayerBussiness())
                {
                    // Veritabanından "İyi Arkadaşlar" listesini (ArrayList) çek
                    ArrayList friends = db.GetFriendsGood(userName);

                    // --- 3. XML OLUŞTURMA ---
                    // Her bir arkadaşı döngüye al
                    for (int i = 0; i < friends.Count; i++)
                    {
                        // Arkadaşın kullanıcı adını al
                        object friendObj = friends[i];
                        string friendName = (friendObj != null) ? friendObj.ToString() : "";

                        // Arkadaş bilgisini XML formatına çevirip sonuç listesine ekle
                        XElement friendNode = new XElement("Item", new XAttribute("UserName", friendName));
                        resultXml.Add(friendNode);
                    }
                }

                isSuccess = true;
                message = "Başarılı!";
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                IMFriendsGood.log.Error("IMFriendsGood yüklenirken hata:", ex);
            }

            // XML'e genel durum bilgilerini (value ve message) ekle
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // --- 4. YANITI GÖNDER ---
            // Önceki dosyaların aksine burada sıkıştırma (Compress) yoktur. Düz XML gönderilir.
            context.Response.ContentType = "text/plain";
            context.Response.Write(resultXml.ToString(false));
        }

        // Token: 0x17000041 RID: 65
        // (get) Token: 0x0600012C RID: 300 RVA: 0x00003828 File Offset: 0x00001A28
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