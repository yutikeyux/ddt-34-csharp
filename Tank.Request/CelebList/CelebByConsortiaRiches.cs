using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using log4net;

namespace Tank.Request.CelebList
{
	// Token: 0x0200008C RID: 140
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class CelebByConsortiaRiches : IHttpHandler
	{
		// Token: 0x06000284 RID: 644 RVA: 0x00002DD9 File Offset: 0x00000FD9
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(CelebByConsortiaRiches.Build(context));
		}

		// Token: 0x06000285 RID: 645 RVA: 0x00011AE0 File Offset: 0x0000FCE0
		public static string Build(HttpContext context)
		{
			bool flag = !csFunction.ValidAdminIP(context.Request.UserHostAddress);
			string result;
			if (flag)
			{
				result = "CelebByConsortiaRiches Fail!";
			}
			else
			{
				result = CelebByConsortiaRiches.Build();
			}
			return result;
		}

		// Token: 0x06000286 RID: 646 RVA: 0x00011B18 File Offset: 0x0000FD18
		public static string Build()
		{
			return csFunction.BuildCelebConsortia("CelebByConsortiaRiches", 10, "CelebByConsortiaRiches_Out");
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000287 RID: 647 RVA: 0x00003828 File Offset: 0x00001A28
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
