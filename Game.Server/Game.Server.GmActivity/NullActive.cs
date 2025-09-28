using System.Collections.Generic;
using Game.Server.GameObjects;
using SqlDataProvider.Data;

namespace Game.Server.GmActivity
{
	public class NullActive : BaseUserGmActivity
	{
		public NullActive(GamePlayer player, GmActivityInfo gmActivityInfo, List<UserGmActivityCondition> userConditions, List<UserGmActivityReward> userRewards)
			: base(player, gmActivityInfo, userConditions, userRewards)
		{
		}

		public override void SetValue(int value)
		{
		}

		public override string CanGetRewardMsg(string giftId, ref int times)
		{
			return "Bu etkinlik türü tanımlı değil!";
		}

		public override List<ItemInfo> RewardList(string giftId, int times = 1)
		{
			return null;
		}
	}
}
