using System;
using System.Web;
using System.Xml.Linq;
using Bussiness;
using Road.Flash;
using SqlDataProvider.Data;

namespace Tank.Request
{
	// Token: 0x02000072 RID: 114
	public class shopcheapitemlist : IHttpHandler
	{
		// Token: 0x06000205 RID: 517 RVA: 0x0000F344 File Offset: 0x0000D544
		public void ProcessRequest(HttpContext context)
		{
			bool value = false;
			string message = "Hata!";
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
					message = "Başarılı!";
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
		// (get) Token: 0x06000206 RID: 518 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}
