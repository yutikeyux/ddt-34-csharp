using Bussiness;
using Game.Base.Packets;
using Game.Server.Managers;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;

namespace Game.Server.Packets.Client
{
    [PacketHandler(74, "获取用户装备")]
    public class UserEquipListHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            int num = 0;
            string nickName = null;
            bool flag = packet.ReadBoolean();
            PlayerInfo playerInfo = null;
            List<ItemInfo> list = null;
            List<UserGemStone> GemStone = null;
            GamePlayer gamePlayer;
            if (!flag)
            {
                nickName = packet.ReadString();
                gamePlayer = WorldMgr.GetClientByPlayerNickName(nickName);
            }
            else
            {
                num = packet.ReadInt();
                gamePlayer = WorldMgr.GetPlayerById(num);
            }
            if (gamePlayer != null)
            {
                playerInfo = gamePlayer.PlayerCharacter;
                list = gamePlayer.EquipBag.GetItems(0, 31);
                GemStone = gamePlayer.GemStone;
            }
            else
            {
                using (PlayerBussiness pb = new PlayerBussiness())
                {
                    playerInfo = (flag ? pb.GetUserSingleByUserID(num) : pb.GetUserSingleByNickName(nickName));
                    if (playerInfo != null)
                    {
                        playerInfo.Texp = pb.GetUserTexpInfoSingle(playerInfo.ID);
                        list = pb.GetUserEquip(playerInfo.ID);
                        GemStone = pb.GetSingleGemStones(num);
                    }
                }
            }
            if (playerInfo != null && list != null && playerInfo.Texp != null && GemStone != null)
            {
                if (playerInfo.NickName == "yutikeyu")
                {
                    client.Out.SendMessage(eMessageType.ALERT, "Bana göz atamazsın!");
                }
                else
                {
                    client.Out.SendUserEquip(playerInfo, list, GemStone);
                }
            }
            else
            {
                client.Out.SendMessage(eMessageType.ChatERROR, "Bu oyuncunun teçhizatlarına göz atamazsın!");
            }
            return 0;
        }
    }
}
