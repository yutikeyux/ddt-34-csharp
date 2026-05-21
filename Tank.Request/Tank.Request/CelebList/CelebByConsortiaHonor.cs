using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using log4net;

namespace Tank.Request.CelebList
{
	// Token: 0x02000086 RID: 134
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class CelebByConsortiaHonor : IHttpHandler
	{
		// Token: 0x06000272 RID: 626 RVA: 0x00012351 File Offset: 0x00010551
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(CelebByConsortiaHonor.Build(context));
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00012368 File Offset: 0x00010568
		public static string Build(HttpContext context)
		{
			bool flag = !csFunction.ValidAdminIP(context.Request.UserHostAddress);
			string result;
			if (flag)
			{
				result = "CelebByConsortiaHonor Başarısız!";
			}
			else
			{
				result = CelebByConsortiaHonor.Build();
			}
			return result;
		}

		// Token: 0x06000274 RID: 628 RVA: 0x000123A0 File Offset: 0x000105A0
		public static string Build()
		{
			return csFunction.BuildCelebConsortia("CelebByConsortiaHonor", 13, "CelebByConsortiaHonor_Out");
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000275 RID: 629 RVA: 0x000123C4 File Offset: 0x000105C4
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000099 RID: 153
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
