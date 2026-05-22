using Bussiness;
using Game.Base.Packets;
using Game.Logic;
using Game.Server.Battle;
using Game.Server.Games;
using Game.Server.Packets;
using Game.Server.RingStation;
using System.Collections.Generic;

namespace Game.Server.Rooms
{
    /// <summary>
    /// Odanın oyun başlatma isteğini işler.
    /// Oda tipine göre PVP veya PVE oyunu başlatır;
    /// başarısızlık durumunda odadaki tüm oyunculara hata bildirimi gönderir.
    /// </summary>
    public class StartGameAction : IAction
    {
        // -----------------------------------------------------------------------
        // ALANLAR
        // -----------------------------------------------------------------------

        private readonly BaseRoom m_room;

        // -----------------------------------------------------------------------
        // YAPICI
        // -----------------------------------------------------------------------

        public StartGameAction(BaseRoom room)
        {
            m_room = room;
        }

        // -----------------------------------------------------------------------
        // ÇALIŞMA MANTIĞI
        // -----------------------------------------------------------------------

        public void Execute()
        {
            // Oda başlatma koşullarını karşılamıyorsa iptal et
            if (!m_room.CanStart()) return;

            // Her oyun başlangıcında NPC alanlarını sıfırla
            m_room.StartWithNpc = false;
            m_room.PickUpNpcId = -1;

            List<GamePlayer> players = m_room.GetPlayers();

            if (m_room.RoomType == eRoomType.Freedom)
            {
                StartFreedomGame(players);
            }
            else if (IsPVE(m_room.RoomType))
            {
                StartPveGame(players);
            }
            else if (IsPVP(m_room.RoomType))
            {
                StartPvpGame();
            }

            // Bekleme odası görünümünü güncelle
            RoomMgr.WaitingRoom.SendUpdateCurrentRoom(m_room);
        }

        // -----------------------------------------------------------------------
        // OYUN BAŞLATMA METODları
        // -----------------------------------------------------------------------

        /// <summary>
        /// Freedom (serbest PvP) oyununu başlatır.
        /// Oyuncuları takım 1 ve takım 2 olarak ayırır.
        /// </summary>
        private void StartFreedomGame(List<GamePlayer> players)
        {
            var team1 = new List<IGamePlayer>();
            var team2 = new List<IGamePlayer>();

            foreach (GamePlayer p in players)
            {
                if (p == null) continue;
                if (p.CurrentRoomTeam == 1) team1.Add(p);
                else team2.Add(p);
            }

            BaseGame game = GameMgr.StartPVPGame(
                m_room.RoomId, team1, team2,
                m_room.MapId, m_room.RoomType, m_room.GameType, m_room.TimeMode);

            HandleGameStart(game);
        }

        /// <summary>
        /// PVE oyununu başlatır.
        /// Sunucu yeniden başlatma senkronizasyonu bekleniyorsa oyuna izin vermez.
        /// </summary>
        private void StartPveGame(List<GamePlayer> players)
        {
            // Sunucu PVE yeniden başlatması bekliyorsa oyunu engelle
            if (GameMgr.SynDate < 0)
            {
                foreach (GamePlayer p in players)
                    p?.Out.SendMessage(eMessageType.ChatERROR,
                        LanguageMgr.GetTranslation("StartGameAction.WaitingPveRestart"));
                return;
            }

            var gamePlayers = new List<IGamePlayer>();
            foreach (GamePlayer p in players)
            {
                if (p != null) gamePlayers.Add(p);
            }

            // Zorluk seviyesine göre zaman modunu güncelle
            UpdatePveRoomTimeMode();

            BaseGame game = GameMgr.StartPVEGame(
                m_room.RoomId, gamePlayers,
                m_room.MapId, m_room.RoomType, m_room.GameType,
                m_room.TimeMode, m_room.HardLevel,
                m_room.LevelLimits, m_room.currentFloor);

            HandleGameStart(game);
        }

