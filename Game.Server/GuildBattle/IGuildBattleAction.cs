using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Server.GuildBattle
{
    public interface IGuildBattleAction
    {
        void Execute(GuildBattleMgr battle, long tick);
        bool IsFinished(long tick);
    }
}
