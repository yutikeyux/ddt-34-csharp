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
	// Token: 0x02000079 RID: 121
	public class totemhonortemplate : IHttpHandler
	{
		// Token: 0x06000231 RID: 561 RVA: 0x00010BC8 File Offset: 0x0000EDC8
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(totemhonortemplate.Bulid(context));
			}
			else
			{
				context.Response.Write("IP is not valid!");
			}
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00010C14 File Offset: 0x0000EE14
		public static string Bulid(HttpContext context)
		{
			bool value = false;
			string message = "Fail!";
			XElement result = new XElement("Result");
			try
			{
				using (ProduceBussiness db = new ProduceBussiness())
				{
					TotemHonorTemplateInfo[] infos = db.GetAllTotemHonorTemplate();
					foreach (TotemHonorTemplateInfo info in infos)
					{
						result.Add(FlashUtils.CreateTotemHonorTemplate(info));
					}
					value = true;
					message = "Success!";
				}
			}
			catch (Exception ex)
			{
				totemhonortemplate.log.Error("Load totemhonortemplate is fail!", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			return csFunction.CreateCompressXml(context, result, "totemhonortemplate", false);
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000233 RID: 563 RVA: 0x00010D0C File Offset: 0x0000EF0C
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000086 RID: 134
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
