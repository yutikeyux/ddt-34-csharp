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
	public class CelebByOfferList : IHttpHandler
	{
		// Token: 0x060002AE RID: 686 RVA: 0x00012A3D File Offset: 0x00010C3D
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(CelebByOfferList.Build(context));
		}

		// Token: 0x060002AF RID: 687 RVA: 0x00012A54 File Offset: 0x00010C54
		public static string Build(HttpContext context)
		{
			bool flag = !csFunction.ValidAdminIP(context.Request.UserHostAddress);
			string result;
			if (flag)
			{
				result = "CelebByOfferList Başarısız!";
			}
			else
			{
				result = CelebByOfferList.Build();
			}
			return result;
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x00012A8C File Offset: 0x00010C8C
		public static string Build()
		{
			return csFunction.BuildCelebUsers("CelebByOfferList", 1, "CelebByOfferList_Out");
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x00012AB0 File Offset: 0x00010CB0
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x040000A3 RID: 163
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
