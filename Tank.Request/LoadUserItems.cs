using Bussiness; // İş katmanı kütüphanesi
using log4net; // Loglama kütüphanesi
using Road.Flash; // Flash istemcisi yardımcı kütüphanesi
using SqlDataProvider.Data; // Veritabanı veri yapıları
using System;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace Tank.Request
{
    // Token: 0x0200004F RID: 79
    // LoadUserItems sınıfı, kullanıcının envanterindeki (User Item) eşyaları yüklemek için kullanılan bir HTTP Handler'dır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class LoadUserItems : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x06000169 RID: 361 RVA: 0x0000B4AC File Offset: 0x000094AC
        // Gelen isteği karşılayan ve kullanıcı eşyalarını hazırlayan metod
        public void ProcessRequest(HttpContext context)
        {
            bool isSuccess = false;
            string message = "Hata!";

            // Kök XML elementini oluştur
            XElement resultXml = new XElement("Result");

            try
            {
                // Kullanıcı ID'sini al (Params kullanımı, Request["ID"] de olabilir)
                int userID = int.Parse(context.Request.Params["ID"]);

                // Veritabanı işlemleri için bağlantı oluştur
                using (PlayerBussiness pb = new PlayerBussiness())
                {
                    // Kullanıcının eşyalarını veritabanından çek
                    // DÜZELTME: GetUserItem muhtemelen List<ItemInfo> döner.
                    // Bu nedenle .ToArray() ile açıkça diziye çeviriyoruz.
                    ItemInfo[] userItems = pb.GetUserItem(userID).ToArray();

                    // Her bir eşyayı döngüye al
                    foreach (ItemInfo userItem in userItems)
                    {
                        // Eşya bilgisini XML formatına çevirip sonuç listesine ekle
                        // FlashUtils, Flash istemcilerine uygun XML oluşturmak için kullanılan bir yardımcı sınıftır.
                        resultXml.Add(FlashUtils.CreateGoodsInfo(userItem));
                    }

                    isSuccess = true;
                    message = "Başarılı!";
                }
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                LoadUserItems.log.Error("LoadUserItems yüklenirken hata:", ex);
            }

            // XML'e genel durum bilgilerini (value ve message) ekle
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // Yanıtı ekrana yaz
            // Önceki örneklerdeki gibi sıkıştırma (Compress) yapılmaz.
            context.Response.ContentType = "text/plain";
            context.Response.Write(resultXml.ToString(false));
        }

        // Token: 0x1700004F RID: 79
        // (get) Token: 0x0600016A RID: 362 RVA: 0x00003828 File Offset: 0x00001A28
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