using Bussiness;
using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Packets.Client
{
    [PacketHandler(72, "大喇叭")]
    public class BigBugleHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int num = packet.ReadInt();
            ItemInfo itemByTemplateID = client.Player.PropBag.GetItemByTemplateID(0, num);
            if (DateTime.Compare(client.Player.LastChatTime.AddSeconds(2.0), DateTime.Now) > 0)
            {
                _ = client.Out.SendMessage(eMessageType.ChatERROR, LanguageMgr.GetTranslation("2 saniye sonra büyük hoparlör mesajı gönderilebilir!"));
                return 1;
            }
            GSPacketIn gSPacketIn = new(72);
            if (itemByTemplateID != null)
            {
                _ = packet.ReadInt();
                _ = packet.ReadString();
                string str = packet.ReadString();
                _ = client.Player.PropBag.RemoveCountFromStack(itemByTemplateID, 1);
                gSPacketIn.WriteInt(itemByTemplateID.Template.Property2);
                gSPacketIn.WriteInt(client.Player.PlayerCharacter.ID);
                gSPacketIn.WriteString(client.Player.PlayerCharacter.NickName);
                gSPacketIn.WriteString(str);
                GameServer.Instance.LoginServer.SendPacket(gSPacketIn);
                client.Player.LastChatTime = DateTime.Now;
                GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
                GamePlayer[] array = allPlayers;
                foreach (GamePlayer gamePlayer in array)
                {
                    gSPacketIn.ClientID = gamePlayer.PlayerCharacter.ID;
                    gamePlayer.Out.SendTCP(gSPacketIn);
                }
            }
            else
            {
                _ = packet.ReadString();
                string str2 = packet.ReadString();
                ItemInfo itemByCategoryID = client.Player.PropBag.GetItemByCategoryID(0, 11, 4);
                _ = client.Player.PropBag.RemoveCountFromStack(itemByCategoryID, 1);
                gSPacketIn.WriteInt(client.Player.ZoneId);
                gSPacketIn.WriteInt(client.Player.PlayerCharacter.ID);
                gSPacketIn.WriteString(client.Player.PlayerCharacter.NickName);
                gSPacketIn.WriteString(str2);
                gSPacketIn.WriteString(client.Player.ZoneName);
                GameServer.Instance.LoginServer.SendPacket(gSPacketIn);
                client.Player.LastChatTime = DateTime.Now;
                GamePlayer[] allPlayers2 = WorldMgr.GetAllPlayers();
                GamePlayer[] array2 = allPlayers2;
                foreach (GamePlayer gamePlayer2 in array2)
                {
                    gSPacketIn.ClientID = gamePlayer2.PlayerCharacter.ID;
                    gamePlayer2.Out.SendTCP(gSPacketIn);
                }
            }
            client.Player.OnUsingItem(num, 1);
            return 0;
        }
    }
}
