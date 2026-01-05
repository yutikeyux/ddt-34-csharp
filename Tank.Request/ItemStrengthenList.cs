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
    // Token: 0x02000044 RID: 68
    // ItemStrengthenList sınıfı, oyun içindeki eşya güçlendirme (Strengthen) yapılarını listelemek için kullanılan bir HTTP Handler'dır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ItemStrengthenList : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x06000137 RID: 311 RVA: 0x000026F3 File Offset: 0x000008F3
        // Gelen isteği karşılayan metod
        public void ProcessRequest(HttpContext context)
        {
            // İsteği hazırlayan metodu çağır ve sonucu yaz
            context.Response.Write(ItemStrengthenList.Build(context));
        }

        // Token: 0x06000138 RID: 312 RVA: 0x0000A4C8 File Offset: 0x000086C8
        // Veritabanından güçlendirme yapılarını çekip XML formatına çeviren metod
        public static string Build(HttpContext context)
        {
            bool isSuccess = false;
            string message = "Fail!";

            // Kök XML elementini oluştur
            XElement resultXml = new XElement("Result");

            try
            {
                // Veritabanı işlemleri için bağlantı oluştur
                // DÜZELTME: Orijinal kodda "ProduceBussiness" (d harfi eksik) kullanılmıştı.
                // Standart projedeki sınıf ismi "ProduceBussiness" olduğu için düzeltildi.
                using (ProduceBussiness db = new ProduceBussiness())
                {
                    // Tüm güçlendirme bilgilerini (StrengthenInfo) veritabanından çek
                    StrengthenInfo[] strengthenList = db.GetAllStrengthen();

                    // Her bir güçlendirme bilgisini döngüye al
                    foreach (StrengthenInfo strengthenInfo in strengthenList)
                    {
                        // Güçlendirme bilgisini XML formatına çevirip sonuç listesine ekle
                        // FlashUtils, Flash istemcilerine uygun XML oluşturmak için kullanılan bir yardımcı sınıftır.
                        resultXml.Add(FlashUtils.CreateStrengthenInfo(strengthenInfo));
                    }

                    isSuccess = true;
                    message = "Success!";
                }
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                ItemStrengthenList.log.Error("ItemStrengthenList yüklenirken hata:", ex);
            }

            // XML'e genel durum bilgilerini (value ve message) ekle
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // Sonucu sıkıştırıp (Compress) döndür
            return csFunction.CreateCompressXml(context, resultXml, "ItemStrengthenList", true);
        }

        // Token: 0x17000044 RID: 68
        // (get) Token: 0x06000139 RID: 313 RVA: 0x00003828 File Offset: 0x00001A28
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