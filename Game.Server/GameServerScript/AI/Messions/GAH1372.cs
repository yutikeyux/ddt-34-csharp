using System;
using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.Messions
{
    // Token: 0x0200024E RID: 590
    public class GAH1372 : AMissionControl
    {
        // Token: 0x06001E65 RID: 7781 RVA: 0x000DDF88 File Offset: 0x000DC188
        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            bool flag = score > 1540;
            int result;
            if (flag)
            {
                result = 3;
            }
            else
            {
                bool flag2 = score > 1410;
                if (flag2)
                {
                    result = 2;
                }
                else
                {
                    bool flag3 = score > 1285;
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

        // Token: 0x06001E66 RID: 7782 RVA: 0x000DDFD8 File Offset: 0x000DC1D8
        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            base.Game.AddLoadingFile(1, "bombs/61.swf", "tank.resource.bombs.Bomb61");
            base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
            base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.boguoLeaderAsset");
            int[] npcIds = new int[]
            {
                this.int_1,
                this.int_2
            };
            base.Game.LoadResources(npcIds);
            int[] npcIds2 = new int[]
            {
                this.int_1
            };
            base.Game.LoadNpcGameOverResources(npcIds2);
            base.Game.SetMap(1073);
        }

        // Token: 0x06001E67 RID: 7783 RVA: 0x000DE088 File Offset: 0x000DC288
        public override void OnStartGame()
        {
            this.ArkaPlan_Efekti = base.Game.Createlayer(0, 0, "moive", "game.asset.living.BossBgAsset", "out", 1, 1);
            this.BogoLiderSarıEfekt = base.Game.Createlayer(680, 330, "font", "game.asset.living.boguoLeaderAsset", "out", 1, 1);
            this.Balufu = base.Game.CreateBoss(this.int_1, 770, -1500, -1, 1, "");
            this.Balufu.FallFrom(this.Balufu.X, this.Balufu.Y, "", 0, 2, 2000);
            this.Balufu.SetRelateDemagemRect(34, -35, 11, 18);
            this.Balufu.AddDelay(10);
            this.Balufu.Say("Buraya gelmeye ne cüret ettiniz?", 0, 6000);
            this.Balufu.PlayMovie("call", 5900, 0);
            this.ArkaPlan_Efekti.PlayMovie("in", 9000, 0);
            this.Balufu.PlayMovie("weakness", 10000, 5000);
            this.BogoLiderSarıEfekt.PlayMovie("in", 9000, 0);
            this.ArkaPlan_Efekti.PlayMovie("out", 15000, 0);
            base.Game.BossCardCount = 1;
            base.OnStartGame();
        }

        // Token: 0x06001E68 RID: 7784 RVA: 0x000DE200 File Offset: 0x000DC400
        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
        }

        // Token: 0x06001E69 RID: 7785 RVA: 0x000DE20C File Offset: 0x000DC40C
        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            bool flag = base.Game.TurnIndex > 1;
            if (flag)
            {
                bool flag2 = this.ArkaPlan_Efekti != null;
                if (flag2)
                {
                    base.Game.RemovePhysicalObj(this.ArkaPlan_Efekti, true);
                    this.ArkaPlan_Efekti = null;
                }
                bool flag3 = this.BogoLiderSarıEfekt != null;
                if (flag3)
                {
                    base.Game.RemovePhysicalObj(this.BogoLiderSarıEfekt, true);
                    this.BogoLiderSarıEfekt = null;
                }
            }
        }

        // Token: 0x06001E6A RID: 7786 RVA: 0x000DE288 File Offset: 0x000DC488
        public override bool CanGameOver()
        {
            base.CanGameOver();
            return !this.Balufu.IsLiving;
        }

        // Token: 0x06001E6B RID: 7787 RVA: 0x000DE2BC File Offset: 0x000DC4BC
        public override int UpdateUIData()
        {
            bool flag = this.Balufu == null;
            int result;
            if (flag)
            {
                result = 0;
            }
            else
            {
                bool flag2 = !this.Balufu.IsLiving;
                if (flag2)
                {
                    result = 1;
                }
                else
                {
                    result = base.UpdateUIData();
                }
            }
            return result;
        }

        // Token: 0x06001E6C RID: 7788 RVA: 0x000DE300 File Offset: 0x000DC500
        public override void OnGameOver()
        {
            base.OnGameOver();
            bool flag = !this.Balufu.IsLiving;
            if (flag)
            {
                base.Game.IsWin = true;
            }
            else
            {
                base.Game.IsWin = false;
            }
        }

        // Token: 0x06001E6D RID: 7789 RVA: 0x000DE348 File Offset: 0x000DC548
        public override void DoOther()
        {
            base.DoOther();
            bool flag = this.Balufu != null;
            if (flag)
            {
                int num = base.Game.Random.Next(0, GAH1372.string_0.Length);
                bool flag2 = this.Balufu != null;
                if (flag2)
                {
                    this.Balufu.Say(GAH1372.string_0[num], 0, 0);
                }
            }
        }

        // Token: 0x06001E6E RID: 7790 RVA: 0x000DE3AC File Offset: 0x000DC5AC
        public override void OnShooted()
        {
            base.OnShooted();
            bool flag = this.Balufu != null && this.Balufu.IsLiving && this.int_0 == 0;
            if (flag)
            {
                int num = base.Game.Random.Next(0, GAH1372.string_1.Length);
                this.Balufu.Say(GAH1372.string_1[num], 0, 1500);
                this.int_0 = 1;
            }
        }

        // Token: 0x06001E6F RID: 7791 RVA: 0x000DE421 File Offset: 0x000DC621
        public GAH1372()
        {
            this.int_1 = 1303;
            this.int_2 = 1309;
        }

        // Token: 0x06001E70 RID: 7792 RVA: 0x000DE441 File Offset: 0x000DC641
        static GAH1372()
        {
        }

        // Token: 0x040010BE RID: 4286
        private SimpleBoss Balufu;

        // Token: 0x040010BF RID: 4287
        private PhysicalObj ArkaPlan_Efekti;

        // Token: 0x040010C0 RID: 4288
        private PhysicalObj BogoLiderSarıEfekt;

        // Token: 0x040010C1 RID: 4289
        private int int_0;

        // Token: 0x040010C2 RID: 4290
        private int int_1;

        // Token: 0x040010C3 RID: 4291
        private int int_2;

        // Token: 0x040010C4 RID: 4292
        private static string[] string_0 = new string[]
        {
            "Tabi efendim!",
            "Beni yenebileceğini mi sanıyorsun?"
        };

        // Token: 0x040010C5 RID: 4293
        private static string[] string_1 = new string[]
        {
            " Ah ...",
            "Bu acıdı! ..."
        };
    }
}
