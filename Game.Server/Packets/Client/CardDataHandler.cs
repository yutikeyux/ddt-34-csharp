using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server;
using Game.Server.GameUtils;
using Game.Server.Managers;
using Game.Server.Packets.Client;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;

[PacketHandler(216, "卡牌系统")]
internal class CardDataHandler : IPacketHandler
{
    public static Random random = new();

    public int HandlePacket(GameClient client, GSPacketIn packet)
    {
        int cmdCard = packet.ReadInt();
        CardInventory cardBag = client.Player.CardBag;
        List<ItemInfo> list = [];
        List<UsersCardInfo> infos;
        if (client.Player.PlayerCharacter.Grade < 20)
        {
            client.Player.SendMessage(LanguageMgr.GetTranslation("GameServer.LevelErrorUsing"));
            return 0;
        }
        using (PlayerBussiness playerBussiness = new())
        {
            infos = playerBussiness.GetUserCardEuqip(client.Player.PlayerCharacter.ID);
        }
        cardBag.BeginChanges();
        int slot;
        int place;
        switch (cmdCard)
        {
            case 0:
                slot = packet.ReadInt();
                place = packet.ReadInt();
                if (slot == place && slot >= 5)
                {
                    return 0;
                }
                if ((slot < 5 && place >= 5) || (slot == place && slot < 5))
                {
                    _ = cardBag.RemoveCardAt(slot);
                    client.Player.EquipBag.UpdatePlayerProperties();
                }
                else if (slot >= 5 && place < 5)
                {
                    UsersCardInfo itemAt2 = cardBag.GetItemAt(slot);
                    if (itemAt2 != null)
                    {
                        if (!cardBag.IsCardEquip(itemAt2.TemplateID))
                        {
                            _ = cardBag.RemoveCardAt(place);
                            UsersCardInfo usersCardInfo2 = itemAt2.Clone();
                            usersCardInfo2.Count = 0;
                            _ = cardBag.AddCardTo(usersCardInfo2, place);
                            client.Player.OnEquipCardEvent();
                            client.Player.EquipBag.UpdatePlayerProperties();
                        }
                        else
                        {
                            client.Player.SendMessage("Bu kart donatıldı!");
                        }
                    }
                }
                else
                {
                    _ = cardBag.MoveCard(slot, place);
                }
                break;
            case 1:
                {
                    place = packet.ReadInt();
                    UsersCardInfo usersCardInfo = new()
                    {
                        Count = -1,
                        UserID = client.Player.PlayerCharacter.ID,
                        Place = place,
                        TemplateID = 314101,
                        isFirstGet = true,
                        Damage = 0,
                        Guard = 0,
                        Attack = 0,
                        Defence = 0,
                        Luck = 0,
                        Agility = 0
                    };
                    _ = client.Player.CardBag.AddCardTo(usersCardInfo, place);
                    break;
                }
            case 2:
                {
                    slot = packet.ReadInt();
                    int count = packet.ReadInt();
                    ItemInfo itemInfo = client.Player.EquipBag.GetItemAt(slot);
                    if (itemInfo != null)
                    {
                        if (count <= 0 || count > itemInfo.Count)
                        {
                            client.Player.SendMessage("Kart mevcut değil.");
                            return 0;
                        }
                        int property = itemInfo.Template.Property5;
                        ItemTemplateInfo itemTemplateInfo = ItemMgr.FindItemTemplate(property);
                        if (itemTemplateInfo == null || itemTemplateInfo.CategoryID != 26)
                        {
                            client.Player.SendMessage("Kart mevcut değil!");
                            return 0;
                        }
                        _ = client.Player.EquipBag.RemoveCountFromStack(itemInfo, itemInfo.Count);
                        int num5 = itemInfo.Count;
                        Random random2 = new();
                        for (int i = 0; i < itemInfo.Count; i++)
                        {
                            num5 += random2.Next(1, 3);
                        }
                        _ = cardBag.AddCard(property, num5);
                    }
                    else
                    {
                        client.Player.SendMessage("Bu kart mevcut değil.");
                    }
                    break;
                }
            case 3:
                {
                    slot = packet.ReadInt();
                    if (slot < 5)
                    {
                        return 0;
                    }
                    UsersCardInfo itemAt = cardBag.GetItemAt(slot);
                    if (itemAt == null)
                    {
                        break;
                    }
                    if (itemAt.Level < CardMgr.MaxLevel())
                    {
                        CardUpdateConditionInfo cardUpdateCondition = CardMgr.GetCardUpdateCondition(itemAt.Level + 1);
                        if (cardUpdateCondition != null && itemAt.Count >= cardUpdateCondition.UpdateCardCount)
                        {
                            Random random = new();
                            itemAt.Count -= cardUpdateCondition.UpdateCardCount;
                            itemAt.CardGP += random.Next(cardUpdateCondition.MinExp, cardUpdateCondition.MaxExp);
                            if (itemAt.CardGP >= cardUpdateCondition.Exp)
                            {
                                CardUpdateInfo cardUpdateInfo = CardMgr.GetCardUpdateInfo(itemAt.TemplateID, cardUpdateCondition.Level);
                                if (cardUpdateInfo != null)
                                {
                                    itemAt.Level++;
                                    itemAt.Attack += cardUpdateInfo.Attack;
                                    itemAt.Defence += cardUpdateInfo.Defend;
                                    itemAt.Agility += cardUpdateInfo.Agility;
                                    itemAt.Luck += cardUpdateInfo.Lucky;
                                    itemAt.Damage += cardUpdateInfo.Damage;
                                    itemAt.Guard += cardUpdateInfo.Guard;
                                    UsersCardInfo cardEquip = cardBag.GetCardEquip(itemAt.TemplateID);
                                    if (cardEquip != null)
                                    {
                                        cardEquip.CopyProp(itemAt);
                                        cardBag.UpdateCard(cardEquip);
                                        client.Player.EquipBag.UpdatePlayerProperties();
                                        client.Player.OnEquipCardEvent();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("cardUpInfo is null - " + itemAt.TemplateID + " - " + cardUpdateCondition.Level + " - exp: " + cardUpdateCondition.Exp);
                                }
                            }
                            cardBag.UpdateCard(itemAt);
                        }
                        else
                        {
                            client.Player.SendMessage("Seviye atlatmak için yeterli kartınız bulunmamakta!");
                        }
                    }
                    else
                    {
                        client.Player.SendMessage("Kartınız zaten en üst seviyeye ulaştı!");
                    }
                    break;
                }
            case 4:
                _ = packet.ReadInt();
                _ = packet.ReadInt();
                break;
        }
        cardBag.CommitChanges();
        return 0;
    }
}
