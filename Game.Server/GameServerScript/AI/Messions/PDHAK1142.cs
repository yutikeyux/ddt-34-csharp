using System;
using System.Collections.Generic;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Effects;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.Messions
{
    // Token: 0x020002BA RID: 698
    public class PDHAK1142 : AMissionControl
    {
        // Token: 0x170000C9 RID: 201
        // (get) Token: 0x060022CB RID: 8907 RVA: 0x000FF41C File Offset: 0x000FD61C
        // (set) Token: 0x060022CC RID: 8908 RVA: 0x000FF434 File Offset: 0x000FD634
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

        // Token: 0x170000CA RID: 202
        // (get) Token: 0x060022CD RID: 8909 RVA: 0x000FF440 File Offset: 0x000FD640
        // (set) Token: 0x060022CE RID: 8910 RVA: 0x000FF458 File Offset: 0x000FD658
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

        // Token: 0x060022CF RID: 8911 RVA: 0x000FF464 File Offset: 0x000FD664
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

        // Token: 0x060022D0 RID: 8912 RVA: 0x000FF4B4 File Offset: 0x000FD6B4
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

        // Token: 0x060022D1 RID: 8913 RVA: 0x000FF530 File Offset: 0x000FD730
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

        // Token: 0x060022D2 RID: 8914 RVA: 0x000FF5BD File Offset: 0x000FD7BD
        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
        }

        // Token: 0x060022D3 RID: 8915 RVA: 0x000FF5C8 File Offset: 0x000FD7C8
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

        // Token: 0x060022D4 RID: 8916 RVA: 0x000FF6C4 File Offset: 0x000FD8C4
        private void BombayýÇaðýr()
        {
            LivingConfig livingConfig = base.Game.BaseLivingConfig();
            livingConfig.IsHelper = true;
            this.FIÇIBOMBA = base.Game.CreateBoss(this.FýçýBomba, 321, 746, 1, 0, "", livingConfig);
            this.FIÇIBOMBA.AddEffect(new ContinueReduceBloodEffect(2, this.YANMACANI, this.FIÇIBOMBA), 0);
        }

        // Token: 0x060022D5 RID: 8917 RVA: 0x000FF730 File Offset: 0x000FD930
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

        // Token: 0x060022D6 RID: 8918 RVA: 0x000FF800 File Offset: 0x000FDA00
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

        // Token: 0x060022D7 RID: 8919 RVA: 0x000FF864 File Offset: 0x000FDA64
        public override int UpdateUIData()
        {
            base.UpdateUIData();
            return this.KIRILANDUVAR;
        }

        // Token: 0x060022D8 RID: 8920 RVA: 0x000FF884 File Offset: 0x000FDA84
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

        // Token: 0x060022D9 RID: 8921 RVA: 0x000FF8D8 File Offset: 0x000FDAD8
        public PDHAK1142()
        {
            this.NpcListesi = new List<SimpleNpc>();
            this.FýçýBomba = 4201;
            this.ZombiGoblin = 4203;
            this.KALE = 4204;
            this.YANMACANI = 500;
            this.DOÐACAKZOMBÝGOBLÝNSAYISI = 1;
        }

        // Token: 0x040013E5 RID: 5093
        private List<SimpleNpc> NpcListesi;

        // Token: 0x040013E6 RID: 5094
        private SimpleBoss KALEDUVARI;

        // Token: 0x040013E7 RID: 5095
        private SimpleBoss FIÇIBOMBA;

        // Token: 0x040013E8 RID: 5096
        private int FýçýBomba;

        // Token: 0x040013E9 RID: 5097
        private int ZombiGoblin;

        // Token: 0x040013EA RID: 5098
        private int KALE;

        // Token: 0x040013EB RID: 5099
        private int YANMACANI;

        // Token: 0x040013EC RID: 5100
        private int DOÐACAKZOMBÝGOBLÝNSAYISI;

        // Token: 0x040013ED RID: 5101
        private int KIRILANDUVAR;
    }
}
