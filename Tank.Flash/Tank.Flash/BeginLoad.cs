using System;
using System.Web.UI;

namespace Tank.Flash
{
	// Token: 0x02000007 RID: 7
	public class BeginLoad : Page
	{
		// Token: 0x06000012 RID: 18 RVA: 0x00002301 File Offset: 0x00000501
		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.Session["Loading"] == null)
			{
				LoadingManager.LoadingCount++;
				this.Session["Loading"] = false;
			}
		}
	}
}
