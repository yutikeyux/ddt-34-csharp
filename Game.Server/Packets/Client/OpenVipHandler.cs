using Bussiness;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Packets.Client
{
    [PacketHandler(92, "场景用户离开")]
    public class OpenVipHandler : IPacketHandler
    {
        private int TotalPrice(int renewal)
        {
            ShopItemInfo itemVipInfo = ShopMgr.FindShopbyTemplateID((int)EquipType.VIPCARD);
            if (itemVipInfo == null)
            {
                return -1;
            }

            int result;
            if (renewal == itemVipInfo.AUnit)
            {
                result = itemVipInfo.AValue1;
            }
            else
                if (renewal == itemVipInfo.BUnit)
                {
                    result = itemVipInfo.BValue1;
                }
                else
                {
                    result = renewal == itemVipInfo.CUnit
                        ? itemVipInfo.CValue1
                        : (int)Math.Ceiling(itemVipInfo.AValue1 * (float)renewal / itemVipInfo.AUnit);
                }

            return result;
        }
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            string nickname = packet.ReadString();
            int renewval_days = packet.ReadInt();
            int money = TotalPrice(renewval_days);
            string msg = "VIP Aktivasyonu başarılı!";

            GamePlayer player = WorldMgr.GetClientByPlayerNickName(nickname);
            DailyRecordInfo dailyRecord = new()
            {
                UserID = client.Player.PlayerCharacter.ID,
                Type = 6,
                Value = "VIP"
            };
            if (client.Player.MoneyDirect(money, false, false, false))
            {
                DateTime now = DateTime.Now;
                int typeVIP = client.Player.SetTypeVIP(renewval_days);
                using PlayerBussiness playerBussiness = new();

                _ = playerBussiness.VIPRenewal(nickname, renewval_days, typeVIP, ref now);
                if (player == null)
                {
                    msg = "Oyuncu" + nickname + " bulunamadı!";
                }
                else
                {
                    if (client.Player.PlayerCharacter.NickName == nickname)
                    {
                        if (client.Player.PlayerCharacter.VIPLevel == 12)
                        {
                            msg = "Tebriklerr!! Maksimum VIP seviyesine ulaştınız!";
                            client.Player.SendMessage(msg);
                            return 0;
                        }
                        else
                        {
                            if (client.Player.PlayerCharacter.typeVIP == 0)
                            {
                                client.Player.OpenVIP(renewval_days, now);
                            }
                            else
                            {
                                client.Player.ContinuousVIP(renewval_days, now);
                                msg = "VIP süresi başarıyla devam ettirildi!";
                            }
                        }
                        client.Player.AddExpVip(money);
                        if (client.Player.PlayerCharacter.typeVIP > 0)
                        {
                            client.Player.PlayerCharacter.VIPNextLevelDaysNeeded = client.Player.GetVIPNextLevelDaysNeeded(client.Player.PlayerCharacter.VIPLevel, client.Player.PlayerCharacter.VIPExp);
                        }

                        _ = client.Out.SendOpenVIP(client.Player);
                    }
                    else
                    {
                        string message2;
                        if (player.PlayerCharacter.typeVIP == 0)
                        {
                            player.OpenVIP(renewval_days, now);
                            msg = "Oyuncu:" + nickname + " için VIP etkinleştirme başarılı!!";
                            message2 = client.Player.PlayerCharacter.NickName + ", Tebrikler!";
                        }
                        else
                        {
                            player.ContinuousVIP(renewval_days, now);
                            msg = "Oyuncu:" + nickname + " için VIP süresi yenileme başlarılı!";
                            message2 = "Arkadaşın " + client.Player.PlayerCharacter.NickName + ", sana VIP hediyesinde bulundu!";
                        }
                        player.AddExpVip(money);
                        if (player.PlayerCharacter.typeVIP > 0)
                        {
                            player.PlayerCharacter.VIPNextLevelDaysNeeded = player.GetVIPNextLevelDaysNeeded(player.PlayerCharacter.VIPLevel, player.PlayerCharacter.VIPExp);
                        }

                        _ = player.Out.SendOpenVIP(player);
                        _ = player.Out.SendMessage(eMessageType.Normal, message2);
                    }
                }
                _ = client.Out.SendMessage(eMessageType.Normal, msg);
            }
            return 0;
        }
    }
}
