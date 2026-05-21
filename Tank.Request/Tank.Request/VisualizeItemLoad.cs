using System;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness;
using log4net;

namespace Tank.Request
{
	// Token: 0x0200007F RID: 127
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class VisualizeItemLoad : IHttpHandler
	{
		// Token: 0x0600024C RID: 588 RVA: 0x000117A0 File Offset: 0x0000F9A0
		public void ProcessRequest(HttpContext context)
		{
			bool value = false;
			string message = "Başarısız!";
			bool sex = bool.Parse(context.Request["sex"]);
			XElement result = new XElement("Result");
			try
			{
				string content = ConfigurationSettings.AppSettings[sex ? "BoyVisualizeItem" : "GrilVisualizeItem"];
				result.Add(new XAttribute("content", content));
				value = true;
				message = "Success!";
			}
			catch (Exception ex)
			{
				VisualizeItemLoad.log.Error("VisualizeItemLoad", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			context.Response.ContentType = "text/plain";
			context.Response.Write(result.ToString(false));
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600024D RID: 589 RVA: 0x0001189C File Offset: 0x0000FA9C
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400008E RID: 142
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
