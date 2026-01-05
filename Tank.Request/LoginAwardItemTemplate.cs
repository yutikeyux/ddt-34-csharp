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
    // Token: 0x02000053 RID: 83
    // LoginAwardItemTemplate sınıfı, oyuncuların kümülatif giriş ödüllerini (Accumulative Login Awards) listelemek için kullanılan bir HTTP Handler'dır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class LoginAwardItemTemplate : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x0600017B RID: 379 RVA: 0x0000C678 File Offset: 0x0000A878
        // Gelen isteği karşılayan metod
        public void ProcessRequest(HttpContext context)
        {
            // İstek yapanın IP adresi yetkili mi kontrol et
            if (csFunction.ValidAdminIP(context.Request.UserHostAddress))
            {
                // Yetkiliyse veriyi çekip yanıt olarak gönder
                context.Response.Write(LoginAwardItemTemplate.Build(context));
            }
            else
            {
                // Yetkili değilse espirili hata mesajı döndür
                context.Response.Write("Tabi Efendim!");
            }
        }

        // Token: 0x17000054 RID: 84
        // (get) Token: 0x0600017C RID: 380 RVA: 0x00003828 File Offset: 0x00001A28
        // IHttpHandler arayüzünün zorunlu üyesi.
        // False döndürmek, bu sınıfın bir pool (havuz) içinde tekrar kullanılmayacağını belirtir.
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        // Token: 0x0600017D RID: 381 RVA: 0x0000C6C4 File Offset: 0x0000A8C4
        // Veritabanından ödülleri çekip XML formatına çeviren metod
        public static string Build(HttpContext context)
        {
            bool isSuccess = false;
            string message = "Hata!";

            // Kök XML elementini oluştur
            XElement resultXml = new XElement("Result");

            try
            {
                // Veritabanı işlemleri için bağlantı oluştur
                // DÜZELTME: Orijinal kodda 'ProduceBussiness' (u harfi) kullanılmıştı.
                // Projenin diğer dosyalarındaki (LoadPVEItems, LoadItemsCategory) isimlendirmesine göre düzeltildi.
                using (ProduceBussiness pb = new ProduceBussiness())
                {
                    // Kümülatif giriş ödüllerini (AccumulAtive) veritabanından çek
                    AccumulAtiveLoginAwardInfo[] awardList = pb.GetAccumulAtiveLoginAwardInfos();

                    // Her bir ödülü döngüye al
                    foreach (AccumulAtiveLoginAwardInfo award in awardList)
                    {
                        // Ödül bilgisini XML formatına çevirip sonuç listesine ekle
                        // FlashUtils, Flash istemcilerine uygun XML oluşturmak için kullanılan bir yardımcı sınıftır.
                        // NOT: Metot adında "AccumulAtive" (yazım hatası) olabilir, ancak kütüphede bu isimle tanımlıdır.
                        resultXml.Add(FlashUtils.CreateAccumulAtiveLoginAwards(award));
                    }

                    isSuccess = true;
                    message = "Başarılı!";
                }
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                LoginAwardItemTemplate.log.Error("LoginAwardItemTemplate yüklenirken hata:", ex);
            }

            // XML'e genel durum bilgilerini (value ve message) ekle
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // Sonucu sıkıştırıp (Compress) döndür
            return csFunction.CreateCompressXml(context, resultXml, "loginawarditemtemplate", true);
        }
    }
}