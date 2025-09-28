using System.Collections.Generic;
using System.Linq;
using Game.Server.GameObjects;
using Game.Server.Managers;
using SqlDataProvider.Data;

namespace Game.Server.GmActivity
{
	public class ItemStrength : BaseUserGmActivity
	{
		public ItemStrength(GamePlayer player, GmActivityInfo gmActivityInfo, List<UserGmActivityCondition> userConditions, List<UserGmActivityReward> userRewards)
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
				if ((userGmActivityCondition.StatusID <= 0 || userGmActivityCondition.StatusValue != 0) && gmCondition.conditionValue == value)
				{
					userGmActivityCondition.StatusValue = value;
					userGmActivityCondition.StatusID = gmCondition.conditionValue;
				}
			}
		}

		public override string CanGetRewardMsg(string giftId, ref int times)
		{
			string result = "ok";
			if (ActivityInfo.activityChildType == 0)
			{
				UserGmActivityCondition userGmActivityCondition = UserConditions.Find((UserGmActivityCondition a) => a.GiftBagID == giftId);
				List<GmActiveConditionInfo> source = GmActivityMgr.FindGmActiveCondition(giftId);
				if (userGmActivityCondition == null)
				{
					result = "Etkinlik için gerekli olan bir koşul bulunamadı. Yöneticiye bildirin.";
				}
				if (userGmActivityCondition.StatusID < source.First().conditionValue || userGmActivityCondition.StatusValue == 0)
				{
					result = "Koşullar geçersiz!";
				}
			}
			return result;
		}
	}
}
