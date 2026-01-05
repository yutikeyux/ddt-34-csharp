using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using log4net;

namespace Tank.Request.CelebList
{
	// Token: 0x02000089 RID: 137
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class celebbyconsortiafightpower : IHttpHandler
	{
		// Token: 0x06000272 RID: 626 RVA: 0x00002D58 File Offset: 0x00000F58
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(celebbyconsortiafightpower.Build(context));
		}

		// Token: 0x06000273 RID: 627 RVA: 0x000119CC File Offset: 0x0000FBCC
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

		// Token: 0x06000274 RID: 628 RVA: 0x00011A04 File Offset: 0x0000FC04
		public static string Build()
		{
			return csFunction.BuildCelebConsortiaFightPower("celebbyconsortiafightpower", "celebbyconsortiafightpower_Out");
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000275 RID: 629 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400009E RID: 158
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
