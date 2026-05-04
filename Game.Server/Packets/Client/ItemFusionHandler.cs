using Bussiness;
using Game.Base.Packets;
using Game.Server.GameUtils;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Server.Packets.Client
{
    [PacketHandler((byte)ePackageType.ITEM_FUSION, "Füzyon")]
    public class ItemFusionHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            _ = new StringBuilder();
            int opertionType = packet.ReadByte();


            DateTime now = DateTime.UtcNow;


            if (client.Player.FusionPacketWindowStart == DateTime.MinValue ||
                (now - client.Player.FusionPacketWindowStart).TotalSeconds >= 1)
            {
                client.Player.FusionPacketWindowStart = now;
                client.Player.FusionPacketCount = 0;
            }

            client.Player.FusionPacketCount++;

            if (client.Player.FusionPacketCount > 7)
            {
                _ = client.Out.SendMessage(eMessageType.ERROR, "Lütfen yavaşlayın.");
                return 0;
            }

            int MinValid = int.MaxValue;
            int MinValidItem = 0;
            List<ItemInfo> Items = [];
            List<ItemInfo> AppendItems = [];
            if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
            {
                _ = client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("Bag.Locked"));
                return 1;
            }

            Items.Clear();
            PlayerInventory storeBag = client.Player.StoreBag;
            for (int i = 1; i <= 4; i++)
            {
                ItemInfo itemAt = storeBag.GetItemAt(i);
                if (itemAt != null)
                {
                    Items.Add(itemAt);
                }
            }

            if (Items.Count >= 4 && (Items[0].TemplateID != Items[1].TemplateID || Items[0].TemplateID != Items[2].TemplateID || Items[0].TemplateID != Items[3].TemplateID))
            {
                _ = client.Out.SendMessage(eMessageType.ERROR, LanguageMgr.GetTranslation("Farklı tipte ürünler mevcut!"));
                return 0;
            }

            if (MinValid == int.MaxValue)
            {
                foreach (ItemInfo item in Items)
                {
                    int[] Valid = new int[4];
                    for (int i = 0; i < Items.Count; i++)
                    {
                        Valid[i] = Items[i].ValidDate;
                    }
                    Array.Sort(Valid);
                    MinValidItem = Items[0].ValidDate != 0 && Items[1].ValidDate != 0 && Items[2].ValidDate != 0 && Items[3].ValidDate != 0 ? Valid[0] : Valid[1];
                }

                MinValid = MinValidItem;
            }
            bool isBind = false;
            //bool IsBind = false;
            bool result = false;
            bool isWeapon = false;
            ItemTemplateInfo rewardItem = FusionMgr.Fusion(Items, AppendItems, ref isBind, ref result);
            if (Items.Count != 4)
            {
                _ = client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("ItemFusionHandler.ItemNotEnough"));
                return 0;
            }
            if (opertionType == 0)
            {
                Dictionary<int, double> previewItemList = FusionMgr.FusionPreview(Items, AppendItems, ref isBind);
                if (previewItemList != null)
                {
                    if (previewItemList.Count != 0)
                    {
                        _ = client.Out.SendFusionPreview(client.Player, previewItemList, isBind, MinValid);
                    }
                }
            }
            else
            {
                int value = 400;
                if (client.Player.PlayerCharacter.Gold < 400)
                {
                    _ = client.Out.SendMessage(eMessageType.ERROR, LanguageMgr.GetTranslation("ItemFusionHandler.NoMoney"));
                    return 0;
                }
                if (rewardItem != null)
                {
                    ItemInfo itemAt = storeBag.GetItemAt(0);
                    if (rewardItem.CategoryID is 7 or 17)
                    {
                        isWeapon = true;
                    }
                    if (itemAt != null)
                    {
                        if (!client.Player.StackItemToAnother(itemAt) && !client.Player.AddItem(itemAt))
                        {
                            _ = client.Player.SendItemsToMail(itemAt, "İade öğeleri", "Sırt çantası dolu", eMailType.StoreCanel);
                        }
                        _ = storeBag.TakeOutItemAt(0);
                    }
                    _ = client.Player.RemoveGold(value);
                    for (int j = 0; j < Items.Count; j++)
                    {
                        Items[j].Count--;
                        client.Player.UpdateItem(Items[j]);
                    }
                    for (int k = 0; k < AppendItems.Count; k++)
                    {
                        AppendItems[k].Count--;
                        client.Player.UpdateItem(AppendItems[k]);
                    }
                    if (result)
                    {
                        if (rewardItem.BagType == eBageType.EquipBag)
                        {
                            if (isWeapon)
                            {
                                MinValid = 3; //silah füzyonlayan 3 gün füzyonlasın not: yuti
                                isBind = true;
                            }
                            else
                            {
                                MinValid = MinValidItem;
                            }
                        }
                        ItemInfo item = ItemInfo.CreateFromTemplate(rewardItem, 1, 105);
                        if (item == null)
                        {
                            return 0;
                        }
                        item.IsBinds = isBind;
                        item.ValidDate = MinValid;
                        client.Player.OnItemFusion(item.Template.FusionType);
                        client.Player.SendMessage(eMessageType.Normal, "Tebrikler! Füzyon Başarılı! Kazanılan: " + item.Template.Name + " x" + item.Count);

                        if (item.Template.CategoryID is 7 or 8 or 9 or 14 or 16 or 17 or 35)
                        {
                            _ = client.Player.SaveNewItems();
                            string translation = LanguageMgr.GetTranslation("ItemFusionHandler.Notice", client.Player.ZoneName, client.Player.PlayerCharacter.NickName, item.TemplateID);
                            GSPacketIn packet2 = WorldMgr.SendSysNotice(eMessageType.ChatNormal, translation, item.ItemID, item.TemplateID, null);
                            GameServer.Instance.LoginServer.SendPacket(packet2);
                            client.Player.AddLog("Fusion", "TemplateID: " + item.TemplateID + "|Name: " + item.Template.Name);
                        }
                        if (!client.Player.StoreBag.AddItemTo(item, 0))
                        {
                            _ = client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation(item.GetBagName()) + LanguageMgr.GetTranslation("ItemFusionHandler.NoPlace"));
                            client.Player.AddLog("Error", "ItemFusionError" + item.Template.Name + "|TemplateID:" + item.TemplateID);
                            _ = client.Player.SendItemsToMail(
                            [
                                item
                            ], LanguageMgr.GetTranslation("GameServer.Fustion.msg2", item.Name), LanguageMgr.GetTranslation("GameServer.Fustion.Msg3"), eMailType.BuyItem);
                        }
                        _ = client.Out.SendFusionResult(client.Player, result);
                    }
                    else
                    {
                        _ = client.Out.SendFusionResult(client.Player, result);
                        _ = client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("ItemFusionHandler.Failed"));
                    }
                    _ = client.Player.SaveIntoDatabase();
                }
                else
                {
                    _ = client.Out.SendMessage(eMessageType.Normal, LanguageMgr.GetTranslation("ItemFusionHandler.NoCondition"));
                }
            }
            return 0;
        }
    }
}
