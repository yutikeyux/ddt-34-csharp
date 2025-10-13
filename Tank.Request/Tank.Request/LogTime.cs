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
	// Token: 0x02000054 RID: 84
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class LogTime : IHttpHandler
	{
		// Token: 0x06000184 RID: 388 RVA: 0x0000CAD0 File Offset: 0x0000ACD0
		public void ProcessRequest(HttpContext context)
		{
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
				}
			}
			catch (Exception ex)
			{
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000185 RID: 389 RVA: 0x0000CC44 File Offset: 0x0000AE44
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000057 RID: 87
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
