using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Bussiness.Interface; // BaseInterface arayüzü kütüphanesi

namespace Tank.Request
{
    // Token: 0x02000055 RID: 85
    // LoginTest sınıfı, giriş işlemini yerel olarak test etmek için kullanılan bir Web Form sayfasıdır.
    public class LoginTest : Page
    {
        // Token: 0x04000057 RID: 87
        // Web Form sayfası için gerekli Form elemanı
        protected HtmlForm form1;

        // Token: 0x06000184 RID: 388 RVA: 0x0000C934 File Offset: 0x0000A734
        // Sayfa yüklendiğinde çalışır
        protected void Page_Load(object sender, EventArgs e)
        {
            // --- 1. TEST PARAMETRELERİ ---
            // Test kulanıcı adı
            string username = "onelife";
            // Test şifresi
            string password = "733789";
            // Test zaman damgası (Unix Timestamp)
            int timestamp = 1255165271;
            // Giriş anahtarı (Salt veya özel string)
            string loginKey = "yk-MotL-qhpAo88-7road-mtl55dantang-login-logddt777";

            // --- 2. GİRİŞ HASH'İ (KEY) OLUŞTURMA ---
            // BaseInterface.md5 metodu, verilen string'in MD5 hash'ini alır.
            // Değer: Username + Password + Timestamp + LoginKey birleştirilir.
            string hash = BaseInterface.md5(username + password + timestamp.ToString() + loginKey);

            // --- 3. İÇERİK STRİNGİNİ OLUŞTURMA ---
            // İstemci formatı: "KullanıcıAdı|Şifre|Zaman|Hash"
            string concatenatedString = string.Concat(new string[] { username, "|", password, "|", timestamp.ToString(), "|", hash });

            // URL Encode edilmiş content parametresi
            string encodedContent = "content=" + HttpUtility.UrlEncode(concatenatedString);

            // --- 4. SANAL İSTEK (LOCAL REQUEST) ---
            // Hedef URL oluşturulur. 'CreateLogin.aspx' endpoint'ine gider.
            // localhost:728, yerel sunucunun 728. portudur.
            string targetUrl = "http://localhost:728/CreateLogin.aspx?content=" + HttpUtility.UrlEncode(concatenatedString);

            // BaseInterface.RequestContent metodu, oluşturulan URL'e bir HTTP isteği simüle eder.
            // Sunucunun döndüğü yanıtı (XML veya String) 'serverResponse' değişkenine alır.
            string serverResponse = BaseInterface.RequestContent(targetUrl);

            // --- 5. YANITI ---
            // Login endpoint'ten gelen cevabı ekrana yazar.
            base.Response.Write(serverResponse);
        }
    }
}