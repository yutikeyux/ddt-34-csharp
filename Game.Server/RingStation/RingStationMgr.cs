using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Bussiness;
using Bussiness.Managers;
using Game.Server.Battle;
using Game.Server.GameObjects;
using Game.Server.RingStation.Battle;
using log4net;
using SqlDataProvider.Data;
using static System.Int32;

namespace Game.Server.RingStation
{
    /// <summary>
    /// Bot zorluk kademesi (max level 60)
    /// </summary>
    public enum BotDifficulty
    {
        Beginner = 0,  // Lv  1-15
        Normal = 1,  // Lv 16-30
        Hard = 2,  // Lv 31-50
        Expert = 3   // Lv 51-60
    }

    /// <summary>
    /// Bot stat çarpanlarını tutan yapı.
    /// Saldırı/savunma düşük, can yüksek — uzun ve zevkli dövüş.
    /// </summary>
    public struct BotScaleResult
    {
        public double AttackMultiplier;   // Saldırı çarpanı  — düşük tutulur
        public double DefenceMultiplier;  // Savunma çarpanı  — düşük tutulur
        public double HpMultiplier;       // Can çarpanı      — 3x sabit
        public double AgilityMultiplier;  // Agility çarpanı  — oyuncuyla eşit (1.0)
        public BotDifficulty Difficulty;
    }

    public class RingStationMgr
    {
        private static Random rand = new Random();
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected static object m_lock = new object();
        private static Dictionary<int, VirtualGamePlayer> m_ringPlayers = new Dictionary<int, VirtualGamePlayer>();
        private static RingStationBattleServer m_server;

        private static readonly string weaklessGuildProgressStr =
            "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=";

        private static VirtualPlayerInfo m_normalPlayer = new VirtualPlayerInfo();
        private static string[] _names;
        private static string[] _Gnames;
        private static List<VirtualPlayerInfo> m_vplayers = new List<VirtualPlayerInfo>();
        private static Dictionary<int, UserRingStationInfo> m_ringstation = new Dictionary<int, UserRingStationInfo>();
        private static List<UserRingStationInfo> m_ranks = new List<UserRingStationInfo>();

        private static Dictionary<int, List<RingstationBattleFieldInfo>> m_battleFields =
            new Dictionary<int, List<RingstationBattleFieldInfo>>();

        private static RingstationConfigInfo m_congfig;

        #region Properties

        public static RingstationConfigInfo ConfigInfo => m_congfig;

        public static VirtualPlayerInfo NormalPlayer
        {
            get { return m_normalPlayer; }
            set { m_normalPlayer = value; }
        }

        public static RingStationBattleServer RingStationBattle => m_server;

        #endregion

        #region Initialization

        public static bool Init()
        {
            bool result = false;
            try
            {
                BattleServer bs = BattleMgr.GetServer(4);
                if (bs == null)
                    return false;

                m_server = new RingStationBattleServer(
                    RingStationConfiguration.ServerID, bs.Ip, bs.Port, "1,7road");

                if (m_server != null)
                {
                    _names = GameProperties.VirtualName.Split(',');
                    lock (m_lock)
                    {
                        m_ringPlayers.Clear();
                    }

                    m_server.Start();

                    if (!SetupVirtualPlayer())
                        return false;

                    result = true;
                }
            }
            catch (Exception exception)
            {
                log.Error("RingStationMgr Init", exception);
            }

            return result;
        }

        #endregion

        #region Bot Difficulty Scaling

        /// <summary>
        /// Bot stat çarpanlarını hesaplar.
        ///
        /// Sabit kurallar (tüm kademelerde geçerli):
        ///   HP        = oyuncu HP x 3.0   — bot zor ölsün
        ///   Agility   = oyuncu Agility x 1.0  — eşit hız
        ///   Attack    = oyuncu Attack  / 1.5  — az vursun
        ///   Defence   = oyuncu Defence / 1.5  — kolay kırılsın
        ///   BaseAtk   = oyuncu BaseAtk / 1.5
        ///   BaseDef   = oyuncu BaseDef / 1.5
        ///
        /// Kademe geçişlerinde saldırı/savunma çarpanı hafifçe yükselerek
        /// oyuncuya "zorluk arttı" hissi verir, ama hiçbir zaman 1.0'ı geçmez.
        ///
        ///   Lv  1-15  => Saldırı/Savunma x 0.55  (Beginner)
        ///   Lv 16-30  => Saldırı/Savunma x 0.58  (Normal)
        ///   Lv 31-50  => Saldırı/Savunma x 0.62  (Hard)
        ///   Lv 51-60  => Saldırı/Savunma x 0.67  (Expert)
        /// </summary>
        public static BotScaleResult CalculateBotScale(int playerGrade)
        {
            double atkDefMultiplier = GetAttackDefenceMultiplier(playerGrade);

            return new BotScaleResult
            {
                AttackMultiplier = atkDefMultiplier,
                DefenceMultiplier = atkDefMultiplier,
                HpMultiplier = 3.0,   // Her zaman 3 kat can
                AgilityMultiplier = 1.0,   // Her zaman oyuncuyla eşit agility
                Difficulty = GetDifficulty(playerGrade)
            };
        }

