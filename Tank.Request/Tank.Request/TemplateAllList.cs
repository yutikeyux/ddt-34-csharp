using System;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness;
using Road.Flash;
using SqlDataProvider.Data;

namespace Tank.Request
{
	// Token: 0x02000078 RID: 120
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class TemplateAllList : IHttpHandler
	{
		// Token: 0x0600022D RID: 557 RVA: 0x00010A68 File Offset: 0x0000EC68
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(TemplateAllList.Bulid(context));
			}
			else
			{
				context.Response.Write("IP is not valid!");
			}
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00010AB4 File Offset: 0x0000ECB4
		public static string Bulid(HttpContext context)
		{
			bool value = false;
			string message = "Fail!";
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
					message = "Success!";
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
		// (get) Token: 0x0600022F RID: 559 RVA: 0x00010BB4 File Offset: 0x0000EDB4
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}
