using System;
using System.Collections.Generic;
using System.Drawing;
using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.NPC
{
    // Token: 0x02000116 RID: 278
    public class SimpleCaptainAi : ABrain
    {
        // Token: 0x06000EC1 RID: 3777 RVA: 0x00064327 File Offset: 0x00062527
        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        // Token: 0x06000EC2 RID: 3778 RVA: 0x00064334 File Offset: 0x00062534
        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            base.Body.CurrentDamagePlus = 1f;
            base.Body.CurrentShootMinus = 1f;
            base.Body.SetRect(((SimpleBoss)base.Body).NpcInfo.X, ((SimpleBoss)base.Body).NpcInfo.Y, ((SimpleBoss)base.Body).NpcInfo.Width, ((SimpleBoss)base.Body).NpcInfo.Height);
            bool flag = base.Body.Direction == -1;
            if (flag)
            {
                base.Body.SetRect(((SimpleBoss)base.Body).NpcInfo.X, ((SimpleBoss)base.Body).NpcInfo.Y, ((SimpleBoss)base.Body).NpcInfo.Width, ((SimpleBoss)base.Body).NpcInfo.Height);
            }
            else
            {
                base.Body.SetRect(-((SimpleBoss)base.Body).NpcInfo.X - ((SimpleBoss)base.Body).NpcInfo.Width, ((SimpleBoss)base.Body).NpcInfo.Y, ((SimpleBoss)base.Body).NpcInfo.Width, ((SimpleBoss)base.Body).NpcInfo.Height);
            }
        }

        // Token: 0x06000EC3 RID: 3779 RVA: 0x000644B8 File Offset: 0x000626B8
        public override void OnCreated()
        {
            base.OnCreated();
        }

        // Token: 0x06000EC4 RID: 3780 RVA: 0x000644C4 File Offset: 0x000626C4
        public override void OnStartAttacking()
        {
            base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
            bool flag = false;
            int num = 0;
            foreach (Player allFightPlayer in base.Game.GetAllFightPlayers())
            {
                bool flag2 = allFightPlayer.IsLiving && allFightPlayer.X > 500 && allFightPlayer.X < 1050;
                if (flag2)
                {
                    int num2 = (int)base.Body.Distance(allFightPlayer.X, allFightPlayer.Y);
                    bool flag3 = num2 > num;
                    if (flag3)
                    {
                        num = num2;
                    }
                    flag = true;
                }
            }
            bool flag4 = flag;
            if (flag4)
            {
                this.method_0(500, 1050);
            }
            else
            {
                bool flag5 = this.int_0 == 0;
                if (flag5)
                {
                    this.method_1();
                    this.int_0++;
                }
                else
                {
                    bool flag6 = this.int_0 == 1;
                    if (flag6)
                    {
                        this.method_2();
                        this.int_0++;
                    }
                    else
                    {
                        this.method_3();
                        this.int_0 = 0;
                    }
                }
            }
        }

        // Token: 0x06000EC5 RID: 3781 RVA: 0x00064610 File Offset: 0x00062810
        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

        // Token: 0x06000EC6 RID: 3782 RVA: 0x0006461C File Offset: 0x0006281C
        private void method_0(int int_2, int int_3)
        {
            this.method_5(3);
            int num = base.Game.Random.Next(0, SimpleCaptainAi.Sohbet_7.Length);
            base.Body.Say(SimpleCaptainAi.Sohbet_7[num], 1, 1000);
            base.Body.CurrentDamagePlus = 100f;
            base.Body.PlayMovie("beat2", 3000, 0);
            base.Body.RangeAttacking(int_2, int_3, "cry", 5000, null);
        }

        // Token: 0x06000EC7 RID: 3783 RVA: 0x000646A4 File Offset: 0x000628A4
        private void method_1()
        {
            this.method_5(3);
            base.Body.CurrentDamagePlus = 2f;
            int num = base.Game.Random.Next(0, SimpleCaptainAi.Sohbet_1.Length);
            base.Body.Say(SimpleCaptainAi.Sohbet_1[num], 1, 0);
            base.Body.FallFrom(base.Body.X, 509, null, 1000, 1, 12);
            base.Body.PlayMovie("beat2", 1000, 0);
            base.Body.RangeAttacking(base.Body.X - 1000, base.Body.X + 1000, "cry", 4000, null);
        }

        // Token: 0x06000EC8 RID: 3784 RVA: 0x00064770 File Offset: 0x00062970
        private void method_2()
        {
            this.method_5(3);
            int num = base.Game.Random.Next(0, SimpleCaptainAi.Sohbet_2.Length);
            base.Body.Say(SimpleCaptainAi.Sohbet_2[num], 1, 0);
            int x = base.Game.Random.Next(670, 880);
            int direction = base.Body.Direction;
            base.Body.MoveTo(x, base.Body.Y, "walk", 1000, "", 4, new LivingCallBack(this.method_4));
            base.Body.ChangeDirection(base.Game.FindlivingbyDir(base.Body), 9000);
        }

        // Token: 0x06000EC9 RID: 3785 RVA: 0x00064834 File Offset: 0x00062A34
        private void method_3()
        {
            this.method_5(3);
            base.Body.JumpTo(base.Body.X, base.Body.Y - 300, "Jump", 1000, 1);
            int num = base.Game.Random.Next(0, SimpleCaptainAi.Sohbet_4.Length);
            base.Body.Say(SimpleCaptainAi.Sohbet_4[num], 1, 3300);
            base.Body.PlayMovie("call", 3500, 0);
            base.Body.CallFuction(new LivingCallBack(this.CreateChild), 4000);
        }

        // Token: 0x06000ECA RID: 3786 RVA: 0x000648E4 File Offset: 0x00062AE4
        private void method_4()
        {
            Player player = base.Game.FindRandomPlayer();
            base.Body.SetRect(0, 0, 0, 0);
            bool flag = player.X > base.Body.Y;
            if (flag)
            {
                base.Body.ChangeDirection(1, 500);
            }
            else
            {
                base.Body.ChangeDirection(-1, 500);
            }
            base.Body.CurrentDamagePlus = 1f;
            bool flag2 = player != null;
            if (flag2)
            {
                int x = base.Game.Random.Next(player.X - 50, player.X + 50);
                bool flag3 = base.Body.ShootPoint(x, player.Y, 61, 1000, 10000, 1, 1f, 2200);
                if (flag3)
                {
                    base.Body.PlayMovie("beat", 1700, 0);
                }
                bool flag4 = base.Body.ShootPoint(x, player.Y, 61, 1000, 10000, 1, 1f, 3200);
                if (flag4)
                {
                    base.Body.PlayMovie("beat", 2700, 0);
                }
            }
        }

        // Token: 0x06000ECB RID: 3787 RVA: 0x00064A20 File Offset: 0x00062C20
        private void method_5(int int_2)
        {
            int direction = base.Body.Direction;
            for (int i = 0; i < int_2; i++)
            {
                base.Body.ChangeDirection(-direction, i * 200 + 100);
                base.Body.ChangeDirection(direction, (i + 1) * 100 + i * 200);
            }
        }

        // Token: 0x06000ECC RID: 3788 RVA: 0x00064A7F File Offset: 0x00062C7F
        public void CreateChild()
        {
            ((SimpleBoss)base.Body).CreateChild(this.pembe_bogolar, this.doğumnoktaları, 8, 2, 1);
        }

        // Token: 0x06000ECD RID: 3789 RVA: 0x00064AA4 File Offset: 0x00062CA4
        public SimpleCaptainAi()
        {
            this.pembe_bogolar = 1009;
            this.bogocuknpc = new List<SimpleNpc>();
        }

        // Token: 0x06000ECE RID: 3790 RVA: 0x00064B08 File Offset: 0x00062D08
        static SimpleCaptainAi()
        {
        }

        // Token: 0x0400074D RID: 1869
        private int int_0;

        // Token: 0x0400074E RID: 1870
        public int currentCount;

        // Token: 0x0400074F RID: 1871
        public int Dander;

        // Token: 0x04000750 RID: 1872
        private int pembe_bogolar;

        // Token: 0x04000751 RID: 1873
        public List<SimpleNpc> bogocuknpc;

        // Token: 0x04000752 RID: 1874
        private static string[] Sohbet_1 = new string[]
        {
            "Çok Büyük <br/> Sarsıntı.... <br/> Yıkılıyor ......",
            "Hepiniz kendi ölümünüzü arıyorsunuz.!",
            "Yenilmez süper deprem <br/> ...... GÜM ... GÜM ......"
        };

        // Token: 0x04000753 RID: 1875
        private static string[] Sohbet_2 = new string[]
        {
            "Ateş etmemi izle!.",
            "Yüzünü yok edeceğim!"
        };

        // Token: 0x04000754 RID: 1876
        private static string[] Sohbet_3 = new string[]
        {
            "Cehenneme git tatlım!",
            "Beni yenebileceğini mi düşünüyorsun?",
            "Beni yenmeye çalışmadan önce daha çok pratik yap."
        };

        // Token: 0x04000755 RID: 1877
        private static string[] Sohbet_4 = new string[]
        {
            "Muhafızlar! <br/> beni koruyun! ! ",
            "Merhaba arkadaşlar, Bogolar! <br/> bana yardım edin!"
        };

        // Token: 0x04000756 RID: 1878
        private static string[] Sohbet_5 = new string[]
        {
            "Ah kahretsin ...",
            "Bana vurmaya cesaret mi ediyorsun ?...",
            "Çok acı veriyor, affetmeyeceğim!!!"
        };

        // Token: 0x04000757 RID: 1879
        private static string[] Sohbet_6 = new string[]
        {
            "Vurmak mı? Hadi vurun!!",
            "Haydi daha yükseğe çıkalım! Haydi daha yükseğe zıplayalım!",
            "Yüksek! <br/> Daha da yükseğe çıkalım!!"
        };

        // Token: 0x04000758 RID: 1880
        private static string[] Sohbet_7 = new string[]
        {
            "Bana yaklaşarak hayatta kalabileceğini mi sanıyorsun?",
            "Kendi ölümüne doğru koşan bir aptal.",
            "Neden bana bu kadar yaklaşıyorsun?",
            "Siz küçük veletler benim popoma dokunacak yaşta değilsiniz!!!"
        };

        // Token: 0x04000759 RID: 1881
        private Point[] doğumnoktaları = new Point[]
        {
            new Point(600, 539),
            new Point(950, 539)
        };
    }
}
