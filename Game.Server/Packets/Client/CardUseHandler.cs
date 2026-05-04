using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.Buffer;
using Game.Server.GameUtils;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;

namespace Game.Server.Packets.Client
{
    [PacketHandler(183, "卡片使用")]
    public class CardUseHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int bageType = packet.ReadInt();
            int num = packet.ReadInt();
            List<int> list = [];
            int num2 = packet.ReadInt();
            for (int i = 0; i < num2; i++)
            {
                int item = packet.ReadInt();
                list.Add(item);
            }
            _ = packet.ReadInt();
            bool flag = packet.ReadBoolean();
            if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked && !flag)
            {
                _ = client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Bag.Locked"));
                return 0;
            }

            string translateId = null;
            string message = null;
            ItemInfo itemInfo = null;
            _ = new ShopItemInfo();
            PlayerInventory playerInventory = null;

            foreach (int item2 in list)
            {
                if (num == -1)
                {
                    int needGold = 0;
                    int needMoney = 0;
                    int Money = client.Player.PlayerCharacter.Money;
                    int MoneyLock = client.Player.PlayerCharacter.MoneyLock;
                    ShopItemInfo shopItemInfo = ShopMgr.GetShopItemInfoById(item2);
                    if (shopItemInfo != null && ShopMgr.IsOnShop(item2))
                    {
                        itemInfo = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(shopItemInfo.TemplateID), 1, 102);
                        if (shopItemInfo.APrice1 == -1 && shopItemInfo.AValue1 != 0)
                        {
                            needMoney = shopItemInfo.AValue1;
                            itemInfo.ValidDate = shopItemInfo.AUnit;
                        }

                        if (itemInfo != null)
                        {
                            // Bakiye kontrolü
                            if (needGold <= client.Player.PlayerCharacter.Gold && (needMoney <= Money || needMoney <= MoneyLock) && (needMoney > 0 || needGold > 0))
                            {
                                bool paymentSuccess = true;

                                // GÜNLÜK LİMİT KONTROLÜ
                                // MoneyDirect fonksiyonu limit kontrolünü yapar ve false dönerse engeller.
                                if (needMoney > 0)
                                {
                                    // Eğer limit doluysa MoneyDirect false döner ve hata mesajı gönderilir.
                                    if (!client.Player.MoneyDirect(needMoney, true, false, true))
                                    {
                                        paymentSuccess = false;
                                        // MoneyDirect zaten "Günlük limit doldu" mesajını gönderdi.
                                    }
                                }

                                if (paymentSuccess)
                                {
                                    if (needGold > 0)
                                    {
                                        _ = client.Player.RemoveGold(needGold);
                                    }
                                    translateId = "CardUseHandler.Success";
                                }
                                else
                                {
                                    // Ödeme başarısız (Limit doldu veya yetersiz bakiye)
                                    itemInfo = null; // Item'ı null yaparak işlemin iptal edilmesini sağla
                                }
                            }
                            else
                            {
                                itemInfo = null;
                            }
                        }
                    }
                    else
                    {
                        translateId = "Bu ürün satın alınamaz!";
                    }
                }
                else
                {
                    playerInventory = client.Player.GetInventory((eBageType)bageType);
                    if (playerInventory != null)
                    {
                        itemInfo = playerInventory.GetItemAt(num);
                        translateId = "CardUseHandler.Success";
                    }
                }

                if (itemInfo == null)
                {
                    continue;
                }

                string empty = string.Empty;
                switch (itemInfo.Template.Property1)
                {
                    case 23:
                        {
                            DateTime ExpireDayOut = DateTime.Now;
                            using (PlayerBussiness playerBussiness = new())
                            {
                                int typeVIP = client.Player.SetTypeVIP(itemInfo.ValidDate);
                                _ = playerBussiness.VIPRenewal(client.Player.PlayerCharacter.NickName, itemInfo.ValidDate, typeVIP, ref ExpireDayOut);
                                if (itemInfo.ValidDate == 0)
                                {
                                    itemInfo.ValidDate = 1;
                                }
                                if (client.Player.PlayerCharacter.typeVIP == 0)
                                {
                                    client.Player.OpenVIP(itemInfo.ValidDate, ExpireDayOut);
                                    message = $"Tebrikler, {itemInfo.ValidDate} VIP ayrıcalık kullanım tarihini aldınız!";
                                }
                                else
                                {
                                    client.Player.ContinuousVIP(itemInfo.ValidDate, ExpireDayOut);
                                    message = $"Ekstra {itemInfo.ValidDate} gün VIP ayrıcalığı kazanırsınız!";
                                }
                                _ = client.Player.Out.SendOpenVIP(client.Player);
                                client.Player.OnVIPUpgrade(client.Player.PlayerCharacter.VIPLevel, client.Player.PlayerCharacter.VIPExp);
                                if (itemInfo.Template.CanDelete)
                                {
                                    _ = client.Player.GetInventory((eBageType)bageType).RemoveCountFromStack(itemInfo, 1);
                                }
                            }
                            _ = client.Out.SendMessage(eMessageType.ChatNormal, message);
                            continue;
                        }
                    case 21:
                        if (itemInfo.IsValidItem())
                        {
                            int num5 = itemInfo.Template.Property2 * itemInfo.Count;
                            if (client.Player.Level == LevelMgr.MaxLevel)
                            {
                                int num6 = num5 / 100;
                                if (num6 > 0)
                                {
                                    _ = client.Player.AddOffer(num6);
                                    client.Player.UpdateProperties();
                                    translateId = string.Format("", num6);
                                }
                            }
                            else
                            {
                                _ = client.Player.AddGP(num5, false);
                                translateId = "GPDanUser.Success";
                            }
                            if (itemInfo.Template.CanDelete)
                            {
                                _ = client.Player.RemoveCountFromStack(itemInfo, itemInfo.Count);
                            }
                        }
                        _ = client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation(translateId, itemInfo.Template.Property2 * itemInfo.Count));
                        continue;
                }

                AbstractBuffer abstractBuffer = BufferList.CreateBuffer(itemInfo.Template, itemInfo.ValidDate);
                if (abstractBuffer != null)
                {
                    abstractBuffer.Start(client.Player);
                    if (num != -1)
                    {
                        _ = (playerInventory?.RemoveCountFromStack(itemInfo, 1));
                    }
                }

                _ = client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation(translateId));
            }
            return 0;
        }
    }
}