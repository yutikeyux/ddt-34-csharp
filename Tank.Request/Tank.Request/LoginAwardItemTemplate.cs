using System;
using System.Reflection;
using System.Web;
using System.Xml.Linq;
using Bussiness;
using log4net;
using Road.Flash;
using SqlDataProvider.Data;

namespace Tank.Request
{
	// Token: 0x02000051 RID: 81
	public class LoginAwardItemTemplate : IHttpHandler
	{
		// Token: 0x06000179 RID: 377 RVA: 0x0000C6E4 File Offset: 0x0000A8E4
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(LoginAwardItemTemplate.build(context));
			}
			else
			{
				context.Response.Write("Erişim yetkisi reddedildi!");
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600017A RID: 378 RVA: 0x0000C730 File Offset: 0x0000A930
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000C744 File Offset: 0x0000A944
		public static string build(HttpContext context)
		{
			bool value = false;
			string message = "Başarısız!";
			XElement result = new XElement("Result");
			try
			{
				using (ProduceBussiness db = new ProduceBussiness())
				{
					AccumulAtiveLoginAwardInfo[] infos = db.GetAccumulAtiveLoginAwardInfos();
					foreach (AccumulAtiveLoginAwardInfo info in infos)
					{
						result.Add(FlashUtils.CreateAccumulAtiveLoginAwards(info));
					}
					value = true;
					message = "Success!";
				}
			}
			catch (Exception ex)
			{
				LoginAwardItemTemplate.log.Error("Load loginawarditemtemplate is Başarısız!", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			return csFunction.CreateCompressXml(context, result, "loginawarditemtemplate", true);
		}

		// Token: 0x04000054 RID: 84
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
