using System.Collections.Generic;
using System.Linq;
using Game.Server.GameObjects;
using Game.Server.Managers;
using SqlDataProvider.Data;

namespace Game.Server.GmActivity
{
	public class MountMaster : BaseUserGmActivity
	{
		public MountMaster(GamePlayer player, GmActivityInfo gmActivityInfo, List<UserGmActivityCondition> userConditions, List<UserGmActivityReward> userRewards)
			: base(player, gmActivityInfo, userConditions, userRewards)
		{
		}

		public override void SetValue(int value)
		{
			List<GmGiftInfo> list = GmActivityMgr.FindGmGifts(ActivityInfo.activityId);
			foreach (GmGiftInfo item in list)
			{
				GmActiveConditionInfo gmCondition = GmActivityMgr.FindGmActiveCondition(item.giftbagId).First();
				UserGmActivityCondition userGmActivityCondition = UserConditions.Find((UserGmActivityCondition t) => t.GiftBagID == gmCondition.giftbagId);
				if ((userGmActivityCondition == null || userGmActivityCondition.StatusID <= 0 || userGmActivityCondition.StatusValue != 0) && value == gmCondition.remain1)
				{
					userGmActivityCondition.StatusID = 1;
					userGmActivityCondition.StatusValue = value;
					break;
				}
			}
		}

		public override string CanGetRewardMsg(string giftId, ref int times)
		{
			string result = "ok";
			List<GmActiveConditionInfo> list = GmActivityMgr.FindGmActiveCondition(giftId);
			UserGmActivityCondition userGmActivityCondition = UserConditions.Find((UserGmActivityCondition a) => a.GiftBagID == giftId);
			UserGmActivityReward userGmActivityReward = UserRewards.Find((UserGmActivityReward a) => a.GiftBagID == giftId);
			if (userGmActivityCondition == null || list == null || list.Count == 0)
			{
				result = "Koşullar geçersiz!";
			}
			if (userGmActivityCondition.StatusValue < list[0].remain1)
			{
				result = "Ödül alabilecek aşamaya ulaşmamışsınız!";
			}
			if (userGmActivityReward.Times > 0)
			{
				result = "Daha önce ödül almışsınız.";
			}
			return result;
		}
	}
}
