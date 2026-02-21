using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using log4net;

namespace Tank.Request.CelebList
{
	// Token: 0x02000085 RID: 133
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class celebbyconsortiafightpower : IHttpHandler
	{
		// Token: 0x0600026C RID: 620 RVA: 0x000122B5 File Offset: 0x000104B5
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(celebbyconsortiafightpower.Build(context));
		}

		// Token: 0x0600026D RID: 621 RVA: 0x000122CC File Offset: 0x000104CC
		public static string Build(HttpContext context)
		{
			bool flag = !csFunction.ValidAdminIP(context.Request.UserHostAddress);
			string result;
			if (flag)
			{
				result = "celebbyconsortiafightpower Fail!";
			}
			else
			{
				result = celebbyconsortiafightpower.Build();
			}
			return result;
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00012304 File Offset: 0x00010504
		public static string Build()
		{
			return csFunction.BuildCelebConsortiaFightPower("celebbyconsortiafightpower", "celebbyconsortiafightpower_Out");
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600026F RID: 623 RVA: 0x00012328 File Offset: 0x00010528
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000098 RID: 152
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
