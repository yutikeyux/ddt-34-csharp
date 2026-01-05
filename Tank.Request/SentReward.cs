using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services;
using Bussiness;
using Bussiness.Interface;
using log4net;

namespace Tank.Request
{
	// Token: 0x0200006E RID: 110
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class SentReward : IHttpHandler
	{
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x0000EA50 File Offset: 0x0000CC50
		public static string GetSentRewardIP
		{
			get
			{
				return ConfigurationManager.AppSettings["SentRewardIP"];
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x0000EA74 File Offset: 0x0000CC74
		public static string GetSentRewardKey
		{
			get
			{
				return ConfigurationManager.AppSettings["SentRewardKey"];
			}
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000EA98 File Offset: 0x0000CC98
		public static bool ValidSentRewardIP(string ip)
		{
			string ips = SentReward.GetSentRewardIP;
			return string.IsNullOrEmpty(ips) || ips.Split(new char[]
			{
				'|'
			}).Contains(ip);
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000EADC File Offset: 0x0000CCDC
		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			try
			{
				int result = 1;
				bool flag = SentReward.ValidSentRewardIP(context.Request.UserHostAddress);
				if (flag)
				{
					string content = HttpUtility.UrlDecode(context.Request["content"]);
					string key = SentReward.GetSentRewardKey;
					BaseInterface inter = BaseInterface.CreateInterface();
					string[] str_param = inter.UnEncryptSentReward(content, ref result, key);
					bool flag2 = str_param.Length == 8 && result != 5 && result != 6 && result != 7;
					if (flag2)
					{
						string mailTitle = str_param[0];
						string mailContent = str_param[1];
						string username = str_param[2];
						int gold = int.Parse(str_param[3]);
						int money = int.Parse(str_param[4]);
						string param = str_param[5];
						bool flag3 = this.checkParam(ref param);
						if (flag3)
						{
							PlayerBussiness pb = new PlayerBussiness();
							result = pb.SendMailAndItemByUserName(mailTitle, mailContent, username, gold, money, param);
						}
						else
						{
							result = 4;
						}
					}
				}
				else
				{
					result = 3;
				}
				context.Response.Write(result);
			}
			catch (Exception ex)
			{
				SentReward.log.Error("SentReward", ex);
			}
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000EC0C File Offset: 0x0000CE0C
		private bool checkParam(ref string param)
		{
			int minValidDate = 0;
			string defaultValidDate = "1";
			int maxSrengthenLeverl = 9;
			int minSrengthenLeverl = 0;
			string minAttribute = "0";
			string firstAttribute = "10";
			string secondAttribute = "20";
			string thirdAttribute = "30";
			string fourthAttribute = "40";
			string haveBind = "1";
			string noBind = "0";
			bool flag = !string.IsNullOrEmpty(param);
			if (flag)
			{
				string[] str_Goods = param.Split(new char[]
				{
					'|'
				});
				int int_Length = str_Goods.Length;
				bool flag2 = int_Length > 0;
				if (flag2)
				{
					param = "";
					int i = 0;
					foreach (string str in str_Goods)
					{
						string[] str_GoodsAttribute = str.Split(new char[]
						{
							','
						});
						bool flag3 = str_GoodsAttribute.Length != 0;
						if (flag3)
						{
							str_Goods[i] = "";
							str_GoodsAttribute[2] = ((int.Parse(str_GoodsAttribute[2]) < minValidDate || string.IsNullOrEmpty(str_GoodsAttribute[2].ToString())) ? defaultValidDate : str_GoodsAttribute[2]);
							str_GoodsAttribute[3] = ((int.Parse(str_GoodsAttribute[3].ToString()) < minSrengthenLeverl || int.Parse(str_GoodsAttribute[3].ToString()) > maxSrengthenLeverl || string.IsNullOrEmpty(str_GoodsAttribute[3].ToString())) ? minSrengthenLeverl.ToString() : str_GoodsAttribute[3]);
							str_GoodsAttribute[4] = ((str_GoodsAttribute[4] == minAttribute || str_GoodsAttribute[4] == firstAttribute || str_GoodsAttribute[4] == secondAttribute || str_GoodsAttribute[4] == thirdAttribute || (str_GoodsAttribute[4] == fourthAttribute && !string.IsNullOrEmpty(str_GoodsAttribute[4].ToString()))) ? str_GoodsAttribute[4] : minAttribute);
							str_GoodsAttribute[5] = ((str_GoodsAttribute[5] == minAttribute || str_GoodsAttribute[5] == firstAttribute || str_GoodsAttribute[5] == secondAttribute || str_GoodsAttribute[5] == thirdAttribute || (str_GoodsAttribute[5] == fourthAttribute && !string.IsNullOrEmpty(str_GoodsAttribute[5].ToString()))) ? str_GoodsAttribute[5] : minAttribute);
							str_GoodsAttribute[6] = ((str_GoodsAttribute[6] == minAttribute || str_GoodsAttribute[6] == firstAttribute || str_GoodsAttribute[6] == secondAttribute || str_GoodsAttribute[6] == thirdAttribute || (str_GoodsAttribute[6] == fourthAttribute && !string.IsNullOrEmpty(str_GoodsAttribute[6].ToString()))) ? str_GoodsAttribute[6] : minAttribute);
							str_GoodsAttribute[7] = ((str_GoodsAttribute[7] == minAttribute || str_GoodsAttribute[7] == firstAttribute || str_GoodsAttribute[7] == secondAttribute || str_GoodsAttribute[7] == thirdAttribute || (str_GoodsAttribute[7] == fourthAttribute && !string.IsNullOrEmpty(str_GoodsAttribute[7].ToString()))) ? str_GoodsAttribute[7] : minAttribute);
							str_GoodsAttribute[8] = ((str_GoodsAttribute[8] == haveBind || (str_GoodsAttribute[8] == noBind && !string.IsNullOrEmpty(str_GoodsAttribute[8]))) ? str_GoodsAttribute[8] : haveBind);
						}
						for (int j = 0; j < 9; j++)
						{
							str_Goods[i] = str_Goods[i] + str_GoodsAttribute[j] + ",";
						}
						str_Goods[i] = str_Goods[i].Remove(str_Goods[i].Length - 1, 1);
						i++;
					}
					for (int k = 0; k < int_Length; k++)
					{
						param = param + str_Goods[k] + "|";
					}
					param = param.Remove(param.Length - 1, 1);
					return true;
				}
			}
			return false;
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000075 RID: 117
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
