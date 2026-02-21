using System;
using System.Web;
using System.Web.UI;

namespace Tank.Request
{
	// Token: 0x02000069 RID: 105
	public class SendItemTest : Page
	{
		// Token: 0x060001EA RID: 490 RVA: 0x0000EF10 File Offset: 0x0000D110
		protected void Page_Load(object sender, EventArgs e)
		{
			HttpCookie aCookie = base.Request.Cookies["userInfo"];
			string value = aCookie.Value;
			string userName = aCookie.Values["bd_sig_user"];
			base.Response.Write(value);
		}
	}
}
