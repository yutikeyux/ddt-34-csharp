using System;
using System.Web;
using System.Xml.Linq;
using Bussiness;
using Road.Flash;
using SqlDataProvider.Data;

namespace Tank.Request
{
	// Token: 0x02000073 RID: 115
	public class ShopGoodsShowList : IHttpHandler
	{
		// Token: 0x06000208 RID: 520 RVA: 0x0000F470 File Offset: 0x0000D670
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(ShopGoodsShowList.Build(context));
			}
			else
			{
				context.Response.Write("Tabi Efendim!");
			}
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000F4BC File Offset: 0x0000D6BC
		public static string Build(HttpContext context)
		{
			bool value = false;
			string message = "Hata!";
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
					message = "Başarılı!";
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
		// (get) Token: 0x0600020A RID: 522 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}
