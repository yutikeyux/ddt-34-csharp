using Game.Base; // IAction arayüzü için
using Game.Base.Packets; // GSPacketIn için
using Game.Server.GameObjects; // GamePlayer için
using Game.Server.Packets; // ePackageType için
using Game.Server.WorldBoss; // WorldBossGamePackageType enum'u için
using log4net;
using System.Reflection;

namespace Game.Server.Rooms
{
    public class ApiTeleportAction : IAction
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private GamePlayer m_player;

        public ApiTeleportAction(GamePlayer player)
        {
            m_player = player;
        }

        public void Execute()
        {
            // --- Bu kod RoomMgr thread'inde güvenle çalışıyor ---

            // 1. ADIM: Oyuncunun "LOBİDE" olmasını garantile
            if (m_player.CurrentRoom != null)
            {
                // Oyuncu bir odada (room lobby).
                // Önce odadan çıkar
                m_player.CurrentRoom.RemovePlayerUnsafe(m_player);
                m_player.Out.SendSceneRemovePlayer(m_player);

                // SONRA Lobiye (WaitingRoom) ekle.
                // Bu, oyuncunun durumunu (state) temizler ve onu bilinen bir konuma alır.
                RoomMgr.WaitingRoom.AddPlayer(m_player);
            }

            // --- Durum: Oyuncu %100 Lobide (WaitingRoom) ---
            // 'm_player.WorldBoss' işlemcisi artık %100 aktif olmalı.

            // 2. ADIM: Oyuncunun World Boss'a girmek istediğini SİMÜLE ET.
            // BU İŞLEMİ, OYUNCUYU LOBİDEN ÇIKARMADAN YAPIYORUZ.

            GSPacketIn enterPacket = new GSPacketIn((byte)ePackageType.WORLDBOSS_CMD, m_player.PlayerCharacter.ID);

            // Alt komut (ENTER_WORLDBOSSROOM = 32)
            // Bu, EnterRoom.cs handler'ını tetikleyecek.
            enterPacket.WriteByte((byte)WorldBossGamePackageType.ENTER_WORLDBOSSROOM);

            if (m_player.WorldBoss != null)
            {
                // Bu, 'EnterRoom.cs' handler'ını tetikler.
                m_player.WorldBoss.ProcessData(m_player, enterPacket);

                // 'EnterRoom.cs' istemciye 'CANENTER (2)' paketini gönderir.
                // İstemci (Flash) 'CANENTER' alır almaz 'sendGameStart()' paketini sunucuya gönderir.
                // 'sendGameStart()' paketi, 'RoomMgr' eylem sırasına BİR SONRAKİ EYLEM olarak girer.
                // Bu eylem, oyuncuyu lobiden çıkarıp PVEGame'e sokar.
            }
            else
            {
                log.Error($"ApiTeleportAction: Player '{m_player.PlayerCharacter.NickName}' WorldBoss processor is null (while in lobby).");
            }

            m_player.SendMessage("Yönetici tarafından World Boss haritasına ışınlanma işlemi başlatıldı...");
        }
    }
}