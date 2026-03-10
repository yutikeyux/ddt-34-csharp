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
	// Token: 0x02000019 RID: 25
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class ConsortiaApplyUsersList : IHttpHandler
	{
		// Token: 0x06000065 RID: 101 RVA: 0x00004B08 File Offset: 0x00002D08
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
				int applyID = int.Parse(context.Request["applyID"]);
				int userID = int.Parse(context.Request["userID"]);
				using (ConsortiaBussiness db = new ConsortiaBussiness())
				{
					ConsortiaApplyUserInfo[] infos = db.GetConsortiaApplyUserPage(page, size, ref total, order, consortiaID, applyID, userID);
					foreach (ConsortiaApplyUserInfo info in infos)
					{
						result.Add(FlashUtils.CreateConsortiaApplyUserInfo(info));
					}
					value = true;
					message = "Başarılı!";
				}
			}
			catch (Exception ex)
			{
				ConsortiaApplyUsersList.log.Error("ConsortiaApplyUsersList", ex);
			}
			result.Add(new XAttribute("total", total));
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			context.Response.ContentType = "text/plain";
			context.Response.Write(result.ToString(false));
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00004CCC File Offset: 0x00002ECC
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000016 RID: 22
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
