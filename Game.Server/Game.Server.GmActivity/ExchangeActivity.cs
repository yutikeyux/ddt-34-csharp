using System.Collections.Generic;
using Bussiness.Managers;
using Game.Server.GameObjects;
using Game.Server.Managers;
using SqlDataProvider.Data;

namespace Game.Server.GmActivity
{
	public class ExchangeActivity : BaseUserGmActivity
	{
		private GamePlayer Player;

		public ExchangeActivity(GamePlayer player, GmActivityInfo gmActivityInfo, List<UserGmActivityCondition> userConditions, List<UserGmActivityReward> userRewards)
			: base(player, gmActivityInfo, userConditions, userRewards)
		{
			Player = player;
		}

		public override string CanGetRewardMsg(string giftId, ref int times)
		{
			string result = "ok";
			List<GmActiveRewardInfo> list = GmActivityMgr.FindGmActiveReward(giftId);
			if (list == null || list.Count == 0)
			{
				result = "Etkinlik koşulları geçersiz. Yöneticiye bildiriniz.";
			}
			else if (ActivityInfo.activityChildType == 0)
			{
				List<GmActiveRewardInfo> list2 = list.FindAll((GmActiveRewardInfo r) => r.rewardType == 0);
				foreach (GmActiveRewardInfo item in list2)
				{
					if (Player.GetItemCount(item.templateId) < item.count * times)
					{
						return "Takas öğesi yetersiz!";
					}
				}
				foreach (GmActiveRewardInfo item2 in list2)
				{
					Player.RemoveTemplate(item2.templateId, item2.count * times);
				}
			}
			return result;
		}

		public override List<ItemInfo> RewardList(string giftId, int times = 1)
		{
			List<ItemInfo> list = new List<ItemInfo>();
			List<GmActiveRewardInfo> list2 = GmActivityMgr.FindGmActiveReward(giftId).FindAll((GmActiveRewardInfo r) => r.rewardType == 1);
			foreach (GmActiveRewardInfo item in list2)
			{
				ItemInfo ıtemInfo = ItemMgr.CreateInfoFromGmReward(item, times);
				if (ıtemInfo != null)
				{
					list.Add(ıtemInfo);
				}
			}
			return list;
		}
	}
}
