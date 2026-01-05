using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness; // İş katmanı kütüphanesi
using log4net; // Loglama kütüphanesi
using SqlDataProvider.Data; // Veritabanı veri yapıları

namespace Tank.Request
{
    // Token: 0x02000051 RID: 81
    // LoadUsersSort sınıfı, kullanıcıları belirli bir kritere göre sıralı listeyi yüklemek için kullanılan bir HTTP Handler'dır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class LoadUsersSort : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x06000172 RID: 370 RVA: 0x0000B9F4 File Offset: 0x00009BF4
        // Gelen isteği karşılayan ve sıralı kullanıcı listesini hazırlayan metod
        public void ProcessRequest(HttpContext context)
        {
            bool isSuccess = false;
            string message = "Hata!";
            int totalCount = 0;

            // Kök XML elementini oluştur
            XElement resultXml = new XElement("Result");

            try
            {
                // --- 1. PARAMETRELERİ AL ---
                // Sayfa numarası sabit 1'dir. Her zaman ilk sayfayı getirir.
                int page = 1;
                // Sayfa boyutu sabit 10'dur. Her zaman ilk 10 kişiyi getirir.
                int pageSize = 10;
                // Sıralama kriteri (Order) istekten alınır. (Örn: 1:Seviye, 2:GP vb.)
                int order = int.Parse(context.Request["order"]);
                // Kullanıcı ID'si sabit -1'dir. Bu, muhtemelen "Tüm kullanıcıları" veya "Belirli bir kullanıcıya aitleri" koşulunu kaldırır.
                int userID = -1;

                // Veritabanı sorgusu için geri dönen boolean (resultValue)
                bool dbQuerySuccess = false;

                // --- 2. VERİTABANI İŞLEMLERİ ---
                using (PlayerBussiness pb = new PlayerBussiness())
                {
                    // Veritabanından sayfalama mantığıyla oyuncu listesini çek
                    PlayerInfo[] playerList = pb.GetPlayerPage(page, pageSize, ref totalCount, order, userID, ref dbQuerySuccess);

                    // Sorgu başarılıysa listeyi oluştur
                    if (dbQuerySuccess)
                    {
                        // Her bir oyuncuyu döngüye al
                        foreach (PlayerInfo player in playerList)
                        {
                            // Oyuncu bilgisini XML formatına çevirip sonuç listesine ekle
                            XElement playerNode = new XElement("Item", new object[]
                            {
                                new XAttribute("ID", player.ID),
                                // Bazı alanlar null gelebilir, kontrol eklenmiş.
                                new XAttribute("NickName", (player.NickName == null) ? "" : player.NickName),
                                new XAttribute("Grade", player.Grade),
                                new XAttribute("Colors", (player.Colors == null) ? "" : player.Colors),
                                new XAttribute("Skin", (player.Skin == null) ? "" : player.Skin),
                                new XAttribute("Sex", player.Sex),
                                new XAttribute("Style", (player.Style == null) ? "" : player.Style),
                                new XAttribute("ConsortiaName", (player.ConsortiaName == null) ? "" : player.ConsortiaName),
                                new XAttribute("Hide", player.Hide),
                                new XAttribute("Offer", player.Offer),
                                new XAttribute("ReputeOffer", player.ReputeOffer),
                                new XAttribute("ConsortiaHonor", player.ConsortiaHonor),
                                new XAttribute("ConsortiaLevel", player.ConsortiaLevel),
                                new XAttribute("ConsortiaRepute", player.ConsortiaRepute),
                                new XAttribute("WinCount", player.Win),
                                new XAttribute("TotalCount", player.Total),
                                new XAttribute("EscapeCount", player.Escape),
                                new XAttribute("Repute", player.Repute),
                                new XAttribute("GP", player.GP)
                            });
                            resultXml.Add(playerNode);
                        }

                        isSuccess = true;
                        message = "Başarılı!";
                    }
                }
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                LoadUsersSort.log.Error("LoadUsersSort yüklenirken hata:", ex);
            }

            // XML'e genel durum bilgilerini (total, value ve message) ekle
            resultXml.Add(new XAttribute("total", totalCount));
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // Yanıtı ekrana yaz
            // DİKKAT: Orijinal kodda sıkıştırma (Compress) YOKTUR.
            // Doğrudan XML string'i olarak gönderilir.
            context.Response.ContentType = "text/plain";
            context.Response.Write(resultXml.ToString(false));
        }

        // Token: 0x17000051 RID: 81
        // (get) Token: 0x06000173 RID: 371 RVA: 0x00003828 File Offset: 0x00001A28
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