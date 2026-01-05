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
    // Token: 0x02000011 RID: 17
    // CardUpdateCondition sınıfı, kartların nasıl güncelleneceğine dair koşulları listelemek için kullanılan bir HTTP Handler'dır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CardUpdateCondition : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x06000042 RID: 66 RVA: 0x000021FF File Offset: 0x000003FF
        // Gelen isteği karşılayan metod
        public void ProcessRequest(HttpContext context)
        {
            // NOT: Bu sınıfta önceki örneklerdeki gibi (csFunction.ValidAdminIP) IP kontrolü YOKTUR.
            // Bu endpoint genel olarak erişilebilir durumdadır.
            context.Response.Write(CardUpdateCondition.Build(context));
        }

        // Token: 0x06000043 RID: 67 RVA: 0x00004900 File Offset: 0x00002B00
        // Veritabanından kart güncelleme koşullarını çekip XML formatına çeviren metod.
        // Not: Orijinal kodda metot adı "Build" (Build) olarak hatalı yazılmış, sistemin uyumluluğu için böyle bırakıldı.
        public static string Build(HttpContext context)
        {
            bool isSuccess = false;
            string message = "Hata!";

            // Kök XML elementini oluştur
            XElement resultXml = new XElement("Result");

            try
            {
                // Veritabanı işlemleri için bağlantı oluştur
                using (ProduceBussiness db = new ProduceBussiness())
                {
                    // Tüm kart güncelleme koşullarını veritabanından çek
                    CardUpdateConditionInfo[] allConditions = db.GetAllCardUpdateCondition();

                    // Her bir koşulu döngüye al
                    foreach (CardUpdateConditionInfo condition in allConditions)
                    {
                        // Koşul bilgilerini XML formatına çevirip sonuç listesine ekle
                        // FlashUtils, Flash istemcilerine uygun XML oluşturmak için kullanılan bir yardımcı sınıftır.
                        resultXml.Add(FlashUtils.CreateCardUpdateCondition(condition));
                    }

                    isSuccess = true;
                    message = "Başarılı!";
                }
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                CardUpdateCondition.log.Error("Kart güncelleme koşulları yüklenirken hata:", ex);
            }

            // XML'e genel durum bilgilerini (value ve message) ekle
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // Sonucu sıkıştırıp (Compress) döndür
            return csFunction.CreateCompressXml(context, resultXml, "CardUpdateCondition", true);
        }

        // Token: 0x1700000E RID: 14
        // (get) Token: 0x06000044 RID: 68 RVA: 0x00003828 File Offset: 0x00001A28
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