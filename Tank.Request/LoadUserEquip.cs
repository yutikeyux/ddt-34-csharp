using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness; // İş katmanı kütüphanesi
using log4net; // Loglama kütüphanesi
using Road.Flash; // Flash istemcisi yardımcı kütüphanesi
using SqlDataProvider.Data; // Veritabanı veri yapıları

namespace Tank.Request
{
    // Token: 0x0200004E RID: 78
    // LoadUserEquip sınıfı, kullanıcının ekipmanlarını yüklemek için kullanılan bir HTTP Handler'dır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class LoadUserEquip : IHttpHandler
    {
        // Log4net ile loglama nesnesi
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Token: 0x06000165 RID: 357 RVA: 0x0000B08C File Offset: 0x0000928C
        // Gelen isteği karşılayan metod
        public void ProcessRequest(HttpContext context)
        {
            bool isSuccess = false;
            string message = "Hata!";

            // Kök XML elementini oluştur
            XElement resultXml = new XElement("Result");

            try
            {
                // Kullanıcı ID'sini al
                int userID = int.Parse(context.Request["ID"]);

                // --- 1. VERİTABANI İŞLEMLERİ ---
                using (PlayerBussiness pb = new PlayerBussiness())
                {
                    // Kullanıcının ana bilgilerini (Karakter özellikleri) çek
                    PlayerInfo userInfo = pb.GetUserSingleByUserID(userID);

                    // --- 2. XML OLUŞTURMA (ATRIBUTE EKLEME) ---
                    resultXml.Add(new object[]
                    {
                        new XAttribute("Agility", userInfo.Agility),
                        new XAttribute("Attack", userInfo.Attack),
                        new XAttribute("Colors", userInfo.Colors),
                        new XAttribute("Skin", userInfo.Skin),
                        new XAttribute("Defence", userInfo.Defence),
                        new XAttribute("GP", userInfo.GP),
                        new XAttribute("Grade", userInfo.Grade),
                        new XAttribute("Luck", userInfo.Luck),
                        new XAttribute("Hide", userInfo.Hide),
                        new XAttribute("Repute", userInfo.Repute),
                        new XAttribute("Offer", userInfo.Offer),
                        new XAttribute("NickName", userInfo.NickName),
                        new XAttribute("ConsortiaName", userInfo.ConsortiaName),
                        new XAttribute("ConsortiaID", userInfo.ConsortiaID),
                        new XAttribute("ReputeOffer", userInfo.ReputeOffer),
                        new XAttribute("ConsortiaHonor", userInfo.ConsortiaHonor),
                        new XAttribute("ConsortiaLevel", userInfo.ConsortiaLevel),
                        new XAttribute("ConsortiaRepute", userInfo.ConsortiaRepute),
                        new XAttribute("WinCount", userInfo.Win),
                        new XAttribute("TotalCount", userInfo.Total),
                        new XAttribute("EscapeCount", userInfo.Escape),
                        new XAttribute("Sex", userInfo.Sex),
                        new XAttribute("Style", userInfo.Style),
                        new XAttribute("FightPower", userInfo.FightPower)
                    });

                    // --- 3. EKİPMANLAR ÇEKME ---
                    // DÜZELTME: GetUserEquip metodu muhtemelen List<ItemInfo> döndürür.
                    // Biz değişkeni ItemInfo[] (Dizi) olarak tanımladık.
                    // Bu uyumsuzluğu gidermek için .ToArray() kullanıyoruz.
                    ItemInfo[] equipList = pb.GetUserEquip(userID).ToArray();

                    // Her bir ekipmanı döngüye al
                    foreach (ItemInfo equipItem in equipList)
                    {
                        // Ekipman bilgisini XML formatına çevirip sonuç listesine ekle
                        // FlashUtils, Flash istemcilerine uygun XML oluşturmak için kullanılan bir yardımcı sınıftır.
                        resultXml.Add(FlashUtils.CreateGoodsInfo(equipItem));
                    }

                    isSuccess = true;
                    message = "Başarılı!";
                }
            }
            catch (Exception ex)
            {
                // Hata oluşursa logla
                LoadUserEquip.log.Error("LoadUserEquip yüklenirken hata:", ex);
            }

            // XML'e genel durum bilgilerini (value ve message) ekle
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            // Yanıtı ekrana yaz (Sıkıştırma yok)
            context.Response.ContentType = "text/plain";
            context.Response.Write(resultXml.ToString(false));
        }

        // Token: 0x1700004E RID: 78
        // (get) Token: 0x06000166 RID: 358 RVA: 0x00003828 File Offset: 0x00001A28
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