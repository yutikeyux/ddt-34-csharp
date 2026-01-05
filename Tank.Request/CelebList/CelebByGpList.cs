using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using log4net;

namespace Tank.Request.CelebList
{
	// Token: 0x02000093 RID: 147
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class CelebByGpList : IHttpHandler
	{
		// Token: 0x060002AE RID: 686 RVA: 0x00002F06 File Offset: 0x00001106
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(CelebByGpList.Build(context));
		}

		// Token: 0x060002AF RID: 687 RVA: 0x00011E38 File Offset: 0x00010038
		public static string Build(HttpContext context)
		{
			bool flag = !csFunction.ValidAdminIP(context.Request.UserHostAddress);
			string result;
			if (flag)
			{
				result = "CelebByGpList Fail!";
			}
			else
			{
				result = CelebByGpList.Build();
			}
			return result;
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x00011E70 File Offset: 0x00010070
		public static string Build()
		{
			return csFunction.BuildCelebUsers("CelebByGpList", 0, "CelebForUsers");
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x040000A8 RID: 168
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
