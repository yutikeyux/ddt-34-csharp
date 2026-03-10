using System;
using System.Text;
using System.Web;
using System.Web.Services;
using System.IO; // Dosya işlemleri için eklendi

namespace Tank.Request.CelebList
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CreateAllCeleb : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string clientIP = context.Request.UserHostAddress;
            bool flag = csFunction.ValidAdminIP(clientIP);

            // --- LOG KAYIT İŞLEMİ ---
            try
            {
                // Log dosyasının yolu (Site kök dizininde oluşturulur)
                string logPath = context.Server.MapPath("~/ip_log.txt");

                // Log satırı: Tarih - IP - Hangi Sayfa - Durum
                string logText = string.Format("[{0}] IP: {1} - Sayfa: CreateAllCeleb - Durum: {2}{3}",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    clientIP,
                    flag ? "Başarılı (Admin)" : "Başarısız (Yetkisiz)",
                    Environment.NewLine);

                // Dosyaya ekleme yapar
                File.AppendAllText(logPath, logText);
            }
            catch (Exception)
            {
                // Hata durumunda işleme devam et
            }
            // --- LOG KAYIT İŞLEMİ SONU ---

            if (flag)
            {
                StringBuilder Build = new StringBuilder();
                Build.Append(CelebByGpList.Build());
                Build.Append(CelebByDayGPList.Build());
                Build.Append(CelebByWeekGPList.Build());
                Build.Append(CelebByOfferList.Build());
                Build.Append(CelebByDayOfferList.Build());
                Build.Append(CelebByWeekOfferList.Build());
                Build.Append(CelebByDayFightPowerList.Build());
                Build.Append(CelebByConsortiaRiches.Build());
                Build.Append(CelebByConsortiaDayRiches.Build());
                Build.Append(CelebByConsortiaWeekRiches.Build());
                Build.Append(CelebByConsortiaHonor.Build());
                Build.Append(CelebByConsortiaDayHonor.Build());
                Build.Append(CelebByConsortiaWeekHonor.Build());
                Build.Append(CelebByConsortiaLevel.Build());
                Build.Append(CelebByDayBestEquip.Build());
                Build.Append(celebbyconsortiafightpower.Build());
                context.Response.ContentType = "text/plain";
                context.Response.Write(Build.ToString());
            }
            else
            {
                context.Response.Write("Yönetici Adresi Geçersiz! Loglarda: " + clientIP);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}