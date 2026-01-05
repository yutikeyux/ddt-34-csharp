using System;
using System.Reflection;
using System.Web;
using System.Web.UI;
using Bussiness;
using Bussiness.Interface;
using log4net;
using SqlDataProvider.Data;

namespace Tank.Request
{
	// Token: 0x02000080 RID: 128
	public class UserNameCheck : Page
	{
		// Token: 0x06000244 RID: 580 RVA: 0x00010934 File Offset: 0x0000EB34
		protected void Page_Load(object sender, EventArgs e)
		{
			int result = 1;
			try
			{
				string username = HttpUtility.UrlDecode(base.Request["username"]);
				string site = (base.Request["site"] == null) ? "" : HttpUtility.UrlDecode(base.Request["site"]);
				bool flag = !string.IsNullOrEmpty(username);
				if (flag)
				{
					username = BaseInterface.GetNameBySite(username, site);
					using (PlayerBussiness db = new PlayerBussiness())
					{
						PlayerInfo info = db.GetUserSingleByUserName(username);
						bool flag2 = info != null;
						if (flag2)
						{
							result = 0;
						}
						else
						{
							result = 2;
						}
					}
				}
			}
			catch (Exception ex)
			{
				UserNameCheck.log.Error("UserNameCheck:", ex);
			}
			base.Response.Write(result);
		}

		// Token: 0x0400008F RID: 143
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