        /// <summary>
        /// Kademeye göre saldırı ve savunma çarpanı.
        /// 1/1.5 ≈ 0.667 taban, kademeler arası küçük artışla oyuncuya zorluk hissi.
        /// </summary>
        private static double GetAttackDefenceMultiplier(int grade)
        {
            if (grade <= 0) grade = 1;

            if (grade <= 15) return 0.55; // Beginner — en kolay
            if (grade <= 30) return 0.58; // Normal
            if (grade <= 50) return 0.62; // Hard
            return 0.67;                  // Expert  (Lv 51-60)
        }

        private static BotDifficulty GetDifficulty(int grade)
        {
            if (grade <= 15) return BotDifficulty.Beginner;
            if (grade <= 30) return BotDifficulty.Normal;
            if (grade <= 50) return BotDifficulty.Hard;
            return BotDifficulty.Expert;
        }

        #endregion

        #region Battle Field Operations

        public static bool ReLoadBattleField()
        {
            try
            {
                RingstationBattleFieldInfo[] tempArr = LoadRingstationBattleFieldDb();
                Dictionary<int, List<RingstationBattleFieldInfo>> tempDict =
                    LoadRingstationBattleFields(tempArr);

                if (tempArr.Length > 0)
                    Interlocked.Exchange(ref m_battleFields, tempDict);
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("ReLoad RingstationBattleField", e);
                return false;
            }
            return true;
        }

        public static RingstationBattleFieldInfo[] LoadRingstationBattleFieldDb()
        {
            using (PlayerBussiness pb = new PlayerBussiness())
            {
                return null;
            }
        }

        public static Dictionary<int, List<RingstationBattleFieldInfo>> LoadRingstationBattleFields(
            RingstationBattleFieldInfo[] RingstationBattleField)
        {
            Dictionary<int, List<RingstationBattleFieldInfo>> infos =
                new Dictionary<int, List<RingstationBattleFieldInfo>>();

            foreach (RingstationBattleFieldInfo info in RingstationBattleField)
            {
                if (!infos.Keys.Contains(info.UserID))
                {
                    IEnumerable<RingstationBattleFieldInfo> temp =
                        RingstationBattleField.Where(s => s.UserID == info.UserID);
                    infos.Add(info.UserID, temp.ToList());
                }
            }
            return infos;
        }

        public static UserRingStationInfo[] GetRingStationRanks()
        {
            List<UserRingStationInfo> list = new List<UserRingStationInfo>();
            foreach (UserRingStationInfo rank in m_ranks)
            {
                list.Add(rank);
                if (list.Count >= 50)
                    break;
            }
            return list.ToArray();
        }

        public static bool UpdateRingBattleFields(
            RingstationBattleFieldInfo dareFlag,
            RingstationBattleFieldInfo successFlag)
        {
            List<RingstationBattleFieldInfo> list;
            int dareId = -1;
            int successId = -1;
            int dareRank = 0;
            int successRank = 0;
            bool saveTodb = false;

            using (PlayerBussiness pb = new PlayerBussiness())
            {
                if (dareFlag != null) dareId = dareFlag.UserID;
                if (successFlag != null) successId = successFlag.UserID;

                UserRingStationInfo dareRing = GetSingleRingStationInfos(dareId);
                if (dareRing != null)
                {
                    if (dareRing.Rank == 0)
                        dareRing.Rank = m_ranks.Count + 1;

                    if (dareRing.ChallengeNum > 0)
                    {
                        dareRing.ChallengeNum--;
                        dareRing.ChallengeTime = DateTime.Now.AddMinutes(10);
                    }

                    if (dareFlag != null && dareFlag.SuccessFlag)
                        dareRing.Total++;

                    dareRank = dareRing.Rank;
                }

                UserRingStationInfo successRing = GetSingleRingStationInfos(successId);
                if (successRing != null)
                    successRank = successRing.Rank;

                if (dareFlag != null)
                {
                    if (dareRing != null)
                    {
                        if (dareFlag.SuccessFlag && successRing != null && dareRing.Rank > successRing.Rank)
                        {
                            dareRing.Rank = successRank;
                            successRing.Rank = dareRank;
                            saveTodb = true;
                        }
                        UpdateRingStationInfo(dareRing);
                    }

                    lock (m_lock)
                    {
                        if (m_battleFields.ContainsKey(dareId))
                            m_battleFields[dareId].Add(dareFlag);
                        else
                        {
                            list = new List<RingstationBattleFieldInfo> { dareFlag };
                            m_battleFields.Add(dareId, list);
                        }
                    }
                }

                if (successFlag != null)
                {
                    if (successRing != null)
                    {
                        successRing.OnFight = false;
                        UpdateRingStationFight(successRing);
                    }

                    lock (m_lock)
                    {
                        if (m_battleFields.ContainsKey(successId))
                            m_battleFields[successId].Add(successFlag);
                        else
                        {
                            list = new List<RingstationBattleFieldInfo> { successFlag };
                            m_battleFields.Add(successId, list);
                        }
                    }
                }

                if (saveTodb)
                {
                    dareFlag.Level = (dareFlag.Level == dareRing.Rank) ? 0 : dareRing.Rank;
                    UpdateRingStationInfo(dareRing);

                    if (successFlag != null)
                    {
                        successFlag.Level = (successFlag.Level == successRing.Rank) ? 0 : successRing.Rank;
                        UpdateRingStationInfo(successRing);
                    }
                }
            }

            return true;
        }

