using Game.Server.RingStation.RoomGamePkg.TankHandle;
using System;

namespace Game.Server.Rooms
{
    // Token: 0x02000033 RID: 51
    public class FakeRoomAction : IAction
    {
        // Token: 0x06000198 RID: 408 RVA: 0x00002A40 File Offset: 0x00000C40
        public FakeRoomAction(string roomName, int playerCount, int maxPlayerCount, int roomType)
        {
            this.roomName = roomName;
            this.playerCount = playerCount;
            this.maxPlayerCount = maxPlayerCount;
            this.roomType = roomType;
        }

        // Token: 0x06000199 RID: 409 RVA: 0x00010D5C File Offset: 0x0000EF5C
        public void Execute()
        {
            BaseRoom[] rooms = RoomMgr.Rooms;
            FakeRoom fakeRoom = null;
            for (int i = 0; i < rooms.Length; i++)
            {
                bool flag = !rooms[i].IsUsing;
                if (flag)
                {
                    fakeRoom = (FakeRoom)(rooms[i] = new FakeRoom(rooms[i].RoomId, this.roomName, this.playerCount, this.maxPlayerCount, this.roomType));
                    break;
                }
            }
            bool flag2 = fakeRoom != null;
            if (flag2)
            {
                fakeRoom.Start();
                RoomMgr.WaitingRoom.SendUpdateRoom(fakeRoom);
            }
        }

        // Token: 0x040000AF RID: 175
        private string roomName;

        // Token: 0x040000B0 RID: 176
        private int playerCount;

        // Token: 0x040000B1 RID: 177
        private int maxPlayerCount;

        // Token: 0x040000B2 RID: 178
        private int roomType;
    }
}
