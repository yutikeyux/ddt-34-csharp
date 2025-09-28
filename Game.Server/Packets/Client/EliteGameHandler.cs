using Game.Base.Packets;

namespace Game.Server.Packets.Client
{
    [PacketHandler((byte)ePackageType.ELITEGAME, "添加拍卖")]
    public class EliteGameHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            if (client.Player.EliteGameHandler != null)
            {
                client.Player.EliteGameHandler.ProcessData(client.Player, packet);
                return 0;
            }
            return 1;
        }
    }
}
