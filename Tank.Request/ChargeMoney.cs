using System;
using System.Configuration; // 'ConfigurationManager' yerine güncel sınıf
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using Bussiness; // İş katmanı kütüphanesi
using Bussiness.CenterService; // Merkez servisi (WCF) kütüphanesi
using Bussiness.Interface; // Arayüz yardımcı kütüphanesi
using log4net; // Loglama kütüphanesi
using SqlDataProvider.Data; // Veritabanı veri yapıları

namespace Tank.Request
{
    // Token: 0x02000014 RID: 20
    // ChargeMoney sınıfı, oyuncuların para yükleme işlemlerini işleyen bir Web Forms sayfasıdır (Page).
    public class ChargeMoney : Page
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x17000011 RID: 17
        // (get) Token: 0x06000050 RID: 80 RVA: 0x0000226B File Offset: 0x0000046B
        // Web.config dosyasında tanımlı "ChargeIP" ayarını okuyan özellik.
        // DÜZELTME: Orijinal kodda eski "ConfigurationManager" kullanılıyordu, "ConfigurationManager" ile güncellendi.
        public static string GetChargeIP
        {
            get
            {
                return ConfigurationManager.AppSettings["ChargeIP"];
            }
        }

        // Token: 0x06000051 RID: 81 RVA: 0x00004C38 File Offset: 0x00002E38
        // Sayfa yüklendiğinde çalışır (Ödeme geri çağrısı / Callback olarak çalışır)
        protected void Page_Load(object sender, EventArgs e)
        {
            // Dönüş kodu (Sonuç)
            // 1: Başlangıç/Hata, 0: Başarılı, 2: Kullanıcı yok, 3: Para tutarı hatalı, 5: IP yetkisiz
            int resultCode = 1;

            try
            {
                // Önce kodu 10 yapıyor, sonradan durumlar güncellenecek.
                resultCode = 10;

                string clientIP = this.Context.Request.UserHostAddress;

                // --- 1. GÜVENLİK KONTROLÜ ---
                // İstek yapan IP, ödeme sisteminin IP listesinde mi?
                if (!ChargeMoney.ValidLoginIP(clientIP))
                {
                    resultCode = 5; // Yetkisiz IP
                }
                else
                {
                    // --- 2. PARAMETRELERİ ALMA ---
                    // İçerik (Şifrelenmiş ödeme verisi)
                    string content = HttpUtility.UrlDecode(base.Request["content"]);

                    // Site (Ödeme platformu bilgisi)
                    string site = (base.Request["site"] == null) ? "" : HttpUtility.UrlDecode(base.Request["site"]).ToLower();

                    // NOT: Parametre adı "nickname" ancak gelen veri aslında "UserID" (Oyuncu ID'si) olarak int'e çevriliyor.
                    // Orijinal kodda böyle kullanılmış, uyumluluk için değiştirmedik.
                    int requestUserID = Convert.ToInt32(HttpUtility.UrlDecode(base.Request["nickname"]));

                    // --- 3. ŞİFRE ÇÖZME VE DOĞRULAMA ---
                    // Arayüz üzerinden şifreli içeriği çöz
                    string[] decryptedData = BaseInterface.CreateInterface().UnEncryptCharge(content, ref resultCode, site);

                    // Çözülen verinin uzunluğu kontrolü (Beklenen parametre sayısı: ChargeID, User, Money, Currency, NeedMoney)
                    if (decryptedData.Length > 5)
                    {
                        string chargeID = decryptedData[0]; // İşlem ID
                        string username = decryptedData[1].Trim(); // Kullanıcı Adı
                        int money = int.Parse(decryptedData[2]); // Miktar (Game Currency?)
                        string currency = decryptedData[3]; // Para Birimi (TL, USD vb.)
                        decimal needMoney = decimal.Parse(decryptedData[4]); // Ödenen Gerçek Para

                        // --- 4. VERİ DOĞRULAMA ---
                        if (!string.IsNullOrEmpty(username))
                        {
                            // Site'ye göre oyuncu adını normalize et (Harf duyarlılığı vb.)
                            string normalizedUsername = BaseInterface.GetNameBySite(username, site);

                            // Miktar sıfırdan büyük olmalı
                            if (money > 0)
                            {
                                // --- 5. VERİTABANI İŞLEMİ ---
                                using (PlayerBussiness pb = new PlayerBussiness())
                                {
                                    // Yükleme işlemini veritabanına ekle (Başarılıysa true döner)
                                    int dbUserID = 0;
                                    DateTime transactionTime = DateTime.Now;

                                    bool success = pb.AddChargeMoney(chargeID, normalizedUsername, money, currency, needMoney, ref dbUserID, ref resultCode, transactionTime, clientIP, requestUserID);

                                    if (success)
                                    {
                                        resultCode = 0; // Başarılı!

                                        // --- 6. MERKEZ SERVİSİNE HABER VER ---
                                        // Oyun sunucusunun/Center'ın oyuncunun parasını güncellemesini sağla
                                        using (CenterServiceClient serviceClient = new CenterServiceClient())
                                        {
                                            serviceClient.ChargeMoney(dbUserID, chargeID);

                                            // --- 7. İSTATİSTİK LOG ---
                                            using (PlayerBussiness pbLog = new PlayerBussiness())
                                            {
                                                PlayerInfo userInfo = pbLog.GetUserSingleByUserID(dbUserID);

                                                if (userInfo != null)
                                                {
                                                    // Oyuncu bilgisi varsa, cinsiyet bilgisiyle beraber logla
                                                    StaticsMgr.Log(transactionTime, normalizedUsername, userInfo.Sex, money, currency, needMoney);
                                                }
                                                else
                                                {
                                                    // Oyuncu bulunamazsa (rare case), varsayılan bir log at
                                                    StaticsMgr.Log(transactionTime, normalizedUsername, true, money, currency, needMoney);

                                                    // Hata logla: İşlem başarılı ama kullanıcı info null geldi
                                                    ChargeMoney.log.Error("ChargeMoney_StaticsMgr:Player is null!");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                resultCode = 3; // Para tutarı hatalı
                            }
                        }
                        else
                        {
                            resultCode = 2; // Kullanıcı adı yok
                        }
                    }
                    // Else: decryptedData.Length <= 5 ise resultCode 'UnEncryptCharge' metodu tarafından zaten set edilmiş demektir.
                }
            }
            catch (Exception ex)
            {
                // Genel hata loglama
                ChargeMoney.log.Error("ChargeMoney işlem hatası:", ex);
            }

            // --- SONUÇ ---
            // Yanıt sadece "HataKodu" + "IPAdresi" formatındadır.
            base.Response.Write(resultCode.ToString() + this.Context.Request.UserHostAddress);
        }

        // Token: 0x06000052 RID: 82 RVA: 0x00004EE8 File Offset: 0x000030E8
        // Gelen IP adresinin yetkili ödeme IP'leri arasında olup olmadığını kontrol eder
        public static bool ValidLoginIP(string ip)
        {
            string chargeIPs = ChargeMoney.GetChargeIP;

            // Mantık:
            // 1. ChargeIP boşsa -> Hepsine izin ver (1).
            // 2. ChargeIP doluysa -> Gelen IP listede varsa (1), yoksa (0).
            int isValid = string.IsNullOrEmpty(chargeIPs) ? 1 : (chargeIPs.Split(new char[] { '|' }).Contains(ip) ? 1 : 0);

            return isValid != 0;
        }
    }
}