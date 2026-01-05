using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness; // İş katmanı kütüphanesi
using log4net; // Loglama kütüphanesi

namespace Tank.Request
{
    // Token: 0x0200000C RID: 12
    // AdvanceQuestTime sınıfı, ileri seviye görev zamanlamaları için kullanılan bir HTTP Handler'dır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class AdvanceQuestTime : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x0600002D RID: 45 RVA: 0x0000403C File Offset: 0x0000223C
        // Gelen isteği karşılayan metod
        public void ProcessRequest(HttpContext context)
        {
            bool isSuccess = false;
            string message = "Hata!";

            // NOT: XML elementi oluşturuluyor ancak kodun sonunda ekrana YAZILMIYOR.
            // Sadece bellekte oluşup boşa gidiyor.
            XElement unusedXml = new XElement("Result");

            try
            {
                // İstekten "userid" parametresini alıyoruz (Dönüş değeri bile atanmıyor, sadece doğrulama için parse ediliyor).
                int.Parse(context.Request["userid"]);

                // --- ŞAŞIRTICI KOD PARÇASI (Dead Code) ---
                // PlayerBussiness oluşturuluyor ama içinde hiçbir işlem yapılmıyor.
                // Muhtemelen kopyala-yapştır (Copy-Paste) sonrası temizlenmeyen bir kod kalıntısıdır.
                // İsteğiniz üzerine silmedim, ancak teknik olarak hiçbir işlevi yoktur.
                using (new PlayerBussiness())
                {
                    // İşlem yapılmıyor
                }

                isSuccess = true;
                message = "Başarılı!";
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla.
                // NOT: Orijinal log mesajı "IMListLoad" (Anlık Mesaj Listesi) olduğu gibi bırakıldı.
                AdvanceQuestTime.log.Error("IMListLoad", ex);
            }

            // Yukarıda oluşturulan XML'e veriler eklendi...
            unusedXml.Add(new XAttribute("value", isSuccess));
            unusedXml.Add(new XAttribute("message", message));

            // ...AMA bu XML ekrana asla yazılmıyor.

            // --- ACTUAL OUTPUT (GERÇEK ÇIKTI) ---
            // İstemciye XML yerine şu formatta basit bir string döner: "0,Tarih,0"
            // Örnek: "0,27.10.2023 14:30:00,0"
            // Muhtemelen istemci tarafındaki sistem zamanı senkronizasyonu için böyle basit bir format beklenmiştir.
            context.Response.ContentType = "text/plain";
            context.Response.Write(string.Format("0,{0},0", DateTime.Now));
        }

        // Token: 0x17000009 RID: 9
        // (get) Token: 0x0600002E RID: 46 RVA: 0x0000215A File Offset: 0x0000035A
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