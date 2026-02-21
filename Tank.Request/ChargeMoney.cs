using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using Bussiness;
using Bussiness.CenterService;
using Bussiness.Interface;
using log4net;
using SqlDataProvider.Data;

namespace Tank.Request
{
	// Token: 0x02000013 RID: 19
	public class ChargeMoney : Page
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600004D RID: 77 RVA: 0x00003F85 File Offset: 0x00002185
		public static string GetChargeIP
		{
			get
			{
				return ConfigurationSettings.AppSettings["ChargeIP"];
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003F98 File Offset: 0x00002198
		protected void Page_Load(object sender, EventArgs e)
		{
			int num = 1;
			try
			{
				num = 10;
				string userHostAddress = this.Context.Request.UserHostAddress;
				bool flag = ChargeMoney.ValidLoginIP(userHostAddress);
				if (flag)
				{
					string content = HttpUtility.UrlDecode(base.Request["content"]);
					string site = (base.Request["site"] == null) ? "" : HttpUtility.UrlDecode(base.Request["site"]).ToLower();
					int UserID = Convert.ToInt32(HttpUtility.UrlDecode(base.Request["nickname"]));
					string[] strArray = BaseInterface.CreateInterface().UnEncryptCharge(content, ref num, site);
					bool flag2 = strArray.Length > 5;
					if (flag2)
					{
						string chargeID = strArray[0];
						string user = strArray[1].Trim();
						int money = int.Parse(strArray[2]);
						string str = strArray[3];
						decimal needMoney = decimal.Parse(strArray[4]);
						bool flag3 = !string.IsNullOrEmpty(user);
						if (flag3)
						{
							string nameBySite = BaseInterface.GetNameBySite(user, site);
							bool flag4 = money > 0;
							if (flag4)
							{
								using (PlayerBussiness playerBussiness = new PlayerBussiness())
								{
									int userID = 0;
									DateTime now = DateTime.Now;
									bool flag5 = playerBussiness.AddChargeMoney(chargeID, nameBySite, money, str, needMoney, ref userID, ref num, now, userHostAddress, UserID);
									if (flag5)
									{
										num = 0;
										using (CenterServiceClient centerServiceClient = new CenterServiceClient())
										{
											centerServiceClient.ChargeMoney(userID, chargeID);
											using (PlayerBussiness playerBussiness2 = new PlayerBussiness())
											{
												PlayerInfo userSingleByUserId = playerBussiness2.GetUserSingleByUserID(userID);
												bool flag6 = userSingleByUserId != null;
												if (flag6)
												{
													StaticsMgr.Log(now, nameBySite, userSingleByUserId.Sex, money, str, needMoney);
												}
												else
												{
													StaticsMgr.Log(now, nameBySite, true, money, str, needMoney);
													ChargeMoney.log.Error("ChargeMoney_StaticsMgr:Player is null!");
												}
											}
										}
									}
								}
							}
							else
							{
								num = 3;
							}
						}
						else
						{
							num = 2;
						}
					}
				}
				else
				{
					num = 5;
				}
			}
			catch (Exception ex)
			{
				ChargeMoney.log.Error("ChargeMoney:", ex);
			}
			base.Response.Write(num.ToString() + this.Context.Request.UserHostAddress);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00004248 File Offset: 0x00002448
		public static bool ValidLoginIP(string ip)
		{
			string getChargeIp = ChargeMoney.GetChargeIP;
			int num = string.IsNullOrEmpty(getChargeIp) ? 1 : (getChargeIp.Split(new char[]
			{
				'|'
			}).Contains(ip) ? 1 : 0);
			return num != 0;
		}

		// Token: 0x04000011 RID: 17
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
