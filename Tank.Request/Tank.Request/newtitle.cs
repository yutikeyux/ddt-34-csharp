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
	// Token: 0x0200005A RID: 90
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class newtitle : IHttpHandler
	{
		// Token: 0x0600019C RID: 412 RVA: 0x0000D3D8 File Offset: 0x0000B5D8
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(newtitle.build(context));
			}
			else
			{
				context.Response.Write("Erişim yetkisi reddedildi!");
			}
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000D424 File Offset: 0x0000B624
		public static string build(HttpContext context)
		{
			bool value = false;
			string message = "Başarısız!";
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
					message = "Success!";
				}
			}
			catch (Exception ex)
			{
				newtitle.log.Error("Load Active is Başarısız!", ex);
			}
			result.Add(new XAttribute("total", t));
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			return csFunction.CreateCompressXml(context, result, "newtitleinfo", false);
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600019E RID: 414 RVA: 0x0000D544 File Offset: 0x0000B744
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400005C RID: 92
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
