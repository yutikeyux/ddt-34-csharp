using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Server.GuildBattle
{
    [Serializable()]
    public class GuildBattleConsortiaInfo
    {
        public int ConsortiaID { get; set; }
        public string ConsortiaName { get; set; }
        public int Rank { get; set; }
        public int Score { get; set; }
        public Point DefaultPoint { get; set; }
    }
}
