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
	public class CelebByConsortiaRiches : IHttpHandler
	{
		// Token: 0x0600027E RID: 638 RVA: 0x00012489 File Offset: 0x00010689
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(CelebByConsortiaRiches.Build(context));
		}

		// Token: 0x0600027F RID: 639 RVA: 0x000124A0 File Offset: 0x000106A0
		public static string Build(HttpContext context)
		{
			bool flag = !csFunction.ValidAdminIP(context.Request.UserHostAddress);
			string result;
			if (flag)
			{
				result = "CelebByConsortiaRiches Başarısız!";
			}
			else
			{
				result = CelebByConsortiaRiches.Build();
			}
			return result;
		}

		// Token: 0x06000280 RID: 640 RVA: 0x000124D8 File Offset: 0x000106D8
		public static string Build()
		{
			return csFunction.BuildCelebConsortia("CelebByConsortiaRiches", 10, "CelebByConsortiaRiches_Out");
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000281 RID: 641 RVA: 0x000124FC File Offset: 0x000106FC
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400009B RID: 155
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
