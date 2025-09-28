using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Server.Packets;
using Game.Server.GameObjects;
using Game.Server.Packets.Client;
using Game.Base.Packets;
using Game.Server.Rooms;
using Game.Logic;
using Game.Server.Buffer;
using SqlDataProvider.Data;
using Bussiness;

namespace Game.Server.Farm.Handle
{
    [FarmHandleAttbute((byte)FarmPackageType.GAIN_FIELD)]
    public class GainFields : IFarmCommandHadler
    {
        public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
        {
            int userId = packet.ReadInt();
            int fieldId = packet.ReadInt();//param1.fieldID
            string msg = LanguageMgr.GetTranslation("EnterFarmHandler.Msg2");
            if (userId == Player.PlayerCharacter.ID && Player.Farm.GainField(fieldId))
            {
                msg = LanguageMgr.GetTranslation("EnterFarmHandler.Msg3");
            }
            else if (userId != Player.PlayerCharacter.ID)
            {
                if (Player.Farm.GainFriendFields(userId, fieldId))
                {
                    msg = LanguageMgr.GetTranslation("EnterFarmHandler.Msg4");
                }
                else
                {
                    msg = LanguageMgr.GetTranslation("EnterFarmHandler.Msg5");
                }
            }
            Player.SendMessage(msg);
            return true;
        }
    }
}