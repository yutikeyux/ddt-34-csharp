using System;
using System.Text;
using System.Web;
using System.Web.Services;

namespace Tank.Request
{
    // Token: 0x02000026 RID: 38
    // CreateAllXml sınıfı, oyunun tüm temel konfigürasyonlarını bir araya getirip dönen bir HTTP Handler'dır.
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CreateAllXml : IHttpHandler
    {
        // Token: 0x0600009A RID: 154 RVA: 0x00006A84 File Offset: 0x00004C84
        // Gelen isteği karşılayan metod
        public void ProcessRequest(HttpContext context)
        {
            // İstek yapanın IP adresi yetkili mi kontrol et
            if (csFunction.ValidAdminIP(context.Request.UserHostAddress))
            {
                // Tüm XML verilerini birleştirmek için StringBuilder kullanılır
                StringBuilder allConfigsXml = new StringBuilder();

                // --- TÜM LİSTELERİ VE YAPILANDIRMALARI ÇEK VE BİRLEŞTİR ---
                // Not: Metot isimleri orijinal koddaki gibi "Build" olarak kullanılmıştır.

                allConfigsXml.Append(ActiveList.Build(context));           // Aktiflik Listesi
                allConfigsXml.Append(BallList.Build(context));             // Toplar (Bombalar)
                allConfigsXml.Append(LoadMapsItems.Build(context));        // Map Eşyaları
                allConfigsXml.Append(LoadPVEItems.Build(context));         // PVE Eşyaları (Metot adı 'Build' doğru yazılmış)
                allConfigsXml.Append(QuestList.Build(context));            // Görev Listesi
                allConfigsXml.Append(TemplateAllList.Build(context));      // Tüm Şablonlar
                allConfigsXml.Append(ShopItemList.Build(context));         // Mağaza Listesi
                allConfigsXml.Append(LoadItemsCategory.Build(context));   // Eşya Kategorileri
                allConfigsXml.Append(ItemStrengthenList.Build(context));   // Eşya Güçlendirme Listesi
                allConfigsXml.Append(MapServerList.Build(context));        // Map Sunucu Listesi
                allConfigsXml.Append(ConsortiaLevelList.Build(context));  // Lonca Seviyeleri
                allConfigsXml.Append(DailyAwardList.Build(context));       // Günlük Ödüller
                allConfigsXml.Append(NPCInfoList.Build(context));          // NPC Bilgileri
                allConfigsXml.Append(LoginAwardItemTemplate.Build(context)); // Giriş Ödülü Şablonu
                allConfigsXml.Append(eventrewarditemlist.Build(context));  // Etkinlik Ödülleri
                allConfigsXml.Append(serverconfig.Build(context));         // Sunucu Konfigürasyonu
                allConfigsXml.Append(ShopGoodsShowList.Build(context));    // Mağaza Gösterimi
                allConfigsXml.Append(newtitle.Build(context));             // Yeni Ünvanlar
                allConfigsXml.Append(petskillelementinfo.Build(context));  // Evcil Hayvan Yetenek Elementleri
                allConfigsXml.Append(petskillinfo.Build(context));         // Evcil Hayvan Yetenekleri
                allConfigsXml.Append(petskilltemplateinfo.Build(context)); // Evcil Hayvan Yetenek Şablonları
                allConfigsXml.Append(pettemplateinfo.Build(context));      // Evcil Hayvan Şablonları
                allConfigsXml.Append(CardUpdateCondition.Build(context));   // Kart Güncelleme Koşulları ('Build')
                allConfigsXml.Append(CardUpdateInfo.Build(context));        // Kart Güncelleme Bilgileri ('Build')
                allConfigsXml.Append(activitysystemitems.Build(context));  // Etkinlik Sistemi Öğeleri ('Build')
                allConfigsXml.Append(suittemplateinfolist.Build(context)); // Takım (Suit) Şablon Listesi ('Build')
                allConfigsXml.Append(DailyLeagueLevelList.Build(context)); // Günlük Lig Seviyeleri ('Build')
                allConfigsXml.Append(DailyLeagueAwardList.Build(context)); // Günlük Lig Ödülleri ('Build')

                // --- YANITI GÖNDER ---
                context.Response.ContentType = "text/plain";

                // Oluşturulan devasa XML dizisini ekrana yazar.
                // UYARI: Birçok XML'in üst üste yapıştırılması oluşturduğu dize tek bir geçerli XML belgesi olmayabilir.
                // Ancak istemci (Flash/Unity) bunu özel bir stream okuyucusuyla işliyor olabilir.
                context.Response.Write(allConfigsXml.ToString());
            }
            else
            {
                // NOT: 'consortiabuffertemp' dosyasında görülen espiri burada tekrarlanmış.
                context.Response.Write("Tabi Efendim!");
            }
        }

        // Token: 0x17000022 RID: 34
        // (get) Token: 0x0600009B RID: 155 RVA: 0x00003828 File Offset: 0x00001A28
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