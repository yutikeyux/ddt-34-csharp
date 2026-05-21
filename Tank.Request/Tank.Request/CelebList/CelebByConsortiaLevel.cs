using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using log4net;

namespace Tank.Request.CelebList
{
	// Token: 0x02000087 RID: 135
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class CelebByConsortiaLevel : IHttpHandler
	{
		// Token: 0x06000278 RID: 632 RVA: 0x000123ED File Offset: 0x000105ED
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(CelebByConsortiaLevel.Build(context));
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00012404 File Offset: 0x00010604
		public static string Build(HttpContext context)
		{
			bool flag = !csFunction.ValidAdminIP(context.Request.UserHostAddress);
			string result;
			if (flag)
			{
				result = "CelebByConsortiaLevel Başarısız!";
			}
			else
			{
				result = CelebByConsortiaLevel.Build();
			}
			return result;
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0001243C File Offset: 0x0001063C
		public static string Build()
		{
			return csFunction.BuildCelebConsortia("CelebByConsortiaLevel", 16, "CelebForConsortia");
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600027B RID: 635 RVA: 0x00012460 File Offset: 0x00010660
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400009A RID: 154
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
