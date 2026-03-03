using System;
using System.Web;
using System.Xml.Linq;
using Bussiness;
using Road.Flash;
using SqlDataProvider.Data;

namespace Tank.Request
{
	// Token: 0x0200006F RID: 111
	public class ShopGoodsShowList : IHttpHandler
	{
		// Token: 0x06000202 RID: 514 RVA: 0x0000FA14 File Offset: 0x0000DC14
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(ShopGoodsShowList.Build(context));
			}
			else
			{
				context.Response.Write("IP is not valid!");
			}
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000FA60 File Offset: 0x0000DC60
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
					ShopGoodsShowListInfo[] shop = db.GetAllShopGoodsShowList();
					foreach (ShopGoodsShowListInfo s in shop)
					{
						Store.Add(FlashUtils.CreateShopShowInfo(s));
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
			return csFunction.CreateCompressXml(context, result, "ShopGoodsShowList", true);
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000204 RID: 516 RVA: 0x0000FB60 File Offset: 0x0000DD60
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}
