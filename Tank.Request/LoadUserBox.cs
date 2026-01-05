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
    // Token: 0x0200004D RID: 77
    // LoadUserBox sınıfı, kullanıcının sandık (User Box) envanterini yüklemek için kullanılan bir HTTP Handler'dır.
    public class LoadUserBox : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        // NOT: Orijinal kodda bu değişken tanımı eksikti, ancak yapı bütünlüğü için diğer örneklerdeki gibi ekledik.
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x06000161 RID: 353 RVA: 0x0000AF58 File Offset: 0x00009158
        // Gelen isteği karşılayan metod
        public void ProcessRequest(HttpContext context)
        {
            // İstek yapanın IP adresi yetkili mi kontrol et
            if (csFunction.ValidAdminIP(context.Request.UserHostAddress))
            {
                // Yetkiliyse veriyi çekip yanıt olarak gönder
                context.Response.Write(LoadUserBox.Build(context));
            }
            else
            {
                // Yetkili değilse espirili hata mesajı döndür
                context.Response.Write("Tabi Efendim!");
            }
        }

        // Token: 0x06000162 RID: 354 RVA: 0x0000AFA4 File Offset: 0x000091A4
        // Veritabanından sandık (Box) bilgilerini çekip XML formatına çeviren metod
        public static string Build(HttpContext context)
        {
            bool isSuccess = false;
            string message = "Hata!";

            // Kök XML elementini oluştur
            XElement resultXml = new XElement("Result");

            try
            {
                // Veritabanı işlemleri için bağlantı oluştur
                using (ProduceBussiness pb = new ProduceBussiness())
                {
                    // Tüm kullanıcı sandıklarını (UserBox) veritabanından çek
                    UserBoxInfo[] userBoxList = pb.GetAllUserBox();

                    // Her bir sandık bilgisini döngüye al
                    foreach (UserBoxInfo userBox in userBoxList)
                    {
                        // Sandık bilgisini XML formatına çevirip sonuç listesine ekle
                        // FlashUtils, Flash istemcilerine uygun XML oluşturmak için kullanılan bir yardımcı sınıftır.
                        resultXml.Add(FlashUtils.CreateUserBoxInfo(userBox));
                    }

                    isSuccess = true;
                    message = "Başarılı!";
                }
            }
            catch (Exception ex)
            {
                // Orijinal kodda catch bloğu boştu ({ }).
                // Hata oluşursa loglama yapmaz.
                LoadUserBox.log.Error("Hata:", ex);
            }

            // XML'e genel durum bilgilerini (value ve message) ekle
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // Sonucu sıkıştırıp (Compress) döndür
            return csFunction.CreateCompressXml(context, resultXml, "LoadUserBox", true);
        }

        // Token: 0x1700004D RID: 77
        // (get) Token: 0x06000163 RID: 355 RVA: 0x00003828 File Offset: 0x00001A28
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