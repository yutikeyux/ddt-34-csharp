using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.Messions
{
    public class DCR5301 : AMissionControl
    {
        private SimpleBoss KızgınGOBLİN;

        private SimpleNpc TonyAMCA;

        private SimpleNpc ZincirÇekiciKöleler;

        private PhysicalObj ElektrikliAlan;

        private PhysicalObj İlkEkipmanEfekti;

        private PhysicalObj İkinciEkipmanEfekti;

        private PhysicalObj ÜçüncüEkipmanEfekti;

        private int OyunSonu;

        private int KızgınGoblin;

        private int YardımcıKöle;

        private int Zincir;

        private int TonyAmca;

        private int Harita;

        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 1150)
            {
                return 3;
            }
            if (score > 925)
            {
                return 2;
            }
            if (score > 700)
            {
                return 1;
            }
            return 0;
        }

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            base.Game.AddLoadingFile(1, "bombs/56.swf", "tank.resource.bombs.Bomb56");
            base.Game.AddLoadingFile(1, "bombs/72.swf", "tank.resource.bombs.Bomb72");
            base.Game.AddLoadingFile(2, "image/game/effect/5/zap.swf", "asset.game.4.zap");
            base.Game.AddLoadingFile(2, "image/game/effect/5/zap2.swf", "asset.game.4.zap2");
            base.Game.AddLoadingFile(2, "image/game/effect/5/dian.swf", "asset.game.4.dian");
            base.Game.AddLoadingFile(2, "image/game/effect/5/minigun.swf", "asset.game.4.minigun");
            base.Game.AddLoadingFile(2, "image/game/effect/5/jinqud.swf", "asset.game.4.jinqud");
            base.Game.AddLoadingFile(2, "image/game/effect/5/xiaopao.swf", "asset.game.4.xiaopao");
            base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
            base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.gebulinzhihuiguanAsset");
            int[] npcIDleri = new int[4]
            {
                KızgınGoblin,
                YardımcıKöle,
                Zincir,
                TonyAmca
            };
            base.Game.LoadResources(npcIDleri);
            int[] npcIDleri2 = new int[1]
            {
                KızgınGoblin
            };
            base.Game.LoadNpcGameOverResources(npcIDleri2);
            base.Game.SetMap(Harita);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
            İkinciEkipmanEfekti = base.Game.Createlayer(0, 0, "moive", "game.asset.living.BossBgAsset", "out", 1, 1);
            ÜçüncüEkipmanEfekti = base.Game.Createlayer(1172, 587, "front", "game.asset.living.gebulinzhihuiguanAsset", "out", 1, 1);
            LivingConfig livingConfig = base.Game.BaseLivingConfig();
            livingConfig.IsFly = true;
            KızgınGOBLİN = base.Game.CreateBoss(KızgınGoblin, 1484, 750, -1, 1, "born", livingConfig);
            base.Game.SendHideBlood(KızgınGOBLİN, 0);
            livingConfig = base.Game.BaseLivingConfig();
            livingConfig.IsTurn = false;
            TonyAMCA = base.Game.CreateNpc(TonyAmca, 1287, 859, 0, 1, livingConfig);
            base.Game.SendHideBlood(TonyAMCA, 0);
            base.Game.SendObjectFocus(TonyAMCA, 1, 700, 0);
            TonyAMCA.Say("Ooo, bu malzemeler çok işimize yaricak!", 0, 2000);
            TonyAMCA.MoveTo(1388, 867, "walk", 4000, TonyAmcaSaldırısı);
        }

        private void TonyAmcaSaldırısı()
        {
            ElektrikliAlan = base.Game.Createlayer(1470, 822, "", "asset.game.4.jinqud", "", 1, 1);
            İlkEkipmanEfekti = base.Game.Createlayer(TonyAMCA.X, TonyAMCA.Y, "", "asset.game.4.dian", "", 1, 1);
            TonyAMCA.PlayMovie("outA", 500, 2000);
            TonyAMCA.Die(2500);
            KızgınGOBLİN.PlayMovie("in", 3000, 5000);
            İkinciEkipmanEfekti.PlayMovie("in", 5000, 0);
            ÜçüncüEkipmanEfekti.PlayMovie("in", 5200, 0);
            İkinciEkipmanEfekti.PlayMovie("out", 8000, 0);
            ÜçüncüEkipmanEfekti.PlayMovie("out", 8200, 0);
            KızgınGOBLİN.CallFuction(KöleDoğumveÇekim, 10000);
        }

        private void KöleDoğumveÇekim()
        {
            LivingConfig livingConfig = base.Game.BaseLivingConfig();
            livingConfig.IsTurn = false;
            livingConfig.CanTakeDamage = false;
            ZincirÇekiciKöleler = base.Game.CreateNpc(YardımcıKöle, 187, 370, 1, 1, livingConfig);
            base.Game.SendLivingActionMapping(ZincirÇekiciKöleler, "stand", "standA");
            base.Game.SendObjectFocus(ZincirÇekiciKöleler, 1, 700, 0);
            ZincirÇekiciKöleler.PlayMovie("in", 1500, 10000);
            ZincirÇekiciKöleler.PlayMovie("walkA", 9000, 3000);
        }

        private void KuşanılanlarıKaldır()
        {
            if (İlkEkipmanEfekti != null)
            {
                base.Game.RemovePhysicalObj(İlkEkipmanEfekti, true);
            }
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            if (İkinciEkipmanEfekti != null)
            {
                base.Game.RemovePhysicalObj(İkinciEkipmanEfekti, true);
                İkinciEkipmanEfekti = null;
            }
            if (ÜçüncüEkipmanEfekti != null)
            {
                base.Game.RemovePhysicalObj(ÜçüncüEkipmanEfekti, true);
                ÜçüncüEkipmanEfekti = null;
            }
            KuşanılanlarıKaldır();
        }

        public override bool CanGameOver()
        {
            base.CanGameOver();
            if (KızgınGOBLİN != null && !KızgınGOBLİN.IsLiving)
            {
                OyunSonu++;
                return true;
            }
            if (base.Game.TurnIndex > 200)
            {
                return true;
            }
            return false;
        }

        public override int UpdateUIData()
        {
            base.UpdateUIData();
            return OyunSonu;
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            if (KızgınGOBLİN != null && !KızgınGOBLİN.IsLiving)
            {
                base.Game.IsWin = true;
            }
            else
            {
                base.Game.IsWin = false;
            }
        }

        public override void DoOther()
        {
            base.DoOther();
        }

        private void OyunKazanlırsa()
        {
            TonyAMCA = base.Game.CreateNpc(TonyAmca, 243, 368, 0, 1);
            base.Game.SendObjectFocus(TonyAMCA, 1, 0, 500);
            TonyAMCA.Say("İşte şimdi bakabilirim bu EkipmanEfektilara!", 0, 1000);
            TonyAMCA.Say("Haydi diğer etaba geçelim!", 0, 3000, 2000);
        }

        public override void OnShooted()
        {
            base.OnShooted();
            if (KızgınGOBLİN != null && !KızgınGOBLİN.IsLiving)
            {
                int waitTimerLeft = base.Game.GetWaitTimerLeft();
                base.Game.ClearAllChild();
                KızgınGOBLİN.CallFuction(OyunKazanlırsa, waitTimerLeft + 3000);
            }
        }

        public override void OnDied()
        {
            base.OnDied();
        }

        public DCR5301()
        {
            KızgınGoblin = 5301;
            YardımcıKöle = 5302;
            Zincir = 5303;
            TonyAmca = 5304;
            Harita = 1151;
        }
    }
}
