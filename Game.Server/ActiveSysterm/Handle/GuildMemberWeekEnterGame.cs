using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Server.Packets;
using Game.Server.GameObjects;
using Game.Server.Packets.Client;
using Game.Base.Packets;
using SqlDataProvider.Data;
using Bussiness;

namespace Game.Server.ActiveSystem.Handle
{
    [ActiveSystemHandleAttbute((byte)ActiveSystemPackageType.ENTER_GAME)]
    public class GuildMemberWeekEnterGame : IActiveSystemCommandHadler
    {
        public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.ACTIVITY_SYSTEM, Player.PlayerCharacter.ID);

            pkg.WriteByte((byte)GuildMemberWeekPackageType.PLAYERTOP10);
            pkg.WriteInt(1);//var _loc_4:* = _loc_2.readInt();
            //_loc_6 = _loc_2.readInt();
            //_loc_7 = _loc_2.readUTF();
            //_loc_8 = _loc_2.readInt();
            //_loc_9 = _loc_2.readInt();
            //client.Out.SendTCP(pkg);
            return true;
			
        }		
    }
}
