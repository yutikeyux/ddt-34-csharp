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
using Game.Logic;

namespace Game.Server.ActiveSystem.Handle
{
    [ActiveSystemHandleAttbute((byte)ActiveSystemPackageType.FIGHT_MONSTER)]
    public class ChristmasFightMonster : IActiveSystemCommandHadler
    {
        public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.ACTIVITY_SYSTEM, Player.PlayerCharacter.ID);
            BaseChristmasRoom christmasRoom = RoomMgr.ChristmasRoom;
            int monterId = packet.ReadInt();
            if (Player.MainWeapon == null)
            {
                Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.SceneGames.NoEquip"));
                return false;
            }
            if (!Player.Actives.AvailTime())
            {
                Player.SendMessage(LanguageMgr.GetTranslation("ActiveSystemHandler.Msg1"));
                //christmas.RemovePlayer(client.Player);
                return false;
            }
            if (christmasRoom.SetFightMonter(monterId, Player.PlayerCharacter.ID))
            {
                MonterInfo monter = christmasRoom.Monters[monterId];
                pkg.WriteByte((byte)ActiveSystemPackageType.CHRISTMAS_MONSTER);
                pkg.WriteByte(3);//type3                        
                pkg.WriteInt(monterId);//_loc_16 = _loc_3.readInt();hasKey
                pkg.WriteInt(monter.state);//_loc_17 = _loc_3.readInt();State                       
                christmasRoom.SendToALL(pkg);
                RoomMgr.CreateRoom(Player, "Christmas", "Cfcs151df166s", eRoomType.Christmas, 3);
            }
            return true;
			
        }		
    }
}
