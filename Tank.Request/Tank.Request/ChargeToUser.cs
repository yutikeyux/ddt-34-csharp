using System;
using System.Reflection;
using System.Web;
using System.Web.UI;
using Bussiness;
using Bussiness.CenterService;
using log4net;
using SqlDataProvider.Data;

namespace Tank.Request
{
	// Token: 0x02000015 RID: 21
	public class ChargeToUser : Page
	{
		// Token: 0x06000055 RID: 85 RVA: 0x000044C8 File Offset: 0x000026C8
		protected void Page_Load(object sender, EventArgs e)
		{
			string result = "false";
			try
			{
				int userID = Convert.ToInt32(HttpUtility.UrlDecode(base.Request["userID"]));
				string chargeID = HttpUtility.UrlDecode(base.Request["chargeID"]);
				using (CenterServiceClient centerServiceClient = new CenterServiceClient())
				{
					centerServiceClient.ChargeMoney(userID, chargeID);
					using (PlayerBussiness playerBussiness2 = new PlayerBussiness())
					{
						PlayerInfo userSingleByUserId = playerBussiness2.GetUserSingleByUserID(userID);
						bool flag = userSingleByUserId != null;
						if (flag)
						{
							result = "ok";
						}
						else
						{
							result = "null";
							ChargeToUser.log.Error("ChargeMoney_StaticsMgr:Player is null!");
						}
					}
				}
			}
			catch (Exception ex)
			{
				ChargeToUser.log.Error("ChargeMoney:", ex);
			}
			base.Response.Write(result);
		}

		// Token: 0x04000012 RID: 18
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
