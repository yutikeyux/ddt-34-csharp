using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Effects;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.NPC
{
    public class FourNormalCattleBoss : ABrain
    {
        private int morruhlar;

        private PhysicalObj BaltaSaldırısıŞimşekliEfekt;

        private PhysicalObj RuhlarıEmGüçlen;

        private int MorRuhSayısı;

        private int MorRuhNPC;

        private float BuffGücü;

        private int Yorgunluk;

        private bool YorulmuşMinotar;

        private Player oyuncu;

        private float KızdıMino;

        private float BufflıGüçlüMino;

        private int RuhlarınSayıları;

        private string[] İlkAtakSohbetleri;

        private string[] İkinciAtakSohbetleri;

        private string[] YorulmaSohbetleri;

        private string[] DördüncüAtakSohbetleri;

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            lcjwAebwpHw();
            base.Body.CurrentShootMinus = 1f;
            if (!YorulmuşMinotar)
            {
                base.Body.Config.HaveShield = true;
            }
            else
            {
                base.Body.Config.HaveShield = false;
            }
        }

        public override void OnCreated()
        {
            base.OnCreated();
            YorulmuşMinotar = false;
            base.Body.CurrentDamagePlus = 1f;
            BufflıGüçlüMino = 1f;
        }

        public override void OnStartAttacking()
        {
            if (YorulmuşMinotar && RuhlarınSayıları > 0)
            {
                RuhlarınSayıları--;
                if (RuhlarınSayıları <= 0)
                {
                    morruhlar = 3;
                }
                return;
            }
            if (!YorulmuşMinotar)
            {
                bool flag = false;
                foreach (Player allFightPlayer in base.Game.GetAllFightPlayers())
                {
                    if (allFightPlayer.IsLiving && allFightPlayer.X > base.Body.X - 200 && allFightPlayer.X < base.Body.X + 200)
                    {
                        flag = true;
                    }
                }
                if (flag)
                {
                    DördüncüAtak(base.Body.X - 200, base.Body.X + 200);
                    return;
                }
            }
            if (morruhlar == 0)
            {
                if (!YorulmuşMinotar)
                {
                    base.Body.CallFuction(BuffAtağı, 2000);
                    if (((SimpleBoss)base.Body).CurrentLivingNpcNum <= 0)
                    {
                        base.Body.Say("Lửa địa ngục đâu, ra mau.", 0, 5000);
                        base.Body.CallFuction(RuhÇağırımı, 7000);
                    }
                    else
                    {
                        base.Body.CallFuction(İlkAtak, 4000);
                    }
                }
                morruhlar++;
            }
            else if (morruhlar == 1)
            {
                if (!YorulmuşMinotar)
                {
                    base.Body.CallFuction(BuffAtağı, 2000);
                    base.Body.CallFuction(NormalAtak, 4000);
                    morruhlar++;
                }
                else
                {
                    morruhlar = 3;
                }
            }
            else if (morruhlar == 2)
            {
                if (!YorulmuşMinotar)
                {
                    base.Body.CallFuction(BuffAtağı, 2000);
                    base.Body.CallFuction(İlkAtak, 4000);
                }
                morruhlar++;
            }
            else
            {
                if (morruhlar != 3)
                {
                    return;
                }
                if (!YorulmuşMinotar)
                {
                    base.Body.CallFuction(BuffAtağı, 2000);
                    if (((SimpleBoss)base.Body).CurrentLivingNpcNum <= 0)
                    {
                        YorulmuşMinotar = true;
                        RuhlarınSayıları = 2;
                        base.Body.Say("Hơ hơ... Ta bị ốm à??", 0, 2200);
                        base.Body.CallFuction(ÜçüncüAtak, 4000);
                    }
                    else
                    {
                        base.Body.CallFuction(ZıplamaAtağı, 4000);
                    }
                }
                else
                {
                    YorulmuşMinotar = false;
                    BufflıGüçlüMino = 1f;
                    base.Body.CallFuction(AtoBAtağı, 2000);
                }
                morruhlar = 0;
            }
        }

        private void DördüncüAtak(int int_4, int int_5)
        {
            KızdıMino = base.Body.CurrentDamagePlus;
            base.Body.CurrentDamagePlus = 1000f;
            base.Body.ChangeDirection(base.Game.FindlivingbyDir(base.Body), 100);
            ((SimpleBoss)base.Body).RandomSay(DördüncüAtakSohbetleri, 0, 2000, 0);
            base.Body.PlayMovie("beatC", 2000, 0);
            base.Body.PlayMovie("beatE", 5000, 0);
            base.Body.RangeAttacking(int_4, int_5, "cry", 7000, null);
            base.Body.CallFuction(MinotarınDurumAyarı, 8000);
        }

        private void NormalAtak()
        {
            oyuncu = base.Game.FindNearestPlayer(base.Body.X, base.Body.Y);
            if (oyuncu != null)
            {
                base.Body.ChangeDirection(oyuncu, 100);
                base.Body.Say("Tên ranh kia hãy đỡ này!!!", 0, 1000);
                base.Body.PlayMovie("beatA", 1200, 0);
                ((PVEGame)base.Game).SendObjectFocus(oyuncu, 1, 3200, 0);
                base.Body.CallFuction(BaltaSaldırısı, 4000);
                if (base.Body.FindDirection(oyuncu) == -1)
                {
                    base.Body.RangeAttacking(oyuncu.X - 50, base.Body.X, "cry", 4800, null);
                }
                else
                {
                    base.Body.RangeAttacking(base.Body.X, oyuncu.X + 50, "cry", 4800, null);
                }
                base.Body.CallFuction(MinotarınDurumAyarı, 6000);
            }
        }

        private void İlkAtak()
        {
            ((SimpleBoss)base.Body).RandomSay(İlkAtakSohbetleri, 0, 1000, 0);
            base.Body.PlayMovie("beatB", 1000, 0);
            base.Body.RangeAttacking(base.Body.X - 10000, base.Body.X + 10000, "cry", 4100, null);
            foreach (Player allLivingPlayer in base.Game.GetAllLivingPlayers())
            {
                allLivingPlayer.AddEffect(new ReduceStrengthEffect(1, Yorgunluk), 4200);
            }
            base.Body.CallFuction(MinotarınDurumAyarı, 5000);
        }

        private void ÜçüncüAtak()
        {
            base.Body.PlayMovie("beatD", 1000, 0);
            ((SimpleBoss)base.Body).RandomSay(YorulmaSohbetleri, 0, 4100, 0);
            base.Body.PlayMovie("AtoB", 4000, 0);
            base.Body.CallFuction(MinotarınDurumAyarı, 7000);
        }

        private void AtoBAtağı()
        {
            base.Body.PlayMovie("AtoB", 1000, 0);
            ((SimpleBoss)base.Body).RandomSay(İkinciAtakSohbetleri, 0, 2200, 0);
            base.Body.CallFuction(ZıplamaAtağı, 2000);
        }

        private void ZıplamaAtağı()
        {
            oyuncu = base.Game.FindRandomPlayer();
            if (oyuncu != null)
            {
                base.Body.PlayMovie("jump", 500, 0);
                ((PVEGame)base.Game).SendObjectFocus(oyuncu, 1, 2000, 0);
                base.Body.BoltMove(oyuncu.X, oyuncu.Y, 2500);
                base.Body.PlayMovie("fall", 2600, 0);
                base.Body.RangeAttacking(oyuncu.X - 100, oyuncu.X + 100, "cry", 3000, null);
                base.Body.CallFuction(MinotarınDurumAyarı, 4000);
            }
        }

        private void BuffAtağı()
        {
            BufflıGüçlüMino += BuffGücü;
            base.Body.CurrentDamagePlus = BufflıGüçlüMino;
            RuhlarıEmGüçlen = ((PVEGame)base.Game).Createlayer(base.Body.X, base.Body.Y - 60, "", "game.crazytank.assetmap.Buff_powup", "", 1, 0);
        }

        private void BaltaSaldırısı()
        {
            if (oyuncu != null)
            {
                BaltaSaldırısıŞimşekliEfekt = ((PVEGame)base.Game).Createlayer(oyuncu.X, oyuncu.Y, "", "asset.game.4.blade", "", 1, 0);
            }
        }

        private void lcjwAebwpHw()
        {
            if (RuhlarıEmGüçlen != null)
            {
                base.Game.RemovePhysicalObj(RuhlarıEmGüçlen, true);
            }
            if (BaltaSaldırısıŞimşekliEfekt != null)
            {
                base.Game.RemovePhysicalObj(BaltaSaldırısıŞimşekliEfekt, true);
            }
        }

        private void RuhÇağırımı()
        {
            LivingConfig livingConfig = ((PVEGame)base.Game).BaseLivingConfig();
            livingConfig.IsFly = true;
            for (int i = 0; i < MorRuhSayısı; i++)
            {
                int x = base.Game.Random.Next(350, 1300);
                int y = base.Game.Random.Next(100, 700);
                ((SimpleBoss)base.Body).CreateChild(MorRuhNPC, x, y, showBlood: true, livingConfig);
            }
        }

        private void method_9()
        {
            ((SimpleBoss)base.Body).RemoveAllChild();
        }

        private void MinotarınDurumAyarı()
        {
            if (YorulmuşMinotar)
            {
                ((PVEGame)base.Game).SendLivingActionMapping(base.Body, "stand", "standB");
            }
            else
            {
                ((PVEGame)base.Game).SendLivingActionMapping(base.Body, "stand", "standA");
            }
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

        public FourNormalCattleBoss()
        {
            MorRuhSayısı = 3;
            MorRuhNPC = 4107;
            BuffGücü = 0.5f;
            Yorgunluk = 50;
            İlkAtakSohbetleri = new string[4]
            {
                "Buna dayanabilecek misin?",
                "Sizi gidi aptallar! Mahvolacaksınız!",
                "Beni nasıl durdurabilirsiniz ki?",
                "Bu zevki uzun zamandır tatmamıştım!"
            };
            İkinciAtakSohbetleri = new string[4]
            {
                "Bakalım kaçmak için nereye gideceksin?",
                "Bana saldırmaya nasıl cürret edersin?",
                "Bakalım nereye kadar kaçabilirsin?",
                "Bu yaptığınız saldırıyı affetmeyeceğim!"
            };
            YorulmaSohbetleri = new string[3]
            {
                "Yoruluyorum!",
                "Sırtıma yük bindi bu neymiş böyle!",
                "Çok Yoruldum..."
            };
            DördüncüAtakSohbetleri = new string[4]
            {
                "Sizi yolcu edeyim hemen!",
                "Kesinlikle öldün tatlım!",
                "Ölmeseydin şaşırırdım!",
                "Tabutunu şimdiden hazırlayalım mı?"
            };
        }
    }
}