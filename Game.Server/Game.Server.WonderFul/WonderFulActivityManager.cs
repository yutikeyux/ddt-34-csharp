using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.GmActivity;
using Game.Server.Managers;
using Game.Server.Packets;
using log4net;
using SqlDataProvider.Data;

namespace Game.Server.WonderFul
{
	public class WonderFulActivityManager
	{
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private static readonly object Locker = new object();

		public static void WonderFulActivityInit(GamePlayer player, int type = 3)
		{
			GSPacketIn gSPacketIn = new GSPacketIn(405, player.PlayerCharacter.ID);
			gSPacketIn.WriteInt(type);
			if (type <= 0)
			{
				return;
			}
			try
			{
				List<GmActivityInfo> list = GmActivityMgr.GmActivityInfos.FindAll((GmActivityInfo q) => q.beginTime < DateTime.Now && q.endTime > DateTime.Now);
				gSPacketIn.WriteInt(list.Count);
				foreach (GmActivityInfo active in list)
				{
					gSPacketIn.WriteString(active.activityId);
					BaseUserGmActivity baseUserGmActivity = player.GmActivity.FindUserGmActivity(active.activityId); //dbd
					List<GmGiftInfo> list2 = (from x in GmActivityMgr.FindGmGifts(active.activityId)
						orderby x.giftbagOrder
						select x).ToList();
					List<UserGmActivityCondition> list3 = new List<UserGmActivityCondition>();
					List<UserGmActivityReward> list4 = new List<UserGmActivityReward>();
					foreach (GmGiftInfo gift in list2)
					{
						List<UserGmActivityCondition> list5 = baseUserGmActivity.UserConditions.FindAll((UserGmActivityCondition q) => q.GiftBagID == gift.giftbagId);
						int day = 0;
						if (list5.Count == 0)
						{
							foreach (UserGmActivityCondition item in from conditionInfo in GmActivityMgr.FindGmActiveCondition(gift.giftbagId)
								select new UserGmActivityCondition
								{
									UserID = player.PlayerId,
									ActivityID = active.activityId,
									GiftBagID = conditionInfo.giftbagId,
									StatusID = ((active.activityType == 60) ? day++ : 0),
									StatusValue = 0
								})
							{
								list3.Add(item);
								baseUserGmActivity.UserConditions.Add(item);
							}
						}
						else
						{
							list3.AddRange(list5);
						}
						List<UserGmActivityReward> list6 = baseUserGmActivity.UserRewards.FindAll((UserGmActivityReward q) => q.GiftBagID == gift.giftbagId);
						if (list6.Count == 0)
						{
							foreach (UserGmActivityReward item2 in from rewardInfo in GmActivityMgr.FindGmActiveReward(gift.giftbagId)
								select new UserGmActivityReward
								{
									UserID = player.PlayerId,
									ActivityID = active.activityId,
									GiftBagID = rewardInfo.giftId,
									Times = 0
								})
							{
								list4.Add(item2);
								baseUserGmActivity.UserRewards.Add(item2);
								if (active.activityType == 14)
								{
									int level = player.Pet.Level;
									List<GmActiveConditionInfo> list7 = GmActivityMgr.FindGmActiveCondition(gift.giftbagId);
									if (item2.GiftBagID == gift.giftbagId && list7[0].remain1 <= level)
									{
										item2.Times = 1;
									}
								}
								else if (active.activityType == 20)
								{
									int currentLevel = player.PlayerCharacter.CurrentLevel;
									List<GmActiveConditionInfo> list8 = GmActivityMgr.FindGmActiveCondition(gift.giftbagId);
									if (item2.GiftBagID == gift.giftbagId && list8[0].remain1 <= currentLevel)
									{
										item2.Times = 1;
									}
								}
							}
						}
						else
						{
							list4.AddRange(list6);
						}
					}
					gSPacketIn.WriteInt(list3.Count);
					list3 = list3.OrderBy((UserGmActivityCondition x) => GmActivityMgr.FindGmGift(x.GiftBagID).giftbagOrder).ToList();
					foreach (UserGmActivityCondition item3 in list3)
					{
						int val = item3.StatusValue;
						if (active.activityType == 0 && active.activityChildType == 2 && item3.StatusID != 0)
						{
							val = item3.StatusValue / item3.StatusID;
						}
						gSPacketIn.WriteInt(item3.StatusID);
						gSPacketIn.WriteInt(val);
					}
					gSPacketIn.WriteInt(list4.Count);
					foreach (UserGmActivityReward item4 in list4)
					{
						gSPacketIn.WriteString(item4.GiftBagID);
						gSPacketIn.WriteInt(item4.Times);
						gSPacketIn.WriteInt(GmActivityMgr.FindGmActiveReward(item4.GiftBagID)[0].allGiftGetTimes);
					}
				}
				player.SendTCP(gSPacketIn);
			}
			catch (Exception exception)
			{
				if (Log.IsErrorEnabled)
				{
					Log.Error("WonderFulActivityInit error:", exception);
				}
			}
		}