        /// <summary>
        /// Standart PVP (Match) oyununu başlatır.
        /// Savaş sunucusuna oda ekler; başarısızlık durumunda hata yayınlar.
        /// </summary>
        private void StartPvpGame()
        {
            m_room.UpdateAvgLevel();

            // Bölge içi Match odası için ring sunucusundan NPC ID al
            if (!m_room.isCrosszone &&
                m_room.RoomType == eRoomType.Match &&
                m_room.Host != null)
            {
                m_room.PickUpNpcId = RingStationConfiguration.NextRoomId();
            }

            BattleServer battleServer = BattleMgr.AddRoom(m_room);

            if (battleServer != null)
            {
                m_room.BattleServer = battleServer;
                m_room.IsPlaying = true;
                m_room.SendStartPickUp();
            }
            else
            {
                // Savaş sunucusu bulunamadı — hata bildir ve iptal et
                BroadcastError(LanguageMgr.GetTranslation("StartGameAction.noBattleServe"));
                m_room.SendCancelPickUp();
            }
        }

        // -----------------------------------------------------------------------
        // YARDIMCI METODlar
        // -----------------------------------------------------------------------

        /// <summary>
        /// Oyun nesnesi başarıyla oluşturulduysa odada başlatır;
        /// aksi halde durumu sıfırlar ve oyunculara hata mesajı gönderir.
        /// </summary>
        private void HandleGameStart(BaseGame game)
        {
            if (game != null)
            {
                m_room.IsPlaying = true;
                m_room.StartGame(game);
                return;
            }

            // Oyun başlatılamadı — durumu sıfırla
            m_room.IsPlaying = false;
            m_room.SendPlayerState();

            // Oda tipine özel hata mesajı
            string errorKey = (m_room.RoomType == eRoomType.Freedom)
                ? "StartGameAction.noBattleServe"
                : "StartGameAction.RoomError";

            BroadcastError(LanguageMgr.GetTranslation(errorKey));
            m_room.SendCancelPickUp();
        }

        /// <summary>
        /// Host dahil tüm odaya hata mesajı yayınlar.
        /// Host null ise işlem yapılmaz.
        /// </summary>
        private void BroadcastError(string message)
        {
            if (m_room.Host == null) return;
            GSPacketIn pkg = m_room.Host.Out.SendMessage(eMessageType.ChatERROR, message);
            m_room.SendToAll(pkg);
        }

        /// <summary>
        /// Oda tipinin PVP (Match) olup olmadığını döndürür.
        /// </summary>
        private static bool IsPVP(eRoomType roomType)
        {
            return roomType == eRoomType.Match;
        }

        /// <summary>
        /// Oda tipinin PVE kategorisine girip girmediğini döndürür.
        /// Freedom ve Match dışındaki tüm oynanabilir oda tipleri PVE kabul edilir.
        /// Academy ve Dungeon_Night orijinal kodda ayrı kontrol ediliyordu;
        /// IsPVE içine alınarak tutarlılık sağlandı.
        /// </summary>
        private static bool IsPVE(eRoomType roomType)
        {
            switch (roomType)
            {
                case eRoomType.Dungeon:
                case eRoomType.Dungeon_Night:
                case eRoomType.Freshman:
                case eRoomType.Labyrinth:
                case eRoomType.ConsortiaBoss:
                case eRoomType.Academy:
                case eRoomType.AcademyDungeon:
                case eRoomType.FightLab:
                case eRoomType.WordBossFight:
                case eRoomType.Christmas:
                case eRoomType.ActivityDungeon:
                case eRoomType.SpecialActivityDungeon:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// PVE odasının zorluk seviyesine göre zaman modunu ayarlar.
        /// Simple → 3 (en uzun), Normal → 2, Hard/Terror → 1 (en kısa), Epic → değişmez.
        /// </summary>
        private void UpdatePveRoomTimeMode()
        {
            switch (m_room.HardLevel)
            {
                case eHardLevel.Simple:
                    m_room.TimeMode = 3;
                    break;
                case eHardLevel.Normal:
                    m_room.TimeMode = 2;
                    break;
                case eHardLevel.Hard:
                case eHardLevel.Terror:
                    m_room.TimeMode = 1;
                    break;
                case eHardLevel.Epic:
                    // Epic seviyede zaman modu değiştirilmez
                    break;
            }
        }
    }
}