        public static RingstationBattleFieldInfo[] GetRingBattleFields(int playerId)
        {
            List<RingstationBattleFieldInfo> list = new List<RingstationBattleFieldInfo>();
            lock (m_lock)
            {
                if (m_battleFields.ContainsKey(playerId))
                    list.AddRange(m_battleFields[playerId]);
            }

            return (from pair in list
                    orderby pair.BattleTime descending
                    select pair).Take(10).ToArray();
        }

        #endregion

        #region Ring Station Data

        public static bool ReLoadUserRingStation()
        {
            try
            {
                UserRingStationInfo[] tempArr = LoadUserRingStationDb();
                Dictionary<int, UserRingStationInfo> tempDict = LoadUserRingStations(tempArr);

                if (tempArr.Length > 0)
                {
                    Interlocked.Exchange(ref m_ringstation, tempDict);
                    m_ranks = (from pair in tempArr
                               where pair.Rank != 0
                               orderby pair.Rank ascending
                               select pair).ToList();
                }
            }
            catch (Exception e)
            {
                if (log.IsErrorEnabled)
                    log.Error("ReLoad All UserRingStation", e);
                return false;
            }
            return true;
        }

        public static UserRingStationInfo[] LoadUserRingStationDb()
        {
            using (PlayerBussiness pb = new PlayerBussiness())
            {
                return null;
            }
        }

        public static Dictionary<int, UserRingStationInfo> LoadUserRingStations(
            UserRingStationInfo[] UserRingStation)
        {
            Dictionary<int, UserRingStationInfo> infos = new Dictionary<int, UserRingStationInfo>();
            using (PlayerBussiness pb = new PlayerBussiness())
            {
                foreach (UserRingStationInfo ring in UserRingStation)
                {
                    if (infos.Keys.Contains(ring.UserID)) continue;
                    try
                    {
                        ring.Info = pb.GetUserSingleByUserID(ring.UserID);
                        if (ring.Info != null)
                        {
                            ring.WeaponID = GetWeaponId(ring.Info.Style);
                            infos.Add(ring.UserID, ring);
                        }
                    }
                    catch { /* ignored */ }
                }
            }
            return infos;
        }

        public static void LoadRingStationInfo(PlayerInfo player, int dame, int guard)
        {
            if (player == null) return;

            using (PlayerBussiness pb = new PlayerBussiness())
            {
                if (m_ringstation.ContainsKey(player.ID))
                {
                    bool saveToDb = false;
                    UserRingStationInfo ring = m_ringstation[player.ID];

                    if (dame != ring.BaseDamage && ring.BaseGuard != guard)
                    {
                        ring.BaseDamage = dame;
                        ring.BaseGuard = guard;
                        ring.BaseEnergy = (int)(1 - player.Agility * 0.001);
                        saveToDb = true;
                    }

                    int weponId = GetWeaponId(player.Style);
                    if (ring.WeaponID != weponId)
                    {
                        ring.WeaponID = weponId;
                        saveToDb = true;
                    }
                }
                else
                {
                    UserRingStationInfo info = new UserRingStationInfo
                    {
                        UserID = player.ID,
                        WeaponID = GetWeaponId(player.Style),
                        BaseDamage = dame,
                        BaseGuard = guard,
                        BaseEnergy = (int)(1 - player.Agility * 0.001),
                        signMsg = LanguageMgr.GetTranslation("RingStation.signMsg"),
                        ChallengeNum = ConfigInfo.ChallengeNum,
                        buyCount = ConfigInfo.buyCount,
                        ChallengeTime = DateTime.Now,
                        LastDate = DateTime.Now,
                        Info = player
                    };
                    m_ringstation.Add(player.ID, info);
                }
            }
        }

