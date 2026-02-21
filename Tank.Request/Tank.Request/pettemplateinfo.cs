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
	// Token: 0x02000063 RID: 99
	public class pettemplateinfo : IHttpHandler
	{
		// Token: 0x060001C9 RID: 457 RVA: 0x0000E15D File Offset: 0x0000C35D
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(pettemplateinfo.build(context));
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000E174 File Offset: 0x0000C374
		public static string build(HttpContext context)
		{
			bool value = false;
			string message = "Fail!";
			XElement result = new XElement("Result");
			try
			{
				using (ProduceBussiness db = new ProduceBussiness())
				{
					PetTemplateInfo[] infos = db.GetAllPetTemplateInfo();
					foreach (PetTemplateInfo info in infos)
					{
						result.Add(FlashUtils.CreatePetTemplate(info));
					}
					value = true;
					message = "Success!";
				}
			}
			catch (Exception ex)
			{
				pettemplateinfo.log.Error("Load pettemplateinfo is fail!", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			return csFunction.CreateCompressXml(context, result, "pettemplateinfo", false);
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060001CB RID: 459 RVA: 0x0000E26C File Offset: 0x0000C46C
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000066 RID: 102
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
