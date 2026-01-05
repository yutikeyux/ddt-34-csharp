using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Ajax;
using Bussiness;

namespace Count
{
    // click sınıfı, ASP.NET Page sınıfından türetilmiştir.
    public class click : Page
    {
        // Form elemanı (Web Forms tarafından otomatik oluşturulan kısım)
        protected HtmlForm form1;

        // Sayfa yüklendiğinde çalışır.
        protected void Page_Load(object sender, EventArgs e)
        {
            // Bu sınıfın içindeki metodların AJAX ile çağrılabilmesi için tür kaydını yapıyoruz.
            Utility.RegisterTypeForAjax(typeof(click));
        }

        // [AjaxMethod] özelliği ile işaretli bu metod, JavaScript (istemci) tarafından asenkron olarak çağrılır.
        // Ziyaretçi sayfadan ayrılırken veya bir işlem yapıldığında tetiklenip verileri toplar.
        [AjaxMethod]
        public string Logoff(string App_Id, string Direct_Url, string Referry_Url, string Begin_time, string ScreenW, string ScreenH, string Color, string Flash)
        {
            HttpContext current = HttpContext.Current;
            Dictionary<string, string> clientInfos = new Dictionary<string, string>();

            try
            {
                // Uygulama Kimliğini ekle
                clientInfos.Add("Application_Id", App_Id);

                // --- IP ADRESİ TESPİTİ (Güvenlik ve Proxy Desteği Eklendi) ---
                string ip = string.Empty;

                // Load Balancer veya Proxy (Cloudflare vb.) arkasındaysa gerçek IP 'HTTP_X_FORWARDED_FOR' başlığındadır.
                if (current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                {
                    // Virgülle ayrılmış olabilir, ilk IP adresini alıyoruz
                    ip = current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(',')[0].Trim();
                }
                else
                {
                    // Proxy yoksa doğrudan istemci IP adresini al
                    ip = current.Request.UserHostAddress;
                }

                clientInfos.Add("IP", ip);
                clientInfos.Add("IPAddress", ip);

                // --- USER AGENT VE CPU ---
                string userAgent = (current.Request.UserAgent == null) ? "Bilinmiyor" : current.Request.UserAgent;
                clientInfos.Add("UserAgent", userAgent);

                // CPU Bilgisi (Eski kontrol, ancak uyumluluk için bırakıldı)
                bool flag = current.Request.ServerVariables["HTTP_UA_CPU"] == null;
                clientInfos.Add("CPU", flag ? "Bilinmiyor" : current.Request.ServerVariables["HTTP_UA_CPU"]);

                // İşletim Sistemi (Aşağıdaki güncellenen metod kullanılır)
                clientInfos.Add("OperSystem", click.GetOSNameByUserAgent(userAgent));

                // --- .NET SUPPORT ---
                // Orijinal kodda null ise ".NETCLR", doluysa "NETCLR" anahtarı kullanılıyordu (Bug). Burayı standartlaştırıyoruz.
                bool flag2 = current.Request.Browser.ClrVersion == null;
                string clrValue = flag2 ? "Desteklenmiyor" : current.Request.Browser.ClrVersion.ToString();
                clientInfos.Add(".NETCLR", clrValue); // Standart isimlendirme

                // --- TARAYICI TESPİTİ (Güncellendi) ---
                // Eski yapı: current.Request.Browser.Browser genellikle modern tarayıcılarda "Default" döner.
                // Güncel yapı: UserAgent manuel parse edilir.
                string browserName = current.Request.Browser.Browser;
                string browserVersion = current.Request.Browser.Version;
                string uaLower = userAgent.ToLower();

                if (uaLower.Contains("edg/") && uaLower.Contains("chrome")) // Edge Chromium
                    browserName = "Edge (Chromium)";
                else if (uaLower.Contains("chrome") && !uaLower.Contains("edg/"))
                    browserName = "Chrome";
                else if (uaLower.Contains("firefox"))
                    browserName = "Firefox";
                else if (uaLower.Contains("safari") && !uaLower.Contains("chrome"))
                    browserName = "Safari";
                else if (uaLower.Contains("opera") || uaLower.Contains("opr"))
                    browserName = "Opera";
                else if (uaLower.Contains("trident") || uaLower.Contains("msie"))
                    browserName = "Internet Explorer";

                clientInfos.Add("Browser", browserName + " " + browserVersion);

                // --- TARAYICI YETENEKLERİ (Legacy Kontroller - Korundu) ---
                // ActiveX (Tarih geçmiş ama hala kontrol ediliyor)
                clientInfos.Add("ActiveX", current.Request.Browser.ActiveXControls ? "True" : "False");

                clientInfos.Add("Cookies", current.Request.Browser.Cookies ? "True" : "False");
                clientInfos.Add("CSS", current.Request.Browser.SupportsCss ? "True" : "False");

                // Dil Kontrolü
                if (current.Request.UserLanguages != null && current.Request.UserLanguages.Length > 0)
                {
                    clientInfos.Add("Language", current.Request.UserLanguages[0]);
                }
                else
                {
                    clientInfos.Add("Language", "tr-TR"); // Varsayılan dil
                }

                // --- CİHAZ TİPİ (Bilgisayar / Mobil) ---
                // Eski yöntem: 'HTTP_ACCEPT' içinde 'wap' araması yetersizdi.
                // Yeni yöntem: Browser özelliği ve UserAgent analizini birleştiriyoruz.
                bool isMobile = current.Request.Browser.IsMobileDevice;
                if (!isMobile)
                {
                    // Tarayıcı nesnesi yakalamadıysa UserAgent'e bak
                    if (userAgent.Contains("Android") || userAgent.Contains("iPhone") || userAgent.Contains("iPad") || userAgent.Contains("Mobile"))
                    {
                        isMobile = true;
                    }
                }
                clientInfos.Add("Computer", isMobile ? "False" : "True");

                clientInfos.Add("Platform", current.Request.Browser.Platform);
                clientInfos.Add("Win16", current.Request.Browser.Win16 ? "True" : "False");
                clientInfos.Add("Win32", current.Request.Browser.Win32 ? "True" : "False");

                // Encoding (Sıkıştırma) bilgisi
                bool flag5 = current.Request.ServerVariables["HTTP_ACCEPT_ENCODING"] == null;
                clientInfos.Add("AcceptEncoding", flag5 ? "Yok" : current.Request.ServerVariables["HTTP_ACCEPT_ENCODING"]);

                // --- İSTEMCİDEN GELEN PARAMETRELER ---
                clientInfos.Add("Referry", Referry_Url);
                clientInfos.Add("Redirect", Direct_Url);
                clientInfos.Add("TimeSpan", Begin_time.ToString());
                clientInfos.Add("ScreenWidth", ScreenW);
                clientInfos.Add("ScreenHeight", ScreenH);
                clientInfos.Add("Color", Color);
                clientInfos.Add("Flash", Flash); // Flash öldü ama uyumluluk için tutuyoruz

                // --- VERİTABANI İŞLEMİ ---
                CountBussiness.InsertContentCount(clientInfos);
            }
            catch (Exception ex)
            {
                // Hata durumunda mesajı geri döndür
                return ex.ToString();
            }
            return "ok";
        }

        // İşletim Sistemi tespit metodunu modernize ettik.
        private static string GetOSNameByUserAgent(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent)) return "Bilinmiyor";

