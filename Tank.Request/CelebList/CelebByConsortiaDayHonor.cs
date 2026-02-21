using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using log4net;

namespace Tank.Request.CelebList
{
	// Token: 0x02000083 RID: 131
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class CelebByConsortiaDayHonor : IHttpHandler
	{
		// Token: 0x06000260 RID: 608 RVA: 0x0001217E File Offset: 0x0001037E
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(CelebByConsortiaDayHonor.Build(context));
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00012194 File Offset: 0x00010394
		public static string Build(HttpContext context)
		{
			bool flag = !csFunction.ValidAdminIP(context.Request.UserHostAddress);
			string result;
			if (flag)
			{
				result = "CelebByConsortiaDayHonor Fail!";
			}
			else
			{
				result = CelebByConsortiaDayHonor.Build();
			}
			return result;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x000121CC File Offset: 0x000103CC
		public static string Build()
		{
			return csFunction.BuildCelebConsortia("CelebByConsortiaDayHonor", 14, "CelebByConsortiaDayHonor_Out");
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000263 RID: 611 RVA: 0x000121F0 File Offset: 0x000103F0
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000096 RID: 150
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
