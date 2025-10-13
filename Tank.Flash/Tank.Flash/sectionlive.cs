using System;
using System.Web;
using System.Web.SessionState;

namespace Tank.Flash
{
	// Token: 0x02000011 RID: 17
	public class sectionlive : IHttpHandler, IRequiresSessionState
	{
		// Token: 0x06000041 RID: 65 RVA: 0x00003088 File Offset: 0x00001288
		public void ProcessRequest(HttpContext context)
		{
			string a = context.Request["skiplive"];
			try
			{
				if (a == "false")
				{
					LoadingManager.Remove(context.Session["username"].ToString());
					context.Response.Write("clear");
				}
				else if (context.Session["username"] != null && !string.IsNullOrEmpty(context.Session["username"].ToString()))
				{
					string name = context.Session["username"].ToString();
					string pass = context.Session["password"].ToString();
					if (LoadingManager.Login(name, pass))
					{
						context.Response.Write("live");
					}
					else
					{
						context.Response.Write("die");
					}
				}
				else
				{
					context.Response.Write("die");
				}
			}
			catch
			{
				context.Response.Write("sessionfail");
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000042 RID: 66 RVA: 0x000031A0 File Offset: 0x000013A0
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}
