using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using log4net;

namespace Tank.Request.CelebList
{
	// Token: 0x0200008A RID: 138
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class CelebByConsortiaWeekRiches : IHttpHandler
	{
		// Token: 0x0600028A RID: 650 RVA: 0x000125C1 File Offset: 0x000107C1
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(CelebByConsortiaWeekRiches.Build(context));
		}

		// Token: 0x0600028B RID: 651 RVA: 0x000125D8 File Offset: 0x000107D8
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

		// Token: 0x0600028C RID: 652 RVA: 0x00012610 File Offset: 0x00010810
		public static string Build()
		{
			return csFunction.BuildCelebConsortia("CelebByConsortiaWeekRiches", 12, "CelebByConsortiaWeekRiches_Out");
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600028D RID: 653 RVA: 0x00012634 File Offset: 0x00010834
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400009D RID: 157
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
