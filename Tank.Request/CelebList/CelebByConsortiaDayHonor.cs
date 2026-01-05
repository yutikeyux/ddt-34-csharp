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
	public class CelebByConsortiaDayHonor : IHttpHandler
	{
		// Token: 0x06000266 RID: 614 RVA: 0x00002D02 File Offset: 0x00000F02
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(CelebByConsortiaDayHonor.Build(context));
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00011914 File Offset: 0x0000FB14
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

		// Token: 0x06000268 RID: 616 RVA: 0x0001194C File Offset: 0x0000FB4C
		public static string Build()
		{
			return csFunction.BuildCelebConsortia("CelebByConsortiaDayHonor", 14, "CelebByConsortiaDayHonor_Out");
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000269 RID: 617 RVA: 0x00003828 File Offset: 0x00001A28
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
