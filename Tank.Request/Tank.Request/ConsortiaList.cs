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
	// Token: 0x02000022 RID: 34
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class ConsortiaList : IHttpHandler
	{
		// Token: 0x0600008B RID: 139 RVA: 0x00005AE8 File Offset: 0x00003CE8
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
				string name = csFunction.ConvertSql(HttpUtility.UrlDecode(context.Request["name"]));
				int level = int.Parse(context.Request["level"]);
				int openApply = int.Parse(context.Request["openApply"]);
				using (ConsortiaBussiness db = new ConsortiaBussiness())
				{
					ConsortiaInfo[] infos = db.GetConsortiaPage(page, size, ref total, order, name, consortiaID, level, openApply);
					foreach (ConsortiaInfo info in infos)
					{
						result.Add(FlashUtils.CreateConsortiaInfo(info));
					}
					value = true;
					message = "Success!";
				}
			}
			catch (Exception ex)
			{
				ConsortiaList.log.Error("ConsortiaList", ex);
			}
			result.Add(new XAttribute("total", total));
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			context.Response.ContentType = "text/plain";
			context.Response.BinaryWrite(StaticFunction.Compress(result.ToString(false)));
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00005CE8 File Offset: 0x00003EE8
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400001F RID: 31
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
