using System.Collections.Generic;
using System.Reflection;
using Bussiness.Managers;
using Game.Server.GameObjects;
using Game.Server.Managers;
using log4net;
using SqlDataProvider.Data;

namespace Game.Server.GmActivity
{
	public class BaseUserGmActivity
	{
		public static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private GamePlayer Player;

		public GmActivityInfo ActivityInfo;

		public List<UserGmActivityCondition> UserConditions;

		public List<UserGmActivityReward> UserRewards;

		public BaseUserGmActivity(GamePlayer player, GmActivityInfo gmActivityInfo, List<UserGmActivityCondition> userGmActivityConditions, List<UserGmActivityReward> userGmActivityRewards)
		{
			Player = player;
			ActivityInfo = gmActivityInfo;
			UserConditions = userGmActivityConditions;
			UserRewards = userGmActivityRewards;
		}

		public static BaseUserGmActivity CreateGmActivity(GamePlayer player, GmActivityInfo gmActivityInfo, List<UserGmActivityCondition> userGmActivityConditions, List<UserGmActivityReward> userGmActivityRewards)
		{
			int activityType = gmActivityInfo.activityType;
			if (1 == 0)
			{
			}
			BaseUserGmActivity result = activityType switch
			{
				0 => new ChargeActivity(player, gmActivityInfo, userGmActivityConditions, userGmActivityRewards), 
				1 => new ConsumeActivity(player, gmActivityInfo, userGmActivityConditions, userGmActivityRewards), 
				2 => new ExchangeActivity(player, gmActivityInfo, userGmActivityConditions, userGmActivityRewards), 
				8 => new ItemStrength(player, gmActivityInfo, userGmActivityConditions, userGmActivityRewards), 
				14 => new MountMaster(player, gmActivityInfo, userGmActivityConditions, userGmActivityRewards), 
				15 => new CarnivalActivity(player, gmActivityInfo, userGmActivityConditions, userGmActivityRewards), 
				20 => new TempleUp(player, gmActivityInfo, userGmActivityConditions, userGmActivityRewards), 
				31 => new SignActive(player, gmActivityInfo, userGmActivityConditions, userGmActivityRewards),
				32 => new LoginStreakActivity(player, gmActivityInfo, userGmActivityConditions, userGmActivityRewards),
				33 => new BattleWinActivity(player, gmActivityInfo, userGmActivityConditions, userGmActivityRewards),
				34 => new GuildDonateActivity(player, gmActivityInfo, userGmActivityConditions, userGmActivityRewards),
				35 => new LevelUpActivity(player, gmActivityInfo, userGmActivityConditions, userGmActivityRewards),
				36 => new BossKillActivity(player, gmActivityInfo, userGmActivityConditions, userGmActivityRewards),
				37 => new PetTrainingActivity(player, gmActivityInfo, userGmActivityConditions, userGmActivityRewards),
				_ => new NullActive(player, gmActivityInfo, userGmActivityConditions, userGmActivityRewards), 
			};
			if (1 == 0)
			{
			}
			return result;
		}

		public virtual void AfterLoad(GamePlayer player)
		{
		}

		public virtual void AddTrigger(GamePlayer player)
		{
		}

		public virtual void RemoveTrigger(GamePlayer player)
		{
		}

		public virtual void SetValue(int Value)
		{
		}

		public virtual string CanGetRewardMsg(string giftId, ref int times)
		{
			return "ok";
		}

		public virtual List<ItemInfo> RewardList(string giftId, int times = 1)
		{
			List<ItemInfo> list = new List<ItemInfo>();
			List<GmActiveRewardInfo> list2 = GmActivityMgr.FindGmActiveReward(giftId);
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
