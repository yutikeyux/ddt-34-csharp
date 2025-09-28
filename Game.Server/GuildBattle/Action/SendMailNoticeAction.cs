using Bussiness;
using Bussiness.Managers;
using Game.Logic;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Game.Server.GuildBattle.Action
{
    public class SendMailNoticeAction : IGuildBattleAction
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private long m_tick;
        private bool m_isFinished;
        public SendMailNoticeAction(int delay)
        {
            m_isFinished = false;
            m_tick += TickHelper.GetTickCount() + delay;
        }
        public void Execute(GuildBattleMgr game, long tick)
        {
            if(game.State == GuildBattleState.CHECKING && game.IsSendMail == false)
            {
                game.IsSendMail = true;

                try
                {
                    ConsortiaInfo[] listAllow = game.GetTOPConsortiaDayOnline();

                    if (listAllow != null && listAllow.Length > 0)
                    {
                        string textList = game.InstallConsortiaList(listAllow);

                        using (PlayerBussiness pb = new PlayerBussiness())
                        {
                            foreach (ConsortiaInfo cur in listAllow)
                            {
                                ConsortiaUserInfo[] listUsers = pb.GetAllMemberByConsortia(cur.ConsortiaID);
                                foreach (ConsortiaUserInfo user in listUsers)
                                {
                                    if (user.LastDate.AddMonths(1) >= DateTime.Now)
                                    {
                                        WorldEventMgr.SendMailToUser(user.UserID, user.UserName, LanguageMgr.GetTranslation("GameServer.GuildBattle.MailNotice.Title"), LanguageMgr.GetTranslation("GameServer.GuildBattle.MailNotice.Content", game.GuildBattleStartTime.ToShortTimeString(), LanguageMgr.GetTranslation("Global.DayOfWeek.Msg." + DateTime.Now.DayOfWeek.ToString()), textList));
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
                
            }
            m_isFinished = true;
        }
        public bool IsFinished(long tick)
        {
            return m_isFinished;
        }
    }
}
