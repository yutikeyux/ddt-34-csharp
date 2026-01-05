using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Bussiness;

namespace Tank.Request
{
	// Token: 0x02000078 RID: 120
	public class SubmitTest : Page
	{
		// Token: 0x06000221 RID: 545 RVA: 0x0000FFA4 File Offset: 0x0000E1A4
		protected void Page_Load(object sender, EventArgs e)
		{
			using (new ConsortiaBussiness())
			{
			}
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00002BB1 File Offset: 0x00000DB1
		protected void Button1_Click(object sender, EventArgs e)
		{
			base.Response.Redirect("/LoginTest.aspx?name=" + this.TextBox1.Text);
		}

		// Token: 0x04000086 RID: 134
		protected HtmlForm form1;

		// Token: 0x04000087 RID: 135
		protected TextBox TextBox1;

		// Token: 0x04000088 RID: 136
		protected Button Button1;
	}
}
