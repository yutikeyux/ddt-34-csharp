using Bussiness;
using Game.Base.Packets;
using System;

namespace Game.Server.Packets.Client
{
    [PacketHandler((int)ePackageType.FIRSTRECHARGE, "FIRSTRECHARGE")]
    public class FirstRechargeGetAwardHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            _ = packet.ReadInt();
            if (DateTime.Compare(client.Player.LastOpenCard.AddSeconds(0.5), DateTime.Now) > 0)
            {
                return 0;
            }
            string translateId = "FirstRechargeGetAward.Successfull";
            _ = new ProduceBussiness();
            //EventRewardInfo[] infos = pb.GetEventRewardInfoByType((int)NoviceActiveType.FIRST_RECHARGE, 1);
            ////EventRewardGoodsInfo[] goods = pb.GetEventRewardGoodsByType((int)NoviceActiveType.FIRST_RECHARGE, 1);
            
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
                return 0;
            }
            if (client.Player.PlayerCharacter.IsGetAward)
            {
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
            _ = client.Player.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation(translateId));
            client.Player.LastOpenCard = DateTime.Now;
            return 1;
        }
    }
}
