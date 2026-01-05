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
	// Token: 0x02000064 RID: 100
	public class petskilltemplateinfo : IHttpHandler
	{
		// Token: 0x060001C6 RID: 454 RVA: 0x000029E8 File Offset: 0x00000BE8
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(petskilltemplateinfo.Build(context));
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000DC78 File Offset: 0x0000BE78
		public static string Build(HttpContext context)
		{
			bool value = false;
			string message = "Hata!";
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
				petskilltemplateinfo.log.Error("Load petskilltemplateinfo is fail!", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			return csFunction.CreateCompressXml(context, result, "petskilltemplateinfo", false);
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x00003828 File Offset: 0x00001A28
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
