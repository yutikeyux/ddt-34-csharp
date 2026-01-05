using System;
using System.Reflection;
using System.Web;
using System.Web.UI;
using Bussiness; // İş katmanı kütüphanesi
using Bussiness.CenterService; // Merkez servisi (WCF) kütüphanesi
using log4net; // Loglama kütüphanesi
using SqlDataProvider.Data; // Veritabanı veri yapıları

namespace Tank.Request
{
    // Token: 0x02000016 RID: 22
    // ChargeToUser sınıfı, belirtilen kullanıcıya ID ile para yüklemek ve işlemin sonucunu döndürmek için kullanılır.
    public class ChargeToUser : Page
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x06000058 RID: 88 RVA: 0x00005140 File Offset: 0x00003340
        // Sayfa yüklendiğinde çalışır
        protected void Page_Load(object sender, EventArgs e)
        {
            // Varsayılan sonuç (Hata durumunda dönecek)
            string result = "false";

            try
            {
                // --- 1. PARAMETRELERİ ALMA ---
                // İşlem yapılacak kullanıcının ID'si ve İşlem ID'si (ChargeID) alınıyor
                int userId = Convert.ToInt32(HttpUtility.UrlDecode(base.Request["userID"]));
                string chargeId = HttpUtility.UrlDecode(base.Request["chargeID"]);

                // --- 2. MERKEZ SERVİSİNE İŞLEM EMRİ ---
                // Oyun sunucusunun/Center'ın kullanıcının hesabına parayı eklemesini sağla
                using (CenterServiceClient serviceClient = new CenterServiceClient())
                {
                    // WCF servisi aracılığıyla para yükleme işlemini tetikle
                    serviceClient.ChargeMoney(userId, chargeId);

                    // --- 3. SONUÇ DOĞRULAMA ---
                    // İşlemden sonra kullanıcının veritabanında hala var olup olmadığını kontrol et (Teyit amaçlı)
                    using (PlayerBussiness pb = new PlayerBussiness())
                    {
                        PlayerInfo userInfo = pb.GetUserSingleByUserID(userId);

                        // Kullanıcı bilgisi varsa işlem başarılı
                        if (userInfo != null)
                        {
                            result = "ok";
                        }
                        else
                        {
                            // İşlem ID geçerli ama kullanıcı bulunamazsa hata durumu
                            result = "null";
                            ChargeToUser.log.Error("ChargeMoney_StaticsMgr:Player is null!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Herhangi bir hata olursa logla
                ChargeToUser.log.Error("ChargeMoney işlem hatası:", ex);
            }

            // Sonucu ekrana (istemciye) yaz
            base.Response.Write(result);
        }
    }
}