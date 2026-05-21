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
	// Token: 0x0200001D RID: 29
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class ConsortiaEquipControlList : IHttpHandler
	{
		// Token: 0x06000076 RID: 118 RVA: 0x00005284 File Offset: 0x00003484
		public void ProcessRequest(HttpContext context)
		{
			bool value = false;
			string message = "Başarısız!";
			XElement result = new XElement("Result");
			int total = 0;
			try
			{
				int page = 1;
				int size = 10;
				int order = 1;
				int consortiaID = int.Parse(context.Request["consortiaID"]);
				int level = int.Parse(context.Request["level"]);
				int type = int.Parse(context.Request["type"]);
				using (ConsortiaBussiness db = new ConsortiaBussiness())
				{
					ConsortiaEquipControlInfo[] infos = db.GetConsortiaEquipControlPage(page, size, ref total, order, consortiaID, level, type);
					foreach (ConsortiaEquipControlInfo info in infos)
					{
						result.Add(FlashUtils.CreateConsortiaEquipControlInfo(info));
					}
					value = true;
					message = "Success!";
				}
			}
			catch (Exception ex)
			{
				ConsortiaEquipControlList.log.Error("ConsortiaList", ex);
			}
			result.Add(new XAttribute("total", total));
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			context.Response.ContentType = "text/plain";
			context.Response.Write(result.ToString(false));
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000077 RID: 119 RVA: 0x0000540C File Offset: 0x0000360C
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400001A RID: 26
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
