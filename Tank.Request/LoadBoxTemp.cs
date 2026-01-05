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
    // Token: 0x02000047 RID: 71
    // LoadBoxTemp sınıfı, oyun içindeki sandık kutuları (Item Box) şablonlarını listelemek için kullanılan bir HTTP Handler'dır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class LoadBoxTemp : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x06000144 RID: 324 RVA: 0x0000A7DC File Offset: 0x000089DC
        // Gelen isteği karşılayan metod
        public void ProcessRequest(HttpContext context)
        {
            // İstek yapanın IP adresi yetkili mi kontrol et
            if (csFunction.ValidAdminIP(context.Request.UserHostAddress))
            {
                // Yetkiliyse veriyi çekip yanıt olarak gönder
                context.Response.Write(LoadBoxTemp.Build(context));
            }
            else
            {
                // Yetkili değilse espirili hata mesajı döndür
                context.Response.Write("Tabi Efendim!");
            }
        }

        // Token: 0x06000145 RID: 325 RVA: 0x0000A828 File Offset: 0x00008A28
        // Veritabanından kutu bilgilerini çekip XML formatına çeviren metod
        public static string Build(HttpContext context)
        {
            bool isSuccess = false;
            string message = "Hata!";

            // Kök XML elementini oluştur
            XElement resultXml = new XElement("Result");

            try
            {
                // Veritabanı işlemleri için bağlantı oluştur
                // DÜZELTME: Orijinal kodda "ProdueBussiness" (u harfi eksik) vardı, düzeltildi.
                using (ProduceBussiness db = new ProduceBussiness())
                {
                    // Tüm kutu bilgisini (ItemBoxInfo) veritabanından çek
                    ItemBoxInfo[] boxList = db.GetItemBoxInfos();

                    // Her bir kutu bilgisini döngüye al
                    foreach (ItemBoxInfo boxInfo in boxList)
                    {
                        // Kutu bilgisini XML formatına çevirip sonuç listesine ekle
                        // FlashUtils, Flash istemcilerine uygun XML oluşturmak için kullanılan bir yardımcı sınıftır.
                        resultXml.Add(FlashUtils.CreateItemBoxInfo(boxInfo));
                    }

                    isSuccess = true;
                    message = "Başarılı!";
                }
            }
            catch (Exception ex)
            {
                LoadBoxTemp.log.Error("LoadBoxTemp yüklenirken hata:", ex);
            }

            // XML'e genel durum bilgilerini (value ve message) ekle
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // Sonucu sıkıştırıp (Compress) döndür
            return csFunction.CreateCompressXml(context, resultXml, "LoadBoxTemp", true);
        }

        // Token: 0x17000047 RID: 71
        // (get) Token: 0x06000146 RID: 326 RVA: 0x00003828 File Offset: 0x00001A28
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