using System;
using System.Collections.Generic;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.NPC
{
    // Token: 0x02000100 RID: 256
    public class SeventhSimpleHouseAi : ABrain
    {
        // Token: 0x06000D95 RID: 3477 RVA: 0x0005BC4F File Offset: 0x00059E4F
        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        // Token: 0x06000D96 RID: 3478 RVA: 0x0005BC5C File Offset: 0x00059E5C
        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            bool flag = this.int_0 == 0 && base.Body.Blood < base.Body.MaxBlood / 2;
            if (flag)
            {
                base.Body.PlayMovie("toA", 0, 1200);
            }
            this.method_0();
            base.Body.CurrentDamagePlus = 1f;
            base.Body.CurrentShootMinus = 1f;
        }

        // Token: 0x06000D97 RID: 3479 RVA: 0x0005BCDA File Offset: 0x00059EDA
        public override void OnCreated()
        {
            base.OnCreated();
        }

        // Token: 0x06000D98 RID: 3480 RVA: 0x0005BCE4 File Offset: 0x00059EE4
        public override void OnStartAttacking()
        {
            bool flag = false;
            foreach (Player allFightPlayer in base.Game.GetAllFightPlayers())
            {
                bool flag2 = allFightPlayer.IsLiving && allFightPlayer.X > base.Body.X - 400 && allFightPlayer.X < base.Body.X + 400;
                if (flag2)
                {
                    flag = true;
                }
            }
            bool flag3 = flag;
            if (flag3)
            {
                this.KillAttack(base.Body.X - 400, base.Body.X + 400);
            }
            else
            {
                this.int_5 = 1;
                this.int_6 = 4;
                this.CreateNpc1();
                this.CreateNpc2();
            }
        }

        // Token: 0x06000D99 RID: 3481 RVA: 0x0005BDD0 File Offset: 0x00059FD0
        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

        // Token: 0x06000D9A RID: 3482 RVA: 0x0005BDDC File Offset: 0x00059FDC
        private void method_0()
        {
            bool flag = this.int_0 == 0;
            if (flag)
            {
                base.Body.PlayMovie("stand", 2000, 0);
            }
            else
            {
                base.Body.PlayMovie("standA", 2000, 0);
            }
        }

        // Token: 0x06000D9B RID: 3483 RVA: 0x0005BE2B File Offset: 0x0005A02B
        public void KillAttack(int fx, int tx)
        {
            base.Body.RangeAttacking(fx, tx, "cry", 1000, null);
        }

        // Token: 0x06000D9C RID: 3484 RVA: 0x0005BE48 File Offset: 0x0005A048
        public void CreateNpc1()
        {
            bool flag = this.int_5 > this.int_3;
            if (flag)
            {
                this.int_5 = this.int_3;
            }
            int num = this.int_3 - base.Game.GetLivedNpcs(this.int_2).Count;
            bool flag2 = this.int_5 > num;
            if (flag2)
            {
                this.int_5 = num;
            }
            bool flag3 = this.int_5 > 0;
            if (flag3)
            {
                for (int i = 0; i < this.int_5; i++)
                {
                    ((SimpleBoss)base.Body).CreateChild(this.int_2, 596, 955, 1, 1, true, ((PVEGame)base.Game).BaseLivingConfig());
                }
            }
        }

        // Token: 0x06000D9D RID: 3485 RVA: 0x0005BF0C File Offset: 0x0005A10C
        public void CreateNpc2()
        {
            bool flag = this.int_6 > this.int_4;
            if (flag)
            {
                this.int_6 = this.int_4;
            }
            int num = this.int_4 - base.Game.GetLivedNpcs(this.int_1).Count;
            bool flag2 = this.int_6 > num;
            if (flag2)
            {
                this.int_6 = num;
            }
            bool flag3 = this.int_6 > 0;
            if (flag3)
            {
                for (int i = 0; i < this.int_6; i++)
                {
                    ((SimpleBoss)base.Body).CreateChild(this.int_1, 792 + i * 50, 950, 1, 1, true, ((PVEGame)base.Game).BaseLivingConfig());
                }
            }
        }

        // Token: 0x06000D9E RID: 3486 RVA: 0x0005BFD4 File Offset: 0x0005A1D4
        public SeventhSimpleHouseAi()
        {
            this.list_0 = new List<SimpleNpc>();
            this.int_1 = 7022;
            this.int_2 = 7021;
            this.int_3 = 5;
            this.int_4 = 15;
        }

        // Token: 0x040006E3 RID: 1763
        private int int_0;

        // Token: 0x040006E4 RID: 1764
        private List<SimpleNpc> list_0;

        // Token: 0x040006E5 RID: 1765
        private int int_1;

        // Token: 0x040006E6 RID: 1766
        private int int_2;

        // Token: 0x040006E7 RID: 1767
        private int int_3;

        // Token: 0x040006E8 RID: 1768
        private int int_4;

        // Token: 0x040006E9 RID: 1769
        private int int_5;

        // Token: 0x040006EA RID: 1770
        private int int_6;
    }
}
