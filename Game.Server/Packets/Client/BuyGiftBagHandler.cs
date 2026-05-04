using Bussiness.Managers;
using Game.Base.Packets;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
    [PacketHandler(46, "物品强化")]
    public class BuyGiftBagHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int value = 4599;
            if (client.Player.PlayerCharacter.Money < value)
            {
                client.Player.SendMessage($"Kupon yetersiz!");
                return 0;
            }
            if (client.Player.RemoveMoney(value) > 0)
            {
                client.Player.ClearStoreBagWithOutPlace(5);
                ItemInfo itemInfo = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(11023), 1, 101);
                itemInfo.Count = 1;
                itemInfo.ValidDate = 0;
                itemInfo.IsBinds = true;
                _ = client.Player.StoreBag.AddItemTo(itemInfo, 0);
                itemInfo = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(11023), 1, 101);
                itemInfo.Count = 1;
                itemInfo.ValidDate = 0;
                itemInfo.IsBinds = true;
                _ = client.Player.StoreBag.AddItemTo(itemInfo, 1);
                itemInfo = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(11023), 1, 101);
                itemInfo.Count = 1;
                itemInfo.ValidDate = 0;
                itemInfo.IsBinds = true;
                _ = client.Player.StoreBag.AddItemTo(itemInfo, 2);
                itemInfo = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(11020), 1, 101);
                itemInfo.Count = 1;
                itemInfo.ValidDate = 0;
                itemInfo.IsBinds = true;
                _ = client.Player.StoreBag.AddItemTo(itemInfo, 3);
                itemInfo = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(11018), 1, 101);
                itemInfo.Count = 1;
                itemInfo.ValidDate = 0;
                itemInfo.IsBinds = true;
                _ = client.Player.StoreBag.AddItemTo(itemInfo, 4);
                client.Player.SendMessage($"İndirimli Hediye Paketi satın alma başarılı!");
            }
            return 0;
        }
    }
}
