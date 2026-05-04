using Game.Base.Packets;

namespace Game.Server.Packets.Client
{
    [PacketHandler(191, "礼堂数据")]
    public class HotSpringCmdDataHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            client.Player.CurrentHotSpringRoom?.ProcessData(client.Player, packet);
            return 0;
        }
    }
}
