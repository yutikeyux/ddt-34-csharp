using Game.Logic;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Game.Server.GuildBattle.Action
{
    public class ReloadRankAction : IGuildBattleAction
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private long m_tick;
        private bool m_isFinished;
        public ReloadRankAction(int delay)
        {
            m_isFinished = false;
            m_tick += TickHelper.GetTickCount() + delay;
        }
        public void Execute(GuildBattleMgr game, long tick)
        {
            int rank = 1;

            GuildBattleConsortiaInfo[] listCor = game.GetAllConsortia().OrderByDescending(a => a.Score).ToArray();
            foreach(GuildBattleConsortiaInfo cor in listCor)
            {
                cor.Rank = rank;
                rank++;
            }

            m_isFinished = true;
        }
        public bool IsFinished(long tick)
        {
            return m_isFinished;
        }
    }
}
