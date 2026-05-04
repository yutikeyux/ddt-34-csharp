using Game.Base.Packets;
using System;

namespace Game.Server.Packets.Client
{
    [Obsolete("已经不用")]
    [PacketHandler((int)ePackageType.AC_ACTION, "user ac action")] //35
    public class ACActionHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            return 1;
        }
    }
}
