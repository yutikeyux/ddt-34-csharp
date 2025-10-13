using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using log4net;

namespace Tank.Request.CelebList
{
	// Token: 0x02000089 RID: 137
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class CelebByConsortiaWeekHonor : IHttpHandler
	{
		// Token: 0x06000284 RID: 644 RVA: 0x00012525 File Offset: 0x00010725
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(CelebByConsortiaWeekHonor.Build(context));
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0001253C File Offset: 0x0001073C
		public static string Build(HttpContext context)
		{
			bool flag = !csFunction.ValidAdminIP(context.Request.UserHostAddress);
			string result;
			if (flag)
			{
				result = "CelebByConsortiaWeekHonor Fail!";
			}
			else
			{
				result = CelebByConsortiaWeekHonor.Build();
			}
			return result;
		}

		// Token: 0x06000286 RID: 646 RVA: 0x00012574 File Offset: 0x00010774
		public static string Build()
		{
			return csFunction.BuildCelebConsortia("CelebByConsortiaWeekHonor", 15, "CelebByConsortiaWeekHonor_Out");
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000287 RID: 647 RVA: 0x00012598 File Offset: 0x00010798
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400009C RID: 156
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
