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
	public class CelebByWeekGPList : IHttpHandler
	{
		// Token: 0x060002B4 RID: 692 RVA: 0x00012AD9 File Offset: 0x00010CD9
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(CelebByWeekGPList.Build(context));
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x00012AF0 File Offset: 0x00010CF0
		public static string Build(HttpContext context)
		{
			bool flag = !csFunction.ValidAdminIP(context.Request.UserHostAddress);
			string result;
			if (flag)
			{
				result = "CelebByWeekGPList Fail!";
			}
			else
			{
				result = CelebByWeekGPList.Build();
			}
			return result;
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00012B28 File Offset: 0x00010D28
		public static string Build()
		{
			return csFunction.BuildCelebUsers("CelebByWeekGPList", 3, "CelebByWeekGPList_Out");
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x00012B4C File Offset: 0x00010D4C
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x040000A4 RID: 164
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
