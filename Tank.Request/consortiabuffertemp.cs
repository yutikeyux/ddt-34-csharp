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
	// Token: 0x0200001A RID: 26
	public class consortiabuffertemp : IHttpHandler
	{
		// Token: 0x06000069 RID: 105 RVA: 0x00004CF8 File Offset: 0x00002EF8
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(consortiabuffertemp.Build(context));
			}
			else
			{
				context.Response.Write("IP is not valid!");
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00004D44 File Offset: 0x00002F44
		public static string Build(HttpContext context)
		{
			bool value = false;
			string message = "Fail!";
			XElement result = new XElement("Result");
			try
			{
				using (ProduceBussiness db = new ProduceBussiness())
				{
					ConsortiaBuffTempInfo[] infos = db.GetAllConsortiaBuffTemp();
					foreach (ConsortiaBuffTempInfo info in infos)
					{
						result.Add(FlashUtils.CreateConsortiaBuffer(info));
					}
				}
				value = true;
				message = "Success!";
			}
			catch (Exception ex)
			{
				consortiabuffertemp.log.Error("consortiabuffertemp", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			csFunction.CreateCompressXml(context, result, "consortiabuffertemp_out", false);
			return csFunction.CreateCompressXml(context, result, "consortiabuffertemp", true);
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00004E4C File Offset: 0x0000304C
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000017 RID: 23
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
