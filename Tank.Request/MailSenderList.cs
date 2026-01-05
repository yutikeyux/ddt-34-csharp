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
    // Token: 0x02000058 RID: 88
    // MailSenderList sınıfı, kullanıcının gönderdiği mailleri listelemek için kullanılan bir HTTP Handler'dır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class MailSenderList : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x0600018D RID: 397 RVA: 0x0000CD70 File Offset: 0x0000AF70
        // Gelen isteği karşılayan metod
        public void ProcessRequest(HttpContext context)
        {
            // İstek yapanın IP adresi yetkili mi kontrol et
            if (csFunction.ValidAdminIP(context.Request.UserHostAddress))
            {
                // Yetkiliyse veriyi çekip yanıt olarak gönder
                context.Response.Write(MailSenderList.Build(context));
            }
            else
            {
                // Yetkili değilse espirili hata mesajı döndür
                context.Response.Write("Tabi Efendim!");
            }
        }

        // Token: 0x0600018E RID: 398 RVA: 0x0000CD70 File Offset: 0x0000AF70
        // Veritabanından gönderilen mailleri çekip XML formatına çeviren metod
        public static string Build(HttpContext context)
        {
            bool isSuccess = false;
            string message = "Hata!";

            // Kök XML elementini oluştur
            XElement resultXml = new XElement("Result");

            try
            {
                // --- 1. PARAMETRELERİ AL ---
                // Giden maillerin sahibinin ID'sini al (QueryString'den)
                int senderID = int.Parse(context.Request.QueryString["selfID"]);

                // ID 0 değilse işlem yap
                if (senderID != 0)
                {
                    // --- 2. VERİTABANI İŞLEMLERİ ---
                    using (PlayerBussiness pb = new PlayerBussiness())
                    {
                        // Veritabanından bu kişi tarafından gönderilen mailleri çek
                        MailInfo[] mailList = pb.GetMailBySenderID(senderID);

                        // Her bir maili döngüye al
                        foreach (MailInfo mail in mailList)
                        {
                            // Mail bilgisini XML formatına çevirip sonuç listesine ekle
                            // FlashUtils.CreateMailInfo(mail, "Item") -> "Item" stringi, mailin içindeki eki (attachment) bilgilerinin XML'deki ana etiket adıdır.
                            resultXml.Add(FlashUtils.CreateMailInfo(mail, "Item"));
                        }
                    }

                    isSuccess = true;
                    message = "Başarılı!";
                }
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                MailSenderList.log.Error("MailSenderList yüklenirken hata:", ex);
            }

            // XML'e genel durum bilgilerini (value ve message) ekle
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // Sonucu sıkıştırıp (Compress) döndür
            return csFunction.CreateCompressXml(context, resultXml, "MailSenderList", true);
        }

        // Token: 0x17000058 RID: 88
        // (get) Token: 0x0600018F RID: 399 RVA: 0x00003828 File Offset: 0x00001A28
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