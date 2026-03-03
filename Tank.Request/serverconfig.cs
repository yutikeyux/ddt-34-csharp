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
	// Token: 0x0200006C RID: 108
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class serverconfig : IHttpHandler
	{
		// Token: 0x060001F6 RID: 502 RVA: 0x0000F59C File Offset: 0x0000D79C
		public static string Build(HttpContext context)
		{
			bool flag = false;
			string str = "Fail!";
			XElement result = new XElement("Result");
			try
			{
				using (ServiceBussiness bussiness = new ServiceBussiness())
				{
					ServerProperty[] allproperties = bussiness.GetAllServerProperty();
					foreach (ServerProperty info in allproperties)
					{
						result.Add(FlashUtils.CreateServerConfig(info));
					}
				}
				flag = true;
				str = "Success!";
			}
			catch (Exception exception)
			{
				serverconfig.log.Error("ServerConfig", exception);
			}
			result.Add(new XAttribute("value", flag));
			result.Add(new XAttribute("message", str));
			return csFunction.CreateCompressXml(context, result, "ServerConfig", false);
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000F694 File Offset: 0x0000D894
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(serverconfig.Build(context));
			}
			else
			{
				context.Response.Write("IP is not valid!");
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x0000F6E0 File Offset: 0x0000D8E0
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000071 RID: 113
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
