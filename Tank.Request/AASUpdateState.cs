using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using Bussiness.CenterService;
using log4net;

namespace Tank.Request
{
	// Token: 0x02000004 RID: 4
	public class AASUpdateState : Page
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000A RID: 10 RVA: 0x000026A8 File Offset: 0x000008A8
		public static string GetAdminIP
		{
			get
			{
				return ConfigurationManager.AppSettings["AdminIP"];
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000026CC File Offset: 0x000008CC
		public static bool ValidLoginIP(string ip)
		{
			string ips = AASUpdateState.GetAdminIP;
			return string.IsNullOrEmpty(ips) || ips.Split(new char[]
			{
				'|'
			}).Contains(ip);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002710 File Offset: 0x00000910
		protected void Page_Load(object sender, EventArgs e)
		{
			int result = 2;
			try
			{
				bool state = bool.Parse(base.Request["state"]);
				bool flag = AASUpdateState.ValidLoginIP(this.Context.Request.UserHostAddress);
				if (flag)
				{
					using (CenterServiceClient temp = new CenterServiceClient())
					{
						bool flag2 = temp.AASUpdateState(state);
						if (flag2)
						{
							result = 0;
						}
						else
						{
							result = 1;
						}
					}
				}
			}
			catch (Exception ex)
			{
				AASUpdateState.log.Error("ASSUpdateState:", ex);
			}
			base.Response.Write(result);
		}

		// Token: 0x04000003 RID: 3
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
