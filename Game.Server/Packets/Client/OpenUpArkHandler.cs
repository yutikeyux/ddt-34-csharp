using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.GameUtils;
using Game.Server.Managers;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Server.Packets.Client
{
    [PacketHandler(63, "打开物品")]
    public class OpenUpArkHandler : IPacketHandler
    {
        public static readonly ILog log = LogManager.GetLogger("FlashErrorLogger");

        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int bageType = packet.ReadByte();
            int slot = packet.ReadInt();
            int num = packet.ReadInt();
            PlayerInventory inventory = client.Player.GetInventory((eBageType)bageType);
            ItemInfo itemAt = inventory.GetItemAt(slot);
            string str = "";

            if (itemAt != null && itemAt.IsValidItem() && itemAt.Template.CategoryID == 11
                && itemAt.Template.Property1 == 6
                && client.Player.PlayerCharacter.Grade >= itemAt.Template.NeedLevel)
            {
                if (num < 1 || num > itemAt.Count)
                {
                    num = itemAt.Count;
                }

                int num2 = 0;
                string str2 = "";
                StringBuilder itemNameBuilder = new StringBuilder();   // item isimleri için
                StringBuilder messageBuilder = new StringBuilder();    // mesaj için

                if (!inventory.RemoveCountFromStack(itemAt, num))
                {
                    return 0;
                }

                Dictionary<int, ItemInfo> dictionary = new Dictionary<int, ItemInfo>();
                List<ItemInfo> list = new List<ItemInfo>();

                messageBuilder.Append(LanguageMgr.GetTranslation("OpenUpArkHandler.Start"));

                for (int i = 0; i < num; i++)
                {
                    int point = 0;
                    int gold = 0;
                    int giftToken = 0;
                    int medal = 0;
                    int exp = 0;
                    int hardCurrency = 0;
                    int leagueMoney = 0;
                    int useableScore = 0;
                    int prestge = 0;
                    int honor = 0;
                    List<ItemInfo> list2 = new List<ItemInfo>();

                    ItemBoxMgr.CreateItemBox(itemAt.TemplateID, list2, ref gold, ref point, ref giftToken,
                        ref medal, ref exp, ref hardCurrency, ref leagueMoney, ref useableScore, ref prestge, ref honor);

                    if (point != 0)
                    {
                        num2 += point;
                        str2 = LanguageMgr.GetTranslation("OpenUpArkHandler.Money");
                        client.Player.AddMoney(point);
                    }
                    if (gold != 0)
                    {
                        num2 += gold;
                        str2 = LanguageMgr.GetTranslation("OpenUpArkHandler.Gold");
                        client.Player.AddGold(gold);
                    }
                    if (giftToken != 0)
                    {
                        num2 += giftToken;
                        str2 = LanguageMgr.GetTranslation("OpenUpArkHandler.GiftToken");
                        client.Player.AddGiftToken(giftToken);
                    }
                    if (medal != 0)
                    {
                        num2 += medal;
                        str2 = LanguageMgr.GetTranslation("OpenUpArkHandler.Medal");
                        client.Player.AddMedal(medal);
                    }
                    if (honor != 0)
                    {
                        num2 += honor;
                        str2 = LanguageMgr.GetTranslation("OpenUpArkHandler.honor");
                        client.Player.AddHonor(honor);
                    }
                    if (exp != 0)
                    {
                        num2 += exp;
                        if (client.Player.Level == LevelMgr.MaxLevel)
                        {
                            int num3 = num2 / 500;
                            if (num3 > 0)
                            {
                                client.Player.AddOffer(num3);
                                str2 = $"Maksimum seviyeye ulaştığınız için elde ettiğiniz toplam tecrübe {num3} mükafata dönüştürüldü!";
                            }
                        }
                        else
                        {
                            str2 = LanguageMgr.GetTranslation("OpenUpArkHandler.Exp");
                            client.Player.AddGP(exp, false);
                        }
                    }
                    if (hardCurrency != 0)
                    {
                        num2 += hardCurrency;
                        str2 = LanguageMgr.GetTranslation("OpenUpArkHandler.hardCurrency");
                        client.Player.AddHardCurrency(hardCurrency);
                    }
                    if (leagueMoney != 0)
                    {
                        num2 += leagueMoney;
                        str2 = LanguageMgr.GetTranslation("OpenUpArkHandler.leagueMoney");
                        client.Player.AddLeagueMoney(leagueMoney);
                    }
                    if (useableScore != 0)
                    {
                        num2 += useableScore;
                        str2 = LanguageMgr.GetTranslation("OpenUpArkHandler.useableScore");
                    }
                    if (prestge != 0)
                    {
                        num2 += prestge;
                        str2 = LanguageMgr.GetTranslation("OpenUpArkHandler.prestge");
                    }

                    foreach (ItemInfo item in list2)
                    {
                        if (!dictionary.ContainsKey(item.TemplateID))
                        {
                            dictionary.Add(item.TemplateID, item);
                        }
                        else
                        {
                            // Sayı biriktirirken compose ve strengthen değerlerini de güncelle
                            dictionary[item.TemplateID].Count += item.Count;

                            // Compose değerleri sıfırdan büyükse güncelle
                            if (item.AttackCompose > 0)
                                dictionary[item.TemplateID].AttackCompose = item.AttackCompose;
                            if (item.DefendCompose > 0)
                                dictionary[item.TemplateID].DefendCompose = item.DefendCompose;
                            if (item.AgilityCompose > 0)
                                dictionary[item.TemplateID].AgilityCompose = item.AgilityCompose;
                            if (item.LuckCompose > 0)
                                dictionary[item.TemplateID].LuckCompose = item.LuckCompose;
                            if (item.StrengthenLevel > 0)
                                dictionary[item.TemplateID].StrengthenLevel = item.StrengthenLevel;
                        }
                    }
                }

                string name = itemAt.Template.Name;

                if (num2 > 0)
                {
                    messageBuilder.Append(num2 + str2);
                }

                if (dictionary.Count > 0)
                {
                    // Önce item isimlerini topla
                    foreach (ItemInfo value3 in dictionary.Values)
                    {
                        itemNameBuilder.Append(value3.Template.Name + "x" + value3.Count + ",");
                    }

                    // İsim listesini mesaja ekle
                    if (itemNameBuilder.Length > 0)
                    {
                        itemNameBuilder.Remove(itemNameBuilder.Length - 1, 1); // son virgülü sil
                        messageBuilder.Append(itemNameBuilder.ToString());
                    }

                    // Mesajı kapat
                    if (messageBuilder.Length > 0 && messageBuilder[messageBuilder.Length - 1] == ',')
                        messageBuilder.Remove(messageBuilder.Length - 1, 1);
                    messageBuilder.Append(".");

                    // Mesajı gönder
                    client.Out.SendMessage(eMessageType.GM_NOTICE, str + messageBuilder.ToString());

                    // Paketi hazırla
                    GSPacketIn gSPacketIn = new GSPacketIn(63, client.Player.PlayerCharacter.ID);
                    gSPacketIn.WriteString(name);
                    gSPacketIn.WriteByte((byte)dictionary.Count);

                    foreach (ItemInfo value3 in dictionary.Values)
                    {
                        // Pakete yaz — tüm compose ve strengthen değerleri artık doğru
                        gSPacketIn.WriteInt(value3.TemplateID);
                        gSPacketIn.WriteInt(value3.Count);
                        gSPacketIn.WriteBoolean(value3.IsBinds);
                        gSPacketIn.WriteInt(value3.ValidDate);
                        gSPacketIn.WriteInt(value3.StrengthenLevel);    // ✅ artık aktarılıyor
                        gSPacketIn.WriteInt(value3.AttackCompose);      // ✅ artık aktarılıyor
                        gSPacketIn.WriteInt(value3.DefendCompose);      // ✅ artık aktarılıyor
                        gSPacketIn.WriteInt(value3.AgilityCompose);     // ✅ artık aktarılıyor
                        gSPacketIn.WriteInt(value3.LuckCompose);        // ✅ artık aktarılıyor

                        // Sunucu duyurusu
                        if (value3.IsTips)
                        {
                            GameServer.Instance.LoginServer.SendPacket(
                                WorldMgr.SendSysNotice(eMessageType.ChatNormal,
                                    $"{client.Player.ZoneName} oyuncusu [{client.Player.PlayerCharacter.NickName}] kutu açarak kazandı: ",
                                    value3.ItemID, value3.TemplateID, null));
                        }

                        // Envantere ekle
                        if (value3.Template.MaxCount < 2)
                        {
                            // Stack olmayan itemler: her biri için ayrı clone oluştur
                            for (int xi = 0; xi < value3.Count; xi++)
                            {
                                ItemInfo clone0 = ItemInfo.CreateFromTemplate(value3.Template, 1, value3.RemoveType);
                                clone0.ValidDate = value3.ValidDate;
                                clone0.IsBinds = value3.IsBinds;
                                clone0.StrengthenLevel = value3.StrengthenLevel;   // ✅ düzeltildi
                                clone0.AttackCompose = value3.AttackCompose;     // ✅ düzeltildi
                                clone0.DefendCompose = value3.DefendCompose;     // ✅ düzeltildi
                                clone0.AgilityCompose = value3.AgilityCompose;    // ✅ düzeltildi
                                clone0.LuckCompose = value3.LuckCompose;       // ✅ düzeltildi

                                client.Player.AddTemplate(clone0, clone0.Template.BagType, 1, eGameView.OtherTypeGet, name);
                            }
                        }
                        else
                        {
                            // Stack olabilen itemler direkt ekle
                            client.Player.AddTemplate(value3, value3.Template.BagType, value3.Count, eGameView.OtherTypeGet, name);
                        }
                    }

                    client.Player.SendTCP(gSPacketIn);
                }
                else
                {
                    // Sadece para/exp/vb. varsa mesajı yine de gönder
                    if (num2 > 0)
                    {
                        if (messageBuilder.Length > 0 && messageBuilder[messageBuilder.Length - 1] == ',')
                            messageBuilder.Remove(messageBuilder.Length - 1, 1);
                        messageBuilder.Append(".");
                        client.Out.SendMessage(eMessageType.GM_NOTICE, str + messageBuilder.ToString());
                    }
                }
            }
            return 1;
        }
    }
}
