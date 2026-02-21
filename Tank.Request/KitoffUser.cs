using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using log4net;

namespace Tank.Request
{
	// Token: 0x02000044 RID: 68
	public class KitoffUser : Page
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600013D RID: 317 RVA: 0x0000A51C File Offset: 0x0000871C
		public static string GetAdminIP
		{
			get
			{
				return ConfigurationSettings.AppSettings["AdminIP"];
			}
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000A540 File Offset: 0x00008740
		public static bool ValidLoginIP(string ip)
		{
			string ips = KitoffUser.GetAdminIP;
			return string.IsNullOrEmpty(ips) || ips.Split(new char[]
			{
				'|'
			}).Contains(ip);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0000A584 File Offset: 0x00008784
		protected void Page_Load(object sender, EventArgs e)
		{
			bool result = false;
			try
			{
				bool flag = KitoffUser.ValidLoginIP(this.Context.Request.UserHostAddress);
				if (flag)
				{
				}
			}
			catch (Exception ex)
			{
				KitoffUser.log.Error("GetAdminIP:", ex);
			}
			base.Response.Write(result);
		}

		// Token: 0x04000049 RID: 73
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
