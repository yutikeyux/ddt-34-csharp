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
    public class SendAwardAction : IGuildBattleAction
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private long m_tick;
        private bool m_isFinished;
        public SendAwardAction(int delay)
        {
            m_isFinished = false;
            m_tick += TickHelper.GetTickCount() + delay;
        }
        public void Execute(GuildBattleMgr game, long tick)
        {
            if(game.State == GuildBattleState.CLOSE && game.IsSendAward == false)
            {
                game.IsSendAward = true;

                List<ItemInfo> items = null;
                string content = "";

                GuildBattleConsortiaInfo[] corList = game.GetAllConsortia().OrderByDescending(a => a.Score).ToArray();
                UserGuildBattleInfo[] userList = game.GetAllUser().ToArray();

                int rank = 1, rankPerson = 1;
                foreach(GuildBattleConsortiaInfo cor in corList)
                {
                    // send award guild
                    UserGuildBattleInfo[] ulist = userList.Where(a => a.ConsortiaID == cor.ConsortiaID).OrderByDescending(a => a.Score).ToArray();

                    rankPerson = 1;
                    foreach (UserGuildBattleInfo u in ulist)
                    {
                        switch(rank)
                        {
                            case 1:
                                items = EventAwardMgr.GetEventAwardByType(eEventType.GUILD_BATTLE_TOP_1);
                                content = LanguageMgr.GetTranslation("GameServer.GuildBattle.MailAward.Content", rank);
                                break;

                            case 2:
                                items = EventAwardMgr.GetEventAwardByType(eEventType.GUILD_BATTLE_TOP_2);
                                content = LanguageMgr.GetTranslation("GameServer.GuildBattle.MailAward.Content", rank);
                                break;

                            case 3:
                                items = EventAwardMgr.GetEventAwardByType(eEventType.GUILD_BATTLE_TOP_3);
                                content = LanguageMgr.GetTranslation("GameServer.GuildBattle.MailAward.Content", rank);
                                break;

                            default:
                                items = EventAwardMgr.GetEventAwardByType(eEventType.GUILD_BATTLE_TOP_4);
                                content = LanguageMgr.GetTranslation("GameServer.GuildBattle.MailAward.Content", rank);
                                break;
                        }

                        if (items != null && items.Count > 0)
                        {
                            WorldEventMgr.SendItemsToMail(items, u.UserID, u.NickName, LanguageMgr.GetTranslation("GameServer.GuildBattle.MailAward.Title"), content);
                        }

                        if(rankPerson <= 20)
                        {
                            items = EventAwardMgr.GetEventAwardByType(eEventType.GUILD_BATTLE_PERSON_TOP);

                            if (items != null && items.Count > 0)
                                WorldEventMgr.SendItemsToMail(items, u.UserID, u.NickName, LanguageMgr.GetTranslation("GameServer.GuildBattle.MailAwardPerson.Title"), LanguageMgr.GetTranslation("GameServer.GuildBattle.MailAwardPerson.Content"));
                        }

                        rankPerson++;
                    }

                    rank++;
                }

                game.SaveCurrentRankToDatabase(true);
            }
            m_isFinished = true;
        }
        public bool IsFinished(long tick)
        {
            return m_isFinished;
        }
    }
}
