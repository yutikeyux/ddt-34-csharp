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
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class clothgrouptemplateinfo : IHttpHandler
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void ProcessRequest(HttpContext context)
        {
            if (csFunction.ValidAdminIP(context.Request.UserHostAddress))
            {
                context.Response.Write(Build(context));
            }
            else
            {
                context.Response.Write("IP is not valid!");
            }
        }

        public static string Build(HttpContext context)
        {
            bool value = false;
            string message = "Basarisiz!";
            XElement result = new XElement("Result");
            try
            {
                using (ProduceBussiness db = new ProduceBussiness())
                {
                    ClothGroupTemplateInfo[] infos = db.GetAllClothGroup();
                    foreach (ClothGroupTemplateInfo info in infos)
                    {
                        result.Add(FlashUtils.CreateClothGroupTemplateInfo(info));
                    }
                }
                value = true;
                message = "Basarili!";
            }
            catch (Exception ex)
            {
                log.Error("clothgrouptemplateinfo", ex);
            }
            result.Add(new XAttribute("value", value));
            result.Add(new XAttribute("message", message));
            return csFunction.CreateCompressXml(context, result, "clothgrouptemplateinfo", true);
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}
