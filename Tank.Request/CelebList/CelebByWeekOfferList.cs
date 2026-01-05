using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using log4net;

namespace Tank.Request.CelebList
{
	// Token: 0x02000096 RID: 150
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class CelebByWeekOfferList : IHttpHandler
	{
		// Token: 0x060002C0 RID: 704 RVA: 0x00002F87 File Offset: 0x00001187
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(CelebByWeekOfferList.Build(context));
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x00011F4C File Offset: 0x0001014C
		public static string Build(HttpContext context)
		{
			bool flag = !csFunction.ValidAdminIP(context.Request.UserHostAddress);
			string result;
			if (flag)
			{
				result = "CelebByWeekOfferList Fail!";
			}
			else
			{
				result = CelebByWeekOfferList.Build();
			}
			return result;
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x00011F84 File Offset: 0x00010184
		public static string Build()
		{
			return csFunction.BuildCelebUsers("CelebByWeekOfferList", 5, "CelebByWeekOfferList_Out");
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060002C3 RID: 707 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x040000AB RID: 171
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
