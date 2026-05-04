using Game.Base.Packets;

namespace Game.Server.Packets.Client
{
    [PacketHandler(249, "礼堂数据")]
    public class MarryDataHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            client.Player.CurrentMarryRoom?.ProcessData(client.Player, packet);
            return 0;
        }
    }
}
