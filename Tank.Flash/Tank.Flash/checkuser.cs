using System;
using System.Configuration;
using System.Web;
using System.Web.SessionState;
using Bussiness;

namespace Tank.Flash
{
	// Token: 0x02000008 RID: 8
	public class checkuser : IHttpHandler, IRequiresSessionState
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000014 RID: 20 RVA: 0x0000233F File Offset: 0x0000053F
		public string SiteTitle
		{
			get
			{
				if (ConfigurationManager.AppSettings["SiteTitle"] != null)
				{
					return ConfigurationManager.AppSettings["SiteTitle"];
				}
				return "DanDanTang";
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002368 File Offset: 0x00000568
		public void ProcessRequest(HttpContext context)
		{
			string text = context.Request["username"];
			string text2 = context.Request["password"];
			using (MemberShipBussiness memberShipBussiness = new MemberShipBussiness())
			{
				if (memberShipBussiness.CheckUsername(text, text2))
				{
					context.Session["username"] = text;
					context.Session["password"] = text2;
					if (memberShipBussiness.CheckAdmin(text))
					{
						LoadingManager.Add(text, text2, true);
					}
					else
					{
						LoadingManager.Add(text, text2);
					}
					context.Response.Write("ok");
				}
				else
				{
					context.Response.Write("Tài khoản hoặc mật khẩu không đúng!");
				}
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002460 File Offset: 0x00000660
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}
