using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using Bussiness.CenterService;
using Bussiness.Interface;
using log4net;

namespace Tank.Request
{
	// Token: 0x0200005E RID: 94
	public class NoticeServerUpdate : Page
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x000035C4 File Offset: 0x000017C4
		public static string GetAdminIP
		{
			get
			{
				return ConfigurationManager.AppSettings["AdminIP"];
			}
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000D510 File Offset: 0x0000B710
		public static bool ValidLoginIP(string ip)
		{
			string ips = NoticeServerUpdate.GetAdminIP;
			return string.IsNullOrEmpty(ips) || ips.Split(new char[]
			{
				'|'
			}).Contains(ip);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000D554 File Offset: 0x0000B754
		protected void Page_Load(object sender, EventArgs e)
		{
			int result = 2;
			try
			{
				int serverID = int.Parse(this.Context.Request["serverID"]);
				int type = int.Parse(this.Context.Request["type"]);
				bool flag = NoticeServerUpdate.ValidLoginIP(this.Context.Request.UserHostAddress);
				if (flag)
				{
					using (CenterServiceClient temp = new CenterServiceClient())
					{
						result = temp.NoticeServerUpdate(serverID, type);
					}
					int num = type;
					int num2 = num;
					if (num2 == 5)
					{
						bool flag2 = result == 0;
						if (flag2)
						{
							result = this.HandleServerMapUpdate();
						}
					}
				}
				else
				{
					result = 5;
				}
			}
			catch (Exception ex)
			{
				NoticeServerUpdate.log.Error("ExperienceRateUpdate:", ex);
				result = 4;
			}
			base.Response.Write(result);
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000D650 File Offset: 0x0000B850
		private int HandleServerMapUpdate()
		{
			string Url = "http://" + HttpContext.Current.Request.Url.Authority.ToString() + "/MapServerList.ashx";
			string strRlt = BaseInterface.RequestContent(Url);
			bool flag = strRlt.Contains("Success");
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = 3;
			}
			return result;
		}

		// Token: 0x0400005F RID: 95
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
