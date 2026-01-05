using System;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;
using Bussiness;
using Road.Flash;
using SqlDataProvider.Data;

namespace Tank.Request
{
	// Token: 0x02000074 RID: 116
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class ShopItemList : IHttpHandler
	{
		// Token: 0x0600020C RID: 524 RVA: 0x0000F5BC File Offset: 0x0000D7BC
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(ShopItemList.Build(context));
			}
			else
			{
				context.Response.Write("Tabi Efendim!");
			}
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000F608 File Offset: 0x0000D808
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
					ShopItemInfo[] shop = db.GetALllShop();
					foreach (ShopItemInfo s in shop)
					{
						Store.Add(FlashUtils.CreateShopInfo(s));
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
			return csFunction.CreateCompressXml(context, result, "ShopItemList", true);
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600020E RID: 526 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}
