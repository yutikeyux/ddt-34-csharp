using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Packets.Client
{
    [PacketHandler(134, "GunnyTurkey")]
    public class DiceHandler : IPacketHandler
    {
        public int maxTime
        {
            get => 3500;

            set => throw new NotImplementedException();
        }
        private readonly ThreadSafeRandom threadSafeRandom = new();
        private readonly int count;
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            byte b = packet.ReadByte();
            _ = client.Player.PlayerCharacter.ID;
            int result;
            switch (b)
            {
                case 10:
                    client.Player.Dice.ReceiveData();
                    _ = client.Player.Out.SendDiceReceiveData(client.Player.Dice);
                    break;
                case 11:
                    {
                        if (client.Player.Dice.Data.LuckIntegral >= client.Player.Dice.IntegralPoint[client.Player.Dice.MAX_LEVEL - 1])
                        {
                            client.Player.SendMessage("En yüksek seviyeye ulaştınız ! Zengin adam yenileniyor ");
                            client.Player.DiceReset();
                        }
                        if (client.Player.PlayerCharacter.myScore == 16)
                        {
                            _ = client.Player.Out.SendMessage(eMessageType.ALERT, "Total zar atma liminite ulaştın. Sistem otomatik olarak güncelliyor !");
                            client.Player.Dice.CreateDiceAward();
                            _ = client.Player.Out.SendDiceReceiveData(client.Player.Dice);
                            client.Player.PlayerCharacter.myScore = 0;
                        }
                        int num = packet.ReadInt();
                        _ = packet.ReadInt();
                        int index;
                        int value;
                        switch (num)
                        {
                            case 1:
                                index = threadSafeRandom.Next(2, 13);
                                value = client.Player.Dice.doubleDicePrice;
                                break;
                            case 2:
                                index = threadSafeRandom.Next(4, 7);
                                value = client.Player.Dice.bigDicePrice;
                                break;
                            case 3:
                                index = threadSafeRandom.Next(1, 4);
                                value = client.Player.Dice.smallDicePrice;
                                break;
                            default:
                                index = threadSafeRandom.Next(1, 7);
                                value = client.Player.Dice.commonDicePrice;
                                break;
                        }
                        if (client.Player.Dice.Data.FreeCount > 0)
                        {
                            DiceDataInfo data = client.Player.Dice.Data;
                            data.FreeCount--;
                            receiveResult(client.Player, index);
                        }
                        else if (client.Player.MoneyDirect(value))
                        {
                            receiveResult(client.Player, index);
                        }
                        client.Player.PlayerCharacter.myScore++;
                        break;
                    }
                case 12:
                    {
                        int refreshPrice = client.Player.Dice.refreshPrice;
                        if (client.Player.MoneyDirect(refreshPrice))
                        {
                            client.Player.Dice.CreateDiceAward();
                            _ = client.Player.Out.SendDiceReceiveData(client.Player.Dice);
                        }
                        break;
                    }
            }
            result = 0;
            return result;
        }

        private void receiveResult(GamePlayer player, int index)
        {
            GSPacketIn gSPacketIn = new(134);
            gSPacketIn.WriteByte(4);
            gSPacketIn.WriteInt(player.Dice.Data.CurrentPosition);
            gSPacketIn.WriteInt(index);
            DiceDataInfo data = player.Dice.Data;
            data.CurrentPosition += index;
            if (player.Dice.Data.CurrentPosition > 18)
            {
                DiceDataInfo data2 = player.Dice.Data;
                data2.CurrentPosition -= 19;
            }
            EventAwardInfo eventAwardInfo = player.Dice.RewardItem[player.Dice.Data.CurrentPosition];
            ItemInfo itemInfo = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(eventAwardInfo.TemplateID), eventAwardInfo.Count, 103);
            itemInfo.IsBinds = false;
            itemInfo.ValidDate = eventAwardInfo.ValidDate;
            if (!player.AddTemplate(itemInfo, "Zengin Adam"))
            {
                _ = player.SendItemToMail(itemInfo, itemInfo.Template.Name, "Evanter dolu !", eMailType.OpenUpArk);
            }
            player.Dice.RewardName = itemInfo.Template.Name;
            int num = threadSafeRandom.Next(2, 13);
            DiceDataInfo data3 = player.Dice.Data;
            data3.LuckIntegral += num;
            int luckIntegralLevel = player.Dice.Data.LuckIntegralLevel;
            if (player.Dice.Data.LuckIntegral >= player.Dice.IntegralPoint[luckIntegralLevel + 1])
            {
                DiceDataInfo data4 = player.Dice.Data;
                data4.LuckIntegralLevel++;
                PlayerInfo playerCharacter = player.PlayerCharacter;
                playerCharacter.luckyNum++;
                player.Dice.GetLevelAward();
            }
            if (player.Dice.Data.LuckIntegralLevel > 3)
            {
                player.Dice.Data.LuckIntegralLevel = 3;
                player.Dice.Data.LuckIntegral = player.Dice.IntegralPoint[player.Dice.MAX_LEVEL - 1];
            }
            gSPacketIn.WriteInt(player.Dice.Data.LuckIntegral);
            gSPacketIn.WriteInt(player.Dice.Data.LuckIntegralLevel);
            gSPacketIn.WriteInt(player.Dice.Data.FreeCount);
            gSPacketIn.WriteString(player.Dice.RewardName);
            player.Out.SendTCP(gSPacketIn);
        }
    }
}
