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
	// Token: 0x02000012 RID: 18
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class UserRankDate : IHttpHandler
	{
		// Token: 0x06000049 RID: 73 RVA: 0x00003E14 File Offset: 0x00002014
		public void ProcessRequest(HttpContext context)
		{
			bool flag = false;
			string str = "Başarısız!";
			XElement node = new XElement("Result");
			try
			{
				string s = HttpUtility.UrlDecode(context.Request["userID"]);
				HttpUtility.UrlDecode(context.Request["ConsortiaID"]);
				using (PlayerBussiness playerBussiness = new PlayerBussiness())
				{
					UserRankDateInfo userRankDateById = playerBussiness.GetUserRankDateByID(int.Parse(s));
					bool flag2 = userRankDateById == null;
					if (!flag2)
					{
						node.Add(FlashUtils.CreateUserRankDateItems(userRankDateById));
						flag = true;
						str = "Başarılı!";
					}
				}
			}
			catch (Exception ex)
			{
				UserRankDate.log.Error("UserRankDate", ex);
			}
			finally
			{
				bool flag3 = flag;
				if (flag3)
				{
					node.Add(new XAttribute("value", flag));
					node.Add(new XAttribute("message", str));
					context.Response.ContentType = "text/plain";
					context.Response.Write(node.ToString(false));
				}
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00003F5C File Offset: 0x0000215C
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000010 RID: 16
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
