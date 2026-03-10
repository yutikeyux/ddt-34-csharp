using System;
using System.Collections.Generic;
using System.Web;
using System.Xml.Linq;
using Bussiness;
using SqlDataProvider.Data;

namespace Tank.Request
{
	// Token: 0x02000031 RID: 49
	public class eventrewarditemlist : IHttpHandler
	{
		// Token: 0x060000E7 RID: 231 RVA: 0x00007BA8 File Offset: 0x00005DA8
		public void ProcessRequest(HttpContext context)
		{
			bool flag = csFunction.ValidAdminIP(context.Request.UserHostAddress);
			if (flag)
			{
				context.Response.Write(eventrewarditemlist.Build(context));
			}
			else
			{
				context.Response.Write("IP is not valid!");
			}
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00007BF4 File Offset: 0x00005DF4
		public static string Build(HttpContext context)
		{
			bool value = false;
			string message = "Başarısız!";
			XElement result = new XElement("Result");
			try
			{
				using (ProduceBussiness db = new ProduceBussiness())
				{
					Dictionary<int, Dictionary<int, EventRewardInfo>> EventRewardInfo = new Dictionary<int, Dictionary<int, EventRewardInfo>>();
					EventRewardInfo[] eventInfos = db.GetAllEventRewardInfo();
					EventRewardGoodsInfo[] eventGoods = db.GetAllEventRewardGoods();
					foreach (EventRewardInfo item in eventInfos)
					{
						item.AwardLists = new List<EventRewardGoodsInfo>();
						bool flag = !EventRewardInfo.ContainsKey(item.ActivityType);
						if (flag)
						{
							Dictionary<int, EventRewardInfo> tmp = new Dictionary<int, EventRewardInfo>();
							tmp.Add(item.SubActivityType, item);
							EventRewardInfo.Add(item.ActivityType, tmp);
						}
						else
						{
							bool flag2 = !EventRewardInfo[item.ActivityType].ContainsKey(item.SubActivityType);
							if (flag2)
							{
								EventRewardInfo[item.ActivityType].Add(item.SubActivityType, item);
							}
						}
					}
					foreach (EventRewardGoodsInfo good in eventGoods)
					{
						bool flag3 = EventRewardInfo.ContainsKey(good.ActivityType) && EventRewardInfo[good.ActivityType].ContainsKey(good.SubActivityType);
						if (flag3)
						{
							EventRewardInfo[good.ActivityType][good.SubActivityType].AwardLists.Add(good);
						}
					}
					XElement ActiveType = null;
					foreach (Dictionary<int, EventRewardInfo> eventInActive in EventRewardInfo.Values)
					{
						foreach (EventRewardInfo info in eventInActive.Values)
						{
							bool flag4 = ActiveType == null;
							if (flag4)
							{
								ActiveType = new XElement("ActivityType", new XAttribute("value", info.ActivityType));
							}
							XElement Items = new XElement("Items", new object[]
							{
								new XAttribute("SubActivityType", info.SubActivityType),
								new XAttribute("Condition", info.Condition)
							});
							foreach (EventRewardGoodsInfo awardGood in info.AwardLists)
							{
								XElement Item = new XElement("Item", new object[]
								{
									new XAttribute("TemplateId", awardGood.TemplateId),
									new XAttribute("StrengthLevel", awardGood.StrengthLevel),
									new XAttribute("AttackCompose", awardGood.AttackCompose),
									new XAttribute("DefendCompose", awardGood.DefendCompose),
									new XAttribute("LuckCompose", awardGood.LuckCompose),
									new XAttribute("AgilityCompose", awardGood.AgilityCompose),
									new XAttribute("IsBind", awardGood.IsBind),
									new XAttribute("ValidDate", awardGood.ValidDate),
									new XAttribute("Count", awardGood.Count)
								});
								Items.Add(Item);
							}
							ActiveType.Add(Items);
						}
						result.Add(ActiveType);
						ActiveType = null;
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
			csFunction.CreateCompressXml(context, result, "eventrewarditemlist_out", false);
			return csFunction.CreateCompressXml(context, result, "eventrewarditemlist", true);
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x000080EC File Offset: 0x000062EC
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}
