using System;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness;
using log4net;
using Road.Flash;
using SqlDataProvider.Data;

namespace Tank.Request
{
	// Token: 0x02000061 RID: 97
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class petlevellist : IHttpHandler
	{
		// Token: 0x060001B7 RID: 439 RVA: 0x0000D91C File Offset: 0x0000BB1C
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(petlevellist.Build(context));
			}
			else
			{
				context.Response.Write("Tabi Efendim!");
			}
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000D968 File Offset: 0x0000BB68
		public static string Build(HttpContext context)
		{
			bool value = false;
			string message = "Hata!";
			XElement result = new XElement("Result");
			int t = 0;
			try
			{
				using (ProduceBussiness db = new ProduceBussiness())
				{
					PetLevel[] infos = db.GetAllPetLevel();
					t = infos.Length;
					foreach (PetLevel info in infos)
					{
						result.Add(FlashUtils.CreatePetLevels(info));
					}
					value = true;
					message = "Başarılı!";
				}
			}
			catch (Exception ex)
			{
				petlevellist.log.Error("Load petlevellist is fail!", ex);
			}
			result.Add(new XAttribute("total", t));
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			return csFunction.CreateCompressXml(context, result, "petlevelinfo", false);
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x00003828 File Offset: 0x00001A28
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
