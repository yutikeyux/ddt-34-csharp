using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using log4net;

namespace Tank.Request.CelebList
{
	// Token: 0x02000088 RID: 136
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class CelebByConsortiaDayRiches : IHttpHandler
	{
		// Token: 0x0600026C RID: 620 RVA: 0x00002D2D File Offset: 0x00000F2D
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(CelebByConsortiaDayRiches.Build(context));
		}

		// Token: 0x0600026D RID: 621 RVA: 0x00011970 File Offset: 0x0000FB70
		public static string Build(HttpContext context)
		{
			bool flag = !csFunction.ValidAdminIP(context.Request.UserHostAddress);
			string result;
			if (flag)
			{
				result = "CelebByConsortiaDayRiches Fail!";
			}
			else
			{
				result = CelebByConsortiaDayRiches.Build();
			}
			return result;
		}

		// Token: 0x0600026E RID: 622 RVA: 0x000119A8 File Offset: 0x0000FBA8
		public static string Build()
		{
			return csFunction.BuildCelebConsortia("CelebByConsortiaDayRiches", 11, "CelebByConsortiaDayRiches_Out");
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600026F RID: 623 RVA: 0x00003828 File Offset: 0x00001A28
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
