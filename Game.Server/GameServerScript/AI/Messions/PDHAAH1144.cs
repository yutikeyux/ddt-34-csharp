using System;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.Messions
{
    // Token: 0x020002BF RID: 703
    public class PDHAAH1144 : AMissionControl
    {
        // Token: 0x0600230A RID: 8970 RVA: 0x00100F94 File Offset: 0x000FF194
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

        // Token: 0x0600230B RID: 8971 RVA: 0x00100FE4 File Offset: 0x000FF1E4
        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            int[] resources = new int[]
            {
                this.İlkMinotar,
                this.MorRuh,
                this.İkinciMinotar,
                this.MaviRuh
            };
            int[] gameOverResource = new int[]
            {
                this.İlkMinotar,
                this.İkinciMinotar
            };
            base.Game.AddLoadingFile(2, "image/game/effect/4/power.swf", "game.crazytank.assetmap.Buff_powup");
            base.Game.AddLoadingFile(2, "image/game/effect/4/blade.swf", "asset.game.4.blade");
            base.Game.AddLoadingFile(2, "image/game/thing/bossbornbgasset.swf", "game.asset.living.BossBgAsset");
            base.Game.AddLoadingFile(2, "image/game/thing/bossbornbgasset.swf", "game.asset.living.tingyuanlieshouAsset");
            base.Game.LoadResources(resources);
            base.Game.LoadNpcGameOverResources(gameOverResource);
            base.Game.SetMap(1144);
        }

        // Token: 0x0600230C RID: 8972 RVA: 0x001010C4 File Offset: 0x000FF2C4
        public override void OnStartGame()
        {
            base.OnStartGame();
            LivingConfig config = base.Game.BaseLivingConfig();
            config.HaveShield = true;
            this.BossGirişEfekti = base.Game.Createlayer(0, 0, "moive", "game.asset.living.BossBgAsset", "out", 1, 0);
            this.BossYazıEfekti = base.Game.Createlayer(1019, 590, "front", "game.asset.living.emozhanshiAsset", "out", 1, 0);
            this.Mino = base.Game.CreateBoss(this.İlkMinotar, 1255, 958, -1, 1, "born", config);
            this.Mino.SetRelateDemagemRect(this.Mino.NpcInfo.X, this.Mino.NpcInfo.Y, this.Mino.NpcInfo.Width, this.Mino.NpcInfo.Height);
            this.Mino.CallFuction(new LivingCallBack(this.İlkMinoyuDoğdur), 1000);
        }

        // Token: 0x0600230D RID: 8973 RVA: 0x001011D0 File Offset: 0x000FF3D0
        private void İlkMinoyuDoğdur()
        {
            base.Game.SendObjectFocus(this.Mino, 1, 500, 0);
            this.Mino.PlayMovie("in", 2000, 0);
            base.Game.SendObjectFocus(this.Mino, 2, 2000, 3000);
            this.Mino.PlayMovie("standA", 9000, 0);
            this.Mino.Say("Bunu çooook uzun zamandır bekliyordum!", 0, 4200);
            this.BossGirişEfekti.PlayMovie("in", 9200, 0);
            this.BossYazıEfekti.PlayMovie("in", 9400, 0);
            this.BossGirişEfekti.PlayMovie("out", 11200, 0);
            this.BossYazıEfekti.PlayMovie("out", 11400, 0);
        }

        // Token: 0x0600230E RID: 8974 RVA: 0x001012B5 File Offset: 0x000FF4B5
        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
        }

        // Token: 0x0600230F RID: 8975 RVA: 0x001012C0 File Offset: 0x000FF4C0
        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            bool flag = this.Mino == null || this.Mino.IsLiving;
            if (flag)
            {
                base.Game.TotalKillCount = 0;
            }
            bool flag2 = base.Game.TurnIndex > 1;
            if (flag2)
            {
                bool flag3 = this.BossGirişEfekti != null;
                if (flag3)
                {
                    base.Game.RemovePhysicalObj(this.BossGirişEfekti, true);
                    this.BossGirişEfekti = null;
                }
                bool flag4 = this.BossYazıEfekti != null;
                if (flag4)
                {
                    base.Game.RemovePhysicalObj(this.BossYazıEfekti, true);
                    this.BossYazıEfekti = null;
                }
            }
        }

        // Token: 0x06002310 RID: 8976 RVA: 0x00101364 File Offset: 0x000FF564
        public override bool CanGameOver()
        {
            bool flag = this.Mino2 != null && !this.Mino2.IsLiving && this.Durum >= 2;
            bool result;
            if (flag)
            {
                result = true;
            }
            else
            {
                bool flag2 = base.Game.TotalTurn > base.Game.MissionInfo.TotalTurn;
                if (flag2)
                {
                    result = true;
                }
                else
                {
                    bool flag3 = this.Durum <= 1 && this.Mino != null && !this.Mino.IsLiving;
                    if (flag3)
                    {
                        this.Durum++;
                        this.Mino2 = base.Game.CreateBoss(this.İkinciMinotar, this.Mino.X, this.Mino.Y, this.Mino.Direction, 1, "standB");
                        this.Mino2.CallFuction(new LivingCallBack(this.CreateBoss), 1000);
                    }
                    result = false;
                }
            }
            return result;
        }

        // Token: 0x06002311 RID: 8977 RVA: 0x0010146C File Offset: 0x000FF66C
        private void CreateBoss()
        {
            base.Game.RemoveLiving(this.Mino.Id);
            this.Mino2.PlayMovie("born", 0, 0);
            this.Mino2.Say("<span class=\"red\">İşte şimdi sıçtınız altınıza!</span>", 0, 200);
        }

        // Token: 0x06002312 RID: 8978 RVA: 0x001014BC File Offset: 0x000FF6BC
        public override int UpdateUIData()
        {
            base.UpdateUIData();
            return base.Game.TotalKillCount;
        }

        // Token: 0x06002313 RID: 8979 RVA: 0x001014E0 File Offset: 0x000FF6E0
        public override void OnGameOver()
        {
            base.OnGameOver();
            bool flag = this.Durum >= 2 && this.Mino2 != null && !this.Mino2.IsLiving && this.Mino != null && !this.Mino.IsLiving;
            if (flag)
            {
                base.Game.IsWin = true;
            }
            else
            {
                base.Game.IsWin = false;
            }
        }

        // Token: 0x06002314 RID: 8980 RVA: 0x0010154E File Offset: 0x000FF74E
        public override void OnShooted()
        {
            base.OnShooted();
        }

        // Token: 0x06002315 RID: 8981 RVA: 0x00101558 File Offset: 0x000FF758
        public PDHAAH1144()
        {
        }

        // Token: 0x0400140E RID: 5134
        private SimpleBoss Mino = null;

        // Token: 0x0400140F RID: 5135
        private SimpleBoss Mino2 = null;

        // Token: 0x04001410 RID: 5136
        private int İlkMinotar = 4308;

        // Token: 0x04001411 RID: 5137
        private int MorRuh = 4307;

        // Token: 0x04001412 RID: 5138
        private int İkinciMinotar = 4309;

        // Token: 0x04001413 RID: 5139
        private int MaviRuh = 4310;

        // Token: 0x04001414 RID: 5140
        private int Durum = 1;

        // Token: 0x04001415 RID: 5141
        private PhysicalObj BossGirişEfekti;

        // Token: 0x04001416 RID: 5142
        private PhysicalObj BossYazıEfekti;
    }
}
