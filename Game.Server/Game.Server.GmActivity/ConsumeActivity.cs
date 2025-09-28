using System.Collections.Generic;
using System.Linq;
using Game.Server.GameObjects;
using Game.Server.Managers;
using SqlDataProvider.Data;

namespace Game.Server.GmActivity
{
	public class ConsumeActivity : BaseUserGmActivity
	{
		public ConsumeActivity(GamePlayer player, GmActivityInfo gmActivityInfo, List<UserGmActivityCondition> userConditions, List<UserGmActivityReward> userRewards)
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
				if (userGmActivityCondition == null || userGmActivityCondition.StatusID <= 0 || userGmActivityCondition.StatusValue != 0)
				{
					userGmActivityCondition.StatusValue += value;
					userGmActivityCondition.StatusID = gmCondition.conditionValue;
				}
			}
		}

		public override string CanGetRewardMsg(string giftId, ref int times)
		{
			string result = "ok";
			List<GmActiveConditionInfo> list = GmActivityMgr.FindGmActiveCondition(giftId);
			UserGmActivityCondition userGmActivityCondition = UserConditions.Find((UserGmActivityCondition a) => a.GiftBagID == giftId);
			UserGmActivityReward userGmActivityReward = UserRewards.Find((UserGmActivityReward a) => a.GiftBagID == giftId);
			if (ActivityInfo.activityChildType != 6 && ActivityInfo.activityChildType != 2 && ActivityInfo.activityChildType != 1)
			{
				result = "Etkinlik tipi tanınmadı. Yöneticiye bildirin";
			}
			else if (userGmActivityCondition == null || list == null || list.Count == 0)
			{
				result = "Koşullar geçersiz!";
			}
			else
			{
				if (userGmActivityCondition.StatusValue < list[0].conditionValue)
				{
					result = "Ödül alabilecek aşamaya ulaşmamışsınız!";
				}
				if (list[2].conditionValue == 0)
				{
					times = userGmActivityCondition.StatusValue / list[0].conditionValue;
				}
				times -= userGmActivityReward.Times;
				if (times < 1)
				{
					result = "Alabileceğiniz ödül bulunmuyor.";
				}
			}
			return result;
		}
	}
}
