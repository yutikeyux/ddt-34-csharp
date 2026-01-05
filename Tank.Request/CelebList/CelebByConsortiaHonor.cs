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
	public class CelebByConsortiaHonor : IHttpHandler
	{
		// Token: 0x06000278 RID: 632 RVA: 0x00002D83 File Offset: 0x00000F83
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(CelebByConsortiaHonor.Build(context));
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00011A28 File Offset: 0x0000FC28
		public static string Build(HttpContext context)
		{
			bool flag = !csFunction.ValidAdminIP(context.Request.UserHostAddress);
			string result;
			if (flag)
			{
				result = "CelebByConsortiaHonor Fail!";
			}
			else
			{
				result = CelebByConsortiaHonor.Build();
			}
			return result;
		}

		// Token: 0x0600027A RID: 634 RVA: 0x00011A60 File Offset: 0x0000FC60
		public static string Build()
		{
			return csFunction.BuildCelebConsortia("CelebByConsortiaHonor", 13, "CelebByConsortiaHonor_Out");
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600027B RID: 635 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400009F RID: 159
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
