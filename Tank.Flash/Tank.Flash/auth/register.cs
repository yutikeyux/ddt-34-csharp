using System;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Bussiness;
using log4net;

namespace Tank.Flash.auth
{
	// Token: 0x02000004 RID: 4
	public class register : IHttpHandler, IRequiresSessionState
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000006 RID: 6 RVA: 0x00002084 File Offset: 0x00000284
		protected string SiteTitle
		{
			get
			{
				if (ConfigurationManager.AppSettings["SiteTitle"] != null)
				{
					return ConfigurationManager.AppSettings["SiteTitle"];
				}
				return "DanDanTang";
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000020AC File Offset: 0x000002AC
		protected bool CheckPara(HttpContext context, ref string message)
		{
			if (context.Session["CheckCode"] == null || this.code.ToLower() != context.Session["CheckCode"].ToString().ToLower())
			{
				message = "Sai mã bảo mật!";
				return false;
			}
			using (MemberShipBussiness memberShipBussiness = new MemberShipBussiness())
			{
				if (memberShipBussiness.ExistsUsername(this.username))
				{
					message = "exit";
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002140 File Offset: 0x00000340
		protected bool CreateUsername(HttpContext context, ref string message)
		{
			this.password = FormsAuthentication.HashPasswordForStoringInConfigFile(this.password, "md5");
			bool result;
			using (MemberShipBussiness memberShipBussiness = new MemberShipBussiness())
			{
				result = memberShipBussiness.CreateUsername(this.SiteTitle, this.username, this.password, this.email, "1", "MD5", this.sex);
			}
			return result;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000021B8 File Offset: 0x000003B8
		public void ProcessRequest(HttpContext context)
		{
			this.username = context.Request["username"];
			this.password = context.Request["password"];
			this.repassword = context.Request["repassword"];
			this.email = context.Request["email"];
			this.code = context.Request["code"];
			this.message = "";
			if (this.CheckPara(context, ref this.message) && this.CreateUsername(context, ref this.message))
			{
				this.message = "ok";
			}
			context.Response.Write(this.message);
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000A RID: 10 RVA: 0x00002278 File Offset: 0x00000478
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000002 RID: 2
		protected static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Token: 0x04000003 RID: 3
		private string code;

		// Token: 0x04000004 RID: 4
		private string email;

		// Token: 0x04000005 RID: 5
		private string message;

		// Token: 0x04000006 RID: 6
		private string password;

		// Token: 0x04000007 RID: 7
		private string repassword;

		// Token: 0x04000008 RID: 8
		private bool sex;

		// Token: 0x04000009 RID: 9
		private string username;
	}
}
