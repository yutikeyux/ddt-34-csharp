using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using SqlDataProvider.Data;
using System.Collections.Generic;

namespace Game.Server.Packets.Client
{
    [PacketHandler(182, "改变物品颜色")]
    public class UserChangeItemColorHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            eMessageType type = eMessageType.GM_NOTICE;
            string translateId = "UserChangeItemColorHandler.Success";
            _ = packet.ReadInt();
            int slot = packet.ReadInt();
            _ = packet.ReadInt();
            int slot2 = packet.ReadInt();
            string text = packet.ReadString();
            string text2 = packet.ReadString();
            int num = packet.ReadInt();
            ItemInfo itemAt = client.Player.EquipBag.GetItemAt(slot2);
            ItemInfo itemAt2 = client.Player.PropBag.GetItemAt(slot);
            if (itemAt != null)
            {
                client.Player.BeginChanges();
                try
                {
                    bool flag = false;
                    if (itemAt2?.IsValidItem() ?? false)
                    {
                        _ = client.Player.PropBag.RemoveCountFromStack(itemAt2, 1);
                        flag = true;
                    }
                    else
                    {
                        _ = ItemMgr.FindItemTemplate(num);
                        List<ShopItemInfo> list = ShopMgr.FindShopbyTemplatID(num);
                        int needMoney = 0;
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (list[i].APrice1 == -1 && list[i].AValue1 != 0)
                            {
                                needMoney = list[i].AValue1;
                            }
                        }
                        if (needMoney <= client.Player.PlayerCharacter.Money || needMoney <= client.Player.PlayerCharacter.MoneyLock)
                        {
                            _ = client.Player.MoneyDirect(needMoney, true, false, true);
                            flag = true;
                        }
                    }
                    if (flag)
                    {
                        itemAt.Color = text ?? "";
                        itemAt.Skin = text2 ?? "";
                        client.Player.EquipBag.UpdateItem(itemAt);
                    }
                }
                finally
                {
                    client.Player.CommitChanges();
                }
            }
            _ = client.Out.SendMessage(type, LanguageMgr.GetTranslation(translateId));
            return 0;
        }
    }
}
