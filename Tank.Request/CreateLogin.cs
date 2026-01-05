using System;
using System.Configuration; // 'ConfigurationManager' yerine güncel sınıf
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using Bussiness.Interface; // Arayüz yardımcı kütüphanesi
using log4net; // Loglama kütüphanesi

namespace Tank.Request
{
    // Token: 0x02000027 RID: 39
    // CreateLogin sınıfı, oyuncu giriş işlemlerini (Login) işleyen bir Web Form sayfasıdır.
    public class CreateLogin : Page
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x17000023 RID: 35
        // (get) Token: 0x0600009D RID: 157 RVA: 0x00006C58 File Offset: 0x00004E58
        // Web.config dosyasında tanımlı "LoginIP" ayarını okuyan özellik.
        // DÜZELTME: Orijinal kodda eski "ConfigurationManager" kullanılıyordu, "ConfigurationManager" ile güncellendi.
        public static string GetLoginIP
        {
            get
            {
                return ConfigurationManager.AppSettings["LoginIP"];
            }
        }

        // Token: 0x0600009E RID: 158 RVA: 0x00006C7C File Offset: 0x00004E7C
        // Gelen IP adresinin yetkili giriş IP'leri arasında olup olmadığını kontrol eder
        public static bool ValidLoginIP(string ip)
        {
            string loginIPs = CreateLogin.GetLoginIP;
            // Mantık:
            // 1. LoginIP listesi boşsa herkese izin ver (True).
            // 2. LoginIP doluysa, Gelen IP'nin listede olup olmadığına bakar.
            return string.IsNullOrEmpty(loginIPs) || loginIPs.Split(new char[] { '|' }).Contains(ip);
        }

        // Token: 0x0600009F RID: 159 RVA: 0x00006CC0 File Offset: 0x00004EC0
        // Sayfa yüklendiğinde çalışır (Giriş isteği geldiğinde)
        protected void Page_Load(object sender, EventArgs e)
        {
            // Dönüş kodu (Sonuç)
            // 0: Başarılı, 1: Başarısız, -91010: Kimlik Bilgisi Boş
            int resultCode = 1;

            try
            {
                // --- 1. PARAMETRELERİ ALMA ---
                // İstemciden gelen şifrelenmiş içerik
                string encryptedContent = HttpUtility.UrlDecode(base.Request["content"]);

                // Hangi site/platform üzerinden giriş yapıldığı
                string siteKey = (base.Request["site"] == null) ? "" : HttpUtility.UrlDecode(base.Request["site"]).ToLower();

                // --- 2. ARAYÜZ (INTERFACE) KULLANIMI ---
                BaseInterface gameInterface = BaseInterface.CreateInterface();

                // Şifreli içeriği çöz (UnEncryptLogin)
                // Bu metot 'resultCode' parametresini 'ref' ile alarak, şifre çözme başarısızsa kodu günceller.
                string[] decryptedData = gameInterface.UnEncryptLogin(encryptedContent, ref resultCode, siteKey);

                // Çözülen verinin uzunluğu kontrol edilir (En az 4 parça bekleniyor: Name, Pass, ...)
                if (decryptedData.Length > 3)
                {
                    string username = decryptedData[0].Trim().ToLower();
                    string password = decryptedData[1].Trim().ToLower();

                    // --- 3. KULLANICI DOĞRULAMA ---
                    bool isValidCredentials = !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password);

                    if (isValidCredentials)
                    {
                        // Site prefix'ine göre kullanıcı adını normalize et
                        // Örn: Siteden gelen "Ahmet" -> DB'deki "S1_Ahmet" olabiliyor olabilir.
                        username = BaseInterface.GetNameBySite(username, siteKey);

                        // --- 4. GİRİŞ İŞLEMİ ---
                        // PlayerManager.Add metodu oyuncuyu online listesine ekler veya giriş yapar.
                        PlayerManager.Add(username, password);

                        resultCode = 0; // Başarılı
                    }
                    else
                    {
                        // Kullanıcı adı veya şifre boş geldi
                        resultCode = -91010;
                    }
                }
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                CreateLogin.log.Error("CreateLogin işlem hatası:", ex);
            }

            // --- 5. YANITI GÖNDER ---
            // Sadece sayısal sonuç kodunu döner.
            base.Response.Write(resultCode);
        }
    }
}