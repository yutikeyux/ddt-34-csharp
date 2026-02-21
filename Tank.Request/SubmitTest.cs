using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Bussiness;

namespace Tank.Request
{
	// Token: 0x02000074 RID: 116
	public class SubmitTest : Page
	{
		// Token: 0x0600021B RID: 539 RVA: 0x000105E4 File Offset: 0x0000E7E4
		protected void Page_Load(object sender, EventArgs e)
		{
			using (new ConsortiaBussiness())
			{
			}
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00010618 File Offset: 0x0000E818
		protected void Button1_Click(object sender, EventArgs e)
		{
			base.Response.Redirect("/LoginTest.aspx?name=" + this.TextBox1.Text);
		}

		// Token: 0x04000080 RID: 128
		protected HtmlForm form1;

		// Token: 0x04000081 RID: 129
		protected TextBox TextBox1;

		// Token: 0x04000082 RID: 130
		protected Button Button1;
	}
}
