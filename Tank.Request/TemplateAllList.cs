using System;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness;
using Road.Flash;
using SqlDataProvider.Data;

namespace Tank.Request
{
	// Token: 0x0200007C RID: 124
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class TemplateAllList : IHttpHandler
	{
		// Token: 0x06000233 RID: 563 RVA: 0x00010370 File Offset: 0x0000E570
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(TemplateAllList.Build(context));
			}
			else
			{
				context.Response.Write("Tabi Efendim!");
			}
		}

		// Token: 0x06000234 RID: 564 RVA: 0x000103BC File Offset: 0x0000E5BC
		public static string Build(HttpContext context)
		{
			bool value = false;
			string message = "Hata!";
			XElement result = new XElement("Result");
			try
			{
				using (ProduceBussiness db = new ProduceBussiness())
				{
					XElement template = new XElement("ItemTemplate");
					ItemTemplateInfo[] items = db.GetAllGoods();
					foreach (ItemTemplateInfo g in items)
					{
						template.Add(FlashUtils.CreateItemInfo(g));
					}
					result.Add(template);
					value = true;
					message = "Başarılı!";
				}
			}
			catch
			{
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			return csFunction.CreateCompressXml(context, result, "TemplateAlllist", true);
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000235 RID: 565 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}
