using Game.Base.Packets;
using Game.Server.GameObjects;
using System.Collections.Generic;
using System.Linq;
using Game.Server.Packets;
using System;
using SqlDataProvider.Data;
using Game.Server.Managers;
using System.Drawing;
using Bussiness;

namespace Game.Server.Rooms
{
    public class MonterInfo
    {
        public int ID { set; get; }
        public int state { set; get; }
        public Point MonsterPos { set; get; }
        public Point MonsterNewPos { set; get; }
        public int type { set; get; }
        public int PlayerID { set; get; }
    }

}