		public static void SendWonderFulReward(GamePlayer player, GSPacketIn packet)
		{
			try
			{
				lock (Locker)
				{
					for (int i = 0; i < packet.ReadInt(); i++)
					{
						string activityId = packet.ReadString();
						int num = packet.ReadInt();
						GmActivityInfo gmActivityInfo = GmActivityMgr.FindGmActivity(activityId);
						BaseUserGmActivity baseUserGmActivity = player.GmActivity.FindUserGmActivity(activityId);
						if (baseUserGmActivity == null)
						{
							player.SendMessage("Etkinliğe dair oyuncu bilgileriniz yüklenemedi. Lütfen sorunu yöneticiye bildirin.");
							break;
						}
						for (int j = 0; j < packet.ReadInt(); j++)
						{
							string[] array = packet.ReadString().Split(',');
							string giftId = array[0];
							int num2 = packet.ReadInt();
							List<ItemInfo> list = new List<ItemInfo>();
							if (gmActivityInfo == null || gmActivityInfo.beginTime > DateTime.Now || gmActivityInfo.endTime < DateTime.Now)
							{
								continue;
							}
							List<GmGiftInfo> list2 = GmActivityMgr.FindGmGifts(activityId);
							if (list2.Count == 0)
							{
								continue;
							}
							List<GmActiveRewardInfo> list3 = GmActivityMgr.FindGmActiveReward(giftId);
							if (list3.Count == 0)
							{
								continue;
							}
							List<GmActiveConditionInfo> list4 = GmActivityMgr.FindGmActiveCondition(giftId);
							UserGmActivityReward userGmActivityReward = baseUserGmActivity.UserRewards.Find((UserGmActivityReward a) => a.ActivityID == activityId && a.GiftBagID == giftId);
							if (userGmActivityReward == null)
							{
								continue;
							}
							int activityType = gmActivityInfo.activityType;
							int num3 = activityType;
							if (num3 == 19)
							{
								bool flag = int.Parse(array[1]) != -8;
								GmActiveConditionInfo gmActiveConditionInfo = list4.Find((GmActiveConditionInfo c) => c.conditionIndex == 1);
								GmActiveConditionInfo gmActiveConditionInfo2 = list4.Find((GmActiveConditionInfo c) => c.conditionIndex == 2);
								GmActiveConditionInfo gmActiveConditionInfo3 = list4.Find((GmActiveConditionInfo c) => c.conditionIndex == 3);
								GmActiveConditionInfo gmActiveConditionInfo4 = list4.Find((GmActiveConditionInfo c) => c.conditionIndex == 4);
								GmActiveConditionInfo gmActiveConditionInfo5 = list4.Find((GmActiveConditionInfo c) => c.conditionIndex == 5);
								GmActiveConditionInfo gmActiveConditionInfo6 = list4.Find((GmActiveConditionInfo c) => c.conditionIndex == 100);
								int num4 = gmActiveConditionInfo3.conditionValue * num;
								GmActiveRewardInfo gmActiveRewardInfo = list3[0];
								if (flag && gmActiveConditionInfo2.conditionValue == -9)
								{
									if (player.PlayerCharacter.GiftToken < num4)
									{
										player.SendMessage("Bağlı kuponunuz yetersiz!");
										continue;
									}
								}
								else if (player.PlayerCharacter.Money < num4)
								{
									player.SendMessage("Kuponunuz yetersiz!");
									continue;
								}
								if (gmActivityInfo.activityChildType == 2)
								{
									if (gmActiveConditionInfo5.conditionValue > 0)
									{
										if (player.PlayerCharacter.typeVIP == 0)
										{
											player.SendMessage("VIP değilsiniz, bu ürünü alamazsınız!");
											continue;
										}
										if (player.PlayerCharacter.VIPLevel < gmActiveConditionInfo5.conditionValue)
										{
											player.SendMessage("Bu ürünü alabilmek için en az VIP " + gmActiveConditionInfo5.conditionValue + " olmalısınız!");
											continue;
										}
									}
								}
								else if (gmActivityInfo.activityChildType == 3)
								{
									if (userGmActivityReward.Times >= gmActiveConditionInfo.conditionValue)
									{
										player.SendMessage(LanguageMgr.GetTranslation("Alım sınırınıza ulaştınız!"));
										continue;
									}
									if (userGmActivityReward.Times + num > gmActiveConditionInfo.conditionValue)
									{
										num = gmActiveConditionInfo.conditionValue - userGmActivityReward.Times;
										num4 = gmActiveConditionInfo3.conditionValue * num;
										if (flag && gmActiveConditionInfo2.conditionValue == -9)
										{
											if (player.PlayerCharacter.GiftToken < num4)
											{
												player.SendMessage("Bağlı kuponunuz yetersiz!");
												continue;
											}
										}
										else if (player.PlayerCharacter.Money < num4)
										{
											player.SendMessage("Kuponunuz yetersiz!");
											continue;
										}
									}
								}
								else if (gmActivityInfo.activityChildType == 5)
								{
									if (gmActiveRewardInfo.allGiftGetTimes == gmActiveConditionInfo6.conditionValue)
									{
										player.SendMessage("Sunucu limitine ulaşıldı!");
										continue;
									}
									if (gmActiveRewardInfo.allGiftGetTimes + num > gmActiveConditionInfo6.conditionValue)
									{
										num = gmActiveConditionInfo6.conditionValue - gmActiveRewardInfo.allGiftGetTimes;
										num4 = gmActiveConditionInfo3.conditionValue * num;
										if (flag && gmActiveConditionInfo2.conditionValue == -9)
										{
											if (player.PlayerCharacter.GiftToken < num4)
											{
												player.SendMessage("Bağlı kuponunuz yetersiz!");
												continue;
											}
										}
										else if (player.PlayerCharacter.Money < num4)
										{
											player.SendMessage("Kuponunuz yetersiz!");
											continue;
										}
									}
								}
								for (int k = 0; k < num; k++)
								{
									ItemInfo ıtemInfo = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(gmActiveRewardInfo.templateId), 1, 102);
									if (ıtemInfo == null)
									{
										break;
									}
									ıtemInfo.IsBinds = gmActiveRewardInfo.isBind == 1;
									ıtemInfo.Count = ((ItemMgr.FindItemTemplate(gmActiveRewardInfo.templateId).MaxCount == 1) ? gmActiveRewardInfo.count : (gmActiveRewardInfo.count * num));
									ıtemInfo.ValidDate = gmActiveRewardInfo.validDate;
									string[] array2 = gmActiveRewardInfo.property.Split(',');
									ıtemInfo.StrengthenLevel = int.Parse(array2[0]);
									ıtemInfo.AttackCompose = int.Parse(array2[1]);
									ıtemInfo.DefendCompose = int.Parse(array2[2]);
									ıtemInfo.AgilityCompose = int.Parse(array2[3]);
									ıtemInfo.LuckCompose = int.Parse(array2[4]);
									list.Add(ıtemInfo);
									if (ItemMgr.FindItemTemplate(gmActiveRewardInfo.templateId).MaxCount > 1)
									{
										break;
									}
								}
								if (flag && gmActiveConditionInfo2.conditionValue == -9)
								{
									if (!player.GiftTokenDirect(gmActiveConditionInfo3.conditionValue))
									{
										return;
									}
									if (!player.SendItemsToMail(list, LanguageMgr.GetTranslation("Etkinliğe katılarak aşağıdaki ödülleri kazandınız!"), "Etkinlik ödülü", eMailType.Manage))
									{
										player.AddGiftToken(gmActiveConditionInfo3.conditionValue);
										continue;
									}
									player.Out.SendMailResponse(player.PlayerCharacter.ID, eMailRespose.Receiver);
									if (gmActivityInfo.activityChildType == 3)
									{
										userGmActivityReward.Times += num;
									}
									else if (gmActivityInfo.activityChildType == 5)
									{
										gmActiveRewardInfo.allGiftGetTimes += num;
									}
									player.SendMessage("Ödül alımı başarılı!");
									WonderFulActivityInit(player);
									return;
								}
								if (!player.MoneyDirect(gmActiveConditionInfo3.conditionValue))
								{
									return;
								}
								if (!player.SendItemsToMail(list, LanguageMgr.GetTranslation("Etkinliğe katılarak aşağıdaki ödülleri kazandınız!"), "Etkinlik ödülü", eMailType.Manage))
								{
									player.AddMoney(gmActiveConditionInfo3.conditionValue);
									continue;
								}
								player.Out.SendMailResponse(player.PlayerCharacter.ID, eMailRespose.Receiver);
								if (gmActivityInfo.activityChildType == 3)
								{
									userGmActivityReward.Times += num;
								}
								else if (gmActivityInfo.activityChildType == 5)
								{
									gmActiveRewardInfo.allGiftGetTimes += num;
								}
								player.SendMessage("Ödül alımı başarılı!");
								RefreshSingleActivity(player, activityId);
								return;
							}
							int times = num;
							string text = baseUserGmActivity.CanGetRewardMsg(giftId, ref times);
							if (text != "ok")
							{
								player.SendMessage(text);
								continue;
							}
							List<ItemInfo> items = baseUserGmActivity.RewardList(giftId, times);
							if (!player.SendItemsToMail(items, gmActivityInfo.activityName + " etkinliğine katılarak aşağıdaki ödülleri kazandınız!", "Etkinlik ödülü", eMailType.Manage))
							{
								player.SendMessage("Mail alınırken bir hata oluştu");
								continue;
							}
							player.Out.SendMailResponse(player.PlayerCharacter.ID, eMailRespose.Receiver);
							player.SendMessage("Ödül alımı başarılı!");
							if (gmActivityInfo.activityType == 8)
							{
								baseUserGmActivity.UserConditions.Find((UserGmActivityCondition a) => a.GiftBagID == giftId).StatusValue = 0;
							}
							else
							{
								userGmActivityReward.Times += times;
							}
							RefreshSingleActivity(player, activityId);
						}
					}
				}
			}
			catch (Exception exception)
			{
				if (Log.IsErrorEnabled)
				{
					Log.Error("SendWonderFulReward error:", exception);
				}
			}
		}

