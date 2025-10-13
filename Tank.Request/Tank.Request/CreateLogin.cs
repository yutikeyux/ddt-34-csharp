using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using Bussiness.Interface;
using log4net;

namespace Tank.Request
{
	// Token: 0x02000026 RID: 38
	public class CreateLogin : Page
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600009A RID: 154 RVA: 0x000062C4 File Offset: 0x000044C4
		public static string GetLoginIP
		{
			get
			{
				return ConfigurationSettings.AppSettings["LoginIP"];
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000062E8 File Offset: 0x000044E8
		public static bool ValidLoginIP(string ip)
		{
			string ips = CreateLogin.GetLoginIP;
			return string.IsNullOrEmpty(ips) || ips.Split(new char[]
			{
				'|'
			}).Contains(ip);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x0000632C File Offset: 0x0000452C
		protected void Page_Load(object sender, EventArgs e)
		{
			int result = 1;
			try
			{
				string content = HttpUtility.UrlDecode(base.Request["content"]);
				string site = (base.Request["site"] == null) ? "" : HttpUtility.UrlDecode(base.Request["site"]).ToLower();
				BaseInterface inter = BaseInterface.CreateInterface();
				string[] str = inter.UnEncryptLogin(content, ref result, site);
				bool flag = str.Length > 3;
				if (flag)
				{
					string name = str[0].Trim().ToLower();
					string password = str[1].Trim().ToLower();
					bool flag2 = !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(password);
					if (flag2)
					{
						name = BaseInterface.GetNameBySite(name, site);
						PlayerManager.Add(name, password);
						result = 0;
					}
					else
					{
						result = -91010;
					}
				}
			}
			catch (Exception ex)
			{
				CreateLogin.log.Error("CreateLogin:", ex);
			}
			base.Response.Write(result);
		}

		// Token: 0x04000022 RID: 34
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
