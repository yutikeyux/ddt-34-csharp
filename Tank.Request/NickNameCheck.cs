using System;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness;
using log4net;

namespace Tank.Request
{
	// Token: 0x0200005D RID: 93
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class NickNameCheck : IHttpHandler
	{
		// Token: 0x060001A3 RID: 419 RVA: 0x0000D390 File Offset: 0x0000B590
		public void ProcessRequest(HttpContext context)
		{
			LanguageMgr.Setup(HttpContext.Current.Server.MapPath(".") + "\\");
			bool flag = false;
			string translation = LanguageMgr.GetTranslation("Oyuncunun adı zaten mevcut.", Array.Empty<object>());
			XElement xelement = new XElement("Result");
			try
			{
				string text = csFunction.ConvertSql(HttpUtility.UrlDecode(context.Request["NickName"]));
				if (Encoding.Default.GetByteCount(text) <= 14)
				{
					if (string.IsNullOrEmpty(text))
					{
						goto IL_EB;
					}
					if (!new Regex("^[a-za-z0-9àáâãèéêìíòóôõùúăđĩũơàáâãèéêìíòóôõùúăđĩũơưăạảấầẩẫậắằẳẵặẹẻẽềềểếưăạảấầẩẫậắằẳẵặẹẻẽềềểếễệỉịọỏốồổỗộớờởỡợụủứừễệỉịọỏốồổỗộớờởỡợụủứừửữựỳỵýỷỹửữựỳỵỷỹ\\s|_.]+$").IsMatch(text))
					{
						translation = LanguageMgr.GetTranslation("UseReworkNameHandler.HasSpecialCharacters", Array.Empty<object>());
						goto IL_EB;
					}
					using (PlayerBussiness playerBussiness = new PlayerBussiness())
					{
						if (playerBussiness.GetUserSingleByNickName(text) == null)
						{
							flag = true;
							translation = LanguageMgr.GetTranslation("Tank.Request.NickNameCheck.Right", Array.Empty<object>());
						}
						goto IL_EB;
					}
				}
				translation = LanguageMgr.GetTranslation("Tank.Request.NickNameCheck.Long", Array.Empty<object>());
				IL_EB:;
			}
			catch (Exception exception)
			{
				NickNameCheck.log.Error("NickNameCheck", exception);
				flag = false;
			}
			xelement.Add(new XAttribute("value", flag));
			xelement.Add(new XAttribute("message", translation));
			context.Response.ContentType = "text/plain";
			context.Response.Write(xelement.ToString(false));
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400005E RID: 94
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
