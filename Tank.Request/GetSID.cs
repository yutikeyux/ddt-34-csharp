using System;
using System.Configuration; // 'ConfigurationManager' yerine güncel sınıf
using System.Security.Cryptography; // RSA şifreleme kütüphanesi
using System.Web;
using System.Web.SessionState; // Oturum durumu desteği
using System.Xml.Linq;

namespace Tank.Request
{
    // Token: 0x02000039 RID: 57
    // GetSID sınıfı, güvenli oturum (Session ID) başlatması için RSA açık anahtarını sağlayan bir HTTP Handler'dır.
    public class GetSID : IHttpHandler, IRequiresSessionState
    {
        // Token: 0x0600010C RID: 268 RVA: 0x000092A0 File Offset: 0x000074A0
        // Gelen isteği karşılayan metod
        public void ProcessRequest(HttpContext context)
        {
            // --- 1. RSA SAĞLAYICI YAPILANDIRMASI (CSP) ---
            // CspParameters, kriptografik servis sağlayıcılarının davranışını ayarlar.
            CspParameters cryptoParams = new CspParameters();
            // UseMachineKeyStore: Windows'un sertifika deposunu kullanmak istediğini belirtir.
            cryptoParams.Flags = CspProviderFlags.UseMachineKeyStore;

            // --- 2. RSA SERVİS SAĞLAYICIYI OLUŞTURMA ---
            // 2048 bit RSA anahtarı oluşturur.
            using (RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(2048))
            {
                // --- 3. ÖZEL ANAHTAR YÜKLEME ---
                // Web.config dosyasında tanımlı olan "privateKey" (Özel Anahtar) parametresini alır.
                // Bu anahtar, sunucunun daha önce oluşturduğu ve sakladığı XML formatındaki anahtardır.
                // DÜZELTME: Orijinal kodda eski "ConfigurationManager" kullanılıyordu, güncellendi.
                string privateKeyXml = ConfigurationManager.AppSettings["privateKey"];

                if (!string.IsNullOrEmpty(privateKeyXml))
                {
                    // Özel anahtarı servis sağlayıcısına yükle.
                    // Bu işlem, RSA servisinin anahtar çiftini (Public/Private) bu anahtarla eşleştirmesini sağlar.
                    rsaProvider.FromXmlString(privateKeyXml);
                }

                // --- 4. AÇIK ANAHTARI (PARAMETRELER) DIŞA AKTARMA ---
                // ExportParameters(false): 'false' parametresi sadece Açık Anahtar (Public Key) parametrelerini döner.
                // Private Key bilgisi dışarı çıkarılmaz.
                RSAParameters rsaParams = rsaProvider.ExportParameters(false);

                // --- 5. YANIT XML'İNİ OLUŞTURMA ---
                XElement resultXml = new XElement("result", new object[]
                {
                    // "m1" (Modulus): Asal sayı (N) ve Üs (Mod) bilgisini içeren bileşenin Base64 string'i.
                    new XAttribute("m1", Convert.ToBase64String(rsaParams.Modulus)),
                    
                    // "m2" (Exponent): Açık Üs bilgisinin Base64 string'i.
                    new XAttribute("m2", Convert.ToBase64String(rsaParams.Exponent))
                });

                // --- 6. YANITI GÖNDERME ---
                // Sıkıştırma yapılmadan, doğrudan metin formatında (XML) gönderir.
                context.Response.ContentType = "text/plain";
                context.Response.Write(resultXml.ToString());
            }
        }

        // Token: 0x1700003B RID: 59
        // (get) Token: 0x0600010D RID: 269 RVA: 0x00003828 File Offset: 0x00001A28
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