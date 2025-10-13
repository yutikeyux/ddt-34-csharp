using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Bussiness.Interface;

namespace Tank.Request
{
	// Token: 0x02000053 RID: 83
	public class LoginTest : Page
	{
		// Token: 0x06000182 RID: 386 RVA: 0x0000C9E4 File Offset: 0x0000ABE4
		protected void Page_Load(object sender, EventArgs e)
		{
			string name = "onelife";
			string pass = "733789";
			int time = 1255165271;
			string loginKey = "yk-MotL-qhpAo88-7road-mtl55dantang-login-logddt777";
			string key = BaseInterface.md5(name + pass + time.ToString() + loginKey);
			string content = "content=" + HttpUtility.UrlEncode(string.Concat(new string[]
			{
				name,
				"|",
				pass,
				"|",
				time.ToString(),
				"|",
				key
			}));
			string str = "http://localhost:728/CreateLogin.aspx?content=" + HttpUtility.UrlEncode(string.Concat(new string[]
			{
				name,
				"|",
				pass,
				"|",
				time.ToString(),
				"|",
				key
			}));
			string url = BaseInterface.RequestContent(str);
			base.Response.Write(url);
		}

		// Token: 0x04000056 RID: 86
		protected HtmlForm form1;
	}
}
