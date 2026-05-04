using Game.Base.Packets;
using Game.Server.Managers;
using System;

namespace Game.Server.Packets.Client
{
    [PacketHandler(123, "场景用户离开")]
    public class DefyAfficheHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            // ---------------------------------------------------------
            // YENİ EKLENEN LİMİT KONTROLÜ (5 DAKİKA)
            // Oyuncunun son işlem yapma zamanı ile şu anki zamanı karşılaştırıyoruz.
            // ---------------------------------------------------------
            double minutesPassed = DateTime.Now.Subtract(client.Player.LastChatTime).TotalMinutes;

            if (minutesPassed < 5)
            {
                // Kalan süreyi hesaplayıp oyuncuya bilgi veriyoruz.
                int remainingMinutes = 5 - (int)minutesPassed;
                _ = client.Out.SendMessage(eMessageType.ChatERROR, $"Bu özelliği tekrar kullanmak için {remainingMinutes} dakika beklemelisiniz!");

                // İşlemi burada kesiyoruz, para kontrolüne bile girmiyor.
                return 0;
            }

            // ---------------------------------------------------------
            // MEVCUT İŞLEM MANTIĞI
            // ---------------------------------------------------------
            string str = packet.ReadString();
            int needMoney = 500;

            // Güvenlik güncellemesi: Para ve limit kontrolü
            if (client.Player.MoneyDirect(needMoney, true, false, true))
            {
                // Ödeme başarılıysa mesajı gönder
                GSPacketIn gSPacketIn = new(123);
                gSPacketIn.WriteString(str);
                GameServer.Instance.LoginServer.SendPacket(gSPacketIn);

                // Zaman damgasını ŞİMDİ olarak güncelliyoruz.
                // Bu, bir sonraki kullanımda 5 dakika sayacı başlatır.
                client.Player.LastChatTime = DateTime.Now;

                GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
                foreach (GamePlayer gamePlayer in allPlayers)
                {
                    gSPacketIn.ClientID = gamePlayer.PlayerCharacter.ID;
                    gamePlayer.Out.SendTCP(gSPacketIn);
                }
                client.Player.OnPlayerDispatches();
            }
            else
            {
                // Ödeme başarısız olduysa (Limit dolduysa veya para yoksa)
                // MoneyDirect içinde mesaj gönderildiği için ekstra işlem yapmıyoruz.
            }

            return 0;
        }
    }
}