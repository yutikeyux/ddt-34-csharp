using System;
using Game.Logic;
using Lsj.Util.Text;
using SqlDataProvider.Data;

namespace Game.Server.Rooms
{
    // Token: 0x02000032 RID: 50
    public class FakeRoom : BaseRoom
    {
        // Token: 0x17000036 RID: 54
        // (get) Token: 0x06000191 RID: 401 RVA: 0x00010B7C File Offset: 0x0000ED7C
        // (set) Token: 0x06000192 RID: 402 RVA: 0x00002A26 File Offset: 0x00000C26
        public  bool IsPlaying
        {
            get
            {
                return this.isPlaying;
            }
            set
            {
            }
        }

        // Token: 0x06000193 RID: 403 RVA: 0x00010B94 File Offset: 0x0000ED94
        public FakeRoom(int roomId, string roomName, int playerCount, int maxPlayerCount, int roomType) : base(roomId)
        {
            this.m_playerCount = playerCount;
            this.m_placesCount = maxPlayerCount;
            base.Name = roomName;
            switch (roomType)
            {
                case 1:
                    base.RoomType = eRoomType.Match;
                    base.Password = Guid.NewGuid().ToString();
                    break;
                case 2:
                    base.RoomType = eRoomType.Match;
                    this.isPlaying = true;
                    break;
                case 3:
                    base.RoomType = eRoomType.Dungeon;
                    base.Password = Guid.NewGuid().ToString();
                    this.pveinfo = PveInfoMgr.GetRandomPve();
                    base.MapId = this.pveinfo.ID;
                    base.HardLevel = this.GetRandomHardLevel();
                    break;
                case 4:
                    base.RoomType = eRoomType.Dungeon;
                    this.pveinfo = PveInfoMgr.GetRandomPve();
                    base.MapId = this.pveinfo.ID;
                    base.HardLevel = this.GetRandomHardLevel();
                    this.isPlaying = true;
                    break;
            }
        }

        // Token: 0x06000194 RID: 404 RVA: 0x00010CB0 File Offset: 0x0000EEB0
        private eHardLevel GetRandomHardLevel()
        {
            for (; ; )
            {
                int num = FakeRoom.random.Next(0, 3);
                bool flag = num == 0 && !StringHelper.IsNullOrEmpty(this.pveinfo.SimpleGameScript);
                if (flag)
                {
                    break;
                }
                bool flag2 = num == 1 && !StringHelper.IsNullOrEmpty(this.pveinfo.NormalGameScript);
                if (flag2)
                {
                    goto Block_3;
                }
                bool flag3 = num == 2 && !StringHelper.IsNullOrEmpty(this.pveinfo.HardGameScript);
                if (flag3)
                {
                    goto Block_5;
                }
                if (num == 3 && !StringHelper.IsNullOrEmpty(this.pveinfo.TerrorGameScript))
                {
                    goto Block_7;
                }
            }
            return eHardLevel.Simple;
        Block_3:
            return eHardLevel.Normal;
        Block_5:
            return eHardLevel.Hard;
        Block_7:
            return eHardLevel.Terror;
        }

        // Token: 0x06000195 RID: 405 RVA: 0x00002A29 File Offset: 0x00000C29
        protected void Reset()
        {
        }

        // Token: 0x06000196 RID: 406 RVA: 0x00002A2C File Offset: 0x00000C2C
        public void Stop()
        {
        }

        // Token: 0x040000AC RID: 172
        private static Random random = new Random(Environment.TickCount);

        // Token: 0x040000AD RID: 173
        private bool isPlaying = false;

        // Token: 0x040000AE RID: 174
        private PveInfo pveinfo;
    }
}
