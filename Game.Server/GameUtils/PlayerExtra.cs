using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Managers;
using Game.Server.Packets;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Game.Server.GameUtils
{
    public class PlayerExtra
    {
        protected object m_lock;
        protected GamePlayer m_player;
        private UsersExtraInfo m_info;
        private Dictionary<int, EventRewardProcessInfo> m_eventInfo;
        private bool m_saveToDb;
        protected Timer _hotSpringTimer;
        protected Timer _pingTimeOnline;

        public GamePlayer Player => this.m_player;

        public UsersExtraInfo Info
        {
            get => this.m_info;
            set => this.m_info = value;
        }

        internal void GetSearchGoodItemsDb()
        {
            throw new NotImplementedException();
        }

        public int MapId { get; internal set; }
        public List<EventAwardInfo> SearchGoodItems
        {
            get
            {
                return this.m_searchGoodItems;
            }
            set
            {
                this.m_searchGoodItems = value;
            }
        }

        public PlayerExtra(GamePlayer player, bool saveTodb)
        {
            this.m_lock = new object();
            this.m_player = player;
            this.m_saveToDb = saveTodb;
        }

        public virtual void LoadFromDatabase()
        {
            if (!this.m_saveToDb)
                return;
            using (PlayerBussiness playerBussiness = new PlayerBussiness())
            {
                this.m_info = playerBussiness.GetSingleUsersExtra(this.m_player.PlayerCharacter.ID);
                if (this.m_info == null)
                    this.m_info = this.CreateUserExtra(this.Player.PlayerCharacter.ID);
                this.m_eventInfo = new Dictionary<int, EventRewardProcessInfo>();
                foreach (EventRewardProcessInfo rewardProcessInfo in playerBussiness.GetUserEventProcess(this.m_player.PlayerCharacter.ID))
                {
                    if (!this.m_eventInfo.ContainsKey(rewardProcessInfo.ActiveType))
                        this.m_eventInfo.Add(rewardProcessInfo.ActiveType, rewardProcessInfo);
                }
            }
        }

        internal void ConvertSearchGoodItems()
        {
            throw new NotImplementedException();
        }

        internal void TakeCard(bool useMoney)
        {
            throw new NotImplementedException();
        }

        internal void CreateSearchGoodItems()
        {
            throw new NotImplementedException();
        }

        private static ThreadSafeRandom rand = new ThreadSafeRandom();
        public UsersExtraInfo CreateUserExtra(int UserID)
        {
            UsersExtraInfo userExtraInfo = new UsersExtraInfo
            {

                UserID = UserID,
                LastTimeHotSpring = DateTime.Now,
                LastFreeTimeHotSpring = DateTime.Now,
                MinHotSpring = 60,
            };
            DateTime dateTime = DateTime.Now;
            dateTime = dateTime.AddDays(-1.0);
            userExtraInfo.LeftRoutteCount = GameProperties.LeftRouterMaxDay;
            userExtraInfo.LeftRoutteRate = 0.0f;
            return userExtraInfo;
        }

        public virtual void SaveToDatabase()
        {
            if (!this.m_saveToDb)
                return;
            using (PlayerBussiness playerBussiness = new PlayerBussiness())
            {
                lock (this.m_lock)
                {
                    if (this.m_info != null && this.m_info.IsDirty)
                        playerBussiness.UpdateUserExtra(this.m_info);
                }
            }
        }
        private UsersExtraInfo m_Info;
        private List<EventAwardInfo> m_searchGoodItems;
        private EventAwardInfo GetAwardByPos()
        {
            object @lock;
            Monitor.Enter(@lock = this.m_lock);
            try
            {
                foreach (EventAwardInfo current in this.m_searchGoodItems)
                {
                    if (current.Position == this.m_Info.nowPosition)
                    {
                        return current;
                    }
                }
            }
            finally
            {
                Monitor.Exit(@lock);
            }
            return null;
        }
        public void PlayNowPosition(int templateID)
        {
            if (templateID == -2 || templateID == -3)
            {
                EventAwardInfo awardByPos = this.GetAwardByPos();
                if (awardByPos != null)
                {
                    this.UpdateGoodItems(awardByPos);
                }
            }
            GSPacketIn gSPacketIn = new GSPacketIn(98);
            gSPacketIn.WriteByte(25);
            gSPacketIn.WriteInt(this.m_Info.nowPosition);
            this.Player.SendTCP(gSPacketIn);
        }
        
        private void AddGoods(int goodId, int count)
        {
            if (goodId <= 0)
            {
                return;
            }
            ItemTemplateInfo itemTemplateInfo = ItemMgr.FindItemTemplate(goodId);
            if (itemTemplateInfo != null)
            {
                ItemInfo itemInfo = ItemInfo.CreateFromTemplate(itemTemplateInfo, 1, 105);
                itemInfo.IsBinds = true;
                itemInfo.Count = count;
                this.Player.AddTemplate(itemInfo, itemInfo.Template.BagType, count, eGameView.OtherTypeGet, "Ek Öğeler");
            }
        }
        public void UpdateGoodItems(EventAwardInfo good)
        {
            if (good.TemplateID == -1 || good.TemplateID == -2 || good.TemplateID == -3 || good.TemplateID == -4)
            {
                return;
            }
            if (good.TemplateID == -5)
            {
                this.CreateTakeCard();
            }
            if (good.TemplateID == -6)
            {
                this.AddBuriedQuest();
            }
            this.AddGoods(good.TemplateID, good.Count);
            for (int i = 0; i < this.m_searchGoodItems.Count; i++)
            {
                if (this.m_searchGoodItems[i].Position == good.Position)
                {
                    this.m_searchGoodItems[i].TemplateID = 0;
                    break;
                }
            }
            GSPacketIn gSPacketIn = new GSPacketIn(98);
            gSPacketIn.WriteByte(23);
            gSPacketIn.WriteInt((good.TemplateID < 0) ? 0 : good.TemplateID);
            this.Player.SendTCP(gSPacketIn);
        }
        private int[] m_buriedQuests;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private void AddBuriedQuest()
        {
            int num = PlayerExtra.rand.Next(this.m_buriedQuests.Length);
            QuestInfo singleQuest = QuestMgr.GetSingleQuest(this.m_buriedQuests[num]);
            string text;
            this.Player.QuestInventory.AddQuest(singleQuest, out text);
            if (!string.IsNullOrEmpty(text))
            {
                PlayerExtra.log.InfoFormat("{0} AddBuriedQuest: {1}", this.Player.PlayerCharacter.NickName, text);
            }
        }
        private List<PlayerExtra.FlopCardInfo> m_flopCard;
        public int takeCardLimit = 3;
        private void CreateTakeCard()
        {
            this.m_flopCard.Clear();
            for (int i = 0; i < 5; i++)
            {
                int count = PlayerExtra.rand.Next(5, 75);
                PlayerExtra.FlopCardInfo flopCardInfo = new PlayerExtra.FlopCardInfo();
                flopCardInfo.Count = count;
                flopCardInfo.TemplateID = 11680;
                this.m_flopCard.Add(flopCardInfo);
            }
            this.takeCardLimit = 3;
            PlayerExtra.rand.ShufferList<PlayerExtra.FlopCardInfo>(this.m_flopCard);
            GSPacketIn gSPacketIn = new GSPacketIn(98);
            gSPacketIn.WriteByte(24);
            gSPacketIn.WriteInt(this.takeCardLimit);
            gSPacketIn.WriteInt(this.m_flopCard.Count);
            foreach (PlayerExtra.FlopCardInfo current in this.m_flopCard)
            {
                gSPacketIn.WriteInt(current.TemplateID);
                gSPacketIn.WriteInt(current.Count);
            }
            this.Player.SendTCP(gSPacketIn);
        }
        public class FlopCardInfo
        {
            public int TemplateID
            {
                get;
                set;
            }
            public int Count
            {
                get;
                set;
            }
        }

        public void RollDiceCallBack(bool isRemindRollBind)
        {
            int num = PlayerExtra.rand.Next(1, 6);
            bool flag = false;
            if (this.Info.FreeCount > 0)
            {
                this.Info.FreeCount--;
                flag = true;
            }
            else
            {
                if (this.Player.MoneyDirect(GameProperties.SearchGoodsPayMoney))
                {
                    flag = true;
                }
            }
            if (flag)
            {
                this.m_Info.nowPosition += num;
                if (this.m_Info.nowPosition > 35)
                {
                    this.m_Info.nowPosition = 35;
                }
                GSPacketIn gSPacketIn = new GSPacketIn(98);
                gSPacketIn.WriteByte(17);
                gSPacketIn.WriteInt(this.m_Info.FreeCount);
                gSPacketIn.WriteInt(num);
                gSPacketIn.WriteInt(this.m_Info.nowPosition);
                this.Player.SendTCP(gSPacketIn);
            }
            EventAwardInfo awardByPos = this.GetAwardByPos();
            if (awardByPos != null)
            {
                int templateID = awardByPos.TemplateID;
                switch (templateID)
                {
                    case -7:
                    case 0:
                        break;
                    case -6:
                        this.UpdateGoodItems(awardByPos);
                        break;
                    case -5:
                        this.UpdateGoodItems(awardByPos);
                        break;
                    case -4:
                        this.m_Info.nowPosition = 35;
                        this.PlayNowPosition(templateID);
                        break;
                    case -3:
                        this.m_Info.nowPosition++;
                        this.PlayNowPosition(templateID);
                        break;
                    case -2:
                        this.m_Info.nowPosition--;
                        this.PlayNowPosition(templateID);
                        break;
                    case -1:
                        this.m_Info.nowPosition = 0;
                        this.PlayNowPosition(templateID);
                        break;
                    default:
                        this.UpdateGoodItems(awardByPos);
                        break;
                }
            }
            if (this.m_Info.nowPosition == 35)
            {
                SearchGoodsTempInfo searchGoodsTempInfo = AwardMgr.GetSearchGoodsTempInfo(this.m_Info.starlevel);
                if (searchGoodsTempInfo != null)
                {
                    this.AddGoods(searchGoodsTempInfo.DestinationReward, 1);
                }
            }
        }
        public void BeginPingOnlineTimer()
        {
            int num = 60000;
            if (this._pingTimeOnline == null)
                this._pingTimeOnline = new Timer(new TimerCallback(this.PingTimeOnlineCheck), (object)null, num, num);
            else
                this._pingTimeOnline.Change(num, num);
        }

        public void StopPingOnlineTimer()
        {
            if (this._pingTimeOnline == null)
                return;
            this._pingTimeOnline.Dispose();
            this._pingTimeOnline = (Timer)null;
        }

        public void BeginHotSpringTimer()
        {
            int num = 60000;
            if (this._hotSpringTimer == null)
                this._hotSpringTimer = new Timer(new TimerCallback(this.HotSpringCheck), (object)null, num, num);
            else
                this._hotSpringTimer.Change(num, num);
        }

        public void StopHotSpringTimer()
        {
            if (this._hotSpringTimer == null)
                return;
            this._hotSpringTimer.Dispose();
            this._hotSpringTimer = (Timer)null;
        }

        public void StopAllTimer()
        {
            this.StopHotSpringTimer();
            this.StopPingOnlineTimer();
        }

        protected void PingTimeOnlineCheck(object sender)
        {
            try
            {
                int tickCount = Environment.TickCount;
                ThreadPriority priority = Thread.CurrentThread.Priority;
                Thread.CurrentThread.Priority = ThreadPriority.Lowest;
                this.m_player.OnOnlineGameAdd(m_player);
                this.m_player.PlayerCharacter.CheckNewDay();
                Thread.CurrentThread.Priority = priority;
                int num = Environment.TickCount - tickCount;
            }
            catch (Exception ex)
            {
                Console.WriteLine("HotSpringCheck: " + (object)ex);
            }
        }

        protected void HotSpringCheck(object sender)
        {
            try
            {
                int tickCount = Environment.TickCount;
                ThreadPriority priority = Thread.CurrentThread.Priority;
                Thread.CurrentThread.Priority = ThreadPriority.Lowest;
                if (m_player.CurrentHotSpringRoom == null)
                {
                    StopHotSpringTimer();
                }
                if (Info.MinHotSpring <= 0)
                {
                    this.m_player.SendMessage("Kaplıcadaki süreniz sona erdi.");
                    this.m_player.CurrentHotSpringRoom.RemovePlayer(this.m_player);
                }
                int getExp = HotSpringMgr.GetExpWithLevel(this.m_player.PlayerCharacter.Grade) / 10;
                if (getExp > 0)
                {
                    Info.MinHotSpring--;
                    m_player.OnPlayerSpa(1);
                    if (Info.MinHotSpring <= 5)
                    {
                        m_player.SendMessage("Sürenizin sonlanması için kalan son " + Info.MinHotSpring + " dakika.");
                    }
                    if (m_player.CurrentHotSpringRoom.Info.roomID > 4)
                    {
                        getExp = getExp * 3 / 2;
                    }
                    m_player.AddGP(getExp, false);
                    m_player.Out.SendHotSpringUpdateTime(m_player, getExp);
                    m_player.OnHotSpingExpAdd(Info.MinHotSpring, getExp);
                }
                Thread.CurrentThread.Priority = priority;
                tickCount = Environment.TickCount - tickCount;
            }
            catch (Exception Err)
            {
                Console.WriteLine("HotSpringCheck: " + Err);
            }
        }

        public EventRewardProcessInfo GetEventProcess(int activeType)
        {
            lock (this.m_lock)
            {
                if (!this.m_eventInfo.ContainsKey(activeType))
                    this.m_eventInfo.Add(activeType, this.setValue(activeType));
                return this.m_eventInfo[activeType];
            }
        }

        public void UpdateEventCondition(int activeType, int value)
        {
            this.UpdateEventCondition(activeType, value, false, 0);
        }

        public void UpdateEventCondition(int activeType, int value, bool isPlus, int awardGot)
        {
            using (PlayerBussiness pb = new PlayerBussiness())
            {
                EventRewardProcessInfo info = GetEventProcess(activeType);
                if (info == null)
                {
                    info = setValue(activeType);
                }
                if (isPlus)
                {
                    info.Conditions += value;
                }
                if (awardGot != 0)
                {
                    info.AwardGot = awardGot;
                }
                if (info.Conditions < value)
                {
                    info.Conditions = value;
                }
                DateTime now = DateTime.Now;
                DateTime endTime = DateTime.Now.AddYears(2);
                pb.UpdateUsersEventProcess(info);
                m_player.Out.SendOpenNoviceActive(0, activeType, info.Conditions, info.AwardGot, now, endTime);
            }
        }

        public void ResetUsersEventProcess(int activeType, bool isReset)
        {
            using (PlayerBussiness pb = new PlayerBussiness())
            {
                EventRewardProcessInfo info = GetEventProcess(activeType);
                if (info == null)
                {
                    info = setValue(activeType);
                }
                info.IsReset = isReset;
                pb.ResetUsersEventProcess(info);
                //m_player.SaveIntoDatabase();
            }
        }

        public void ResetNoviceEvent(NoviceActiveType activeType)
        {
            EventRewardProcessInfo eventProcess = this.GetEventProcess((int)activeType);
            eventProcess.AwardGot = 0;
            eventProcess.Conditions = 0;
            using (PlayerBussiness pb = new PlayerBussiness())
                pb.UpdateUsersEventProcess(eventProcess);

        }

        public bool CheckNoviceActiveOpen(NoviceActiveType activeType)
        {
            switch (activeType)
            {
                case NoviceActiveType.GRADE_UP_ACTIVE:
                    return true;
                case NoviceActiveType.RECHANGE_MONEY_ACTIVE:
                    return true;
                case NoviceActiveType.STRENGTHEN_WEAPON_ACTIVE:
                    return true;
                case NoviceActiveType.UPGRADE_VIP_ACTIVE:
                    return true;
                case NoviceActiveType.UPDATE_FIGHTPOWER:
                    return true;
                case NoviceActiveType.USE_MONEY_ACTIVE:
                    return true;
                case NoviceActiveType.USE_MONEY_ACTIVE_OFWEEK:
                    return true;
                case NoviceActiveType.RECHANGE_MONEY_ACTIVE_OFWEEK:
                    return true;
                default:
                    return false;
            }
        }

        //public string GetNoviceActivityName(NoviceActiveType activeType)
        //{
        //string format = "Unknown";
        //switch (activeType)
        //{
        //case NoviceActiveType.GRADE_UP_ACTIVE:
        //format = "Tăng cấp nhận thưởng";
        //break;
        //case NoviceActiveType.STRENGTHEN_WEAPON_ACTIVE:
        //format = "Cường hóa tặng quà";
        //break;
        //case NoviceActiveType.USE_MONEY_ACTIVE:
        //format = "Tiêu phí thưởng mỗi ngày";
        //break;
        //case NoviceActiveType.RECHANGE_MONEY_ACTIVE:
        //format = "Nạp thưởng mỗi ngày";
        //break;
        //case NoviceActiveType.UPGRADE_VIP_ACTIVE:
        //format = "Tăng vip nhận quà";
        // break;
        //case NoviceActiveType.UPDATE_FIGHTPOWER:
        //format = "Quà lực chiến";
        //break;
        // case NoviceActiveType.USE_MONEY_ACTIVE_OFWEEK:
        // format = "Tiêu xu thưởng hằng tuần";
        // break;
        // case NoviceActiveType.RECHANGE_MONEY_ACTIVE_OFWEEK:
        //format = "Nạp xu thưởng hằng tuần";
        //        break;
        //  }
        //  return string.Format(format);
        //}

        private EventRewardProcessInfo setValue(int activeType) => new EventRewardProcessInfo()
        {
            UserID = this.m_player.PlayerCharacter.ID,
            ActiveType = activeType,
            Conditions = 0,
            AwardGot = 0,
            IsReset = false
        };
    }
}
