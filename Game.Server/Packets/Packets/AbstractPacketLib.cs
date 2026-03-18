using Bussiness;
using Bussiness.Managers;
using Game.Server;
using Game.Server.Buffer;
using Game.Server.EliteGame;
using Game.Server.GameUtils;
using Game.Server.LittleGame;
using Game.Server.Managers;
using Game.Server.Packets;
using Game.Server.Quests;
using Game.Server.Rooms;
using Game.Server.SceneMarryRooms;
using log4net;
using Newtonsoft.Json;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game.Base.Packets
{
    [PacketLib(1)]
    public class AbstractPacketLib : IPacketLib
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected readonly GameClient m_gameClient;

        public AbstractPacketLib(GameClient client)
        {
            m_gameClient = client;
        }

        public void SendOpenFoodActive(GmActivityInfo info)
        {

        }
        public void SendOpenGodsRoad()
        {
            GSPacketIn pkg = new GSPacketIn(86);
            pkg.WriteByte(86);
            SendTCP(pkg);
        }

        public void SendCatchBeastOpen(int playerID, bool isOpen)
        {
            GSPacketIn pkg = new GSPacketIn(145, playerID);
            pkg.WriteByte(32);
            pkg.WriteBoolean(isOpen);
            SendTCP(pkg);
        }
        public void SendUpdateChickActivation(UserChickActiveInfo chickInfo)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(84);
            gSPacketIn.WriteInt(2);
            gSPacketIn.WriteInt(2);
            gSPacketIn.WriteInt(chickInfo.IsKeyOpened);
            gSPacketIn.WriteInt(1);
            gSPacketIn.WriteDateTime(chickInfo.KeyOpenedTime);
            gSPacketIn.WriteInt(chickInfo.KeyOpenedType);
            gSPacketIn.WriteInt((chickInfo.EveryDay.Day < DateTime.Now.Day && DateTime.Now.DayOfWeek == DayOfWeek.Monday) ? 0 : 1);
            gSPacketIn.WriteInt((chickInfo.EveryDay.Day < DateTime.Now.Day && DateTime.Now.DayOfWeek == DayOfWeek.Tuesday) ? 0 : 1);
            gSPacketIn.WriteInt((chickInfo.EveryDay.Day < DateTime.Now.Day && DateTime.Now.DayOfWeek == DayOfWeek.Wednesday) ? 0 : 1);
            gSPacketIn.WriteInt((chickInfo.EveryDay.Day < DateTime.Now.Day && DateTime.Now.DayOfWeek == DayOfWeek.Thursday) ? 0 : 1);
            gSPacketIn.WriteInt((chickInfo.EveryDay.Day < DateTime.Now.Day && DateTime.Now.DayOfWeek == DayOfWeek.Friday) ? 0 : 1);
            gSPacketIn.WriteInt((chickInfo.EveryDay.Day < DateTime.Now.Day && DateTime.Now.DayOfWeek == DayOfWeek.Saturday) ? 0 : 1);
            gSPacketIn.WriteInt((chickInfo.EveryDay.Day < DateTime.Now.Day && DateTime.Now.DayOfWeek == DayOfWeek.Sunday) ? 0 : 1);
            gSPacketIn.WriteInt((chickInfo.AfterThreeDays.Day < DateTime.Now.Day && chickInfo.OnThreeDay(DateTime.Now)) ? 0 : 1);
            gSPacketIn.WriteInt((chickInfo.AfterThreeDays.Day < DateTime.Now.Day && chickInfo.OnThreeDay(DateTime.Now)) ? 0 : 1);
            gSPacketIn.WriteInt((chickInfo.AfterThreeDays.Day < DateTime.Now.Day && chickInfo.OnThreeDay(DateTime.Now)) ? 0 : 1);
            gSPacketIn.WriteInt((chickInfo.Weekly < chickInfo.StartOfWeek(DateTime.Now, DayOfWeek.Saturday) && DateTime.Now.DayOfWeek == DayOfWeek.Saturday) ? 0 : 1);
            gSPacketIn.WriteInt(chickInfo.CurrentLvAward);
            this.SendTCP(gSPacketIn);
        }

        public void SendOpenHappyRecharge(int playerID)
        {
            GSPacketIn pkg = new GSPacketIn(145, playerID);
            pkg.WriteByte(178);
            pkg.WriteBoolean(GameProperties.HappyRechargeOpenClose);
            SendTCP(pkg);
        }
        public static IPacketLib CreatePacketLibForVersion(int rawVersion, GameClient client)
        {
            Type[] derivedClasses = ScriptMgr.GetDerivedClasses(typeof(IPacketLib));
            Type[] array = derivedClasses;
            foreach (Type type in array)
            {
                object[] customAttributes = type.GetCustomAttributes(typeof(PacketLibAttribute), inherit: false);
                for (int j = 0; j < customAttributes.Length; j++)
                {
                    if (((PacketLibAttribute)customAttributes[j]).RawVersion != rawVersion)
                    {
                        continue;
                    }
                    try
                    {
                        return (IPacketLib)Activator.CreateInstance(type, client);
                    }
                    catch (Exception exception)
                    {
                        if (log.IsErrorEnabled)
                        {
                            log.Error("error creating packetlib (" + type.FullName + ") for raw version " + rawVersion, exception);
                        }
                    }
                }
            }
            return null;
        }

        public void SendTCP(GSPacketIn packet)
        {
            m_gameClient.SendTCP(packet);
        }

        public void SendAcademyGradute(GamePlayer app, int type)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(141);
            gSPacketIn.WriteByte(11);
            gSPacketIn.WriteInt(type);
            gSPacketIn.WriteInt(app.PlayerId);
            gSPacketIn.WriteString(app.PlayerCharacter.NickName);
            SendTCP(gSPacketIn);
        }

        public GSPacketIn SendAcademySystemNotice(string text, bool isAlert)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(141);
            gSPacketIn.WriteByte(17);
            gSPacketIn.WriteString(text);
            gSPacketIn.WriteBoolean(isAlert);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendAcademyAppState(PlayerInfo player, int removeUserId)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(141);
            gSPacketIn.WriteByte(10);
            gSPacketIn.WriteInt(player.apprenticeshipState);
            gSPacketIn.WriteInt(player.masterID);
            gSPacketIn.WriteString(player.masterOrApprentices);
            gSPacketIn.WriteInt(removeUserId);
            gSPacketIn.WriteInt(player.graduatesCount);
            gSPacketIn.WriteString(player.honourOfMaster);
            gSPacketIn.WriteDateTime(player.freezesDate);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        //     public GSPacketIn SendConsortiaTaskInfo(BaseConsortiaTask baseTask)
        //     {
        //GSPacketIn gSPacketIn = new GSPacketIn(129);
        //gSPacketIn.WriteByte(22);
        //gSPacketIn.WriteByte(3);
        //if (baseTask != null)
        //{
        //	gSPacketIn.WriteInt(baseTask.ConditionList.Count);
        //	foreach (KeyValuePair<int, ConsortiaTaskInfo> condition in baseTask.ConditionList)
        //	{
        //		gSPacketIn.WriteInt(condition.Key);
        //		gSPacketIn.WriteInt(3);
        //		gSPacketIn.WriteString(condition.Value.CondictionTitle);
        //		gSPacketIn.WriteInt(baseTask.GetTotalValueByConditionPlace(condition.Key));
        //		gSPacketIn.WriteInt(condition.Value.Para2);
        //		gSPacketIn.WriteInt(baseTask.GetValueByConditionPlace(m_gameClient.Player.PlayerCharacter.ID, condition.Key));
        //	}
        //	gSPacketIn.WriteInt(baseTask.Info.TotalExp);
        //	gSPacketIn.WriteInt(baseTask.Info.TotalOffer);
        //	gSPacketIn.WriteInt(baseTask.Info.TotalRiches);
        //	gSPacketIn.WriteInt(baseTask.Info.BuffID);
        //	gSPacketIn.WriteDateTime(baseTask.Info.StartTime);
        //	gSPacketIn.WriteInt(baseTask.Info.VaildDate);
        //}
        //else
        //{
        //	gSPacketIn.WriteInt(0);
        //	gSPacketIn.WriteInt(0);
        //	gSPacketIn.WriteInt(0);
        //	gSPacketIn.WriteInt(0);
        //	gSPacketIn.WriteInt(0);
        //	gSPacketIn.WriteDateTime(DateTime.Now);
        //	gSPacketIn.WriteInt(0);
        //}
        //SendTCP(gSPacketIn);
        //return gSPacketIn;
        //     }

        public GSPacketIn SendSystemConsortiaChat(string content, bool sendToSelf)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(129);
            gSPacketIn.WriteByte(20);
            gSPacketIn.WriteByte(0);
            gSPacketIn.WriteString("");
            gSPacketIn.WriteString(content);
            if (sendToSelf)
            {
                SendTCP(gSPacketIn);
            }
            return gSPacketIn;
        }

        public void SendShopGoodsCountUpdate(List<ShopFreeCountInfo> list)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(168);
            gSPacketIn.WriteInt(list.Count);
            foreach (ShopFreeCountInfo item in list)
            {
                gSPacketIn.WriteInt(item.ShopID);
                gSPacketIn.WriteInt(item.Count);
            }
            gSPacketIn.WriteInt(0);
            gSPacketIn.WriteInt(0);
            gSPacketIn.WriteInt(0);
            SendTCP(gSPacketIn);
        }

        public void SendEliteGameStartRoom()
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.ELITEGAME);
            pkg.WriteByte((byte)EliteGamePackageType.ELITE_MATCH_RANK_START);
            SendTCP(pkg);
        }

        public void SendEliteGameInfo(int type)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(162);
            gSPacketIn.WriteByte(1);
            gSPacketIn.WriteInt(type);
            SendTCP(gSPacketIn);
        }

        public GSPacketIn SendLabyrinthUpdataInfo(int ID, UserLabyrinthInfo laby)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(131, ID);
            gSPacketIn.WriteByte(2);
            gSPacketIn.WriteInt(laby.myProgress);
            gSPacketIn.WriteInt(laby.currentFloor);
            gSPacketIn.WriteBoolean(laby.completeChallenge);
            gSPacketIn.WriteInt(laby.remainTime);
            gSPacketIn.WriteInt(laby.accumulateExp);
            gSPacketIn.WriteInt(laby.cleanOutAllTime);
            gSPacketIn.WriteInt(laby.cleanOutGold);
            gSPacketIn.WriteInt(laby.myRanking);
            gSPacketIn.WriteBoolean(laby.isDoubleAward);
            gSPacketIn.WriteBoolean(laby.isInGame);
            gSPacketIn.WriteBoolean(laby.isCleanOut);
            gSPacketIn.WriteBoolean(laby.serverMultiplyingPower);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendUpdateUserPet(PetInventory bag, int[] slots)
        {
            if (m_gameClient.Player == null)
            {
                return null;
            }
            GSPacketIn gSPacketIn = new GSPacketIn(68, m_gameClient.Player.PlayerId);
            gSPacketIn.WriteByte(1);
            gSPacketIn.WriteInt(m_gameClient.Player.PlayerId);
            gSPacketIn.WriteInt(m_gameClient.Player.ZoneId);
            gSPacketIn.WriteInt(slots.Length);
            foreach (int num in slots)
            {
                gSPacketIn.WriteInt(num);
                UsersPetInfo petAt = bag.GetPetAt(num);
                if (petAt == null)
                {
                    gSPacketIn.WriteBoolean(val: false);
                    continue;
                }
                gSPacketIn.WriteBoolean(true);
                gSPacketIn.WriteInt(petAt.ID);
                gSPacketIn.WriteInt(petAt.TemplateID);
                gSPacketIn.WriteString(petAt.Name);
                gSPacketIn.WriteInt(petAt.UserID);
                gSPacketIn.WriteInt(petAt.Attack - petAt.ReduceProp(petAt.Attack));
                gSPacketIn.WriteInt(petAt.Defence - petAt.ReduceProp(petAt.Defence));
                gSPacketIn.WriteInt(petAt.Luck - petAt.ReduceProp(petAt.Luck));
                gSPacketIn.WriteInt(petAt.Agility - petAt.ReduceProp(petAt.Agility));
                gSPacketIn.WriteInt(petAt.Blood - petAt.ReduceProp(petAt.Blood));
                gSPacketIn.WriteInt(petAt.Damage);
                gSPacketIn.WriteInt(petAt.Guard);
                gSPacketIn.WriteInt(petAt.AttackGrow);
                gSPacketIn.WriteInt(petAt.DefenceGrow);
                gSPacketIn.WriteInt(petAt.LuckGrow);
                gSPacketIn.WriteInt(petAt.AgilityGrow);
                gSPacketIn.WriteInt(petAt.BloodGrow);
                gSPacketIn.WriteInt(petAt.DamageGrow);
                gSPacketIn.WriteInt(petAt.GuardGrow);
                gSPacketIn.WriteInt(petAt.Level);
                gSPacketIn.WriteInt(petAt.GP);
                gSPacketIn.WriteInt(petAt.MaxGP);
                gSPacketIn.WriteInt(petAt.Hunger);
                gSPacketIn.WriteInt(petAt.PetHappyStar);
                gSPacketIn.WriteInt(petAt.MP);
                string[] array = petAt.Skill.Split('|');
                gSPacketIn.WriteInt(array.Length);
                string[] array2 = array;
                foreach (string text in array2)
                {
                    gSPacketIn.WriteInt(int.Parse(text.Split(',')[0]));
                    gSPacketIn.WriteInt(int.Parse(text.Split(',')[1]));
                }
                string[] array3 = petAt.SkillEquip.Split('|');
                gSPacketIn.WriteInt(array3.Length);
                string[] array4 = array3;
                foreach (string text2 in array4)
                {
                    gSPacketIn.WriteInt(int.Parse(text2.Split(',')[1]));
                    gSPacketIn.WriteInt(int.Parse(text2.Split(',')[0]));
                }
                gSPacketIn.WriteBoolean(petAt.IsEquip);
                gSPacketIn.WriteInt(petAt.PetEquips.Count);
                foreach (PetEquipInfo petEquip in petAt.PetEquips)
                {
                    gSPacketIn.WriteInt(petEquip.eqType);
                    gSPacketIn.WriteInt(petEquip.eqTemplateID);
                    gSPacketIn.WriteDateTime(petEquip.startTime);
                    gSPacketIn.WriteInt(petEquip.ValidDate);
                }
                gSPacketIn.WriteInt(petAt.currentStarExp);
            }
            gSPacketIn.WriteInt(m_gameClient.Player.PetBag.EatPets.weaponLevel);
            gSPacketIn.WriteInt(m_gameClient.Player.PetBag.EatPets.clothesLevel);
            gSPacketIn.WriteInt(m_gameClient.Player.PetBag.EatPets.hatLevel);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendPetInfo(int id, int zoneId, UsersPetInfo[] pets, EatPetsInfo eatpet)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(68, id);
            gSPacketIn.WriteByte(1);
            gSPacketIn.WriteInt(id);
            gSPacketIn.WriteInt(zoneId);
            gSPacketIn.WriteInt(pets.Length);
            foreach (UsersPetInfo usersPetInfo in pets)
            {
                gSPacketIn.WriteInt(usersPetInfo.Place);
                gSPacketIn.WriteBoolean(val: true);
                gSPacketIn.WriteInt(usersPetInfo.ID);
                gSPacketIn.WriteInt(usersPetInfo.TemplateID);
                gSPacketIn.WriteString(usersPetInfo.Name);
                gSPacketIn.WriteInt(usersPetInfo.UserID);
                gSPacketIn.WriteInt(usersPetInfo.Attack);
                gSPacketIn.WriteInt(usersPetInfo.Defence);
                gSPacketIn.WriteInt(usersPetInfo.Luck);
                gSPacketIn.WriteInt(usersPetInfo.Agility);
                gSPacketIn.WriteInt(usersPetInfo.Blood);
                gSPacketIn.WriteInt(usersPetInfo.Damage);
                gSPacketIn.WriteInt(usersPetInfo.Guard);
                gSPacketIn.WriteInt(usersPetInfo.AttackGrow);
                gSPacketIn.WriteInt(usersPetInfo.DefenceGrow);
                gSPacketIn.WriteInt(usersPetInfo.LuckGrow);
                gSPacketIn.WriteInt(usersPetInfo.AgilityGrow);
                gSPacketIn.WriteInt(usersPetInfo.BloodGrow);
                gSPacketIn.WriteInt(usersPetInfo.DamageGrow);
                gSPacketIn.WriteInt(usersPetInfo.GuardGrow);
                gSPacketIn.WriteInt(usersPetInfo.Level);
                gSPacketIn.WriteInt(usersPetInfo.GP);
                gSPacketIn.WriteInt(usersPetInfo.MaxGP);
                gSPacketIn.WriteInt(usersPetInfo.Hunger);
                gSPacketIn.WriteInt(usersPetInfo.PetHappyStar);
                gSPacketIn.WriteInt(usersPetInfo.MP);
                string[] array = usersPetInfo.Skill.Split('|');
                string[] array2 = usersPetInfo.SkillEquip.Split('|');
                string[] array3 = array;
                foreach (string text in array3)
                {
                    gSPacketIn.WriteInt(int.Parse(text.Split(',')[0]));
                    gSPacketIn.WriteInt(int.Parse(text.Split(',')[1]));
                }
                gSPacketIn.WriteInt(array2.Length);
                string[] array4 = array2;
                foreach (string text2 in array4)
                {
                    gSPacketIn.WriteInt(int.Parse(text2.Split(',')[1]));
                    gSPacketIn.WriteInt(int.Parse(text2.Split(',')[0]));
                }
                gSPacketIn.WriteBoolean(usersPetInfo.IsEquip);
                gSPacketIn.WriteInt(usersPetInfo.PetEquips.Count);
                foreach (PetEquipInfo petEquip in usersPetInfo.PetEquips)
                {
                    gSPacketIn.WriteInt(petEquip.eqType);
                    gSPacketIn.WriteInt(petEquip.eqTemplateID);
                    gSPacketIn.WriteDateTime(petEquip.startTime);
                    gSPacketIn.WriteInt(petEquip.ValidDate);
                }
                gSPacketIn.WriteInt(usersPetInfo.currentStarExp);
            }
            gSPacketIn.WriteInt(eatpet?.weaponLevel ?? 0);
            gSPacketIn.WriteInt(eatpet?.clothesLevel ?? 0);
            gSPacketIn.WriteInt(eatpet?.hatLevel ?? 0);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn sendBuyBadge(int consortiaID, int BadgeID, int ValidDate, bool result, string BadgeBuyTime, int playerid)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(129, playerid);
            gSPacketIn.WriteByte(28);
            gSPacketIn.WriteInt(consortiaID);
            gSPacketIn.WriteInt(BadgeID);
            gSPacketIn.WriteInt(ValidDate);
            gSPacketIn.WriteDateTime(Convert.ToDateTime(BadgeBuyTime));
            gSPacketIn.WriteBoolean(result);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public void SendEdictumVersion()
		{
			EdictumInfo[] allEdictumVersion = WorldMgr.GetAllEdictumVersion();
			Random random = new Random();
			if (allEdictumVersion.Length != 0)
			{
				GSPacketIn packet = new GSPacketIn(75);
				packet.WriteInt(allEdictumVersion.Length);
				EdictumInfo[] array = allEdictumVersion;
				foreach (EdictumInfo edictumInfo in array)
				{
					packet.WriteInt(edictumInfo.ID + random.Next(10000));
				}
				SendTCP(packet);
			}
		}

        public void SendLeftRouleteOpen(UsersExtraInfo info)
        {
            GSPacketIn gSPacketIn = new GSPacketIn((int)ePackageType.LEFT_GUN_ROULETTE);
            bool isOpen = false;
            gSPacketIn.WriteInt(1);
            gSPacketIn.WriteInt(1);
            gSPacketIn.WriteBoolean(isOpen);
            if (isOpen)
            {
                gSPacketIn.WriteInt((!((double)info.LeftRoutteRate > 0.0)) ? info.LeftRoutteCount : 0);
                gSPacketIn.WriteString($"{info.LeftRoutteRate:N1}");
                string leftRouterRateData = GameProperties.LeftRouterRateData;
                for (int i = 0; i < leftRouterRateData.Length; i++)
                {
                    char c = leftRouterRateData[i];
                    if (c == '.' || c == '|')
                    {
                        gSPacketIn.WriteInt(0);
                    }
                    else
                    {
                        gSPacketIn.WriteInt(int.Parse(c.ToString()));
                    }
                }
            }
            SendTCP(gSPacketIn);
        }

        public void SendLeftRouleteResult(UsersExtraInfo info)
        {
            GSPacketIn gSPacketIn = new GSPacketIn((int)ePackageType.LEFT_GUN_ROULETTE_START);
            gSPacketIn.WriteInt((!((double)info.LeftRoutteRate > 0.0)) ? info.LeftRoutteCount : 0);
            gSPacketIn.WriteString($"{info.LeftRoutteRate:N1}");
            SendTCP(gSPacketIn);
        }

        public void SendEnthrallLight()
        {
            GSPacketIn gSPacketIn = new GSPacketIn((int)ePackageType.ENTHRALL_LIGHT);
            gSPacketIn.WriteBoolean(val: false);
            gSPacketIn.WriteInt(0);
            gSPacketIn.WriteBoolean(val: false);
            gSPacketIn.WriteBoolean(val: false);
            SendTCP(gSPacketIn);
        }

        public void SendLoginFailed(string msg)
        {
            GSPacketIn gSPacketIn = new GSPacketIn((int)ePackageType.LOGIN);
            gSPacketIn.WriteByte(1);
            gSPacketIn.WriteString(msg);
            SendTCP(gSPacketIn);
        }

        public void SendOpenNoviceActive(int channel, int activeId, int condition, int awardGot, DateTime startTime, DateTime endTime)
        {
            GSPacketIn pkg = new GSPacketIn((int)ePackageType.NOVICEACTIVITY);
            pkg.WriteInt(channel);
            switch (channel)
            {
                case 0:
                    pkg.WriteInt(activeId);
                    pkg.WriteInt(condition);
                    pkg.WriteInt(awardGot);
                    pkg.WriteDateTime(startTime);
                    pkg.WriteDateTime(endTime);
                    break;
                case 1:
                    pkg.WriteBoolean(val: false);
                    break;
            }
            SendTCP(pkg);
        }

        public void SendUpdateFirstRecharge(bool isRecharge, bool isGetAward)
        {
            GSPacketIn gSPacketIn = new GSPacketIn((int)ePackageType.FIRSTRECHARGE);
            gSPacketIn.WriteBoolean(isRecharge);
            gSPacketIn.WriteBoolean(isGetAward);
            SendTCP(gSPacketIn);
        }

        public GSPacketIn sendBuyBadge(int BadgeID, int ValidDate, bool result, string BadgeBuyTime, int playerid)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(164, playerid);
            gSPacketIn.WriteInt(BadgeID);
            gSPacketIn.WriteInt(BadgeID);
            gSPacketIn.WriteInt(ValidDate);
            gSPacketIn.WriteDateTime(Convert.ToDateTime(BadgeBuyTime));
            gSPacketIn.WriteBoolean(result);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendOpenTimeBox(int condtion, bool isSuccess)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(53);
            gSPacketIn.WriteBoolean(isSuccess);
            gSPacketIn.WriteInt(condtion);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendConsortiaMail(bool result, int playerid)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(215, playerid);
            gSPacketIn.WriteBoolean(result);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendAddFriend(PlayerInfo user, int relation, bool state)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(160, user.ID);
            gSPacketIn.WriteByte(160);
            gSPacketIn.WriteBoolean(state);
            if (state)
            {
                gSPacketIn.WriteInt(user.ID);
                gSPacketIn.WriteString(user.NickName);
                gSPacketIn.WriteByte(user.typeVIP);
                gSPacketIn.WriteInt(user.VIPLevel);
                gSPacketIn.WriteBoolean(user.Sex);
                gSPacketIn.WriteString(user.Style);
                gSPacketIn.WriteString(user.Colors);
                gSPacketIn.WriteString(user.Skin);
                gSPacketIn.WriteInt((user.State == 1) ? 1 : 0);
                gSPacketIn.WriteInt(user.Grade);
                gSPacketIn.WriteInt(user.Hide);
                gSPacketIn.WriteString(user.ConsortiaName);
                gSPacketIn.WriteInt(user.Total);
                gSPacketIn.WriteInt(user.Escape);
                gSPacketIn.WriteInt(user.Win);
                gSPacketIn.WriteInt(user.Offer);
                gSPacketIn.WriteInt(user.Repute);
                gSPacketIn.WriteInt(relation);
                gSPacketIn.WriteString(user.UserName);
                gSPacketIn.WriteInt(user.Nimbus);
                gSPacketIn.WriteInt(user.FightPower);
                gSPacketIn.WriteInt(user.apprenticeshipState);
                gSPacketIn.WriteInt(user.masterID);
                gSPacketIn.WriteString(user.masterOrApprentices);
                gSPacketIn.WriteInt(user.graduatesCount);
                gSPacketIn.WriteString(user.honourOfMaster);
                gSPacketIn.WriteInt(user.AchievementPoint);
                gSPacketIn.WriteString(user.Honor);
                gSPacketIn.WriteBoolean(user.IsMarried);
            }
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendFriendRemove(int FriendID)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(160, FriendID);
            gSPacketIn.WriteByte(161);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendFriendState(int playerID, int state, byte typeVip, int viplevel)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(160, playerID);
            gSPacketIn.WriteByte(165);
            gSPacketIn.WriteInt(state);
            gSPacketIn.WriteInt(typeVip);
            gSPacketIn.WriteInt(viplevel);
            gSPacketIn.WriteBoolean(val: true);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn sendOneOnOneTalk(int receiverID, bool isAutoReply, string SenderNickName, string msg, int playerid)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(160, playerid);
            gSPacketIn.WriteByte(51);
            gSPacketIn.WriteInt(receiverID);
            gSPacketIn.WriteString(SenderNickName);
            gSPacketIn.WriteDateTime(DateTime.Now);
            gSPacketIn.WriteString(msg);
            gSPacketIn.WriteBoolean(isAutoReply);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendUpdateConsotiaBoss(ConsortiaBossInfo bossInfo)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(162);
            gSPacketIn.WriteByte((byte)bossInfo.typeBoss);
            gSPacketIn.WriteInt(bossInfo.powerPoint);
            gSPacketIn.WriteInt(bossInfo.callBossCount);
            gSPacketIn.WriteDateTime(bossInfo.BossOpenTime);
            gSPacketIn.WriteInt(bossInfo.BossLevel);
            gSPacketIn.WriteBoolean(val: false);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendUpdateConsotiaBuffer(GamePlayer player, Dictionary<string, BufferInfo> bufflist)
        {
            List<ConsortiaBuffTempInfo> allConsortiaBuff = ConsortiaExtraMgr.GetAllConsortiaBuff();
            GSPacketIn gSPacketIn = new GSPacketIn(129, player.PlayerId);
            gSPacketIn.WriteByte(26);
            gSPacketIn.WriteInt(allConsortiaBuff.Count);
            foreach (ConsortiaBuffTempInfo item in allConsortiaBuff)
            {
                if (bufflist.ContainsKey(item.id.ToString()))
                {
                    BufferInfo bufferInfo = bufflist[item.id.ToString()];
                    gSPacketIn.WriteInt(item.id);
                    gSPacketIn.WriteBoolean(val: true);
                    gSPacketIn.WriteDateTime(bufferInfo.BeginDate);
                    gSPacketIn.WriteInt(bufferInfo.ValidDate / 24 / 60);
                }
                else
                {
                    gSPacketIn.WriteInt(item.id);
                    gSPacketIn.WriteBoolean(val: false);
                    gSPacketIn.WriteDateTime(DateTime.Now);
                    gSPacketIn.WriteInt(0);
                }
            }
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendPlayerDrill(int ID, Dictionary<int, UserDrillInfo> drills)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(121, ID);
            gSPacketIn.WriteByte(6);
            gSPacketIn.WriteInt(ID);
            gSPacketIn.WriteInt(drills[0].HoleExp);
            gSPacketIn.WriteInt(drills[1].HoleExp);
            gSPacketIn.WriteInt(drills[2].HoleExp);
            gSPacketIn.WriteInt(0);
            gSPacketIn.WriteInt(0);
            gSPacketIn.WriteInt(0);
            gSPacketIn.WriteInt(drills[0].HoleLv);
            gSPacketIn.WriteInt(drills[1].HoleLv);
            gSPacketIn.WriteInt(drills[2].HoleLv);
            gSPacketIn.WriteInt(0);
            gSPacketIn.WriteInt(0);
            gSPacketIn.WriteInt(0);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendUpdateAchievementData(List<AchievementDataInfo> infos)
        {
            bool flag;
            if (infos != null)
            {
                _ = m_gameClient.Player.PlayerCharacter.ID;
                flag = true;
            }
            else
            {
                flag = false;
            }
            if (!flag)
            {
                return null;
            }
            GSPacketIn gSPacketIn = new GSPacketIn(231, m_gameClient.Player.PlayerCharacter.ID);
            gSPacketIn.WriteInt(infos.Count);
            for (int i = 0; i < infos.Count; i++)
            {
                AchievementDataInfo achievementDataInfo = infos[i];
                gSPacketIn.WriteInt(achievementDataInfo.AchievementID);
                gSPacketIn.WriteInt(achievementDataInfo.CompletedDate.Year);
                gSPacketIn.WriteInt(achievementDataInfo.CompletedDate.Month);
                gSPacketIn.WriteInt(achievementDataInfo.CompletedDate.Day);
            }
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendAchievementSuccess(AchievementDataInfo d)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(230);
            gSPacketIn.WriteInt(d.AchievementID);
            gSPacketIn.WriteInt(d.CompletedDate.Year);
            gSPacketIn.WriteInt(d.CompletedDate.Month);
            gSPacketIn.WriteInt(d.CompletedDate.Day);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendUpdateAchievements(List<UsersRecordInfo> infos)
        {
            bool flag;
            if (infos != null && m_gameClient != null && m_gameClient.Player != null)
            {
                _ = m_gameClient.Player.PlayerCharacter.ID;
                flag = true;
            }
            else
            {
                flag = false;
            }
            if (!flag)
            {
                return null;
            }
            GSPacketIn gSPacketIn = new GSPacketIn(229, m_gameClient.Player.PlayerCharacter.ID);
            gSPacketIn.WriteInt(infos.Count);
            for (int i = 0; i < infos.Count; i++)
            {
                UsersRecordInfo usersRecordInfo = infos[i];
                gSPacketIn.WriteInt(usersRecordInfo.RecordID);
                gSPacketIn.WriteInt(usersRecordInfo.Total);
            }
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendUpdateAchievements(UsersRecordInfo info)
        {
            bool flag;
            if (info != null && m_gameClient != null && m_gameClient.Player != null)
            {
                _ = m_gameClient.Player.PlayerCharacter.ID;
                flag = true;
            }
            else
            {
                flag = false;
            }
            if (!flag)
            {
                return null;
            }
            GSPacketIn gSPacketIn = new GSPacketIn(229, m_gameClient.Player.PlayerCharacter.ID);
            gSPacketIn.WriteInt(1);
            for (int i = 0; i < 1; i++)
            {
                gSPacketIn.WriteInt(info.RecordID);
                gSPacketIn.WriteInt(info.Total);
            }
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendInitAchievements(List<UsersRecordInfo> infos)
        {
            bool flag;
            if (infos != null && m_gameClient.Player != null)
            {
                _ = m_gameClient.Player.PlayerCharacter.ID;
                flag = true;
            }
            else
            {
                flag = false;
            }
            if (!flag)
            {
                return null;
            }
            GSPacketIn gSPacketIn = new GSPacketIn(228, m_gameClient.Player.PlayerCharacter.ID);
            gSPacketIn.WriteInt(infos.Count);
            for (int i = 0; i < infos.Count; i++)
            {
                UsersRecordInfo usersRecordInfo = infos[i];
                gSPacketIn.WriteInt(usersRecordInfo.RecordID);
                gSPacketIn.WriteInt(usersRecordInfo.Total);
            }
            SendTCP(gSPacketIn);
            SendUpdateAchievements(infos);
            return gSPacketIn;
        }

        public void SendLoginSuccess()
        {
            if (m_gameClient.Player != null)
            {
                GSPacketIn pkg = new GSPacketIn(1, m_gameClient.Player.PlayerCharacter.ID);
                pkg.WriteByte(0);
                pkg.WriteInt(m_gameClient.Player.ZoneId);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Attack);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Defence);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Agility);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Luck);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.GP);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Repute);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Gold);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Money + m_gameClient.Player.PlayerCharacter.MoneyLock);
                pkg.WriteInt(m_gameClient.Player.GetMedalNum());
                pkg.WriteInt(0);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Hide);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.FightPower);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.apprenticeshipState);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.masterID);
                pkg.WriteString(m_gameClient.Player.PlayerCharacter.masterOrApprentices);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.graduatesCount);
                pkg.WriteString(m_gameClient.Player.PlayerCharacter.honourOfMaster);
                pkg.WriteDateTime(m_gameClient.Player.PlayerCharacter.freezesDate);
                pkg.WriteByte(m_gameClient.Player.PlayerCharacter.typeVIP);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.VIPLevel);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.VIPExp);
                pkg.WriteDateTime(m_gameClient.Player.PlayerCharacter.VIPExpireDay);
                pkg.WriteDateTime(m_gameClient.Player.PlayerCharacter.LastDate);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.VIPNextLevelDaysNeeded);
                pkg.WriteDateTime(DateTime.Now);
                pkg.WriteBoolean(m_gameClient.Player.PlayerCharacter.CanTakeVipReward);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.OptionOnOff);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.AchievementPoint);
                pkg.WriteString(m_gameClient.Player.PlayerCharacter.Honor);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.honorId);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.OnlineTime);
                pkg.WriteBoolean(m_gameClient.Player.PlayerCharacter.Sex);
                pkg.WriteString(m_gameClient.Player.PlayerCharacter.Style + "&" + m_gameClient.Player.PlayerCharacter.Colors);
                pkg.WriteString(m_gameClient.Player.PlayerCharacter.Skin);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.ConsortiaID);
                pkg.WriteString(m_gameClient.Player.PlayerCharacter.ConsortiaName);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.badgeID);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.DutyLevel);
                pkg.WriteString(m_gameClient.Player.PlayerCharacter.DutyName);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Right);
                pkg.WriteString(m_gameClient.Player.PlayerCharacter.ChairmanName);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.ConsortiaHonor);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.ConsortiaRiches);
                pkg.WriteBoolean(m_gameClient.Player.PlayerCharacter.HasBagPassword);
                pkg.WriteString(m_gameClient.Player.PlayerCharacter.PasswordQuest1);
                pkg.WriteString(m_gameClient.Player.PlayerCharacter.PasswordQuest2);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.FailedPasswordAttemptCount);
                pkg.WriteString(m_gameClient.Player.PlayerCharacter.UserName);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Nimbus);
                pkg.WriteString(m_gameClient.Player.PlayerCharacter.PvePermission);
                pkg.WriteString(m_gameClient.Player.PlayerCharacter.FightLabPermission);
                pkg.WriteInt(99999);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.BoxProgression);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.GetBoxLevel);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.AlreadyGetBox);
                pkg.WriteDateTime(m_gameClient.Player.Extra.Info.LastTimeHotSpring);
                pkg.WriteDateTime(m_gameClient.Player.PlayerCharacter.ShopFinallyGottenTime);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Riches);
                pkg.WriteInt(m_gameClient.Player.MatchInfo.dailyScore);
                pkg.WriteInt(m_gameClient.Player.MatchInfo.dailyWinCount);
                pkg.WriteInt(m_gameClient.Player.MatchInfo.dailyGameCount);
                pkg.WriteBoolean(m_gameClient.Player.MatchInfo.DailyLeagueFirst);
                pkg.WriteInt(m_gameClient.Player.MatchInfo.DailyLeagueLastScore);
                pkg.WriteInt(m_gameClient.Player.MatchInfo.weeklyScore);
                pkg.WriteInt(m_gameClient.Player.MatchInfo.weeklyGameCount);
                pkg.WriteInt(m_gameClient.Player.MatchInfo.weeklyRanking);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Texp.spdTexpExp);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Texp.attTexpExp);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Texp.defTexpExp);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Texp.hpTexpExp);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Texp.lukTexpExp);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Texp.texpTaskCount);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.Texp.texpCount);
                pkg.WriteDateTime(m_gameClient.Player.PlayerCharacter.Texp.texpTaskDate);
                pkg.WriteBoolean(val: false);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.badLuckNumber);
                pkg.WriteInt(0);
                pkg.WriteDateTime(DateTime.Now);
                pkg.WriteInt(0);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.accumulativeLoginDays);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.accumulativeAwardDays);
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.totemId);//totemid
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.necklaceExp);//_loc_3.necklaceExp = _loc_2.readInt();
                pkg.WriteInt(m_gameClient.Player.PlayerCharacter.fineSuitExp);
                SendTCP(pkg);
            }
        }

        public void SendLoginSuccess2()
        {
        }

        public void method_0(byte[] m, byte[] e)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(7);
            gSPacketIn.Write(m);
            gSPacketIn.Write(e);
            SendTCP(gSPacketIn);
        }

        public void SendCheckCode()
        {
            if (m_gameClient.Player != null && m_gameClient.Player.PlayerCharacter.CheckCount >= GameProperties.CHECK_MAX_FAILED_COUNT)
            {
                if (m_gameClient.Player.PlayerCharacter.CheckError == 0)
                {
                    m_gameClient.Player.PlayerCharacter.CheckCount += 10000;
                }
                GSPacketIn gSPacketIn = new GSPacketIn(200, m_gameClient.Player.PlayerCharacter.ID, 10240);

                if (m_gameClient.Player.PlayerCharacter.CheckError < 1)
                {
                    gSPacketIn.WriteByte(1);
                }
                else
                {
                    gSPacketIn.WriteByte(2);
                }
                gSPacketIn.WriteBoolean(val: true);
                gSPacketIn.WriteByte(1);
                gSPacketIn.WriteString("hi");
                m_gameClient.Player.PlayerCharacter.CheckCode = CheckCode.GenerateCheckCode();
                gSPacketIn.Write(CheckCode.CreateImage(m_gameClient.Player.PlayerCharacter.CheckCode));
                SendTCP(gSPacketIn);
            }
        }

        public void SendKitoff(string msg)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(2);
            gSPacketIn.WriteString(msg);
            SendTCP(gSPacketIn);
        }

        public void SendEditionError(string msg)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(12);
            gSPacketIn.WriteString(msg);
            SendTCP(gSPacketIn);
        }

        public void SendWaitingRoom(bool result)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(16);
            gSPacketIn.WriteByte((byte)(result ? 1u : 0u));
            SendTCP(gSPacketIn);
        }

        public GSPacketIn SendPlayerState(int id, byte state)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(32, id);
            gSPacketIn.WriteByte(state);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public virtual GSPacketIn SendMessage(eMessageType type, string message)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(3);
            gSPacketIn.WriteInt((int)type);
            gSPacketIn.WriteString(message);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public void SendReady()
        {
            SendTCP(new GSPacketIn(0));
        }

        public void SendUpdatePrivateInfo(PlayerInfo info, int medal)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(38, info.ID);
            gSPacketIn.WriteInt(info.Money + info.MoneyLock);
            gSPacketIn.WriteInt(medal);
            gSPacketIn.WriteInt(info.Score);
            gSPacketIn.WriteInt(info.Gold);
            gSPacketIn.WriteInt(info.GiftToken);
            //gSPacketIn.WriteInt(info.badLuckNumber);
            gSPacketIn.WriteInt(info.damageScores);
            if (GameProperties.IsOpenPetScore)
            {
                gSPacketIn.WriteInt(info.petScore);
            }
            gSPacketIn.WriteInt(info.hardCurrency);
            gSPacketIn.WriteInt(info.myHonor);//myHonor
            SendTCP(gSPacketIn);
        }

        public GSPacketIn SendUpdatePublicPlayer(PlayerInfo info, UserMatchInfo matchInfo, UsersExtraInfo extraInfo)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(67, info.ID);
            gSPacketIn.WriteInt(info.GP);
            gSPacketIn.WriteInt(info.Offer);
            gSPacketIn.WriteInt(info.RichesOffer);
            gSPacketIn.WriteInt(info.RichesRob);
            gSPacketIn.WriteInt(info.Win);
            gSPacketIn.WriteInt(info.Total);
            gSPacketIn.WriteInt(info.Escape);
            gSPacketIn.WriteInt(info.Attack);
            gSPacketIn.WriteInt(info.Defence);
            gSPacketIn.WriteInt(info.Agility);
            gSPacketIn.WriteInt(info.Luck);
            gSPacketIn.WriteInt(info.hp);
            gSPacketIn.WriteInt(info.Hide);
            gSPacketIn.WriteString(info.Style);
            gSPacketIn.WriteString(info.Colors);
            gSPacketIn.WriteString(info.Skin);
            gSPacketIn.WriteBoolean(info.IsShowConsortia);
            gSPacketIn.WriteInt(info.ConsortiaID);
            gSPacketIn.WriteString(info.ConsortiaName);
            gSPacketIn.WriteInt(info.badgeID);
            gSPacketIn.WriteInt(0);
            gSPacketIn.WriteInt(0);
            gSPacketIn.WriteInt(info.Nimbus);
            gSPacketIn.WriteString(info.PvePermission);
            gSPacketIn.WriteString(info.FightLabPermission);
            gSPacketIn.WriteInt(info.FightPower);
            gSPacketIn.WriteInt(info.apprenticeshipState);
            gSPacketIn.WriteInt(info.masterID);
            gSPacketIn.WriteString(info.masterOrApprentices);
            gSPacketIn.WriteInt(info.graduatesCount);
            gSPacketIn.WriteString(info.honourOfMaster);
            gSPacketIn.WriteInt(info.AchievementPoint);
            gSPacketIn.WriteString(info.Honor);
            gSPacketIn.WriteDateTime(info.LastSpaDate);
            gSPacketIn.WriteInt(info.charmGP);
            gSPacketIn.WriteInt(0);
            gSPacketIn.WriteDateTime(info.ShopFinallyGottenTime);
            gSPacketIn.WriteInt(info.Riches);
            gSPacketIn.WriteInt(matchInfo.addDayPrestge);
            gSPacketIn.WriteInt(matchInfo.dailyWinCount);
            gSPacketIn.WriteInt(matchInfo.dailyGameCount);
            gSPacketIn.WriteInt(matchInfo.totalPrestige);
            gSPacketIn.WriteInt(matchInfo.weeklyGameCount);
            gSPacketIn.WriteInt(info.Texp.spdTexpExp);
            gSPacketIn.WriteInt(info.Texp.attTexpExp);
            gSPacketIn.WriteInt(info.Texp.defTexpExp);
            gSPacketIn.WriteInt(info.Texp.hpTexpExp);
            gSPacketIn.WriteInt(info.Texp.lukTexpExp);
            gSPacketIn.WriteInt(info.Texp.texpTaskCount);
            gSPacketIn.WriteInt(info.Texp.texpCount);
            gSPacketIn.WriteDateTime(info.Texp.texpTaskDate);
            gSPacketIn.WriteInt(0);
            gSPacketIn.WriteInt(info.evolutionGrade);
            gSPacketIn.WriteInt(info.evolutionExp);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public void SendPingTime(GamePlayer player)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(4);
            player.PingStart = DateTime.Now.Ticks;
            gSPacketIn.WriteInt(player.PlayerCharacter.AntiAddiction);
            SendTCP(gSPacketIn);
        }

        public GSPacketIn SendNetWork(int id, long delay)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(6, id);
            gSPacketIn.WriteInt((int)delay / 1000 / 10);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendUserEquip(PlayerInfo player, List<ItemInfo> items, List<UserGemStone> UserGemStone)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.ITEM_EQUIP, player.ID);
            pkg.WriteInt(player.ID);
            pkg.WriteString(player.NickName);
            pkg.WriteInt(player.Agility);
            pkg.WriteInt(player.Attack);
            pkg.WriteString(player.Colors);
            pkg.WriteString(player.Skin);
            pkg.WriteInt(player.Defence);
            pkg.WriteInt(player.GP);
            pkg.WriteInt(player.Grade);
            pkg.WriteInt(player.Luck);
            pkg.WriteInt(player.hp);
            pkg.WriteInt(player.Hide);
            pkg.WriteInt(player.Repute);
            pkg.WriteBoolean(player.Sex);
            pkg.WriteString(player.Style);
            pkg.WriteInt(player.Offer);
            pkg.WriteByte(player.typeVIP);
            pkg.WriteInt(player.VIPLevel);
            pkg.WriteInt(player.Win);
            pkg.WriteInt(player.Total);
            pkg.WriteInt(player.Escape);
            pkg.WriteInt(player.ConsortiaID);
            pkg.WriteString(player.ConsortiaName);
            pkg.WriteInt(player.badgeID);
            pkg.WriteInt(player.RichesOffer);
            pkg.WriteInt(player.RichesRob);
            pkg.WriteBoolean(player.IsMarried);
            pkg.WriteInt(player.SpouseID);
            pkg.WriteString(player.SpouseName);
            pkg.WriteString(player.DutyName);
            pkg.WriteInt(player.Nimbus);
            pkg.WriteInt(player.FightPower);
            pkg.WriteInt(player.apprenticeshipState);
            pkg.WriteInt(player.masterID);
            pkg.WriteString(player.masterOrApprentices);
            pkg.WriteInt(player.graduatesCount);
            pkg.WriteString(player.honourOfMaster);
            pkg.WriteInt(player.AchievementPoint);
            pkg.WriteString(player.Honor);
            pkg.WriteDateTime(DateTime.Now.AddDays(-2.0));
            pkg.WriteInt(player.Texp.spdTexpExp);
            pkg.WriteInt(player.Texp.attTexpExp);
            pkg.WriteInt(player.Texp.defTexpExp);
            pkg.WriteInt(player.Texp.hpTexpExp);
            pkg.WriteInt(player.Texp.lukTexpExp);
            pkg.WriteBoolean(val: false);
            pkg.WriteInt(0);
            pkg.WriteInt(player.totemId);//totemid
            pkg.WriteInt(player.necklaceExp);//_loc_5.necklaceExp = _loc_2.readInt();
            pkg.WriteInt(items.Count);
            foreach (ItemInfo item in items)
            {
                pkg.WriteByte((byte)item.BagType);
                pkg.WriteInt(item.UserID);
                pkg.WriteInt(item.ItemID);
                pkg.WriteInt(item.Count);
                pkg.WriteInt(item.Place);
                pkg.WriteInt(item.TemplateID);
                pkg.WriteInt(item.AttackCompose);
                pkg.WriteInt(item.DefendCompose);
                pkg.WriteInt(item.AgilityCompose);
                pkg.WriteInt(item.LuckCompose);
                pkg.WriteInt(item.StrengthenLevel);
                pkg.WriteBoolean(item.IsBinds);
                pkg.WriteBoolean(item.IsJudge);
                pkg.WriteDateTime(item.BeginDate);
                pkg.WriteInt(item.ValidDate);
                pkg.WriteString(item.Color);
                pkg.WriteString(item.Skin);
                pkg.WriteBoolean(item.IsUsed);
                pkg.WriteInt(item.Hole1);
                pkg.WriteInt(item.Hole2);
                pkg.WriteInt(item.Hole3);
                pkg.WriteInt(item.Hole4);
                pkg.WriteInt(item.Hole5);
                pkg.WriteInt(item.Hole6);
                pkg.WriteString(item.Pic);
                pkg.WriteInt(item.RefineryLevel);
                pkg.WriteDateTime(DateTime.Now);
                pkg.WriteByte((byte)item.Hole5Level);
                pkg.WriteInt(item.Hole5Exp);
                pkg.WriteByte((byte)item.Hole6Level);
                pkg.WriteInt(item.Hole6Exp);
                pkg.WriteInt(item.curExp);
                pkg.WriteBoolean(item.IsValidGoldItem());
                if (item.isGold)
                {
                    pkg.WriteInt(item.goldValidDate);
                    pkg.WriteDateTime(item.goldBeginTime);
                }
                pkg.WriteString(item.latentEnergyCurStr);
                pkg.WriteString(item.latentEnergyNewStr);
                pkg.WriteDateTime(item.latentEnergyEndTime);
            }
            pkg.WriteInt(UserGemStone.Count);
            for (int i = 0; i < UserGemStone.Count; i++)
            {
                pkg.WriteInt(UserGemStone[i].FigSpiritId);
                pkg.WriteString(UserGemStone[i].FigSpiritIdValue);
                pkg.WriteInt(UserGemStone[i].EquipPlace);
            }
            //equipGhost
            Dictionary<string, UserEquipGhostInfo> equipGhostList = JsonConvert.DeserializeObject<Dictionary<string, UserEquipGhostInfo>>(player.GhostEquipList);

            if (equipGhostList == null)
            {
                pkg.WriteInt(0);
            }
            else
            {
                pkg.WriteInt(equipGhostList.Count);

                foreach (UserEquipGhostInfo egInfo in equipGhostList.Values)
                {
                    pkg.WriteInt(egInfo.BagType);
                    pkg.WriteInt(egInfo.Place);
                    pkg.WriteInt(egInfo.Level);
                    pkg.WriteInt(egInfo.TotalGhost);
                }

            }
            pkg.Compress();
            SendTCP(pkg);
            return pkg;
        }

        public void SendDateTime()
        {
            GSPacketIn pkg = new GSPacketIn(5);
            pkg.WriteDateTime(DateTime.Now);
            SendTCP(pkg);
        }

        public GSPacketIn SendDailyAward(GamePlayer player)
        {
            bool val = false;
            DateTime date = DateTime.Now.Date;
            DateTime date2 = player.PlayerCharacter.LastAward.Date;
            if (date != date2)
            {
                val = true;
            }
            GSPacketIn pkg = new GSPacketIn(13);
            pkg.WriteBoolean(val);
            pkg.WriteInt(0);
            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendUpdateRoomList(List<BaseRoom> roomlist)
        {
            GSPacketIn pkg = new GSPacketIn(94);
            pkg.WriteByte(9);
            pkg.WriteInt(roomlist.Count);
            int num = ((roomlist.Count < 9) ? roomlist.Count : 9);
            pkg.WriteInt(num); // room count per page
            for (int i = 0; i < num; i++)
            {
                BaseRoom baseRoom = roomlist[i];
                pkg.WriteInt(baseRoom.RoomId);
                pkg.WriteByte((byte)baseRoom.RoomType);
                pkg.WriteByte(baseRoom.TimeMode);
                pkg.WriteByte((byte)baseRoom.PlayerCount);
                pkg.WriteByte((byte)baseRoom.viewerCnt);// viewer count
                pkg.WriteByte((byte)baseRoom.maxViewerCnt);// max viewer count
                pkg.WriteByte((byte)baseRoom.PlacesCount);
                pkg.WriteBoolean((!string.IsNullOrEmpty(baseRoom.Password)) ? true : false);
                pkg.WriteInt(baseRoom.MapId);
                pkg.WriteBoolean(baseRoom.IsPlaying);
                pkg.WriteString(baseRoom.Name);
                pkg.WriteByte((byte)baseRoom.GameType);
                pkg.WriteByte((byte)baseRoom.HardLevel);
                pkg.WriteInt(baseRoom.LevelLimits);
                pkg.WriteBoolean(baseRoom.isOpenBoss);
            }
            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendSceneAddPlayer(GamePlayer player)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(18, player.PlayerCharacter.ID);
            gSPacketIn.WriteInt(player.PlayerCharacter.Grade);
            gSPacketIn.WriteBoolean(player.PlayerCharacter.Sex);
            gSPacketIn.WriteString(player.PlayerCharacter.NickName);
            gSPacketIn.WriteByte(player.PlayerCharacter.typeVIP);
            gSPacketIn.WriteInt(player.PlayerCharacter.VIPLevel);
            gSPacketIn.WriteString(player.PlayerCharacter.ConsortiaName);
            gSPacketIn.WriteInt(player.PlayerCharacter.Offer);
            gSPacketIn.WriteInt(player.PlayerCharacter.Win);
            gSPacketIn.WriteInt(player.PlayerCharacter.Total);
            gSPacketIn.WriteInt(player.PlayerCharacter.Escape);
            gSPacketIn.WriteInt(player.PlayerCharacter.ConsortiaID);
            gSPacketIn.WriteInt(player.PlayerCharacter.Repute);
            gSPacketIn.WriteBoolean(player.PlayerCharacter.IsMarried);
            if (player.PlayerCharacter.IsMarried)
            {
                gSPacketIn.WriteInt(player.PlayerCharacter.SpouseID);
                gSPacketIn.WriteString(player.PlayerCharacter.SpouseName);
            }
            gSPacketIn.WriteString(player.PlayerCharacter.UserName);
            gSPacketIn.WriteInt(player.PlayerCharacter.FightPower);
            gSPacketIn.WriteInt(player.PlayerCharacter.apprenticeshipState);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendSceneRemovePlayer(GamePlayer player)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(21, player.PlayerCharacter.ID);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendRoomPlayerAdd(GamePlayer player)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(94, player.PlayerId);
            gSPacketIn.WriteByte(4);
            bool val = player.CurrentRoom != null;
            gSPacketIn.WriteBoolean(val);
            gSPacketIn.WriteByte((byte)player.CurrentRoomIndex);
            gSPacketIn.WriteByte((byte)player.CurrentRoomTeam);
            gSPacketIn.WriteBoolean(val: false);
            gSPacketIn.WriteInt(player.PlayerCharacter.Grade);
            gSPacketIn.WriteInt(player.PlayerCharacter.Offer);
            gSPacketIn.WriteInt(player.PlayerCharacter.Hide);
            gSPacketIn.WriteInt(player.PlayerCharacter.Repute);
            gSPacketIn.WriteInt((int)player.PingTime / 1000 / 10);
            gSPacketIn.WriteInt(player.ZoneId);
            gSPacketIn.WriteInt(player.PlayerCharacter.ID);
            gSPacketIn.WriteString(player.PlayerCharacter.NickName);
            gSPacketIn.WriteByte(player.PlayerCharacter.typeVIP);
            gSPacketIn.WriteInt(player.PlayerCharacter.VIPLevel);
            gSPacketIn.WriteBoolean(player.PlayerCharacter.Sex);
            gSPacketIn.WriteString(player.PlayerCharacter.Style);
            gSPacketIn.WriteString(player.PlayerCharacter.Colors);
            gSPacketIn.WriteString(player.PlayerCharacter.Skin);
            gSPacketIn.WriteInt(player.EquipBag.GetItemAt(6)?.TemplateID ?? (-1));
            if (player.SecondWeapon == null)
            {
                gSPacketIn.WriteInt(0);
            }
            else
            {
                gSPacketIn.WriteInt(player.SecondWeapon.TemplateID);
            }
            gSPacketIn.WriteInt(player.PlayerCharacter.ConsortiaID);
            gSPacketIn.WriteString(player.PlayerCharacter.ConsortiaName);
            gSPacketIn.WriteInt(player.PlayerCharacter.badgeID);
            gSPacketIn.WriteInt(player.PlayerCharacter.Win);
            gSPacketIn.WriteInt(player.PlayerCharacter.Total);
            gSPacketIn.WriteInt(player.PlayerCharacter.Escape);
            gSPacketIn.WriteInt(player.PlayerCharacter.ConsortiaLevel);
            gSPacketIn.WriteInt(player.PlayerCharacter.ConsortiaRepute);
            gSPacketIn.WriteBoolean(player.PlayerCharacter.IsMarried);
            if (player.PlayerCharacter.IsMarried)
            {
                gSPacketIn.WriteInt(player.PlayerCharacter.SpouseID);
                gSPacketIn.WriteString(player.PlayerCharacter.SpouseName);
            }
            gSPacketIn.WriteString(player.PlayerCharacter.UserName);
            gSPacketIn.WriteInt(player.PlayerCharacter.Nimbus);
            gSPacketIn.WriteInt(player.PlayerCharacter.FightPower);
            gSPacketIn.WriteInt(player.PlayerCharacter.apprenticeshipState);
            gSPacketIn.WriteInt(player.PlayerCharacter.masterID);
            gSPacketIn.WriteString(player.PlayerCharacter.masterOrApprentices);
            gSPacketIn.WriteInt(player.PlayerCharacter.graduatesCount);
            gSPacketIn.WriteString(player.PlayerCharacter.honourOfMaster);
            if (player.MatchInfo == null)
            {
                gSPacketIn.WriteBoolean(false);
                gSPacketIn.WriteInt(0);
            }
            else
            {
                gSPacketIn.WriteBoolean(player.MatchInfo.DailyLeagueFirst);
                gSPacketIn.WriteInt(player.MatchInfo.DailyLeagueLastScore);
            }
            if (player.Pet == null)
            {
                gSPacketIn.WriteInt(0);
            }
            else
            {
                gSPacketIn.WriteInt(1);
                gSPacketIn.WriteInt(player.Pet.Place);
                gSPacketIn.WriteInt(player.Pet.TemplateID);
                gSPacketIn.WriteInt(player.Pet.ID);
                gSPacketIn.WriteString(player.Pet.Name);
                gSPacketIn.WriteInt(player.PlayerCharacter.ID);
                gSPacketIn.WriteInt(player.Pet.Level);
                string[] array = player.Pet.SkillEquip.Split('|');
                gSPacketIn.WriteInt(array.Length);
                string[] array2 = array;
                foreach (string text in array2)
                {
                    gSPacketIn.WriteInt(int.Parse(text.Split(',')[1]));
                    gSPacketIn.WriteInt(int.Parse(text.Split(',')[0]));
                }
            }
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendRoomPlayerRemove(GamePlayer player)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(94, player.PlayerId);
            gSPacketIn.WriteByte(5);
            gSPacketIn.Parameter1 = player.PlayerId;
            gSPacketIn.ClientID = player.PlayerId;
            gSPacketIn.WriteInt(player.ZoneId);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendRoomUpdatePlayerStates(byte[] states)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(94);
            gSPacketIn.WriteByte(15);
            for (int i = 0; i < states.Length; i++)
            {
                gSPacketIn.WriteByte(states[i]);
            }
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendRoomUpdatePlacesStates(int[] states)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(94);
            gSPacketIn.WriteByte(10);
            for (int i = 0; i < states.Length; i++)
            {
                gSPacketIn.WriteInt(states[i]);
            }
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendRoomPlayerChangedTeam(GamePlayer player)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(94, player.PlayerId);
            gSPacketIn.WriteByte(6);
            gSPacketIn.WriteByte((byte)player.CurrentRoomTeam);
            gSPacketIn.WriteByte((byte)player.CurrentRoomIndex);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendRoomCreate(BaseRoom room)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(94);
            gSPacketIn.WriteByte(0);
            gSPacketIn.WriteInt(room.RoomId);
            gSPacketIn.WriteByte((byte)room.RoomType);
            gSPacketIn.WriteByte((byte)room.HardLevel);
            gSPacketIn.WriteByte(room.TimeMode);
            gSPacketIn.WriteByte((byte)room.PlayerCount);
            gSPacketIn.WriteByte((byte)room.viewerCnt);
            gSPacketIn.WriteByte((byte)room.PlacesCount);
            gSPacketIn.WriteBoolean((!string.IsNullOrEmpty(room.Password)) ? true : false);
            gSPacketIn.WriteInt(room.MapId);
            gSPacketIn.WriteBoolean(room.IsPlaying);
            gSPacketIn.WriteString(room.Name);
            gSPacketIn.WriteByte((byte)room.GameType);
            gSPacketIn.WriteInt(room.LevelLimits);
            gSPacketIn.WriteBoolean(room.isCrosszone);
            gSPacketIn.WriteBoolean(room.isWithinLeageTime);
            gSPacketIn.WriteBoolean(room.isOpenBoss);
            gSPacketIn.WriteString(room.Pic);
            /*
			_loc3_.isOpenBoss = _loc2_.readBoolean();
			_loc3_.pic = param1.pkg.readUTF();
			*/
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendRoomLoginResult(bool result)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(94);
            gSPacketIn.WriteByte(1);
            gSPacketIn.WriteBoolean(result);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendRoomPairUpStart(BaseRoom room)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(94);
            gSPacketIn.WriteByte(13);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendGameRoomInfo(GamePlayer player, BaseRoom game)
        {
            return new GSPacketIn(94, player.PlayerCharacter.ID);
        }

        public GSPacketIn SendRoomType(GamePlayer player, BaseRoom game)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(94);
            gSPacketIn.WriteByte(12);
            gSPacketIn.WriteByte((byte)game.GameType);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendRoomPairUpCancel(BaseRoom room)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(94);
            gSPacketIn.WriteByte(11);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendRoomClear(GamePlayer player, BaseRoom game)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(96, player.PlayerCharacter.ID);
            gSPacketIn.WriteInt(game.RoomId);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendEquipChange(GamePlayer player, int place, int goodsID, string style)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(66, player.PlayerCharacter.ID);
            gSPacketIn.WriteByte((byte)place);
            gSPacketIn.WriteInt(goodsID);
            gSPacketIn.WriteString(style);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendRoomChange(BaseRoom room)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(94);
            gSPacketIn.WriteByte(2);
            gSPacketIn.WriteInt(room.MapId);
            gSPacketIn.WriteByte((byte)room.RoomType);
            gSPacketIn.WriteString(room.Password);
            gSPacketIn.WriteString(room.Name);
            gSPacketIn.WriteByte(room.TimeMode);
            gSPacketIn.WriteByte((byte)room.HardLevel);
            gSPacketIn.WriteInt(room.LevelLimits);
            gSPacketIn.WriteBoolean(room.isCrosszone);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendGameRoomSetupChange(BaseRoom room)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(94);
            gSPacketIn.WriteByte(2);
            gSPacketIn.WriteBoolean(room.isOpenBoss);//_current.isOpenBoss
            if (room.isOpenBoss)
                gSPacketIn.WriteString(room.Pic);
            gSPacketIn.WriteInt(room.MapId);
            gSPacketIn.WriteByte((byte)room.RoomType);
            gSPacketIn.WriteString((room.Password == null) ? "" : room.Password);
            gSPacketIn.WriteString((room.Name == null) ? "Gunny1" : room.Name);
            gSPacketIn.WriteByte(room.TimeMode);
            gSPacketIn.WriteByte((byte)room.HardLevel);
            gSPacketIn.WriteInt(room.LevelLimits);
            gSPacketIn.WriteBoolean(room.isCrosszone);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendFusionPreview(GamePlayer player, Dictionary<int, double> previewItemList, bool isbind, int MinValid)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(76, player.PlayerCharacter.ID);
            gSPacketIn.WriteInt(previewItemList.Count);
            foreach (KeyValuePair<int, double> previewItem in previewItemList)
            {
                gSPacketIn.WriteInt(previewItem.Key);
                gSPacketIn.WriteInt(MinValid);
                int num = Convert.ToInt32(previewItem.Value);
                gSPacketIn.WriteInt((num > 100) ? 100 : ((num >= 0) ? num : 0));
            }
            gSPacketIn.WriteBoolean(isbind);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        //public GSPacketIn SendFusionResult(GamePlayer player, bool result)
        //{
        //    GSPacketIn gSPacketIn = new GSPacketIn((byte)ePackageType.ITEM_FUSION, player.PlayerCharacter.ID);
        //    //if (result)
        //    //{

        //    //}
        //    gSPacketIn.WriteInt(0);
        //    gSPacketIn.WriteBoolean(result);
        //    SendTCP(gSPacketIn);
        //    return gSPacketIn;
        //}

        public GSPacketIn SendFusionResult(GamePlayer player, bool result)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.ITEM_FUSION, player.PlayerCharacter.ID);
            pkg.WriteBoolean(result);
            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendRefineryPreview(GamePlayer player, int templateid, bool isbind, ItemInfo item)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(111, player.PlayerCharacter.ID);
            gSPacketIn.WriteInt(templateid);
            gSPacketIn.WriteInt(item.ValidDate);
            gSPacketIn.WriteBoolean(isbind);
            gSPacketIn.WriteInt(item.AgilityCompose);
            gSPacketIn.WriteInt(item.AttackCompose);
            gSPacketIn.WriteInt(item.DefendCompose);
            gSPacketIn.WriteInt(item.LuckCompose);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public void SendUpdateInventorySlot(PlayerInventory bag, int[] updatedSlots)
        {
            if (m_gameClient.Player == null)
            {
                return;
            }
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.GRID_GOODS, m_gameClient.Player.PlayerCharacter.ID, 10240);
            pkg.WriteInt(bag.BagType);
            pkg.WriteInt(updatedSlots.Length);
            foreach (int num in updatedSlots)
            {
                pkg.WriteInt(num);
                ItemInfo itemAt = bag.GetItemAt(num);
                if (itemAt == null)
                {
                    pkg.WriteBoolean(val: false);
                    continue;
                }
                pkg.WriteBoolean(val: true);
                pkg.WriteInt(itemAt.UserID);
                pkg.WriteInt(itemAt.ItemID);
                pkg.WriteInt(itemAt.Count);
                pkg.WriteInt(itemAt.Place);
                pkg.WriteInt(itemAt.TemplateID);
                pkg.WriteInt(itemAt.AttackCompose);
                pkg.WriteInt(itemAt.DefendCompose);
                pkg.WriteInt(itemAt.AgilityCompose);
                pkg.WriteInt(itemAt.LuckCompose);
                pkg.WriteInt(itemAt.StrengthenLevel);
                pkg.WriteInt(itemAt.StrengthenExp);
                pkg.WriteBoolean(itemAt.IsBinds);
                pkg.WriteBoolean(itemAt.IsJudge);
                pkg.WriteDateTime(itemAt.BeginDate);
                pkg.WriteInt(itemAt.ValidDate);
                pkg.WriteString((itemAt.Color == null) ? "" : itemAt.Color);
                pkg.WriteString((itemAt.Skin == null) ? "" : itemAt.Skin);
                pkg.WriteBoolean(itemAt.IsUsed);
                pkg.WriteInt(itemAt.Hole1);
                pkg.WriteInt(itemAt.Hole2);
                pkg.WriteInt(itemAt.Hole3);
                pkg.WriteInt(itemAt.Hole4);
                pkg.WriteInt(itemAt.Hole5);
                pkg.WriteInt(itemAt.Hole6);
                pkg.WriteString(itemAt.Pic);
                pkg.WriteInt(itemAt.RefineryLevel);
                pkg.WriteDateTime(DateTime.Now.AddDays(5.0));
                pkg.WriteInt(itemAt.StrengthenTimes);
                pkg.WriteByte((byte)itemAt.Hole5Level);
                pkg.WriteInt(itemAt.Hole5Exp);
                pkg.WriteByte((byte)itemAt.Hole6Level);
                pkg.WriteInt(itemAt.Hole6Exp);
                pkg.WriteInt(itemAt.curExp);
                pkg.WriteBoolean(itemAt.cellLocked);
                pkg.WriteBoolean(itemAt.isGold);
                if (itemAt.isGold)
                {
                    pkg.WriteInt(itemAt.goldValidDate);
                    pkg.WriteDateTime(itemAt.goldBeginTime);
                }
                pkg.WriteString(itemAt.latentEnergyCurStr);
                pkg.WriteString(itemAt.latentEnergyCurStr);
                pkg.WriteDateTime(itemAt.latentEnergyEndTime);
            }
            SendTCP(pkg);
        }

        public void SendUpdateCardData(CardInventory bag, int[] updatedSlots)
        {
            if (m_gameClient.Player == null)
            {
                return;
            }
            GSPacketIn gSPacketIn = new GSPacketIn(216, m_gameClient.Player.PlayerCharacter.ID);
            gSPacketIn.WriteInt(m_gameClient.Player.PlayerCharacter.ID);
            gSPacketIn.WriteInt(updatedSlots.Length);
            foreach (int num in updatedSlots)
            {
                gSPacketIn.WriteInt(num);
                UsersCardInfo itemAt = bag.GetItemAt(num);
                if (itemAt != null && itemAt.TemplateID != 0)
                {
                    gSPacketIn.WriteBoolean(val: true);
                    gSPacketIn.WriteInt(itemAt.CardID);
                    gSPacketIn.WriteInt(itemAt.UserID);
                    gSPacketIn.WriteInt(itemAt.Count);
                    gSPacketIn.WriteInt(itemAt.Place);
                    gSPacketIn.WriteInt(itemAt.TemplateID);
                    gSPacketIn.WriteInt(itemAt.TotalAttack);
                    gSPacketIn.WriteInt(itemAt.TotalDefence);
                    gSPacketIn.WriteInt(itemAt.TotalAgility);
                    gSPacketIn.WriteInt(itemAt.TotalLuck);
                    gSPacketIn.WriteInt(itemAt.Damage);
                    gSPacketIn.WriteInt(itemAt.Guard);
                    gSPacketIn.WriteInt(itemAt.Level);
                    gSPacketIn.WriteInt(itemAt.CardGP);
                    gSPacketIn.WriteBoolean(itemAt.isFirstGet);
                }
                else
                {
                    gSPacketIn.WriteBoolean(val: false);
                }
            }
            SendTCP(gSPacketIn);
        }

        public void SendUpdateCardData(PlayerInfo player, List<UsersCardInfo> userCard)
        {
            if (m_gameClient.Player == null)
            {
                return;
            }
            GSPacketIn gSPacketIn = new GSPacketIn(216, player.ID);
            gSPacketIn.WriteInt(player.ID);
            gSPacketIn.WriteInt(userCard.Count);
            foreach (UsersCardInfo item in userCard)
            {
                gSPacketIn.WriteInt(item.Place);
                gSPacketIn.WriteBoolean(val: true);
                gSPacketIn.WriteInt(item.CardID);
                gSPacketIn.WriteInt(item.UserID);
                gSPacketIn.WriteInt(item.Count);
                gSPacketIn.WriteInt(item.Place);
                gSPacketIn.WriteInt(item.TemplateID);
                gSPacketIn.WriteInt(item.TotalAttack);
                gSPacketIn.WriteInt(item.TotalDefence);
                gSPacketIn.WriteInt(item.TotalAgility);
                gSPacketIn.WriteInt(item.TotalLuck);
                gSPacketIn.WriteInt(item.Damage);
                gSPacketIn.WriteInt(item.Guard);
                gSPacketIn.WriteInt(item.Level);
                gSPacketIn.WriteInt(item.CardGP);
                gSPacketIn.WriteBoolean(item.isFirstGet);
            }
            SendTCP(gSPacketIn);
        }

        public GSPacketIn SendUpdateQuests(GamePlayer player, byte[] states, BaseQuest[] infos)
        {
            if (player == null || states == null || infos == null)
            {
                return null;
            }
            GSPacketIn gSPacketIn = new GSPacketIn(178, player.PlayerCharacter.ID);
            gSPacketIn.WriteInt(infos.Length);
            foreach (BaseQuest baseQuest in infos)
            {
                gSPacketIn.WriteInt(baseQuest.Data.QuestID);
                gSPacketIn.WriteBoolean(baseQuest.Data.IsComplete);
                gSPacketIn.WriteInt(baseQuest.Data.Condition1);
                gSPacketIn.WriteInt(baseQuest.Data.Condition2);
                gSPacketIn.WriteInt(baseQuest.Data.Condition3);
                gSPacketIn.WriteInt(baseQuest.Data.Condition4);
                gSPacketIn.WriteDateTime(baseQuest.Data.CompletedDate.Date);
                gSPacketIn.WriteInt(baseQuest.Data.RepeatFinish);
                gSPacketIn.WriteInt(baseQuest.Data.RandDobule);
                gSPacketIn.WriteBoolean(baseQuest.Data.IsExist);
            }
            gSPacketIn.Write(states);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendUpdateBuffer(GamePlayer player, AbstractBuffer[] infos)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(185, player.PlayerId);
            gSPacketIn.WriteInt(infos.Length);
            foreach (AbstractBuffer abstractBuffer in infos)
            {
                gSPacketIn.WriteInt(abstractBuffer.Info.Type);
                gSPacketIn.WriteBoolean(abstractBuffer.Info.IsExist);
                gSPacketIn.WriteDateTime(abstractBuffer.Info.BeginDate);
                if (abstractBuffer.IsPayBuff())
                {
                    gSPacketIn.WriteInt(abstractBuffer.Info.ValidDate / 60 / 24);
                }
                else
                {
                    gSPacketIn.WriteInt(abstractBuffer.Info.ValidDate);
                }
                gSPacketIn.WriteInt(abstractBuffer.Info.Value);
                gSPacketIn.WriteInt(abstractBuffer.Info.ValidCount);
                gSPacketIn.WriteInt(abstractBuffer.Info.TemplateID);
            }
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }
        public GSPacketIn SendDiceActiveClose(int ID)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(134, ID);
            gSPacketIn.WriteByte(2);
            this.SendTCP(gSPacketIn);
            return gSPacketIn;
        }
        public GSPacketIn SendDiceReceiveData(PlayerDice Dice)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(134, Dice.Data.UserID);
            gSPacketIn.WriteByte(3);
            gSPacketIn.WriteBoolean(Dice.Data.UserFirstCell);
            gSPacketIn.WriteInt(Dice.Data.CurrentPosition);
            gSPacketIn.WriteInt(Dice.Data.LuckIntegralLevel);
            gSPacketIn.WriteInt(Dice.Data.LuckIntegral);
            gSPacketIn.WriteInt(Dice.Data.FreeCount);
            gSPacketIn.WriteInt(Dice.RewardItem.Count);
            for (int i = 0; i < Dice.RewardItem.Count; i++)
            {
                gSPacketIn.WriteInt(Dice.RewardItem[i].TemplateID);
                gSPacketIn.WriteInt(i);
                gSPacketIn.WriteInt(Dice.RewardItem[i].StrengthenLevel);
                gSPacketIn.WriteInt(Dice.RewardItem[i].Count);
                gSPacketIn.WriteInt(Dice.RewardItem[i].ValidDate);
                gSPacketIn.WriteBoolean(Dice.RewardItem[i].IsBinds);
            }
            this.SendTCP(gSPacketIn);
            return gSPacketIn;
        }
        public GSPacketIn SendDiceReceiveResult(PlayerDice Dice)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(134, Dice.Data.UserID);
            gSPacketIn.WriteByte(4);
            gSPacketIn.WriteInt(Dice.Data.CurrentPosition);
            gSPacketIn.WriteInt(Dice.result);
            gSPacketIn.WriteInt(Dice.Data.LuckIntegral);
            gSPacketIn.WriteInt(Dice.Data.Level);
            gSPacketIn.WriteInt(Dice.Data.FreeCount);
            gSPacketIn.WriteString(Dice.RewardName);
            this.SendTCP(gSPacketIn);
            return gSPacketIn;
        }
        public GSPacketIn SendDiceActiveOpen(PlayerDice Dice)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(134, Dice.Data.UserID);
            gSPacketIn.WriteByte(1);
            gSPacketIn.WriteInt(Dice.Data.FreeCount);
            gSPacketIn.WriteInt(Dice.refreshPrice);
            gSPacketIn.WriteInt(Dice.commonDicePrice);
            gSPacketIn.WriteInt(Dice.doubleDicePrice);
            gSPacketIn.WriteInt(Dice.bigDicePrice);
            gSPacketIn.WriteInt(Dice.smallDicePrice);
            gSPacketIn.WriteInt(Dice.MAX_LEVEL);
            for (int i = 0; i < Dice.MAX_LEVEL; i++)
            {
                List<DiceLevelAwardInfo> list = Dice.LevelAward[i];
                gSPacketIn.WriteInt(Dice.IntegralPoint[i]);
                gSPacketIn.WriteInt(list.Count);
                for (int j = 0; j < list.Count; j++)
                {
                    gSPacketIn.WriteInt(list[j].TemplateID);
                    gSPacketIn.WriteInt(list[j].Count);
                }
            }
            this.SendTCP(gSPacketIn);
            return gSPacketIn;
        }
        public GSPacketIn SendBufferList(GamePlayer player, List<AbstractBuffer> infos)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(186, player.PlayerId);
            gSPacketIn.WriteInt(infos.Count);
            foreach (AbstractBuffer info2 in infos)
            {
                BufferInfo info = info2.Info;
                gSPacketIn.WriteInt(info.Type);
                gSPacketIn.WriteBoolean(info.IsExist);
                gSPacketIn.WriteDateTime(info.BeginDate);
                if (info2.IsPayBuff())
                {
                    gSPacketIn.WriteInt(info.ValidDate / 60 / 24);
                }
                else
                {
                    gSPacketIn.WriteInt(info.ValidDate);
                }
                gSPacketIn.WriteInt(info.Value);
                gSPacketIn.WriteInt(info.ValidCount);
                gSPacketIn.WriteInt(info.TemplateID);
            }
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendMailResponse(int playerID, eMailRespose type)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(117);
            gSPacketIn.WriteInt(playerID);
            gSPacketIn.WriteInt((int)type);
            GameServer.Instance.LoginServer.SendPacket(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendConsortiaLevelUp(byte type, byte level, bool result, string msg, int playerid)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(159, playerid);
            gSPacketIn.WriteByte(type);
            gSPacketIn.WriteByte(level);
            gSPacketIn.WriteBoolean(result);
            gSPacketIn.WriteString(msg);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendAuctionRefresh(AuctionInfo info, int auctionID, bool isExist, ItemInfo item)
        {
            GSPacketIn pkg = new GSPacketIn((int)ePackageType.AUCTION_REFRESH);
            pkg.WriteInt(auctionID);
            pkg.WriteBoolean(isExist);
            if (isExist)
            {
                pkg.WriteInt(info.AuctioneerID);
                pkg.WriteString(info.AuctioneerName);
                pkg.WriteDateTime(info.BeginDate);
                pkg.WriteInt(info.BuyerID);
                pkg.WriteString(info.BuyerName);
                pkg.WriteInt(info.ItemID);
                pkg.WriteInt(info.Mouthful);
                pkg.WriteInt(info.PayType);
                pkg.WriteInt(info.Price);
                pkg.WriteInt(info.Rise);
                pkg.WriteInt(info.ValidDate);
                pkg.WriteBoolean(item != null);
                if (item != null)
                {
                    pkg.WriteInt(item.Count);
                    pkg.WriteInt(item.TemplateID);
                    pkg.WriteInt(item.AttackCompose);
                    pkg.WriteInt(item.DefendCompose);
                    pkg.WriteInt(item.AgilityCompose);
                    pkg.WriteInt(item.LuckCompose);
                    pkg.WriteInt(item.StrengthenLevel);
                    pkg.WriteBoolean(item.IsBinds);
                    pkg.WriteBoolean(item.IsJudge);
                    pkg.WriteDateTime(item.BeginDate);
                    pkg.WriteInt(item.ValidDate);
                    pkg.WriteString(item.Color);
                    pkg.WriteString(item.Skin);
                    pkg.WriteBoolean(item.IsUsed);
                    pkg.WriteInt(item.Hole1);
                    pkg.WriteInt(item.Hole2);
                    pkg.WriteInt(item.Hole3);
                    pkg.WriteInt(item.Hole4);
                    pkg.WriteInt(item.Hole5);
                    pkg.WriteInt(item.Hole6);
                    pkg.WriteString(item.Pic);
                    pkg.WriteInt(item.RefineryLevel);
                    pkg.WriteDateTime(DateTime.Now);
                    pkg.WriteByte((byte)item.Hole5Level);
                    pkg.WriteInt(item.Hole5Exp);
                    pkg.WriteByte((byte)item.Hole6Level);
                    pkg.WriteInt(item.Hole6Exp);
                    pkg.WriteInt(item.Hole6Exp);
                    pkg.WriteInt(item.curExp);
                }
            }
            pkg.Compress();
            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendAASState(bool result)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(224);
            gSPacketIn.WriteBoolean(result);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendIDNumberCheck(bool result)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(226);
            gSPacketIn.WriteBoolean(result);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendAASInfoSet(bool result)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(224);
            gSPacketIn.WriteBoolean(result);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendAASControl(bool result, bool bool_0, bool IsMinor)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(227);
            gSPacketIn.WriteBoolean(val: true);
            gSPacketIn.WriteInt(1);
            gSPacketIn.WriteBoolean(val: true);
            gSPacketIn.WriteBoolean(IsMinor);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendMarryRoomInfo(GamePlayer player, MarryRoom room)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(241, player.PlayerCharacter.ID);
            bool flag = room != null;
            gSPacketIn.WriteBoolean(flag);
            if (flag)
            {
                gSPacketIn.WriteInt(room.Info.ID);
                gSPacketIn.WriteBoolean(room.Info.IsHymeneal);
                gSPacketIn.WriteString(room.Info.Name);
                gSPacketIn.WriteBoolean(!(room.Info.Pwd == ""));
                gSPacketIn.WriteInt(room.Info.MapIndex);
                gSPacketIn.WriteInt(room.Info.AvailTime);
                gSPacketIn.WriteInt(room.Count);
                gSPacketIn.WriteInt(room.Info.PlayerID);
                gSPacketIn.WriteString(room.Info.PlayerName);
                gSPacketIn.WriteInt(room.Info.GroomID);
                gSPacketIn.WriteString(room.Info.GroomName);
                gSPacketIn.WriteInt(room.Info.BrideID);
                gSPacketIn.WriteString(room.Info.BrideName);
                gSPacketIn.WriteDateTime(room.Info.BeginTime);
                gSPacketIn.WriteByte((byte)room.RoomState);
                gSPacketIn.WriteString(room.Info.RoomIntroduction);
            }
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendMarryRoomLogin(GamePlayer player, bool result)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(242, player.PlayerCharacter.ID);
            gSPacketIn.WriteBoolean(result);
            if (result)
            {
                gSPacketIn.WriteInt(player.CurrentMarryRoom.Info.ID);
                gSPacketIn.WriteString(player.CurrentMarryRoom.Info.Name);
                gSPacketIn.WriteInt(player.CurrentMarryRoom.Info.MapIndex);
                gSPacketIn.WriteInt(player.CurrentMarryRoom.Info.AvailTime);
                gSPacketIn.WriteInt(player.CurrentMarryRoom.Count);
                gSPacketIn.WriteInt(player.CurrentMarryRoom.Info.PlayerID);
                gSPacketIn.WriteString(player.CurrentMarryRoom.Info.PlayerName);
                gSPacketIn.WriteInt(player.CurrentMarryRoom.Info.GroomID);
                gSPacketIn.WriteString(player.CurrentMarryRoom.Info.GroomName);
                gSPacketIn.WriteInt(player.CurrentMarryRoom.Info.BrideID);
                gSPacketIn.WriteString(player.CurrentMarryRoom.Info.BrideName);
                gSPacketIn.WriteDateTime(player.CurrentMarryRoom.Info.BeginTime);
                gSPacketIn.WriteBoolean(player.CurrentMarryRoom.Info.IsHymeneal);
                gSPacketIn.WriteByte((byte)player.CurrentMarryRoom.RoomState);
                gSPacketIn.WriteString(player.CurrentMarryRoom.Info.RoomIntroduction);
                gSPacketIn.WriteBoolean(player.CurrentMarryRoom.Info.GuestInvite);
                gSPacketIn.WriteInt(player.MarryMap);
                gSPacketIn.WriteBoolean(player.CurrentMarryRoom.Info.IsGunsaluteUsed);
            }
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendPlayerEnterMarryRoom(GamePlayer player)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(243, player.PlayerCharacter.ID);
            gSPacketIn.WriteInt(player.PlayerCharacter.Grade);
            gSPacketIn.WriteInt(player.PlayerCharacter.Hide);
            gSPacketIn.WriteInt(player.PlayerCharacter.Repute);
            gSPacketIn.WriteInt(player.PlayerCharacter.ID);
            gSPacketIn.WriteString(player.PlayerCharacter.NickName);
            gSPacketIn.WriteByte(player.PlayerCharacter.typeVIP);
            gSPacketIn.WriteInt(player.PlayerCharacter.VIPLevel);
            gSPacketIn.WriteBoolean(player.PlayerCharacter.Sex);
            gSPacketIn.WriteString(player.PlayerCharacter.Style);
            gSPacketIn.WriteString(player.PlayerCharacter.Colors);
            gSPacketIn.WriteString(player.PlayerCharacter.Skin);
            gSPacketIn.WriteInt(player.X);
            gSPacketIn.WriteInt(player.Y);
            gSPacketIn.WriteInt(player.PlayerCharacter.FightPower);
            gSPacketIn.WriteInt(player.PlayerCharacter.Win);
            gSPacketIn.WriteInt(player.PlayerCharacter.Total);
            gSPacketIn.WriteInt(player.PlayerCharacter.Offer);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendMarryInfoRefresh(MarryInfo info, int ID, bool isExist)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(239);
            gSPacketIn.WriteInt(ID);
            gSPacketIn.WriteBoolean(isExist);
            if (isExist)
            {
                gSPacketIn.WriteInt(info.UserID);
                gSPacketIn.WriteBoolean(info.IsPublishEquip);
                gSPacketIn.WriteString(info.Introduction);
            }
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendPlayerMarryStatus(GamePlayer player, int userID, bool isMarried)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(246, player.PlayerCharacter.ID);
            gSPacketIn.WriteInt(userID);
            gSPacketIn.WriteBoolean(isMarried);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendPlayerMarryApply(GamePlayer player, int userID, string userName, string loveProclamation, int id)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(247, player.PlayerCharacter.ID);
            gSPacketIn.WriteInt(userID);
            gSPacketIn.WriteString(userName);
            gSPacketIn.WriteString(loveProclamation);
            gSPacketIn.WriteInt(id);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendPlayerDivorceApply(GamePlayer player, bool result, bool isProposer)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(248, player.PlayerCharacter.ID);
            gSPacketIn.WriteBoolean(result);
            gSPacketIn.WriteBoolean(isProposer);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendMarryApplyReply(GamePlayer player, int UserID, string UserName, bool result, bool isApplicant, int id)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(250, player.PlayerCharacter.ID);
            gSPacketIn.WriteInt(UserID);
            gSPacketIn.WriteBoolean(result);
            gSPacketIn.WriteString(UserName);
            gSPacketIn.WriteBoolean(isApplicant);
            gSPacketIn.WriteInt(id);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendBigSpeakerMsg(GamePlayer player, string msg)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(72, player.PlayerCharacter.ID);
            gSPacketIn.WriteInt(player.PlayerCharacter.ID);
            gSPacketIn.WriteString(player.PlayerCharacter.NickName);
            gSPacketIn.WriteString(msg);
            GameServer.Instance.LoginServer.SendPacket(gSPacketIn);
            GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
            for (int i = 0; i < allPlayers.Length; i++)
            {
                allPlayers[i].Out.SendTCP(gSPacketIn);
            }
            return gSPacketIn;
        }

        public GSPacketIn SendPlayerLeaveMarryRoom(GamePlayer player)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(244, player.PlayerCharacter.ID);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendMarryRoomInfoToPlayer(GamePlayer player, bool state, MarryRoomInfo info)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(252, player.PlayerCharacter.ID);
            gSPacketIn.WriteBoolean(state);
            if (state)
            {
                gSPacketIn.WriteInt(info.ID);
                gSPacketIn.WriteString(info.Name);
                gSPacketIn.WriteInt(info.MapIndex);
                gSPacketIn.WriteInt(info.AvailTime);
                gSPacketIn.WriteInt(info.PlayerID);
                gSPacketIn.WriteInt(info.GroomID);
                gSPacketIn.WriteInt(info.BrideID);
                gSPacketIn.WriteDateTime(info.BeginTime);
                gSPacketIn.WriteBoolean(info.IsGunsaluteUsed);
            }
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendMarryInfo(GamePlayer player, MarryInfo info)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(235, player.PlayerCharacter.ID);
            gSPacketIn.WriteString(info.Introduction);
            gSPacketIn.WriteBoolean(info.IsPublishEquip);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendContinuation(GamePlayer player, MarryRoomInfo info)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(249, player.PlayerCharacter.ID);
            gSPacketIn.WriteByte(3);
            gSPacketIn.WriteInt(info.AvailTime);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendMarryProp(GamePlayer player, MarryProp info)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(234, player.PlayerCharacter.ID);
            gSPacketIn.WriteBoolean(info.IsMarried);
            gSPacketIn.WriteInt(info.SpouseID);
            gSPacketIn.WriteString(info.SpouseName);
            gSPacketIn.WriteBoolean(info.IsCreatedMarryRoom);
            gSPacketIn.WriteInt(info.SelfMarryRoomID);
            gSPacketIn.WriteBoolean(info.IsGotRing);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public void SendWeaklessGuildProgress(PlayerInfo player)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(15, player.ID);
            gSPacketIn.WriteInt(player.weaklessGuildProgress.Length);
            for (int i = 0; i < player.weaklessGuildProgress.Length; i++)
            {
                gSPacketIn.WriteByte(player.weaklessGuildProgress[i]);
            }
            SendTCP(gSPacketIn);
        }

        public void SendUserLuckyNum()
        {
            GSPacketIn gSPacketIn = new GSPacketIn(161);
            gSPacketIn.WriteInt(1);
            gSPacketIn.WriteString("");
            SendTCP(gSPacketIn);
        }

        public GSPacketIn SendUserRanks(int Id, List<UserRankInfo> ranks)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(34, Id);
            gSPacketIn.WriteInt(ranks.Count);
            foreach (UserRankInfo rank in ranks)
            {
                gSPacketIn.WriteInt(rank.NewTitleID);
                gSPacketIn.WriteString(rank.Name);
                gSPacketIn.WriteDateTime(rank.BeginDate);
                if (rank.Validate == 0)
                {
                    gSPacketIn.WriteDateTime(DateTime.Now.AddYears(1));
                }
                else
                {
                    gSPacketIn.WriteDateTime(rank.BeginDate.AddDays((double)rank.Validate));
                }
            }
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendUpdatePlayerProperty(PlayerInfo info, PlayerProperty PlayerProp)
        {
            string[] array = new string[4]
            {
                "Attack",
                "Defence",
                "Agility",
                "Luck"
            };
            GSPacketIn gSPacketIn = new GSPacketIn(167, info.ID);
            gSPacketIn.WriteInt(m_gameClient.Player.PlayerCharacter.ID);
            string[] array2 = array;
            string[] array3 = array2;
            foreach (string key in array3)
            {
                gSPacketIn.WriteInt(0);
                gSPacketIn.WriteInt(PlayerProp.Current["Texp"][key]);
                gSPacketIn.WriteInt(PlayerProp.Current["Card"][key]);
                gSPacketIn.WriteInt(PlayerProp.Current["Suit"][key]);
            }
            gSPacketIn.WriteInt(0);
            gSPacketIn.WriteInt(0);
            gSPacketIn.WriteInt(PlayerProp.Current["Damage"]["Bead"]);
            gSPacketIn.WriteInt(PlayerProp.Current["Armor"]["Bead"]);
            gSPacketIn.WriteInt(PlayerProp.Current["Damage"]["Suit"]);
            gSPacketIn.WriteInt(PlayerProp.Current["Armor"]["Suit"]);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendOpenVIP(GamePlayer Player)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(92, Player.PlayerCharacter.ID);
            gSPacketIn.WriteByte(Player.PlayerCharacter.typeVIP);
            gSPacketIn.WriteInt(Player.PlayerCharacter.VIPLevel);
            gSPacketIn.WriteInt(Player.PlayerCharacter.VIPExp);
            gSPacketIn.WriteDateTime(Player.PlayerCharacter.VIPExpireDay);
            gSPacketIn.WriteDateTime(Player.PlayerCharacter.LastDate);
            gSPacketIn.WriteInt(Player.PlayerCharacter.VIPNextLevelDaysNeeded);
            gSPacketIn.WriteBoolean(Player.PlayerCharacter.CanTakeVipReward);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendEnterHotSpringRoom(GamePlayer player)
        {
            if (player.CurrentHotSpringRoom == null)
            {
                return null;
            }
            GSPacketIn gSPacketIn = new GSPacketIn(202, player.PlayerCharacter.ID);
            gSPacketIn.WriteInt(player.CurrentHotSpringRoom.Info.roomID);
            gSPacketIn.WriteInt(player.CurrentHotSpringRoom.Info.roomNumber);
            gSPacketIn.WriteString(player.CurrentHotSpringRoom.Info.roomName);
            gSPacketIn.WriteString(player.CurrentHotSpringRoom.Info.roomPassword);
            gSPacketIn.WriteInt(player.CurrentHotSpringRoom.Info.effectiveTime);
            gSPacketIn.WriteInt(player.CurrentHotSpringRoom.Count);
            gSPacketIn.WriteInt(player.CurrentHotSpringRoom.Info.playerID);
            gSPacketIn.WriteString(player.CurrentHotSpringRoom.Info.playerName);
            gSPacketIn.WriteDateTime(player.CurrentHotSpringRoom.Info.startTime);
            gSPacketIn.WriteString(player.CurrentHotSpringRoom.Info.roomIntroduction);
            gSPacketIn.WriteInt(player.CurrentHotSpringRoom.Info.roomType);
            gSPacketIn.WriteInt(player.CurrentHotSpringRoom.Info.maxCount);
            gSPacketIn.WriteDateTime(player.Extra.Info.LastTimeHotSpring);
            gSPacketIn.WriteInt(player.Extra.Info.MinHotSpring);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendHotSpringUpdateTime(GamePlayer player, int expAdd)
        {
            if (player.CurrentHotSpringRoom == null)
            {
                return null;
            }
            GSPacketIn gSPacketIn = new GSPacketIn(191, player.PlayerCharacter.ID);
            gSPacketIn.WriteByte(7);
            gSPacketIn.WriteInt(player.Extra.Info.MinHotSpring);
            gSPacketIn.WriteInt(expAdd);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendGetUserGift(PlayerInfo player, UserGiftInfo[] allGifts)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(218);
            gSPacketIn.WriteInt(player.ID);
            gSPacketIn.WriteInt(player.charmGP);
            gSPacketIn.WriteInt(allGifts.Length);
            foreach (UserGiftInfo userGiftInfo in allGifts)
            {
                gSPacketIn.WriteInt(userGiftInfo.TemplateID);
                gSPacketIn.WriteInt(userGiftInfo.Count);
            }
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendEatPetsInfo(EatPetsInfo info)
        {
            if (info == null)
            {
                return null;
            }
            GSPacketIn gSPacketIn = new GSPacketIn(68, m_gameClient.Player.PlayerId);
            gSPacketIn.WriteByte(33);
            gSPacketIn.WriteInt(info.weaponExp);
            gSPacketIn.WriteInt(info.weaponLevel);
            gSPacketIn.WriteInt(info.clothesExp);
            gSPacketIn.WriteInt(info.clothesLevel);
            gSPacketIn.WriteInt(info.hatExp);
            gSPacketIn.WriteInt(info.hatLevel);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendRefreshPet(GamePlayer player, UsersPetInfo[] pets, ItemInfo[] items, bool refreshBtn)
        {
            if (pets.Length == 0)
            {
                return null;
            }
            int val = 10;
            int val2 = 10;
            int val3 = 100;
            GSPacketIn gSPacketIn = new GSPacketIn(68, player.PlayerCharacter.ID);
            gSPacketIn.WriteByte(5);
            gSPacketIn.WriteBoolean(refreshBtn);
            gSPacketIn.WriteInt(pets.Length);
            foreach (UsersPetInfo usersPetInfo in pets)
            {
                gSPacketIn.WriteInt(usersPetInfo.Place);
                gSPacketIn.WriteInt(usersPetInfo.TemplateID);
                gSPacketIn.WriteString(usersPetInfo.Name);
                gSPacketIn.WriteInt(usersPetInfo.Attack);
                gSPacketIn.WriteInt(usersPetInfo.Defence);
                gSPacketIn.WriteInt(usersPetInfo.Luck);
                gSPacketIn.WriteInt(usersPetInfo.Agility);
                gSPacketIn.WriteInt(usersPetInfo.Blood);
                gSPacketIn.WriteInt(usersPetInfo.Damage);
                gSPacketIn.WriteInt(usersPetInfo.Guard);
                gSPacketIn.WriteInt(usersPetInfo.AttackGrow);
                gSPacketIn.WriteInt(usersPetInfo.DefenceGrow);
                gSPacketIn.WriteInt(usersPetInfo.LuckGrow);
                gSPacketIn.WriteInt(usersPetInfo.AgilityGrow);
                gSPacketIn.WriteInt(usersPetInfo.BloodGrow);
                gSPacketIn.WriteInt(usersPetInfo.DamageGrow);
                gSPacketIn.WriteInt(usersPetInfo.GuardGrow);
                gSPacketIn.WriteInt(usersPetInfo.Level);
                gSPacketIn.WriteInt(usersPetInfo.GP);
                gSPacketIn.WriteInt(usersPetInfo.MaxGP);
                gSPacketIn.WriteInt(usersPetInfo.Hunger);
                gSPacketIn.WriteInt(usersPetInfo.MP);
                string[] array = usersPetInfo.Skill.Split('|');
                gSPacketIn.WriteInt(array.Length);
                string[] array2 = array;
                foreach (string text in array2)
                {
                    gSPacketIn.WriteInt(int.Parse(text.Split(',')[0]));
                    gSPacketIn.WriteInt(int.Parse(text.Split(',')[1]));
                }
                gSPacketIn.WriteInt(val);
                gSPacketIn.WriteInt(val2);
                gSPacketIn.WriteInt(val3);
            }
            if (!player.PlayerCharacter.IsFistGetPet)
            {
                gSPacketIn.WriteInt(0);
            }
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public void SendLeagueNotice(int id, int restCount, int maxCount, byte type)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.LEAGUE_START_NOTICE, id);
            pkg.WriteByte(type);
            if (type == 1)
            {
                pkg.WriteInt(restCount);
                pkg.WriteInt(maxCount);
            }
            else
            {
                pkg.WriteInt(restCount);
            }
            SendTCP(pkg);
        }

        public GSPacketIn SendEnterFarm(PlayerInfo Player, UserFarmInfo farm, UserFieldInfo[] fields)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(81, Player.ID);
            gSPacketIn.WriteByte(1);
            gSPacketIn.WriteInt(farm.FarmID);
            gSPacketIn.WriteBoolean(farm.isFarmHelper);
            gSPacketIn.WriteInt(farm.isAutoId);
            gSPacketIn.WriteDateTime(farm.AutoPayTime);
            gSPacketIn.WriteInt(farm.AutoValidDate);
            gSPacketIn.WriteInt(farm.GainFieldId);
            gSPacketIn.WriteInt(farm.KillCropId);
            gSPacketIn.WriteInt(fields.Length);
            foreach (UserFieldInfo userFieldInfo in fields)
            {
                gSPacketIn.WriteInt(userFieldInfo.FieldID);
                gSPacketIn.WriteInt(userFieldInfo.SeedID);
                gSPacketIn.WriteDateTime(userFieldInfo.PayTime);
                gSPacketIn.WriteDateTime(userFieldInfo.PlantTime);
                gSPacketIn.WriteInt(userFieldInfo.GainCount);
                gSPacketIn.WriteInt(userFieldInfo.FieldValidDate);
                gSPacketIn.WriteInt(userFieldInfo.AccelerateTime);
            }
            if (farm.FarmID == Player.ID)
            {
                gSPacketIn.WriteInt(GameProperties.FastGrowNeedMoney);
                gSPacketIn.WriteString(farm.PayFieldMoney);
                gSPacketIn.WriteString(farm.PayAutoMoney);
                gSPacketIn.WriteDateTime(farm.AutoPayTime);
                gSPacketIn.WriteInt(farm.AutoValidDate);
                gSPacketIn.WriteInt(Player.VIPLevel);
                gSPacketIn.WriteInt(farm.buyExpRemainNum);
            }
            else
            {
                gSPacketIn.WriteBoolean(farm.isArrange);
            }
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendSeeding(PlayerInfo Player, UserFieldInfo field)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(81, Player.ID);
            gSPacketIn.WriteByte(2);
            gSPacketIn.WriteInt(field.FieldID);
            gSPacketIn.WriteInt(field.SeedID);
            gSPacketIn.WriteDateTime(field.PlantTime);
            gSPacketIn.WriteDateTime(field.PayTime);
            gSPacketIn.WriteInt(field.GainCount);
            gSPacketIn.WriteInt(field.FieldValidDate);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SenddoMature(PlayerFarm farm)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(81, farm.Player.PlayerCharacter.ID);
            gSPacketIn.WriteByte(3);
            gSPacketIn.WriteInt(farm.CurrentFields.Length);
            UserFieldInfo[] currentFields = farm.CurrentFields;
            foreach (UserFieldInfo userFieldInfo in currentFields)
            {
                if (userFieldInfo != null)
                {
                    gSPacketIn.WriteBoolean(val: true);
                    gSPacketIn.WriteInt(userFieldInfo.FieldID);
                    gSPacketIn.WriteInt(userFieldInfo.GainCount);
                    gSPacketIn.WriteInt(userFieldInfo.AccelerateTime);
                }
                else
                {
                    gSPacketIn.WriteBoolean(val: false);
                }
            }
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendtoGather(PlayerInfo Player, UserFieldInfo field)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(81, Player.ID);
            gSPacketIn.WriteByte(4);
            gSPacketIn.WriteBoolean(val: true);
            gSPacketIn.WriteInt(field.FieldID);
            gSPacketIn.WriteInt(field.SeedID);
            gSPacketIn.WriteDateTime(field.PlantTime);
            gSPacketIn.WriteInt(field.GainCount);
            gSPacketIn.WriteInt(field.AccelerateTime);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendCompose(GamePlayer Player)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(81, Player.PlayerCharacter.ID);
            gSPacketIn.WriteByte(5);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendPayFields(GamePlayer Player, List<int> fieldIds)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(81, Player.PlayerCharacter.ID);
            gSPacketIn.WriteByte(6);
            gSPacketIn.WriteInt(Player.PlayerCharacter.ID);
            gSPacketIn.WriteInt(fieldIds.Count);
            foreach (int fieldId in fieldIds)
            {
                UserFieldInfo fieldAt = Player.Farm.GetFieldAt(fieldId);
                gSPacketIn.WriteInt(fieldAt.FieldID);
                gSPacketIn.WriteInt(fieldAt.SeedID);
                gSPacketIn.WriteDateTime(fieldAt.PayTime);
                gSPacketIn.WriteDateTime(fieldAt.PlantTime);
                gSPacketIn.WriteInt(fieldAt.GainCount);
                gSPacketIn.WriteInt(fieldAt.FieldValidDate);
                gSPacketIn.WriteInt(fieldAt.AccelerateTime);
            }
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendKillCropField(PlayerInfo Player, UserFieldInfo field)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(81, Player.ID);
            gSPacketIn.WriteByte(7);
            gSPacketIn.WriteBoolean(val: true);
            gSPacketIn.WriteInt(field.FieldID);
            gSPacketIn.WriteInt(field.SeedID);
            gSPacketIn.WriteInt(field.AccelerateTime);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendHelperSwitchField(PlayerInfo Player, UserFarmInfo farm)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(81, Player.ID);
            gSPacketIn.WriteByte(9);
            gSPacketIn.WriteBoolean(farm.isFarmHelper);
            gSPacketIn.WriteInt(farm.isAutoId);
            gSPacketIn.WriteDateTime(farm.AutoPayTime);
            gSPacketIn.WriteInt(farm.AutoValidDate);
            gSPacketIn.WriteInt(farm.GainFieldId);
            gSPacketIn.WriteInt(farm.KillCropId);
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }
    

        public GSPacketIn SendChickenBoxOpen(int ID, int flushPrice, int[] openCardPrice, int[] eagleEyePrice)
        {
            GSPacketIn gSPacketIn = new GSPacketIn((byte)ePackageType.NEWCHICKENBOX_SYS, ID);
            gSPacketIn.WriteInt(1);
            gSPacketIn.WriteInt(openCardPrice.Length);
            for (int num = openCardPrice.Length; num > 0; num--)
            {
                gSPacketIn.WriteInt(openCardPrice[num - 1]);
            }
            gSPacketIn.WriteInt(eagleEyePrice.Length);
            for (int num2 = eagleEyePrice.Length; num2 > 0; num2--)
            {
                gSPacketIn.WriteInt(eagleEyePrice[num2 - 1]);
            }
            gSPacketIn.WriteInt(flushPrice);
            gSPacketIn.WriteDateTime(DateTime.Parse(GameProperties.NewChickenEndTime));
            SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        public GSPacketIn SendLuckStarOpen(int ID)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.NEWCHICKENBOX_SYS, ID);
            pkg.WriteInt((int)NewChickenBoxPackageType.ACTIVITY_OPEN);
            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendPlayerFigSpiritinit(int ID, List<UserGemStone> gems)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.FIGHT_SPIRIT, ID);
            pkg.WriteByte((byte)FightSpiritPackageType.FIGHT_SPIRIT_INIT);
            pkg.WriteBoolean(true);
            pkg.WriteInt(gems.Count);
            foreach (UserGemStone current in gems)
            {
                pkg.WriteInt(current.UserID);
                pkg.WriteInt(current.FigSpiritId);
                pkg.WriteString(current.FigSpiritIdValue);
                pkg.WriteInt(current.EquipPlace);
            }
            this.SendTCP(pkg);
            return pkg;
        }

        //public GSPacketIn SendPlayerFigSpiritUp(int ID, UserGemStone gem, bool isUp, bool isMaxLevel, bool isFall, int num, int dir)
        //{
        //    GSPacketIn pkg = new GSPacketIn((byte)ePackageType.FIGHT_SPIRIT, ID);
        //    pkg.WriteByte((byte)FightSpiritPackageType.PLAYER_FIGHT_SPIRIT_UP);
        //    string[] array = gem.FigSpiritIdValue.Split(new char[]
        //    {
        //        '|'
        //    });
        //    pkg.WriteBoolean(isUp);
        //    pkg.WriteBoolean(isMaxLevel);
        //    pkg.WriteBoolean(isFall);
        //    pkg.WriteInt(num);
        //    pkg.WriteInt(array.Length);
        //    for (int i = 0; i < array.Length; i++)
        //    {
        //        string text = array[i];
        //        pkg.WriteInt(gem.FigSpiritId);
        //        pkg.WriteInt(Convert.ToInt32(text.Split(new char[]
        //        {
        //            ','
        //        })[0]));
        //        pkg.WriteInt(Convert.ToInt32(text.Split(new char[]
        //        {
        //            ','
        //        })[1]));
        //        pkg.WriteInt(Convert.ToInt32(text.Split(new char[]
        //        {
        //            ','
        //        })[2]));
        //    }
        //    pkg.WriteInt(gem.EquipPlace);
        //    pkg.WriteInt(dir);
        //    this.SendTCP(pkg);
        //    return pkg;
        //}

        public GSPacketIn SendPlayerFigSpiritUp(int ID, UserGemStone gem, bool isUp, bool isMaxLevel, bool isFall, int num, int dir)
        {
            //GSPacketIn gSPacketIn = new GSPacketIn(209, ID);
            GSPacketIn packet = new GSPacketIn((byte)ePackageType.FIGHT_SPIRIT, ID);
            packet.WriteByte((byte)FightSpiritPackageType.PLAYER_FIGHT_SPIRIT_UP);
            string[] strArrays = gem.FigSpiritIdValue.Split(new char[] { '|' });
            packet.WriteBoolean(isUp);
            packet.WriteBoolean(isMaxLevel);
            packet.WriteBoolean(isFall);
            packet.WriteInt(num);
            packet.WriteInt((int)strArrays.Length);
            for (int i = 0; i < (int)strArrays.Length; i++)
            {
                string str = strArrays[i];
                packet.WriteInt(gem.FigSpiritId);
                packet.WriteInt(Convert.ToInt32(str.Split(new char[] { ',' })[0]));
                packet.WriteInt(Convert.ToInt32(str.Split(new char[] { ',' })[1]));
                packet.WriteInt(Convert.ToInt32(str.Split(new char[] { ',' })[2]));
            }
            packet.WriteInt(gem.EquipPlace);
            packet.WriteInt(dir);
            this.SendTCP(packet);
            return packet;
        }

        public GSPacketIn SendAvatarCollect(PlayerAvatarCollection avtCollect)
        {
            GSPacketIn gSPacketIn = new GSPacketIn(402);
            gSPacketIn.WriteByte(5);
            gSPacketIn.WriteInt(avtCollect.AvatarCollect.Count);
            if (avtCollect.AvatarCollect.Count > 0)
            {
                foreach (UserAvatarCollectionInfo current in avtCollect.AvatarCollect)
                {
                    gSPacketIn.WriteInt(current.AvatarID);
                    gSPacketIn.WriteInt(current.Sex);
                    if (current.Items == null)
                    {
                        current.UpdateItems();
                    }
                    gSPacketIn.WriteInt(current.Items.Count);
                    if (current.Items.Count > 0)
                    {
                        foreach (UserAvatarCollectionDataInfo current2 in current.Items)
                        {
                            gSPacketIn.WriteInt(current2.TemplateID);
                        }
                    }
                    gSPacketIn.WriteDateTime(current.TimeEnd);
                }
            }
            this.SendTCP(gSPacketIn);
            return gSPacketIn;
        }

        #region WorldBoss

        public GSPacketIn SendNewPacket(GamePlayer Player)
        {
            GSPacketIn packet = new GSPacketIn((byte)ePackageType.WORLDBOSS_CMD, Player.PlayerCharacter.ID);
            packet.WriteByte((byte)WorldBossPackageType.OPEN);

            SendTCP(packet);
            return packet;
        }

        public void SendOpenWorldBoss(int pX, int pY)
        {
            BaseWorldBossRoom world = RoomMgr.WorldBossRoom;
            GSPacketIn packet = new GSPacketIn((byte)ePackageType.WORLDBOSS_CMD);
            packet.WriteByte((byte)WorldBossPackageType.OPEN);
            packet.WriteString(world.BossResourceId); //this._bossResourceId = event.pkg.readUTF();1:rong 2:dracula 4:Captain
            packet.WriteInt(world.CurrentPve); //_currentPVE_ID = event.pkg.readInt();1243 30002 30001 30004
            packet.WriteString("BOSS"); //event.pkg.readUTF();
            packet.WriteString(world.Name); //_bossInfo.name = event.pkg.readUTF();
            packet.WriteInt(world.MaxBlood); //_bossInfo.total_Blood = event.pkg.readLong(); 100bilion
            packet.WriteInt(0); //var _loc_2:* = event.pkg.readInt();
            packet.WriteInt(0); //var _loc_3:* = event.pkg.readInt();
            packet.WriteInt(1); //_bossInfo.playerDefaultPos = new Point(event.pkg.readInt(), event.pkg.readInt());
                                //for
            packet.WriteInt(pX == 0 ? world.PlayerDefaultPosX : pX);
            packet.WriteInt(pY == 0 ? world.PlayerDefaultPosY : pY);
            //}
            packet.WriteDateTime(world.BeginTime); //_bossInfo.begin_time = event.pkg.readDate();
            packet.WriteDateTime(world.EndTime); //_bossInfo.end_time = event.pkg.readDate();

            packet.WriteInt(world.FightTime); //_bossInfo.fight_time = event.pkg.readInt();
            packet.WriteBoolean(world.FightOver); //_bossInfo.fightOver = event.pkg.readBoolean();
            packet.WriteBoolean(world.RoomClose); //_bossInfo.roomClose = event.pkg.readBoolean();
            packet.WriteInt(world.TicketId); //_bossInfo.ticketID = event.pkg.readInt();
            packet.WriteInt(world.NeedTicketCount); //_bossInfo.need_ticket_count = event.pkg.readInt();
            packet.WriteInt(world.TimeCd); //_bossInfo.timeCD = event.pkg.readInt();
            packet.WriteInt(world.ReviveMoney); //_bossInfo.reviveMoney = event.pkg.readInt();
            //packet.WriteInt(world.ReFightMoney); //_bossInfo.reFightMoney = event.pkg.readInt();
            //packet.WriteInt(world.addInjureBuffMoney); //_bossInfo.addInjureBuffMoney = event.pkg.readInt();
            //packet.WriteInt(world.addInjureValue); //_bossInfo.addInjureValue = event.pkg.readInt();
            packet.WriteInt(1); //var _loc_4:* = event.pkg.readInt();
                                //while (_loc_5 < _loc_4)
                                //{  
            packet.WriteInt(1);    //_loc_6.ID = event.pkg.readInt();
            packet.WriteString("Otomatik Buff");    //_loc_6.name = event.pkg.readUTF();
            packet.WriteInt(30);    //_loc_6.price = event.pkg.readInt();
            packet.WriteString("30 Kupona 1 kez satın al ve mükemmel hasar ver.");    //_loc_6.decription = event.pkg.readUTF();  
            packet.WriteInt(-1);//_loc_8.costID = event.pkg.readInt();
                                //}
                                //foreach (var item in WorldBossBuffInfo)
                                //{
                                //    packet.WriteInt(ID);
                                //    packet.WriteString(name);
                                //    packet.WriteInt(price);
                                //    packet.WriteString(decription);
                                //    packet.WriteInt(costID);
                                //}
            packet.WriteBoolean(true); //_isShowBlood = event.pkg.readBoolean();
            packet.WriteBoolean(true); //_autoBlood = event.pkg.readBoolean();
            SendTCP(packet);
        }

        #endregion

        public void SendGuildMemberWeekOpenClose(UsersExtraInfo info)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.ACTIVITY_SYSTEM, info.UserID);
            pkg.WriteByte((byte)GuildMemberWeekPackageType.OPEN);
            pkg.WriteBoolean(false);//model.isOpen = param1.readBoolean();
            pkg.WriteString("2022-05-29 06:10:50.170"); //_model.ActivityStartTime = param1.readUTF();
            pkg.WriteString("2023-05-12 00:42:54.000"); //_model.ActivityEndTime = param1.readUTF();
            SendTCP(pkg);
        }

        public GSPacketIn SendPlayerRefreshTotem(PlayerInfo player)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.TOTEM, player.ID);
            pkg.WriteInt(1);
            pkg.WriteInt(player.myHonor);
            pkg.WriteInt(player.totemId);
            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendUpdateUpCount(PlayerInfo player)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.HONOR_UP_COUNT, player.ID);
            pkg.WriteInt(player.MaxBuyHonor);//max 20t/day
            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendNecklaceStrength(PlayerInfo player)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.NECKLACE_STRENGTH, player.ID);
            pkg.WriteInt(player.necklaceExp);
            pkg.WriteInt(player.necklaceExpAdd);
            pkg.WriteInt(0);//this.Self.necklaceArtificeNeedCount = _arg_1.pkg.readInt();
            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendUserSyncEquipGhost(GamePlayer p)
        {
            GSPacketIn pkg = new GSPacketIn((int)ePackageType.SYNC_EQUIP_GHOST);
            List<UserEquipGhostInfo> lists = p.GetAllEquipGhost();
            pkg.WriteInt(lists.Count);
            foreach (UserEquipGhostInfo info in lists)
            {
                pkg.WriteInt(info.BagType);
                pkg.WriteInt(info.Place);
                pkg.WriteInt(info.Level);
                pkg.WriteInt(info.TotalGhost);
            }
            SendTCP(pkg);
            return pkg;
        }

        public GSPacketIn SendSingleRoomCreate(BaseRoom room)
        {
            return SendSingleRoomCreate(room, null);
        }


        public GSPacketIn SendSingleRoomCreate(BaseRoom room, GamePlayer player)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.GAME_ROOM);
            pkg.WriteByte((byte)GameRoomPackageType.SINGLE_ROOM_BEGIN);
            pkg.WriteInt(room.RoomId);
            pkg.WriteByte((byte)room.RoomType);
            pkg.WriteBoolean(room.IsPlaying);
            pkg.WriteByte((byte)room.GameType);
            pkg.WriteInt(room.MapId);
            pkg.WriteBoolean(room.isCrosszone);//_loc_3.isCrossZone = _loc_2.readBoolean();
            pkg.WriteInt(room.AreaID);//ZoneID
            pkg.WriteBoolean(room.IsShowLoading);//_isShowGameLoading = event.pkg.readBoolean();
            if (player == null)
            {
                SendTCP(pkg);
            }
            else
            {
                player.SendTCP(pkg);
            }
            return pkg;
        }

        public void SendOpenOrCloseChristmas(int lastPacks, bool isOpen)
        {
            GSPacketIn packet = new GSPacketIn((byte)ePackageType.ACTIVITY_SYSTEM);
            packet.WriteByte((byte)ActiveSystemPackageType.CHRISTMAS_OPENORCLOSE);
            packet.WriteBoolean(isOpen);//this._model.isOpen = param1.readBoolean();
            if (isOpen)
            {
                DateTime beginTime = DateTime.Parse(GameProperties.ChristmasBeginDate);
                DateTime endTime = DateTime.Parse(GameProperties.ChristmasEndDate);
                packet.WriteDateTime(beginTime);//    this._model.beginTime = param1.readDate();
                packet.WriteDateTime(endTime);//    this._model.endTime = param1.readDate();
                string[] christmasGifts = GameProperties.ChristmasGifts.Split('|');
                packet.WriteInt(christmasGifts.Length);//    this._model.packsLen = param1.readInt();
                int loc3 = 0;
                while (loc3 < christmasGifts.Length)
                {
                    string[] _loc_4 = christmasGifts[loc3].Split(',');
                    packet.WriteInt(int.Parse(_loc_4[0]));//        _loc_4.TemplateID = param1.readInt();
                    packet.WriteInt(int.Parse(_loc_4[1]));//        this._model.snowPackNum[_loc_3] = param1.readInt();
                    loc3++;
                }
                packet.WriteInt(lastPacks);//    this._model.lastPacks = param1.readInt();
                packet.WriteInt(GameProperties.ChristmasBuildSnowmanDoubleMoney);//    this._model.money = param1.readInt();
            }
            SendTCP(packet);
        }

        public void SendPyramidOpenClose(PyramidConfigInfo info)
        {
            GSPacketIn packet = new GSPacketIn((short)145, info.UserID);
            packet.WriteByte((byte)0);
            packet.WriteBoolean(info.isOpen);
            packet.WriteBoolean(info.isScoreExchange);
            packet.WriteDateTime(info.beginTime);
            packet.WriteDateTime(info.endTime);
            packet.WriteInt(info.freeCount);
            packet.WriteInt(info.turnCardPrice);
            packet.WriteInt(info.revivePrice.Length);
            for (int index = 0; index < info.revivePrice.Length; ++index)
                packet.WriteInt(info.revivePrice[index]);
            this.SendTCP(packet);
        }

    }
}
