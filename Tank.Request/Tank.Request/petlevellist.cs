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
	// Token: 0x0200005F RID: 95
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class petlevellist : IHttpHandler
	{
		// Token: 0x060001B5 RID: 437 RVA: 0x0000DC20 File Offset: 0x0000BE20
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(petlevellist.Bulid(context));
			}
			else
			{
				context.Response.Write("IP is not valid!");
			}
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000DC6C File Offset: 0x0000BE6C
		public static string Bulid(HttpContext context)
		{
			bool value = false;
			string message = "Fail!";
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
					message = "Success!";
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
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x0000DD8C File Offset: 0x0000BF8C
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000062 RID: 98
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
