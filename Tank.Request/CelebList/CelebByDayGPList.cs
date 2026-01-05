using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using log4net;

namespace Tank.Request.CelebList
{
	// Token: 0x02000091 RID: 145
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class CelebByDayGPList : IHttpHandler
	{
		// Token: 0x060002A2 RID: 674 RVA: 0x00002EB0 File Offset: 0x000010B0
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(CelebByDayGPList.Build(context));
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00011D80 File Offset: 0x0000FF80
		public static string Build(HttpContext context)
		{
			bool flag = !csFunction.ValidAdminIP(context.Request.UserHostAddress);
			string result;
			if (flag)
			{
				result = "CelebByDayGPList Fail!";
			}
			else
			{
				result = CelebByDayGPList.Build();
			}
			return result;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00011DB8 File Offset: 0x0000FFB8
		public static string Build()
		{
			return csFunction.BuildCelebUsers("CelebByDayGPList", 2, "CelebForUsersByDay");
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x040000A6 RID: 166
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
