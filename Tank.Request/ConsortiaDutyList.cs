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
	// Token: 0x0200001B RID: 27
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class ConsortiaDutyList : IHttpHandler
	{
		// Token: 0x0600006E RID: 110 RVA: 0x00004E78 File Offset: 0x00003078
		public void ProcessRequest(HttpContext context)
		{
			bool value = false;
			string message = "Fail!";
			XElement result = new XElement("Result");
			int total = 0;
			try
			{
				int page = int.Parse(context.Request["page"]);
				int size = int.Parse(context.Request["size"]);
				int order = int.Parse(context.Request["order"]);
				int consortiaID = int.Parse(context.Request["consortiaID"]);
				int dutyID = int.Parse(context.Request["dutyID"]);
				using (ConsortiaBussiness db = new ConsortiaBussiness())
				{
					ConsortiaDutyInfo[] infos = db.GetConsortiaDutyPage(page, size, ref total, order, consortiaID, dutyID);
					foreach (ConsortiaDutyInfo info in infos)
					{
						result.Add(FlashUtils.CreateConsortiaDutyInfo(info));
					}
					value = true;
					message = "Success!";
				}
			}
			catch (Exception ex)
			{
				ConsortiaDutyList.log.Error("ConsortiaDutyList", ex);
			}
			result.Add(new XAttribute("total", total));
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			context.Response.ContentType = "text/plain";
			context.Response.Write(result.ToString(false));
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00005024 File Offset: 0x00003224
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000018 RID: 24
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
