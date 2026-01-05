using System;
using System.Web;
using System.Web.UI;

namespace Tank.Request
{
	// Token: 0x0200006D RID: 109
	public class SendItemTest : Page
	{
		// Token: 0x060001F0 RID: 496 RVA: 0x0000EA04 File Offset: 0x0000CC04
		protected void Page_Load(object sender, EventArgs e)
		{
			HttpCookie aCookie = base.Request.Cookies["userInfo"];
			string value = aCookie.Value;
			string userName = aCookie.Values["bd_sig_user"];
			base.Response.Write(value);
		}
	}
}
