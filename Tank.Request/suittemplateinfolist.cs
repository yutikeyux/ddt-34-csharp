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
	// Token: 0x0200007A RID: 122
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class suittemplateinfolist : IHttpHandler
	{
		// Token: 0x06000229 RID: 553 RVA: 0x0001011C File Offset: 0x0000E31C
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(suittemplateinfolist.Build(context));
			}
			else
			{
				context.Response.Write("Tabi Efendim!");
			}
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00010168 File Offset: 0x0000E368
		public static string Build(HttpContext context)
		{
			bool value = false;
			string message = "Hata!";
			XElement result = new XElement("Result");
			try
			{
				using (ProduceBussiness db = new ProduceBussiness())
				{
					Suit_TemplateInfo[] infos = db.Load_Suit_TemplateInfo();
					foreach (Suit_TemplateInfo info in infos)
					{
						result.Add(FlashUtils.CreateSuit_TemplateInfo(info));
					}
				}
				value = true;
				message = "Başarılı!";
			}
			catch (Exception ex)
			{
				suittemplateinfolist.log.Error("BallList", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			return csFunction.CreateCompressXml(context, result, "suittemplateinfolist", true);
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600022B RID: 555 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400008A RID: 138
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
