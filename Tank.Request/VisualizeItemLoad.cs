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
	// Token: 0x02000083 RID: 131
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class VisualizeItemLoad : IHttpHandler
	{
		// Token: 0x06000252 RID: 594 RVA: 0x00010F90 File Offset: 0x0000F190
		public void ProcessRequest(HttpContext context)
		{
			bool value = false;
			string message = "Hata!";
			bool sex = bool.Parse(context.Request["sex"]);
			XElement result = new XElement("Result");
			try
			{
				string content = ConfigurationManager.AppSettings[sex ? "BoyVisualizeItem" : "GrilVisualizeItem"];
				result.Add(new XAttribute("content", content));
				value = true;
				message = "Başarılı!";
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
		// (get) Token: 0x06000253 RID: 595 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000094 RID: 148
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
