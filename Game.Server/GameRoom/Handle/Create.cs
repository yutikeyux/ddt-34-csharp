using Bussiness;
using Game.Base.Packets;
using Game.Logic;
using Game.Server.Buffer;
using Game.Server.Packets;
using Game.Server.Rooms;
using System;

namespace Game.Server.GameRoom.Handle
{
    [GameRoomHandleAttbute((byte)GameRoomPackageType.GAME_ROOM_CREATE)]
    public class Create : IGameRoomCommandHadler
    {
        public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
        {
            byte roomType = packet.ReadByte();
            byte timeType = packet.ReadByte();
            string name = packet.ReadString();
            string password = packet.ReadString();

            if ((eRoomType)roomType == eRoomType.WordBossFight)
            {
                if (!RoomMgr.WorldBossRoom.WorldOpen || RoomMgr.WorldBossRoom.Blood <= 0)
                {
                    Player.CurrentRoom.RemovePlayerUnsafe(Player);
                    return false;
                }

                Player.LastEnterWorldBoss = DateTime.Now;
                Player.WorldbossBood = RoomMgr.WorldBossRoom.Blood;

                AbstractBuffer buffer = BufferList.CreatePayBuffer((int)BuffType.WorldBossHP, 50000, 1);
                if (buffer != null)
                {
                    buffer.Start(Player);
                }

                buffer = BufferList.CreatePayBuffer((int)BuffType.WorldBossAddDamage, 30000, 1);
                buffer?.Start(Player);
            }

            if (Player.MainWeapon == null)
            {
                Player.Out.SendMessage(eMessageType.BIGBUGLE_NOTICE, "Silah yok, katılım yok.");
                return false;
            }

            RoomMgr.CreateRoom(Player, name, password, (eRoomType)roomType, timeType);
            return true;
        }
    }
}