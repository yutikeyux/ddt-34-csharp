using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using log4net;

namespace Tank.Request.CelebList
{
	// Token: 0x02000084 RID: 132
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class CelebByConsortiaDayRiches : IHttpHandler
	{
		// Token: 0x06000266 RID: 614 RVA: 0x00012219 File Offset: 0x00010419
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(CelebByConsortiaDayRiches.Build(context));
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00012230 File Offset: 0x00010430
		public static string Build(HttpContext context)
		{
			bool flag = !csFunction.ValidAdminIP(context.Request.UserHostAddress);
			string result;
			if (flag)
			{
				result = "CelebByConsortiaDayRiches Başarısız!";
			}
			else
			{
				result = CelebByConsortiaDayRiches.Build();
			}
			return result;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00012268 File Offset: 0x00010468
		public static string Build()
		{
			return csFunction.BuildCelebConsortia("CelebByConsortiaDayRiches", 11, "CelebByConsortiaDayRiches_Out");
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000269 RID: 617 RVA: 0x0001228C File Offset: 0x0001048C
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000097 RID: 151
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
