using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using System.Xml.Linq;
using log4net; // Loglama kütüphanesi
using zlib; // Veri sıkıştırma kütüphanesi

namespace Tank.Request
{
    // Token: 0x02000017 RID: 23
    // CheckRegistration sınıfı, kayıt durumunu kontrol etmek için kullanılan bir HTTP Handler'dır.
    // IRequiresSessionState arayüzü, bu sınıfın Session oturumlarına erişebileceğini belirtir.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CheckRegistration : IHttpHandler, IRequiresSessionState
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x0600005B RID: 91 RVA: 0x00005244 File Offset: 0x00003444
        // Gelen isteği karşılayan metod
        public void ProcessRequest(HttpContext context)
        {
            // --- DEĞİŞKEN TANIMLARI ---
            // NOT: Bu kod şu an veritabanına bağlanmaz. Sadece sabit değer döner.
            // Gerçek bir kontrol yapılmadığı için muhtemelen bir "Stub" (placeholder) koddur.
            bool isRegistered = true;
            string message = "Registered!";
            int status = 1; // Kayıt durumu (1 = Başarılı/Kayıtlı)

            // Sonuç XML'ini oluştur
            XElement resultXml = new XElement("Result");

            // XML'e nitelikleri (attributes) ekle
            resultXml.Add(new XAttribute("value", isRegistered));
            resultXml.Add(new XAttribute("message", message));
            resultXml.Add(new XAttribute("status", status));

            // Çıktı tipini belirle
            context.Response.ContentType = "text/plain";

            // --- SIKIŞTIRMA VE GÖNDERME ---
            // Static sınıfının içindeki Compress metodunu kullanarak XML'i sıkıştırıp binary olarak yazar
            context.Response.BinaryWrite(StaticFunction.Compress(resultXml.ToString()));
        }

        // Token: 0x0600005C RID: 92 RVA: 0x000052E4 File Offset: 0x000034E4
        // Sınıf içinde tanımlı alternatif bir sıkıştırma metodu.
        // NOT: ProcessRequest metodu yukarıda 'StaticFunction.Compress' kullandığı için
        // bu yerel metot o anda doğrudan çağrılmaz. Ancak kodun bütünlüğü için korundu.
        public static byte[] Compress(byte[] data)
        {
            MemoryStream memoryStream = new MemoryStream();

            // ZOutputStream kullanarak bellek akışına sıkıştırma yapar. '3' sıkıştırma seviyesidir.
            ZOutputStream zOutStream = new ZOutputStream(memoryStream, 3);

            zOutStream.Write(data, 0, data.Length);
            zOutStream.Flush();
            zOutStream.Close();

            return memoryStream.ToArray();
        }

        // Token: 0x17000013 RID: 19
        // (get) Token: 0x0600005D RID: 93 RVA: 0x00003828 File Offset: 0x00001A28
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