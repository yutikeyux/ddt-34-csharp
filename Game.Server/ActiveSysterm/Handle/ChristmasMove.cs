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
using Game.Server.Rooms;

namespace Game.Server.ActiveSystem.Handle
{
    [ActiveSystemHandleAttbute((byte)ActiveSystemPackageType.MOVE)]
    public class ChristmasMove : IActiveSystemCommandHadler
    {
        public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.ACTIVITY_SYSTEM, Player.PlayerCharacter.ID);
            BaseChristmasRoom christmasRoom = RoomMgr.ChristmasRoom;
            int vx = packet.ReadInt();
            int vy = packet.ReadInt();
            string str = packet.ReadString();
            Player.X = vx;
            Player.Y = vy;
            pkg.WriteByte((byte)ActiveSystemPackageType.MOVE);
            pkg.WriteInt(Player.PlayerId);
            pkg.WriteInt(vx);
            pkg.WriteInt(vy);
            pkg.WriteString(str);
            christmasRoom.SendToALL(pkg);
            return true;
			
        }		
    }
}
