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
    // Token: 0x02000048 RID: 72
    // LoadItemsCategory sınıfı, oyun içindeki eşya kategorilerini (Eşya Grupları) listelemek için kullanılan bir HTTP Handler'dır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class LoadItemsCategory : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x06000148 RID: 328 RVA: 0x00002734 File Offset: 0x00000934
        // Gelen isteği karşılayan metod
        public void ProcessRequest(HttpContext context)
        {
            // Veriyi hazırlayan metodu çağır ve sonucu yaz
            // DÜZELTME: Orijinal kodda 'L'oadItemsCategory (küçük harf) olarak çağrılmıştı.
            // C# method isimleri case-sensitive olduğu için sınıf ismi ile uyumlu hale getirildi.
            context.Response.Write(LoadItemsCategory.Build(context));
        }

        // Token: 0x06000149 RID: 329 RVA: 0x0000A910 File Offset: 0x00008B10
        // Veritabanından kategorileri çekip XML formatına çeviren metod
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
                    // Tüm kategorileri (CategoryInfo) veritabanından çek
                    CategoryInfo[] categoryList = db.GetAllCategory();

                    // Her bir kategoriyi döngüye al
                    foreach (CategoryInfo category in categoryList)
                    {
                        // Kategori bilgisini XML formatına çevirip sonuç listesine ekle
                        // FlashUtils, Flash istemcilerine uygun XML oluşturmak için kullanılan bir yardımcı sınıftır.
                        resultXml.Add(FlashUtils.CreateCategoryInfo(category));
                    }
                }

                isSuccess = true;
                message = "Başarılı!";
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                LoadItemsCategory.log.Error("LoadItemsCategory yüklenirken hata:", ex);
            }

            // XML'e genel durum bilgilerini (value ve message) ekle
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // Sonucu sıkıştırıp (Compress) döndür
            return csFunction.CreateCompressXml(context, resultXml, "LoadItemsCategory", true);
        }

        // Token: 0x17000048 RID: 72
        // (get) Token: 0x0600014A RID: 330 RVA: 0x00003828 File Offset: 0x00001A28
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