        public static int GetWeaponId(string style)
        {
            if (!string.IsNullOrEmpty(style))
            {
                string[] styles = style.Split(',');
                string weapon = styles[6];
                if (weapon.IndexOf("|", StringComparison.Ordinal) != -1)
                    return Parse(weapon.Split('|')[0]);
            }
            return 7008;
        }

        #endregion

        #region Challenge & Rank

        public static UserRingStationInfo GetRingStationChallenge(int playerId, int rank, ref bool isAutoBot)
        {
            lock (m_lock)
            {
                if (m_ringstation.ContainsKey(playerId) && rank != 0)
                    return m_ringstation[playerId];
            }

            isAutoBot = true;
            return BaseRingStationChallenges(playerId);
        }

        public static void SetChallenge(int playerId, bool onFight)
        {
            lock (m_lock)
            {
                if (m_ringstation.ContainsKey(playerId))
                    m_ringstation[playerId].OnFight = onFight;
            }
        }

        public static UserRingStationInfo GetSingleRingStationInfos(int playerId)
        {
            lock (m_lock)
            {
                if (m_ringstation.ContainsKey(playerId))
                    return m_ringstation[playerId];
            }
            return null;
        }

        public static bool UpdateRingStationInfo(UserRingStationInfo ring)
        {
            if (ring == null) return false;
            using (PlayerBussiness pb = new PlayerBussiness())
            {
                lock (m_lock)
                {
                    if (m_ringstation.ContainsKey(ring.UserID))
                        m_ringstation[ring.UserID] = ring;
                }
            }
            return false;
        }

        public static bool UpdateRingStationFight(UserRingStationInfo ring)
        {
            if (ring == null) return false;
            lock (m_lock)
            {
                if (m_ringstation.ContainsKey(ring.UserID))
                {
                    m_ringstation[ring.UserID] = ring;
                    return true;
                }
            }
            return false;
        }

        public static List<UserRingStationInfo> FindRingStationInfoByRank(int userId, int min, int max)
        {
            return m_ringstation.Values
                .Where(info => info.UserID != userId)
                .Where(info => info.Rank >= min && info.Rank <= max)
                .ToList();
        }

        public static UserRingStationInfo[] GetRingStationInfos(int userId, int rank)
        {
            NormalPlayer = GetVirtualPlayerInfo();
            Dictionary<int, UserRingStationInfo> list = new Dictionary<int, UserRingStationInfo>();
            int baseValue = 5;

            if (rank > 0)
            {
                int minValue = rank;
                int maxValue = rank - baseValue;

                if (minValue <= 0)
                {
                    minValue = m_ranks.Count;
                    maxValue = m_ranks.Count - baseValue;
                }
                else if (maxValue <= 0)
                {
                    minValue = 1;
                    maxValue = baseValue;
                }

                List<UserRingStationInfo> infos = FindRingStationInfoByRank(userId, minValue, maxValue);
                if (infos.Count == 4)
                {
                    for (int i = 0; list.Count < 4; i++)
                    {
                        UserRingStationInfo info = infos[rand.Next(infos.Count)];
                        if (info == null) continue;
                        if (!list.ContainsKey(info.UserID))
                            list.Add(info.UserID, info);
                        infos.Remove(info);
                    }
                }
            }

            if (list.Count == 0)
            {
                UserRingStationInfo boot = BaseRingStationChallenges(0);
                list.Add(boot.Info.ID, boot);
            }

            return list.Values.ToArray();
        }

        #endregion

        #region Player Pool

        public static bool AddPlayer(int playerId, VirtualGamePlayer player)
        {
            lock (m_lock)
            {
                if (m_ringPlayers.ContainsKey(playerId))
                    return true;
                m_ringPlayers.Add(playerId, player);
            }
            return true;
        }

        public static bool RemovePlayer(int playerId)
        {
            lock (m_lock)
            {
                if (m_ringPlayers.ContainsKey(playerId))
                    return m_ringPlayers.Remove(playerId);
            }
            return false;
        }

        public static VirtualGamePlayer GetPlayerById(int playerId)
        {
            VirtualGamePlayer result = null;
            lock (m_lock)
            {
                if (m_ringPlayers.ContainsKey(playerId))
                    result = m_ringPlayers[playerId];
            }
            return result;
        }

