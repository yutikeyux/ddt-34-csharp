using Bussiness;
using Bussiness.CenterService;
using log4net;
using Road.Flash;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace Tank.Request
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ServerList : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            bool value = false;
            string message = "Başarısız!";
            int total = 0;

            // Flash tarafı new XML(param1) ile bunu alacak. Kök isminin "Result" olması standarttır.
            XElement result = new XElement("Result");

            try
            {
                using (CenterServiceClient temp = new CenterServiceClient())
                {
                    IList<ServerData> list = temp.GetServerList();
                    foreach (ServerData s in list)
                    {
                        // State -1 genellikle gizli/bakımdaki sunucular içindir, listeye eklenmez.
                        bool flag = s.State == -1;
                        if (!flag)
                        {
                            total += s.Online;

                            // FlashUtils.CreateServerInfo zaten "Item" elementini oluşturuyor.
                            // Port: Veritabanından gelen Port'tan 69 çıkarılıyor (Flash tarafı tekrar ekleyecek).
                            result.Add(FlashUtils.CreateServerInfo(s.Id, s.Name, s.Ip, s.Port - 69, s.State, s.MustLevel, s.LowestLevel, s.Online));
                        }
                    }
                }
                value = true;
                message = "Başarılı!";
            }
            catch (Exception ex)
            {
                ServerList.log.Error("Load server list error:", ex);
                // Hata durumunda 'value' false kalacak ve Flash hatayı yakalayacak.
            }

            // --- DÜZELTİLEN XML ATTRIBUTE KISMI ---

            // Flash kodu: if(_loc2_.@value == "true") -> "value" arıyor.
            result.Add(new XAttribute("value", value));

            // Flash kodu: message = _loc2_.@message; -> "message" arıyor.
            result.Add(new XAttribute("message", message));

            // Flash kodu: this.agentId = _loc2_.@agentId; -> "agentId" arıyor.
            // Buradaki değeri (örn: 1) kendi sistemine göre değiştirebilirsin.
            result.Add(new XAttribute("agentId", 1));

            // Flash kodu: this.zoneName = _loc2_.@AreaName; -> "AreaName" arıyor.
            // Buradaki değeri kendi sunucu ismine göre değiştirebilirsin.
            result.Add(new XAttribute("AreaName", "DDTQuest"));

            // Toplam oyuncu sayısı (Flash tarafı kullanmasa da yapı için gerekebilir).
            result.Add(new XAttribute("total", total));

            // -------------------------------------

            context.Response.ContentType = "text/plain";
            context.Response.Write(result.ToString());
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    }
}