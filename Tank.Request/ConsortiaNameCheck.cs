using System;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml.Linq;
using Bussiness;
using log4net;

namespace Tank.Request
{
	// Token: 0x02000023 RID: 35
	public class ConsortiaNameCheck : IHttpHandler
	{
		// Token: 0x0600008F RID: 143 RVA: 0x00005D14 File Offset: 0x00003F14
		public void ProcessRequest(HttpContext context)
		{
			string path = HttpContext.Current.Server.MapPath(".");
			path += "\\";
			LanguageMgr.Setup(path);
			bool value = false;
			string message = LanguageMgr.GetTranslation("Tank.Request.ConsortiaCheck.Exist", Array.Empty<object>());
			XElement result = new XElement("Result");
			try
			{
				string ConsortiaName = csFunction.ConvertSql(HttpUtility.UrlDecode(context.Request["NickName"]));
				bool flag = Encoding.Default.GetByteCount(ConsortiaName) <= 14;
				if (flag)
				{
					bool flag2 = !string.IsNullOrEmpty(ConsortiaName);
					if (flag2)
					{
						using (ConsortiaBussiness db = new ConsortiaBussiness())
						{
							bool flag3 = db.GetConsortiaSingleByName(ConsortiaName) == null;
							if (flag3)
							{
								value = true;
								message = LanguageMgr.GetTranslation("Tank.Request.ConsortiaCheck.Right", Array.Empty<object>());
							}
						}
					}
				}
				else
				{
					message = LanguageMgr.GetTranslation("Tank.Request.ConsortiaCheck.Long", Array.Empty<object>());
				}
			}
			catch (Exception ex)
			{
				ConsortiaNameCheck.log.Error("ConsortiaCheck", ex);
				value = false;
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			context.Response.ContentType = "text/plain";
			context.Response.Write(result.ToString(false));
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00005E98 File Offset: 0x00004098
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000020 RID: 32
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
