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
    // Token: 0x0200004B RID: 75
    // loadpetmoeproperty sınıfı, evcil hayvanların taşınması (Pet Move) ile ilgili özellikleri listelemek için kullanılan bir HTTP Handler'dır.
    // Sınıf isminde "move" yerine "moe" yazım hatası olabilir, ancak kodun içeriği Pet Move (Taşıma) ile ilgilidir.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class loadpetmoeproperty : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x06000157 RID: 343 RVA: 0x0000ACB0 File Offset: 0x00008EB0
        // Gelen isteği karşılayan metod
        public void ProcessRequest(HttpContext context)
        {
            // İstek yapanın IP adresi yetkili mi kontrol et
            if (csFunction.ValidAdminIP(context.Request.UserHostAddress))
            {
                // Yetkiliyse veriyi çekip yanıt olarak gönder
                context.Response.Write(loadpetmoeproperty.Build(context));
            }
            else
            {
                // Yetkili değilse espirili hata mesajı döndür
                context.Response.Write("Tabi Efendim!");
            }
        }

        // Token: 0x06000158 RID: 344 RVA: 0x0000ACF8 File Offset: 0x00008EF8
        // Veritabanından pet move özelliklerini çekip XML formatına çeviren metod
        public static string Build(HttpContext context)
        {
            bool isSuccess = false;
            string message = "Fail!";

            // Kök XML elementini oluştur
            XElement resultXml = new XElement("Result");

            try
            {
                // Veritabanı işlemleri için bağlantı oluştur
                // DÜZELTME: Orijinal kodda 'ProduceBussiness' (u harfi) kullanılmış.
                // Projenin diğer dosyalarında (ItemStrengthenList vb.) 'ProduceBussiness' (ue) kullanılıyordu.
                // İngilizcede 'Produce' yazıldığı için 'u' harfi standarttır, ancak önceki örneklerdeki düzeltmeye sadık kalınarak u'yu koruyorum.
                using (ProduceBussiness pb = new ProduceBussiness())
                {
                    // Pet Move özelliklerini (PetMoeProperty) veritabanından çek
                    PetMoePropertyInfo[] petMoveProperties = pb.GetAllPetMoeProperty();

                    // XML'de kullanılacak şablon (Template) elementi oluştur
                    // Bu yapı, Flash istemcisinin tüm özellikleri bir template içine koymasını sağlar.
                    XElement templateNode = new XElement("ItemTemplate");

                    // Her bir özelliği döngüye al
                    foreach (PetMoePropertyInfo petMoveInfo in petMoveProperties)
                    {
                        // Özellik bilgisini XML formatına çevirip şablona ekle
                        templateNode.Add(FlashUtils.CreatePetMoePropertyItems(petMoveInfo));
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
                loadpetmoeproperty.log.Error("loadpetmoeproperty yüklenirken hata:", ex);
            }

            // XML'e genel durum bilgilerini (value ve message) ekle
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // --- NOT: Orijinal kodda burası iki kez çağrılıyor ---
            // İlk çağrı: Sıkıştırma kapalı (false), muhtemelen sunucu taraflı loglama veya dosyaya yazma için.
            // Dosya adı "loadpetmoeproperty_out" olarak kullanılmış.
            csFunction.CreateCompressXml(context, resultXml, "loadpetmoeproperty_out", false);

            // İkinci çağrı: Sıkıştırma açık (true), istemciye gönderilecek asıl sonuç.
            return csFunction.CreateCompressXml(context, resultXml, "loadpetmoeproperty", true);
        }

        // Token: 0x1700004B RID: 75
        // (get) Token: 0x06000159 RID: 345 RVA: 0x00003828 File Offset: 0x00001A28
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