            string ua = userAgent;

            // Mobil Cihazlar Öncelikli Kontrol
            if (ua.Contains("Windows Phone")) return "Windows Phone";
            if (ua.Contains("Android")) return "Android";
            if (ua.Contains("iPad")) return "iPad (iOS)";
            if (ua.Contains("iPhone") || ua.Contains("iPod")) return "iPhone (iOS)";

            // Macintosh
            if (ua.Contains("Macintosh") || ua.Contains("Mac OS X"))
            {
                return "macOS";
            }

            // Windows Sürümleri (NT Çekirdeği)
            if (ua.Contains("Windows NT 10.0") || ua.Contains("Windows NT 11.0"))
                return "Windows 10 / 11";
            if (ua.Contains("Windows NT 6.3"))
                return "Windows 8.1";
            if (ua.Contains("Windows NT 6.2"))
                return "Windows 8";
            if (ua.Contains("Windows NT 6.1"))
                return "Windows 7";
            if (ua.Contains("Windows NT 6.0"))
                return "Windows Vista";
            if (ua.Contains("Windows NT 5.2"))
                return "Windows Server 2003 / XP 64-bit";
            if (ua.Contains("Windows NT 5.1"))
                return "Windows XP";
            if (ua.Contains("Windows NT 5.0"))
                return "Windows 2000";
            if (ua.Contains("Windows NT 4"))
                return "Windows NT4";
            if (ua.Contains("Windows 98") || ua.Contains("Win98"))
                return "Windows 98";
            if (ua.Contains("Windows 95"))
                return "Windows 95";

            // Linux Türevleri
            if (ua.Contains("Ubuntu"))
                return "Ubuntu";
            if (ua.Contains("Linux"))
                return "Linux";
            if (ua.Contains("CrOS"))
                return "Chrome OS";

            // Diğerleri
            if (ua.Contains("Unix"))
                return "UNIX";
            if (ua.Contains("SunOS"))
                return "SunOS";

            return "Bilinmeyen İşletim Sistemi";
        }
    }
}