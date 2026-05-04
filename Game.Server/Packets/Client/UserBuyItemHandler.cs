using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.Managers;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Game.Server.Packets.Client
{
    [PacketHandler(44, "购买物品")]
    public class UserBuyItemHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            bool flag2 = UserBuyItemHandler.countConnect >= 3000;
            int result;
            if (flag2)
            {
                client.Disconnect();
                result = 0;
            }
            else
            {
                int gold = 0;
                int money = 0;
                int offer = 0;
                int gifttoken = 0;
                int petScore = 0;
                int Score = 0;
                int dmgScore = 0;
                StringBuilder payGoods = new();
                eMessageType eMsg = eMessageType.GM_NOTICE;
                string msg = "UserBuyItemHandler.Success";
                GSPacketIn gSPacketIn = new(44, client.Player.PlayerCharacter.ID);
                List<ItemInfo> buyitems = [];
                Dictionary<int, int> needitemsinfo = [];
                List<bool> dresses = [];
                List<int> places = [];
                StringBuilder types = new();
                bool isBinds = true;
                ShopItemInfo shopItem = null;
                ConsortiaInfo consotia = ConsortiaMgr.FindConsortiaInfo(client.Player.PlayerCharacter.ConsortiaID);
                int count = packet.ReadInt();
                bool flag3 = count is > 0 and <= 99;
                if (flag3)
                {
                    List<string> ids = [];
                    for (int i = 0; i < count; i++)
                    {
                        int GoodsID = packet.ReadInt();
                        int type = packet.ReadInt();
                        string color = packet.ReadString();
                        bool dress = packet.ReadBoolean();
                        string skin = packet.ReadString();
                        int place = packet.ReadInt();
                        ids.Add(GoodsID.ToString());
                        shopItem = ShopMgr.GetShopItemInfoById(GoodsID);
                        bool flag4 = shopItem == null || !ShopMgr.IsOnShop(shopItem.ID);
                        if (!flag4)
                        {
                            bool flag5 = shopItem.ShopID != 2 && ShopMgr.CanBuy(shopItem.ShopID, (consotia != null) ? consotia.ShopLevel : 1, ref isBinds, client.Player.PlayerCharacter.ConsortiaID, client.Player.PlayerCharacter.Riches);
                            if (!flag5)
                            {
                                _ = client.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation("UserBuyItemHandler.FailByPermission", Array.Empty<object>()));
                                return 1;
                            }
                            bool flag6 = shopItem.ShopID == 20;
                            if (flag6)
                            {
                                bool flag7 = client.Player.PlayerCharacter.ShopFinallyGottenTime.Date == DateTime.Now.Date || !WorldMgr.UpdateShopFreeCount(shopItem.ID, shopItem.LimitCount);
                                if (flag7)
                                {
                                    _ = client.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation("UserBuyItemHandler.FailByPermission2", Array.Empty<object>()));
                                    return 1;
                                }
                                List<ShopFreeCountInfo> allShopFreeCount = WorldMgr.GetAllShopFreeCount();
                                client.Out.SendShopGoodsCountUpdate(allShopFreeCount);
                                client.Player.PlayerCharacter.ShopFinallyGottenTime = DateTime.Now.Date;
                                needitemsinfo.Add(-9999, 1);
                            }
                            ItemInfo item = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(shopItem.TemplateID), 1, 102);
                            bool flag8 = shopItem.BuyType == 0;
                            if (flag8)
                            {
                                bool flag9 = 1 == type;
                                if (flag9)
                                {
                                    item.ValidDate = shopItem.AUnit;
                                }
                                bool flag10 = 2 == type;
                                if (flag10)
                                {
                                    item.ValidDate = shopItem.BUnit;
                                }
                                bool flag11 = 3 == type;
                                if (flag11)
                                {
                                    item.ValidDate = shopItem.CUnit;
                                }
                            }
                            else
                            {
                                bool flag12 = 1 == type;
                                if (flag12)
                                {
                                    item.Count = shopItem.AUnit;
                                }
                                bool flag13 = 2 == type;
                                if (flag13)
                                {
                                    item.Count = shopItem.BUnit;
                                }
                                bool flag14 = 3 == type;
                                if (flag14)
                                {
                                    item.Count = shopItem.CUnit;
                                }
                            }
                            bool flag15 = item == null && shopItem == null;
                            if (!flag15)
                            {
                                item.Color = color ?? "";
                                item.Skin = skin ?? "";
                                item.IsBinds = isBinds || Convert.ToBoolean(shopItem.IsBind);
                                item.IsBinds = true;
                                _ = types.Append(type);
                                _ = types.Append(",");
                                buyitems.Add(item);
                                dresses.Add(dress);
                                places.Add(place);
                                List<int> list5 = ItemInfo.SetItemType(shopItem, type, ref gold, ref money, ref offer, ref gifttoken, ref petScore, ref Score, ref dmgScore);
                                for (int j = 0; j < list5.Count; j += 2)
                                {
                                    bool flag16 = needitemsinfo.ContainsKey(list5[j]);
                                    if (flag16)
                                    {
                                        Dictionary<int, int> dictionary = needitemsinfo;
                                        int key = list5[j];
                                        dictionary[key] += list5[j + 1];
                                    }
                                    else
                                    {
                                        needitemsinfo.Add(list5[j], list5[j + 1]);
                                    }
                                }
                            }
                        }
                    }
                    bool flag17 = buyitems.Count == 0;
                    if (flag17)
                    {
                        result = 1;
                    }
                    else
                    {
                        bool flag18 = client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked;
                        if (flag18)
                        {
                            _ = client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Bag.Locked", Array.Empty<object>()));
                            result = 1;
                        }
                        else
                        {
                            bool flag = true;
                            foreach (KeyValuePair<int, int> item2 in needitemsinfo)
                            {
                                bool flag19 = item2.Key != -9999 && client.Player.GetTemplateCount(item2.Key) < item2.Value;
                                if (flag19)
                                {
                                    flag = false;
                                }
                            }
                            bool flag20 = !flag;
                            if (flag20)
                            {
                                string translateId2 = "UserBuyItemHandler.NoBuyItem";
                                _ = client.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation(translateId2, Array.Empty<object>()));
                                result = 1;
                            }
                            else
                            {
                                int moneyBefore = client.Player.PlayerCharacter.Money;
                                int moneyLockBefore = client.Player.PlayerCharacter.MoneyLock;
                                bool flag21 = shopItem != null;
                                if (flag21)
                                {
                                    bool flag22 = shopItem.LimitCount != -1;
                                    if (flag22)
                                    {
                                        bool flag23 = shopItem.LimitCount == 0 || buyitems.Count > shopItem.LimitCount;
                                        if (flag23)
                                        {
                                            _ = client.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, LanguageMgr.GetTranslation("UserBuyItemHandler.LimitCount1", new object[]
                                            {
                                                shopItem.LimitCount
                                            }));
                                            return 1;
                                        }
                                    }
                                }
                                bool flag24 = gold >= 0 && money >= 0 && offer >= 0 && gifttoken >= 0 && petScore >= 0 && Score >= 0 && dmgScore >= 0 && (gold > 0 || money > 0 || offer > 0 || gifttoken > 0 || petScore > 0 || Score > 0 || dmgScore > 0 || needitemsinfo.Count > 0);
                                if (flag24)
                                {
                                    int myMoney = client.Player.PlayerCharacter.Money;
                                    int myMoneyLock = client.Player.PlayerCharacter.MoneyLock;
                                    bool flag25 = gold <= client.Player.PlayerCharacter.Gold && (money <= myMoney || money <= myMoneyLock) && offer <= client.Player.PlayerCharacter.Offer && gifttoken <= client.Player.PlayerCharacter.GiftToken && petScore <= client.Player.PlayerCharacter.petScore && Score <= client.Player.PlayerCharacter.Score && dmgScore <= client.Player.PlayerCharacter.damageScores;
                                    if (flag25)
                                    {
                                        // ---------------------------------------------------------
                                        // GÜVENLİK GÜNCELLEMESİ: PARA KESME KONTROLÜ
                                        // ---------------------------------------------------------

                                        // 1. Kupon (Money) harcaması varsa kontrol et
                                        bool moneySuccess = true;
                                        if (money > 0)
                                        {
                                            // MoneyDirect, limit doluysa FALSE döner.
                                            moneySuccess = client.Player.MoneyDirect(money, true, false, true);
                                        }

                                        // 2. Eğer ödeme başarılıysa (Limit uygunsa) işlemleri yap
                                        if (moneySuccess)
                                        {
                                            _ = client.Player.RemoveGold(gold);
                                            _ = client.Player.RemoveOffer(offer);
                                            _ = client.Player.RemoveGiftToken(gifttoken);
                                            _ = client.Player.RemovePetScore(petScore);
                                            _ = client.Player.RemoveScore(Score);
                                            _ = client.Player.RemoveDamageScores(dmgScore);

                                            bool flag26 = shopItem != null;
                                            if (flag26)
                                            {
                                                bool flag27 = shopItem.LimitCount > 0;
                                                if (flag27)
                                                {
                                                    shopItem.LimitCount -= buyitems.Count;
                                                }
                                            }
                                            ProduceBussiness pb = new();
                                            _ = pb.UpdateShop(shopItem);
                                            int moneyAfter = client.Player.PlayerCharacter.Money;
                                            int moneyLockAfter = client.Player.PlayerCharacter.MoneyLock;
                                            string str = string.Format("money: {0} | gold: {1} | offter: {2} | gifttoken: {3} | petScore: {4} | boguScore: {5} | moneyBefore: {6} | moneyAfter: {7} | dmg Score: {8} | moneyLockBefore: {9} | moneyLockAfter: {10}", new object[]
                                            {
                                                money,
                                                gold,
                                                offer,
                                                gifttoken,
                                                petScore,
                                                Score,
                                                moneyBefore,
                                                moneyAfter,
                                                dmgScore,
                                                moneyLockBefore,
                                                moneyLockAfter
                                            });
                                            foreach (KeyValuePair<int, int> item3 in needitemsinfo)
                                            {
                                                bool flag28 = item3.Key != -9999;
                                                if (flag28)
                                                {
                                                    _ = client.Player.RemoveTemplateInShop(item3.Key, item3.Value);
                                                }
                                                _ = payGoods.Append(item3.Key.ToString() + ",");
                                            }
                                            bool flag29 = needitemsinfo.Count > 0;
                                            if (flag29)
                                            {
                                                client.Player.UpdateProperties();
                                            }
                                            string str2 = str + " | itemNeed: " + string.Join<int>(",", needitemsinfo.Keys.ToArray<int>());
                                            string text3 = "";
                                            int num5 = 0;
                                            MailInfo mailInfo = new();
                                            StringBuilder stringBuilder3 = new();
                                            _ = stringBuilder3.Append(LanguageMgr.GetTranslation("GoodsPresentHandler.AnnexRemark", Array.Empty<object>()));
                                            for (int k = 0; k < buyitems.Count; k++)
                                            {
                                                string str3 = text3;
                                                bool flag30 = !(text3 == "");
                                                string str5;
                                                if (flag30)
                                                {
                                                    string str4 = buyitems[k].TemplateID.ToString();
                                                    str5 = "," + str4;
                                                }
                                                else
                                                {
                                                    str5 = buyitems[k].TemplateID.ToString();
                                                }
                                                text3 = str3 + str5;
                                                bool flag31 = client.Player.AddTemplate(buyitems[k], buyitems[k].Template.BagType, buyitems[k].Count, false);
                                                if (flag31)
                                                {
                                                    bool flag32 = !dresses[k] || !buyitems[k].CanEquip();
                                                    if (!flag32)
                                                    {
                                                        int num6 = client.Player.EquipBag.FindItemEpuipSlot(buyitems[k].Template);
                                                        bool flag33 = (num6 != 9 && num6 != 10) || (places[k] != 9 && places[k] != 10);
                                                        if (flag33)
                                                        {
                                                            bool flag34 = (num6 == 7 || num6 == 8) && (places[k] == 7 || places[k] == 8);
                                                            if (flag34)
                                                            {
                                                                num6 = places[k];
                                                            }
                                                        }
                                                        else
                                                        {
                                                            num6 = places[k];
                                                        }
                                                        _ = client.Player.EquipBag.MoveItem(buyitems[k].Place, num6, 0);
                                                        msg = "UserBuyItemHandler.Save";
                                                    }
                                                }
                                                else
                                                {
                                                    using PlayerBussiness playerBussiness = new();
                                                    buyitems[k].UserID = 0;
                                                    _ = playerBussiness.AddGoods(buyitems[k]);
                                                    num5++;
                                                    _ = stringBuilder3.Append(num5);
                                                    _ = stringBuilder3.Append("、");
                                                    _ = stringBuilder3.Append(buyitems[k].Template.Name);
                                                    _ = stringBuilder3.Append("x");
                                                    _ = stringBuilder3.Append(buyitems[k].Count);
                                                    _ = stringBuilder3.Append(";");
                                                    switch (num5)
                                                    {
                                                        case 1:
                                                            {
                                                                string text4 = mailInfo.Annex1 = buyitems[k].ItemID.ToString();
                                                                mailInfo.Annex1Name = buyitems[k].Template.Name;
                                                                break;
                                                            }
                                                        case 2:
                                                            {
                                                                string text4 = mailInfo.Annex2 = buyitems[k].ItemID.ToString();
                                                                mailInfo.Annex2Name = buyitems[k].Template.Name;
                                                                break;
                                                            }
                                                        case 3:
                                                            {
                                                                string text4 = mailInfo.Annex3 = buyitems[k].ItemID.ToString();
                                                                mailInfo.Annex3Name = buyitems[k].Template.Name;
                                                                break;
                                                            }
                                                        case 4:
                                                            {
                                                                string text4 = mailInfo.Annex4 = buyitems[k].ItemID.ToString();
                                                                mailInfo.Annex4Name = buyitems[k].Template.Name;
                                                                break;
                                                            }
                                                        case 5:
                                                            {
                                                                string text4 = mailInfo.Annex5 = buyitems[k].ItemID.ToString();
                                                                mailInfo.Annex5Name = buyitems[k].Template.Name;
                                                                break;
                                                            }
                                                    }
                                                    bool flag35 = num5 == 5;
                                                    if (flag35)
                                                    {
                                                        num5 = 0;
                                                        mailInfo.AnnexRemark = stringBuilder3.ToString();
                                                        _ = stringBuilder3.Remove(0, stringBuilder3.Length);
                                                        _ = stringBuilder3.Append(LanguageMgr.GetTranslation("GoodsPresentHandler.AnnexRemark", Array.Empty<object>()));
                                                        mailInfo.Content = LanguageMgr.GetTranslation("UserBuyItemHandler.Title", Array.Empty<object>()) + mailInfo.Annex1Name + "]";
                                                        mailInfo.Gold = 0;
                                                        mailInfo.Money = 0;
                                                        mailInfo.Receiver = client.Player.PlayerCharacter.NickName;
                                                        mailInfo.ReceiverID = client.Player.PlayerCharacter.ID;
                                                        mailInfo.Sender = mailInfo.Receiver;
                                                        mailInfo.SenderID = mailInfo.ReceiverID;
                                                        mailInfo.Title = mailInfo.Content;
                                                        mailInfo.Type = 8;
                                                        _ = playerBussiness.SendMail(mailInfo);
                                                        eMsg = eMessageType.BIGBUGLE_NOTICE;
                                                        msg = "UserBuyItemHandler.Mail";
                                                        mailInfo.Revert();
                                                    }
                                                }
                                            }
                                            string content = str2 + " | listsBuy: " + text3;
                                            bool flag36 = num5 > 0;
                                            if (flag36)
                                            {
                                                using PlayerBussiness playerBussiness2 = new();
                                                mailInfo.AnnexRemark = stringBuilder3.ToString();
                                                mailInfo.Content = LanguageMgr.GetTranslation("UserBuyItemHandler.Title", Array.Empty<object>()) + mailInfo.Annex1Name + "]";
                                                mailInfo.Gold = 0;
                                                mailInfo.Money = 0;
                                                mailInfo.Receiver = client.Player.PlayerCharacter.NickName;
                                                mailInfo.ReceiverID = client.Player.PlayerCharacter.ID;
                                                mailInfo.Sender = mailInfo.Receiver;
                                                mailInfo.SenderID = mailInfo.ReceiverID;
                                                mailInfo.Title = mailInfo.Content;
                                                mailInfo.Type = 8;
                                                _ = playerBussiness2.SendMail(mailInfo);
                                                eMsg = eMessageType.BIGBUGLE_NOTICE;
                                                msg = "UserBuyItemHandler.Mail";
                                            }
                                            bool flag37 = eMsg == eMessageType.BIGBUGLE_NOTICE;
                                            if (flag37)
                                            {
                                                _ = client.Out.SendMailResponse(client.Player.PlayerCharacter.ID, eMailRespose.Receiver);
                                            }
                                            client.Player.OnPaid(money, gold, offer, gifttoken, petScore, 0, dmgScore, payGoods.ToString());
                                            client.Player.AddLog("Buy Shop", content);
                                        }
                                        // ÖDEME BAŞARISIZSA (LİMİT DOLU)
                                        else
                                        {
                                            _ = client.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, "Satın alma başarısız! Günlük kupon harcama limitinizi aşmış olabilirsiniz.");
                                            return 0;
                                        }
                                        // ---------------------------------------------------------
                                        // GÜVENLİK GÜNCELLEMESİ SONU
                                        // ---------------------------------------------------------
                                    }
                                    else
                                    {
                                        bool flag38 = money > client.Player.PlayerCharacter.Money && money > 0;
                                        if (flag38)
                                        {
                                            msg = "UserBuyItemHandler.NoMoney";
                                        }
                                        bool flag39 = gold > client.Player.PlayerCharacter.Gold;
                                        if (flag39)
                                        {
                                            msg = "UserBuyItemHandler.NoGold";
                                        }
                                        bool flag40 = offer > client.Player.PlayerCharacter.Offer;
                                        if (flag40)
                                        {
                                            msg = "UserBuyItemHandler.NoOffer";
                                        }
                                        bool flag41 = gifttoken > client.Player.PlayerCharacter.GiftToken;
                                        if (flag41)
                                        {
                                            msg = "UserBuyItemHandler.GiftToken";
                                        }
                                        bool flag42 = petScore > client.Player.PlayerCharacter.petScore;
                                        if (flag42)
                                        {
                                            msg = "UserBuyItemHandler.petScore";
                                        }
                                        bool flag43 = Score > client.Player.PlayerCharacter.Score;
                                        if (flag43)
                                        {
                                            msg = "UserBuyItemHandler.boguScore";
                                        }
                                        bool flag44 = dmgScore > client.Player.PlayerCharacter.damageScores;
                                        if (flag44)
                                        {
                                            msg = "Şu anki puanın yetersiz!";
                                        }
                                        eMsg = eMessageType.BIGBUGLE_NOTICE;
                                    }
                                    _ = client.Out.SendMessage(eMsg, LanguageMgr.GetTranslation(msg, Array.Empty<object>()));
                                    gSPacketIn.WriteInt(1);
                                    gSPacketIn.WriteInt(3);
                                    client.Player.SendTCP(gSPacketIn);
                                    result = 0;
                                }
                                else
                                {
                                    client.Player.SendMessage("Bir hata oluştu! Sorun yutiye bildirildi!");
                                    UserBuyItemHandler.log.Error("username: " + client.Player.PlayerCharacter.UserName + " - hack money down.");
                                    result = 0;
                                }
                            }
                        }
                    }
                }
                else
                {
                    client.Player.SendMessage("Ne yapmaya çalışıyon acaba?");
                    UserBuyItemHandler.log.Error(string.Concat(new string[]
                    {
                        "username: ",
                        client.Player.PlayerCharacter.UserName,
                        " - hack money down (count: ",
                        count.ToString(),
                        ")."
                    }));
                    result = 0;
                }
            }
            return result;
        }

        public static int countConnect = 0;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    }
}