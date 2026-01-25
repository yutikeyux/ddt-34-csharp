using System;
using System.Collections.Generic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.Messions
{
    // Token: 0x020002B4 RID: 692
    public class NTM1087 : AMissionControl
    {
        // Token: 0x06002287 RID: 8839 RVA: 0x000FD408 File Offset: 0x000FB608
        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            bool flag = score > 900;
            int result;
            if (flag)
            {
                result = 3;
            }
            else
            {
                bool flag2 = score > 825;
                if (flag2)
                {
                    result = 2;
                }
                else
                {
                    bool flag3 = score > 725;
                    if (flag3)
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

        // Token: 0x06002288 RID: 8840 RVA: 0x000FD458 File Offset: 0x000FB658
        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
            base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.boguoLeaderAsset");
            base.Game.AddLoadingFile(2, "image/bomb/blastout/blastout61.swf", "bullet61");
            base.Game.AddLoadingFile(2, "image/bomb/bullet/bullet61.swf", "bullet61");
            int[] npcIdList = new int[]
            {
                this.redNpcID,
                this.blueNpcID,
                this.bossID
            };
            base.Game.LoadResources(npcIdList);
            base.Game.LoadNpcGameOverResources(npcIdList);
            base.Game.SetMap(this.mapId);
        }

        // Token: 0x06002289 RID: 8841 RVA: 0x000FD517 File Offset: 0x000FB717
        public override void OnStartGame()
        {
            this.CreateNpc();
        }

        // Token: 0x0600228A RID: 8842 RVA: 0x000FD521 File Offset: 0x000FB721
        public override void OnNewTurnStarted()
        {
        }

        // Token: 0x0600228B RID: 8843 RVA: 0x000FD524 File Offset: 0x000FB724
        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
        }

        // Token: 0x0600228C RID: 8844 RVA: 0x000FD530 File Offset: 0x000FB730
        public override bool CanGameOver()
        {
            foreach (SimpleNpc npc in this.simpleNpcList)
            {
                bool isLiving = npc.IsLiving;
                if (isLiving)
                {
                    return false;
                }
            }
            bool flag = this.m_boss != null && !this.m_boss.IsLiving;
            bool result;
            if (flag)
            {
                result = true;
            }
            else
            {
                bool flag2 = this.m_boss == null;
                if (flag2)
                {
                    this.CreateBoss();
                }
                result = false;
            }
            return result;
        }

        // Token: 0x0600228D RID: 8845 RVA: 0x000FD5D4 File Offset: 0x000FB7D4
        public void CreateBoss()
        {
            this.m_moive = base.Game.Createlayer(0, 0, "moive", "game.asset.living.BossBgAsset", "out", 1, 0);
            this.m_front = base.Game.Createlayer(200, 470, "font", "game.asset.living.boguoLeaderAsset", "out", 1, 0);
            this.m_boss = base.Game.CreateBoss(this.bossID, 260, 560, 1, 1, "");
            this.m_boss.FallFrom(260, 620, "fall", 0, 2, 1000);
            this.m_boss.SetRelateDemagemRect(this.m_boss.NpcInfo.X, this.m_boss.NpcInfo.Y, this.m_boss.NpcInfo.Width, this.m_boss.NpcInfo.Height);
            this.m_boss.Say("Onları yendiğinde eline ne geçti? Şimdi ben varım！", 0, 3000);
            this.m_moive.PlayMovie("in", 6000, 0);
            this.m_front.PlayMovie("in", 6000, 0);
            this.m_moive.PlayMovie("out", 9000, 0);
            this.m_front.PlayMovie("out", 9000, 0);
        }

        // Token: 0x0600228E RID: 8846 RVA: 0x000FD73C File Offset: 0x000FB93C
        public override int UpdateUIData()
        {
            base.UpdateUIData();
            return base.Game.TotalKillCount;
        }

        // Token: 0x0600228F RID: 8847 RVA: 0x000FD760 File Offset: 0x000FB960
        public override void OnGameOver()
        {
            base.OnGameOver();
            bool flag = base.Game.GetLivedLivings().Count == 0;
            if (flag)
            {
                base.Game.IsWin = true;
            }
            else
            {
                base.Game.IsWin = false;
            }
        }

        // Token: 0x06002290 RID: 8848 RVA: 0x000FD7AC File Offset: 0x000FB9AC
        private void CreateNpc()
        {
            int[,] points = new int[,]
            {
                {
                    260,
                    620
                },
                {
                    312,
                    625
                },
                {
                    350,
                    621
                },
                {
                    285,
                    620
                },
                {
                    331,
                    625
                }
            };
            for (int i = 0; i <= 2; i++)
            {
                this.simpleNpcList.Add(base.Game.CreateNpc(this.redNpcID, points[i, 0], points[i, 1], 1, 1));
            }
            for (int j = 3; j <= 4; j++)
            {
                this.simpleNpcList.Add(base.Game.CreateNpc(this.blueNpcID, points[j, 0], points[j, 1], 1, 1));
            }
        }

        // Token: 0x06002291 RID: 8849 RVA: 0x000FD857 File Offset: 0x000FBA57
        public NTM1087()
        {
        }

        // Token: 0x040013B1 RID: 5041
        private int mapId = 2012;

        // Token: 0x040013B2 RID: 5042
        private SimpleBoss m_boss;

        // Token: 0x040013B3 RID: 5043
        private PhysicalObj m_moive;

        // Token: 0x040013B4 RID: 5044
        private PhysicalObj m_front;

        // Token: 0x040013B5 RID: 5045
        private int bossID = 23003;

        // Token: 0x040013B6 RID: 5046
        private int redNpcID = 23001;

        // Token: 0x040013B7 RID: 5047
        private int blueNpcID = 23002;

        // Token: 0x040013B8 RID: 5048
        private List<SimpleNpc> simpleNpcList = new List<SimpleNpc>();
    }
}
