using System;
using System.Collections.Generic;
using System.Linq;
using Game.Server.GameObjects;
using Game.Server.Managers;
using SqlDataProvider.Data;

namespace Game.Server.GmActivity
{
	public class SignActive : BaseUserGmActivity
	{
		public SignActive(GamePlayer player, GmActivityInfo gmActivityInfo, List<UserGmActivityCondition> userConditions, List<UserGmActivityReward> userRewards)
			: base(player, gmActivityInfo, userConditions, userRewards)
		{
		}

		public override void SetValue(int value)
		{
			List<GmGiftInfo> list = GmActivityMgr.FindGmGifts(ActivityInfo.activityId);
			foreach (GmGiftInfo item in list)
			{
				GmActiveConditionInfo gmCondition = GmActivityMgr.FindGmActiveCondition(item.giftbagId).First();
				GmActivityInfo SignActivityInfo = ActivityInfo;
				if (SignActivityInfo == null)
				{
					break;
				}
				DateTime today = DateTime.Today;
				int days = (today - SignActivityInfo.beginTime).Days;
				int num = days + 1;
				if (num < 1)
				{
					break;
				}
				List<string> list2 = new List<string>();
				foreach (GmActivityInfo item2 in GmActivityMgr.SignActivity)
				{
					UserGmActivityCondition userGmActivityCondition = UserConditions.Find((UserGmActivityCondition t) => t.GiftBagID == gmCondition.giftbagId);
					if (userGmActivityCondition == null)
					{
						continue;
					}
					List<GmActiveConditionInfo> list3 = GmActivityMgr.FindGmActiveCondition(gmCondition.giftbagId);
					switch (list3[0].conditionIndex)
					{
					case 1:
						if (list3[0].conditionValue == num && userGmActivityCondition.StatusValue == 0)
						{
							userGmActivityCondition.StatusValue = 1;
						}
						break;
					case 2:
						list2.Add(gmCondition.giftbagId);
						break;
					}
				}
				foreach (string giftId in list2)
				{
					UserGmActivityCondition userGmActivityCondition2 = UserConditions.Find((UserGmActivityCondition t) => t.GiftBagID == giftId);
					List<GmActiveConditionInfo> list4 = GmActivityMgr.FindGmActiveCondition(giftId);
					int conditionValue = list4[0].conditionValue;
					if (userGmActivityCondition2.StatusID >= conditionValue)
					{
						continue;
					}
					List<UserGmActivityCondition> list5 = UserConditions.FindAll((UserGmActivityCondition x) => x.ActivityID == SignActivityInfo.activityId);
					List<UserGmActivityCondition> source = list5.FindAll((UserGmActivityCondition z) => GmActivityMgr.FindGmActiveCondition(z.GiftBagID).Any((GmActiveConditionInfo x) => x.conditionIndex == 1));
					int num2 = source.Count((UserGmActivityCondition x) => x.StatusValue != 0);
					userGmActivityCondition2.StatusID = ((num2 > conditionValue) ? conditionValue : num2);
					if (userGmActivityCondition2.StatusID == conditionValue)
					{
						userGmActivityCondition2.StatusValue = 1;
					}
				}
			}
		}

		public override string CanGetRewardMsg(string giftId, ref int times)
		{
			string result = "ok";
			List<GmActiveConditionInfo> source = GmActivityMgr.FindGmActiveCondition(giftId);
			UserGmActivityCondition userGmActivityCondition = UserConditions.Find((UserGmActivityCondition a) => a.GiftBagID == giftId);
			UserGmActivityReward userGmActivityReward = UserRewards.Find((UserGmActivityReward a) => a.GiftBagID == giftId);
			if (userGmActivityCondition == null)
			{
				result = "Koşullar geçersiz!";
			}
			if (userGmActivityCondition.StatusValue != 1)
			{
				result = "Ödül alabilecek durumda değilsiniz!";
			}
			else if (source.Any((GmActiveConditionInfo x) => x.conditionIndex == 2) && userGmActivityCondition.StatusID < source.FirstOrDefault((GmActiveConditionInfo x) => x.conditionIndex == 2).conditionValue)
			{
				result = "Ödül alabilecek durumda değilsiniz!";
			}
			if (userGmActivityReward.Times > 0)
			{
				result = "Daha önce ödül almışsınız.";
			}
			return result;
		}
	}
}
