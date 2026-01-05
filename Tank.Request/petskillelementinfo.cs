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
	public class petskillelementinfo : IHttpHandler
	{
		// Token: 0x060001BC RID: 444 RVA: 0x00002992 File Offset: 0x00000B92
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(petskillelementinfo.Build(context));
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0000DA88 File Offset: 0x0000BC88
		public static string Build(HttpContext context)
		{
			bool value = false;
			string message = "Hata!";
			XElement result = new XElement("Result");
			try
			{
				using (ProduceBussiness db = new ProduceBussiness())
				{
					PetSkillElementInfo[] infos = db.GetAllPetSkillElementInfo();
					foreach (PetSkillElementInfo info in infos)
					{
						result.Add(FlashUtils.CreatePetSkillElement(info));
					}
					value = true;
					message = "Başarılı!";
				}
			}
			catch (Exception ex)
			{
				petskillelementinfo.log.Error("Load petskillelementinfo is fail!", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			return csFunction.CreateCompressXml(context, result, "petskillelementinfo", false);
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001BE RID: 446 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000064 RID: 100
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
