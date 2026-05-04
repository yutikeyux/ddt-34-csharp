using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;

namespace Game.Server.Packets.Client
{
    [PacketHandler((int)ePackageType.EQUIP_BRING_UP, "客户端日记")]
    public class EquipBringUpHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int count = packet.ReadInt();
            //Console.WriteLine("count {0}", count);
            ItemInfo tagItem = client.Player.StoreBag.GetItemAt(0);
            if (client.Player.PlayerCharacter.Grade < 45)
            {
                client.Player.SendMessage(LanguageMgr.GetTranslation("GameServer.LevelErrorUsing"));
                return 0;
            }
            if (tagItem == null)
            {
                client.Player.SendMessage(LanguageMgr.GetTranslation("EquipBringUpHandler.ItemNotFound"));
                return 0;
            }
            ItemTemplateInfo nextItem = tagItem.Template;
            if (nextItem == null)
            {
                client.Player.SendMessage(LanguageMgr.GetTranslation("EquipBringUpHandler.DoNotSuport"));
                return 0;
            }
            bool isSuccessful = false;
            string msg = LanguageMgr.GetTranslation("EquipBringUpHandler.Update.Fail");
            string scorssMsg = string.Empty;
            int exp = 0;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    eBageType bagType = (eBageType)packet.ReadInt(); //pkg.writeInt(arg.shift());
                    int place = packet.ReadInt();
                    ItemInfo beEatenItem = client.Player.GetItemAt(bagType, place);
                    if (beEatenItem != null && beEatenItem.IsBring() && !beEatenItem.cellLocked)
                    {
                        exp += CalculateExperience(tagItem.Template, beEatenItem);
                        int CategoryID = client.Player.GetItemAt(bagType, place).Template.CategoryID;
                        int TemplateID = client.Player.GetItemAt(bagType, place).TemplateID;
                        if (CategoryID is 8 or 9)
                        {
                            _ = client.Player.EquipBag.RemoveItemAt(place);
                        }
                        else if (TemplateID == 12252)
                        {
                            _ = client.Player.RemoveTemplate(TemplateID, count);
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    //Console.WriteLine("bagType: {0}, place: {1}", bagType, place);
                }
                if (exp != 0)
                {
                    isSuccessful = true;
                    //tagItem.curExp += exp;
                }
                else
                {
                    tagItem = client.Player.StoreBag.GetItemAt(0);
                }
                //Console.WriteLine("exp {0}, count {1}", exp, count);
            }
            else
            {
                _ = packet.ReadInt(); //pkg.writeInt(arg.shift());
                exp = packet.ReadInt(); //pkg.writeInt(arg.shift());
                int needMoney = GameProperties.ItemDevelopPrice * exp;
                if (client.Player.MoneyDirect(needMoney, IsAntiMult: false, true, true))
                {
                    isSuccessful = true;
                    //tagItem.curExp += exp;
                }
                //Console.WriteLine("bagType: {0}, buyCount: {1}", bagType, exp);
            }

            if (isSuccessful)
            {
                tagItem.IsBinds = true;
                int totalExp = exp + tagItem.curExp;
                int curExp = tagItem.curExp;
                while (curExp <= totalExp)
                {
                    nextItem = ItemMgr.FindItemTemplate(nextItem.FineSuitType);
                    //Console.WriteLine("curExp: {0}, tempItem.Property2: {1}:: totalExp: {2} exp: {3}", curExp, nextItem.Property2, totalExp, exp);
                    if (nextItem != null && totalExp >= nextItem.Property2)
                    {
                        msg = LanguageMgr.GetTranslation("EquipBringUpHandler.Update.Success2", tagItem.Template.Name, nextItem.Name);
                        ItemInfo newItem = ItemInfo.CloneFromTemplate(nextItem, tagItem);
                        _ = client.Player.StoreBag.RemoveItemAt(0);
                        _ = client.Player.StoreBag.AddItemTo(newItem, 0);
                        tagItem = newItem;
                        if (nextItem.Property1 >= 5)
                        {
                            scorssMsg = LanguageMgr.GetTranslation("EquipBringUpHandler.congratulation", client.Player.PlayerCharacter.NickName, tagItem.TemplateID);
                        }
                    }
                    else
                    {
                        msg = LanguageMgr.GetTranslation("EquipBringUpHandler.Update.Success1", exp);
                    }
                    if (nextItem != null)
                    {
                        curExp += nextItem.Property2;
                    }
                }
                tagItem.curExp = totalExp;
            }

            client.Player.SendMessage(msg);

            GSPacketIn pkg = new((int)ePackageType.EQUIP_BRING_UP);
            pkg.WriteBoolean(isSuccessful);
            client.Player.SendTCP(pkg);

            client.Player.StoreBag.UpdateItem(tagItem);

            //client.Player.StoreBag.SaveNewItemToDatabase();

            if (!string.IsNullOrEmpty(scorssMsg) && tagItem.ItemID > 0)
            {
                GSPacketIn sysNotice = WorldMgr.SendSysNotice(eMessageType.SYS_TIP_NOTICE, scorssMsg, tagItem.ItemID, tagItem.TemplateID/*, client.Player.ZoneId*/, null);
                GameServer.Instance.LoginServer.SendPacket(sysNotice);
            }
            return 0;
        }

        private int CalculateExperience(ItemTemplateInfo tagItem, ItemInfo beEatenItem)
        {
            int tagLevel = tagItem.Property1;
            int tagQuality = tagItem.Property3;

            int eatenLevel = beEatenItem.Template.Property1;
            int eatenOrigExp = beEatenItem.Template.Property2;
            int eatenQuality = beEatenItem.Template.Property3;
            int eatenCurExp = beEatenItem.curExp;

            if (tagQuality < eatenQuality)
            {
                ItemTemplateInfo beEatenTempleteInfo = GetTempleteInfoByLevel(eatenLevel, tagItem);
                eatenOrigExp = beEatenTempleteInfo.Property2;
                eatenCurExp = eatenOrigExp;
            }

            if (eatenCurExp == 0)
            {
                eatenCurExp = eatenOrigExp;
            }
            return eatenCurExp;
        }

        private ItemTemplateInfo GetTempleteInfoByLevel(int eatenLevel, ItemTemplateInfo tagItemInfo)
        {
            int curLevel = tagItemInfo.Property1;
            if (curLevel > eatenLevel)
            {
                while (curLevel > eatenLevel)
                {
                    if (tagItemInfo.Property4 == 0)
                    {
                        break;
                    }

                    tagItemInfo = ItemMgr.FindItemTemplate(tagItemInfo.Property4);
                    curLevel = tagItemInfo.Property1;
                }
            }
            else if (curLevel < eatenLevel)
            {
                while (curLevel < eatenLevel)
                {
                    if (tagItemInfo.FineSuitType == 0)
                    {
                        break;
                    }

                    tagItemInfo = ItemMgr.FindItemTemplate(tagItemInfo.FineSuitType);
                    curLevel = tagItemInfo.Property1;
                }
            }
            return tagItemInfo;
        }
    }
}
