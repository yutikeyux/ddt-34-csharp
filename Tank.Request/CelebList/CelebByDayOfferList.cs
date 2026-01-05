using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using log4net;

namespace Tank.Request.CelebList
{
	// Token: 0x02000092 RID: 146
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class CelebByDayOfferList : IHttpHandler
	{
		// Token: 0x060002A8 RID: 680 RVA: 0x00002EDB File Offset: 0x000010DB
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(CelebByDayOfferList.Build(context));
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x00011DDC File Offset: 0x0000FFDC
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

		// Token: 0x060002AA RID: 682 RVA: 0x00011E14 File Offset: 0x00010014
		public static string Build()
		{
			return csFunction.BuildCelebUsers("CelebByDayOfferList", 4, "CelebByDayOfferList_Out");
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060002AB RID: 683 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x040000A7 RID: 167
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
