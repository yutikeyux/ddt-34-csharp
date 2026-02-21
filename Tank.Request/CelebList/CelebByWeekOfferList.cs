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
	public class CelebByWeekOfferList : IHttpHandler
	{
		// Token: 0x060002BA RID: 698 RVA: 0x00012B75 File Offset: 0x00010D75
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(CelebByWeekOfferList.Build(context));
		}

		// Token: 0x060002BB RID: 699 RVA: 0x00012B8C File Offset: 0x00010D8C
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

		// Token: 0x060002BC RID: 700 RVA: 0x00012BC4 File Offset: 0x00010DC4
		public static string Build()
		{
			return csFunction.BuildCelebUsers("CelebByWeekOfferList", 5, "CelebByWeekOfferList_Out");
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060002BD RID: 701 RVA: 0x00012BE8 File Offset: 0x00010DE8
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
