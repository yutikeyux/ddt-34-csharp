using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using log4net;

namespace Tank.Request
{
	// Token: 0x0200002F RID: 47
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class elitematchplayerlist : IHttpHandler
	{
		// Token: 0x060000E1 RID: 225 RVA: 0x00007B11 File Offset: 0x00005D11
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(elitematchplayerlist.Build(context));
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00007B28 File Offset: 0x00005D28
		public static string Build(HttpContext context)
		{
			bool flag = !csFunction.ValidAdminIP(context.Request.UserHostAddress);
			string result;
			if (flag)
			{
				result = "elitematchplayerlist Başarısız!";
			}
			else
			{
				result = elitematchplayerlist.Build();
			}
			return result;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00007B60 File Offset: 0x00005D60
		public static string Build()
		{
			return csFunction.BuildEliteMatchPlayerList("elitematchplayerlist");
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00007B7C File Offset: 0x00005D7C
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000035 RID: 53
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
