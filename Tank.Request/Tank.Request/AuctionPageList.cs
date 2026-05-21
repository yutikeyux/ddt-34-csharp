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
	// Token: 0x0200000D RID: 13
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class AuctionPageList : IHttpHandler
	{
		// Token: 0x06000032 RID: 50 RVA: 0x00003614 File Offset: 0x00001814
		public void ProcessRequest(HttpContext context)
		{
			bool value = false;
			string message = "Başarısız!";
			int total = 0;
			XElement result = new XElement("Result");
			try
			{
				int page = int.Parse(context.Request["page"]);
				string name = csFunction.ConvertSql(HttpUtility.UrlDecode(context.Request["name"]));
				int type = int.Parse(context.Request["type"]);
				int pay = int.Parse(context.Request["pay"]);
				int userID = int.Parse(context.Request["userID"]);
				int buyID = int.Parse(context.Request["buyID"]);
				int order = int.Parse(context.Request["order"]);
				bool sort = bool.Parse(context.Request["sort"]);
				string AuctionIDs = csFunction.ConvertSql(HttpUtility.UrlDecode(context.Request["Auctions"]));
				AuctionIDs = (string.IsNullOrEmpty(AuctionIDs) ? "0" : AuctionIDs);
				int size = 50;
				using (PlayerBussiness db = new PlayerBussiness())
				{
					AuctionInfo[] infos = db.GetAuctionPage(page, name, type, pay, ref total, userID, buyID, order, sort, size, AuctionIDs);
					foreach (AuctionInfo info in infos)
					{
						XElement temp = FlashUtils.CreateAuctionInfo(info);
						using (PlayerBussiness pb = new PlayerBussiness())
						{
							ItemInfo item = pb.GetUserItemSingle(info.ItemID);
							bool flag = item != null;
							if (flag)
							{
								temp.Add(FlashUtils.CreateGoodsInfo(item));
							}
							result.Add(temp);
						}
					}
					value = true;
					message = "Success!";
				}
			}
			catch (Exception ex)
			{
				AuctionPageList.log.Error("AuctionPageList", ex);
			}
			result.Add(new XAttribute("total", total));
			result.Add(new XAttribute("value", value));
			result.Add(new XAttribute("message", message));
			context.Response.ContentType = "text/plain";
			context.Response.Write(result.ToString(false));
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000033 RID: 51 RVA: 0x000038C0 File Offset: 0x00001AC0
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400000C RID: 12
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
