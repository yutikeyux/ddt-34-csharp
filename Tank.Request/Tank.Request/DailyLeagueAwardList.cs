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
	// Token: 0x0200002A RID: 42
	public class DailyLeagueAwardList : IHttpHandler
	{
		// Token: 0x060000B7 RID: 183 RVA: 0x00006FFC File Offset: 0x000051FC
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(DailyLeagueAwardList.Build(context));
			}
			else
			{
				context.Response.Write("Erişim yetkisi reddedildi!");
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x00007048 File Offset: 0x00005248
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0000705C File Offset: 0x0000525C
		public static string Build(HttpContext context)
		{
			bool value = false;
			string message = "Başarısız!";
			XElement result = new XElement("Result");
			try
			{
				using (ProduceBussiness db = new ProduceBussiness())
				{
					DailyLeagueAwardInfo[] infos = db.GetAllDailyLeagueAward();
					foreach (DailyLeagueAwardInfo info in infos)
					{
						result.Add(FlashUtils.CreateDailyLeagueAward(info));
					}
				}
				value = true;
				message = "Success!";
			}
			catch (Exception ex)
			{
				DailyLeagueAwardList.log.Error("dailyleagueaward", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			csFunction.CreateCompressXml(context, result, "dailyleagueaward_out", false);
			return csFunction.CreateCompressXml(context, result, "dailyleagueaward", true);
		}

		// Token: 0x04000027 RID: 39
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
