using Bussiness;
using Game.Base.Packets;
using SqlDataProvider.Data;
using System;

namespace Game.Server.Packets.Client
{
    [PacketHandler(236, "添加征婚信息")]
    public class MarryInfoAddHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            if (client.Player.PlayerCharacter.MarryInfoID != 0)
            {
                return 1;
            }
            bool isPublishEquip = packet.ReadBoolean();
            string introduction = packet.ReadString();
            int iD = client.Player.PlayerCharacter.ID;
            eMessageType type = eMessageType.GM_NOTICE;
            string translateId = "MarryInfoAddHandler.Fail";
            int num = 10000;
            if (num > client.Player.PlayerCharacter.Gold)
            {
                type = eMessageType.BIGBUGLE_NOTICE;
                translateId = "MarryInfoAddHandler.Msg1";
            }
            else
            {
                MarryInfo marryInfo = new()
                {
                    UserID = iD,
                    IsPublishEquip = isPublishEquip,
                    Introduction = introduction,
                    RegistTime = DateTime.Now
                };
                using PlayerBussiness playerBussiness = new();
                if (playerBussiness.AddMarryInfo(marryInfo))
                {
                    _ = client.Player.RemoveGold(num);
                    translateId = "MarryInfoAddHandler.Msg2";
                    client.Player.PlayerCharacter.MarryInfoID = marryInfo.ID;
                    _ = client.Out.SendMarryInfoRefresh(marryInfo, marryInfo.ID, isExist: true);
                }
            }
            _ = client.Out.SendMessage(type, LanguageMgr.GetTranslation(translateId));
            return 0;
        }
    }
}
