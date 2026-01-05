using System;
using System.Configuration; // 'ConfigurationManager' yerine güncel sınıf
using System.Web;
using System.Web.Services;
using Bussiness.Interface; // Arayüz yardımcı kütüphanesi

namespace Tank.Request
{
    // Token: 0x02000015 RID: 21
    // ChargeTest sınıfı, para yükleme işlemlerini yönlendirerek test etmek için kullanılan bir HTTP Handler'dır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ChargeTest : IHttpHandler
    {
        // Token: 0x06000055 RID: 85 RVA: 0x00004F30 File Offset: 0x00003130
        // Gelen isteği karşılayan ve test URL'ini oluşturup tetikleyen metod
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                // --- 1. PARAMETRELERİ ALMA ---
                string chargeID = context.Request["chargeID"];
                string userName = context.Request["userName"];
                int money = int.Parse(context.Request["money"]);
                string payWay = context.Request["payWay"];
                decimal needMoney = decimal.Parse(context.Request["needMoney"]);
                string nickname = (context.Request["nickname"] == null) ? "" : HttpUtility.UrlDecode(context.Request["nickname"]);

                // NOT: 'chargekey' değişkeni alınıyor ancak kodun ilerleyen kısmında kullanılmıyor.
                // İmza (Signature) oluşturma aşamasında sunucu taraflı 'key' kullanılıyor.
                string chargekey = HttpUtility.UrlDecode(context.Request["chargekey"]);

                // --- 2. İMLEMEYİ OLUŞTURMA (SIGNATURE) ---
                // 'site' değişkeni burada boş string ile başlatılmış.
                // Bu, kodun her zaman 'else' bloğuna girerek 'BaseInterface.GetChargeKey' kullanmasını sağlar.
                // Yani özel bir site anahtarı yerine genel anahtar kullanılır.
                string site = "";

                string secretKey = string.Empty;

                // Orijinal kodda bu blok 'site' boş olduğu için asla çalışmaz.
                // Ancak olası bir gelecek güncelleme için korundu.
                if (!string.IsNullOrEmpty(site))
                {
                    // Site bazlı özel anahtar alma (Örn: "ChargeKey_a")
                    secretKey = ConfigurationManager.AppSettings[string.Format("ChargeKey_a", site)];
                }
                else
                {
                    // Varsayılan genel şifreleme anahtarı
                    secretKey = BaseInterface.GetChargeKey;
                }

                // Parametreleri ve anahtarı birleştirip MD5 ile hash'leyerek imzayı oluşturur
                string md5Signature = BaseInterface.md5(string.Concat(new string[]
                {
                    chargeID,
                    userName,
                    money.ToString(),
                    payWay,
                    needMoney.ToString(),
                    secretKey
                }));

                // --- 3. TARGET URL OLUŞTURMA ---
                // Orijinal kodda hardcoded (sabit) bir domain adresi var.
                // Bu test aracının gerçek canlı sunucuya veya belirli bir test sunucusuna istek attığını gösterir.
                string targetUrl = string.Concat(new string[]
                {
                    "https://ddt-quest-s1.trbombom.com/ddt-quest-s1/ChargeMoney.aspx?content=",
                    chargeID,
                    "|",
                    userName,
                    "|",
                    money.ToString(),
                    "|",
                    payWay,
                    "|",
                    needMoney.ToString(),
                    "|",
                    md5Signature
                });

                // Site ve Nickname parametrelerini URL'e ekle
                targetUrl = targetUrl + "&site=" + site;
                targetUrl = targetUrl + "&nickname=" + HttpUtility.UrlEncode(nickname);

                // --- 4. İSTEĞİ GÖNDER ---
                // Oluşturulan URL'e bir HTTP isteği atar ve yanıtı istemciye döndür.
                context.Response.Write(BaseInterface.RequestContent(targetUrl));
            }
            catch (Exception ex)
            {
                // Hata olursa hata mesajını ekrana yazar
                context.Response.Write(ex.ToString());
            }
        }

        // Token: 0x17000012 RID: 18
        // (get) Token: 0x06000056 RID: 86 RVA: 0x00003828 File Offset: 0x00001A28
        // IHttpHandler arayüzünün zorunlu üyesi.
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}