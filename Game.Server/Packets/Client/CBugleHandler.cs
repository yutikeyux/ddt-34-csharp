using Bussiness;
using Game.Base.Packets;
using Game.Server.Api;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Packets.Client
{
    [PacketHandler(73, "大喇叭")]
    public class CBugleHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int templateId = 11100;
            int clientId = packet.ReadInt();
            ItemInfo itemByTemplateID = client.Player.PropBag.GetItemByTemplateID(0, templateId);

            // Soğuma süresi kontrolü
            if (DateTime.Compare(client.Player.LastChatTime.AddSeconds(0.0), DateTime.Now) > 0)
            {
                _ = client.Out.SendMessage(eMessageType.ChatERROR, LanguageMgr.GetTranslation("Yavaşla!"));
                return 1;
            }

            GSPacketIn gSPacketIn = new(73, clientId);

            if (itemByTemplateID != null)
            {
                _ = packet.ReadString(); // İlk okuma (genelde boş veya format bilgisi)
                string str = packet.ReadString();

                if (!string.IsNullOrWhiteSpace(str) && !str.StartsWith("!"))
                {
                    // Python Chat Bridge gönderimi
                    PythonChatBridge.Send(
                        client.Player.PlayerCharacter.NickName,
                        client.Player.PlayerCharacter.Grade,
                        $"[Bugle] {str}",
                        0,
                        null
                    );
                }

                // Eşyayı envanterden düş
                _ = client.Player.PropBag.RemoveCountFromStack(itemByTemplateID, 1);

                // Paket hazırlama
                gSPacketIn.WriteInt(client.Player.ZoneId);
                gSPacketIn.WriteInt(client.Player.PlayerCharacter.ID);
                gSPacketIn.WriteString(client.Player.PlayerCharacter.NickName);
                gSPacketIn.WriteString(str);
                gSPacketIn.WriteString(client.Player.ZoneName);

                // Login Server'a gönder
                GameServer.Instance.LoginServer.SendPacket(gSPacketIn);

                // Diğer Login Serverlara gönder
                foreach (var item in GameServer.Instance.OtherLoginServer)
                {
                    if (item.IsConnected)
                    {
                        GSPacketIn gSPacketIn2 = new(72);
                        gSPacketIn2.WriteInt(itemByTemplateID.Template.Property2);
                        gSPacketIn2.WriteInt(client.Player.PlayerCharacter.ID);
                        gSPacketIn2.WriteString(client.Player.PlayerCharacter.NickName);
                        gSPacketIn2.WriteString(str);
                        item.SendPacket(gSPacketIn2);
                        item.SendPacket(gSPacketIn);
                    }
                }

                // Soğuma süresini güncelle
                client.Player.LastChatTime = DateTime.Now;

                // --- ÖNEMLİ DÜZELTME BURADA ---
                // Görev/Event güncellemesi HERKES İÇİN DEĞİL, SADECE KULLANAN KİŞİ İÇİN 1 KEZ YAPILMALIDIR.
                // Bu kodları 'foreach' döngüsünden çıkarıyoruz.
                EventRewardProcessInfo info = client.Player.Extra.GetEventProcess((int)NoviceActiveType.DISCORD_HOPARLORU);
                client.Player.Extra.UpdateEventCondition((int)NoviceActiveType.DISCORD_HOPARLORU, info.Conditions + 1);

                // Tüm oyunculara mesajı gönder
                GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
                foreach (GamePlayer gamePlayer in allPlayers)
                {
                    gSPacketIn.ClientID = gamePlayer.PlayerCharacter.ID;
                    gamePlayer.Out.SendTCP(gSPacketIn);
                }
            }
            return 0;
        }
    }
}