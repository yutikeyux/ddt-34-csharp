using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using log4net;

namespace Tank.Request.CelebList
{
	// Token: 0x02000095 RID: 149
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class CelebByWeekGPList : IHttpHandler
	{
		// Token: 0x060002BA RID: 698 RVA: 0x00002F5C File Offset: 0x0000115C
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(CelebByWeekGPList.Build(context));
		}

		// Token: 0x060002BB RID: 699 RVA: 0x00011EF0 File Offset: 0x000100F0
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

		// Token: 0x060002BC RID: 700 RVA: 0x00011F28 File Offset: 0x00010128
		public static string Build()
		{
			return csFunction.BuildCelebUsers("CelebByWeekGPList", 3, "CelebByWeekGPList_Out");
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060002BD RID: 701 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x040000AA RID: 170
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
