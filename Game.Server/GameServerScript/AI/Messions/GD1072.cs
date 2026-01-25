using System;
using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.Messions
{
    // Token: 0x0200025F RID: 607
    public class GD1072 : AMissionControl
    {
        // Token: 0x06001F13 RID: 7955 RVA: 0x000E3E60 File Offset: 0x000E2060
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

        // Token: 0x06001F14 RID: 7956 RVA: 0x000E3EB0 File Offset: 0x000E20B0
        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            base.Game.AddLoadingFile(1, "bombs/61.swf", "tank.resource.bombs.Bomb61");
            base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
            base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.boguoLeaderAsset");
            int[] npcIds = new int[]
            {
                this.BogoLideri,
                this.Mavi_Bogo_Koruma
            };
            base.Game.LoadResources(npcIds);
            base.Game.LoadNpcGameOverResources(npcIds);
            base.Game.SetMap(1073);
        }

        // Token: 0x06001F15 RID: 7957 RVA: 0x000E3F50 File Offset: 0x000E2150
        public override void OnStartGame()
        {
            base.OnStartGame();
            this.ArkaPlan_Efekti = base.Game.Createlayer(0, 0, "moive", "game.asset.living.BossBgAsset", "out", 1, 0);
            this.BogoLiderSarıEfekt = base.Game.Createlayer(680, 330, "font", "game.asset.living.boguoLeaderAsset", "out", 1, 0);
            this.Balufu = base.Game.CreateBoss(this.BogoLideri, 770, -1500, -1, 1, "");
            this.Balufu.FallFrom(this.Balufu.X, this.Balufu.Y, "fall", 0, 2, 1000);
            this.Balufu.SetRelateDemagemRect(34, -35, 11, 18);
            this.Balufu.AddDelay(10);
            this.Balufu.Say("Bogo Krallığı'nda sizin ne işiniz var? Yaşamaktan mı sıkıldınız yoksa?", 0, 6000);
            this.Balufu.PlayMovie("call", 5900, 0);
            this.ArkaPlan_Efekti.PlayMovie("in", 9000, 0);
            this.Balufu.PlayMovie("weakness", 10000, 5000);
            this.BogoLiderSarıEfekt.PlayMovie("in", 9000, 0);
            this.ArkaPlan_Efekti.PlayMovie("out", 15000, 0);
            base.Game.BossCardCount = 1;
        }

        // Token: 0x06001F16 RID: 7958 RVA: 0x000E40C8 File Offset: 0x000E22C8
        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
        }

        // Token: 0x06001F17 RID: 7959 RVA: 0x000E40D4 File Offset: 0x000E22D4
        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            this.int_0 = 0;
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

        // Token: 0x06001F18 RID: 7960 RVA: 0x000E4158 File Offset: 0x000E2358
        public override bool CanGameOver()
        {
            base.CanGameOver();
            bool flag = base.Game.TurnIndex > base.Game.MissionInfo.TotalTurn - 1;
            bool result;
            if (flag)
            {
                result = true;
            }
            else
            {
                bool flag2 = !this.Balufu.IsLiving;
                result = flag2;
            }
            return result;
        }

        // Token: 0x06001F19 RID: 7961 RVA: 0x000E41B4 File Offset: 0x000E23B4
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

        // Token: 0x06001F1A RID: 7962 RVA: 0x000E41F8 File Offset: 0x000E23F8
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

        // Token: 0x06001F1B RID: 7963 RVA: 0x000E4240 File Offset: 0x000E2440
        public override void DoOther()
        {
            base.DoOther();
            bool flag = this.Balufu != null;
            if (flag)
            {
                int num = base.Game.Random.Next(0, GD1072.Sohbet_1.Length);
                bool flag2 = this.Balufu != null;
                if (flag2)
                {
                    this.Balufu.Say(GD1072.Sohbet_1[num], 0, 0);
                }
            }
        }

        // Token: 0x06001F1C RID: 7964 RVA: 0x000E42A4 File Offset: 0x000E24A4
        public override void OnShooted()
        {
            base.OnShooted();
            bool flag = this.Balufu != null && this.Balufu.IsLiving && this.int_0 == 0;
            if (flag)
            {
                int num = base.Game.Random.Next(0, GD1072.Sohbet_2.Length);
                this.Balufu.Say(GD1072.Sohbet_2[num], 0, 1500);
                this.int_0 = 1;
            }
        }

        // Token: 0x06001F1D RID: 7965 RVA: 0x000E4319 File Offset: 0x000E2519
        public GD1072()
        {
            this.BogoLideri = 1003;
            this.Mavi_Bogo_Koruma = 1009;
        }

        // Token: 0x04001145 RID: 4421
        private SimpleBoss Balufu;

        // Token: 0x04001146 RID: 4422
        private PhysicalObj ArkaPlan_Efekti;

        // Token: 0x04001147 RID: 4423
        private PhysicalObj BogoLiderSarıEfekt;

        // Token: 0x04001148 RID: 4424
        private int int_0;

        // Token: 0x04001149 RID: 4425
        private int BogoLideri;

        // Token: 0x0400114A RID: 4426
        private int Mavi_Bogo_Koruma;

        // Token: 0x0400114B RID: 4427
        private static string[] Sohbet_1 = new string[]
        {
            "Seni çıktığın deliğe geri sokacağım!",
            "Bu kadar mı? Beni yenebileceğini mi sandınn?"
        };

        // Token: 0x0400114C RID: 4428
        private static string[] Sohbet_2 = new string[]
        {
            "Acıyor, dostum! Acıdıı..",
            "Her şey Nirlo için!"
        };
    }
}
