using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;

namespace Game.Server.Packets.Client
{
    [PacketHandler((int)ePackageType.FIRSTRECHARGE, "FIRSTRECHARGE")]
	public class FirstRechargeGetAwardHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
			packet.ReadInt();
			if (DateTime.Compare(client.Player.LastOpenCard.AddSeconds(0.5), DateTime.Now) > 0)
			{
				return 0;
			}
			string translateId = "FirstRechargeGetAward.Successfull";
			ProduceBussiness pb = new ProduceBussiness();
			//EventRewardInfo[] infos = pb.GetEventRewardInfoByType((int)NoviceActiveType.FIRST_RECHARGE, 1);
			////EventRewardGoodsInfo[] goods = pb.GetEventRewardGoodsByType((int)NoviceActiveType.FIRST_RECHARGE, 1);
			List<ItemInfo> items = new List<ItemInfo>();
			//foreach (EventRewardGoodsInfo item in goods)
			//{
			//	ItemInfo info = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(item.TemplateId), 1, 104);
			//	info.StrengthenLevel = item.StrengthLevel;
		//	info.AttackCompose = item.AttackCompose;
			//	info.DefendCompose = item.DefendCompose;
			//	info.AgilityCompose = item.AgilityCompose;
			//	info.LuckCompose = item.LuckCompose;
			//	info.IsBinds = item.IsBind;
			//	info.Count = item.Count;
			//	info.ValidDate = item.ValidDate;
			//	items.Add(info);
			//}
			if (!client.Player.PlayerCharacter.IsRecharged)
			{
				translateId = "FirstRechargeGetAward.NotCharge";
				return 0;
			}
			if (client.Player.PlayerCharacter.IsGetAward)
			{
				translateId = "FirstRechargeGetAward.AlreadyGetAward";
				return 0;
			}
			//EventRewardInfo[] list = infos;
			//for (int i = 0; i < list.Length; i++)
			//{
			//	_ = list[i];
			//	if (client.Player.PlayerCharacter.IsRecharged && !client.Player.PlayerCharacter.IsGetAward)
			//	{
			//		if (!client.Player.SendItemsToMail(items, LanguageMgr.GetTranslation("FirstRechargeGetAward.Content"), LanguageMgr.GetTranslation("FirstRechargeGetAward.Title"), eMailType.Manage))
			//		{
			//			translateId = "FirstRechargeGetAward.Error";
			//			return 0;
			//		}
			//		client.Player.PlayerCharacter.IsGetAward = true;
			//	}
			//}
			client.Player.Out.SendUpdateFirstRecharge(client.Player.PlayerCharacter.IsRecharged, client.Player.PlayerCharacter.IsGetAward);
			client.Player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation(translateId));
			client.Player.LastOpenCard = DateTime.Now;
			return 1;
        }
    }
}
