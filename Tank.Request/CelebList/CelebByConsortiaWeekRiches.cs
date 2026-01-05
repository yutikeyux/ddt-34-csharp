using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using log4net;

namespace Tank.Request.CelebList
{
	// Token: 0x0200008E RID: 142
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class CelebByConsortiaWeekRiches : IHttpHandler
	{
		// Token: 0x06000290 RID: 656 RVA: 0x00002E2F File Offset: 0x0000102F
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(CelebByConsortiaWeekRiches.Build(context));
		}

		// Token: 0x06000291 RID: 657 RVA: 0x00011B98 File Offset: 0x0000FD98
		public static string Build(HttpContext context)
		{
			bool flag = !csFunction.ValidAdminIP(context.Request.UserHostAddress);
			string result;
			if (flag)
			{
				result = "CelebByConsortiaWeekRiches Fail!";
			}
			else
			{
				result = CelebByConsortiaWeekRiches.Build();
			}
			return result;
		}

		// Token: 0x06000292 RID: 658 RVA: 0x00011BD0 File Offset: 0x0000FDD0
		public static string Build()
		{
			return csFunction.BuildCelebConsortia("CelebByConsortiaWeekRiches", 12, "CelebByConsortiaWeekRiches_Out");
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000293 RID: 659 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x040000A3 RID: 163
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
