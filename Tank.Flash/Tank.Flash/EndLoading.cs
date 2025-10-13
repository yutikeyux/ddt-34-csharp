using System;
using System.Web.UI;

namespace Tank.Flash
{
	// Token: 0x0200000B RID: 11
	public class EndLoading : Page
	{
		// Token: 0x06000023 RID: 35 RVA: 0x0000288E File Offset: 0x00000A8E
		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.Session["Loading"] != null)
			{
				LoadingManager.LoadingCount--;
				this.Session["Loading"] = null;
			}
		}
	}
}
