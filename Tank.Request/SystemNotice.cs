using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using Bussiness.CenterService;
using log4net;

namespace Tank.Request
{
	// Token: 0x0200007B RID: 123
	public class SystemNotice : Page
	{
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600022E RID: 558 RVA: 0x000035C4 File Offset: 0x000017C4
		public static string GetChargeIP
		{
			get
			{
				return ConfigurationManager.AppSettings["AdminIP"];
			}
		}

		// Token: 0x0600022F RID: 559 RVA: 0x00010260 File Offset: 0x0000E460
		public static bool ValidLoginIP(string ip)
		{
			string ips = SystemNotice.GetChargeIP;
			return string.IsNullOrEmpty(ips) || ips.Split(new char[]
			{
				'|'
			}).Contains(ip);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x000102A4 File Offset: 0x0000E4A4
		protected void Page_Load(object sender, EventArgs e)
		{
			int result = 1;
			try
			{
				bool flag = SystemNotice.ValidLoginIP(this.Context.Request.UserHostAddress);
				if (flag)
				{
					string content = HttpUtility.UrlDecode(base.Request["content"]);
					bool flag2 = !string.IsNullOrEmpty(content);
					if (flag2)
					{
						using (CenterServiceClient temp = new CenterServiceClient())
						{
							bool flag3 = temp.SystemNotice(content);
							if (flag3)
							{
								result = 0;
							}
						}
					}
				}
				else
				{
					result = 2;
				}
			}
			catch (Exception ex)
			{
				SystemNotice.log.Error("SystemNotice:", ex);
			}
			base.Response.Write(result);
		}

		// Token: 0x0400008B RID: 139
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
