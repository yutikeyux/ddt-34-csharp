using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using log4net;

namespace Tank.Request
{
	// Token: 0x02000046 RID: 70
	public class KitoffUser : Page
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600013F RID: 319 RVA: 0x000035C4 File Offset: 0x000017C4
		public static string GetAdminIP
		{
			get
			{
				return ConfigurationManager.AppSettings["AdminIP"];
			}
		}

		// Token: 0x06000140 RID: 320 RVA: 0x0000A730 File Offset: 0x00008930
		public static bool ValidLoginIP(string ip)
		{
			string ips = KitoffUser.GetAdminIP;
			return string.IsNullOrEmpty(ips) || ips.Split(new char[]
			{
				'|'
			}).Contains(ip);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0000A774 File Offset: 0x00008974
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

		// Token: 0x0400004A RID: 74
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
