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
    // Token: 0x02000012 RID: 18
    // CardUpdateInfo sınıfı, kartların güncelleme bilgilerini listelemek için kullanılan bir HTTP Handler'dır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CardUpdateInfo : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x06000047 RID: 71 RVA: 0x0000222A File Offset: 0x0000042A//
        // Gelen isteği karşılayan metod
        public void ProcessRequest(HttpContext context)
        {
            // NOT: Bu sınıfta IP kontrolü (Admin kontrolü) yoktur. Herkese açık bir endpointtir.
            context.Response.Write(CardUpdateInfo.build(context));
        }

        // Token: 0x06000048 RID: 72 RVA: 0x000049F8 File Offset: 0x00002BF8
        // Veritabanından kart güncelleme bilgilerini çekip XML formatına çeviren metod.
        // Not: Orijinal kodda metot adı "Build" (Build) olarak hatalı yazılmış, sistemin uyumluluğu için böyle bırakıldı.
        public static string build(HttpContext context)
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
                    // Tüm kart güncelleme bilgilerini veritabanından çek
                    // ÖNEMLİ: Sınıf adı 'CardUpdateInfo' ile veritabanı modeli 'SqlDataProvider.Data.CardUpdateInfo' çakışıyor.
                    // Bu yüzden tam yol (Fully Qualified Name) kullanılmıştır.
                    SqlDataProvider.Data.CardUpdateInfo[] allUpdates = db.GetAllCardUpdateInfo();

                    // Her bir güncelleme bilgisini döngüye al
                    foreach (SqlDataProvider.Data.CardUpdateInfo updateInfo in allUpdates)
                    {
                        // Güncelleme bilgilerini XML formatına çevirip sonuç listesine ekle
                        // FlashUtils, Flash istemcilerine uygun XML oluşturmak için kullanılan bir yardımcı sınıftır.
                        resultXml.Add(FlashUtils.CreateCardUpdateInfo(updateInfo));
                    }

                    isSuccess = true;
                    message = "Başarılı!";
                }
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                CardUpdateInfo.log.Error("Load CardUpdateInfo is fail!", ex);
            }

            // XML'e genel durum bilgilerini (value ve message) ekle
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // Sonucu sıkıştırıp (Compress) döndür
            return csFunction.CreateCompressXml(context, resultXml, "CardUpdateInfo", true);
        }

        // Token: 0x1700000F RID: 15
        // (get) Token: 0x06000049 RID: 73 RVA: 0x00003828 File Offset: 0x00001A28
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