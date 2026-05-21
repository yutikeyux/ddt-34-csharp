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
	// Token: 0x0200004D RID: 77
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class LoadUserItems : IHttpHandler
	{
		// Token: 0x06000167 RID: 359 RVA: 0x0000B418 File Offset: 0x00009618
		public void ProcessRequest(HttpContext context)
		{
			bool value = false;
			string message = "Başarısız!";
			XElement result = new XElement("Result");
			try
			{
				int userid = int.Parse(context.Request.Params["ID"]);
				using (PlayerBussiness db = new PlayerBussiness())
				{
					ItemInfo[] items = db.GetUserItem(userid);
					foreach (ItemInfo item in items)
					{
						result.Add(FlashUtils.CreateGoodsInfo(item));
					}
				}
				value = true;
				message = "Success!";
			}
			catch (Exception ex)
			{
				LoadUserItems.log.Error("LoadUserItems", ex);
			}
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			context.Response.ContentType = "text/plain";
			context.Response.Write(result.ToString(false));
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000168 RID: 360 RVA: 0x0000B544 File Offset: 0x00009744
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000050 RID: 80
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
