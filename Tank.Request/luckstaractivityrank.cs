using System;
using System.Web;
using System.Xml.Linq;
using Bussiness;
using Road.Flash;
using SqlDataProvider.Data;

namespace Tank.Request
{
	// Token: 0x02000055 RID: 85
	public class luckstaractivityrank : IHttpHandler
	{
		// Token: 0x06000188 RID: 392 RVA: 0x0000CC70 File Offset: 0x0000AE70
		public void ProcessRequest(HttpContext context)
		{
			int selfid = Convert.ToInt32(context.Request["selfid"]);
			string key = context.Request["key"];
			XElement ranks = new XElement("Ranks");
			LuckstarActivityRankInfo myRankInfo = new LuckstarActivityRankInfo();
			myRankInfo.nickName = "";
			using (PlayerBussiness db = new PlayerBussiness())
			{
				LuckstarActivityRankInfo[] LuckstarActivityRanks = db.GetAllLuckstarActivityRank();
				foreach (LuckstarActivityRankInfo r in LuckstarActivityRanks)
				{
					ranks.Add(FlashUtils.LuckstarActivityRank(r));
					bool flag = r.UserID == selfid;
					if (flag)
					{
						myRankInfo = r;
					}
				}
			}
			XElement myRank = new XElement("myRank", new object[]
			{
				new XAttribute("rank", myRankInfo.rank),
				new XAttribute("useStarNum", myRankInfo.useStarNum),
				new XAttribute("nickName", myRankInfo.nickName)
			});
			ranks.Add(myRank);
			bool value = true;
			string message = "Başarılı!";
			ranks.Add(new XAttribute("lastUpdateTime", DateTime.Now.ToString("MM-dd hh:mm")));
			ranks.Add(new XAttribute("value", value));
			ranks.Add(new XAttribute("message", message));
			context.Response.ContentType = "text/plain";
			context.Response.Write(ranks.ToString(false));
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000189 RID: 393 RVA: 0x0000CE4C File Offset: 0x0000B04C
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}
