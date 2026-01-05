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
	// Token: 0x0200007D RID: 125
	public class totemhonortemplate : IHttpHandler
	{
		// Token: 0x06000237 RID: 567 RVA: 0x000104BC File Offset: 0x0000E6BC
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(totemhonortemplate.Build(context));
			}
			else
			{
				context.Response.Write("Tabi Efendim!");
			}
		}

		// Token: 0x06000238 RID: 568 RVA: 0x00010508 File Offset: 0x0000E708
		public static string Build(HttpContext context)
		{
			bool value = false;
			string message = "Hata!";
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
					message = "Başarılı!";
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
		// (get) Token: 0x06000239 RID: 569 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400008C RID: 140
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
