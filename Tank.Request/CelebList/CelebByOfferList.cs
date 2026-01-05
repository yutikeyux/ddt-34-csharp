using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using log4net;

namespace Tank.Request.CelebList
{
	// Token: 0x02000094 RID: 148
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class CelebByOfferList : IHttpHandler
	{
		// Token: 0x060002B4 RID: 692 RVA: 0x00002F31 File Offset: 0x00001131
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(CelebByOfferList.Build(context));
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x00011E94 File Offset: 0x00010094
		public static string Build(HttpContext context)
		{
			bool flag = !csFunction.ValidAdminIP(context.Request.UserHostAddress);
			string result;
			if (flag)
			{
				result = "CelebByOfferList Fail!";
			}
			else
			{
				result = CelebByOfferList.Build();
			}
			return result;
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x00011ECC File Offset: 0x000100CC
		public static string Build()
		{
			return csFunction.BuildCelebUsers("CelebByOfferList", 1, "CelebByOfferList_Out");
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x040000A9 RID: 169
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
