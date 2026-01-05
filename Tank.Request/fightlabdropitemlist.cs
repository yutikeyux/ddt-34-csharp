using System;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness; // İş katmanı kütüphanesi
using log4net; // Loglama kütüphanesi
using SqlDataProvider.Data; // Veritabanı veri yapıları

namespace Tank.Request
{
    // Token: 0x02000038 RID: 56
    // fightlabdropitemlist sınıfı, deneysel savaş (Fight Lab) eşyalarını listeler.
    public class fightlabdropitemlist : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x06000107 RID: 263 RVA: 0x00009070 File Offset: 0x00007270
        // Gelen isteği karşılayan metod
        public void ProcessRequest(HttpContext context)
        {
            // İstek yapanın IP adresi yetkili mi kontrol et
            if (csFunction.ValidAdminIP(context.Request.UserHostAddress))
            {
                // Yetkiliyse veriyi çekip yanıt olarak gönder
                context.Response.Write(fightlabdropitemlist.Build(context));
            }
            else
            {
                // Yetkili değilse espirili hata mesajı döndür
                context.Response.Write("Tabi Efendim!");
            }
        }

        // Token: 0x06000108 RID: 264 RVA: 0x000090BC File Offset: 0x000072BC
        // Veritabanından eşyaları çekip filtreleyen ve XML oluşturan metod
        public static string Build(HttpContext context)
        {
            bool isSuccess = false;
            string message = "Fail!";

            // Kök XML elementini oluştur
            XElement resultXml = new XElement("Result");

            try
            {
                // --- 1. FİLTRE ID LİSTESİ (COPYIDS) ---
                // Bu dizi sadece burada tanımlıdır ve bir White List (İzinli Listesi) görevi görür.
                // Sadece bu ID'ler listelenir.
                int[] filterIds = new int[]
                {
                    10000, 10001, 10002, 10010, 10011, 10012,
                    10020, 10021, 10022, 10030, 10031, 10032,
                    10040, 10041, 10042
                };

                // --- 2. VERİTABANI İŞLEMLERİ ---
                using (ProduceBussiness db = new ProduceBussiness())
                {
                    // Tüm drop item'larını veritabanından çek
                    DropItem[] allDropItems = db.GetAllDropItems();

                    // Her bir drop item'ini döngüye al
                    foreach (DropItem dropItem in allDropItems)
                    {
                        // Filtreleme: Eğer item'in ID'si yukarıdaki 'filterIds' listesinde yoksa atla.
                        if (filterIds.Contains(dropItem.DropId))
                        {
                            // --- 3. XML OLUŞTURMA VE ID PARSİNGİ ---
                            // Orijinal kodda DropId (int) string'e çevrilip parçalanıyor.
                            // Bu muhtemelen tek bir integer içinde birden fazla bilgi (ID ve Zorluk) paketlenmiş olabilir.
                            string dropIdStr = dropItem.DropId.ToString();

                            // Substring(0, 4) -> İlk 4 hane ID'yi temsil eder.
                            string parsedID = dropIdStr.Substring(0, 4);

                            // Substring(4, 1) -> 5. hane Zorluk ("Easy") bilgisini temsil eder (muhtemelen 0: Normal, 1: Kolay vb.)
                            string parsedEasy = dropIdStr.Substring(4, 1);

                            resultXml.Add(new XElement("Item", new object[]
                            {
                                new XAttribute("ID", parsedID),
                                new XAttribute("Easy", parsedEasy),
                                new XAttribute("AwardItem", dropItem.ItemId),
                                new XAttribute("Count", dropItem.BeginData)
                            }));
                        }
                    }
                }

                isSuccess = true;
                message = "Success!";
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                fightlabdropitemlist.log.Error("fightlabdropitemlist yüklenirken hata:", ex);
            }

            // XML'e genel durum bilgilerini (value ve message) ekle
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // --- NOT: Orijinal kodda burası iki kez çağrılıyor ---
            // İlk çağrı: Sıkıştırma kapalı (false), muhtemelen sunucu taraflı loglama veya dosyaya yazma için.
            csFunction.CreateCompressXml(context, resultXml, "fightlabdropitemlist_out", false);

            // İkinci çağrı: Sıkıştırma açık (true), istemciye gönderilecek asıl sonuç.
            return csFunction.CreateCompressXml(context, resultXml, "fightlabdropitemlist", true);
        }

        // Token: 0x1700003A RID: 58
        // (get) Token: 0x06000109 RID: 265 RVA: 0x00003828 File Offset: 0x00001A28
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