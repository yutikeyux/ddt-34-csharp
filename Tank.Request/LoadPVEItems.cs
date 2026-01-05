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
    // Token: 0x0200004C RID: 76
    // LoadPVEItems sınıfı, oyun içindeki PVE (Oyuncuya Karşı Ortam) seviyelerini veya eşyalarını listelemek için kullanılan bir HTTP Handler'dır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class LoadPVEItems : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x0600015C RID: 348 RVA: 0x0000AE14 File Offset: 0x00009014
        // Gelen isteği karşılayan metod
        public void ProcessRequest(HttpContext context)
        {
            // İstek yapanın IP adresi yetkili mi kontrol et
            if (csFunction.ValidAdminIP(context.Request.UserHostAddress))
            {
                // Yetkiliyse veriyi çekip yanıt olarak gönder
                context.Response.Write(LoadPVEItems.Build(context));
            }
            else
            {
                // Yetkili değilse espirili hata mesajı döndür
                context.Response.Write("Tabi Efendim!");
            }
        }

        // Token: 0x0600015D RID: 349 RVA: 0x0000AE60 File Offset: 0x00009060
        // Veritabanından PVE bilgilerini çekip XML formatına çeviren metod
        public static string Build(HttpContext context)
        {
            bool isSuccess = false;
            string message = "Fail!";

            // Kök XML elementini oluştur
            XElement resultXml = new XElement("Result");

            try
            {
                // Veritabanı işlemleri için bağlantı oluştur
                // PveBussiness sınıfı, PVE (Player vs Environment) ile ilgili işlemleri yönetir.
                using (PveBussiness pb = new PveBussiness())
                {
                    // Tüm PVE bilgilerini (Seviyeler, Mapler vb.) veritabanından çek
                    PveInfo[] pveInfoList = pb.GetAllPveInfos();

                    // Her bir PVE bilgisini döngüye al
                    foreach (PveInfo pveInfo in pveInfoList)
                    {
                        // PVE bilgisini XML formatına çevirip sonuç listesine ekle
                        // FlashUtils, Flash istemcilerine uygun XML oluşturmak için kullanılan bir yardımcı sınıftır.
                        resultXml.Add(FlashUtils.CreatePveInfo(pveInfo));
                    }

                    isSuccess = true;
                    message = "Başarılı!";
                }
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                LoadPVEItems.log.Error("LoadPVEItems yüklenirken hata:", ex);
            }

            // XML'e genel durum bilgilerini (value ve message) ekle
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // Sonucu sıkıştırıp (Compress) döndür
            return csFunction.CreateCompressXml(context, resultXml, "LoadPVEItems", true);
        }

        // Token: 0x1700004C RID: 76
        // (get) Token: 0x0600015E RID: 350 RVA: 0x00003828 File Offset: 0x00001A28
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