        public static List<VirtualGamePlayer> GetAllPlayer()
        {
            List<VirtualGamePlayer> list = new List<VirtualGamePlayer>();
            lock (m_lock)
            {
                foreach (VirtualGamePlayer current in m_ringPlayers.Values)
                    list.Add(current);
            }
            return list;
        }

        #endregion

        #region Timer

        protected static Timer m_statusScanTimer;

        public static void BeginTimer()
        {
            int interval = 60 * 1000;
            if (m_statusScanTimer == null)
                m_statusScanTimer = new Timer(new TimerCallback(StatusScan), null, interval, interval);
            else
                m_statusScanTimer.Change(interval, interval);
        }

        protected static void StatusScan(object sender)
        {
            try
            {
                log.Info("Begin Scan RingStation Info....");
                int startTick = Environment.TickCount;
                ThreadPriority oldprio = Thread.CurrentThread.Priority;
                Thread.CurrentThread.Priority = ThreadPriority.Lowest;

                bool saveToDb = false;
                if (ReLoadUserRingStation())
                {
                    List<UserRingStationInfo> list = m_ringstation.Values.ToList();

                    if (ConfigInfo.IsFirstUpdateRank && list.Count > 10)
                    {
                        List<UserRingStationInfo> infos = (from pair in list
                                                           orderby pair.Total descending
                                                           select pair).ToList();
                        for (int i = 0; i < infos.Count; i++)
                        {
                            UserRingStationInfo ring = infos[i];
                            ring.Rank = i + 1;
                            UpdateRingStationInfo(ring);
                        }

                        ConfigInfo.IsFirstUpdateRank = false;
                        saveToDb = true;
                    }

                    m_ranks = (from pair in list
                               where pair.Rank != 0
                               orderby pair.Rank ascending
                               select pair).ToList();

                    if (m_ranks.Count > 0)
                    {
                        UserRingStationInfo champion = m_ranks[0];
                        if (champion.Info != null)
                        {
                            ConfigInfo.ChampionText = champion.Info.NickName;
                            saveToDb = true;
                        }
                    }

                    if (ConfigInfo.IsEndTime())
                    {
                        lock (m_lock)
                        {
                            m_congfig.AwardTime = DateTime.Now.AddDays(3);
                            saveToDb = true;
                        }

                        foreach (UserRingStationInfo p in list)
                        {
                            p.ReardEnable = true;
                            UpdateRingStationInfo(p);
                        }
                    }
                }

                Thread.CurrentThread.Priority = oldprio;
                log.Info("End Scan RingStation Info....");
            }
            catch (Exception e)
            {
                log.Error("StatusScan ", e);
            }
        }

        public static void StopAllTimer()
        {
            if (m_statusScanTimer != null)
            {
                m_statusScanTimer.Change(Timeout.Infinite, Timeout.Infinite);
                m_statusScanTimer.Dispose();
                m_statusScanTimer = null;
            }
        }

        #endregion

        #region Virtual Player Setup

