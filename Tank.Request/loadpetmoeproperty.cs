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
	// Token: 0x02000049 RID: 73
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class loadpetmoeproperty : IHttpHandler
	{
		// Token: 0x06000155 RID: 341 RVA: 0x0000AB84 File Offset: 0x00008D84
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(loadpetmoeproperty.Build(context));
			}
			else
			{
				context.Response.Write("IP is not valid!");
			}
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000ABCC File Offset: 0x00008DCC
		public static string Build(HttpContext context)
		{
			bool flag = false;
			string str = "Başarısız!";
			XElement result = new XElement("Result");
			try
			{
				using (ProduceBussiness produceBussiness = new ProduceBussiness())
				{
					XElement xelement = new XElement("ItemTemplate");
					foreach (PetMoePropertyInfo petm in produceBussiness.GetAllPetMoeProperty())
					{
						xelement.Add(FlashUtils.CreatePetMoePropertyItems(petm));
					}
					result.Add(xelement);
					flag = true;
					str = "Başarılı!";
				}
			}
			catch (Exception ex)
			{
				loadpetmoeproperty.log.Error("loadpetmoeproperty", ex);
			}
			result.Add(new XAttribute("value", flag));
			result.Add(new XAttribute("message", str));
			csFunction.CreateCompressXml(context, result, "loadpetmoeproperty_out", false);
			return csFunction.CreateCompressXml(context, result, "loadpetmoeproperty", true);
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000157 RID: 343 RVA: 0x0000ACE8 File Offset: 0x00008EE8
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400004D RID: 77
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
