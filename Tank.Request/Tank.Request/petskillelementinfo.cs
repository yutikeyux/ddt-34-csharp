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
	// Token: 0x02000060 RID: 96
	public class petskillelementinfo : IHttpHandler
	{
		// Token: 0x060001BA RID: 442 RVA: 0x0000DDB5 File Offset: 0x0000BFB5
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(petskillelementinfo.build(context));
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000DDCC File Offset: 0x0000BFCC
		public static string build(HttpContext context)
		{
			bool value = false;
			string message = "Başarısız!";
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
					message = "Success!";
				}
			}
			catch (Exception ex)
			{
				petskillelementinfo.log.Error("Load petskillelementinfo is Başarısız!", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			return csFunction.CreateCompressXml(context, result, "petskillelementinfo", false);
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001BC RID: 444 RVA: 0x0000DEC4 File Offset: 0x0000C0C4
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000063 RID: 99
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