        public static bool SetupVirtualPlayer()
        {
            int[] weaponArr = { 7001, 7002, 7003, 7005, 7006, 7007, 7008, 7009, 7010, 7011, 7012, 7013, 7014 };
            int[] headArr = { 1119, 1104, 1105, 1112, 1113, 1122, 1126, 1136, 1137, 1138, 1140, 1141 };
            int[] glassArr = { 2102, 2103, 2105, 2106, 2108, 2109, 2116, 2117, 2120, 2121, 2122, 2123 };
            int[] hairArr = { 3102, 3103, 3104, 3105, 3106, 3107, 3108, 3109, 3110, 3111, 3112, 3113, 3114, 3115, 3116 };
            int[] effArr = { 4101, 4102, 4103, 4104, 4105, 4106, 4107, 4108, 4109, 4110, 4111, 4112, 4113, 4114, 4115, 4116 };
            int[] clothArr = { 5117, 5102, 5103, 5104, 5105, 5106, 5107, 5108, 5109, 5110, 5111, 5112, 5113, 5114, 5115, 5116 };
            int[] faceArr = { 6101, 6102, 6103, 6104, 6105, 6106, 6107, 6108, 6109, 6110, 6111, 6112, 6113, 6114, 6115, 6116 };
            int[] wingArr = { 15002, 15003, 15004, 15005, 15006, 15007, 15008, 15009 };

            int count = weaponArr.Length;
            int h = 0, g = 0, ha = 0, e = 0, c = 0, f = 0, w = 0;

            for (int i = 0; i < count; i++)
            {
                ItemTemplateInfo temwe = ItemMgr.FindItemTemplate(weaponArr[i]);
                ItemTemplateInfo temhe = ItemMgr.FindItemTemplate(headArr[h]);
                ItemTemplateInfo temgl = ItemMgr.FindItemTemplate(glassArr[g]);
                ItemTemplateInfo temha = ItemMgr.FindItemTemplate(hairArr[ha]);
                ItemTemplateInfo temef = ItemMgr.FindItemTemplate(effArr[e]);
                ItemTemplateInfo temcl = ItemMgr.FindItemTemplate(clothArr[c]);
                ItemTemplateInfo temfa = ItemMgr.FindItemTemplate(faceArr[f]);
                ItemTemplateInfo temwi = ItemMgr.FindItemTemplate(wingArr[w]);

                if (temwe != null && temhe != null && temgl != null && temha != null &&
                    temef != null && temcl != null && temfa != null && temwi != null)
                {
                    string style = $"{headArr[h]}|{temhe.Pic}," +
                                   $"{glassArr[g]}|{temgl.Pic}," +
                                   $"{hairArr[ha]}|{temha.Pic}," +
                                   $"{effArr[e]}|{temef.Pic}," +
                                   $"{clothArr[c]}|{temcl.Pic}," +
                                   $"{faceArr[f]}|{temfa.Pic}," +
                                   $"{weaponArr[i]}|{temwe.Pic},,{wingArr[w]}|{temwi.Pic},,,,,,,,,";

                    m_vplayers.Add(new VirtualPlayerInfo { Style = style, Weapon = weaponArr[i] });
                }

                h = (h + 1) % headArr.Length;
                g = (g + 1) % glassArr.Length;
                ha = (ha + 1) % hairArr.Length;
                e = (e + 1) % effArr.Length;
                c = (c + 1) % clothArr.Length;
                f = (f + 1) % faceArr.Length;
                w = (w + 1) % wingArr.Length;
            }

            return m_vplayers.Count > Math.Abs(count / 2);
        }

        public static VirtualPlayerInfo GetVirtualPlayerInfo()
        {
            return m_vplayers[rand.Next(m_vplayers.Count)];
        }

        #endregion

        #region Bot Creation

        /// <summary>
        /// Rank istasyonu meydan okuması için bot oluşturur.
        ///
        /// HP      = oyuncu HP x 3        — bot zor ölsün
        /// Agility = oyuncu Agility x 1   — eşit hız
        /// Attack  = oyuncu Attack  x 0.55-0.67  — az vursun
        /// Defence = oyuncu Defence x 0.55-0.67  — kolay kırılsın
        /// </summary>
        public static int CreateRingStationChallenge(
            UserRingStationInfo player, int roomtype, int gametype)
        {
            int npcId = player.Info.ID;

            BaseRoomRingStation room = new BaseRoomRingStation(RingStationConfiguration.NextRoomId())
            {
                RoomType = roomtype,
                GameType = gametype,
                PickUpNpcId = npcId,
                IsAutoBot = true,
                IsFreedom = false
            };

            BotScaleResult scale = CalculateBotScale(player.Info.Grade);

            VirtualGamePlayer rp = new VirtualGamePlayer
            {
                NickName = player.Info.NickName,
                GP = player.Info.GP > MaxValue ? MaxValue : Convert.ToInt32(player.Info.GP),
                Grade = player.Info.Grade,

                // Saldırı ve savunma düşürülür
                Attack = (int)(player.Info.Attack * scale.AttackMultiplier),
                Defence = (int)(player.Info.Defence * scale.DefenceMultiplier),
                Luck = (int)(player.Info.Luck * scale.AttackMultiplier),

                // Agility oyuncuyla tamamen eşit
                Agility = player.Info.Agility,

                // FightPower referans amaçlı, saldırı çarpanıyla ölçeklenir
                FightPower = (int)(player.Info.FightPower * scale.AttackMultiplier),

                // Can 3 kat
                hp = (int)(player.Info.hp * scale.HpMultiplier),
                BaseBlood = player.Info.hp,

                // Base statlar da aynı kurala tabi
                BaseAttack = (int)(player.BaseDamage * scale.AttackMultiplier),
                BaseDefence = (int)(player.BaseGuard * scale.DefenceMultiplier),

                // BaseAgility oyuncuyla eşit
                BaseAgility = player.BaseEnergy,

                Style = player.Info.Style,
                Colors = player.Info.Colors,
                Hide = player.Info.Hide,
                TemplateID = player.WeaponID,
                StrengthLevel = 1,
                WeaklessGuildProgressStr = weaklessGuildProgressStr,
                ID = npcId
            };

            if (m_server != null)
            {
                AddPlayer(rp.ID, rp);
                room.AddPlayer(rp);
                m_server.AddRoom(room);
            }

            return npcId;
        }

