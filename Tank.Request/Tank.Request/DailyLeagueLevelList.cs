using System;
using System.Reflection;
using System.Web;
using System.Xml.Linq;
using Bussiness;
using log4net;
using Road.Flash;
using SqlDataProvider.Data;

namespace Tank.Request
{
	// Token: 0x0200002B RID: 43
	public class DailyLeagueLevelList : IHttpHandler
	{
		// Token: 0x060000BC RID: 188 RVA: 0x0000717C File Offset: 0x0000537C
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(DailyLeagueLevelList.Build(context));
			}
			else
			{
				context.Response.Write("Erişim yetkisi reddedildi!");
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000BD RID: 189 RVA: 0x000071C8 File Offset: 0x000053C8
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000071DC File Offset: 0x000053DC
		public static string Build(HttpContext context)
		{
			bool value = false;
			string message = "Başarısız!";
			XElement result = new XElement("Result");
			try
			{
				using (ProduceBussiness db = new ProduceBussiness())
				{
					FairBattleRewardInfo[] infos = db.GetAllFairBattleReward();
					foreach (FairBattleRewardInfo info in infos)
					{
						result.Add(FlashUtils.CreateFairBattleReward(info));
					}
				}
				value = true;
				message = "Success!";
			}
			catch (Exception ex)
			{
				DailyLeagueLevelList.log.Error("dailyleaguelevel", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			csFunction.CreateCompressXml(context, result, "dailyleaguelevel_out", false);
			return csFunction.CreateCompressXml(context, result, "dailyleaguelevel", true);
		}

		// Token: 0x04000028 RID: 40
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
