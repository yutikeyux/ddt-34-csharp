using System.Collections.Generic;
using Game.Logic;
using Game.Server.GameObjects;
using Game.Server.GameUtils;
using Game.Server.Managers;
using SqlDataProvider.Data;

namespace Game.Server.GmActivity
{
	public class CarnivalActivity : BaseUserGmActivity
	{
		private GamePlayer Player;

		public CarnivalActivity(GamePlayer player, GmActivityInfo gmActivityInfo, List<UserGmActivityCondition> userConditions, List<UserGmActivityReward> userRewards)
			: base(player, gmActivityInfo, userConditions, userRewards)
		{
			Player = player;
		}

		

		

		private void Player_NewGearEvent1(int CategoryID)
		{
			int num = 0;
			for (int i = 0; i < 31; i++)
			{
				ItemInfo ıtemAt = Player.EquipBag.GetItemAt(i);
				if (ıtemAt != null)
				{
					num += ıtemAt.StrengthenLevel;
				}
			}
			SetValue(num);
		}

		private void Player_NewGearEvent(int CategoryID)
		{
			int num = 0;
			for (int i = 0; i < 31; i++)
			{
				ItemInfo ıtemAt = Player.EquipBag.GetItemAt(i);
				if (ıtemAt != null)
				{
					num += ıtemAt.AttackCompose + ıtemAt.DefendCompose + ıtemAt.AgilityCompose + ıtemAt.LuckCompose;
				}
			}
			SetValue(num);
		}

		

		private void Player_UpdateFightPowerEvent(GamePlayer player)
		{
			SetValue((int)player.PlayerCharacter.FightPower);
		}

		private void Player_LevelUp(GamePlayer player)
		{
			SetValue(player.PlayerCharacter.Grade);
		}

		private void Player_UserToemGemstonetEvent()
		{
			int num = Player.PlayerCharacter.totemId - 10000;
			if (num % 7 == 0)
			{
				int value = num / 7;
				SetValue(value);
			}
		}

		public override void SetValue(int value)
		{
			List<GmGiftInfo> list = GmActivityMgr.FindGmGifts(ActivityInfo.activityId);
			foreach (GmGiftInfo gmGift in list)
			{
				UserGmActivityCondition userGmActivityCondition = UserConditions.Find((UserGmActivityCondition t) => t.GiftBagID == gmGift.giftbagId);
				if (value > userGmActivityCondition.StatusValue)
				{
					userGmActivityCondition.StatusValue = value;
				}
				GmActiveConditionInfo gmActiveConditionInfo = GmActivityMgr.FindGmActiveCondition(gmGift.giftbagId).Find((GmActiveConditionInfo x) => x.conditionIndex == 0);
				if (gmActiveConditionInfo != null && value >= gmActiveConditionInfo.conditionValue)
				{
					userGmActivityCondition.StatusID = 1;
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
			if (userGmActivityCondition.StatusValue < list[0].conditionValue)
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
