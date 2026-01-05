using Bussiness; // İş katmanı kütüphanesi
using log4net;
using Road.Flash; // Flash istemcisi yardımcı kütüphanesi
using SqlDataProvider.Data; // Veritabanı veri yapıları
using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
namespace Tank.Request
{
    // Token: 0x02000010 RID: 16
    // bombconfig sınıfı, oyundaki bombaların (topların) ayarlarını listelemek için kullanılan bir HTTP Handler'dır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class bombconfig : IHttpHandler
    {
        // Token: 0x0600003E RID: 62 RVA: 0x000047CC File Offset: 0x000029CC
        // Gelen isteği karşılayan metod
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void ProcessRequest(HttpContext context)
        {
            // İstek yapanın IP adresi yetkili mi kontrol et
            if (csFunction.ValidAdminIP(context.Request.UserHostAddress))
            {
                // Yetkiliyse veriyi çekip yanıt olarak gönder
                context.Response.Write(bombconfig.Build(context));
            }
            else
            {
                // Yetkili değilse hata mesajı döndür
                context.Response.Write("Tabi Efendim!");
            }
        }

        // Token: 0x0600003F RID: 63 RVA: 0x00004818 File Offset: 0x00002A18
        // Veritabanından bomba ayarlarını (BallConfig) çekip XML formatına çeviren metod.
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
                    // Tüm bomba konfigürasyonlarını veritabanından çek
                    BallConfigInfo[] allConfigs = db.GetAllBallConfig();

                    // Her bir konfigürasyonu döngüye al
                    foreach (BallConfigInfo config in allConfigs)
                    {
                        // Konfigürasyon bilgilerini XML formatına çevirip sonuç listesine ekle
                        // FlashUtils, Flash istemcilerine uygun XML oluşturmak için kullanılan bir yardımcı sınıftır.
                        resultXml.Add(FlashUtils.CreateBallConfigInfo(config));
                    }
                }

                isSuccess = true;
                message = "Başarılı!";
            }
            catch (Exception ex)
            {
                // ÖNEMLİ NOT: Orijinal kodda catch bloğu tamamen boştur. Hata (Exception) yakalanıp yok sayılıyor.
                // Bu dosyada 'log4net' namespace'i import edilmediği için loglama yapılamıyor.
                bombconfig.log.Error("BombConfig Hatası", ex);
            }

            // XML'e genel durum bilgilerini (value ve message) ekle
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // Sonucu sıkıştırıp (Compress) döndür
            return csFunction.CreateCompressXml(context, resultXml, "bombconfig", true);
        }

        // Token: 0x1700000D RID: 13
        // (get) Token: 0x06000040 RID: 64 RVA: 0x00003828 File Offset: 0x00001A28
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