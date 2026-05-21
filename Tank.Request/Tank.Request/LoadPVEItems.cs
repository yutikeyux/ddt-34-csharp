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
	// Token: 0x0200004A RID: 74
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class LoadPVEItems : IHttpHandler
	{
		// Token: 0x0600015A RID: 346 RVA: 0x0000AD14 File Offset: 0x00008F14
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(LoadPVEItems.Build(context));
			}
			else
			{
				context.Response.Write("Erişim yetkisi reddedildi!");
			}
		}

		// Token: 0x0600015B RID: 347 RVA: 0x0000AD60 File Offset: 0x00008F60
		public static string Build(HttpContext context)
		{
			bool value = false;
			string message = "Fail";
			XElement result = new XElement("Result");
			try
			{
				using (PveBussiness db = new PveBussiness())
				{
					PveInfo[] infos = db.GetAllPveInfos();
					foreach (PveInfo info in infos)
					{
						result.Add(FlashUtils.CreatePveInfo(info));
					}
				}
				value = true;
				message = "Success!";
			}
			catch (Exception ex)
			{
				LoadPVEItems.log.Error("LoadPVEItems", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			return csFunction.CreateCompressXml(context, result, "LoadPVEItems", true);
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600015C RID: 348 RVA: 0x0000AE58 File Offset: 0x00009058
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400004E RID: 78
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