		public static void RefreshSingleActivity(GamePlayer player, string activityId)
		{
			GSPacketIn gSPacketIn = new GSPacketIn(405, player.PlayerCharacter.ID);
			gSPacketIn.WriteInt(3);
			try
			{
				GmActivityInfo active = GmActivityMgr.GmActivityInfos.Find((GmActivityInfo q) => q.activityId == activityId && q.beginTime < DateTime.Now && q.endTime > DateTime.Now);
				if (active != null)
				{
					gSPacketIn.WriteInt(1);
					gSPacketIn.WriteString(active.activityId);
					BaseUserGmActivity baseUserGmActivity = player.GmActivity.FindUserGmActivity(active.activityId);
					List<GmGiftInfo> list = (from x in GmActivityMgr.FindGmGifts(active.activityId)
						orderby x.giftbagOrder
						select x).ToList();
					List<UserGmActivityCondition> list2 = new List<UserGmActivityCondition>();
					List<UserGmActivityReward> list3 = new List<UserGmActivityReward>();
					foreach (GmGiftInfo gift in list)
					{
						List<UserGmActivityCondition> list4 = baseUserGmActivity.UserConditions.FindAll((UserGmActivityCondition q) => q.GiftBagID == gift.giftbagId);
						int day = 0;
						if (list4.Count == 0)
						{
							foreach (UserGmActivityCondition item in from conditionInfo in GmActivityMgr.FindGmActiveCondition(gift.giftbagId)
								select new UserGmActivityCondition
								{
									UserID = player.PlayerId,
									ActivityID = active.activityId,
									GiftBagID = conditionInfo.giftbagId,
									StatusID = ((active.activityType == 60) ? day++ : 0),
									StatusValue = 0
								})
							{
								list2.Add(item);
								baseUserGmActivity.UserConditions.Add(item);
							}
						}
						else
						{
							list2.AddRange(list4);
						}
						List<UserGmActivityReward> list5 = baseUserGmActivity.UserRewards.FindAll((UserGmActivityReward q) => q.GiftBagID == gift.giftbagId);
						if (list5.Count == 0)
						{
							foreach (UserGmActivityReward item2 in from rewardInfo in GmActivityMgr.FindGmActiveReward(gift.giftbagId)
								select new UserGmActivityReward
								{
									UserID = player.PlayerId,
									ActivityID = active.activityId,
									GiftBagID = rewardInfo.giftId,
									Times = 0
								})
							{
								list3.Add(item2);
								baseUserGmActivity.UserRewards.Add(item2);
							}
						}
						else
						{
							list3.AddRange(list5);
						}
					}
					gSPacketIn.WriteInt(list2.Count);
					list2 = list2.OrderBy((UserGmActivityCondition x) => GmActivityMgr.FindGmGift(x.GiftBagID).giftbagOrder).ToList();
					foreach (UserGmActivityCondition item3 in list2)
					{
						int val = item3.StatusValue;
						if (active.activityType == 0 && active.activityChildType == 2 && item3.StatusID != 0)
						{
							val = item3.StatusValue / item3.StatusID;
						}
						gSPacketIn.WriteInt(item3.StatusID);
						gSPacketIn.WriteInt(val);
					}
					gSPacketIn.WriteInt(list3.Count);
					foreach (UserGmActivityReward item4 in list3)
					{
						gSPacketIn.WriteString(item4.GiftBagID);
						gSPacketIn.WriteInt(item4.Times);
						gSPacketIn.WriteInt(GmActivityMgr.FindGmActiveReward(item4.GiftBagID)[0].allGiftGetTimes);
					}
				}
				else
				{
					gSPacketIn.WriteInt(0);
				}
				player.SendTCP(gSPacketIn);
			}
			catch (Exception exception)
			{
				if (Log.IsErrorEnabled)
				{
					Log.Error("WonderFulActivity RefreshSingleActivity error:", exception);
				}
			}
		}

