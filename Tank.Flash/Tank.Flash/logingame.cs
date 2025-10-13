using System;
using System.Configuration;
using System.Web;
using System.Web.UI;
using Bussiness.Interface;
using Road.Flash;

namespace Tank.Flash
{
	// Token: 0x02000010 RID: 16
	public class logingame : Page
	{
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00002DD7 File Offset: 0x00000FD7
		public string LoginOnUrl
		{
			get
			{
				return ConfigurationManager.AppSettings["LoginOnUrl"];
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00002DE8 File Offset: 0x00000FE8
		public static string FlashUrl
		{
			get
			{
				return ConfigurationManager.AppSettings["FlashUrl"];
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002DFC File Offset: 0x00000FFC
		protected void Page_Load(object sender, EventArgs e)
		{
			if (this.Session["username"] == null && string.IsNullOrEmpty(this.Session["username"].ToString()))
			{
				base.Response.Redirect(this.LoginOnUrl, false);
			}
			else if (!LoadingManager.Login(this.Session["username"].ToString(), this.Session["password"].ToString()))
			{
				base.Response.Redirect(this.LoginOnUrl, false);
			}
			try
			{
				string text = this.Session["username"].ToString();
				string text2 = Guid.NewGuid().ToString();
				string text3 = BaseInterface.ConvertDateTimeInt(DateTime.Now).ToString();
				string text4 = string.Empty;
				if (string.IsNullOrEmpty(text4))
				{
					text4 = BaseInterface.GetLoginKey;
				}
				string text5 = BaseInterface.md5(text + text2 + text3.ToString() + text4);
				string url = BaseInterface.LoginUrl + "?content=" + HttpUtility.UrlEncode(string.Concat(new string[]
				{
					text,
					"|",
					text2,
					"|",
					text3.ToString(),
					"|",
					text5
				}));
				string text6 = BaseInterface.RequestContent(url);
				if (text6 == "0")
				{
					string s = string.Concat(new string[]
					{
						logingame.FlashUrl,
						"?user=",
						HttpUtility.UrlEncode(text),
						"&key=",
						HttpUtility.UrlEncode(text2)
					});
					if ("1" == ConfigurationManager.AppSettings["content2"])
					{
						text2 = ((this.Session["password"] == null) ? text2 : this.Session["password"].ToString());
						string text7 = "5C90D3C2C576A773";
						string privateKey = "5628eb9a3485fbf61f51927b8a8eee03c5962c6b64847aeb";
						string plainText = text + "|" + text2;
						s = logingame.FlashUrl + "?content2=" + CryptoHelper.TripleDesEncrypt(privateKey, plainText, ref text7);
					}
					base.Response.Write(s);
				}
				else
				{
					base.Response.Write(text6);
				}
			}
			catch (Exception ex)
			{
				base.Response.Write(ex.ToString());
			}
		}
	}
}
