using System;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml.Linq;
using Bussiness; // İş katmanı kütüphanesi
using log4net; // Loglama kütüphanesi

namespace Tank.Request
{
    // Token: 0x02000024 RID: 36
    // ConsortiaNameCheck sınıfı, yeni kurulacak lonca için adın uygunluğunu kontrol eden bir HTTP Handler'dır.
    public class ConsortiaNameCheck : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x06000092 RID: 146 RVA: 0x00006714 File Offset: 0x00004914
        // Gelen isteği karşılayan ve isim kontrolünü yapan metod
        public void ProcessRequest(HttpContext context)
        {
            // --- 1. DİL YÖNETİMİ AYARLARI ---
            // Her istekte dil dosyalarının bulunduğu yolu ayarlıyor.
            // Not: Performans açısından her Page_Load'ta bunun yapılması maliyetlidir.
            string path = HttpContext.Current.Server.MapPath(".");
            path += "\\";
            LanguageMgr.Setup(path);

            bool isValid = false;
            // Varsayılan hata mesajı (Eğer herhangi bir kontrol başarısız olursa veya hata alırsa bu döner)
            string message = LanguageMgr.GetTranslation("Tank.Request.ConsortiaCheck.Exist", Array.Empty<object>());

            // Kök XML elementini oluştur
            XElement resultXml = new XElement("Result");

            try
            {
                // --- 2. İSİM ALIMI VE TEMİZLİK ---
                // İstemciden gelen ismi al, URL Decode et ve SQL Injection koruması uygula
                string consortiaName = csFunction.ConvertSql(HttpUtility.UrlDecode(context.Request["NickName"]));

                // --- 3. UZUNLUK KONTROLÜ ---
                // Encoding.Default.GetByteCount, ismin karakter sayısını değil, byte uzunluğunu hesaplar.
                // Bu muhtemelen veritabanındaki alanın sınırını kontrol etmek içindir (Max 14 Byte).
                bool isValidLength = Encoding.Default.GetByteCount(consortiaName) <= 14;

                if (isValidLength)
                {
                    // --- 4. BOŞ KARAKTER KONTROLÜ ---
                    bool isNotEmpty = !string.IsNullOrEmpty(consortiaName);

                    if (isNotEmpty)
                    {
                        // --- 5. VERİTABANI KONTROLÜ ---
                        using (ConsortiaBussiness db = new ConsortiaBussiness())
                        {
                            // İsim veritabanında mevcut mu? (null ise kullanılabilir demektir)
                            bool isAvailable = (db.GetConsortiaSingleByName(consortiaName) == null);

                            if (isAvailable)
                            {
                                // Tüm kontrollerden geçti, isim uygundur.
                                isValid = true;
                                // Başarı mesajı (Örn: "İsim uygun")
                                message = LanguageMgr.GetTranslation("Tank.Request.ConsortiaCheck.Right", Array.Empty<object>());
                            }
                            // Else: İsim zaten kullanımda, isValid false kalır ve message varsayılan "Exist" mesajı olur.
                        }
                    }
                }
                else
                {
                    // İsim çok uzun (14 byte'ı aşıyor)
                    message = LanguageMgr.GetTranslation("Tank.Request.ConsortiaCheck.Long", Array.Empty<object>());
                }
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                ConsortiaNameCheck.log.Error("ConsortiaNameCheck işleminde hata:", ex);
                isValid = false;
            }

            // --- 6. YANITI HAZIRLAMA ---
            resultXml.Add(new XAttribute("value", isValid));
            resultXml.Add(new XAttribute("message", message));

            context.Response.ContentType = "text/plain";
            context.Response.Write(resultXml.ToString(false));
        }

        // Token: 0x17000020 RID: 32
        // (get) Token: 0x06000093 RID: 147 RVA: 0x00003828 File Offset: 0x00001A28
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