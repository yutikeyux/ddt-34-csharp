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
	// Token: 0x02000018 RID: 24
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class ConsortiaApplyAllyList : IHttpHandler
	{
		// Token: 0x06000061 RID: 97 RVA: 0x00004918 File Offset: 0x00002B18
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
				int applyID = int.Parse(context.Request["applyID"]);
				int state = int.Parse(context.Request["state"]);
				using (ConsortiaBussiness db = new ConsortiaBussiness())
				{
					ConsortiaApplyAllyInfo[] infos = db.GetConsortiaApplyAllyPage(page, size, ref total, order, consortiaID, applyID, state);
					foreach (ConsortiaApplyAllyInfo info in infos)
					{
						result.Add(FlashUtils.CreateConsortiaApplyAllyInfo(info));
					}
					value = true;
					message = "Success!";
				}
			}
			catch (Exception ex)
			{
				ConsortiaApplyAllyList.log.Error("ConsortiaApplyAllyList", ex);
			}
			result.Add(new XAttribute("total", total));
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			context.Response.ContentType = "text/plain";
			context.Response.Write(result.ToString(false));
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00004ADC File Offset: 0x00002CDC
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000015 RID: 21
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
