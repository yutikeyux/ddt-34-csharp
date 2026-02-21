using System;
using System.Reflection;
using System.Web;
using log4net;

namespace Tank.Request
{
	// Token: 0x02000027 RID: 39
	public class CreatShortCut : IHttpHandler
	{
		// Token: 0x0600009F RID: 159 RVA: 0x00006460 File Offset: 0x00004660
		public void ProcessRequest(HttpContext context)
		{
			string gameurl = context.Request["gameurl"];
			context.Response.Write("Not support right now");
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00006490 File Offset: 0x00004690
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000023 RID: 35
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
