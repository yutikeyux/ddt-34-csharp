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
	// Token: 0x02000062 RID: 98
	public class petskilltemplateinfo : IHttpHandler
	{
		// Token: 0x060001C4 RID: 452 RVA: 0x0000E025 File Offset: 0x0000C225
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(petskilltemplateinfo.Build(context));
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000E03C File Offset: 0x0000C23C
		public static string Build(HttpContext context)
		{
			bool value = false;
			string message = "Başarısız!";
			XElement result = new XElement("Result");
			try
			{
				using (ProduceBussiness db = new ProduceBussiness())
				{
					PetSkillTemplateInfo[] infos = db.GetAllPetSkillTemplateInfo();
					foreach (PetSkillTemplateInfo info in infos)
					{
						result.Add(FlashUtils.CreatePetSkillTemplate(info));
					}
					value = true;
					message = "Başarılı!";
				}
			}
			catch (Exception ex)
			{
				petskilltemplateinfo.log.Error("Load petskilltemplateinfo is Başarısız!", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			return csFunction.CreateCompressXml(context, result, "petskilltemplateinfo", false);
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x0000E134 File Offset: 0x0000C334
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000065 RID: 101
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
