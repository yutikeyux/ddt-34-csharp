using System;
using System.Collections.Generic;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.NPC
{
    // Token: 0x020000F7 RID: 247
    public class SeventhNormalHouseAi : ABrain
    {
        // Token: 0x06000D24 RID: 3364 RVA: 0x000589F7 File Offset: 0x00056BF7
        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        // Token: 0x06000D25 RID: 3365 RVA: 0x00058A04 File Offset: 0x00056C04
        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            bool flag = this.int_0 == 0 && base.Body.Blood < base.Body.MaxBlood / 2;
            if (flag)
            {
                base.Body.PlayMovie("toA", 0, 0);
            }
            if(this.int_0 == 0 && base.Body.Blood < base.Body.MaxBlood /3)
            {
                base.Body.PlayMovie("toB", 0, 0);
            }
            this.method_0();
            base.Body.CurrentDamagePlus = 1f;
            base.Body.CurrentShootMinus = 1f;
        }

        // Token: 0x06000D26 RID: 3366 RVA: 0x00058A82 File Offset: 0x00056C82
        public override void OnCreated()
        {
            base.OnCreated();
        }

        // Token: 0x06000D27 RID: 3367 RVA: 0x00058A8C File Offset: 0x00056C8C
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

        // Token: 0x06000D28 RID: 3368 RVA: 0x00058B78 File Offset: 0x00056D78
        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

        // Token: 0x06000D29 RID: 3369 RVA: 0x00058B84 File Offset: 0x00056D84
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

        // Token: 0x06000D2A RID: 3370 RVA: 0x00058BD3 File Offset: 0x00056DD3
        public void KillAttack(int fx, int tx)
        {
            base.Body.RangeAttacking(fx, tx, "cry", 1000, null);
        }

        // Token: 0x06000D2B RID: 3371 RVA: 0x00058BF0 File Offset: 0x00056DF0
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

        // Token: 0x06000D2C RID: 3372 RVA: 0x00058CB4 File Offset: 0x00056EB4
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

        // Token: 0x06000D2D RID: 3373 RVA: 0x00058D7C File Offset: 0x00056F7C
        public SeventhNormalHouseAi()
        {
            this.list_0 = new List<SimpleNpc>();
            this.int_1 = 7122;
            this.int_2 = 7121;
            this.int_3 = 5;
            this.int_4 = 15;
        }

        // Token: 0x040006A9 RID: 1705
        private int int_0;

        // Token: 0x040006AA RID: 1706
        private List<SimpleNpc> list_0;

        // Token: 0x040006AB RID: 1707
        private int int_1;

        // Token: 0x040006AC RID: 1708
        private int int_2;

        // Token: 0x040006AD RID: 1709
        private int int_3;

        // Token: 0x040006AE RID: 1710
        private int int_4;

        // Token: 0x040006AF RID: 1711
        private int int_5;

        // Token: 0x040006B0 RID: 1712
        private int int_6;
    }
}
