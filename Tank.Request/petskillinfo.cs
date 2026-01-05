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
	public class petskillinfo : IHttpHandler
	{
		// Token: 0x060001C1 RID: 449 RVA: 0x000029BD File Offset: 0x00000BBD
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(petskillinfo.Build(context));
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000DB80 File Offset: 0x0000BD80
		public static string Build(HttpContext context)
		{
			bool value = false;
			string message = "Hata!";
			XElement result = new XElement("Result");
			try
			{
				using (ProduceBussiness db = new ProduceBussiness())
				{
					PetSkillInfo[] infos = db.GetAllPetSkillInfo();
					foreach (PetSkillInfo info in infos)
					{
						result.Add(FlashUtils.CreatePetSkillInfo(info));
					}
					value = true;
					message = "Başarılı!";
				}
			}
			catch (Exception ex)
			{
				petskillinfo.log.Error("Load petskillinfo is fail!", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			return csFunction.CreateCompressXml(context, result, "petskillinfo", false);
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x00003828 File Offset: 0x00001A28
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
