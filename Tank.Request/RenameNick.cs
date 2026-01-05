using System;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness;
using Bussiness.Interface;
using log4net;
using Road.Flash;

namespace Tank.Request
{
	// Token: 0x0200006B RID: 107
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class RenameNick : IHttpHandler
	{
		// Token: 0x060001E8 RID: 488 RVA: 0x0000E6B4 File Offset: 0x0000C8B4
		public void ProcessRequest(HttpContext context)
		{
			string path = HttpContext.Current.Server.MapPath(".");
			path += "\\";
			LanguageMgr.Setup(path);
			bool value = false;
			string message = LanguageMgr.GetTranslation(" thay đổi tên nhân vật thất bại.", Array.Empty<object>());
			XElement result = new XElement("Result");
			try
			{
				BaseInterface inter = BaseInterface.CreateInterface();
				string p = context.Request["p"];
				string text = (context.Request["site"] == null) ? "" : HttpUtility.UrlDecode(context.Request["site"]);
				string IP = context.Request.UserHostAddress;
				bool flag = !string.IsNullOrEmpty(p);
				if (flag)
				{
					byte[] src = CryptoHelper.RsaDecryt2(StaticFunction.RsaCryptor, p);
					string[] strList = Encoding.UTF8.GetString(src, 7, src.Length - 7).Split(new char[]
					{
						','
					});
					bool flag2 = strList.Length == 5;
					if (flag2)
					{
						string name = strList[0];
						string pwd = strList[1];
						string newPwd = strList[2];
						string nickname = strList[3];
						string newNickname = strList[4];
						using (PlayerBussiness db = new PlayerBussiness())
						{
							bool flag3 = db.RenameNick(name, nickname, newNickname, ref message);
							if (flag3)
							{
								PlayerManager.Update(name, newPwd);
								value = true;
								message = LanguageMgr.GetTranslation(" thay đổi tên nhân vật thành công.", Array.Empty<object>());
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				RenameNick.log.Error("RenameNick", ex);
				value = false;
				message = LanguageMgr.GetTranslation(" thay đổi tên nhân vật thất bại ..", Array.Empty<object>());
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			context.Response.ContentType = "text/plain";
			context.Response.Write(result.ToString(false));
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000074 RID: 116
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
