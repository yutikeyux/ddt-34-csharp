using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness;
using log4net;

namespace Tank.Request
{
	// Token: 0x02000005 RID: 5
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class AccountRegister : IHttpHandler
	{
		// Token: 0x0600000F RID: 15 RVA: 0x000027E0 File Offset: 0x000009E0
		public void ProcessRequest(HttpContext context)
		{
			XElement result = new XElement("Result");
			bool registerResult = false;
			try
			{
				string username = HttpUtility.UrlDecode(context.Request["username"]);
				string password = HttpUtility.UrlDecode(context.Request["password"]);
				string nickName = HttpUtility.UrlDecode(context.Request["password"]);
				bool sex = false;
				int money = 100;
				int giftoken = 100;
				int gold = 100;
				using (PlayerBussiness db = new PlayerBussiness())
				{
					registerResult = db.RegisterUser(username, password, nickName, sex, money, giftoken, gold);
				}
			}
			catch (Exception ex)
			{
				AccountRegister.log.Error("RegisterResult", ex);
			}
			finally
			{
				result.Add(new XAttribute("value", "vl"));
				result.Add(new XAttribute("message", registerResult));
				context.Response.ContentType = "text/plain";
				context.Response.Write(result.ToString(false));
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00002924 File Offset: 0x00000B24
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000004 RID: 4
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
