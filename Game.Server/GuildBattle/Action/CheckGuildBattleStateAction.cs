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
    public class CheckGuildBattleStateAction : IGuildBattleAction
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private long m_tick;
        private bool m_isFinished;
        public CheckGuildBattleStateAction(int delay)
        {
            m_isFinished = false;
            m_tick += TickHelper.GetTickCount() + delay;
        }

        public void Execute(GuildBattleMgr game, long tick)
        {
            if (m_tick <= tick)
            {
                //Console.WriteLine("//////tick CheckGuildBattleStateAction: " + game.State);

                DateTime now = DateTime.Now;
                switch(game.State)
                {
                    case GuildBattleState.CLOSE:
                        {
                            if(now.DayOfWeek == game.DayStart)
                                game.State = GuildBattleState.CHECKING;
                        }
                        break;

                    case GuildBattleState.CHECKING:
                        {
                            if (now.DayOfWeek != game.DayStart)
                            {
                                game.ChangeOpenClose(false);
                            }
                            else
                            {
                                if (game.IsSendMail == false && now.Hour >= 17 && now.Hour < 18 && !game.CheckAction(typeof(SendMailNoticeAction)))
                                {
                                    game.AddAction(new SendMailNoticeAction(1000));
                                }

                                if (game.CanStartGame(now))
                                {
                                    game.ChangeOpenClose(true, DateTime.Now.AddHours(1));
                                }
                            }
                        }
                        break;

                    case GuildBattleState.OPEN:

                        if(game.TimeStop <= now)
                        {
                            // stop game
                            game.ChangeOpenClose(false);

                            if (!game.CheckAction(typeof(SendAwardAction)))
                                game.AddAction(new SendAwardAction(1000));

                        }

                        break;
                }
                game.WaitTime(1000);
                m_isFinished = true;
            }
        }

        public bool IsFinished(long tick)
        {
            return m_isFinished;
        }
    }
}