        /// <summary>
        /// Özgür savaş modunda çoklu bot oluşturur.
        /// </summary>
        public static void CreateAutoBot(
            GamePlayer player, int roomtype, int gametype, int npcId, int playerCount)
        {
            BaseRoomRingStation room = new BaseRoomRingStation(RingStationConfiguration.NextRoomId())
            {
                RoomType = roomtype,
                GameType = gametype,
                PickUpNpcId = npcId,
                IsAutoBot = true,
                IsFreedom = true
            };

            BotScaleResult scale = CalculateBotScale(player.PlayerCharacter.Grade);

            for (int x = 0; x < playerCount; x++)
            {
                VirtualGamePlayer rp = new VirtualGamePlayer
                {
                    NickName = _names[rand.Next(_names.Length)],
                    ConsortiaName = "BloodBrother",
                    GP = player.PlayerCharacter.GP,
                    Grade = player.PlayerCharacter.Grade,

                    Attack = (int)(player.PlayerCharacter.Attack * scale.AttackMultiplier),
                    Defence = (int)(player.PlayerCharacter.Defence * scale.DefenceMultiplier),
                    Luck = (int)(player.PlayerCharacter.Luck * scale.AttackMultiplier),

                    // Agility tamamen eşit
                    Agility = player.PlayerCharacter.Agility,

                    FightPower = (int)(player.PlayerCharacter.FightPower * scale.AttackMultiplier),

                    // Can 3 kat
                    hp = (int)(player.PlayerCharacter.hp * scale.HpMultiplier),

                    BaseAttack = (int)(player.GetBaseAttack() * scale.AttackMultiplier),
                    BaseDefence = (int)(player.GetBaseDefence() * scale.DefenceMultiplier),

                    // BaseAgility tamamen eşit
                    BaseAgility = player.GetBaseAgility(),
                    BaseBlood = (int)(player.GetBaseBlood() * scale.HpMultiplier),

                    badgeID = player.PlayerCharacter.badgeID,
                    WeaklessGuildProgressStr = weaklessGuildProgressStr
                };

                VirtualPlayerInfo vp = GetVirtualPlayerInfo();
                rp.Style = vp.Style;
                rp.Colors = ",,,,,,,,,,,,,,,";
                rp.Hide = 1111111111;
                rp.TemplateID = vp.Weapon;
                rp.StrengthLevel = player.MainWeapon.StrengthenLevel;
                rp.ID = RingStationConfiguration.NextPlayerID();

                AddPlayer(rp.ID, rp);
                room.AddPlayer(rp);
            }

            m_server?.AddRoom(room);
        }

        /// <summary>
        /// Kuyruğa alınmış meydan okumalar için tek bot oluşturur.
        /// </summary>
        public static int GetAutoBot(
            GamePlayer player, int roomtype, int gametype, int playerCount)
        {
            int npcId = RingStationConfiguration.NextPlayerID();

            BaseRoomRingStation room = new BaseRoomRingStation(RingStationConfiguration.NextRoomId())
            {
                RoomType = roomtype,
                GameType = gametype,
                PickUpNpcId = npcId,
                IsAutoBot = true,
                IsFreedom = false
            };

            BotScaleResult scale = CalculateBotScale(player.PlayerCharacter.Grade);

            for (int x = 0; x < playerCount; x++)
            {
                VirtualGamePlayer rp = new VirtualGamePlayer
                {
                    GP = player.PlayerCharacter.GP > MaxValue
                                    ? MaxValue
                                    : Convert.ToInt32(player.PlayerCharacter.GP),
                    Grade = player.PlayerCharacter.Grade,

                    Attack = (int)(player.PlayerCharacter.Attack * scale.AttackMultiplier),
                    Defence = (int)(player.PlayerCharacter.Defence * scale.DefenceMultiplier),
                    Luck = (int)(player.PlayerCharacter.Luck * scale.AttackMultiplier),

                    // Agility tamamen eşit
                    Agility = player.PlayerCharacter.Agility,

                    FightPower = (int)(player.PlayerCharacter.FightPower * scale.AttackMultiplier),

                    // Can 3 kat
                    hp = (int)(player.PlayerCharacter.hp * scale.HpMultiplier),

                    BaseAttack = (int)(player.GetBaseAttack() * scale.AttackMultiplier),
                    BaseDefence = (int)(player.GetBaseDefence() * scale.DefenceMultiplier),

                    // BaseAgility tamamen eşit
                    BaseAgility = player.GetBaseAgility(),
                    BaseBlood = player.GetBaseBlood() * scale.HpMultiplier,

                    NickName = _names[rand.Next(_names.Length)] + npcId + x
                };

                VirtualPlayerInfo vp = GetVirtualPlayerInfo();
                rp.Style = vp.Style;
                rp.Colors = ",,,,,,,,,,,,,,,";
                rp.Hide = 1111112223;
                rp.TemplateID = vp.Weapon;
                rp.StrengthLevel = 0;
                rp.WeaklessGuildProgressStr = weaklessGuildProgressStr;
                rp.ID = npcId + x;

                AddPlayer(rp.ID, rp);
                room.AddPlayer(rp);
            }

            m_server?.AddRoom(room);
            return npcId;
        }

