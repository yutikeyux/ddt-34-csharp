using System;
using System.Web;
using Bussiness;
using log4net.Config;

namespace Tank.Request
{
	// Token: 0x0200003B RID: 59
	public class Global : HttpApplication
	{
		// Token: 0x06000114 RID: 276 RVA: 0x000090B8 File Offset: 0x000072B8
		protected void Application_Start(object sender, EventArgs e)
		{
			string path = base.Server.MapPath("~");
			LanguageMgr.Setup(path);
			XmlConfigurator.Configure();
			StaticsMgr.Setup();
			PlayerManager.Setup();
		}

		// Token: 0x06000115 RID: 277 RVA: 0x000090F0 File Offset: 0x000072F0
		protected void Session_Start(object sender, EventArgs e)
		{
		}

		// Token: 0x06000116 RID: 278 RVA: 0x000090F0 File Offset: 0x000072F0
		protected void Application_BeginRequest(object sender, EventArgs e)
		{
		}

		// Token: 0x06000117 RID: 279 RVA: 0x000090F0 File Offset: 0x000072F0
		protected void Application_AuthenticateRequest(object sender, EventArgs e)
		{
		}

		// Token: 0x06000118 RID: 280 RVA: 0x000090F0 File Offset: 0x000072F0
		protected void Application_Error(object sender, EventArgs e)
		{
		}

		// Token: 0x06000119 RID: 281 RVA: 0x000090F0 File Offset: 0x000072F0
		protected void Session_End(object sender, EventArgs e)
		{
		}

		// Token: 0x0600011A RID: 282 RVA: 0x000090F3 File Offset: 0x000072F3
		protected void Application_End(object sender, EventArgs e)
		{
			StaticsMgr.Stop();
		}
	}
}
