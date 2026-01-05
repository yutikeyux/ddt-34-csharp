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
	// Token: 0x0200007F RID: 127
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class UserGoodsInfo : IHttpHandler
	{
		// Token: 0x06000240 RID: 576 RVA: 0x0001082C File Offset: 0x0000EA2C
		public void ProcessRequest(HttpContext context)
		{
			bool value = false;
			string message = "Hata!";
			XElement result = new XElement("Result");
			try
			{
				int goodsID = int.Parse(context.Request.Params["ID"]);
				using (PlayerBussiness db = new PlayerBussiness())
				{
					ItemInfo item = db.GetUserItemSingle(goodsID);
					result.Add(FlashUtils.CreateGoodsInfo(item));
				}
				value = true;
				message = "Başarılı!";
			}
			catch (Exception ex)
			{
				UserGoodsInfo.log.Error("UserGoodsInfo", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			context.Response.ContentType = "text/plain";
			context.Response.Write(result.ToString(false));
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000241 RID: 577 RVA: 0x00003828 File Offset: 0x00001A28
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400008E RID: 142
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
