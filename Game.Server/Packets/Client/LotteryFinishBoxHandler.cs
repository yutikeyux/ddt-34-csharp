using Game.Base.Packets;
using Game.Server.GameUtils;
using SqlDataProvider.Data;
using System.Collections.Generic;

namespace Game.Server.Packets.Client
{
    [PacketHandler(28, "打开物品")]
	public class LotteryFinishBoxHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
			PlayerInventory caddyBag = client.Player.CaddyBag;
			List<ItemInfo> list = new List<ItemInfo>();
			for (int i = 0; i < caddyBag.Capalility; i++)
			{
				ItemInfo itemAt = caddyBag.GetItemAt(i);
				if (itemAt != null)
				{
					if (!client.Player.AddItem(itemAt))
					{
						list.Add(itemAt);
					}
					caddyBag.TakeOutItem(itemAt);
				}
			}
			if (list.Count > 0)
			{
				client.Player.SendItemsToMail(list, "İade edilen öğeler", "Malezya Ruleti", eMailType.BuyItem);
			}
			if (client.Player.Lottery != -1 && client.Player.LotteryAwardList.Count > 0)
			{
				List<ItemInfo> list2 = new List<ItemInfo>();
				foreach (ItemInfo lotteryAward in client.Player.LotteryAwardList)
				{
					if (!client.Player.AddItem(lotteryAward))
					{
						list2.Add(lotteryAward);
					}
				}
				if (list2.Count > 0)
				{
					client.Player.SendItemsToMail(list2, "Çantanızın dolu olması nedeniyle öğeler iade edildi.", "Malezya Rulet Ödülü", eMailType.BuyItem);
					client.Player.SendMessage("Çanta dolusu eşyayı geri postayla gönder.");
				}
				client.Player.ResetLottery();
			}
			else
			{
				client.Player.ResetLottery();
			}
			return 1;
        }
    }
}
