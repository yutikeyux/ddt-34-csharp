using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Tank.Request
{
	// Token: 0x0200006F RID: 111
	public class SentRewardTest : Page
	{
		// Token: 0x060001FA RID: 506 RVA: 0x0000EFA4 File Offset: 0x0000D1A4
		protected void Page_Load(object sender, EventArgs e)
		{
			string mailTitle = "大幅度是";
			string mailContent = "大幅度是";
			string username = "watson";
			string gold = "6666";
			string money = "99999";
			string param = "11020,4,0,0,0,0,0,0,1|7014,2,9,400,400,400,400,400,0";
			string content = string.Concat(new string[]
			{
				mailTitle,
				"#",
				mailContent,
				"#",
				username,
				"#",
				gold,
				"#",
				money,
				"#",
				param,
				"#"
			});
			DateTime time = DateTime.Now;
			base.Response.Redirect("http://192.168.0.4:828/SentReward.ashx?content=" + base.Server.UrlEncode(content));
		}

		// Token: 0x04000076 RID: 118
		protected HtmlForm form1;
	}
}
