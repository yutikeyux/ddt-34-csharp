using System;
using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.Messions
{
    // Token: 0x02000261 RID: 609
    public class GK1272 : AMissionControl
    {
        // Token: 0x06001F28 RID: 7976 RVA: 0x000E4BB0 File Offset: 0x000E2DB0
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

        // Token: 0x06001F29 RID: 7977 RVA: 0x000E4C00 File Offset: 0x000E2E00
        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            base.Game.AddLoadingFile(2, "image/bomb/blastout/blastout61.swf", "bullet61");
            base.Game.AddLoadingFile(2, "image/bomb/bullet/bullet61.swf", "bullet61");
            base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
            base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.boguoLeaderAsset");
            int[] npcIds = new int[]
            {
                this.rumcuJicygk,
                this.int_1
            };
            base.Game.LoadResources(npcIds);
            base.Game.LoadNpcGameOverResources(npcIds);
            base.Game.SetMap(1073);
        }

        // Token: 0x06001F2A RID: 7978 RVA: 0x000E4CB8 File Offset: 0x000E2EB8
        public override void OnStartGame()
        {
            base.OnStartGame();
            this.ArkaPlan_Efekti = base.Game.Createlayer(0, 0, "moive", "game.asset.living.BossBgAsset", "out", 1, 0);
            this.BogoLiderSarıEfekt = base.Game.Createlayer(680, 330, "font", "game.asset.living.boguoLeaderAsset", "out", 1, 0);
            this.Balufu = base.Game.CreateBoss(this.rumcuJicygk, 770, -1500, -1, 1, "");
            this.Balufu.FallFrom(this.Balufu.X, this.Balufu.Y, "fall", 0, 1, 1000);
            this.Balufu.SetRelateDemagemRect(34, -35, 11, 18);
            this.Balufu.AddDelay(10);
            this.Balufu.Say("Eğer krallığıma izinsiz girmeye cüret ederseniz, ölmeye hazır olun!", 0, 6000);
            this.Balufu.PlayMovie("call", 5900, 0);
            this.ArkaPlan_Efekti.PlayMovie("in", 9000, 0);
            this.Balufu.PlayMovie("weakness", 10000, 5000);
            this.BogoLiderSarıEfekt.PlayMovie("in", 9000, 0);
            this.ArkaPlan_Efekti.PlayMovie("out", 15000, 0);
            base.Game.BossCardCount = 1;
        }

        // Token: 0x06001F2B RID: 7979 RVA: 0x000E4E30 File Offset: 0x000E3030
        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
        }

        // Token: 0x06001F2C RID: 7980 RVA: 0x000E4E3C File Offset: 0x000E303C
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

        // Token: 0x06001F2D RID: 7981 RVA: 0x000E4EC0 File Offset: 0x000E30C0
        public override bool CanGameOver()
        {
            bool flag = base.Game.TurnIndex > base.Game.MissionInfo.TotalTurn - 1;
            bool result;
            if (flag)
            {
                result = true;
            }
            else
            {
                base.CanGameOver();
                bool flag2 = base.Game.TurnIndex > base.Game.MissionInfo.TotalTurn - 1;
                if (flag2)
                {
                    result = true;
                }
                else
                {
                    bool flag3 = !this.Balufu.IsLiving;
                    result = flag3;
                }
            }
            return result;
        }

        // Token: 0x06001F2E RID: 7982 RVA: 0x000E4F44 File Offset: 0x000E3144
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

        // Token: 0x06001F2F RID: 7983 RVA: 0x000E4F88 File Offset: 0x000E3188
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

        // Token: 0x06001F30 RID: 7984 RVA: 0x000E4FD0 File Offset: 0x000E31D0
        public override void DoOther()
        {
            base.DoOther();
            bool flag = this.Balufu != null;
            if (flag)
            {
                int num = base.Game.Random.Next(0, GK1272.string_0.Length);
                bool flag2 = this.Balufu != null;
                if (flag2)
                {
                    this.Balufu.Say(GK1272.string_0[num], 0, 0);
                }
            }
        }

        // Token: 0x06001F31 RID: 7985 RVA: 0x000E5034 File Offset: 0x000E3234
        public override void OnShooted()
        {
            base.OnShooted();
            bool flag = this.Balufu != null && this.Balufu.IsLiving && this.int_0 == 0;
            if (flag)
            {
                int num = base.Game.Random.Next(0, GK1272.string_1.Length);
                this.Balufu.Say(GK1272.string_1[num], 0, 1500);
                this.int_0 = 1;
            }
        }

        // Token: 0x06001F32 RID: 7986 RVA: 0x000E50A9 File Offset: 0x000E32A9
        public GK1272()
        {
            this.rumcuJicygk = 1203;
            this.int_1 = 1209;
        }

        // Token: 0x06001F33 RID: 7987 RVA: 0x000E50C9 File Offset: 0x000E32C9
        static GK1272()
        {
        }

        // Token: 0x04001157 RID: 4439
        private SimpleBoss Balufu;

        // Token: 0x04001158 RID: 4440
        private PhysicalObj ArkaPlan_Efekti;

        // Token: 0x04001159 RID: 4441
        private PhysicalObj BogoLiderSarıEfekt;

        // Token: 0x0400115A RID: 4442
        private int int_0;

        // Token: 0x0400115B RID: 4443
        private int rumcuJicygk;

        // Token: 0x0400115C RID: 4444
        private int int_1;

        // Token: 0x0400115D RID: 4445
        private static string[] string_0 = new string[]
        {
            "Geçmiş olsun!",
            "Yaşamana izin vermeyeceğim!"
        };

        // Token: 0x0400118B RID: 4491
        private static string[] string_1 = new string[]
        {
            " Ahh bu acıdı ...",
            " benim canım yanmaz gardaş"
        };
    }
}