		public static List<ItemInfo> FoodActivityGetRewards(int cookingScore)
		{
			if (GmActivityMgr.FoodActivity == null)
			{
				return null;
			}
			cookingScore += 60;
			List<ItemInfo> list = new List<ItemInfo>();
			foreach (GmGiftInfo item in GmActivityMgr.FindGmGifts(GmActivityMgr.FoodActivity.activityId))
			{
				List<GmActiveConditionInfo> source = GmActivityMgr.FindGmActiveCondition(item.giftbagId);
				int conditionValue = source.First((GmActiveConditionInfo x) => x.conditionIndex == 0).conditionValue;
				int conditionValue2 = source.First((GmActiveConditionInfo x) => x.conditionIndex == 1).conditionValue;
				if (cookingScore < conditionValue || cookingScore > conditionValue2)
				{
					continue;
				}
				List<GmActiveRewardInfo> list2 = GmActivityMgr.FindGmActiveReward(item.giftbagId);
				foreach (GmActiveRewardInfo item2 in list2)
				{
					ItemInfo ıtemInfo = ItemMgr.CreateInfoFromGmReward(item2);
					if (ıtemInfo != null)
					{
						list.Add(ıtemInfo);
					}
				}
				return list;
			}
			return null;
		}

		public static List<ItemInfo> FlowerGivingRankRewards(int RewardMark, int Rank, int RewardType)
		{
			if (GmActivityMgr.FlowerGivingActivity == null)
			{
				return null;
			}
			List<ItemInfo> list = new List<ItemInfo>();
			foreach (GmGiftInfo item in GmActivityMgr.FindGmGifts(GmActivityMgr.FlowerGivingActivity.activityId))
			{
				if (item.rewardMark != RewardMark)
				{
					continue;
				}
				List<GmActiveConditionInfo> source = GmActivityMgr.FindGmActiveCondition(item.giftbagId);
				if (!source.Any((GmActiveConditionInfo x) => x.conditionIndex == RewardType && x.conditionValue == Rank))
				{
					continue;
				}
				foreach (GmActiveRewardInfo item2 in GmActivityMgr.FindGmActiveReward(item.giftbagId))
				{
					if (item2.rewardType == RewardType)
					{
						ItemInfo ıtemInfo = ItemMgr.CreateInfoFromGmReward(item2);
						if (ıtemInfo != null)
						{
							list.Add(ıtemInfo);
						}
					}
				}
				break;
			}
			return list;
		}

