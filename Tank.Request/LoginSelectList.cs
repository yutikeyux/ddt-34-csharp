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
	// Token: 0x02000052 RID: 82
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class LoginSelectList : IHttpHandler
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600017E RID: 382 RVA: 0x00002D8D File Offset: 0x00000F8D
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000C854 File Offset: 0x0000AA54
		public void ProcessRequest(HttpContext context)
		{
			bool flag = false;
			string str = "Fail!";
			XElement xelement = new XElement("Result");
			try
			{
				string str2 = HttpUtility.UrlDecode(context.Request["username"]);
				HttpUtility.UrlDecode(context.Request["password"]);
				using (PlayerBussiness playerBussiness = new PlayerBussiness())
				{
					PlayerInfo[] userLoginList = playerBussiness.GetUserLoginList(str2);
					bool flag2 = userLoginList.Length == 0;
					if (!flag2)
					{
						PlayerInfo[] array = userLoginList;
						foreach (PlayerInfo playerInfo in array)
						{
							bool flag3 = !string.IsNullOrEmpty(playerInfo.NickName);
							if (flag3)
							{
								xelement.Add(FlashUtils.CreateUserLoginList(playerInfo));
							}
						}
						flag = true;
						str = "Success!";
					}
				}
			}
			catch (Exception ex)
			{
				LoginSelectList.log.Error("LoginSelectList", ex);
			}
			finally
			{
				xelement.Add(new XAttribute("value", flag));
				xelement.Add(new XAttribute("message", str));
				context.Response.ContentType = "text/plain";
				context.Response.Write(xelement.ToString(false));
			}
		}

		// Token: 0x04000055 RID: 85
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
