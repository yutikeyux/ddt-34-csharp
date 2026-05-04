using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
    [PacketHandler((byte)ePackageType.ITEM_STORE, "储存物品")]
    public class StoreItemHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            if (client.Player.PlayerCharacter.ConsortiaID == 0)
            {
                return 1;
            }

            int bagType = packet.ReadByte();
            int bagPlace = packet.ReadInt();

            if (bagType == 0 && bagPlace < 31)
            {
                return 1;
            }

            ConsortiaInfo info = ConsortiaMgr.FindConsortiaInfo(client.Player.PlayerCharacter.ConsortiaID);
            if (info != null)
            {
                _ = client.Player.ConsortiaBag;
                _ = client.Player.GetInventory((eBageType)bagType);
            }

            return 0;
        }
    }
}
