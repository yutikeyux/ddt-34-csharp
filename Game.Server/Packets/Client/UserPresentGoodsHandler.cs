using Game.Base.Packets;
using System.Text;

namespace Game.Server.Packets.Client
{
    [PacketHandler(57, "购买物品")]
    public class UserPresentGoodsHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            _ = new StringBuilder();
            _ = packet.ReadString();
            _ = packet.ReadString();
            _ = packet.ReadInt();
            client.Player.SendMessage("Bu özellik geçici olarak kilitlenmiştir, lütfen yuti ile iletişime geçin.");
            return 0;
        }
    }
}
