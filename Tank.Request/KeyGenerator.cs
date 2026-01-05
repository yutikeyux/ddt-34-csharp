using System;
using System.Security.Cryptography; // RSA şifreleme kütüphanesi
using System.Text; // StringBuilder kütüphanesi
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace Tank.Request
{
    // Token: 0x02000045 RID: 69
    // KeyGenerator sınıfı, istemci için yeni bir RSA anahtar çifti oluşturup gönderen bir HTTP Handler'dır.
    // Önceki GetSID dosyası mevcut anahtarı verirken, bu dosya YENİ anahtar oluşturur.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class KeyGenerator : IHttpHandler
    {
        // Token: 0x0600013C RID: 316 RVA: 0x0000A5C0 File Offset: 0x000087C0
        // Gelen isteği karşılayan ve anahtar oluşturan metod
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            // --- 1. RSA SAĞLAYICISINI AYARLAMA ---
            // CspParameters, kriptografik servis sağlayıcılarının davranışını ayarlar.
            // UseMachineKeyStore: Windows'un sertifika deposunu kullanmak istediğini belirtir.
            CspParameters cryptoParams = new CspParameters();
            cryptoParams.Flags = CspProviderFlags.UseMachineKeyStore;

            // --- 2. 2048-BIT RSA ANAHTARI OLUŞTURMA ---
            // Bu işlem her istekte yeniden çalışır. Yani sürekli yeni bir anahtar üretilir.
            using (RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider(2048))
            {
                // --- 3. PARAMETRELERİ AL (HEX DÖNÜŞÜMÜ) ---
                // ExportParameters(true): 'true' parametresi hem Açık hem Özel Anahtar parametrelerini döner.
                RSAParameters rsaParams = rsaProvider.ExportParameters(true);

                // "model" (Matematiksel Modulus) byte dizisini Hex string'e çevirir.
                // ToString("X2"): Her byte'ı 2 haneli Hex formatına çevirir (Örn: 15 -> "0F").
                StringBuilder modulusHex = new StringBuilder();
                for (int i = 0; i < rsaParams.Modulus.Length; i++)
                {
                    modulusHex.Append(rsaParams.Modulus[i].ToString("X2"));
                }

                // "exponent" (Matematiksel Üs) byte dizisini Hex string'e çevirir.
                StringBuilder exponentHex = new StringBuilder();
                for (int j = 0; j < rsaParams.Exponent.Length; j++)
                {
                    exponentHex.Append(rsaParams.Exponent[j].ToString("X2"));
                }

                // --- 4. XML OLUŞTURMA ---
                XElement resultXml = new XElement("list");

                // A. Özel Anahtar (Private Key) Node'u
                // rsa.ToXmlString(true): Tüm anahtar çiftini XML formatına çevirir.
                // Bu XML istemci tarafında (Flash/Unity) RSA nesnesine dönüştürülerek şifreleme için kullanılır.
                XElement privateKeyNode = new XElement("private", new XAttribute("key", rsaProvider.ToXmlString(true)));
                resultXml.Add(privateKeyNode);

                // B. Açık Anahtar (Public Key) Node'u
                // Modulus ve Exponent ayrı Hex formatında ve string olarak gönderilir.
                // Bu, istemci tarafından kendi algoritmalarında kullanılmak için olabilir.
                XElement publicKeyNode = new XElement("public", new object[]
                {
                    new XAttribute("model", modulusHex.ToString()),
                    new XAttribute("exponent", exponentHex.ToString())
                });
                resultXml.Add(publicKeyNode);

                // --- 5. YANITI GÖNDER ---
                // Sıkıştırma yapılmadan, doğrudan XML string'i gönderilir.
                context.Response.Write(resultXml.ToString());
            }
        }

        // Token: 0x17000045 RID: 69
        // (get) Token: 0x0600013D RID: 317 RVA: 0x00003828 File Offset: 0x00001A28
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