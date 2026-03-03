using System;
using System.Collections.Generic;
using System.Linq;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Effects;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.NPC
{
    // Token: 0x0200007A RID: 122
    public class FourTerrorCattleBoss : ABrain
    {
        // Token: 0x0600071A RID: 1818 RVA: 0x0002F88F File Offset: 0x0002DA8F
        public override void OnCreated()
        {
            base.OnCreated();
            this.ResetBossState();
        }

        // Token: 0x0600071B RID: 1819 RVA: 0x0002F8A0 File Offset: 0x0002DAA0
        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            this.CleanupEffects();
            base.Body.CurrentShootMinus = 1f;
            base.Body.Config.HaveShield = !this.YorulmuşMinotar;
        }

        // Token: 0x0600071C RID: 1820 RVA: 0x0002F8DC File Offset: 0x0002DADC
        public override void OnStartAttacking()
        {
            bool flag = this.YorulmuşMinotar && this.RuhlarınSayıları > 0;
            if (flag)
            {
                this.RuhlarınSayıları--;
                bool flag2 = this.RuhlarınSayıları <= 0;
                if (flag2)
                {
                    this.morruhlar = 3;
                }
            }
            else
            {
                bool flag3 = !this.YorulmuşMinotar && this.IsPlayerNearby();
                if (flag3)
                {
                    this.DördüncüAtak(base.Body.X - 200, base.Body.X + 200);
                }
                else
                {
                    switch (this.morruhlar)
                    {
                        case 0:
                            this.HandlePhaseZero();
                            break;
                        case 1:
                            {
                                bool flag4 = !this.YorulmuşMinotar;
                                if (flag4)
                                {
                                    this.HandlePhaseOne();
                                }
                                else
                                {
                                    this.morruhlar = 3;
                                }
                                break;
                            }
                        case 2:
                            {
                                bool flag5 = !this.YorulmuşMinotar;
                                if (flag5)
                                {
                                    this.HandlePhaseTwo();
                                }
                                this.morruhlar = 3;
                                break;
                            }
                        case 3:
                            this.HandlePhaseThree();
                            this.morruhlar = 0;
                            break;
                    }
                }
            }
        }

        // Token: 0x0600071D RID: 1821 RVA: 0x0002F9EC File Offset: 0x0002DBEC
        private void HandlePhaseZero()
        {
            base.Body.CallFuction(delegate ()
            {
                this.BuffAtağı();
                bool flag = ((SimpleBoss)base.Body).CurrentLivingNpcNum <= 0;
                if (flag)
                {
                    base.Body.Say("Nefret Ateşlerimmm!!! Nerede kaldınız? Çıkın ortaya!", 0, 5000);
                    base.Body.CallFuction(new LivingCallBack(this.RuhÇağırımı), 2000);
                }
                else
                {
                    base.Body.CallFuction(new LivingCallBack(this.İlkAtak), 2000);
                }
            }, 2000);
            this.morruhlar++;
        }

        // Token: 0x0600071E RID: 1822 RVA: 0x0002FA1A File Offset: 0x0002DC1A
        private void HandlePhaseOne()
        {
            base.Body.CallFuction(delegate ()
            {
                this.BuffAtağı();
                base.Body.CallFuction(new LivingCallBack(this.NormalAtak), 2000);
                this.morruhlar++;
            }, 2000);
        }

        // Token: 0x0600071F RID: 1823 RVA: 0x0002FA3A File Offset: 0x0002DC3A
        private void HandlePhaseTwo()
        {
            base.Body.CallFuction(delegate ()
            {
                this.BuffAtağı();
                base.Body.CallFuction(new LivingCallBack(this.İlkAtak), 2000);
            }, 2000);
        }

        // Token: 0x06000720 RID: 1824 RVA: 0x0002FA5A File Offset: 0x0002DC5A
        private void HandlePhaseThree()
        {
            base.Body.CallFuction(delegate ()
            {
                this.BuffAtağı();
                bool flag = ((SimpleBoss)base.Body).CurrentLivingNpcNum <= 0;
                if (flag)
                {
                    this.YorulmuşMinotar = true;
                    this.RuhlarınSayıları = 2;
                    base.Body.Say("Ruhlarr?? Nereye kayboldunuz?!!", 0, 2200);
                    base.Body.CallFuction(new LivingCallBack(this.ÜçüncüAtak), 2000);
                }
                else
                {
                    base.Body.CallFuction(new LivingCallBack(this.FinalAbsorbAttack), 2000);
                }
            }, 2000);
        }

        // Token: 0x06000721 RID: 1825 RVA: 0x0002FA7C File Offset: 0x0002DC7C
        private void FinalAbsorbAttack()
        {
            base.Body.Say("Ruhlarım bana dönün ve güç verin!", 0, 0);
            List<Living> souls = (from l in base.Game.GetLivedLivings()
                                  where l is SimpleNpc && (l as SimpleNpc).NpcInfo.ID == this.MorRuhNPC
                                  select l).ToList<Living>();
            foreach (Living soul in souls)
            {
                soul.MoveTo(base.Body.X, base.Body.Y, "fly", 1000, 30);
                ((PVEGame)base.Game).SendRemoveLiving(this.MorRuhNPC);
            }
            base.Body.CallFuction(delegate ()
            {
                this.RuhlarıEmGüçlen = ((PVEGame)base.Game).Createlayer(base.Body.X, base.Body.Y - 60, "", "game.crazytank.assetmap.Buff_powup", "", 1, 0);
                this.BufflıGüçlüMino += this.BuffGücü;
                base.Body.CurrentDamagePlus = this.BufflıGüçlüMino;
                base.Body.PlayMovie("beatC", 5000, 1000);
                base.Body.Say("Ruhlar gücümü artırdı!", 0, 0);
                base.Body.CallFuction(delegate ()
                {
                    this.ZıplamaAtağı();
                    base.Body.CallFuction(new LivingCallBack(this.EkRuhsuzAtak), 5000);
                }, 3000);
            }, 5500);
        }

        // Token: 0x06000722 RID: 1826 RVA: 0x0002FB5C File Offset: 0x0002DD5C
        private void EkRuhsuzAtak()
        {
            base.Body.BoltMove(600, 400, 2000);
            base.Body.Say("Ruhlar olmadan da sizi ezerim!", 0, 0);
            base.Body.PlayMovie("beatB", 2500, 0);
            base.Body.RangeAttacking(400, 800, "cry", 3000, null);
            base.Body.CallFuction(new LivingCallBack(this.MinotarınDurumAyarı), 4000);
        }

        // Token: 0x06000723 RID: 1827 RVA: 0x0002FBF0 File Offset: 0x0002DDF0
        private void DördüncüAtak(int minX, int maxX)
        {
            base.Body.CurrentDamagePlus = 1000f;
            base.Body.ChangeDirection(base.Game.FindlivingbyDir(base.Body), 100);
            ((SimpleBoss)base.Body).RandomSay(this.DördüncüAtakSohbetleri, 0, 2000, 0);
            base.Body.PlayMovie("beatE", 5000, 0);
            base.Body.RangeAttacking(minX, maxX, "cry", 7000, null);
            base.Body.CallFuction(new LivingCallBack(this.MinotarınDurumAyarı), 8000);
        }

        // Token: 0x06000724 RID: 1828 RVA: 0x0002FC98 File Offset: 0x0002DE98
        private void NormalAtak()
        {
            Player oyuncu = base.Game.FindNearestPlayer(base.Body.X, base.Body.Y);
            bool flag = oyuncu != null;
            if (flag)
            {
                base.Body.ChangeDirection(oyuncu, 100);
                base.Body.Say("Baltamın tadına bak!!", 0, 1000);
                base.Body.PlayMovie("beatA", 1200, 0);
                ((PVEGame)base.Game).SendObjectFocus(oyuncu, 1, 3200, 0);
                base.Body.CallFuction(new LivingCallBack(this.BaltaSaldırısı), 4000);
                int leftX = (base.Body.FindDirection(oyuncu) == -1) ? (oyuncu.X - 50) : base.Body.X;
                int rightX = (base.Body.FindDirection(oyuncu) == -1) ? base.Body.X : (oyuncu.X + 50);
                base.Body.RangeAttacking(leftX, rightX, "cry", 4800, null);
                base.Body.CallFuction(new LivingCallBack(this.MinotarınDurumAyarı), 6000);
            }
        }

        // Token: 0x06000725 RID: 1829 RVA: 0x0002FDCC File Offset: 0x0002DFCC
        private void İlkAtak()
        {
            ((SimpleBoss)base.Body).RandomSay(this.İlkAtakSohbetleri, 0, 1000, 0);
            base.Body.PlayMovie("beatB", 1000, 0);
            base.Body.RangeAttacking(base.Body.X - 10000, base.Body.X + 10000, "cry", 4100, null);
            foreach (Player player in base.Game.GetAllLivingPlayers())
            {
                player.AddEffect(new ReduceStrengthEffect(2, this.Yorgunluk), 4200);
                player.AddEffect(new ContinueReduceBloodEffect(2, 200, player), 4200);
            }
            base.Body.CallFuction(new LivingCallBack(this.MinotarınDurumAyarı), 5000);
        }

        // Token: 0x06000726 RID: 1830 RVA: 0x0002FEE0 File Offset: 0x0002E0E0
        private void ÜçüncüAtak()
        {
            base.Body.PlayMovie("beatD", 1000, 0);
            ((SimpleBoss)base.Body).RandomSay(this.YorulmaSohbetleri, 0, 4100, 0);
            base.Body.PlayMovie("AtoB", 4000, 0);
            base.Body.CallFuction(new LivingCallBack(this.MinotarınDurumAyarı), 7000);
        }

        // Token: 0x06000727 RID: 1831 RVA: 0x0002FF58 File Offset: 0x0002E158
        private void ZıplamaAtağı()
        {
            Player oyuncu = base.Game.FindRandomPlayer();
            bool flag = oyuncu != null;
            if (flag)
            {
                base.Body.PlayMovie("jump", 500, 0);
                base.Body.Say("Kaç benden <span class=\"red\">" + oyuncu.Name + "</span>!", 0, 1000);
                ((PVEGame)base.Game).SendObjectFocus(oyuncu, 1, 2000, 0);
                base.Body.ChangeDirection(base.Game.FindlivingbyDir(base.Body), 100);
                base.Body.BoltMove(oyuncu.X, oyuncu.Y, 2500);
                base.Body.PlayMovie("fall", 2600, 0);
                base.Body.RangeAttacking(oyuncu.X - 100, oyuncu.X + 100, "cry", 3000, null);
                base.Body.CallFuction(new LivingCallBack(this.MinotarınDurumAyarı), 4000);
            }
        }

        // Token: 0x06000728 RID: 1832 RVA: 0x0003006F File Offset: 0x0002E26F
        private void BuffAtağı()
        {
            this.BufflıGüçlüMino += this.BuffGücü;
            base.Body.CurrentDamagePlus = this.BufflıGüçlüMino;
        }

        // Token: 0x06000729 RID: 1833 RVA: 0x00030098 File Offset: 0x0002E298
        private void BaltaSaldırısı()
        {
            Player targetPlayer = base.Game.FindNearestPlayer(base.Body.X, base.Body.Y);
            bool flag = targetPlayer != null;
            if (flag)
            {
                this.BaltaSaldırısıŞimşekliEfekt = ((PVEGame)base.Game).Createlayer(targetPlayer.X, targetPlayer.Y, "", "asset.game.4.blade", "", 1, 0);
            }
        }

        // Token: 0x0600072A RID: 1834 RVA: 0x00030108 File Offset: 0x0002E308
        private void RuhÇağırımı()
        {
            LivingConfig config = ((PVEGame)base.Game).BaseLivingConfig();
            config.IsFly = true;
            config.IsTurn = false;
            config.CanTakeDamage = true;
            for (int i = 0; i < this.MorRuhSayısı; i++)
            {
                int x = base.Game.Random.Next(350, 1300);
                int y = base.Game.Random.Next(100, 700);
                ((SimpleBoss)base.Body).CreateChild(this.MorRuhNPC, x, y, 1, false, config);
            }
        }

        // Token: 0x0600072B RID: 1835 RVA: 0x000301A8 File Offset: 0x0002E3A8
        private void MinotarınDurumAyarı()
        {
            string standAction = this.YorulmuşMinotar ? "standB" : "standA";
            ((PVEGame)base.Game).SendLivingActionMapping(base.Body, "stand", standAction);
            base.Body.ChangeDirection(base.Game.FindlivingbyDir(base.Body), 100);
        }

        // Token: 0x0600072C RID: 1836 RVA: 0x00030208 File Offset: 0x0002E408
        private bool IsPlayerNearby()
        {
            return base.Game.GetAllFightPlayers().Any((Player p) => p.IsLiving && p.X > base.Body.X - 200 && p.X < base.Body.X + 200);
        }

        // Token: 0x0600072D RID: 1837 RVA: 0x00030238 File Offset: 0x0002E438
        private void CleanupEffects()
        {
            bool flag = this.RuhlarıEmGüçlen != null;
            if (flag)
            {
                base.Game.RemovePhysicalObj(this.RuhlarıEmGüçlen, true);
                this.RuhlarıEmGüçlen = null;
            }
            bool flag2 = this.BaltaSaldırısıŞimşekliEfekt != null;
            if (flag2)
            {
                base.Game.RemovePhysicalObj(this.BaltaSaldırısıŞimşekliEfekt, true);
                this.BaltaSaldırısıŞimşekliEfekt = null;
            }
        }

        // Token: 0x0600072E RID: 1838 RVA: 0x00030298 File Offset: 0x0002E498
        private void ResetBossState()
        {
            this.YorulmuşMinotar = false;
            this.morruhlar = 0;
            this.RuhlarınSayıları = 0;
            this.BufflıGüçlüMino = 1f;
            base.Body.CurrentDamagePlus = 1f;
        }

        // Token: 0x0600072F RID: 1839 RVA: 0x000302CB File Offset: 0x0002E4CB
        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

        // Token: 0x06000730 RID: 1840 RVA: 0x000302D8 File Offset: 0x0002E4D8
        public FourTerrorCattleBoss()
        {
        }

        // Token: 0x040003D9 RID: 985
        private int morruhlar;

        // Token: 0x040003DA RID: 986
        private bool YorulmuşMinotar;

        // Token: 0x040003DB RID: 987
        private int RuhlarınSayıları;

        // Token: 0x040003DC RID: 988
        private float BufflıGüçlüMino = 1f;

        // Token: 0x040003DD RID: 989
        private readonly int MorRuhSayısı = 3;

        // Token: 0x040003DE RID: 990
        private readonly int MorRuhNPC = 4307;

        // Token: 0x040003DF RID: 991
        private readonly float BuffGücü = 0.5f;

        // Token: 0x040003E0 RID: 992
        private readonly int Yorgunluk = 50;

        // Token: 0x040003E1 RID: 993
        private PhysicalObj BaltaSaldırısıŞimşekliEfekt;

        // Token: 0x040003E2 RID: 994
        private PhysicalObj RuhlarıEmGüçlen;

        // Token: 0x040003E3 RID: 995
        private readonly string[] İlkAtakSohbetleri = new string[]
        {
            "Buna dayanabilecek misin?",
            "Sizi gidi aptallar! Mahvolacaksınız!",
            "Beni nasıl durdurabilirsiniz ki?",
            "Bu zevki uzun zamandır tatmamıştım!"
        };

        // Token: 0x040003E4 RID: 996
        private readonly string[] İkinciAtakSohbetleri = new string[]
        {
            "Bakalım kaçmak için nereye gideceksin?",
            "Bana saldırmaya nasıl cürret edersin?",
            "Bakalım nereye kadar kaçabilirsin?",
            "Bu yaptığınız saldırıyı affetmeyeceğim!"
        };

        // Token: 0x040003E5 RID: 997
        private readonly string[] YorulmaSohbetleri = new string[]
        {
            "Yoruluyorum!",
            "Sırtıma yük bindi bu neymiş böyle!",
            "Çok Yoruldum..."
        };

        // Token: 0x040003E6 RID: 998
        private readonly string[] DördüncüAtakSohbetleri = new string[]
        {
            "Sizi yolcu edeyim hemen!",
            "Kesinlikle öldün tatlım!",
            "Ölmeseydin şaşırırdım!",
            "Tabutunu şimdiden hazırlayalım mı?"
        };
    }
}
