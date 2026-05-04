using Game.Base.Packets;
using Game.Server.Managers;

namespace Game.Server.Packets.Client
{
    [PacketHandler(90, "场景用户离开")]
    public class SignAwardHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int dailyLog = packet.ReadInt();
            string message = "Ödül Alımı başarılı!";
            if (AwardMgr.AddSignAwards(client.Player, dailyLog))
            {
                _ = client.Out.SendMessage(eMessageType.GM_NOTICE, message);
            }
            return 0;
        }
    }
}
