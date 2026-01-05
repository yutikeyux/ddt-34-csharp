using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using log4net;

namespace Tank.Request.CelebList
{
	// Token: 0x0200008B RID: 139
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class CelebByConsortiaLevel : IHttpHandler
	{
		// Token: 0x0600027E RID: 638 RVA: 0x00002DAE File Offset: 0x00000FAE
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(CelebByConsortiaLevel.Build(context));
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00011A84 File Offset: 0x0000FC84
		public static string Build(HttpContext context)
		{
			bool flag = !csFunction.ValidAdminIP(context.Request.UserHostAddress);
			string result;
			if (flag)
			{
				result = "CelebByConsortiaLevel Fail!";
			}
			else
			{
				result = CelebByConsortiaLevel.Build();
			}
			return result;
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00011ABC File Offset: 0x0000FCBC
		public static string Build()
		{
			return csFunction.BuildCelebConsortia("CelebByConsortiaLevel", 16, "CelebForConsortia");
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000281 RID: 641 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x040000A0 RID: 160
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
