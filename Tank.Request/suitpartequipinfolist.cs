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
	// Token: 0x02000075 RID: 117
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class suitpartequipinfolist : IHttpHandler
	{
		// Token: 0x0600021E RID: 542 RVA: 0x0001063C File Offset: 0x0000E83C
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(suitpartequipinfolist.Build(context));
			}
			else
			{
				context.Response.Write("IP is not valid!");
			}
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00010688 File Offset: 0x0000E888
		public static string Build(HttpContext context)
		{
			bool value = false;
			string message = "Fail!";
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
				message = "Success!";
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
		// (get) Token: 0x06000220 RID: 544 RVA: 0x00010780 File Offset: 0x0000E980
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000083 RID: 131
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
