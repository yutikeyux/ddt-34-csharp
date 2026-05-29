using System;
using System.Text;
using System.Web;
using System.Web.Services;
using System.IO; // Dosya işlemleri için bu kütüphaneyi ekledik

namespace Tank.Request
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CreateAllXml : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string clientIP = context.Request.UserHostAddress;
            bool flag = csFunction.ValidAdminIP(clientIP);

            // --- LOG KAYIT İŞLEMİ ---
            try
            {
                // Log dosyasının yolu (Site kök dizininde oluşturulur)
                string logPath = context.Server.MapPath("~/ip_log.txt");

                // Log satırı formatı: Tarih - Saat - IP - Durum
                string logText = string.Format("[{0}] IP: {1} - Giriş Durumu: {2}{3}",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    clientIP,
                    flag ? "Başarılı (Admin)" : "Başarısız (Yetkisiz)",
                    Environment.NewLine);

                // Dosyaya ekleme yapar (dosya yoksa oluşturur)
                File.AppendAllText(logPath, logText);
            }
            catch (Exception ex)
            {
                // Dosya yazma izni vb. hatalarda sunucu hatası vermemesi için boş bırakılabilir veya loglanabilir
            }
            // --- LOG KAYIT İŞLEMİ SONU ---

            if (flag)
            {
                StringBuilder build = new StringBuilder();
                build.Append(ActiveList.Build(context));
                build.Append(BallList.Build(context));
                build.Append(LoadMapsItems.Build(context));
                build.Append(LoadPVEItems.Build(context));
                build.Append(QuestList.Build(context));
                build.Append(TemplateAllList.Build(context));
                build.Append(ShopItemList.Build(context));
                build.Append(LoadItemsCategory.Build(context));
                build.Append(ItemStrengthenList.Build(context));
                build.Append(MapServerList.Build(context));
                build.Append(ConsortiaLevelList.Build(context));
                build.Append(DailyAwardList.Build(context));
                build.Append(NPCInfoList.Build(context));
                build.Append(LoginAwardItemTemplate.Build(context));
                build.Append(eventrewarditemlist.Build(context));
                build.Append(serverconfig.Build(context));
                build.Append(ShopGoodsShowList.Build(context));
                build.Append(newtitle.Build(context));
                build.Append(petskillelementinfo.Build(context));
                build.Append(petskillinfo.Build(context));
                build.Append(petskilltemplateinfo.Build(context));
                build.Append(pettemplateinfo.Build(context));
                build.Append(CardUpdateCondition.Build(context));
                build.Append(CardUpdateInfo.Build(context));
                build.Append(activitysystemitems.Build(context));
                build.Append(suittemplateinfolist.Build(context));
                build.Append(DailyLeagueLevelList.Build(context));
                build.Append(DailyLeagueAwardList.Build(context));
                build.Append(clothgrouptemplateinfo.Build(context));
                build.Append(clothpropertytemplateinfo.Build(context));
                build.Append(consortiabuffertemp.Build(context));
                build.Append(runetemplatelist.Build(context));
                build.Append(petlevellist.Build(context));
                build.Append(loadpetfightproperty.Build(context));
                build.Append(loadpetmoeproperty.Build(context));
                build.Append(totemhonortemplate.Build(context));
                build.Append(suitpartequipinfolist.Build(context));
                build.Append(achievementlist.Build(context));
                build.Append(GMActivityInfo.Build(context));
                build.Append(bombconfig.Build(context));
                build.Append(fightlabdropitemlist.Build(context));
                context.Response.ContentType = "text/plain";
                context.Response.Write(build.ToString());
            }
            else
            {
                context.Response.Write("Yönetici Adresi Geçersiz!");
                context.Response.Write(" Loglarda " + clientIP);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}