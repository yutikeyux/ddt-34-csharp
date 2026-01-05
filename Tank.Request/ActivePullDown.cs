using System;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml.Linq;
using Bussiness; // İş katmanı kütüphanesi
using Bussiness.CenterService; // Merkez servisi (WCF) kütüphanesi
using log4net; // Loglama kütüphanesi
using Road.Flash; // Flash yardımcı kütüphanesi

namespace Tank.Request
{
    // Token: 0x02000009 RID: 9
    // ActivePullDown sınıfı, oyuncunun etkinlik ödülünü çekmesini (claim) sağlayan bir HTTP Handler'dır.
    public class ActivePullDown : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x17000006 RID: 6
        // (get) Token: 0x06000020 RID: 32 RVA: 0x0000215A File Offset: 0x0000035A
        // IHttpHandler arayüzünün zorunlu üyesi.
        // False döndürmek, bu sınıfın bir pool (havuz) içinde tekrar kullanılmayacağını belirtir.
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        // Token: 0x06000021 RID: 33 RVA: 0x00003C1C File Offset: 0x00001E1C
        // Gelen isteği karşılayan ve ödülü işleyen metod
        public void ProcessRequest(HttpContext context)
        {
            // --- 1. DİL YÖNETİMİ AYARLARI ---
            // Not: Her istekte yol taraması ve Setup çağrısı yapmak performans açısından çok verimli değildir.
            // Ancak orijinal mantık korunmuştur.
            string serverPath = HttpContext.Current.Server.MapPath(".");
            serverPath += "\\";
            LanguageMgr.Setup(serverPath);

            // --- 2. PARAMETRELERİ ALMA ---
            // İstekten kullanıcı ID ve Etkinlik ID'sini al
            int userId = Convert.ToInt32(context.Request["selfid"]);
            int activeID = Convert.ToInt32(context.Request["activeID"]);

            string key = context.Request["key"]; // Kullanılmıyor ancak parametre olarak bekleniyor olabilir
            string activeKey = context.Request["activeKey"]; // Şifreli ödül ID'si

            bool isSuccess = false;
            // Varsayılan hata mesajı key'i
            string message = "ActivePullDownHandler.Fail";
            string awardID = ""; // Çözülecek olan Ödül ID'si

            XElement resultXml = new XElement("Result");

            // --- 3. ŞİFRE ÇÖZME (RSA DECRYPTION) ---
            // İstemciden gelen 'activeKey' boş değilse çözme işlemi yapılır
            if (!string.IsNullOrEmpty(activeKey))
            {
                // RSA 2 ile şifre çözme işlemi
                byte[] decryptedBytes = CryptoHelper.RsaDecryt2(StaticFunction.RsaCryptor, activeKey);

                // Çözülen byte dizisini string'e çevir (UTF-8)
                awardID = Encoding.UTF8.GetString(decryptedBytes, 0, decryptedBytes.Length);
            }

            try
            {
                // --- 4. İŞ KATMANI İŞLEMLERİ ---
                using (PlayerBussiness pb = new PlayerBussiness())
                {
                    // Veritabanında 'PullDown' işlemi çalıştırılır.
                    // Metot 0 dönerse işlem başarılıdır (Ref ile 'message' değişkenini günceller).
                    bool dbSuccess = (pb.PullDown(activeID, awardID, userId, ref message) == 0);

                    if (dbSuccess)
                    {
                        // İşlem başarılıysa kullanıcıya oyun içi posta gönderilir (CenterService kullanılır).
                        // Bu, oyuncunun ödülü alacağını bilmesini sağlar.
                        using (CenterServiceClient serviceClient = new CenterServiceClient())
                        {
                            serviceClient.MailNotice(userId);
                        }
                    }
                }

                // İşlem başarılı olarak işaretlenir
                isSuccess = true;

                // Veritabanından veya sistemden gelen 'message' anahtar key'ini (örn: "Success") dil dosyasından çevirir.
                message = LanguageMgr.GetTranslation(message, Array.Empty<object>());
            }
            catch (Exception ex)
            {
                // Herhangi bir hata olursa logla
                ActivePullDown.log.Error("ActivePullDown işlem hatası:", ex);
            }

            // --- 5. YANITI HAZIRLAMA ---
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // Yanıt içeriği türü ve çıktı
            context.Response.ContentType = "text/plain";
            context.Response.Write(resultXml.ToString(false));
        }
    }
}