using System;
using System.Configuration;
using System.Web;
using System.Web.Services;
using Bussiness.Interface;

namespace Tank.Request
{
	// Token: 0x02000014 RID: 20
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class ChargeTest : IHttpHandler
	{
		// Token: 0x06000052 RID: 82 RVA: 0x000042A4 File Offset: 0x000024A4
		public void ProcessRequest(HttpContext context)
		{
			try
			{
				string chargeID = context.Request["chargeID"];
				string userName = context.Request["userName"];
				int money = int.Parse(context.Request["money"]);
				string payWay = context.Request["payWay"];
				decimal needMoney = decimal.Parse(context.Request["needMoney"]);
				string nickname = (context.Request["nickname"] == null) ? "" : HttpUtility.UrlDecode(context.Request["nickname"]);
				string chargekey = HttpUtility.UrlDecode(context.Request["chargekey"]);
				string site = "";
				QYInterface qy = new QYInterface();
				string key = string.Empty;
				bool flag = !string.IsNullOrEmpty(site);
				if (flag)
				{
					key = ConfigurationSettings.AppSettings[string.Format("ChargeKey_a", site)];
				}
				else
				{
					key = BaseInterface.GetChargeKey;
				}
				string v = BaseInterface.md5(string.Concat(new string[]
				{
					chargeID,
					userName,
					money.ToString(),
					payWay,
					needMoney.ToString(),
					key
				}));
				string Url = string.Concat(new string[]
				{
					"http://gn-quest.3brogames.com/ChargeMoney.aspx?content=",
					chargeID,
					"|",
					userName,
					"|",
					money.ToString(),
					"|",
					payWay,
					"|",
					needMoney.ToString(),
					"|",
					v
				});
				Url = Url + "&site=" + site;
				Url = Url + "&nickname=" + HttpUtility.UrlEncode(nickname);
				context.Response.Write(BaseInterface.RequestContent(Url));
			}
			catch (Exception ex)
			{
				context.Response.Write(ex.ToString());
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000053 RID: 83 RVA: 0x000044B4 File Offset: 0x000026B4
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}
