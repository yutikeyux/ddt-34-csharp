using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Tank.Request
{
	// Token: 0x0200006B RID: 107
	public class SentRewardTest : Page
	{
		// Token: 0x060001F4 RID: 500 RVA: 0x0000F4DC File Offset: 0x0000D6DC
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

		// Token: 0x04000070 RID: 112
		protected HtmlForm form1;
	}
}
