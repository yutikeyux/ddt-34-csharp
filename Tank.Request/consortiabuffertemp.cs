using System;
using System.Reflection;
using System.Web;
using System.Xml.Linq;
using Bussiness; // İş katmanı kütüphanesi
using log4net; // Loglama kütüphanesi
using Road.Flash; // Flash istemcisi yardımcı kütüphanesi
using SqlDataProvider.Data; // Veritabanı veri yapıları

namespace Tank.Request
{
    // Token: 0x0200001B RID: 27
    // consortiabuffertemp sınıfı, loncaların geçici efektlerini (buff) listelemek için kullanılan bir HTTP Handler'dır.
    // Not: Sınıf ismi küçük harfle başlıyor, proje standardı bu olabilir.
    public class consortiabuffertemp : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x0600006C RID: 108 RVA: 0x000058A8 File Offset: 0x00003AA8
        // Gelen isteği karşılayan metod
        public void ProcessRequest(HttpContext context)
        {
            // İstek yapanın IP adresi yetkili mi kontrol et
            if (csFunction.ValidAdminIP(context.Request.UserHostAddress))
            {
                // Yetkiliyse veriyi çekip yanıt olarak gönder
                context.Response.Write(consortiabuffertemp.Build(context));
            }
            else
            {
                context.Response.Write("Tabi Efendim!");
            }
        }

        // Token: 0x0600006D RID: 109 RVA: 0x000058F4 File Offset: 0x00003AF4
        // Veritabanından geçici lonca efektlerini çekip XML formatına çeviren metod.
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
                    // Tüm geçici lonca efektlerini (Buff Temp) veritabanından çek
                    ConsortiaBuffTempInfo[] allBuffs = db.GetAllConsortiaBuffTemp();

                    // Her bir efekti döngüye al
                    foreach (ConsortiaBuffTempInfo buff in allBuffs)
                    {
                        // Efekt bilgisini XML formatına çevirip sonuç listesine ekle
                        // FlashUtils, Flash istemcilerine uygun XML oluşturmak için kullanılan bir yardımcı sınıftır.
                        resultXml.Add(FlashUtils.CreateConsortiaBuffer(buff));
                    }

                    isSuccess = true;
                    message = "Başarılı!";
                }
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                consortiabuffertemp.log.Error("ConsortiaBuffTemp yüklenirken hata:", ex);
            }

            // XML'e genel durum bilgilerini (value ve message) ekle
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // --- NOT: Orijinal kodda burası iki kez çağrılıyor ---
            // İlk çağrı: Sıkıştırma kapalı (false), muhtemelen sunucu taraflı loglama veya dosyaya yazma için.
            csFunction.CreateCompressXml(context, resultXml, "consortiabuffertemp_out", false);

            // İkinci çağrı: Sıkıştırma açık (true), istemciye gönderilecek asıl sonuç.
            return csFunction.CreateCompressXml(context, resultXml, "consortiabuffertemp", true);
        }

        // Token: 0x17000017 RID: 23
        // (get) Token: 0x0600006E RID: 110 RVA: 0x00003828 File Offset: 0x00001A28
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