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
    // Token: 0x02000059 RID: 89
    // MapServerList sınıfı, oyun içindeki harita sunucularını (Map Servers) listelemek için kullanılan bir HTTP Handler'dır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class MapServerList : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x06000191 RID: 401 RVA: 0x0000288E File Offset: 0x00000A8E
        // Gelen isteği karşılayan metod
        public void ProcessRequest(HttpContext context)
        {
            // Veriyi hazırlayan metodu çağır ve sonucu yaz
            context.Response.Write(MapServerList.Build(context));
        }

        // Token: 0x06000192 RID: 402 RVA: 0x0000CEB0 File Offset: 0x0000B0B0
        // Veritabanından harita sunucularını çekip XML formatına çeviren metod
        public static string Build(HttpContext context)
        {
            bool isSuccess = false;
            string message = "Fail!";

            // Kök XML elementini oluştur
            XElement resultXml = new XElement("Result");

            try
            {
                // Veritabanı işlemleri için bağlantı oluştur
                using (MapBussiness mb = new MapBussiness())
                {
                    // Tüm harita sunucularını (MapServerList) veritabanından çek
                    ServerMapInfo[] serverMapList = mb.GetAllServerMap();

                    // Her bir harita sunucusunu döngüye al
                    foreach (ServerMapInfo map in serverMapList)
                    {
                        // Harita sunucu bilgisini XML formatına çevirip sonuç listesine ekle
                        // FlashUtils, Flash istemcilerine uygun XML oluşturmak için kullanılan bir yardımcı sınıftır.
                        resultXml.Add(FlashUtils.CreateMapServer(map));
                    }

                    isSuccess = true;
                    message = "Başarılı!";
                }
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                MapServerList.log.Error("MapServerList yüklenirken hata:", ex);
            }

            // XML'e genel durum bilgilerini (value ve message) ekle
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // Sonucu sıkıştırıp (Compress) döndür
            return csFunction.CreateCompressXml(context, resultXml, "MapServerList", true);
        }

        // Token: 0x17000059 RID: 89
        // (get) Token: 0x06000193 RID: 403 RVA: 0x00003828 File Offset: 0x00001A28
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