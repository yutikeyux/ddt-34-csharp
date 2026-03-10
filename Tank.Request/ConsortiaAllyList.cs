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
	// Token: 0x02000017 RID: 23
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class ConsortiaAllyList : IHttpHandler
	{
		// Token: 0x0600005D RID: 93 RVA: 0x000046F0 File Offset: 0x000028F0
		public void ProcessRequest(HttpContext context)
		{
			bool value = false;
			string message = "Başarısız!";
			XElement result = new XElement("Result");
			int total = 0;
			try
			{
				int page = int.Parse(context.Request["page"]);
				int size = int.Parse(context.Request["size"]);
				int order = int.Parse(context.Request["order"]);
				int consortiaID = int.Parse(context.Request["consortiaID"]);
				int state = int.Parse(context.Request["state"]);
				string name = csFunction.ConvertSql(HttpUtility.UrlDecode((context.Request["name"] == null) ? "" : context.Request["name"]));
				using (ConsortiaBussiness db = new ConsortiaBussiness())
				{
					ConsortiaAllyInfo[] infos = db.GetConsortiaAllyPage(page, size, ref total, order, consortiaID, state, name);
					foreach (ConsortiaAllyInfo info in infos)
					{
						result.Add(FlashUtils.CreateConsortiaAllyInfo(info));
					}
					value = true;
					message = "Başarılı!";
				}
			}
			catch (Exception ex)
			{
				ConsortiaAllyList.log.Error("ConsortiaAllyList", ex);
			}
			result.Add(new XAttribute("total", total));
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			context.Response.ContentType = "text/plain";
			context.Response.Write(result.ToString(false));
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600005E RID: 94 RVA: 0x000048EC File Offset: 0x00002AEC
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000014 RID: 20
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