		public static List<ItemInfo> FlowerGivingAccuGivingRewards(int index, string giftbagId = null)
		{
			if (GmActivityMgr.FlowerGivingActivity == null)
			{
				return null;
			}
			List<ItemInfo> list = new List<ItemInfo>();
			if (giftbagId == null)
			{
				GmGiftInfo gmGiftInfo = GmActivityMgr.FindGmGifts(GmActivityMgr.FlowerGivingActivity.activityId).First((GmGiftInfo x) => x.rewardMark == 6 && x.giftbagOrder == index);
				giftbagId = gmGiftInfo.giftbagId;
			}
			if (!string.IsNullOrEmpty(giftbagId))
			{
				foreach (GmActiveRewardInfo item in GmActivityMgr.FindGmActiveReward(giftbagId))
				{
					ItemInfo ıtemInfo = ItemMgr.CreateInfoFromGmReward(item);
					if (ıtemInfo != null)
					{
						list.Add(ıtemInfo);
					}
				}
			}
			return list;
		}

		public static List<ItemInfo> FlowerGivingSendFlowerRewards(int flowerCount)
		{
			if (GmActivityMgr.FlowerGivingActivity == null)
			{
				return null;
			}
			List<ItemInfo> list = new List<ItemInfo>();
			List<GmGiftInfo> list2 = (from x in GmActivityMgr.FindGmGifts(GmActivityMgr.FlowerGivingActivity.activityId)
				where x.rewardMark == 5
				select x).ToList();
			foreach (GmGiftInfo item in list2)
			{
				GmActiveConditionInfo gmActiveConditionInfo = GmActivityMgr.FindGmActiveCondition(item.giftbagId).First((GmActiveConditionInfo x) => x.conditionIndex == 0);
				if (gmActiveConditionInfo == null || gmActiveConditionInfo.conditionValue != flowerCount)
				{
					continue;
				}
				foreach (GmActiveRewardInfo item2 in GmActivityMgr.FindGmActiveReward(item.giftbagId))
				{
					ItemInfo ıtemInfo = ItemMgr.CreateInfoFromGmReward(item2);
					if (ıtemInfo != null)
					{
						list.Add(ıtemInfo);
					}
				}
				break;
			}
			return list;
		}
	}
}
