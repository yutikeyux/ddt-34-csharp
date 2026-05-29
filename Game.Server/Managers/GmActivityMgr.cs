using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bussiness;
using log4net;
using SqlDataProvider.Data;

namespace Game.Server.Managers
{
	public class GmActivityMgr
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public static Dictionary<int, GmActivityInfo> DiscountActives = new Dictionary<int, GmActivityInfo>();

		public static GmActivityInfo FoodActivity;

		public static GmActivityInfo FlowerGivingActivity;

		public static List<GmActivityInfo> PayActives = new List<GmActivityInfo>();

		public static List<GmActivityInfo> ConsumeActives = new List<GmActivityInfo>();

		public static List<GmActivityInfo> StrengthenActives = new List<GmActivityInfo>();

		public static List<GmActivityInfo> MountActives = new List<GmActivityInfo>();

		public static List<GmActivityInfo> TempleActives = new List<GmActivityInfo>();

		public static List<GmActivityInfo> SignActivity = new List<GmActivityInfo>();

		public static List<GmActivityInfo> LoginStreakActives = new List<GmActivityInfo>();

		public static List<GmActivityInfo> BattleWinActives = new List<GmActivityInfo>();

		public static List<GmActivityInfo> GuildDonateActives = new List<GmActivityInfo>();

		public static List<GmActivityInfo> LevelUpActives = new List<GmActivityInfo>();

		public static List<GmActivityInfo> BossKillActives = new List<GmActivityInfo>();

		public static List<GmActivityInfo> PetTrainingActives = new List<GmActivityInfo>();

		public static List<GmActivityInfo> GmActivityInfos;

		public static List<GmGiftInfo> GmGiftInfos;

		public static List<GmActiveConditionInfo> GmActiveConditionInfos;

		public static List<GmActiveRewardInfo> GmActiveRewardInfos;

		private static void LoadActiveList()
		{
			DiscountActives = new Dictionary<int, GmActivityInfo>();
			GmActivityInfo gmActivityInfo = null;
			GmActivityInfo gmActivityInfo2 = null;
			PayActives = new List<GmActivityInfo>();
			ConsumeActives = new List<GmActivityInfo>();
			StrengthenActives = new List<GmActivityInfo>();
			MountActives = new List<GmActivityInfo>();
			TempleActives = new List<GmActivityInfo>();
			SignActivity = new List<GmActivityInfo>();
			LoginStreakActives = new List<GmActivityInfo>();
			BattleWinActives = new List<GmActivityInfo>();
			GuildDonateActives = new List<GmActivityInfo>();
			LevelUpActives = new List<GmActivityInfo>();
			BossKillActives = new List<GmActivityInfo>();
			PetTrainingActives = new List<GmActivityInfo>();
			foreach (GmActivityInfo item in GmActivityInfos.FindAll((GmActivityInfo a) => a.endTime > DateTime.Now && a.beginTime <= DateTime.Now))
			{
				int activityType = item.activityType;
				if (1 == 0)
				{
				}
				List<GmActivityInfo> list = activityType switch
				{
					0 => PayActives,
					1 => ConsumeActives,
					8 => StrengthenActives,
					14 => MountActives,
					20 => TempleActives,
					31 => SignActivity,
					32 => LoginStreakActives,
					33 => BattleWinActives,
					34 => GuildDonateActives,
					35 => LevelUpActives,
					36 => BossKillActives,
					37 => PetTrainingActives,
					_ => null,
				};
				if (1 == 0)
				{
				}
				List<GmActivityInfo> list2 = list;
				switch (item.activityType)
				{
					default:
						list2?.Add(item);
						break;
					case 101:
						if (!DiscountActives.ContainsKey(item.activityChildType))
						{
							DiscountActives.Add(item.activityChildType, item);
						}
						break;
					case 13:
						gmActivityInfo = item;
						break;
					case 11:
						gmActivityInfo2 = item;
						break;
				}
			}
		}

		public static List<GmActiveConditionInfo> FindGmActiveCondition(string giftId)
		{
			return (from x in GmActiveConditionInfos.FindAll((GmActiveConditionInfo q) => q.giftbagId == giftId)
					orderby x.conditionIndex
					select x).ToList();
		}

		public static List<GmActiveRewardInfo> FindGmActiveReward(string giftId)
		{
			return GmActiveRewardInfos.FindAll((GmActiveRewardInfo q) => q.giftId == giftId);
		}

		public static GmActivityInfo FindGmActivity(string activityId)
		{
			return GmActivityInfos.Find((GmActivityInfo q) => q.activityId == activityId);
		}

		public static List<GmGiftInfo> FindGmGifts(string activeId)
		{
			return GmGiftInfos.FindAll((GmGiftInfo q) => q.activityId == activeId);
		}

		public static GmGiftInfo FindGmGift(string giftId)
		{
			return GmGiftInfos.First((GmGiftInfo q) => q.giftbagId == giftId);
		}

		public static bool Init()
		{
			return ReLoad();
		}

		public static void LoadGmActivityDb()
		{
			using ProduceBussiness produceBussiness = new ProduceBussiness();
			GmActivityInfos = produceBussiness.GetAllGmActivity().ToList();
		}

		public static void LoadGmGiftDb()
		{
			using ProduceBussiness produceBussiness = new ProduceBussiness();
			GmGiftInfos = produceBussiness.GetAllGmGift().ToList();
		}

		public static void LoadGmActiveConditionDb()
		{
			using ProduceBussiness produceBussiness = new ProduceBussiness();
			GmActiveConditionInfos = produceBussiness.GetAllGmActiveCondition().ToList();
		}

		public static void LoadGmActiveRewardDb()
		{
			using ProduceBussiness produceBussiness = new ProduceBussiness();
			GmActiveRewardInfos = produceBussiness.GetAllGmActiveReward().ToList();
		}
        public delegate void PlayerEventHandle(GamePlayer player, int value);
        public static event PlayerEventHandle PlayerVIPLevel;
        public static void OnPlayerUpgradeVIP(GamePlayer player, int VIPLevel)
        {
            PlayerVIPLevel?.Invoke(player, VIPLevel);
        }
        public static bool ReLoad()
		{
			try
			{
				LoadGmActivityDb();
				LoadGmGiftDb();
				LoadGmActiveConditionDb();
				LoadGmActiveRewardDb();
				LoadActiveList();
			}
			catch (Exception exception)
			{
				if (Log.IsErrorEnabled)
				{
					Log.Error("ReLoad GmActivity", exception);
				}
				return false;
			}
			return true;
		}
	}
}
