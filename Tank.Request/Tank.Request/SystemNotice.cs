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
	// Token: 0x02000077 RID: 119
	public class SystemNotice : Page
	{
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000228 RID: 552 RVA: 0x0001091C File Offset: 0x0000EB1C
		public static string GetChargeIP
		{
			get
			{
				return ConfigurationSettings.AppSettings["AdminIP"];
			}
		}

		// Token: 0x06000229 RID: 553 RVA: 0x00010940 File Offset: 0x0000EB40
		public static bool ValidLoginIP(string ip)
		{
			string ips = SystemNotice.GetChargeIP;
			return string.IsNullOrEmpty(ips) || ips.Split(new char[]
			{
				'|'
			}).Contains(ip);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00010984 File Offset: 0x0000EB84
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

		// Token: 0x04000085 RID: 133
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
