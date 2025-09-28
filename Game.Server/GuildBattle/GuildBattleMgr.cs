using Bussiness;
using Game.Base;
using Game.Base.Packets;
using Game.Logic;
using Game.Server.GameObjects;
using Game.Server.GuildBattle.Action;
using Game.Server.Managers;
using Game.Server.Packets;
using Game.Server.Rooms;
using log4net;
using Newtonsoft.Json;
using SqlDataProvider.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Game.Server.GuildBattle
{
    [Serializable()]
    public class GuildBattleMgr
    {
        [NonSerialized]
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static ArrayList m_actions;

        private long m_passTick = 0;

        public int CurrentActionCount = 0;

        private long m_waitTimer = 0;

        public GuildBattleState State;

        private DayOfWeek m_DayStart;

        public DateTime TimeStart;

        public DateTime TimeStop;

        public bool IsOpen { get; set; }

        public bool IsSendMail { get; set; }

        public bool IsSendAward { get; set; }

        public DateTime LastDateReloadTop { get; set; }

        public DayOfWeek DayStart { get { return m_DayStart; } }

        public readonly DateTime GuildBattleStartTime = DateTime.Parse(GameProperties.GuildBattleStartTime);
        
        public readonly List<Point> GuildPointDefault = new List<Point>() { new Point(649, 954), new Point(988, 584), new Point(1643, 611), new Point(2359, 600), new Point(2503, 987), new Point(2086, 1512), new Point(1599, 1445), new Point(868, 1362) };

        private Dictionary<int, UserGuildBattleInfo> m_listPlayer = new Dictionary<int, UserGuildBattleInfo>();
        
        private Dictionary<int, GuildBattleConsortiaInfo> m_listConsortia = new Dictionary<int, GuildBattleConsortiaInfo>();

        //private string m_curPatch = AppDomain.CurrentDomain.BaseDirectory + "datas/guildbattle.dat";// ConfigurationManager.AppSettings["DataPath"] + "guildbattle.dat";

        private object m_object = new object();

        public GuildBattleMgr()
        {
            m_actions = new ArrayList();
            State = GuildBattleState.CLOSE;
            m_listPlayer = new Dictionary<int, UserGuildBattleInfo>();
            m_listConsortia = new Dictionary<int, GuildBattleConsortiaInfo>();
            IsSendMail = false;
            IsSendAward = false;
            TimeStart = DateTime.Now;
            TimeStop = DateTime.Now;
            m_DayStart = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), GameProperties.GuildBattleStartDay);
        }

        public void SaveCurrentRankToDatabase(bool clear)
        {
            using(ConsortiaBussiness pb = new ConsortiaBussiness())
            {
                if (clear)
                {
                    pb.RemoveAllConsortiaBattleRank();
                    pb.RemoveAllConsortiaBattlePlayerRank();
                }

                GuildBattleConsortiaInfo[] allGuilds = GetAllConsortia().OrderByDescending(a => a.Score).ToArray();
                UserGuildBattleInfo[] allPlayers = GetAllUser().OrderByDescending(a => a.Score).ToArray();

                int rank = 1;
                foreach(GuildBattleConsortiaInfo cor in allGuilds)
                {
                    ConsortiaWarRankInfo data = new ConsortiaWarRankInfo();
                    data.ConsortiaID = cor.ConsortiaID;
                    data.ConsortiaName = cor.ConsortiaName;
                    data.Rank = rank;
                    data.Score = cor.Score;
                    data.TimeCreate = DateTime.Now;

                    pb.AddConsortiaBattleRank(data);

                    rank++;
                }

                foreach (UserGuildBattleInfo user in allPlayers)
                {
                    ConsortiaWarPlayerRankInfo data = new ConsortiaWarPlayerRankInfo();
                    data.UserID = user.UserID;
                    data.NickName = user.NickName;
                    data.ZoneID = GameServer.Instance.Configuration.AreaID;
                    data.ZoneName = GameServer.Instance.Configuration.ServerName;
                    data.ConsortiaID = user.ConsortiaID;
                    data.Score = user.Score;
                    data.TimeCreate = DateTime.Now;

                    pb.AddConsortiaBattlePlayerRank(data);
                }

                if(clear)
                {
                    m_listConsortia = new Dictionary<int, GuildBattleConsortiaInfo>();
                    m_listPlayer = new Dictionary<int, UserGuildBattleInfo>();
                }
            }
        }

        public void ChangeOpenClose(bool _isOpen)
        {
            ChangeOpenClose(_isOpen, DateTime.Now);
        }
        public void ChangeOpenClose(bool _isOpen, DateTime timeEnd)
        {
            if (_isOpen)
            {
                State = GuildBattleState.OPEN;
                IsSendAward = false;
                m_listPlayer = new Dictionary<int, UserGuildBattleInfo>();

                if (LastDateReloadTop.Date != DateTime.Now.Date)
                    InstallConsortiaList(GetTOPConsortiaDayOnline());
                //m_listConsortia = new Dictionary<int, GuildBattleConsortiaInfo>();
            }
            else
            {
                State = GuildBattleState.CLOSE;
                IsSendMail = false;
            }
            IsOpen = _isOpen;
            TimeStart = DateTime.Now;
            TimeStop = timeEnd;

            SendAllOpenClose();
        }

        public void RemoveAllPlayerInRoom()
        {
            lock(m_listPlayer)
            {
                foreach(UserGuildBattleInfo u in m_listPlayer.Values)
                {
                    if(u != null && u.IsActive && u.Player != null && u.Player.CurrentRoom != null)
                    {
                        u.Player.CurrentRoom.RemovePlayerUnsafe(u.Player);
                    }
                    u.IsActive = false;
                    u.Player = null;
                }
            }
        }

        public ConsortiaInfo[] GetTOPConsortiaWeekRiches()
        {
            ConsortiaInfo[] listAllow = null;
            using (ConsortiaBussiness pb = new ConsortiaBussiness())
            {
                listAllow = pb.GetRankWeekRichesConsortias();
            }
            return listAllow;
        }

        public ConsortiaInfo[] GetTOPConsortiaDayRiches()
        {
            ConsortiaInfo[] listAllow = null;
            using (ConsortiaBussiness pb = new ConsortiaBussiness())
            {
                listAllow = pb.GetRankDayRichesConsortias();
            }
            return listAllow;
        }

        public ConsortiaInfo[] GetTOPConsortiaDayOnline()
        {
            ConsortiaInfo[] listAllow = null;
            using (ConsortiaBussiness pb = new ConsortiaBussiness())
            {
                listAllow = pb.GetRankDayOnlineConsortias();
            }
            return listAllow;
        }

        public string InstallConsortiaList(ConsortiaInfo[] list)
        {
            List<string> textContent = new List<string>();
            lock(m_listConsortia)
            {
                m_listConsortia.Clear();
                int count = 0;
                foreach(ConsortiaInfo cur in list)
                {
                    if(!m_listConsortia.ContainsKey(cur.ConsortiaID))
                    {
                        GuildBattleConsortiaInfo guild = new GuildBattleConsortiaInfo();
                        guild.ConsortiaID = cur.ConsortiaID;
                        guild.ConsortiaName = cur.ConsortiaName;
                        guild.Rank = 0;
                        guild.Score = 0;
                        guild.DefaultPoint = GuildPointDefault[count];

                        m_listConsortia.Add(guild.ConsortiaID, guild);

                        textContent.Add(cur.ConsortiaName);
                        count++;
                    }
                }
            }
            LastDateReloadTop = DateTime.Now;

            return string.Join(",", textContent.ToArray());
        }

        public bool AddPlayer(GamePlayer p)
        {
            bool canAdd = false;

            GuildBattleConsortiaInfo corInfo = FindConsortia(p.PlayerCharacter.ConsortiaID);

            if(p.PlayerCharacter.ConsortiaID != 0 && corInfo != null && State == GuildBattleState.OPEN)
            {
                UserGuildBattleInfo player = FindUser(p.PlayerId);
                if (player != null)
                {
                    player.NickName = p.PlayerCharacter.NickName;
                    player.IsActive = true;
                    player.Player = p;
                    canAdd = true;
                }
                else
                {
                    player = new UserGuildBattleInfo(p);
                    canAdd = AddUser(player);
                }

                player.Postion = corInfo.DefaultPoint;

                if (p.PlayerCharacter.ReduceStartBlood <= 0)
                    p.PlayerCharacter.ReduceStartBlood = p.PlayerCharacter.hp;

                if (canAdd)
                {
                    SendInitSeftInfo(player);
                    //SendAddPlayer(player);
                }
            }
            return canAdd;
        }

        public void RemovePlayer(GamePlayer p)
        {
            UserGuildBattleInfo u = FindUser(p.PlayerId);
            if(u != null)
            {
                RemovePlayer(u);
            }
        }

        public void RemovePlayer(UserGuildBattleInfo p)
        {
            SendRemovePlayer(p);

            p.IsActive = false;
            p.WinStreak = 0;
            p.FailBuffCount = 0;
            p.LostCount = 0;
            p.Status = UserGuildBattleStatus.NORMAL;
        }
        
        public void ChallengeGame(GamePlayer p1, GamePlayer p2)
        {
            lock(m_object)
            {
                if(p1.CurrentRoom != null && p2.CurrentRoom != null && p1.CurrentRoom.RoomType == eRoomType.ConsortiaBattle && p2.CurrentRoom.RoomType == eRoomType.ConsortiaBattle && p1.CurrentRoom.IsPlaying == false && p2.CurrentRoom.IsPlaying == false)
                {
                    UserGuildBattleInfo u1 = FindUser(p1.PlayerId);
                    UserGuildBattleInfo u2 = FindUser(p2.PlayerId);

                    if(u1 != null && u2 != null && u1.Status == UserGuildBattleStatus.NORMAL && u2.Status == UserGuildBattleStatus.NORMAL && !u1.IsDead && !u2.IsDead)
                    {
                        // can challenge
                        u1.Status = UserGuildBattleStatus.FIGHTING;
                        u2.Status = UserGuildBattleStatus.FIGHTING;
                        //SendUpdateSceneInfo(u1);
                        //SendUpdateSceneInfo(u2);

                        if(u1.FailBuffCount >= 1)
                        {
                            u1.Player.PlayerCharacter.AgiPlusGuildBattle = 30;
                            u1.Player.PlayerCharacter.AttPlusGuildBattle = 30;
                        }
                        else
                        {
                            u1.Player.PlayerCharacter.AgiPlusGuildBattle = 0;
                            u1.Player.PlayerCharacter.AttPlusGuildBattle = 0;
                        }
                        if (u2.FailBuffCount >= 1)
                        {
                            u2.Player.PlayerCharacter.AgiPlusGuildBattle = 30;
                            u2.Player.PlayerCharacter.AttPlusGuildBattle = 30;
                        }
                        else
                        {
                            u2.Player.PlayerCharacter.AgiPlusGuildBattle = 0;
                            u2.Player.PlayerCharacter.AttPlusGuildBattle = 0;
                        }

                        SendUpdatePlayerStatus(u1);
                        SendUpdatePlayerStatus(u2);

                        RoomMgr.StartConsortiaBattle(p1.CurrentRoom, p2.CurrentRoom);
                    }
                    else
                    {
                        p1.SendMessage(LanguageMgr.GetTranslation("GameServer.GuildBattle.RoomStillWaiting"));
                    }
                }
                else
                {
                    p1.SendMessage(LanguageMgr.GetTranslation("GameServer.GuildBattle.RoomStillWaiting"));
                }
            }
        }

        public void UpdateScoreMatch(int winId, int lostId)
        {
            UserGuildBattleInfo win = FindUser(winId);
            UserGuildBattleInfo lost = FindUser(lostId);
            
            if(win != null && lost != null && (win.IsActive == false || win.Status == UserGuildBattleStatus.FIGHTING) && (lost.IsActive == false || lost.Status == UserGuildBattleStatus.FIGHTING))
            {
                win.Status = UserGuildBattleStatus.NORMAL;
                lost.Status = UserGuildBattleStatus.NORMAL;

                int stackEnd = lost.WinStreak;

                if (stackEnd >= 3)
                    SendBroadcastMsg(2, stackEnd, win.NickName, lost.NickName);

                win.WinStreak++;
                win.VictoryCount++;
                win.FailBuffCount = 0;
                win.LostCount = 0;

                lost.WinStreak = 0;
                lost.LostCount++;
                if (lost.LostCount >= 3)
                    lost.FailBuffCount = 1;

                if (win.WinStreak == 3 || win.WinStreak == 6 || win.WinStreak >= 10)
                    SendBroadcastMsg(1, stackEnd, win.NickName, lost.NickName);

                AddScore(win, stackEnd);
            }
        }

        public void AddScore(UserGuildBattleInfo p, int endStack)
        {
            int totalScore = 0;

            if (p.WinStreak >= 3 && p.WinStreak < 6)
                totalScore += 50;
            else if (p.WinStreak >= 6 && p.WinStreak < 10)
                totalScore += 70;
            else if (p.WinStreak >= 10)
                totalScore += 110;
            else
                totalScore += 30;

            if (endStack >= 6 && endStack < 10)
                totalScore += 50;
            else if (endStack == 10)
                totalScore += 70;
            else if (endStack > 10)
                totalScore += 90;

            if (p.DupeScoreConsortiaBattle)
                totalScore *= 2;

            p.Score += totalScore;

            GuildBattleConsortiaInfo cor = FindConsortia(p.ConsortiaID);
            cor.Score += totalScore;
        }

        #region Packet
        private void SendBroadcastMsg(byte type, int winStreak, string winName, string lostName)
        {
            GSPacketIn pkg = new GSPacketIn((int)ePackageType.CONSORTIA_BATTLE);
            pkg.WriteByte((byte)ConsBatPackageType.BROADCAST);
            pkg.WriteByte(type);
            if (type == 1)
            {
                pkg.WriteInt(winStreak);
                pkg.WriteString(winName);
            }
            else
            {
                pkg.WriteInt(winStreak);
                pkg.WriteString(lostName);
                pkg.WriteString(winName);
            }

            SendToAll(pkg);
        }
        public void SendAllPlayerList(UserGuildBattleInfo ex, bool toAll)
        {
            UserGuildBattleInfo[] allPlayers = GetAllActiveUser();

            GSPacketIn pkg = new GSPacketIn((int)ePackageType.CONSORTIA_BATTLE);
            pkg.WriteByte((byte)ConsBatPackageType.ADD_PLAYER);
            pkg.WriteInt(allPlayers.Length);

            foreach(UserGuildBattleInfo user in allPlayers)
            {
                pkg.WriteInt(user.UserID);
                pkg.WriteDateTime(user.TombStoneEndTime);
                pkg.WriteByte((byte)user.Status);
                pkg.WriteInt(user.Postion.X);
                pkg.WriteInt(user.Postion.Y);
                pkg.WriteBoolean(user.Player.PlayerCharacter.Sex);
                pkg.WriteInt(user.ConsortiaID);
                pkg.WriteString(user.ConsortiaName);
                pkg.WriteInt(user.WinStreak);
                pkg.WriteInt(user.FailBuffCount);
                //pkg.WriteBoolean(user.Player.PlayerCharacter.Sex);
                //pkg.WriteString(user.Player.PlayerCharacter.Style);
                //pkg.WriteString(user.Player.PlayerCharacter.Colors);
            }

            if (toAll)
                SendToAll(pkg, ex);
            else
                ex.Player.SendTCP(pkg);
        }
        public void SendAddPlayer(UserGuildBattleInfo user)
        {
            GSPacketIn pkg = new GSPacketIn((int)ePackageType.CONSORTIA_BATTLE);
            pkg.WriteByte((byte)ConsBatPackageType.ADD_PLAYER);
            pkg.WriteInt(1);
            pkg.WriteInt(user.UserID);
            pkg.WriteDateTime(user.TombStoneEndTime);
            pkg.WriteByte((byte)user.Status);
            pkg.WriteInt(user.Postion.X);
            pkg.WriteInt(user.Postion.Y);
            pkg.WriteBoolean(user.Player.PlayerCharacter.Sex);
            pkg.WriteInt(user.ConsortiaID);
            pkg.WriteString(user.ConsortiaName);
            pkg.WriteInt(user.WinStreak);
            pkg.WriteInt(user.FailBuffCount);
            //pkg.WriteBoolean(user.Player.PlayerCharacter.Sex);
            //pkg.WriteString(user.Player.PlayerCharacter.Style);
            //pkg.WriteString(user.Player.PlayerCharacter.Colors);

            SendToAll(pkg);
        }
        public void SendInitSeftInfo(UserGuildBattleInfo user)
        {
            GSPacketIn pkg = new GSPacketIn((int)ePackageType.CONSORTIA_BATTLE);
            pkg.WriteByte((byte)ConsBatPackageType.ENTER_SELF_INFO);
            pkg.WriteBoolean(true);
            pkg.WriteDateTime(user.TombStoneEndTime);
            pkg.WriteInt(user.Postion.X);
            pkg.WriteInt(user.Postion.Y);
            pkg.WriteInt(user.Player.PlayerCharacter.ReduceStartBlood);
            pkg.WriteInt(user.VictoryCount);
            pkg.WriteInt(user.WinStreak);
            pkg.WriteInt(user.Score);
            pkg.WriteBoolean(user.Player.PlayerCharacter.ActivePowFirstGame);
            pkg.WriteBoolean(user.DupeScoreConsortiaBattle);
            user.Player.SendTCP(pkg);
        }
        public void SendPlayerMove(UserGuildBattleInfo user, string moveStr)
        {
            GSPacketIn pkg = new GSPacketIn((int)ePackageType.CONSORTIA_BATTLE);
            pkg.WriteByte((byte)ConsBatPackageType.PLAYER_MOVE);
            pkg.WriteInt(user.UserID);
            pkg.WriteInt(user.Postion.X);
            pkg.WriteInt(user.Postion.Y);
            pkg.WriteString(moveStr);

            SendToAll(pkg, user);
        }
        public void SendRemovePlayer(UserGuildBattleInfo user)
        {
            GSPacketIn pkg = new GSPacketIn((int)ePackageType.CONSORTIA_BATTLE);
            pkg.WriteByte((byte)ConsBatPackageType.DELETE_PLAYER);
            pkg.WriteInt(user.UserID);
            SendToAll(pkg);
        }
        public void SendUpdateScore(UserGuildBattleInfo user, byte type)
        {
            GSPacketIn pkg = new GSPacketIn((int)ePackageType.CONSORTIA_BATTLE);
            pkg.WriteByte((byte)ConsBatPackageType.UPDATE_SCORE);
            pkg.WriteByte(type);
            if(type == 1)
            {
                GuildBattleConsortiaInfo[] allList = GetAllConsortia().OrderByDescending(a => a.Score).ToArray();
                pkg.WriteInt(allList.Length);

                int rank = 1;
                foreach (GuildBattleConsortiaInfo cor in allList)
                {
                    pkg.WriteString(cor.ConsortiaName);
                    pkg.WriteInt(rank);
                    pkg.WriteInt(cor.Score);
                    rank++;
                }
            }
            else
            {
                UserGuildBattleInfo[] allUsers = GetUserByConsortia(user.ConsortiaID).OrderByDescending(a => a.Score).ToArray();

                pkg.WriteInt(allUsers.Length);

                int rank = 1;
                foreach (UserGuildBattleInfo u in allUsers)
                {
                    pkg.WriteString(u.NickName);
                    pkg.WriteInt(rank);
                    pkg.WriteInt(u.Score);
                    rank++;
                }
            }
            user.Player.SendTCP(pkg);
        }
        public void SendUpdateSceneInfo(UserGuildBattleInfo user)
        {
            GSPacketIn pkg = new GSPacketIn((int)ePackageType.CONSORTIA_BATTLE);
            pkg.WriteByte((byte)ConsBatPackageType.UPDATE_SCENE_INFO);
            pkg.WriteInt(user.Player.PlayerCharacter.ReduceStartBlood);
            pkg.WriteInt(user.VictoryCount);
            pkg.WriteInt(user.WinStreak);
            pkg.WriteInt(user.Score);
            pkg.WriteBoolean(user.Player.PlayerCharacter.ActivePowFirstGame);
            pkg.WriteBoolean(user.DupeScoreConsortiaBattle);
            pkg.WriteDateTime(user.TombStoneEndTime);

            user.Player.SendTCP(pkg);
        }

        public void SendUpdatePlayerStatus(UserGuildBattleInfo user)
        {
            GSPacketIn pkg = new GSPacketIn((int)ePackageType.CONSORTIA_BATTLE);
            pkg.WriteByte((byte)ConsBatPackageType.PLAYER_STATUS);
            pkg.WriteInt(user.UserID);
            pkg.WriteDateTime(user.TombStoneEndTime);
            pkg.WriteByte((byte)user.Status);
            pkg.WriteInt(user.Postion.X);
            pkg.WriteInt(user.Postion.Y);
            pkg.WriteInt(user.WinStreak);
            pkg.WriteInt(user.FailBuffCount);
            pkg.WriteBoolean(user.Player.PlayerCharacter.Sex);
            pkg.WriteString(user.Player.PlayerCharacter.Style);
            pkg.WriteString(user.Player.PlayerCharacter.Colors);

            SendToAll(pkg);
        }
        public void SendAllOpenClose()
        {
            GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();

            GuildBattleConsortiaInfo[] allConsortias = GetAllConsortia();

            foreach (GamePlayer p in allPlayers)
            {
                GSPacketIn pkg = SendOpenClosePkg(allConsortias.SingleOrDefault(a => a.ConsortiaID == p.PlayerCharacter.ConsortiaID) != null);
                p.SendTCP(pkg);
            }
        }

        public void SendSingleOpenClose(GamePlayer p)
        {
            GSPacketIn pkg = SendOpenClosePkg(FindConsortia(p.PlayerCharacter.ConsortiaID) != null);
            p.SendTCP(pkg);
        }

        private GSPacketIn SendOpenClosePkg(bool canEnter)
        {
            GSPacketIn pkg = new GSPacketIn((int)ePackageType.CONSORTIA_BATTLE);
            pkg.WriteByte((byte)ConsBatPackageType.START_OR_CLOSE);
            pkg.WriteBoolean(IsOpen);
            pkg.WriteDateTime(TimeStart);
            pkg.WriteDateTime(TimeStop);
            pkg.WriteBoolean(canEnter);
            
            return pkg;
        }

        public void SendToTeam(GSPacketIn pkg, int consortiaId)
        {
            UserGuildBattleInfo[] temp = GetAllActiveUser(consortiaId);

            foreach (UserGuildBattleInfo p in temp)
            {
                if (p != null && p.Player != null)
                {
                    p.Player.SendTCP(pkg);
                }
            }
        }

        public void SendToTeam(GSPacketIn pkg, UserGuildBattleInfo except)
        {
            UserGuildBattleInfo[] temp = GetAllActiveUser(except.ConsortiaID);

            foreach (UserGuildBattleInfo p in temp)
            {
                if (p != null && p.Player != null && p != except)
                {
                    p.Player.SendTCP(pkg);
                }
            }
        }

        public void SendToAll(GSPacketIn pkg)
        {
            SendToAll(pkg, null);
        }

        public void SendToAll(GSPacketIn pkg, UserGuildBattleInfo except)
        {
            UserGuildBattleInfo[] allClient = GetAllActiveUser();
            foreach (UserGuildBattleInfo p in allClient)
            {
                if (p != null && p != except)
                {
                    p.Player.SendTCP(pkg);
                }
            }
        }
        #endregion
        public bool AddUser(UserGuildBattleInfo user)
        {
            lock (m_listPlayer)
            {
                if (!m_listPlayer.ContainsKey(user.UserID))
                {
                    m_listPlayer[user.UserID] = user;
                    return true;
                }

                return false;
            }
        }
        public UserGuildBattleInfo FindUser(int userid)
        {
            lock(m_listPlayer)
            {
                if (m_listPlayer.ContainsKey(userid))
                    return m_listPlayer[userid];

                return null;
            }
        }
        public UserGuildBattleInfo[] GetAllUser()
        {
            return m_listPlayer.Values.ToArray();
        }
        public UserGuildBattleInfo[] GetAllActiveUser()
        {
            return m_listPlayer.Values.Where(a => a.IsActive == true).ToArray();
        }

        public UserGuildBattleInfo[] GetAllActiveUser(int consortiaId)
        {
            return m_listPlayer.Values.Where(a => a.IsActive == true && a.ConsortiaID == consortiaId).ToArray();
        }

        public UserGuildBattleInfo[] GetUserByConsortia(int consortiaId)
        {
            return m_listPlayer.Values.Where(a => a.ConsortiaID == consortiaId).ToArray();
        }
        public GuildBattleConsortiaInfo FindConsortia(int consortiaId)
        {
            lock(m_listConsortia)
            {
                if (m_listConsortia.ContainsKey(consortiaId))
                    return m_listConsortia[consortiaId];

                return null;
            }
        }
        public GuildBattleConsortiaInfo[] GetAllConsortia()
        {
            return m_listConsortia.Values.ToArray();
        }
        public bool CanEnterGame(int consortiaId)
        {
            if (FindConsortia(consortiaId) != null && State == GuildBattleState.OPEN)
                return true;
            else
                return false;
        }
        public bool CanStartGame(DateTime now)
        {
            if(now.DayOfWeek == DayStart)
            {
                TimeSpan timeCanStart = GuildBattleStartTime.TimeOfDay;
                if (timeCanStart <= now.TimeOfDay && now.TimeOfDay.Hours < timeCanStart.Hours + 1)
                    return true;
            }
            return false;
        }

        public void AddCountDownRevive(UserGuildBattleInfo user, int timeout)
        {
            user.IsDead = true;
            user.TombStoneEndTime = DateTime.Now.AddSeconds(timeout);
            
            if (!CheckAction(typeof(PlayerReviveCountDownAction)))
                AddAction(new PlayerReviveCountDownAction(user, timeout * 1000, false));
        }

        public void QuickRevive(UserGuildBattleInfo user, bool stay)
        {
            AddAction(new PlayerReviveCountDownAction(user, 0, stay));
        }

        public bool CheckAction(IGuildBattleAction action)
        {
            lock (m_actions)
            {
                if (m_actions.Contains(action))
                    return true;
                else
                    return false;
            }
        }

        public bool CheckAction(Type type)
        {
            lock (m_actions)
            {
                if (m_actions.ToArray().SingleOrDefault(a => a.GetType().Equals(type)) != null)
                    return true;

                return false;
            }
        }

        public void AddAction(IGuildBattleAction action)
        {
            lock (m_actions)
            {
                m_actions.Add(action);
            }
        }

        public void AddAction(ArrayList actions)
        {
            lock (m_actions)
            {
                m_actions.AddRange(actions);
            }
        }

        public void Update(long tick)
        {
            if (m_passTick >= tick) return;
            
            ArrayList temp;

            lock (m_actions)
            {
                temp = (ArrayList)m_actions.Clone();
                m_actions.Clear();
            }

            CurrentActionCount = temp.Count;
            if (temp.Count > 0)
            {
                ArrayList left = new ArrayList();
                foreach (IGuildBattleAction action in temp)
                {
                    try
                    {
                        action.Execute(this, tick);
                        if (action.IsFinished(tick) == false)
                        {
                            left.Add(action);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("Map update error:", ex);
                    }
                }
                AddAction(left);
            }
            else if (m_waitTimer < tick)
            {
                CheckState(0);
            }
        }

        public void CheckState(int delay)
        {
            AddAction(new CheckGuildBattleStateAction(delay));
        }

        public void WaitTime(int delay)
        {
            m_waitTimer = Math.Max(m_waitTimer, TickHelper.GetTickCount() + delay);
        }
        
    }
}
