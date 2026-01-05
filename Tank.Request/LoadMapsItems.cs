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
    // Token: 0x02000049 RID: 73
    // LoadMapsItems sınıfı, oyun içindeki harita listesini (Maps) yüklemek için kullanılan bir HTTP Handler'dır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class LoadMapsItems : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x0600014D RID: 333 RVA: 0x0000AA08 File Offset: 0x00008C08
        // Gelen isteği karşılayan metod
        public void ProcessRequest(HttpContext context)
        {
            // İstek yapanın IP adresi yetkili mi kontrol et
            if (csFunction.ValidAdminIP(context.Request.UserHostAddress))
            {
                // Yetkiliyse veriyi çekip yanıt olarak gönder
                context.Response.Write(LoadMapsItems.Build(context));
            }
            else
            {
                // Yetkili değilse espirili hata mesajı döndür
                context.Response.Write("Tabi Efendim!");
            }
        }

        // Token: 0x0600014E RID: 334 RVA: 0x0000AA54 File Offset: 0x00008C54
        // Veritabanından haritaları çekip XML formatına çeviren metod
        public static string Build(HttpContext context)
        {
            bool isSuccess = false;
            string message = "Fail!";

            // Kök XML elementini oluştur
            XElement resultXml = new XElement("Result");

            try
            {
                // Veritabanı işlemleri için bağlantı oluştur
                using (MapBussiness db = new MapBussiness())
                {
                    // Tüm haritaları veritabanından çek
                    MapInfo[] mapList = db.GetAllMap();

                    // Her bir haritayı döngüye al
                    foreach (MapInfo mapInfo in mapList)
                    {
                        // Harita bilgisini XML formatına çevirip sonuç listesine ekle
                        // FlashUtils, Flash istemcilerine uygun XML oluşturmak için kullanılan bir yardımcı sınıftır.
                        resultXml.Add(FlashUtils.CreateMapInfo(mapInfo));
                    }
                }

                isSuccess = true;
                message = "Başarılı!";
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                LoadMapsItems.log.Error("LoadMapsItems yüklenirken hata:", ex);
            }

            // XML'e genel durum bilgilerini (value ve message) ekle
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // Sonucu sıkıştırıp (Compress) döndür
            return csFunction.CreateCompressXml(context, resultXml, "LoadMapsItems", true);
        }

        // Token: 0x17000049 RID: 73
        // (get) Token: 0x0600014F RID: 335 RVA: 0x00003828 File Offset: 0x00001A28
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