using System;
using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.Messions
{
    // Token: 0x02000265 RID: 613
    public class GT1172 : AMissionControl
    {
        // Token: 0x06001F55 RID: 8021 RVA: 0x000E6648 File Offset: 0x000E4848
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

        // Token: 0x06001F56 RID: 8022 RVA: 0x000E6698 File Offset: 0x000E4898
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
            base.Game.LoadNpcGameOverResources(npcIds);
            base.Game.SetMap(1073);
        }

        // Token: 0x06001F57 RID: 8023 RVA: 0x000E6738 File Offset: 0x000E4938
        public override void OnStartGame()
        {
            base.OnStartGame();
            this.ArkaPlan_Efekti = base.Game.Createlayer(0, 0, "moive", "game.asset.living.BossBgAsset", "out", 1, 0);
            this.BogoLiderSarıEfekt = base.Game.Createlayer(680, 330, "font", "game.asset.living.boguoLeaderAsset", "out", 1, 0);
            this.Balufu = base.Game.CreateBoss(this.int_1, 770, -1500, -1, 1, "");
            this.Balufu.FallFrom(this.Balufu.X, this.Balufu.Y, "fall", 0, 2, 1000);
            this.Balufu.SetRelateDemagemRect(34, -35, 11, 18);
            this.Balufu.AddDelay(10);
            this.Balufu.Say("Dám xâm phạm địa bàn của ta, chán sống rồi à?", 0, 6000);
            this.Balufu.PlayMovie("call", 5900, 0);
            this.ArkaPlan_Efekti.PlayMovie("in", 9000, 0);
            this.Balufu.PlayMovie("weakness", 10000, 5000);
            this.BogoLiderSarıEfekt.PlayMovie("in", 9000, 0);
            this.ArkaPlan_Efekti.PlayMovie("out", 15000, 0);
            base.Game.BossCardCount = 1;
        }

        // Token: 0x06001F58 RID: 8024 RVA: 0x000E68B0 File Offset: 0x000E4AB0
        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
        }

        // Token: 0x06001F59 RID: 8025 RVA: 0x000E68BC File Offset: 0x000E4ABC
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

        // Token: 0x06001F5A RID: 8026 RVA: 0x000E6940 File Offset: 0x000E4B40
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

        // Token: 0x06001F5B RID: 8027 RVA: 0x000E699C File Offset: 0x000E4B9C
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

        // Token: 0x06001F5C RID: 8028 RVA: 0x000E69E0 File Offset: 0x000E4BE0
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

        // Token: 0x06001F5D RID: 8029 RVA: 0x000E6A28 File Offset: 0x000E4C28
        public override void DoOther()
        {
            base.DoOther();
            bool flag = this.Balufu != null;
            if (flag)
            {
                int num = base.Game.Random.Next(0, GT1172.string_0.Length);
                bool flag2 = this.Balufu != null;
                if (flag2)
                {
                    this.Balufu.Say(GT1172.string_0[num], 0, 0);
                }
            }
        }

        // Token: 0x06001F5E RID: 8030 RVA: 0x000E6A8C File Offset: 0x000E4C8C
        public override void OnShooted()
        {
            base.OnShooted();
            bool flag = this.Balufu != null && this.Balufu.IsLiving && this.int_0 == 0;
            if (flag)
            {
                int num = base.Game.Random.Next(0, GT1172.string_1.Length);
                this.Balufu.Say(GT1172.string_1[num], 0, 1500);
                this.int_0 = 1;
            }
        }

        // Token: 0x06001F5F RID: 8031 RVA: 0x000E6B01 File Offset: 0x000E4D01
        public GT1172()
        {
            this.int_1 = 1103;
            this.int_2 = 1109;
        }

        // Token: 0x06001F60 RID: 8032 RVA: 0x000E6B21 File Offset: 0x000E4D21
        static GT1172()
        {
        }

        // Token: 0x04001184 RID: 4484
        private SimpleBoss Balufu;

        // Token: 0x04001185 RID: 4485
        private PhysicalObj ArkaPlan_Efekti;

        // Token: 0x04001186 RID: 4486
        private PhysicalObj BogoLiderSarıEfekt;

        // Token: 0x04001187 RID: 4487
        private int int_0;

        // Token: 0x04001188 RID: 4488
        private int int_1;

        // Token: 0x04001189 RID: 4489
        private int int_2;

        // Token: 0x0400118A RID: 4490
        private static string[] string_0 = new string[]
        {
            "Gửi cho bạn trở về nhà!",
            "Một mình, bạn có ảo tưởng có thể đánh bại tôi?"
        };

        // Token: 0x0400118B RID: 4491
        private static string[] string_1 = new string[]
        {
            " Đau ah! Đau ...",
            "Quốc vương vạn tuế ..."
        };
    }
}
