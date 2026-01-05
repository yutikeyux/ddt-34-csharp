using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness;
using log4net;
using Road.Flash;
using SqlDataProvider.Data;

namespace Tank.Request
{
	// Token: 0x0200005C RID: 92
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class newtitle : IHttpHandler
	{
		// Token: 0x0600019E RID: 414 RVA: 0x0000D224 File Offset: 0x0000B424
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(newtitle.Build(context));
			}
			else
			{
				context.Response.Write("Tabi Efendim!");
			}
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000D270 File Offset: 0x0000B470
		public static string Build(HttpContext context)
		{
			bool value = false;
			string message = "Hata!";
			XElement result = new XElement("Result");
			int t = 0;
			try
			{
				using (ProduceBussiness db = new ProduceBussiness())
				{
					NewTitleInfo[] infos = db.GetAllNewTitle();
					t = infos.Length;
					foreach (NewTitleInfo info in infos)
					{
						result.Add(FlashUtils.CreateNewTitleInfo(info));
					}
					value = true;
					message = "Başarılı!";
				}
			}
			catch (Exception ex)
			{
				newtitle.log.Error("Load Active is fail!", ex);
			}
			result.Add(new XAttribute("total", t));
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			return csFunction.CreateCompressXml(context, result, "newtitleinfo", false);
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400005D RID: 93
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
