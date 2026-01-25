using System;
using System.Collections.Generic;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Effects;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.Messions
{
    // Token: 0x020002B7 RID: 695
    public class PDHAAH1142 : AMissionControl
    {
        // Token: 0x170000C7 RID: 199
        // (get) Token: 0x060022A6 RID: 8870 RVA: 0x000FE284 File Offset: 0x000FC484
        // (set) Token: 0x060022A7 RID: 8871 RVA: 0x000FE29C File Offset: 0x000FC49C
        public int DuvarBoþluðu
        {
            get
            {
                return this.KIRILANDUVAR;
            }
            set
            {
                this.KIRILANDUVAR = value;
            }
        }

        // Token: 0x170000C8 RID: 200
        // (get) Token: 0x060022A8 RID: 8872 RVA: 0x000FE2A8 File Offset: 0x000FC4A8
        // (set) Token: 0x060022A9 RID: 8873 RVA: 0x000FE2C0 File Offset: 0x000FC4C0
        public SimpleBoss DuvarBombasý
        {
            get
            {
                return this.FIÇIBOMBA;
            }
            set
            {
                this.FIÇIBOMBA = value;
            }
        }

        // Token: 0x060022AA RID: 8874 RVA: 0x000FE2CC File Offset: 0x000FC4CC
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

        // Token: 0x060022AB RID: 8875 RVA: 0x000FE31C File Offset: 0x000FC51C
        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            int[] npcIDleri = new int[]
            {
                this.KALE,
                this.ZombiGoblin,
                this.FýçýBomba
            };
            base.Game.LoadResources(npcIDleri);
            base.Game.LoadNpcGameOverResources(npcIDleri);
            base.Game.AddLoadingFile(2, "image/game/effect/4/gate.swf", "game.asset.Gate");
            base.Game.SetMap(1142);
        }

        // Token: 0x060022AC RID: 8876 RVA: 0x000FE398 File Offset: 0x000FC598
        public override void OnStartGame()
        {
            base.OnStartGame();
            LivingConfig livingConfig = base.Game.BaseLivingConfig();
            livingConfig.CanTakeDamage = false;
            this.KALEDUVARI = base.Game.CreateBoss(this.KALE, 1520, 350, -1, 1, "", livingConfig);
            base.Game.SendHideBlood(this.KALEDUVARI, 0);
            this.BombayýÇaðýr();
            base.Game.SendFreeFocus(1500, 250, 1, 2000, 3000);
        }

        // Token: 0x060022AD RID: 8877 RVA: 0x000FE425 File Offset: 0x000FC625
        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
        }

        // Token: 0x060022AE RID: 8878 RVA: 0x000FE430 File Offset: 0x000FC630
        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            bool flag = this.FIÇIBOMBA != null && this.FIÇIBOMBA.IsLiving && this.FIÇIBOMBA.X >= 600 && base.Game.FindAllNpcLiving().Length == 0 && base.Game.CurrentTurnLiving is Player;
            if (flag)
            {
                this.GobliniÇaðýr();
            }
            bool flag2 = this.FIÇIBOMBA != null && !this.FIÇIBOMBA.IsLiving;
            if (flag2)
            {
                base.Game.RemoveLiving(this.FIÇIBOMBA, true);
                this.BombayýÇaðýr();
            }
            bool flag3 = base.Game.CurrentTurnLiving is Player && this.FIÇIBOMBA != null && this.FIÇIBOMBA.IsLiving && this.KALEDUVARI.Properties1 == 1;
            if (flag3)
            {
                this.FIÇIBOMBA.PlayMovie("standB", 1200, 0);
            }
        }

        // Token: 0x060022AF RID: 8879 RVA: 0x000FE52C File Offset: 0x000FC72C
        private void BombayýÇaðýr()
        {
            LivingConfig livingConfig = base.Game.BaseLivingConfig();
            livingConfig.IsHelper = true;
            this.FIÇIBOMBA = base.Game.CreateBoss(this.FýçýBomba, 321, 746, 1, 0, "", livingConfig);
            this.FIÇIBOMBA.AddEffect(new ContinueReduceBloodEffect(2, this.YANMACANI, this.FIÇIBOMBA), 0);
        }

        // Token: 0x060022B0 RID: 8880 RVA: 0x000FE598 File Offset: 0x000FC798
        private void GobliniÇaðýr()
        {
            int num = 0;
            this.NpcListesi = new List<SimpleNpc>();
            for (int i = 0; i < this.DOÐACAKZOMBÝGOBLÝNSAYISI; i++)
            {
                bool flag = num > 0;
                if (flag)
                {
                    int num2 = base.Game.Random.Next(5, 110);
                    this.NpcListesi.Add(base.Game.CreateNpc(this.ZombiGoblin, this.FIÇIBOMBA.X - num2, this.FIÇIBOMBA.Y, 0));
                }
                else
                {
                    this.NpcListesi.Add(base.Game.CreateNpc(this.ZombiGoblin, this.FIÇIBOMBA.X - 20, this.FIÇIBOMBA.Y, 0));
                }
                num++;
            }
        }

        // Token: 0x060022B1 RID: 8881 RVA: 0x000FE668 File Offset: 0x000FC868
        public override bool CanGameOver()
        {
            base.CanGameOver();
            bool flag = this.KIRILANDUVAR >= base.Game.MissionInfo.TotalCount;
            bool result;
            if (flag)
            {
                result = true;
            }
            else
            {
                bool flag2 = base.Game.TotalTurn > base.Game.MissionInfo.TotalTurn;
                result = flag2;
            }
            return result;
        }

        // Token: 0x060022B2 RID: 8882 RVA: 0x000FE6CC File Offset: 0x000FC8CC
        public override int UpdateUIData()
        {
            base.UpdateUIData();
            return this.KIRILANDUVAR;
        }

        // Token: 0x060022B3 RID: 8883 RVA: 0x000FE6EC File Offset: 0x000FC8EC
        public override void OnGameOver()
        {
            base.OnGameOver();
            bool flag = this.KIRILANDUVAR >= base.Game.MissionInfo.TotalCount;
            if (flag)
            {
                base.Game.IsWin = true;
            }
            else
            {
                base.Game.IsWin = false;
            }
        }

        // Token: 0x060022B4 RID: 8884 RVA: 0x000FE740 File Offset: 0x000FC940
        public PDHAAH1142()
        {
            this.NpcListesi = new List<SimpleNpc>();
            this.FýçýBomba = 4301;
            this.ZombiGoblin = 4303;
            this.KALE = 4304;
            this.YANMACANI = 500;
            this.DOÐACAKZOMBÝGOBLÝNSAYISI = 1;
        }

        // Token: 0x040013CC RID: 5068
        private List<SimpleNpc> NpcListesi;

        // Token: 0x040013CD RID: 5069
        private SimpleBoss KALEDUVARI;

        // Token: 0x040013CE RID: 5070
        private SimpleBoss FIÇIBOMBA;

        // Token: 0x040013CF RID: 5071
        private int FýçýBomba;

        // Token: 0x040013D0 RID: 5072
        private int ZombiGoblin;

        // Token: 0x040013D1 RID: 5073
        private int KALE;

        // Token: 0x040013D2 RID: 5074
        private int YANMACANI;

        // Token: 0x040013D3 RID: 5075
        private int DOÐACAKZOMBÝGOBLÝNSAYISI;

        // Token: 0x040013D4 RID: 5076
        private int KIRILANDUVAR;
    }
}
