using System;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness;
using Road.Flash;
using SqlDataProvider.Data;

namespace Tank.Request
{
	// Token: 0x02000070 RID: 112
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class ShopItemList : IHttpHandler
	{
		// Token: 0x06000206 RID: 518 RVA: 0x0000FB74 File Offset: 0x0000DD74
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(ShopItemList.Build(context));
			}
			else
			{
				context.Response.Write("IP is not valid!");
			}
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000FBC0 File Offset: 0x0000DDC0
		public static string Build(HttpContext context)
		{
			bool value = false;
			string message = "Fail!";
			XElement result = new XElement("Result");
			try
			{
				using (ProduceBussiness db = new ProduceBussiness())
				{
					XElement Store = new XElement("Store");
					ShopItemInfo[] shop = db.GetALllShop();
					foreach (ShopItemInfo s in shop)
					{
						Store.Add(FlashUtils.CreateShopInfo(s));
					}
					result.Add(Store);
					value = true;
					message = "Success!";
				}
			}
			catch
			{
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			return csFunction.CreateCompressXml(context, result, "ShopItemList", true);
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000208 RID: 520 RVA: 0x0000FCC0 File Offset: 0x0000DEC0
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}
