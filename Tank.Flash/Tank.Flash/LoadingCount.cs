using System;
using System.Web.UI;

namespace Tank.Flash
{
	// Token: 0x0200000D RID: 13
	public class LoadingCount : Page
	{
		// Token: 0x0600002D RID: 45 RVA: 0x000028FB File Offset: 0x00000AFB
		protected void Page_Load(object sender, EventArgs e)
		{
			base.Response.Write(string.Format("{0}", LoadingManager.LoadingCount));
		}
	}
}
