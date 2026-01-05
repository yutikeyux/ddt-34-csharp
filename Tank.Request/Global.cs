using System;
using System.Web;
using Bussiness; // İş katmanı kütüphanesi
using log4net.Config; // Log4net XML Konfigürasyon kütüphanesi

namespace Tank.Request
{
    // Token: 0x0200003C RID: 60
    // Global sınıfı, ASP.NET uygulamasının (Web Site/Backend) giriş noktasıdır.
    // IIS sunucusu uygulama başlatıldığında bu sınıfın metodları tetiklenir.
    public class Global : HttpApplication
    {
        // Token: 0x06000117 RID: 279 RVA: 0x00009738 File Offset: 0x00007938
        // Uygulama ilk başladığında çalışan metod.
        // Genellikle AppPool yeniden başladığında veya sunucu ilk kez başlatıldığında tetiklenir.
        protected void Application_Start(object sender, EventArgs e)
        {
            // Uygulamanın fiziksel kök yolunu al (Örn: C:\inetpub\wwwroot\)
            string rootPath = base.Server.MapPath("~");

            // --- SİSTEM BİLEŞENLERİNİ BAŞLATMA ---

            // 1. Dil Yönetimi (LanguageMgr)
            // Oyunun çoklu dil desteğini sağlayan dil dosyalarını yükler.
            LanguageMgr.Setup(rootPath);

            // 2. XML Konfigürasyonu (XmlConfigurator)
            // Muhtemelen Log4net (Loglama) ayarlarının bulunduğu dosyayı okur ve log sistemini başlatır.
            XmlConfigurator.Configure();

            // 3. Statik Yöneticisi (StaticsMgr)
            // Uygulama genelinde kullanılan statik verileri, haritaları veya yapılandırma dosyalarını yükler.
            StaticsMgr.Setup();

            // 4. Oyuncu Yöneticisi (PlayerManager)
            // Oyuncuların online listesini tutan, giriş/çıkış işlemlerini yöneten sistemi başlatır.
            PlayerManager.Setup();
        }

        // Token: 0x06000118 RID: 280 RVA: 0x00002670 File Offset: 0x00000870
        // Yeni bir kullanıcı siteye girip oturum (Session) başlattığında çalışan metod.
        // Orijinal kodda boş bırakılmış.
        protected void Session_Start(object sender, EventArgs e)
        {
            // Oturum başlangıç işlemleri (Kullanıcı sayacı vb.) yapılabilir.
        }

        // Token: 0x06000119 RID: 281 RVA: 0x00002670 File Offset: 0x00000870
        // Sunucu gelen her bir HTTP isteğini işlemeden hemen önce çalışan metod.
        // Bu metod genellikle URL Rewrite (Adres yenileme), Global Header ekleme veya IP banlama için kullanılır.
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // İstek başlangıç işlemleri (Örn: HTTP Header değiştirme).
        }

        // Token: 0x0600011A RID: 282 RVA: 0x00002670 File Offset: 0x00000870
        // Kimlik doğrulama (Authentication) aşamasında çalışan metod.
        // Kullanıcının oturum açıp açamayacağı kontrol edilmek istenirse buraya yazılır.
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            // Özel oturum açma mantığı (Custom Authentication).
        }

        // Token: 0x0600011B RID: 283 RVA: 0x00002670 File Offset: 0x00000870
        // Uygulama içindeki hiçbir kod hatası (Exception) yakalanmadığında çalışan metod.
        // Tüm global hataları (Error 500 vb.) buradan yakalayıp loglayabiliriz.
        protected void Application_Error(object sender, EventArgs e)
        {
            // Hata yakalama ve loglama:
            // Exception ex = Server.GetLastError();
            // log.Error("Global Hata:", ex);
        }

        // Token: 0x0600011C RID: 284 RVA: 0x00002670 File Offset: 0x00000870
        // Bir kullanıcının oturumu (Session) sona erdiğinde (Zaman aşımı veya Logout) çalışan metod.
        // Orijinal kodda boş bırakılmış.
        protected void Session_End(object sender, EventArgs e)
        {
            // Oturum bitiş işlemleri.
        }

        // Token: 0x0600011D RID: 285 RVA: 0x00002673 File Offset: 0x00000873
        // Uygulama kapanırken çalışan metod.
        // AppPool Recycle, IIS Reset veya kodun derlenmiş haliyle değişmesi durumunda tetiklenir.
        protected void Application_End(object sender, EventArgs e)
        {
            // Statik kaynakları durdur ve bellekteki verileri temizle.
            // Bu, memory leak (bellek sızıntısı) önlemek için önemlidir.
            StaticsMgr.Stop();
        }
    }
}