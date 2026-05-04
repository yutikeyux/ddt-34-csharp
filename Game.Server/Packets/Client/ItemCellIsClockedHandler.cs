using Game.Base.Packets;
using Game.Server.GameUtils;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
    [PacketHandler((int)ePackageType.ITEM_CELL_IS_LOCKED, "客户端日记")]
    public class ItemCellIsClockedHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            eBageType bagType = (eBageType)packet.ReadInt();
            int place = packet.ReadInt();
            bool isLocked = packet.ReadBoolean();
            //Console.WriteLine("bagType {0}, place {1}, isLocked {2}", bagType, place, isLocked);
            PlayerInventory bag = client.Player.GetInventory(bagType);
            ItemInfo item = bag.GetItemAt(place);
            if (item != null)
            {
                item.cellLocked = isLocked;
                bag.UpdateItem(item);
            }
            return 0;
        }
    }
}
