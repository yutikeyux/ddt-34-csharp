using Bussiness;
using Game.Base.Packets;
using Game.Logic;
using Game.Logic.Phy.Object;
using Game.Server.Battle;
using Game.Server.Games;
using Game.Server.Packets;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Game.Server.Rooms
{
    public class BaseRoom
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // -----------------------------------------------------------------------
        // ALANLAR (Fields)
        // -----------------------------------------------------------------------

        /// <summary>Odadaki oyuncu slotları (10 slot: 0-7 oyuncu, 8-9 izleyici)</summary>
        private GamePlayer[] m_places;

        /// <summary>Her slotun durumu: -1 = açık/boş, 0 = kapalı, >0 = PlayerId</summary>
        private int[] m_placesState;

        /// <summary>Her slotun oyuncu durumu: 0 = pasif, 1 = hazır, 2 = host</summary>
        private byte[] m_playerState;

        public int m_playerCount = 0;
        public int m_placesCount = 10;

        private bool m_isUsing = false;
        private GamePlayer m_host;

        public bool IsPlaying;
        public bool IsShowLoading;

        public int RoomId;
        public int PickUpNpcId;

        /// <summary>Maksimum izleyici sayısı (varsayılan: 2)</summary>
        public int maxViewerCnt = 2;

        public DateTime horaInicio;

        private int m_viewerCnt = 0;

        public int GameStyle = 0;
        public int barrierNum = 0;

        public string Name;
        public string Pic;
        public string Password;

        public bool isCrosszone;
        public bool isWithinLeageTime;
        public bool isOpenBoss;

        public eRoomType RoomType;
        public eGameType GameType;
        public eHardLevel HardLevel;

        public int LevelLimits;
        public int currentFloor;
        public byte TimeMode;
        public int MapId;

        public string m_roundName;
        public int AreaID;

        private bool m_startWithNpc;
        private int m_avgLevel = 0;

        private AbstractGame m_game;
        public BattleServer BattleServer;

        // -----------------------------------------------------------------------
        // ÖZELLİKLER (Properties)
        // -----------------------------------------------------------------------

        public int viewerCnt => m_viewerCnt;
        public GamePlayer Host => m_host;
        public int PlayerCount => m_playerCount;
        public int PlacesCount => m_placesCount;
        public bool IsUsing => m_isUsing;
        public bool NeedPassword => !string.IsNullOrEmpty(Password);
        public bool IsEmpty => m_playerCount == 0;
        public int AvgLevel => m_avgLevel;
        public AbstractGame Game => m_game;

        public byte[] PlayerState
        {
            get => m_playerState;
            set => m_playerState = value;
        }

        public string RoundName
        {
            get => m_roundName;
            set => m_roundName = value;
        }

        public bool StartWithNpc
        {
            get => m_startWithNpc;
            set => m_startWithNpc = value;
        }

        public int GuildId
        {
            get
            {
                if (m_host == null) return 0;
                return m_host.PlayerCharacter.ConsortiaID;
            }
        }

        // -----------------------------------------------------------------------
        // YAPICI (Constructor)
        // -----------------------------------------------------------------------

        public BaseRoom(int roomId)
        {
            RoomId = roomId;
            m_places = new GamePlayer[10];
            m_placesState = new int[10];
            m_playerState = new byte[10];
            IsShowLoading = false;
            PickUpNpcId = -1;
            Reset();
        }

        // -----------------------------------------------------------------------
        // ODA YAŞAM DÖNGÜSÜ
        // -----------------------------------------------------------------------

        /// <summary>Odayı aktif hale getirir ve sıfırlar.</summary>
        public void Start()
        {
            if (!m_isUsing)
            {
                m_isUsing = true;
                Reset();
            }
        }

        /// <summary>Odayı kapatır, oyunu temizler ve bekleme odasını günceller.</summary>
        public void Stop()
        {
            if (m_isUsing)
            {
                m_isUsing = false;
                if (m_game != null)
                {
                    m_game.GameStopped -= OnGameStopped;
                    m_game = null;
                    IsPlaying = false;
                }
                RoomMgr.WaitingRoom.SendUpdateCurrentRoom(this);
            }
        }

        /// <summary>Tüm slot ve oda verilerini varsayılan değerlerine döndürür.</summary>
        private void Reset()
        {
            for (int i = 0; i < 10; i++)
            {
                m_places[i] = null;
                m_placesState[i] = -1;
                m_playerState[i] = 0;
            }
            m_host = null;
            IsPlaying = false;
            m_placesCount = 10;
            m_playerCount = 0;
            m_viewerCnt = 0;
            isCrosszone = false;
            HardLevel = eHardLevel.Simple;
            PickUpNpcId = -1;
            StartWithNpc = false;
            Pic = string.Empty;
            MapId = 10000;
            currentFloor = 0;
            isOpenBoss = false;
        }

        // -----------------------------------------------------------------------
        // KAPASİTE KONTROL METODları
        // -----------------------------------------------------------------------

        /// <summary>Odaya normal oyuncu olarak yer var mı?</summary>
        public bool CanAddPlayer()
        {
            return m_playerCount < m_placesCount;
        }

        /// <summary>
        /// Oda tipine göre izleyici slotu boş mu?
        /// - Freedom : Ayrı izleyici slotu yok; genel kapasite kontrolü yapılır.
        /// - Match    : Slot 4 izleyici slotudur (1 izleyici).
        /// - Dungeon  : Slot 4 ve 5 izleyici slotlarıdır (2 izleyici).
        /// - Diğer    : Slot 8 ve 9 izleyici slotlarıdır (2 izleyici).
        /// </summary>
        public bool CanAddViewPlayer()
        {
            switch (RoomType)
            {
                case eRoomType.Freedom:
                    return CanAddPlayer();

                case eRoomType.Match:
                    return m_playerState[4] <= 0;

                case eRoomType.Dungeon:
                    return m_playerState[4] <= 0 || m_playerState[5] <= 0;

                default:
                    return m_playerState[8] <= 0 || m_playerState[9] <= 0;
            }
        }

        /// <summary>
        /// Odanın başlatılıp başlatılamayacağını kontrol eder.
        /// Her oda tipi için normal oyuncu ve izleyici sayısının tutarlı olup olmadığı doğrulanır.
        /// </summary>
        public bool CanStart()
        {
            switch (RoomType)
            {
                case eRoomType.Freedom:
                    {
                        // İki takımın da en az bir oyuncusu olmalı
                        int team1 = 0, team2 = 0;
                        for (int i = 0; i < 10; i++)
                        {
                            if (m_playerState[i] > 0)
                            {
                                if (i % 2 == 0) team1++;
                                else team2++;
                            }
                        }
                        return team1 > 0 && team2 > 0;
                    }

                case eRoomType.Match:
                    {
                        // 4 normal slot (0-3) + 1 izleyici slotu (4)
                        int normalCount = 0;
                        int viewerCount = 0;
                        for (int i = 0; i < 5; i++)
                        {
                            if (m_playerState[i] > 0)
                            {
                                if (i < 4) normalCount++;
                                else viewerCount++;
                            }
                        }
                        return normalCount == m_playerCount && viewerCount == m_viewerCnt;
                    }

                case eRoomType.Dungeon:
                    {
                        // 4 normal slot (0-3) + 2 izleyici slotu (4-5)
                        int normalCount = 0;
                        int viewerCount = 0;
                        for (int i = 0; i < 6; i++)
                        {
                            if (m_playerState[i] > 0)
                            {
                                if (i < 4) normalCount++;
                                else viewerCount++;
                            }
                        }
                        return normalCount == m_playerCount && viewerCount == m_viewerCnt;
                    }

                default:
                    {
                        // 8 normal slot (0-7) + 2 izleyici slotu (8-9)
                        int normalCount = 0;
                        int viewerCount = 0;
                        for (int i = 0; i < 10; i++)
                        {
                            if (m_playerState[i] > 0)
                            {
                                if (i < 8) normalCount++;
                                else viewerCount++;
                            }
                        }
                        return normalCount == m_playerCount && viewerCount == m_viewerCnt;
                    }
            }
        }

        // -----------------------------------------------------------------------
        // OYUNCU LİSTESİ
        // -----------------------------------------------------------------------

        /// <summary>Tüm slotlardaki (oyuncu + izleyici) oyuncuları döndürür.</summary>
        public List<GamePlayer> GetPlayers()
        {
            var list = new List<GamePlayer>();
            lock (m_places)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (m_places[i] != null)
                        list.Add(m_places[i]);
                }
            }
            return list;
        }

        /// <summary>Yalnızca savaşan oyuncuları (slot 0-7) döndürür.</summary>
        public List<GamePlayer> GetPlayersFight()
        {
            var list = new List<GamePlayer>();
            lock (m_places)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (m_places[i] != null)
                        list.Add(m_places[i]);
                }
            }
            return list;
        }

        // -----------------------------------------------------------------------
        // ODA GÜNCELLEME
        // -----------------------------------------------------------------------

        /// <summary>Oda ayarlarını günceller ve oyun tipini yeniden hesaplar.</summary>
        public void UpdateRoom(string name, string pwd, eRoomType roomType, byte timeMode, int mapId)
        {
            Name = name;
            Password = pwd;
            RoomType = roomType;
            TimeMode = timeMode;
            MapId = mapId;

            UpdateRoomGameType();

            m_placesCount = (roomType == eRoomType.Freedom) ? 8 : 4;

            // Kapasite dışında kalan slotları kapat
            for (int i = m_placesCount; i < 10; i++)
            {
                m_placesState[i] = 0;
            }
        }

        /// <summary>Oda tipine göre oyun tipini belirler.</summary>
        public void UpdateRoomGameType()
        {
            switch (RoomType)
            {
                case eRoomType.FightLab:
                    GameType = eGameType.FightLab;
                    break;
                case eRoomType.Boss:
                case eRoomType.Dungeon:
                case eRoomType.Academy:
                case eRoomType.Christmas:
                case eRoomType.Labyrinth:
                    GameType = eGameType.Dungeon;
                    break;
                case eRoomType.Freshman:
                    GameType = eGameType.Freshman;
                    break;
                case eRoomType.Match:
                case eRoomType.Freedom:
                    GameType = eGameType.Free;
                    break;
                case eRoomType.ConsortiaBattle:
                    GameType = eGameType.ConsortiaBattle;
                    break;
                default:
                    GameType = eGameType.ALL;
                    break;
            }
        }

        /// <summary>Belirtilen oyuncunun durum baytını günceller; istenirse tüm odaya yayınlar.</summary>
        public void UpdatePlayerState(GamePlayer player, byte state, bool sendToClient)
        {
            m_playerState[player.CurrentRoomIndex] = state;
            if (sendToClient)
                SendPlayerState();
        }

        /// <summary>Ortalama seviyeyi savaşan oyuncular (slot 0-7) üzerinden hesaplar.</summary>
        public void UpdateAvgLevel()
        {
            int total = 0;
            for (int i = 0; i < 8; i++)
            {
                if (m_places[i] != null)
                    total += m_places[i].PlayerCharacter.Grade;
            }
            if (m_playerCount > 0 && total > 0)
                m_avgLevel = total / m_playerCount;
        }

        /// <summary>
        /// Oda, Match tipindeyse tüm oyuncuların aynı loncadan olup olmadığına göre
        /// oyun stilini (Normal/Guild) günceller.
        /// </summary>
        public void UpdateGameStyle()
        {
            if (m_host == null || RoomType != eRoomType.Match) return;

            if (IsAllSameGuild())
            {
                GameStyle = 1;
                GameType = eGameType.Guild;
            }
            else
            {
                GameStyle = 0;
                GameType = eGameType.Free;
            }

            GSPacketIn pkg = m_host.Out.SendRoomType(m_host, this);
            SendToAll(pkg);
        }

        /// <summary>Odadaki tüm oyuncular aynı loncadan mı?</summary>
        public bool IsAllSameGuild()
        {
            int guildId = GuildId;
            if (guildId == 0) return false;

            List<GamePlayer> players = GetPlayers();
            if (players.Count < 2) return false;

            foreach (GamePlayer p in players)
            {
                if (p.PlayerCharacter.ConsortiaID != guildId)
                    return false;
            }
            return true;
        }

        /// <summary>Host oyuncusunu değiştirir ve durum baytlarını günceller.</summary>
        public void SetHost(GamePlayer player)
        {
            if (m_host == player) return;

            if (m_host != null)
                UpdatePlayerState(m_host, 0, sendToClient: false);

            m_host = player;
            UpdatePlayerState(player, 2, sendToClient: true);
        }

        // -----------------------------------------------------------------------
        // PAKET GÖNDERİM METODları
        // -----------------------------------------------------------------------

        public void SendToAll(GSPacketIn pkg, IGamePlayer except) { }

        public void SendToAll(GSPacketIn pkg)
        {
            SendToAll(pkg, (GamePlayer)null);
        }

        public void SendToAll(GSPacketIn pkg, GamePlayer except)
        {
            GamePlayer[] snapshot;
            lock (m_places)
            {
                snapshot = (GamePlayer[])m_places.Clone();
            }

            foreach (GamePlayer p in snapshot)
            {
                if (p != null && p != except)
                    p.Out.SendTCP(pkg);
            }
        }

        public void SendToTeam(GSPacketIn pkg, int team)
        {
            SendToTeam(pkg, team, null);
        }

        public void SendToTeam(GSPacketIn pkg, int team, GamePlayer except)
        {
            GamePlayer[] snapshot;
            lock (m_places)
            {
                snapshot = (GamePlayer[])m_places.Clone();
            }

            foreach (GamePlayer p in snapshot)
            {
                if (p != null && p != except && p.CurrentRoomTeam == team)
                    p.Out.SendTCP(pkg);
            }
        }

        public void SendToHost(GSPacketIn pkg)
        {
            m_host?.Out.SendTCP(pkg);
        }

        public void SendPlayerState()
        {
            if (m_host == null) return;
            GSPacketIn pkg = m_host.Out.SendRoomUpdatePlayerStates(m_playerState);
            SendToAll(pkg, m_host);
        }

        public void SendPlaceState()
        {
            if (m_host == null) return;
            GSPacketIn pkg = m_host.Out.SendRoomUpdatePlacesStates(m_placesState);
            SendToAll(pkg, m_host);
        }

        public void SendCancelPickUp()
        {
            if (m_host == null) return;
            GSPacketIn pkg = m_host.Out.SendRoomPairUpCancel(this);
            SendToAll(pkg, m_host);
        }

        public void SendStartPickUp()
        {
            if (m_host == null) return;
            GSPacketIn pkg = m_host.Out.SendRoomPairUpStart(this);
            SendToAll(pkg, m_host);
        }

        public void SendMessage(eMessageType type, string msg)
        {
            if (m_host == null) return;
            GSPacketIn pkg = m_host.Out.SendMessage(type, msg);
            SendToAll(pkg, m_host);
        }

        public void SendRoomSetupChange(BaseRoom room)
        {
            if (m_host == null) return;
            GSPacketIn pkg = m_host.Out.SendGameRoomSetupChange(room);
            SendToAll(pkg, m_host);
        }

        // -----------------------------------------------------------------------
        // OYUNCU EKLEME / ÇIKARMA
        // -----------------------------------------------------------------------

        /// <summary>
        /// Kampanya savaşı için oyuncu ekler.
        /// Slot 0-7 arasında ilk boş yeri bulur; host yoksa bu oyuncuyu host yapar.
        /// </summary>
        public bool AddPlayerCampBattle(GamePlayer player)
        {
            int slotIndex = -1;
            lock (m_places)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (m_places[i] == null && m_placesState[i] == -1)
                    {
                        m_places[i] = player;
                        m_placesState[i] = player.PlayerId;
                        m_playerCount++;
                        slotIndex = i;
                        break;
                    }
                }
            }

            if (slotIndex == -1) return false;

            player.CurrentRoom = this;
            player.CurrentRoomIndex = slotIndex;
            player.CurrentRoomTeam = 1;

            if (m_host == null)
            {
                m_host = player;
                UpdatePlayerState(player, 2, sendToClient: false);
            }
            else
            {
                UpdatePlayerState(player, 0, sendToClient: false);
            }

            return true;
        }

        /// <summary>
        /// Odaya oyuncu ekler. Slot 8-9 izleyici slotlarıdır.
        /// Eklenen oyuncuya ve odadaki diğer oyunculara bildirim paketi gönderilir.
        /// </summary>
        public bool AddPlayerUnsafe(GamePlayer player)
        {
            int slotIndex = -1;
            lock (m_places)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (m_places[i] == null && m_placesState[i] == -1)
                    {
                        m_places[i] = player;
                        m_placesState[i] = player.PlayerId;

                        if (i < 8) m_playerCount++;
                        else m_viewerCnt++;

                        slotIndex = i;
                        break;
                    }
                }
            }

            // Başarısız ekleme durumunda erken çık
            if (slotIndex == -1)
            {
                player.IsViewer = false;
                return false;
            }

            player.CurrentRoom = this;
            player.CurrentRoomIndex = slotIndex;
            player.IsViewer = slotIndex >= 8;

            if (slotIndex >= 8)
            {
                // İzleyici slotu
                player.CurrentRoomTeam = 99;
            }
            else if (RoomType == eRoomType.Freedom)
            {
                // Freedom: çift slot = takım 1, tek slot = takım 2
                player.CurrentRoomTeam = slotIndex % 2 + 1;
            }
            else
            {
                player.CurrentRoomTeam = 1;
            }

            // Yeni oyuncuyu odakilere, odakileri yeni oyuncuya tanıt
            GSPacketIn addPkg = player.Out.SendRoomPlayerAdd(player);
            SendToAll(addPkg, player);

            GSPacketIn bufPkg = player.Out.SendBufferList(player, player.BufferList.GetAllBuffer());
            SendToAll(bufPkg, player);

            List<GamePlayer> existing = GetPlayers();
            foreach (GamePlayer other in existing)
            {
                if (other == player) continue;
                player.Out.SendRoomPlayerAdd(other);
                player.Out.SendBufferList(other, other.BufferList.GetAllBuffer());
            }

            // Host ataması
            if (m_host == null)
            {
                m_host = player;
                UpdatePlayerState(player, 2, sendToClient: true);
            }
            else
            {
                UpdatePlayerState(player, 0, sendToClient: true);
            }

            SendPlaceState();
            UpdateGameStyle();

            if (RoomType == eRoomType.ConsortiaBattle)
                GameMgr.GuildBattle.AddPlayer(player);

            return true;
        }

        public bool RemovePlayerUnsafe(GamePlayer player)
        {
            return RemovePlayerUnsafe(player, isKick: false);
        }

        /// <summary>
        /// Oyuncuyu odadan çıkarır.
        /// Oyun oynanıyorsa oyun motoruna bildirir; host çıktıysa yeni host atar.
        /// </summary>
        public bool RemovePlayerUnsafe(GamePlayer player, bool isKick)
        {
            int slotIndex = -1;
            lock (m_places)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (m_places[i] == player)
                    {
                        m_places[i] = null;
                        m_playerState[i] = 0;
                        m_placesState[i] = -1;

                        if (i < 8) m_playerCount--;
                        else m_viewerCnt--;

                        slotIndex = i;
                        break;
                    }
                }
            }

            if (slotIndex == -1) return false;

            UpdatePosUnsafe(slotIndex, isOpened: false, place: -1, placeView: -100);
            player.CurrentRoom = null;
            player.TempBag.ClearBag();

            GSPacketIn removePkg = player.Out.SendRoomPlayerRemove(player);
            SendToAll(removePkg);

            if (isKick)
            {
                player.Out.SendMessage(eMessageType.ChatERROR,
                    LanguageMgr.GetTranslation("Game.Server.SceneGames.KickRoom"));
            }

            // Host değişimi
            bool hostChanged = false;
            if (m_host == player)
            {
                if (m_playerCount > 0 || m_viewerCnt > 0)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (m_places[j] != null)
                        {
                            SetHost(m_places[j]);
                            hostChanged = true;
                            break;
                        }
                    }
                }
                else
                {
                    m_host = null;
                }
            }

            if (IsPlaying)
            {
                // Geçici görünüm varsa güncelle
                if (!string.IsNullOrEmpty(player.PlayerCharacter.tempStyle))
                    player.UpdatePublicPlayer();

                if (m_game != null)
                {
                    // Yeni host PVE oyunundaysa hazır durumunu sıfırla
                    if (hostChanged && m_game is PVEGame pveGame)
                    {
                        foreach (Player gamePlayer in pveGame.Players.Values)
                        {
                            if (gamePlayer.PlayerDetail == m_host)
                                gamePlayer.Ready = false;
                        }
                    }
                    m_game.RemovePlayer(player, isKick);
                }

                if (BattleServer != null)
                {
                    if (m_game != null)
                    {
                        BattleServer.Server.SendPlayerDisconnet(Game.Id, player.TempGameId, RoomId);
                        if (PlayerCount == 0)
                            BattleServer.RemoveRoom(this);
                    }
                    else
                    {
                        SendMessage(eMessageType.ChatERROR,
                            LanguageMgr.GetTranslation("Game.Server.SceneGames.PairUp.Failed"));
                        RoomMgr.AddAction(new CancelPickupAction(BattleServer, this));
                        BattleServer.RemoveRoom(this);
                        IsPlaying = false;
                    }
                }
            }
            else
            {
                UpdateGameStyle();

                // Host değiştiyse zorluk seviyesini sıfırla ve oda ayarlarını gönder
                if (hostChanged)
                {
                    HardLevel = (RoomType == eRoomType.Dungeon)
                        ? eHardLevel.Normal
                        : eHardLevel.Simple;

                    foreach (GamePlayer p in GetPlayers())
                        p.Out.SendGameRoomSetupChange(this);
                }
            }

            if (RoomType == eRoomType.ConsortiaBattle)
                GameMgr.GuildBattle.RemovePlayer(player);

            return true;
        }

        /// <summary>Belirtilen slottaki oyuncuyu kick uygulayarak odadan çıkarır.</summary>
        public void RemovePlayerAtUnsafe(int pos)
        {
            if (pos < 0 || pos > 9 || m_places[pos] == null) return;

            if (m_places[pos].KickProtect)
            {
                string msg = LanguageMgr.GetTranslation(
                    "Game.Server.SceneGames.Protect", m_places[pos].PlayerCharacter.NickName);
                GSPacketIn pkt = new GSPacketIn(3);
                pkt.WriteInt(0);
                pkt.WriteString(msg);
                SendToHost(pkt);
            }
            else
            {
                RemovePlayerUnsafe(m_places[pos], isKick: true);
            }
        }

        /// <summary>Tüm oyuncuları odadan çıkarır ve bekleme odasına gönderir.</summary>
        public void RemoveAllPlayer()
        {
            for (int i = 0; i < 10; i++)
            {
                if (m_places[i] != null)
                {
                    RoomMgr.AddAction(new ExitRoomAction(this, m_places[i]));
                    RoomMgr.AddAction(new EnterWaitingRoomAction(m_places[i]));
                }
            }
        }

        // -----------------------------------------------------------------------
        // TAKIM / GÖRÜNÜM DEĞİŞTİRME
        // -----------------------------------------------------------------------

        /// <summary>
        /// Oyuncuyu karşı takıma geçirir (yalnızca Freedom modunda).
        /// Mevcut takımın karşı tarafında boş slot aranır.
        /// </summary>
        public bool SwitchTeamUnsafe(GamePlayer player)
        {
            if (RoomType == eRoomType.Match) return false;

            int newSlot = -1;
            lock (m_places)
            {
                // Mevcut indeksin mod 2 tersi = karşı takımın başlangıç indeksi
                int startIndex = (player.CurrentRoomIndex + 1) % 2;
                for (int i = startIndex; i < 8; i += 2)
                {
                    if (m_places[i] == null && m_placesState[i] == -1)
                    {
                        newSlot = i;
                        m_places[player.CurrentRoomIndex] = null;
                        m_places[i] = player;
                        m_placesState[player.CurrentRoomIndex] = -1;
                        m_placesState[i] = player.PlayerId;
                        m_playerState[i] = m_playerState[player.CurrentRoomIndex];
                        m_playerState[player.CurrentRoomIndex] = 0;
                        break;
                    }
                }
            }

            if (newSlot == -1) return false;

            player.CurrentRoomIndex = newSlot;
            player.CurrentRoomTeam = newSlot % 2 + 1;

            GSPacketIn pkg = player.Out.SendRoomPlayerChangedTeam(player);
            SendToAll(pkg, player);
            SendPlaceState();
            return true;
        }

        /// <summary>
        /// Oyuncu / izleyici slot geçişi (v2 — tam senkronize).
        /// placeView >= 8 ise oyuncu → izleyici, placeView < 8 ise izleyici → oyuncu.
        /// Hedef slot doluysa işlem yapılmaz ve false döner.
        /// </summary>
        public bool SwitchToView(GamePlayer player, int placeView)
        {
            if (placeView < 0 || placeView > 9)
                return false;

            // Hedef slot boş mu?
            if (m_places[placeView] != null || m_placesState[placeView] != -1)
            {
                player.Out.SendMessage(eMessageType.GM_NOTICE,
                    LanguageMgr.GetTranslation("BaseRoom.SlotFull"));
                return false;
            }

            int oldSlot = player.CurrentRoomIndex;

            // Slot verilerini taşı
            m_places[oldSlot] = null;
            m_places[placeView] = player;
            m_placesState[oldSlot] = -1;
            m_placesState[placeView] = player.PlayerId;
            m_playerState[placeView] = m_playerState[oldSlot];
            m_playerState[oldSlot] = 0;

            player.CurrentRoomIndex = placeView;

            if (placeView >= 8)
            {
                // Oyuncu → İzleyici
                player.IsViewer = true;
                player.CurrentRoomTeam = 99;
                m_playerCount--;
                m_viewerCnt++;
            }
            else
            {
                // İzleyici → Oyuncu
                player.IsViewer = false;
                m_playerCount++;
                m_viewerCnt--;

                if (RoomType == eRoomType.Freedom)
                {
                    player.CurrentRoomTeam = placeView % 2 + 1;
                    GSPacketIn teamPkg = player.Out.SendRoomPlayerChangedTeam(player);
                    SendToAll(teamPkg, player);
                }
                else
                {
                    player.CurrentRoomTeam = 1;
                }
            }

            SendPlaceState();
            SendPlayerState();
            return true;
        }

        // -----------------------------------------------------------------------
        // OYUN YAŞAM DÖNGÜSÜ
        // -----------------------------------------------------------------------

        /// <summary>
        /// Yeni bir oyun başlatır. Çalışan oyun varsa önce durdurur.
        /// </summary>
        public void StartGame(AbstractGame game)
        {
            if (m_game != null)
            {
                foreach (GamePlayer p in GetPlayers())
                    m_game.RemovePlayer(p, IsKick: false);

                OnGameStopped(m_game);
            }

            horaInicio = DateTime.Now;
            m_game = game;
            IsPlaying = true;
            m_game.GameStopped += OnGameStopped;
        }

        private void OnGameStopped(AbstractGame game)
        {
            if (game == null) return;

            foreach (GamePlayer p in GetPlayers())
            {
                if (!string.IsNullOrEmpty(p.PlayerCharacter.tempStyle))
                    p.UpdatePublicPlayer();
            }

            m_game.GameStopped -= OnGameStopped;
            m_game = null;
            IsPlaying = false;
            horaInicio = DateTime.MinValue;

            RoomMgr.WaitingRoom.SendUpdateCurrentRoom(this);
        }

        // -----------------------------------------------------------------------
        // YARDIMCI METODlar
        // -----------------------------------------------------------------------

        /// <summary>Slot pozisyonunu açar/kapatır ve kapasite sayaçlarını günceller.</summary>
        public bool UpdatePosUnsafe(int pos, bool isOpened, int place, int placeView)
        {
            if (pos < 0 || pos > 9) return false;

            if (m_placesState[pos] == place) return false;

            if (m_places[pos] != null)
                RemovePlayerUnsafe(m_places[pos]);

            m_placesState[pos] = place;
            SendPlaceState();

            if (place == -1)
            {
                if (pos < 8) m_placesCount++;
                else maxViewerCnt++;
            }
            else if (place == 0)
            {
                if (pos < 8) m_placesCount--;
                else maxViewerCnt--;
            }

            return true;
        }

        /// <summary>Host dışındaki tüm oyuncuların durum baytlarını sıfırlar.</summary>
        public void ResetPlayerState()
        {
            for (int i = 0; i < m_playerState.Length; i++)
            {
                if (m_playerState[i] != 2)
                    m_playerState[i] = 0;
            }
        }

        /// <summary>Oyuncunun seviye aralığını döndürür.</summary>
        public eLevelLimits GetLevelLimit(GamePlayer player)
        {
            int grade = player.PlayerCharacter.Grade;
            if (grade <= 10) return eLevelLimits.ZeroToTen;
            if (grade <= 20) return eLevelLimits.ElevenToTwenty;
            return eLevelLimits.TwentyOneToThirty;
        }

        // -----------------------------------------------------------------------
        // İSİM / BİLGİ METODları
        // -----------------------------------------------------------------------

        public string GetNameByMapId()
        {
            string prefix = LanguageMgr.GetTranslation("BaseRoom.Msg2");
            string name = LanguageMgr.GetTranslation("BaseRoom.Msg1");

            MapInfo mapInfo = MapMgr.FindMapInfo(MapId);
            if (mapInfo != null)
                name = mapInfo.Name;

            PveInfo pveInfo = PveInfoMgr.GetPveInfoById(MapId);
            if (pveInfo != null)
            {
                name = pveInfo.Name + GetNameHardLv();
            }
            else
            {
                prefix = LanguageMgr.GetTranslation("BaseRoom.Msg3");
            }

            return prefix + name;
        }

        public string GetNameHardLv()
        {
            switch (HardLevel)
            {
                case eHardLevel.Normal: return LanguageMgr.GetTranslation("BaseRoom.Msg5");
                case eHardLevel.Hard: return LanguageMgr.GetTranslation("BaseRoom.Msg6");
                case eHardLevel.Terror: return LanguageMgr.GetTranslation("BaseRoom.Msg7");
                default: return LanguageMgr.GetTranslation("BaseRoom.Msg4");
            }
        }

        /// <summary>Belirli harita ID'si için bilet ID'sini döndürür (haritaya özel).</summary>
        public int GetDungeonTicketId(int mapId)
        {
            if (mapId == 12016) return 11742;
            return 0;
        }

        /// <summary>Zorluk seviyesine göre Arena bilet ID'sini döndürür.</summary>
        public int GetDungeonTicketId(eHardLevel level)
        {
            switch (level)
            {
                case eHardLevel.Easy: return 200619;
                case eHardLevel.Normal: return 200620;
                case eHardLevel.Hard: return 200621;
                case eHardLevel.Terror: return 200622;
                default: return 0;
            }
        }

        /// <summary>Zorluk seviyesine göre Dünya Kupası bilet ID'sini döndürür.</summary>
        public int GetCupTicketId(eHardLevel level)
        {
            switch (level)
            {
                case eHardLevel.Easy: return 201279;
                default: return 0;
            }
        }

        /// <summary>Zorluk seviyesine göre Harika Zindan bilet ID'sini döndürür.</summary>
        public int GetWonderDungeonTicketId(eHardLevel level)
        {
            // Tüm seviyelerde aynı bilet kullanılıyor (11573)
            switch (level)
            {
                case eHardLevel.Easy:
                case eHardLevel.Normal:
                case eHardLevel.Hard:
                case eHardLevel.Terror:
                    return 11573;
                default:
                    return 0;
            }
        }

        /// <summary>Oyun verisi paketini aktif oyuna iletir.</summary>
        public void ProcessData(GSPacketIn packet)
        {
            m_game?.ProcessData(packet);
        }

        public override string ToString()
        {
            return $"Id:{RoomId}, Oyuncu:{PlayerCount}, Oyun:{Game}, Oynanıyor:{IsPlaying}";
        }
    }
}