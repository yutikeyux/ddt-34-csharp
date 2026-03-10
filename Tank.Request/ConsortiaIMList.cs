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
	// Token: 0x0200001F RID: 31
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class ConsortiaIMList : IHttpHandler
	{
		// Token: 0x0600007E RID: 126 RVA: 0x000055F8 File Offset: 0x000037F8
		public void ProcessRequest(HttpContext context)
		{
			bool value = false;
			string message = "Başarısız!";
			int total = 0;
			XElement result = new XElement("Result");
			try
			{
				int id = int.Parse(context.Request["id"]);
				using (ConsortiaBussiness db = new ConsortiaBussiness())
				{
					ConsortiaInfo info = db.GetConsortiaSingle(id);
					bool flag = info != null;
					if (flag)
					{
						result.Add(new XAttribute("Level", info.Level));
						result.Add(new XAttribute("Repute", info.Repute));
					}
				}
				using (ConsortiaBussiness db2 = new ConsortiaBussiness())
				{
					ConsortiaUserInfo[] infos = db2.GetConsortiaUsersPage(1, 1000, ref total, -1, id, -1, -1);
					foreach (ConsortiaUserInfo info2 in infos)
					{
						result.Add(FlashUtils.CreateConsortiaIMInfo(info2));
					}
					value = true;
					message = "Başarılı!";
				}
			}
			catch (Exception ex)
			{
				ConsortiaIMList.log.Error("ConsortiaIMList", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			context.Response.Write(result.ToString(false));
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600007F RID: 127 RVA: 0x0000579C File Offset: 0x0000399C
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400001C RID: 28
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
