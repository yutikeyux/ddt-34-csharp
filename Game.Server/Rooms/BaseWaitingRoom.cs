using Game.Base.Packets;
using Game.Logic;
using Game.Server.Managers;
using Game.Server.Packets;
using System.Collections.Generic;

namespace Game.Server.Rooms
{
    /// <summary>
    /// Oyuncuların oda seçerken beklediği lobi alanını yönetir.
    /// Oyuncuları takip eder ve oda listesi güncellemelerini dağıtır.
    /// </summary>
    public class BaseWaitingRoom
    {
        // -----------------------------------------------------------------------
        // ALANLAR
        // -----------------------------------------------------------------------

        /// <summary>Bekleme odasındaki oyuncular: PlayerId → GamePlayer</summary>
        private readonly Dictionary<int, GamePlayer> m_list;

        // -----------------------------------------------------------------------
        // YAPICI
        // -----------------------------------------------------------------------

        public BaseWaitingRoom()
        {
            m_list = new Dictionary<int, GamePlayer>();
        }

        // -----------------------------------------------------------------------
        // OYUNCU YÖNETİMİ
        // -----------------------------------------------------------------------

        /// <summary>
        /// Oyuncuyu bekleme odasına ekler.
        /// Zaten kayıtlıysa tekrar eklenmez.
        /// Başarılıysa diğer oyunculara bildirim paketi yayınlanır.
        /// </summary>
        public bool AddPlayer(GamePlayer player)
        {
            bool added = false;
            lock (m_list)
            {
                if (!m_list.ContainsKey(player.PlayerId))
                {
                    m_list.Add(player.PlayerId, player);
                    added = true;
                }
            }

            if (added)
            {
                GSPacketIn packet = player.Out.SendSceneAddPlayer(player);
                SendToAll(packet, player);
            }

            return added;
        }

        /// <summary>
        /// Oyuncuyu bekleme odasından çıkarır.
        /// Başarılıysa diğer oyunculara bildirim paketi yayınlanır.
        /// </summary>
        public bool RemovePlayer(GamePlayer player)
        {
            bool removed;
            lock (m_list)
            {
                removed = m_list.Remove(player.PlayerId);
            }

            if (removed)
            {
                GSPacketIn packet = player.Out.SendSceneRemovePlayer(player);
                SendToAll(packet, player);
            }

            // Not: Orijinal kod her zaman true döndürüyordu; gerçek sonucu döndürüyoruz.
            return removed;
        }

        // -----------------------------------------------------------------------
        // SAHNE GÜNCELLEMELERİ
        // -----------------------------------------------------------------------

        /// <summary>
        /// Yeni giren oyuncuyu diğerlerine, diğerlerini yeni oyuncuya tanıtır.
        /// </summary>
        public void SendSceneUpdate(GamePlayer player)
        {
            // Yeni oyuncuyu lobideki herkese duyur
            GSPacketIn packet = player.Out.SendSceneAddPlayer(player);
            SendToAll(packet, player);

            // Lobideki mevcut oyuncuları yeni oyuncuya gönder
            GamePlayer[] current = GetPlayersSafe();
            foreach (GamePlayer other in current)
            {
                if (other != player)
                    player.Out.SendSceneAddPlayer(other);
            }
        }

        // -----------------------------------------------------------------------
        // ODA LİSTESİ GÜNCELLEMELERİ
        // -----------------------------------------------------------------------

        /// <summary>
        /// Tek bir oyuncuya, durumuna göre (Online → PvP, Away → PvE) oda listesini gönderir.
        /// </summary>
        public void SendUpdateRoom(GamePlayer player)
        {
            List<BaseRoom> rooms = (player.PlayerState == ePlayerState.Away)
                ? RoomMgr.GetAllPveRooms()
                : RoomMgr.GetAllMatchRooms();

            player.Out.SendUpdateRoomList(rooms);
        }

