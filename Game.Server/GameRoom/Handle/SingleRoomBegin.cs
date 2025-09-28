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
using Game.Server.Managers;

namespace Game.Server.GameRoom.Handle
{
    [GameRoomHandleAttbute((byte)GameRoomPackageType.SINGLE_ROOM_BEGIN)]
    public class SingleRoomBegin : IGameRoomCommandHadler
    {
        public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
        {
            int roomType = packet.ReadInt();
            if (Player.MainWeapon == null)
            {
                Player.SendMessage(LanguageMgr.GetTranslation("Game.Server.SceneGames.NoEquip"));
                return false;
            }
            switch (roomType)
            {
                case 4:
                    {
                        //if (Player.CanActive("guildbattle"))
                        //{
                            if (Player.CurrentRoom != null)
                            {
                                Player.CurrentRoom.RemovePlayerUnsafe(Player);
                            }
                            if (!Player.IsActive)
                                return false;

                            RoomMgr.WaitingRoom.RemovePlayer(Player);
                            RoomMgr.CreateConsortiaBattleRoom(Player);
                        //}
                        break;
                    }
                default:
                    Console.WriteLine("SINGLE_ROOM_BEGIN: {0}", roomType);
                    break;
            }

            return true;
        }
    }
}
