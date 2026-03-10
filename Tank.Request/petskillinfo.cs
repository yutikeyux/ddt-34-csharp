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
	// Token: 0x02000061 RID: 97
	public class petskillinfo : IHttpHandler
	{
		// Token: 0x060001BF RID: 447 RVA: 0x0000DEED File Offset: 0x0000C0ED
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Write(petskillinfo.Build(context));
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000DF04 File Offset: 0x0000C104
		public static string Build(HttpContext context)
		{
			bool value = false;
			string message = "Başarısız!";
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
				petskillinfo.log.Error("Load petskillinfo is Başarısız!", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			return csFunction.CreateCompressXml(context, result, "petskillinfo", false);
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x0000DFFC File Offset: 0x0000C1FC
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
