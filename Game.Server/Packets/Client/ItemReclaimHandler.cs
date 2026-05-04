using Bussiness;
using Game.Base.Packets;
using Game.Server.GameUtils;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
    [PacketHandler((int)ePackageType.REClAIM_GOODS, "物品比较")]
    public class ItemReclaimHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            eBageType bagType = (eBageType)packet.ReadByte();
            int place = packet.ReadInt();
            int count = packet.ReadInt();
            PlayerInventory bag = client.Player.GetInventory(bagType);
            if (bag != null && bag.GetItemAt(place) != null)
            {
                if (bag.GetItemAt(place).Count <= count)
                {
                    count = bag.GetItemAt(place).Count;
                }
                ItemTemplateInfo item = bag.GetItemAt(place).Template;
                int price = count * item.ReclaimValue;
                if (item.ReclaimType == 3)
                {
                    _ = client.Out.SendMessage(eMessageType.GM_NOTICE, $"Bu ürün satılamaz.");
                    return 0;
                    //client.Player.AddMoney(num3);
                    //client.Out.SendMessage(eMessageType.GM_NOTICE, $"Bạn nhận được {num3} xu.");
                }
                else if (item.ReclaimType == 2)
                {
                    _ = client.Player.AddGiftToken(price);
                    //client.Out.SendMessage(eMessageType.GM_NOTICE, $"Bạn nhận được {num3} lễ kim.");
                    _ = client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("ItemReclaimHandler.Success1", price));
                }
                else if (item.ReclaimType == 1)
                {
                    _ = client.Player.AddGold(price);
                    //client.Out.SendMessage(eMessageType.GM_NOTICE, $"Bạn nhận được {price} vàng.");
                    _ = client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("ItemReclaimHandler.Success2", price));
                }
                if (item.TemplateID == 11408)
                {
                    _ = client.Player.RemoveMedal(count);
                }
                _ = bag.RemoveItemAt(place);
                return 0;
            }
            //client.Out.SendMessage(eMessageType.GM_NOTICE, $"Bán vật phẩm không thành công.");
            _ = client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("ItemReclaimHandler.NoSuccess"));
            return 1;
        }
    }
}
