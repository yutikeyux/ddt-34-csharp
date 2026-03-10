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
	// Token: 0x02000076 RID: 118
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class suittemplateinfolist : IHttpHandler
	{
		// Token: 0x06000223 RID: 547 RVA: 0x000107AC File Offset: 0x0000E9AC
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(suittemplateinfolist.Build(context));
			}
			else
			{
				context.Response.Write("IP is not valid!");
			}
		}

		// Token: 0x06000224 RID: 548 RVA: 0x000107F8 File Offset: 0x0000E9F8
		public static string Build(HttpContext context)
		{
			bool value = false;
			string message = "Başarısız!";
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
		// (get) Token: 0x06000225 RID: 549 RVA: 0x000108F0 File Offset: 0x0000EAF0
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000084 RID: 132
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
