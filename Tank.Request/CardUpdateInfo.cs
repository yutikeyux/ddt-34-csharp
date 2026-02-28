using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness;
using log4net;
using Road.Flash;
using SqlDataProvider.Data;

namespace Tank.Request
{
    // Token: 0x02000011 RID: 17
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CardUpdateInfo : IHttpHandler
    {
        // Token: 0x06000044 RID: 68 RVA: 0x00003CD9 File Offset: 0x00001ED9
        public void ProcessRequest(HttpContext context)
        {
            context.Response.Write(CardUpdateInfo.Bulid(context));
        }

        // Token: 0x06000045 RID: 69 RVA: 0x00003CF0 File Offset: 0x00001EF0
        public static string Bulid(HttpContext context)
        {
            bool value = false;
            string message = "Fail!";
            XElement result = new XElement("Result");
            try
            {
                using (ProduceBussiness db = new ProduceBussiness())
                {
                    SqlDataProvider.Data.CardUpdateInfo[] infos = db.GetAllCardUpdateInfo();
                    foreach (SqlDataProvider.Data.CardUpdateInfo info in infos)
                    {
                        result.Add(FlashUtils.CreateCardUpdateInfo(info));
                    }
                    value = true;
                    message = "Success!";
                }
            }
            catch (Exception ex)
            {
                CardUpdateInfo.log.Error("Load CardUpdateInfo is fail!", ex);
            }
            result.Add(new XAttribute("value", value));
            result.Add(new XAttribute("message", message));
            return csFunction.CreateCompressXml(context, result, "CardUpdateInfo", true);
        }

        // Token: 0x1700000F RID: 15
        // (get) Token: 0x06000046 RID: 70 RVA: 0x00003DE8 File Offset: 0x00001FE8
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        // Token: 0x0400000F RID: 15
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    }
}