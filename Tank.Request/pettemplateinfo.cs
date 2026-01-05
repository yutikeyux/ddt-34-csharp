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
	// Token: 0x02000065 RID: 101
	public class pettemplateinfo : IHttpHandler
	{
		// Token: 0x060001CB RID: 459 RVA: 0x00002A13 File Offset: 0x00000C13
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(pettemplateinfo.Build(context));
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000DD70 File Offset: 0x0000BF70
		public static string Build(HttpContext context)
		{
			bool value = false;
			string message = "Hata!";
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
					message = "Başarılı!";
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
		// (get) Token: 0x060001CD RID: 461 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000067 RID: 103
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
