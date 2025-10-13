using System;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Tank.Flash
{
	// Token: 0x0200000A RID: 10
	public class playgame : Page
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001B RID: 27 RVA: 0x000025FB File Offset: 0x000007FB
		public string Content
		{
			get
			{
				return this._content;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002603 File Offset: 0x00000803
		public string LoginOnUrl
		{
			get
			{
				return ConfigurationManager.AppSettings["LoginOnUrl"];
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600001D RID: 29 RVA: 0x00002614 File Offset: 0x00000814
		public string SiteTitle
		{
			get
			{
				if (ConfigurationManager.AppSettings["SiteTitle"] != null)
				{
					return ConfigurationManager.AppSettings["SiteTitle"];
				}
				return "DDTank";
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600001E RID: 30 RVA: 0x0000263C File Offset: 0x0000083C
		public string Config
		{
			get
			{
				return ConfigurationManager.AppSettings["FlashConfig"];
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600001F RID: 31 RVA: 0x0000264D File Offset: 0x0000084D
		public string Flash
		{
			get
			{
				return ConfigurationManager.AppSettings["FlashSite"];
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000020 RID: 32 RVA: 0x0000265E File Offset: 0x0000085E
		public string AutoParam
		{
			get
			{
				return this.autoParam;
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002668 File Offset: 0x00000868
		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
				if (this.Session["username"] == null && string.IsNullOrEmpty(this.Session["username"].ToString()))
				{
					base.Response.Redirect(this.LoginOnUrl, false);
				}
				else if (!LoadingManager.Login(base.Request["user"], this.Session["password"].ToString()))
				{
					LoadingManager.Remove(this.Session["username"].ToString());
					base.Response.Redirect(this.LoginOnUrl, false);
				}
				else if ("1" == ConfigurationManager.AppSettings["content2"])
				{
					string text = base.Request["content2"];
					if (!string.IsNullOrEmpty(text))
					{
						this._content = text;
					}
					else
					{
						base.Response.Redirect(this.LoginOnUrl, false);
					}
				}
				else
				{
					string text2 = HttpUtility.UrlDecode(base.Request["user"]);
					string text3 = HttpUtility.UrlDecode(base.Request["key"]);
					HttpUtility.UrlDecode(base.Request["config"]);
					string str = (base.Request["editby"] == null) ? "" : HttpUtility.UrlDecode(base.Request["editby"]);
					if (!string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text3))
					{
						this._content = "user=" + HttpUtility.UrlEncode(text2) + "&key=" + HttpUtility.UrlEncode(text3);
						this.autoParam = "editby=" + HttpUtility.UrlEncode(str);
					}
					else
					{
						base.Response.Redirect(this.LoginOnUrl, false);
					}
				}
			}
			catch
			{
				base.Response.Redirect(this.LoginOnUrl, false);
			}
		}

		// Token: 0x0400000A RID: 10
		private string _content = "";

		// Token: 0x0400000B RID: 11
		private string autoParam = "";

		// Token: 0x0400000C RID: 12
		protected HtmlHead Head1;
	}
}
