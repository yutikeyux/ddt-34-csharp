using System.Collections.Generic;

namespace Game.Server.Rooms
{
    /// <summary>
    /// Oyuncuyu bekleme odasına (lobi) ekler.
    /// Önce mevcut odadan çıkarır, ardından bekleme odasına kaydeder ve
    /// oda listesi ile sahne güncellemelerini gönderir.
    /// </summary>
    public class EnterWaitingRoomAction : IAction
    {
        // -----------------------------------------------------------------------
        // ALANLAR
        // -----------------------------------------------------------------------

        private readonly GamePlayer m_player;

        // -----------------------------------------------------------------------
        // YAPICI
        // -----------------------------------------------------------------------

        public EnterWaitingRoomAction(GamePlayer player)
        {
            m_player = player;
        }

        // -----------------------------------------------------------------------
        // ÇALIŞMA MANTIĞI
        // -----------------------------------------------------------------------

        public void Execute()
        {
            // Geçersiz oyuncu referansı — işlem yapma
            if (m_player == null) return;

            // Oyuncu hâlâ bir odadaysa güvenli şekilde çıkar
            m_player.CurrentRoom?.RemovePlayerUnsafe(m_player);

            BaseWaitingRoom waitingRoom = RoomMgr.WaitingRoom;

            if (waitingRoom.AddPlayer(m_player))
            {
                // Oyuncu yeni eklendi:
                // 1. Güncel oda listesini oyuncuya gönder
                List<BaseRoom> allRooms = RoomMgr.GetAllRooms();
                m_player.Out.SendUpdateRoomList(allRooms);

                // 2. Lobideki mevcut oyuncuları yeni oyuncuya tanıt
                GamePlayer[] current = waitingRoom.GetPlayersSafe();
                foreach (GamePlayer other in current)
                {
                    if (other != m_player)
                        m_player.Out.SendSceneAddPlayer(other);
                }
            }
            else
            {
                // Oyuncu zaten lobideydi:
                // Oda listesini ve sahne durumunu yenile
                waitingRoom.SendUpdateRoom(m_player);
                waitingRoom.SendSceneUpdate(m_player);
            }
        }
    }
}