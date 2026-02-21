using System;
using System.Web;
using System.Xml.Linq;
using Bussiness;
using Road.Flash;
using SqlDataProvider.Data;

namespace Tank.Request
{
	// Token: 0x0200006E RID: 110
	public class shopcheapitemlist : IHttpHandler
	{
		// Token: 0x060001FF RID: 511 RVA: 0x0000F8D4 File Offset: 0x0000DAD4
		public void ProcessRequest(HttpContext context)
		{
			bool value = false;
			string message = "Fail!";
			XElement result = new XElement("Result");
			try
			{
				using (ProduceBussiness db = new ProduceBussiness())
				{
					ShopItemInfo[] shop = db.GetALllShop();
					foreach (ShopItemInfo s in shop)
					{
						bool flag = s.IsCheap && s.EndDate > DateTime.Now && s.Label == 4f;
						if (flag)
						{
							result.Add(FlashUtils.CreateShopCheapItems(s));
						}
					}
					value = true;
					message = "Success!";
				}
			}
			catch
			{
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			context.Response.ContentType = "text/plain";
			context.Response.Write(result.ToString(false));
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000200 RID: 512 RVA: 0x0000FA00 File Offset: 0x0000DC00
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}
