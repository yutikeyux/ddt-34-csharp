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
	// Token: 0x02000070 RID: 112
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class serverconfig : IHttpHandler
	{
		// Token: 0x060001FC RID: 508 RVA: 0x0000F064 File Offset: 0x0000D264
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

		// Token: 0x060001FD RID: 509 RVA: 0x0000F15C File Offset: 0x0000D35C
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(serverconfig.Build(context));
			}
			else
			{
				context.Response.Write("Tabi Efendim!");
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060001FE RID: 510 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000077 RID: 119
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
