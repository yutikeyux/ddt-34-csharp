using System;
using System.Web;

namespace Tank.Flash.auth
{
	// Token: 0x02000002 RID: 2
	public class forgotpass1 : IHttpHandler
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public void ProcessRequest(HttpContext context)
		{
			string s = "Request False!";
			context.Response.Write(s);
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000002 RID: 2 RVA: 0x0000206F File Offset: 0x0000026F
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}
