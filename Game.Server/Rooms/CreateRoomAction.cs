using Bussiness;
using Game.Base.Packets;
using Game.Logic;
using System;

namespace Game.Server.Rooms
{
    /// <summary>
    /// Oyuncunun yeni bir oda oluşturma isteğini işler.
    /// Boş bir slot arar, odayı yapılandırır ve oyuncuyu içine ekler.
    /// </summary>
    public class CreateRoomAction : IAction
    {
        // -----------------------------------------------------------------------
        // ALANLAR
        // -----------------------------------------------------------------------

        private readonly GamePlayer m_player;
        private readonly string m_name;
        private readonly string m_password;
        private readonly eRoomType m_roomType;
        private readonly byte m_timeType;

        /// <summary>
        /// Statik Random: her action için yeni nesne oluşturmanın
        /// neden olduğu seed tekrarı ve gereksiz bellek kullanımını önler.
        /// </summary>
        private static readonly Random s_rand = new Random();

        // -----------------------------------------------------------------------
        // YAPICI
        // -----------------------------------------------------------------------

        public CreateRoomAction(
            GamePlayer player,
            string name,
            string password,
            eRoomType roomType,
            byte timeType)
        {
            m_player = player;
            m_name = name;
            m_password = password;
            m_roomType = roomType;
            m_timeType = timeType;
        }

        // -----------------------------------------------------------------------
        // ÇALIŞMA MANTIĞI
        // -----------------------------------------------------------------------

        public void Execute()
        {
            // Oyuncu zaten bir odadaysa önce çıkar
            m_player.CurrentRoom?.RemovePlayerUnsafe(m_player);

            // Oturum aktif değilse işlem yapma
            if (!m_player.IsActive) return;

            // Boş oda slotu bul
            BaseRoom targetRoom = FindEmptyRoom();

            if (targetRoom == null)
            {
                // Sunucuda boş slot kalmadı; oyuncuya bildirim gönder
                m_player.Out.SendMessage(
                    eMessageType.BIGBUGLE_NOTICE,
                    LanguageMgr.GetTranslation("CreateRoomAction.NoRoomSlot"));
                m_player.Out.SendRoomLoginResult(false);
                return;
            }

            // Bekleme odasından çıkar ve odayı başlat
            RoomMgr.WaitingRoom.RemovePlayer(m_player);
            targetRoom.Start();

            // Oda tipine özel başlangıç ayarları
            ConfigureRoom(targetRoom);

            // Oda bilgilerini güncelle (isim, şifre, tip, harita)
            targetRoom.UpdateRoom(m_name, m_password, m_roomType, m_timeType, mapId: 0);

            // Oyuncuya oda oluşturma paketini gönder, ardından odaya ekle
            m_player.Out.SendRoomCreate(targetRoom);
            targetRoom.AddPlayerUnsafe(m_player);

            // Bekleme odası listelerini güncelle
            RoomMgr.WaitingRoom.SendUpdateCurrentRoom(targetRoom);
            RoomMgr.WaitingRoom.SendUpdateRoom(targetRoom);
        }

        // -----------------------------------------------------------------------
        // YARDIMCI METODlar
        // -----------------------------------------------------------------------

        /// <summary>
        /// Oda dizisinde sıralı arama yaparak ilk boş (IsUsing == false) slotu döndürür.
        /// Orijinal kodda her iterasyonda tekrar rand.Next() çekiliyordu; bu sonsuz döngüye
        /// yol açabilirdi. Güvenli sıralı tarama kullanılıyor; başlangıç noktası rastgele
        /// seçilerek dağılım korunuyor.
        /// </summary>
        private static BaseRoom FindEmptyRoom()
        {
            BaseRoom[] rooms = RoomMgr.Rooms;
            int length = rooms.Length;

            // Rastgele bir başlangıç noktasından dairesel tarama
            int start = s_rand.Next(length);
            for (int i = 0; i < length; i++)
            {
                int index = (start + i) % length;
                if (!rooms[index].IsUsing)
                    return rooms[index];
            }

            return null;
        }

        /// <summary>
        /// Oda tipine ve oyuncu/sunucu durumuna göre oda ayarlarını yapar.
        /// </summary>
        private void ConfigureRoom(BaseRoom room)
        {
            if (m_roomType == eRoomType.Dungeon)
            {
                room.HardLevel = eHardLevel.Normal;
                room.LevelLimits = (int)room.GetLevelLimit(m_player);
                room.isOpenBoss = false;
                room.currentFloor = 0;
                room.maxViewerCnt = 2;
            }

            // Lig zamanıysa bölgeler arası eşleştirme aktif et
            if (room.isWithinLeageTime)
                room.isCrosszone = true;

            room.AreaID = m_player.ZoneId;
        }
    }
}