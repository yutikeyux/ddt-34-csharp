using Bussiness;
using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
    [PacketHandler(246, "请求结婚状态")]
    internal class MarryStatusHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int num = packet.ReadInt();
            GamePlayer playerById = WorldMgr.GetPlayerById(num);
            if (playerById != null)
            {
                _ = client.Player.Out.SendPlayerMarryStatus(client.Player, playerById.PlayerCharacter.ID, playerById.PlayerCharacter.IsMarried);
            }
            else
            {
                using PlayerBussiness playerBussiness = new();
                PlayerInfo userSingleByUserID = playerBussiness.GetUserSingleByUserID(num);
                _ = client.Player.Out.SendPlayerMarryStatus(client.Player, userSingleByUserID.ID, userSingleByUserID.IsMarried);
            }
            return 0;
        }
    }
}
