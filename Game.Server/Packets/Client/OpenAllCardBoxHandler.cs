using Game.Base.Packets;
using Game.Server.GameUtils;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Packets.Client
{
    [PacketHandler(204, "打开物品")]
    public class OpenAllCardBoxHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            PlayerInventory caddyBag = client.Player.CaddyBag;
            CardInventory cardBag = client.Player.CardBag;
            for (int i = 0; i < caddyBag.Capalility; i++)
            {
                ItemInfo itemAt = caddyBag.GetItemAt(i);
                if (itemAt != null)
                {
                    _ = caddyBag.RemoveItem(itemAt);
                    Random random = new();
                    int property = itemAt.Template.Property5;
                    int num = random.Next(1, 3);
                    _ = cardBag.AddCard(property, num);
                    client.Player.SendMessage("Tebrikler! " + itemAt.Template.Name + " açtınız ve " + num + " kazandınız!");
                }
            }
            return 1;
        }
    }
}
