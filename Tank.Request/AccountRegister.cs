using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness; // İş katmanı kütüphanesi
using log4net; // Loglama kütüphanesi

namespace Tank.Request
{
    // AccountRegister sınıfı, HTTP isteklerini işleyen bir Handler'dır.
    // [WebService] ve [WebServiceBinding] öznitelikleri burada işlevsiz kalabilir,
    // ancak orijinal yapı bozulmaması için korunmuştur.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class AccountRegister : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x0600000F RID: 15 RVA: 0x000036E4 File Offset: 0x000018E4
        // Gelen isteği (Request) karşılayan ve yanıt (Response) oluşturan metod
        public void ProcessRequest(HttpContext context)
        {
            // XML formatında bir sonuç nesnesi oluşturuyoruz
            XElement result = new XElement("Result");

            // Kayıt işleminin başarılı olup olmadığını tutan değişken
            bool registerResult = false;

            try
            {
                // --- 1. PARAMETRE ALIMI VE GÜVENLİĞİ ---
                // Gelen veriler (örn: URL üzerinden GET veya POST ile) URL Decode işlemine tabi tutulur.
                // Bu, Türkçe karakterlerin veya özel sembollerin doğru okunmasını sağlar.
                string username = HttpUtility.UrlDecode(context.Request["username"]);
                string password = HttpUtility.UrlDecode(context.Request["password"]);

                // KRİTİK DÜZELTME: Orijinal kodda burada "nickName" yerine tekrar "password" parametresi okunuyordu.
                // Bu ciddi bir hataydı. Burayı düzelterek doğrudan "nickName" parametresini okuyoruz.
                string nickName = HttpUtility.UrlDecode(context.Request["nickName"]);

                // --- 2. BAŞLANGIÇ DEĞERLERİ ---
                // Yeni kullanıcıya verilecek varsayılan özellikler (Hoşgeldin paketi gibi)
                bool sex = false; // Cinsiyet (False = Kadın/Belirtilmemiş)
                int money = 100;   // Başlangıç parası
                int giftoken = 100; // Başlangıç hediye tokenı
                int gold = 100;    // Başlangıç altını

                // --- 3. VERİTABANI İŞLEMİ ---
                // 'using' bloğu, PlayerBussiness nesnesi işi bitince bellekten temizlenir (Dispose).
                using (PlayerBussiness db = new PlayerBussiness())
                {
                    // Business katmanındaki RegisterUser metodunu çağırarak kullanıcıyı kaydederiz.
                    // Metot başarılıysa true, başarısızsa (kullanıcı adı varsa vb.) false döner.
                    registerResult = db.RegisterUser(username, password, nickName, sex, money, giftoken, gold);
                }
            }
            catch (Exception ex)
            {
                // Herhangi bir hata oluşursa log kaydı atarız.
                // Kayıt başarısız sayılır (registerResult false kalır).
                AccountRegister.log.Error("Kullanıcı kaydı sırasında hata:", ex);
            }
            finally
            {
                // --- 4. YANITI HAZIRLAMA (RESPONSE) ---

                // XML sonucuna nitelikler (attributes) ekliyoruz.
                // "value": Orijinal kodda sabit "vl" değeri var, istemci tarafı bu kodu kontrol ediyor olabilir.
                result.Add(new XAttribute("value", "vl"));

                // "message": Kayıt sonucunu (True/False) içerir.
                result.Add(new XAttribute("message", registerResult));

                // Yanıtın içerik türünü belirler.
                // Not: Veri XML olsa da, orijinal kod "text/plain" kullanıyor. Eski uyumluluk için korundu.
                context.Response.ContentType = "text/plain";

                // XML'i string'e çevirip ekrana (istemciye) yazar. ToString(false) ile formatlamasız düz yazılır.
                context.Response.Write(result.ToString(false));
            }
        }

        // Token: 0x17000003 RID: 3
        // (get) Token: 0x06000010 RID: 16 RVA: 0x00003828 File Offset: 0x00001A28
        // IHttpHandler arayüzünün zorunlu üyesi.
        // False döndürmek, bu sınıfın bir pool (havuz) içinde tekrar kullanılmayacağını, her isteğe yeni bir instance oluşturulacağını belirtir.
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}