        /// <summary>
        /// Belirtilen oda değiştiğinde bekleme odasındaki tüm oyuncuların listesini günceller.
        /// Online oyuncular PvP odalarını, Away oyuncular PvE odalarını görür.
        /// </summary>
        public void SendUpdateRoom(BaseRoom room)
        {
            GamePlayer[] snapshot = GetPlayersSafe();
            List<BaseRoom> allRooms = RoomMgr.GetAllRooms();

            // Oyuncuları duruma göre ayır
            var onlinePlayers = new List<GamePlayer>();
            var awayPlayers = new List<GamePlayer>();

            foreach (GamePlayer p in snapshot)
            {
                if (p.PlayerState == ePlayerState.Online)
                    onlinePlayers.Add(p);
                else if (p.PlayerState == ePlayerState.Away)
                    awayPlayers.Add(p);
            }

            // Odaları tipine göre ayır
            var pvpRooms = new List<BaseRoom>();
            var pveRooms = new List<BaseRoom>();

            foreach (BaseRoom r in allRooms)
            {
                if (r.RoomType == eRoomType.Freedom || r.RoomType == eRoomType.Match)
                {
                    pvpRooms.Add(r);
                }
                else if (r.RoomType == eRoomType.Dungeon ||
                         r.RoomType == eRoomType.AcademyDungeon ||
                         r.RoomType == eRoomType.ActivityDungeon ||
                         r.RoomType == eRoomType.SpecialActivityDungeon)
                {
                    pveRooms.Add(r);
                }
            }

            SendUpdateRoom(onlinePlayers, pvpRooms);
            SendUpdateRoom(awayPlayers, pveRooms);
        }

        /// <summary>
        /// Verilen oyuncu listesine aynı oda listesini verimli şekilde gönderir.
        /// İlk oyuncu için paket oluşturulur; diğerleri aynı paketi alır (tekrar serileştirmeden).
        /// </summary>
        public void SendUpdateRoom(List<GamePlayer> players, List<BaseRoom> rooms)
        {
            GSPacketIn sharedPacket = null;

            foreach (GamePlayer player in players)
            {
                if (sharedPacket == null)
                    sharedPacket = player.Out.SendUpdateRoomList(rooms);
                else
                    player.Out.SendTCP(sharedPacket);
            }
        }

        /// <summary>
        /// Oyun dışındaki tüm oyunculara güncel oda listesini gönderir.
        /// </summary>
        public void SendUpdateWaitingRoom(BaseRoom room)
        {
            List<BaseRoom> allRooms = RoomMgr.GetAllRooms();
            foreach (GamePlayer player in WorldMgr.GetAllPlayersNoGame())
            {
                player.Out.SendUpdateRoomList(allRooms);
            }
        }

        /// <summary>
        /// Belirli bir odanın içindeki oyunculara güncel oda listesini gönderir.
        /// Paket yalnızca bir kez oluşturulur; diğer oyuncular aynı paketi alır.
        /// </summary>
        public void SendUpdateCurrentRoom(BaseRoom room)
        {
            if (room == null) return;

            List<BaseRoom> allRooms = RoomMgr.GetAllRooms(room);
            List<GamePlayer> roomPlayers = room.GetPlayers();
            GSPacketIn sharedPacket = null;

            foreach (GamePlayer player in roomPlayers)
            {
                if (sharedPacket == null)
                    sharedPacket = player.Out.SendUpdateRoomList(allRooms);
                else
                    player.Out.SendTCP(sharedPacket);
            }
        }

        // -----------------------------------------------------------------------
        // PAKET YAYINI
        // -----------------------------------------------------------------------

        /// <summary>Bekleme odasındaki tüm oyunculara paket gönderir.</summary>
        public void SendToAll(GSPacketIn packet)
        {
            SendToAll(packet, null);
        }

        /// <summary>Bekleme odasındaki tüm oyunculara paket gönderir; except oyuncusu atlanır.</summary>
        public void SendToAll(GSPacketIn packet, GamePlayer except)
        {
            GamePlayer[] snapshot = GetPlayersSafe();
            foreach (GamePlayer player in snapshot)
            {
                if (player != null && player != except)
                    player.Out.SendTCP(packet);
            }
        }

        // -----------------------------------------------------------------------
        // GÜVENLİ ANLIK GÖRÜNTÜ
        // -----------------------------------------------------------------------

        /// <summary>
        /// Oyuncu listesinin kilit altında alınmış anlık kopyasını döndürür.
        /// Döngü sırasında koleksiyonun değişmesini önler.
        /// </summary>
        public GamePlayer[] GetPlayersSafe()
        {
            lock (m_list)
            {
                var snapshot = new GamePlayer[m_list.Count];
                m_list.Values.CopyTo(snapshot, 0);
                return snapshot;
            }
        }
    }
}