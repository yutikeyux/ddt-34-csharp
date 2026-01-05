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
	// Token: 0x02000079 RID: 121
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class suitpartequipinfolist : IHttpHandler
	{
		// Token: 0x06000224 RID: 548 RVA: 0x0000FFD8 File Offset: 0x0000E1D8
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(suitpartequipinfolist.Build(context));
			}
			else
			{
				context.Response.Write("Tabi Efendim!");
			}
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00010024 File Offset: 0x0000E224
		public static string Build(HttpContext context)
		{
			bool value = false;
			string message = "Hata!";
			XElement result = new XElement("Result");
			try
			{
				using (ProduceBussiness db = new ProduceBussiness())
				{
					Suit_TemplateID[] infos = db.Load_Suit_TemplateID();
					foreach (Suit_TemplateID info in infos)
					{
						result.Add(FlashUtils.CreateSuit_TemplateID(info));
					}
				}
				value = true;
				message = "Başarılı!";
			}
			catch (Exception ex)
			{
				suitpartequipinfolist.log.Error("BallList", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			return csFunction.CreateCompressXml(context, result, "suitpartequipinfolist", true);
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000226 RID: 550 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000089 RID: 137
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
