using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness; // İş katmanı kütüphanesi
using log4net; // Loglama kütüphanesi
using SqlDataProvider.Data; // Veritabanı veri yapıları

namespace Tank.Request
{
    // Token: 0x0200001D RID: 29
    // ConsortiaEquipControl sınıfı, lonca ekipmanlarının kontrolünü veya seviyelerini listelemek için kullanılan bir HTTP Handler'dır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ConsortiaEquipControl : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x06000075 RID: 117 RVA: 0x00005BA8 File Offset: 0x00003DA8
        // Gelen isteği karşılayan metod
        public void ProcessRequest(HttpContext context)
        {
            bool isSuccess = false;
            string message = "Hata!";
            int totalCount = 0;

            // Kök XML elementini oluştur
            XElement resultXml = new XElement("Result");

            try
            {
                // Lonca ID'sini al
                int consortiaID = int.Parse(context.Request["consortiaID"]);

                // Veritabanı işlemleri için bağlantı oluştur
                using (ConsortiaBussiness db = new ConsortiaBussiness())
                {
                    // İÇ İÇE DÖNGÜ YAPISI:
                    // type (i): 1'den 2'ye kadar (Type: 1 ve 2)
                    // level (j): 1'den 10'a kadar (Level: 1, 2, ... 10)

                    for (int type = 1; type < 3; type++)
                    {
                        for (int level = 1; level < 11; level++)
                        {
                            // Lonca ID, Seviye ve Tip bilgisine göre veritabanında eşleşen ekipmanı ara
                            // Not: Metot parametre sırası sırasıyla consortiaID, level, type şeklindedir.
                            ConsortiaEquipControlInfo equipInfo = db.GetConsortiaEquipRiches(consortiaID, level, type);

                            // Kayıt varsa listeye ekle
                            if (equipInfo != null)
                            {
                                resultXml.Add(new XElement("Item", new object[]
                                {
                                    new XAttribute("type", equipInfo.Type),
                                    new XAttribute("level", equipInfo.Level),
                                    new XAttribute("riches", equipInfo.Riches)
                                }));
                                totalCount++;
                            }
                        }
                    }

                    isSuccess = true;
                    message = "Başarılı!";
                }
            }
            catch (Exception ex)
            {
                // Orijinal kodda "ConsortiaEventList" diye yanlış bir log mesajı vardı.
                // Bu, bu dosyadan kopyalanmış başka bir kodun kalıntısıdır.
                ConsortiaEquipControl.log.Error("ConsortiaEquipControl yüklenirken hata:", ex);
            }

            // XML'e genel durum bilgilerini (value ve message) ekle
            resultXml.Add(new XAttribute("total", totalCount));
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // Çıktı tipini belirle ve yanıtı yaz
            context.Response.ContentType = "text/plain";
            context.Response.Write(resultXml.ToString(false));
        }

        // Token: 0x17000019 RID: 25
        // (get) Token: 0x06000076 RID: 118 RVA: 0x00003828 File Offset: 0x00001A28
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