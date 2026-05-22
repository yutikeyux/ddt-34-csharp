using Game.Logic;
using Game.Server.Battle;
using Game.Server.Packets;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Game.Server.Rooms
{
    /// <summary>
    /// Tüm oyun odalarýný, bekleme odasýný ve oda iþlemlerinin
    /// sýralý yürütüldüðü aksiyon kuyruðunu yönetir.
    /// </summary>
    public static class RoomMgr
    {
        // -----------------------------------------------------------------------
        // ALANLAR
        // -----------------------------------------------------------------------

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static bool m_running;
        private static Queue<IAction> m_actionQueue;
        private static Thread m_thread;
        private static BaseRoom[] m_rooms;
        private static BaseWaitingRoom m_waitingRoom;
        private static BaseChristmasRoom m_christmasRoom;

        /// <summary>Oda iþ parçacýðýnýn uyku aralýðý (ms)</summary>
        public static readonly int THREAD_INTERVAL = 40;

        /// <summary>Boþ odalarýn temizlenme aralýðý (ms)</summary>
        public static readonly int CLEAR_ROOM_INTERVAL = 400;

        private static long m_clearTick = 0L;

        // -----------------------------------------------------------------------
        // ÖZELLÝKLER
        // -----------------------------------------------------------------------

        /// <summary>Tüm oda slotlarýnýn dizisi.</summary>
        public static BaseRoom[] Rooms => m_rooms;

        /// <summary>Oyuncularýn oda beklerken bulunduðu lobi.</summary>
        public static BaseWaitingRoom WaitingRoom => m_waitingRoom;

        /// <summary>Noel etkinlik odasý.</summary>
        public static BaseChristmasRoom ChristmasRoom => m_christmasRoom;

        /// <summary>Dünya Boss odasý.</summary>
        public static BaseWorldBossRoom WorldBossRoom { get; private set; }

        // -----------------------------------------------------------------------
        // KURULUM VE YAÞAM DÖNGÜSÜ
        // -----------------------------------------------------------------------

        /// <summary>
        /// RoomMgr'ý belirtilen maksimum oda sayýsýyla baþlatýr.
        /// maxRoom en az 1 olmalýdýr.
        /// </summary>
        public static bool Setup(int maxRoom)
        {
            maxRoom = Math.Max(1, maxRoom);

            m_actionQueue = new Queue<IAction>();
            m_rooms = new BaseRoom[maxRoom];
            m_waitingRoom = new BaseWaitingRoom();
            WorldBossRoom = new BaseWorldBossRoom();
            m_christmasRoom = new BaseChristmasRoom();

            for (int i = 0; i < maxRoom; i++)
                m_rooms[i] = new BaseRoom(i + 1);

            // Thread Setup'ta oluþturulur; Start() çaðrýlana kadar baþlamaz.
            m_thread = new Thread(RoomThread)
            {
                Name = "RoomMgrThread",
                Priority = ThreadPriority.Highest,
                IsBackground = true
            };

            return true;
        }

        /// <summary>Oda iþ parçacýðýný baþlatýr.</summary>
        public static void Start()
        {
            if (!m_running)
            {
                m_running = true;
                m_thread.Start();
            }
        }

        /// <summary>Oda iþ parçacýðýný durdurur ve bitmesini bekler.</summary>
        public static void Stop()
        {
            if (m_running)
            {
                m_running = false;
                m_thread.Join();
            }
        }

        // -----------------------------------------------------------------------
        // ODA ÝÞ PARÇACIÐI
        // -----------------------------------------------------------------------

        private static void RoomThread()
        {
            // Priority Setup'ta atandý; burada tekrar atamaya gerek yok.
            long sleepDebt = 0L;
            m_clearTick = TickHelper.GetTickCount();

            while (m_running)
            {
                long tickStart = TickHelper.GetTickCount();
                int processed = 0;

                try
                {
                    processed = ExecuteActions();

                    if (m_clearTick <= tickStart)
                    {
                        m_clearTick += CLEAR_ROOM_INTERVAL;
                        ClearRooms(tickStart);
                    }
                }
                catch (Exception ex)
                {
                    log.Error("RoomMgr thread hatasý:", ex);
                }

                long elapsed = TickHelper.GetTickCount() - tickStart;

                if (elapsed > THREAD_INTERVAL)
                    log.WarnFormat("RoomMgr çok uzun sürdü: {0} ms, iþlem: {1}", elapsed, processed);

                sleepDebt += THREAD_INTERVAL - elapsed;

                if (sleepDebt > 0)
                {
                    Thread.Sleep((int)sleepDebt);
                    sleepDebt = 0L;
                }
                else if (sleepDebt < -1000)
                {
                    // Birikmiþ gecikmeyi kademeli olarak telafi et
                    sleepDebt += 1000;
                }
            }
        }

        /// <summary>
        /// Kuyruktaki tüm aksiyonlarý snapshot alarak yürütür.
        /// Kuyruk kilidi yalnýzca snapshot alýnýrken tutulur; yürütme sýrasýnda serbest býrakýlýr.
        /// </summary>
        private static int ExecuteActions()
        {
            IAction[] snapshot = null;

            lock (m_actionQueue)
            {
                if (m_actionQueue.Count > 0)
                {
                    snapshot = new IAction[m_actionQueue.Count];
                    m_actionQueue.CopyTo(snapshot, 0);
                    m_actionQueue.Clear();
                }
            }

            if (snapshot == null) return 0;

            foreach (IAction action in snapshot)
            {
                try
                {
                    long before = TickHelper.GetTickCount();
                    action.Execute();
                    long duration = TickHelper.GetTickCount() - before;

                    if (duration > THREAD_INTERVAL)
                        log.WarnFormat("Aksiyon çok uzun sürdü: {0} — {1} ms", action.GetType().Name, duration);
                }
                catch (Exception ex)
                {
                    log.Error("Aksiyon yürütme hatasý:", ex);
                }
            }

            return snapshot.Length;
        }

        // -----------------------------------------------------------------------
        // ODA TEMÝZLEME
        // -----------------------------------------------------------------------

        /// <summary>Oyuncusu kalmamýþ aktif odalarý durdurur.</summary>
        public static void ClearRooms(long tick)
        {
            foreach (BaseRoom room in m_rooms)
            {
                if (room.IsUsing && room.PlayerCount == 0)
                    room.Stop();
            }
        }

        /// <summary>Aktif Dungeon odalarýný kilitli biçimde temizler.</summary>
        public static void ClearPveRooms()
        {
            lock (m_rooms)
            {
                foreach (BaseRoom room in m_rooms)
                {
                    if (room.IsUsing && room.RoomType == eRoomType.Dungeon)
                        room.Stop();
                }
            }
        }

        // -----------------------------------------------------------------------
        // AKSÝYON KUYRUÐU
        // -----------------------------------------------------------------------

        /// <summary>Verilen aksiyonu iþ parçacýðýnýn kuyruðuna ekler.</summary>
        public static void AddAction(IAction action)
        {
            lock (m_actionQueue)
            {
                m_actionQueue.Enqueue(action);
            }
        }

        // -----------------------------------------------------------------------
        // AKSÝYON KOLAYLAÞTIRICI METODlar
        // -----------------------------------------------------------------------

        public static void FakeRoom(string roomName, int playerCount, int maxPlayerCount, int roomType)
            => AddAction(new FakeRoomAction(roomName, playerCount, maxPlayerCount, roomType));

        public static void CreateRoom(GamePlayer player, string name, string password, eRoomType roomType, byte timeType)
            => AddAction(new CreateRoomAction(player, name, password, roomType, timeType));

        public static void EnterRoom(GamePlayer player, int roomId, string pwd, int type, bool isInvite)
            => AddAction(new EnterRoomAction(player, roomId, pwd, type, isInvite));

        public static void ExitRoom(BaseRoom room, GamePlayer player)
            => AddAction(new ExitRoomAction(room, player));

        public static void StartGame(BaseRoom room)
            => AddAction(new StartGameAction(room));

        public static void StartGameMission(BaseRoom room)
            => AddAction(new StartGameMissionAction(room));

        public static void UpdatePlayerState(GamePlayer player, byte state)
            => AddAction(new UpdatePlayerStateAction(player, player.CurrentRoom, state));

        public static void UpdateRoomPos(BaseRoom room, int pos, bool isOpened, int place, int placeView)
            => AddAction(new UpdateRoomPosAction(room, pos, isOpened, place, placeView));

        public static void KickPlayer(BaseRoom baseRoom, byte index)
            => AddAction(new KickPlayerAction(baseRoom, index));

        public static void EnterWaitingRoom(GamePlayer player)
            => AddAction(new EnterWaitingRoomAction(player));

        public static void ExitWaitingRoom(GamePlayer player)
            => AddAction(new ExitWaitRoomAction(player));

        public static void CancelPickup(BattleServer server, BaseRoom room)
            => AddAction(new CancelPickupAction(server, room));

        public static void UpdateRoomGameType(
            BaseRoom room, eRoomType roomType, byte timeMode, eHardLevel hardLevel,
            int levelLimits, int mapId, string password, string roomName,
            bool isCrosszone, bool isOpenBoss, string pic, int currentFloor)
            => AddAction(new RoomSetupChangeAction(
                room, roomType, timeMode, hardLevel,
                levelLimits, mapId, password, roomName,
                isCrosszone, isOpenBoss, pic, currentFloor));

        internal static void SwitchTeam(GamePlayer player)
            => AddAction(new SwitchTeamAction(player));

        public static void StartProxyGame(BaseRoom room, ProxyGame game)
            => AddAction(new StartProxyGameAction(room, game));

        public static void StopProxyGame(BaseRoom room)
            => AddAction(new StopProxyGameAction(room));

        public static void CreateConsortiaBattleRoom(GamePlayer player)
            => AddAction(new CreateConsortiaBattleRoomAction(player));

        public static void StartConsortiaBattle(BaseRoom red, BaseRoom blue)
            => AddAction(new StartConsortiaBattleRoomAction(red, blue));

        // -----------------------------------------------------------------------
        // ODA LÝSTESÝ SORGU METODlarý
        // -----------------------------------------------------------------------

        /// <summary>Aktif (IsUsing) tüm odalarý döndürür.</summary>
        public static List<BaseRoom> GetAllUsingRoom()
        {
            var list = new List<BaseRoom>();
            lock (m_rooms)
            {
                foreach (BaseRoom room in m_rooms)
                {
                    if (room.IsUsing)
                        list.Add(room);
                }
            }
            return list;
        }

        /// <summary>En az bir oyuncusu olan tüm odalarý döndürür.</summary>
        public static List<BaseRoom> GetAllRooms()
        {
            var list = new List<BaseRoom>();
            lock (m_rooms)
            {
                foreach (BaseRoom room in m_rooms)
                {
                    if (!room.IsEmpty)
                        list.Add(room);
                }
            }
            return list;
        }

        /// <summary>
        /// Belirli bir odayý listenin baþýna koyarak,
        /// host durumuna göre uygun oda tiplerini ekler.
        /// Host Away ise PvE odalarý, deðilse Match odalarý döner.
        /// </summary>
        public static List<BaseRoom> GetAllRooms(BaseRoom referenceRoom)
        {
            var list = new List<BaseRoom>();
            if (referenceRoom == null) return list;

            list.Add(referenceRoom);

            bool isHostAway = referenceRoom.Host != null
                && referenceRoom.Host.PlayerState == ePlayerState.Away;

            list.AddRange(isHostAway ? GetAllPveRooms() : GetAllMatchRooms());

            return list;
        }

        /// <summary>Aktif PvE odalarýný (Dungeon, Academy, Boss) döndürür.</summary>
        public static List<BaseRoom> GetAllPveRooms()
        {
            var list = new List<BaseRoom>();
            lock (m_rooms)
            {
                foreach (BaseRoom room in m_rooms)
                {
                    if (room.IsUsing && IsPveRoomType(room.RoomType))
                        list.Add(room);
                }
            }
            return list;
        }

        /// <summary>Aktif PvP odalarýný (Match, Freedom) döndürür.</summary>
        public static List<BaseRoom> GetAllMatchRooms()
        {
            var list = new List<BaseRoom>();
            lock (m_rooms)
            {
                foreach (BaseRoom room in m_rooms)
                {
                    if (room.IsUsing &&
                        (room.RoomType == eRoomType.Match || room.RoomType == eRoomType.Freedom))
                        list.Add(room);
                }
            }
            return list;
        }

        // -----------------------------------------------------------------------
        // ÖZEL YARDIMCI
        // -----------------------------------------------------------------------

        /// <summary>Oda tipinin PvE kategorisinde olup olmadýðýný döndürür.</summary>
        private static bool IsPveRoomType(eRoomType roomType)
        {
            return roomType == eRoomType.Dungeon
                || roomType == eRoomType.Academy
                || roomType == eRoomType.Boss;
        }
    }
}