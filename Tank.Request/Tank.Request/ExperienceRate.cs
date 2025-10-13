using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Bussiness.CenterService;
using log4net;

namespace Tank.Request
{
	// Token: 0x02000033 RID: 51
	public class ExperienceRate : Page
	{
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x000082BC File Offset: 0x000064BC
		public static string GetAdminIP
		{
			get
			{
				return ConfigurationSettings.AppSettings["AdminIP"];
			}
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x000082E0 File Offset: 0x000064E0
		public static bool ValidLoginIP(string ip)
		{
			string ips = ExperienceRate.GetAdminIP;
			return string.IsNullOrEmpty(ips) || ips.Split(new char[]
			{
				'|'
			}).Contains(ip);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00008324 File Offset: 0x00006524
		protected void Page_Load(object sender, EventArgs e)
		{
			int result = 2;
			try
			{
				int serverId = int.Parse(this.Context.Request["serverId"]);
				bool flag = ExperienceRate.ValidLoginIP(this.Context.Request.UserHostAddress);
				if (flag)
				{
					using (CenterServiceClient temp = new CenterServiceClient())
					{
						result = temp.ExperienceRateUpdate(serverId);
					}
				}
			}
			catch (Exception ex)
			{
				ExperienceRate.log.Error("ExperienceRateUpdate:", ex);
			}
			base.Response.Write(result);
		}

		// Token: 0x0400003A RID: 58
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Token: 0x0400003B RID: 59
		protected HtmlForm form1;
	}
}
