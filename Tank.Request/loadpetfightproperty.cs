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
    // Token: 0x0200004A RID: 74
    // loadpetfightproperty sınıfı, oyun içindeki evcil hayvan dövüş özelliklerini listelemek için kullanılan bir HTTP Handler'dır.
    public class loadpetfightproperty : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x06000152 RID: 338 RVA: 0x0000AB4C File Offset: 0x00008D4C
        // Gelen isteği karşılayan metod
        public void ProcessRequest(HttpContext context)
        {
            // İstek yapanın IP adresi yetkili mi kontrol et
            if (csFunction.ValidAdminIP(context.Request.UserHostAddress))
            {
                // Yetkiliyse veriyi çekip yanıt olarak gönder
                context.Response.Write(loadpetfightproperty.Build(context));
            }
            else
            {
                // Yetkili değilse espirili hata mesajı döndür
                context.Response.Write("Tabi Efendim!");
            }
        }

        // Token: 0x06000153 RID: 339 RVA: 0x0000AB94 File Offset: 0x00008D94
        // Veritabanından pet dövüş özelliklerini çekip XML formatına çeviren metod
        public static string Build(HttpContext context)
        {
            bool isSuccess = false;
            string message = "Fail!";

            // Kök XML elementini oluştur
            XElement resultXml = new XElement("Result");

            try
            {
                // Veritabanı işlemleri için bağlantı oluştur
                using (ProduceBussiness pb = new ProduceBussiness())
                {
                    // Kök XML şablon elementi oluştur
                    // Dikkat: Burası 'Result' içinde bir 'ItemTemplate' elementi oluşturur.
                    // Bu yapı, Flash istemcisinin tüm özellikleri bir şablon içinde beklemesine göre ayarlanmıştır.
                    XElement templateNode = new XElement("ItemTemplate");

                    // Tüm pet dövüş özelliklerini veritabanından çek
                    PetFightPropertyInfo[] petFightProperties = pb.GetAllPetFightProperty();

                    // Her bir özelliği döngüye al
                    foreach (PetFightPropertyInfo petFightInfo in petFightProperties)
                    {
                        // Özellik bilgisini XML formatına çevirip şablona ekle
                        // FlashUtils, Flash istemcilerine uygun XML oluşturmak için kullanılan bir yardımcı sınıftır.
                        // NOT: Orijinal kodda 'CreatePetFightProterpy' (Proterpy kelime hatası?) kullanılmıştı.
                        // Bu kütüphanenin metod adı olarak korunmuştur.
                        templateNode.Add(FlashUtils.CreatePetFightProterpy(petFightInfo));
                    }

                    // Şablonu ana sonuca ekle
                    resultXml.Add(templateNode);

                    isSuccess = true;
                    message = "Success!";
                }
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                loadpetfightproperty.log.Error("loadpetfightproperty yüklenirken hata:", ex);
            }

            // XML'e genel durum bilgilerini (value ve message) ekle
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // --- NOT: Orijinal kodda burası iki kez çağrılıyor ---
            // İlk çağrı: Sıkıştırma kapalı (false), muhtemelen sunucu taraflı loglama veya dosyaya yazma için.
            // Dosya adı "loadpetfightproperty_out" olarak kullanılmış.
            csFunction.CreateCompressXml(context, resultXml, "loadpetfightproperty_out", false);

            // İkinci çağrı: Sıkıştırma açık (true), istemciye gönderilecek asıl sonuç.
            return csFunction.CreateCompressXml(context, resultXml, "loadpetfightproperty", true);
        }

        // Token: 0x1700004A RID: 74
        // (get) Token: 0x06000154 RID: 340 RVA: 0x00003828 File Offset: 0x00001A28
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