using Game.Base.Packets;

namespace Game.Server.Packets.Client
{
    // Token: 0x02000718 RID: 1816
    [PacketHandler(159, "场景用户离开")]
    public class WonderfulActivityHandler : IPacketHandler
    {
        // Token: 0x0600405F RID: 16479 RVA: 0x001C9E30 File Offset: 0x001C8030
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            _ = (int)packet.ReadByte();
            GSPacketIn gspacketIn = new(238, client.Player.PlayerCharacter.ID);
            client.SendTCP(gspacketIn);
            return 0;
        }
    }
}
