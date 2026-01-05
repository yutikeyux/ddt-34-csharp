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
	// Token: 0x02000013 RID: 19
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class UserRankDate : IHttpHandler
	{
		// Token: 0x0600004C RID: 76 RVA: 0x00004AF0 File Offset: 0x00002CF0
		public void ProcessRequest(HttpContext context)
		{
			bool flag = false;
			string str = "Fail!";
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
						str = "Success!";
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
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000011 RID: 17
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
