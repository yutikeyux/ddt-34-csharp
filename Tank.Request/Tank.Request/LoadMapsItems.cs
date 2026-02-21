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
	// Token: 0x02000047 RID: 71
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class LoadMapsItems : IHttpHandler
	{
		// Token: 0x0600014B RID: 331 RVA: 0x0000A884 File Offset: 0x00008A84
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(LoadMapsItems.build(context));
			}
			else
			{
				context.Response.Write("IP is not valid!");
			}
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0000A8D0 File Offset: 0x00008AD0
		public static string build(HttpContext context)
		{
			bool value = false;
			string message = "Fail";
			XElement result = new XElement("Result");
			try
			{
				using (MapBussiness db = new MapBussiness())
				{
					MapInfo[] infos = db.GetAllMap();
					foreach (MapInfo info in infos)
					{
						result.Add(FlashUtils.CreateMapInfo(info));
					}
				}
				value = true;
				message = "Success!";
			}
			catch (Exception ex)
			{
				LoadMapsItems.log.Error("LoadMapsItems", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			return csFunction.CreateCompressXml(context, result, "LoadMapsItems", true);
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600014D RID: 333 RVA: 0x0000A9C8 File Offset: 0x00008BC8
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400004B RID: 75
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
