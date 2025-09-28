using System;
using System.Collections.Generic;
using Bussiness;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
//using Game.Server.GameServerScript.AI;

namespace GameServerScript.AI.Messions
{
    public class HarryGianDuDeS1 : AMissionControl
    {
        private List<SimpleNpc> list_0 = new List<SimpleNpc>();

        private int BossID = 71054;

        private int[] BossPos = new int[2] { 958,692 };

        private int kill = 0;

        private int MapID = 1209;

        //private BossChat BossChatRandom = new BossChat();
        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            int result;
            if (score > 1870)
            {
                result = 3;
            }
            else
            {
                if (score > 1825)
                {
                    result = 2;
                }
                else
                {
                    if (score > 1780)
                    {
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                }
            }
            return result;
        }

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            //base.Game.AddLoadingFile(2, "image/game/effect/11/064.swf", "asset.game.nine.064");
            int[] bossID = { BossID };
            base.Game.LoadResources(bossID);
            base.Game.LoadNpcGameOverResources(bossID);
            base.Game.SetMap(MapID);
        }

        private SimpleBoss m_king;

        private PhysicalObj physicalObj1;

        private PhysicalObj physicalObj2;

        public override void OnStartGame()
        {
            base.OnStartGame();
            m_king = Game.CreateBoss(this.BossID, BossPos[0], BossPos[1], 1, 1, "stand");
            //m_king.Say(LanguageMgr.GetTranslation(BossChatRandom.PlayBossChatRnd(), Array.Empty<object>()), 0, 200, 0);
            m_king.Direction = -1;
            physicalObj1?.PlayMovie("in", 6000, 0);
            physicalObj2?.PlayMovie("in", 6100, 0);
            physicalObj1?.PlayMovie("out", 10000, 1000);
            physicalObj2?.PlayMovie("out", 9900, 0);
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
        }

        public override bool CanGameOver()
        {
            base.CanGameOver();
            bool result;
            if (this.m_king != null && !this.m_king.IsLiving)
            {
                kill++;
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        public override int UpdateUIData()
        {
            //return base.Game.TotalKillCount;
            return kill;
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            if (this.m_king != null && !this.m_king.IsLiving)
            {
                base.Game.IsWin = true;
            }
            else
            {
                base.Game.IsWin = false;
            }
        }

        protected int GetNpcCountByID(int Id)
        {
            int num = 0;
            foreach (SimpleNpc simpleNpc in this.list_0)
            {
                if (simpleNpc.NpcInfo.ID == Id)
                {
                    num++;
                }
            }
            return num;
        }
    }
}
