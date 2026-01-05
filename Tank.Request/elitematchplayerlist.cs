using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using log4net; // Loglama kütüphanesi

namespace Tank.Request
{
    // Token: 0x02000030 RID: 48
    // elitematchplayerlist sınıfı, Elit Maç (Elite Match) oyuncularını listelemek için kullanılan bir HTTP Handler'dır.
    // Not: Sınıf isminde "EliteMatch" yerine "elitematch" (yazım hatası) kullanılmış.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class elitematchplayerlist : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x060000E4 RID: 228 RVA: 0x00002581 File Offset: 0x00000781
        // Gelen isteği karşılayan metod
        public void ProcessRequest(HttpContext context)
        {
            // İsteği hazırlayan metodu çağır ve sonucu yaz
            context.Response.Write(elitematchplayerlist.Build(context));
        }

        // Token: 0x060000E5 RID: 229 RVA: 0x00008358 File Offset: 0x00006558
        // İlk aşama IP kontrolü yapan metod
        public static string Build(HttpContext context)
        {
            // MANTIK AÇIKLAMASI:
            // csFunction.ValidAdminIP(...) IP yetkiliyse true döner.
            // Ancak kodun başına '!' (Değil) işareti konmuş: !ValidAdminIP
            // Yani: IP Yetkili DEĞİLSE -> true, IP Yetkiliyse -> false döner.
            bool isInvalidIP = !csFunction.ValidAdminIP(context.Request.UserHostAddress);

            string response;

            if (isInvalidIP)
            {
                // IP Yetkili DEĞİLSE, hata mesajı döner
                response = "elitematchplayerlist Fail!";
            }
            else
            {
                // IP Yetkiliyse, listeyi oluşturan metodu çağır
                response = elitematchplayerlist.Build();
            }
            return response;
        }

        // Token: 0x060000E6 RID: 230 RVA: 0x00008390 File Offset: 0x00006590
        // İkinci aşama: Listeyi oluşturan static metod
        public static string Build()
        {
            // Bu sınıfın içinde veritabanı sorgusu yapmaz.
            // csFunction sınıfında daha önce analiz ettiğimiz "BuildEliteMatchPlayerList" metodunu çağırır.
            // Bu metot, oyuncuları 40'ın altı ve üstü olarak ayırıp dosyaya yazar ve sonuç döner.
            return csFunction.BuildEliteMatchPlayerList("elitematchplayerlist");
        }

        // Token: 0x17000031 RID: 49
        // (get) Token: 0x060000E7 RID: 231 RVA: 0x00003828 File Offset: 0x00001A28
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