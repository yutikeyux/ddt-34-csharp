using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using log4net;

namespace Tank.Request.CelebList
{
	// Token: 0x0200008E RID: 142
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class CelebByDayOfferList : IHttpHandler
	{
		// Token: 0x060002A2 RID: 674 RVA: 0x00012905 File Offset: 0x00010B05
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(CelebByDayOfferList.Build(context));
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0001291C File Offset: 0x00010B1C
		public static string Build(HttpContext context)
		{
			bool flag = !csFunction.ValidAdminIP(context.Request.UserHostAddress);
			string result;
			if (flag)
			{
				result = "CelebByDayOfferList Fail!";
			}
			else
			{
				result = CelebByDayOfferList.Build();
			}
			return result;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00012954 File Offset: 0x00010B54
		public static string Build()
		{
			return csFunction.BuildCelebUsers("CelebByDayOfferList", 4, "CelebByDayOfferList_Out");
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x00012978 File Offset: 0x00010B78
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x040000A1 RID: 161
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
