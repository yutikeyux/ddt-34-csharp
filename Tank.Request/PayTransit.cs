using System;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Services;
using log4net;

namespace Tank.Request
{
	// Token: 0x02000060 RID: 96
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class PayTransit : IHttpHandler
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x0000D7A0 File Offset: 0x0000B9A0
		public string PayURL
		{
			get
			{
				string login = "PayURL_" + this.site;
				return ConfigurationManager.AppSettings[login];
			}
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000D7D0 File Offset: 0x0000B9D0
		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			string username = "";
			string url = string.Empty;
			try
			{
				bool flag = !string.IsNullOrEmpty(context.Request["username"]);
				if (flag)
				{
					username = HttpUtility.UrlDecode(context.Request["username"].Trim());
				}
				this.site = ((context.Request["site"] == null) ? "" : HttpUtility.UrlDecode(context.Request["site"]).ToLower());
				bool flag2 = !string.IsNullOrEmpty(this.site);
				if (flag2)
				{
					url = this.PayURL;
					int place = username.IndexOf('_');
					bool flag3 = place != -1;
					if (flag3)
					{
						username = username.Substring(place + 1, username.Length - place - 1);
					}
				}
				bool flag4 = string.IsNullOrEmpty(url);
				if (flag4)
				{
					url = ConfigurationManager.AppSettings["PayURL"];
				}
				context.Response.Redirect(string.Format(url, username, this.site), false);
			}
			catch (Exception ex)
			{
				PayTransit.log.Error("PayTransit:", ex);
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000061 RID: 97
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Token: 0x04000062 RID: 98
		private string site = "";
	}
}
