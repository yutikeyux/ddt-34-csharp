using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using log4net;

namespace Tank.Request.CelebList
{
	// Token: 0x0200008D RID: 141
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class CelebByConsortiaWeekHonor : IHttpHandler
	{
		// Token: 0x0600028A RID: 650 RVA: 0x00002E04 File Offset: 0x00001004
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(CelebByConsortiaWeekHonor.Build(context));
		}

		// Token: 0x0600028B RID: 651 RVA: 0x00011B3C File Offset: 0x0000FD3C
		public static string Build(HttpContext context)
		{
			bool flag = !csFunction.ValidAdminIP(context.Request.UserHostAddress);
			string result;
			if (flag)
			{
				result = "CelebByConsortiaWeekHonor Fail!";
			}
			else
			{
				result = CelebByConsortiaWeekHonor.Build();
			}
			return result;
		}

		// Token: 0x0600028C RID: 652 RVA: 0x00011B74 File Offset: 0x0000FD74
		public static string Build()
		{
			return csFunction.BuildCelebConsortia("CelebByConsortiaWeekHonor", 15, "CelebByConsortiaWeekHonor_Out");
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600028D RID: 653 RVA: 0x00003828 File Offset: 0x00001A28
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
