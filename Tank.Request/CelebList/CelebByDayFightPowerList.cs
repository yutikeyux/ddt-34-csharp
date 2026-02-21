using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using log4net;

namespace Tank.Request.CelebList
{
	// Token: 0x0200008C RID: 140
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class CelebByDayFightPowerList : IHttpHandler
	{
		// Token: 0x06000296 RID: 662 RVA: 0x000127CD File Offset: 0x000109CD
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(CelebByDayFightPowerList.Build(context));
		}

		// Token: 0x06000297 RID: 663 RVA: 0x000127E4 File Offset: 0x000109E4
		public static string Build(HttpContext context)
		{
			bool flag = !csFunction.ValidAdminIP(context.Request.UserHostAddress);
			string result;
			if (flag)
			{
				result = "CelebByDayFightPowerList Fail!";
			}
			else
			{
				result = CelebByDayFightPowerList.Build();
			}
			return result;
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0001281C File Offset: 0x00010A1C
		public static string Build()
		{
			return csFunction.BuildCelebUsers("CelebByDayFightPowerList", 6, "CelebByDayFightPowerList_Out");
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000299 RID: 665 RVA: 0x00012840 File Offset: 0x00010A40
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400009F RID: 159
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
