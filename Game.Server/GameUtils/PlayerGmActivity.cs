using System;
using System.Collections.Generic;
using System.Linq;
using Bussiness;
using Game.Server.GameObjects;
using Game.Server.GmActivity;
using Game.Server.Managers;
using SqlDataProvider.Data;

namespace Game.Server.GameUtils
{
	public class PlayerGmActivity
	{
		private GamePlayer m_player;

		private bool m_saveToDb;

		protected object m_lock = new object();

		public List<BaseUserGmActivity> GmActivities = new List<BaseUserGmActivity>();

		public PlayerGmActivity(GamePlayer player, bool saveTodb)
		{
			m_player = player;
			m_saveToDb = saveTodb;
			LoadFromDatabase();
		}

		public void LoadFromDatabase()
		{
			using PlayerBussiness playerBussiness = new PlayerBussiness();
			List<UserGmActivityCondition> userGmActivityConditionInfo = playerBussiness.GetUserGmActivityConditionInfo(m_player.PlayerCharacter.ID);
			List<UserGmActivityReward> userGmActivityRewardInfo = playerBussiness.GetUserGmActivityRewardInfo(m_player.PlayerCharacter.ID);
			List<BaseUserGmActivity> list = GmActivities.Where((BaseUserGmActivity x) => x.ActivityInfo.endTime < DateTime.Now).ToList();
			foreach (BaseUserGmActivity item2 in list)
			{
				item2.RemoveTrigger(m_player);
				GmActivities.Remove(item2);
			}
			List<GmActivityInfo> list2 = GmActivityMgr.GmActivityInfos.FindAll((GmActivityInfo q) => q.beginTime < DateTime.Now && q.endTime > DateTime.Now);
			foreach (GmActivityInfo active in list2)
			{
				List<GmGiftInfo> list3 = GmActivityMgr.FindGmGifts(active.activityId);
				List<UserGmActivityCondition> list4 = new List<UserGmActivityCondition>();
				List<UserGmActivityReward> list5 = new List<UserGmActivityReward>();
				foreach (GmGiftInfo gift in list3)
				{
					List<UserGmActivityCondition> list6 = userGmActivityConditionInfo.FindAll((UserGmActivityCondition q) => q.GiftBagID == gift.giftbagId);
					int num = 0;
					if (list6.Count == 0)
					{
						List<GmActiveConditionInfo> list7 = GmActivityMgr.FindGmActiveCondition(gift.giftbagId);
						if (list7 != null && list7.Count > 0)
						{
							UserGmActivityCondition item = new UserGmActivityCondition
							{
								UserID = m_player.PlayerId,
								ActivityID = active.activityId,
								GiftBagID = gift.giftbagId,
								StatusID = ((active.activityType == 60) ? num++ : 0),
								StatusValue = 0
							};
							list4.Add(item);
							userGmActivityConditionInfo.Add(item);
						}
					}
					else
					{
						list4.AddRange(list6);
					}
					List<UserGmActivityReward> list8 = userGmActivityRewardInfo.FindAll((UserGmActivityReward q) => q.GiftBagID == gift.giftbagId);
					if (list8.Count == 0)
					{
						foreach (UserGmActivityReward item3 in from rewardInfo in GmActivityMgr.FindGmActiveReward(gift.giftbagId)
															   select new UserGmActivityReward
															   {
																   UserID = m_player.PlayerId,
																   ActivityID = active.activityId,
																   GiftBagID = rewardInfo.giftId,
																   Times = 0
															   })
						{
							list5.Add(item3);
							userGmActivityRewardInfo.Add(item3);
							if (active.activityType == 14)
							{
								int level = m_player.Pet.Level;

								List<GmActiveConditionInfo> list9 = GmActivityMgr.FindGmActiveCondition(gift.giftbagId);
								if (item3.GiftBagID == gift.giftbagId && list9[0].remain1 <= level)
								{
									item3.Times = 1;
								}
							}
							else if (active.activityType == 20)
							{
								int currentLevel = m_player.PlayerCharacter.CurrentLevel;
								List<GmActiveConditionInfo> list10 = GmActivityMgr.FindGmActiveCondition(gift.giftbagId);
								if (item3.GiftBagID == gift.giftbagId && list10[0].remain1 <= currentLevel)
								{
									item3.Times = 1;
								}
							}
						}
					}
					else
					{
						list5.AddRange(list8);
					}
				}
				if (!GmActivities.Any((BaseUserGmActivity x) => x.ActivityInfo.activityId == active.activityId))
				{
					BaseUserGmActivity baseUserGmActivity = BaseUserGmActivity.CreateGmActivity(m_player, active, list4, list5);
					baseUserGmActivity.AddTrigger(m_player);
					GmActivities.Add(baseUserGmActivity);
				}
			}
		}

		public virtual void SaveToDatabase()
		{
			if (!m_saveToDb)
			{
				return;
			}
			object @lock = m_lock;
			lock (@lock)
			{
				using PlayerBussiness playerBussiness = new PlayerBussiness();
				foreach (BaseUserGmActivity gmActivity in GmActivities)
				{
					foreach (UserGmActivityReward userReward in gmActivity.UserRewards)
					{
						if (userReward.IsDirty)
						{
							playerBussiness.UpdateUserGmActivityRewardInfo(userReward);
						}
					}
					foreach (UserGmActivityCondition userCondition in gmActivity.UserConditions)
					{
						if (userCondition.IsDirty)
						{
							playerBussiness.UpdateUserGmActivityConditionInfo(userCondition);
						}
					}
				}
			}
		}

		public BaseUserGmActivity FindUserGmActivity(string activeId)
		{
			return GmActivities.Find((BaseUserGmActivity x) => x.ActivityInfo.activityId == activeId);
		}
	}
}
