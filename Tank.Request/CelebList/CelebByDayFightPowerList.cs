using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using log4net;

namespace Tank.Request.CelebList
{
	// Token: 0x02000090 RID: 144
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class CelebByDayFightPowerList : IHttpHandler
	{
		// Token: 0x0600029C RID: 668 RVA: 0x00002E85 File Offset: 0x00001085
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(CelebByDayFightPowerList.Build(context));
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00011D24 File Offset: 0x0000FF24
		public static string Build(HttpContext context)
		{
			bool flag = !csFunction.ValidAdminIP(context.Request.UserHostAddress);
			string result;
			if (flag)
			{
				result = "CelebByDayFightPowerList Fail!";
			}
			else
			{
				result = CelebByDayFightPowerList.Build();
			}
			return result;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x00011D5C File Offset: 0x0000FF5C
		public static string Build()
		{
			return csFunction.BuildCelebUsers("CelebByDayFightPowerList", 6, "CelebByDayFightPowerList_Out");
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600029F RID: 671 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x040000A5 RID: 165
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
