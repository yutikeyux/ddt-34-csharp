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
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class LogTime : IHttpHandler
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void ProcessRequest(HttpContext context)
        {
            bool isSuccess = false;
            string message = "Fail!";
            int totalCount = 0;

            XElement resultXml = new XElement("Result");

            try
            {
                // --- 1. PARAMETRELERİ AL (null/boş kontrolü ile) ---
                int page = 1;
                int size = 20;
                int order = 0;
                int consortiaID = 0;
                int state = 0;

                if (!TryGetInt(context.Request["page"], out page) || page < 1)
                    page = 1;

                if (!TryGetInt(context.Request["size"], out size) || size < 1)
                    size = 20;

                if (!TryGetInt(context.Request["order"], out order))
                    order = 0;

                if (!TryGetInt(context.Request["consortiaID"], out consortiaID))
                    consortiaID = 0;

                if (!TryGetInt(context.Request["state"], out state))
                    state = 0;

                // --- 2. İSİM GÜVENLİĞİ ---
                string rawName = context.Request["name"];
                string decodedName = string.IsNullOrEmpty(rawName) ? "" : HttpUtility.UrlDecode(rawName);
                string consortiumName = csFunction.ConvertSql(decodedName);

                // --- 3. VERİTABANI İŞLEMLERİ ---
                using (ConsortiaBussiness db = new ConsortiaBussiness())
                {
                    ConsortiaAllyInfo[] allyList = db.GetConsortiaAllyPage(
                        page, size, ref totalCount, order, consortiaID, state, consortiumName);

                    foreach (ConsortiaAllyInfo ally in allyList)
                    {
                        resultXml.Add(FlashUtils.CreateConsortiaAllyInfo(ally));
                    }
                }

                isSuccess = true;
                message = "Success!";
            }
            catch (Exception ex)
            {
                log.Error("LogTime yüklenirken hata:", ex);
            }

            // --- 4. YANITI OLUŞTUR ---
            resultXml.Add(new XAttribute("total", totalCount));
            resultXml.Add(new XAttribute("value", isSuccess));
            resultXml.Add(new XAttribute("message", message));

            context.Response.ContentType = "text/plain";
            context.Response.Write(resultXml.ToString(false));
        }

        // Güvenli int okuma yardımcı metodu
        private bool TryGetInt(string value, out int result)
        {
            if (string.IsNullOrEmpty(value))
            {
                result = 0;
                return false;
            }
            return int.TryParse(value, out result);
        }

        public bool IsReusable
        {//
            get
            {
                return false;
            }
        }
    }
}
