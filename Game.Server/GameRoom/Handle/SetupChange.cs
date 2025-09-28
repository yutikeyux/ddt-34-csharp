using Game.Base.Packets;
using Game.Logic;
using Game.Server.Rooms;
using SqlDataProvider.Data;

namespace Game.Server.GameRoom.Handle
{
    [GameRoomHandleAttbute(2)]
    public class SetupChange : IGameRoomCommandHadler
    {
        public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
        {
            if (Player.CurrentRoom != null && Player == Player.CurrentRoom.Host && !Player.CurrentRoom.IsPlaying)
            {
                int mapId = packet.ReadInt();
                eRoomType roomType = (eRoomType)packet.ReadByte();
                bool isOpenBoss = packet.ReadBoolean();//_loc_12.isOpenBoss.writeBoolean(param3);
                string pic = "";
                string password = packet.ReadString();
                string roomname = packet.ReadString();
                byte timeMode = packet.ReadByte();
                byte hardLevel = packet.ReadByte();
                int levelLimits = packet.ReadInt();
                bool isCrosszone = packet.ReadBoolean();
                int currentFloor = 1;
                if (mapId == 0 && roomType == eRoomType.Labyrinth)
                {
                    mapId = 401;
                    currentFloor = Player.Labyrinth.currentFloor;
                }
                if (roomType == eRoomType.Dungeon && mapId != 10000)
                {
                    PveInfo pve = PveInfoMgr.GetPveInfoById(mapId);
                    string user = Player.PlayerCharacter.UserName;
                    if (pve != null && isOpenBoss)
                    {
                        if (pve.ID >= 15 && user != "khanhlam")
                        {
                            isOpenBoss = false; // mở thì xoá dòng này với dòng dưới và bỏ comment
                            Player.SendMessage("Bu zindan bu özelliğe izin vermiyor");
                        }
                        else if (GameServer.Instance.Configuration.ZoneId == 1001 || GameServer.Instance.Configuration.ZoneId == 1002 || GameServer.Instance.Configuration.ZoneId == 1003)
                        {
                            int price = pve.GetPrice((int)hardLevel);
                            switch (Player.PlayerCharacter.VIPLevel)
                            {
                                case 5:
                                    price = price * (100 - 5) / 100;//Giảm 5%
                                    break;
                                case 6:
                                    price = price * (100 - 10) / 100;//Giảm 10%
                                    break;
                                case 7:
                                    price = price * (100 - 25) / 100;//Giảm 25%
                                    break;
                                case 8:
                                    price = price * (100 - 37) / 100;//Giảm 37%
                                    break;
                                case 9:
                                    price = price * (100 - 48) / 100;//Giảm 48%
                                    break;
                                case 10:
                                    price = price * (100 - 55) / 100;//Giảm 55%
                                    break;
                                case 11:
                                    price = price * (100 - 69) / 100;//Giảm 69%
                                    break;
                                case 12:
                                    price = price * (100 - 88) / 100;//Giảm 88%
                                    break;
                                default:
                                    break;
                            }
                            Player.MoneyDirect(price, IsAntiMult: false, false, true);
                            Player.SendMessage("VIP ne kadar yüksekse, fiyat o kadar düşüktür!");
                            RoomMgr.UpdateRoomGameType(Player.CurrentRoom, roomType, timeMode, (eHardLevel)hardLevel, levelLimits, mapId, password, roomname, isCrosszone, isOpenBoss, pic, currentFloor);
                            return true;
                        }
                        else
                        {
                            isOpenBoss = false; // mở thì xoá dòng này với dòng dưới và bỏ comment
                            Player.SendMessage("Bu özellik geçici olarak kapalıdır, sizden ücret alınmayacaktır");
                        }
                    }
                }
                RoomMgr.UpdateRoomGameType(Player.CurrentRoom, roomType, timeMode, (eHardLevel)hardLevel, levelLimits, mapId, password, roomname, isCrosszone, isOpenBoss, pic, currentFloor);
            }
            return true;
        }
    }
}
