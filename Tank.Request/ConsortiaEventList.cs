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
	// Token: 0x0200001E RID: 30
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class ConsortiaEventList : IHttpHandler
	{
		// Token: 0x0600007A RID: 122 RVA: 0x00005438 File Offset: 0x00003638
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
				using (ConsortiaBussiness db = new ConsortiaBussiness())
				{
					ConsortiaEventInfo[] infos = db.GetConsortiaEventPage(page, size, ref total, order, consortiaID);
					foreach (ConsortiaEventInfo info in infos)
					{
						result.Add(FlashUtils.CreateConsortiaEventInfo(info));
					}
					value = true;
					message = "Başarılı!";
				}
			}
			catch (Exception ex)
			{
				ConsortiaEventList.log.Error("ConsortiaEventList", ex);
			}
			result.Add(new XAttribute("total", total));
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			context.Response.ContentType = "text/plain";
			context.Response.Write(result.ToString(false));
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600007B RID: 123 RVA: 0x000055CC File Offset: 0x000037CC
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400001B RID: 27
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
