using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Server.Packets;
using Game.Server.GameObjects;
using Game.Server.Packets.Client;
using Game.Server.Managers;
using Game.Base.Packets;
using SqlDataProvider.Data;
using Bussiness;

namespace Game.Server.ActiveSystem.Handle
{
    [ActiveSystemHandleAttbute((byte)ActiveSystemPackageType.LANTERNRIDDLES_ANSWER)]
    public class LanternriddlesAnswer : IActiveSystemCommandHadler
    {
        public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.ACTIVITY_SYSTEM, Player.PlayerCharacter.ID);

            packet.ReadInt();//_loc_4.writeInt(param1);QuestionID
            packet.ReadInt();//_loc_4.writeInt(param2);QuestionIndex
            int option = packet.ReadInt();//_loc_4.writeInt(param3);Option            
            //ActiveSystemMgr.LanternriddlesAnswer(Player.PlayerCharacter.ID, option);
            return true;
			
        }		
    }
}
