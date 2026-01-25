using System;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.Messions
{
    // Token: 0x020002BB RID: 699
    public class PDHAK1143 : AMissionControl
    {
        // Token: 0x060022DA RID: 8922 RVA: 0x000FF92C File Offset: 0x000FDB2C
        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            bool flag = score > 1750;
            int result;
            if (flag)
            {
                result = 3;
            }
            else
            {
                bool flag2 = score > 1675;
                if (flag2)
                {
                    result = 2;
                }
                else
                {
                    bool flag3 = score > 1600;
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

        // Token: 0x060022DB RID: 8923 RVA: 0x000FF97C File Offset: 0x000FDB7C
        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            int[] npcidleri = new int[]
            {
                this.Kartal,
                this.Kurt,
                this.int_2
            };
            int[] npcIds2 = new int[]
            {
                this.Kartal,
                this.Kurt
            };
            base.Game.AddLoadingFile(2, "image/game/effect/4/feather.swf", "asset.game.4.feather");
            base.Game.AddLoadingFile(2, "image/game/thing/bossbornbgasset.swf", "game.asset.living.BossBgAsset");
            base.Game.AddLoadingFile(2, "image/game/thing/bossbornbgasset.swf", "game.asset.living.tingyuanlieshouAsset");
            base.Game.LoadResources(npcidleri);
            base.Game.LoadNpcGameOverResources(npcIds2);
            base.Game.SetMap(1143);
        }

        // Token: 0x060022DC RID: 8924 RVA: 0x000FFA3C File Offset: 0x000FDC3C
        public override void OnStartGame()
        {
            base.OnStartGame();
            this.Arkaplan_Efekt = base.Game.Createlayer(0, 0, "moive", "game.asset.living.BossBgAsset", "out", 1, 0);
            this.Efekt_Yazý = base.Game.Createlayer(1098, 706, "front", "game.asset.living.tingyuanlieshouAsset", "out", 1, 0);
            LivingConfig kartalkuþ = base.Game.BaseLivingConfig();
            kartalkuþ.IsFly = true;
            this.kartalkus = base.Game.CreateBoss(this.Kartal, 354, 344, -1, 1, "born", kartalkuþ);
            this.kartalkus.SetRelateDemagemRect(this.kartalkus.NpcInfo.X, this.kartalkus.NpcInfo.Y, this.kartalkus.NpcInfo.Width, this.kartalkus.NpcInfo.Height);
            base.Game.SendObjectFocus(this.kartalkus, 1, 100, 0);
            base.Game.SendFreeFocus(1460, 962, 1, 3000, 0);
            this.kartalkus.CallFuction(new LivingCallBack(this.KurtDoður), 4000);
        }

        // Token: 0x060022DD RID: 8925 RVA: 0x000FFB7C File Offset: 0x000FDD7C
        private void KurtDoður()
        {
            this.simpleBoss_1 = base.Game.CreateBoss(this.Kurt, 1460, 962, -1, 1, "born");
            this.simpleBoss_1.SetRelateDemagemRect(this.simpleBoss_1.NpcInfo.X, this.simpleBoss_1.NpcInfo.Y, this.simpleBoss_1.NpcInfo.Width, this.simpleBoss_1.NpcInfo.Height);
            this.Arkaplan_Efekt.PlayMovie("in", 3000, 0);
            this.Efekt_Yazý.PlayMovie("in", 3200, 0);
            this.Arkaplan_Efekt.PlayMovie("out", 6000, 0);
            this.Efekt_Yazý.PlayMovie("out", 6000, 0);
        }

        // Token: 0x060022DE RID: 8926 RVA: 0x000FFC5A File Offset: 0x000FDE5A
        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
        }

        // Token: 0x060022DF RID: 8927 RVA: 0x000FFC64 File Offset: 0x000FDE64
        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            bool flag = base.Game.TurnIndex > 1;
            if (flag)
            {
                bool flag2 = this.Arkaplan_Efekt != null;
                if (flag2)
                {
                    base.Game.RemovePhysicalObj(this.Arkaplan_Efekt, true);
                    this.Arkaplan_Efekt = null;
                }
                bool flag3 = this.Efekt_Yazý != null;
                if (flag3)
                {
                    base.Game.RemovePhysicalObj(this.Efekt_Yazý, true);
                    this.Efekt_Yazý = null;
                }
            }
        }

        // Token: 0x060022E0 RID: 8928 RVA: 0x000FFCE0 File Offset: 0x000FDEE0
        public override bool CanGameOver()
        {
            bool flag = this.kartalkus != null && !this.kartalkus.IsLiving && this.simpleBoss_1 != null && !this.simpleBoss_1.IsLiving;
            bool result;
            if (flag)
            {
                this.int_3++;
                result = true;
            }
            else
            {
                bool flag2 = base.Game.TotalTurn > base.Game.MissionInfo.TotalTurn;
                result = flag2;
            }
            return result;
        }

        // Token: 0x060022E1 RID: 8929 RVA: 0x000FFD60 File Offset: 0x000FDF60
        public override int UpdateUIData()
        {
            base.UpdateUIData();
            return this.int_3;
        }

        // Token: 0x060022E2 RID: 8930 RVA: 0x000FFD80 File Offset: 0x000FDF80
        public override void OnGameOver()
        {
            base.OnGameOver();
            bool flag = this.kartalkus != null && !this.kartalkus.IsLiving && this.simpleBoss_1 != null && !this.simpleBoss_1.IsLiving;
            if (flag)
            {
                base.Game.IsWin = true;
            }
            else
            {
                base.Game.IsWin = false;
            }
        }

        // Token: 0x060022E3 RID: 8931 RVA: 0x000FFDE5 File Offset: 0x000FDFE5
        public override void OnShooted()
        {
            base.OnShooted();
        }

        // Token: 0x060022E4 RID: 8932 RVA: 0x000FFDEF File Offset: 0x000FDFEF
        public PDHAK1143()
        {
            this.Kartal = 4205;
            this.Kurt = 4206;
            this.int_2 = 4202;
        }

        // Token: 0x040013EE RID: 5102
        private SimpleBoss kartalkus;

        // Token: 0x040013EF RID: 5103
        private SimpleBoss simpleBoss_1;

        // Token: 0x040013F0 RID: 5104
        private int Kartal;

        // Token: 0x040013F1 RID: 5105
        private int Kurt;

        // Token: 0x040013F2 RID: 5106
        private int int_2;

        // Token: 0x040013F3 RID: 5107
        private int int_3;

        // Token: 0x040013F4 RID: 5108
        private PhysicalObj Arkaplan_Efekt;

        // Token: 0x040013F5 RID: 5109
        private PhysicalObj Efekt_Yazý;
    }
}
