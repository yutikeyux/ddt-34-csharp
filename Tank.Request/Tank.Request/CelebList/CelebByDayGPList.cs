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
	public class CelebByDayGPList : IHttpHandler
	{
		// Token: 0x0600029C RID: 668 RVA: 0x00012869 File Offset: 0x00010A69
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(CelebByDayGPList.Build(context));
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00012880 File Offset: 0x00010A80
		public static string Build(HttpContext context)
		{
			bool flag = !csFunction.ValidAdminIP(context.Request.UserHostAddress);
			string result;
			if (flag)
			{
				result = "CelebByDayGPList Başarısız!";
			}
			else
			{
				result = CelebByDayGPList.Build();
			}
			return result;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x000128B8 File Offset: 0x00010AB8
		public static string Build()
		{
			return csFunction.BuildCelebUsers("CelebByDayGPList", 2, "CelebForUsersByDay");
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600029F RID: 671 RVA: 0x000128DC File Offset: 0x00010ADC
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