        /// <summary>
        /// Tutorial / ilk karşılaşma için sabit statlarla temel bot oluşturur.
        /// Ölçekleme uygulanmaz.
        /// </summary>
        public static void CreateBaseAutoBot(int roomtype, int gametype, int npcId)
        {
            BaseRoomRingStation room = new BaseRoomRingStation(RingStationConfiguration.NextRoomId())
            {
                RoomType = roomtype,
                GameType = gametype,
                PickUpNpcId = npcId,
                IsAutoBot = true,
                IsFreedom = true
            };

            VirtualGamePlayer rp = new VirtualGamePlayer
            {
                NickName = _names[rand.Next(_names.Length)] + npcId,
                GP = 1283,
                Grade = 5,
                Attack = 60,    // 100 / 1.5 ≈ 67, biraz daha düşük tutuldu
                Defence = 60,
                Luck = 60,
                Agility = 100,   // Eşit agility
                hp = 9000,  // 3000 x 3
                FightPower = 800,
                BaseAttack = 133,   // 200 / 1.5 ≈ 133
                BaseDefence = 80,    // 120 / 1.5 = 80
                BaseAgility = 240,   // Eşit
                BaseBlood = 3000
            };

            VirtualPlayerInfo vp = GetVirtualPlayerInfo();
            rp.Style = vp.Style;
            rp.Colors = ",,,,,,,,,,,,,,,";
            rp.Hide = 1111111111;
            rp.TemplateID = vp.Weapon;
            rp.StrengthLevel = 0;
            rp.WeaklessGuildProgressStr =
                "R/O/DeABAtgWdWsIAAAAAAAAgCAECwAAAAAAABgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=";
            rp.ID = npcId;

            if (m_server != null)
            {
                AddPlayer(rp.ID, rp);
                room.AddPlayer(rp);
                m_server.AddRoom(room);
            }
        }

        /// <summary>
        /// Rank dışı / yeni oyuncular için varsayılan bot profili oluşturur.
        /// </summary>
        public static UserRingStationInfo BaseRingStationChallenges(int id)
        {
            UserRingStationInfo ur = new UserRingStationInfo
            {
                Rank = 0,
                WeaponID = NormalPlayer.Weapon,
                signMsg = LanguageMgr.GetTranslation("BaseRingStationChallenges.Msg2"),
                // BaseDamage ve BaseGuard da 1.5 kat düşürülür, can 3 kat artırılır
                BaseDamage = 161,  // 242 / 1.5 ≈ 161
                BaseGuard = 80,   // 120 / 1.5 = 80
                BaseEnergy = 240   // Agility eşit kalır
            };

            PlayerInfo info = new PlayerInfo
            {
                ID = id == 0 ? RingStationConfiguration.NextPlayerID() : id,
                UserName = "NormalInfo",
                NickName = LanguageMgr.GetTranslation("BaseRingStationChallenges.Msg1"),
                typeVIP = 1,
                VIPLevel = 1,
                Grade = 25,
                Sex = false,
                Style = NormalPlayer.Style,
                Colors = ",,,,,,,,,,,,,,,",
                Skin = "",
                ConsortiaName = "",
                Hide = 1111111111,
                Offer = 0,
                Win = 0,
                Total = 0,
                Escape = 0,
                Repute = 0,
                Nimbus = 0,
                GP = 1437053,
                FightPower = 9580,   // 14370 / 1.5 ≈ 9580
                AchievementPoint = 0,
                Attack = 150,    // 225 / 1.5 = 150
                Defence = 107,    // 160 / 1.5 ≈ 107
                Agility = 50,     // Eşit kalır
                Luck = 40,     // 60 / 1.5 = 40
                hp = 10500,  // 3500 x 3 = 10500
                IsAutoBot = true
            };

            ur.Info = info;
            return ur;
        }

        #endregion
    }
}