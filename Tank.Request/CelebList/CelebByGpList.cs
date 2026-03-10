using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using log4net;

namespace Tank.Request.CelebList
{
	// Token: 0x0200008F RID: 143
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class CelebByGpList : IHttpHandler
	{
		// Token: 0x060002A8 RID: 680 RVA: 0x000129A1 File Offset: 0x00010BA1
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(CelebByGpList.Build(context));
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x000129B8 File Offset: 0x00010BB8
		public static string Build(HttpContext context)
		{
			bool flag = !csFunction.ValidAdminIP(context.Request.UserHostAddress);
			string result;
			if (flag)
			{
				result = "CelebByGpList Başarısız!";
			}
			else
			{
				result = CelebByGpList.Build();
			}
			return result;
		}

		// Token: 0x060002AA RID: 682 RVA: 0x000129F0 File Offset: 0x00010BF0
		public static string Build()
		{
			return csFunction.BuildCelebUsers("CelebByGpList", 0, "CelebForUsers");
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060002AB RID: 683 RVA: 0x00012A14 File Offset: 0x00010C14
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x040000A2 RID: 162
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
