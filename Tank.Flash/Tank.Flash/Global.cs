using System;
using System.Web;

namespace Tank.Flash
{
	// Token: 0x0200000C RID: 12
	public class Global : HttpApplication
	{
		// Token: 0x06000025 RID: 37 RVA: 0x000028C7 File Offset: 0x00000AC7
		protected void Application_Start(object sender, EventArgs e)
		{
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000028C9 File Offset: 0x00000AC9
		protected void Session_Start(object sender, EventArgs e)
		{
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000028CB File Offset: 0x00000ACB
		protected void Application_BeginRequest(object sender, EventArgs e)
		{
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000028CD File Offset: 0x00000ACD
		protected void Application_AuthenticateRequest(object sender, EventArgs e)
		{
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000028CF File Offset: 0x00000ACF
		protected void Application_Error(object sender, EventArgs e)
		{
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000028D1 File Offset: 0x00000AD1
		protected void Session_End(object sender, EventArgs e)
		{
			if (base.Session["Loading"] != null)
			{
				LoadingManager.LoadingCount--;
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000028F1 File Offset: 0x00000AF1
		protected void Application_End(object sender, EventArgs e)
		{
		}
	}
}
