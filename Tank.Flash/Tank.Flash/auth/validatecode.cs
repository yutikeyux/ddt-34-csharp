using System;
using System.Web;
using System.Web.SessionState;
using Bussiness;

namespace Tank.Flash.auth
{
	// Token: 0x02000006 RID: 6
	public class validatecode : IHttpHandler, IRequiresSessionState
	{
		// Token: 0x0600000F RID: 15 RVA: 0x000022A4 File Offset: 0x000004A4
		public void ProcessRequest(HttpContext context)
		{
			string text = CheckCode.GenerateCheckCode();
			byte[] buffer = CheckCode.CreateImage(text);
			context.Session["CheckCode"] = text;
			context.Response.ClearContent();
			context.Response.ContentType = "image/Gif";
			context.Response.BinaryWrite(buffer);
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000022F6 File Offset: 0x000004F6
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}
