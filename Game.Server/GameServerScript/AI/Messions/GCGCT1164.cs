using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
//civciv normal etap 4
namespace GameServerScript.AI.Messions
{
    public class GCGCT1164 : AMissionControl
    {
        private PhysicalObj PisBenBenKaynak1;

        private PhysicalObj PisBenBenKaynak2;

        private PhysicalObj PisBenBenKaynak3;

        private SimpleBoss PisBenBenKaynak;

        private SimpleNpc KafestekiCivcivler;

        private int int_0;

        private int PisBenBen;

        private int Kafes;

        private int ZehirliYumurtalar;

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
            base.Game.AddLoadingFile(1, "bombs/83.swf", "tank.resource.bombs.Bomb83");
            base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
            base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.choudanbenbenAsset");
            base.Game.AddLoadingFile(2, "image/game/effect/7/choud.swf", "asset.game.seven.choud");
            base.Game.AddLoadingFile(2, "image/game/effect/7/jinqucd.swf", "asset.game.seven.jinqucd");
            base.Game.AddLoadingFile(2, "image/game/effect/7/du.swf", "asset.game.seven.du");
            int[] npcIds = new int[3]
            {
                PisBenBen,
                Kafes,
                ZehirliYumurtalar
            };
            base.Game.LoadResources(npcIds);
            int[] npcIds2 = new int[1]
            {
                PisBenBen
            };
            base.Game.LoadNpcGameOverResources(npcIds2);
            base.Game.SetMap(1164);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
            PisBenBenKaynak1 = base.Game.Createlayer(0, 0, "kingmoive", "game.asset.living.BossBgAsset", "out", 1, 1);
            PisBenBenKaynak2 = base.Game.Createlayer(300, 595, "font", "game.asset.living.choudanbenbenAsset", "out", 1, 1);
            PisBenBenKaynak3 = base.Game.Createlayer(2170, 636, "", "game.living.Living178", "stand", 1, 1);
            LivingConfig livingConfig = base.Game.BaseLivingConfig();
            livingConfig.IsTurn = false;
            livingConfig.IsFly = true;
            KafestekiCivcivler = base.Game.CreateNpc(Kafes, 1920, 900, 1, -1, livingConfig);
            KafestekiCivcivler.PlayMovie("stand", 1000, 0);
            KafestekiCivcivler.Say("Zehirlenmek istemiyoruz. Yardım edin!! Yardım edin!!", 0, 2000);
            KafestekiCivcivler.CallFuction(PisBenBenİlkAtak, 4000);
        }

        private void PisBenBenİlkAtak()
        {
            PisBenBenKaynak = base.Game.CreateBoss(PisBenBen, 200, 590, 1, 1, "born");
            PisBenBenKaynak.SetRelateDemagemRect(PisBenBenKaynak.NpcInfo.X, PisBenBenKaynak.NpcInfo.Y, PisBenBenKaynak.NpcInfo.Width, PisBenBenKaynak.NpcInfo.Height);
            PisBenBenKaynak1.PlayMovie("in", 1000, 0);
            PisBenBenKaynak2.PlayMovie("in", 2000, 0);
            PisBenBenKaynak1.PlayMovie("out", 5000, 0);
            PisBenBenKaynak2.PlayMovie("out", 5400, 0);
            PisBenBenKaynak.Say("Civcivleri kurtarmaya mı çalışıyorsunuz? O kadar kolay değil.", 0, 6000);
            PisBenBenKaynak.PlayMovie("skill", 8000, 0);
            PisBenBenKaynak.Say("Madem bu kadar yeteneklisin, kalkanı kır da görelim!", 0, 8000);
            base.Game.SendObjectFocus(KafestekiCivcivler, 1, 9000, 0);
            KafestekiCivcivler.PlayMovie("standB", 10000, 0);
            KafestekiCivcivler.Config.CanTakeDamage = false;
            KafestekiCivcivler.Say("Kalkanı kırmak için Pis Benben'in ölmesi gerekiyor!", 0, 11000);
            base.Game.SendObjectFocus(PisBenBenKaynak, 1, 13000, 0);
            PisBenBenKaynak.Say("Hepinizi kafesime koyup zehirleyeceğim!", 0, 14000, 3000);
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            if (base.Game.TurnIndex > 1)
            {
                if (PisBenBenKaynak1 != null)
                {
                    base.Game.RemovePhysicalObj(PisBenBenKaynak1, sendToClient: true);
                    PisBenBenKaynak1 = null;
                }
                if (PisBenBenKaynak2 != null)
                {
                    base.Game.RemovePhysicalObj(PisBenBenKaynak2, sendToClient: true);
                    PisBenBenKaynak2 = null;
                }
            }
        }

        public override bool CanGameOver()
        {
            if (PisBenBenKaynak != null && !PisBenBenKaynak.IsLiving && KafestekiCivcivler != null && !KafestekiCivcivler.IsLiving)
            {
                int_0++;
                return true;
            }
            if (base.Game.TotalTurn > base.Game.MissionInfo.TotalTurn)
            {
                return true;
            }
            return false;
        }

        public override void OnDied()
        {
            base.OnDied();
            if (PisBenBenKaynak != null && !PisBenBenKaynak.IsLiving && KafestekiCivcivler.IsLiving)
            {
                int waitTimerLeft = base.Game.GetWaitTimerLeft();
                base.Game.SendObjectFocus(KafestekiCivcivler, 1, waitTimerLeft + 500, 500);
                KafestekiCivcivler.PlayMovie("out", waitTimerLeft + 1000, 0);
                KafestekiCivcivler.Say("Hadi bizi kurtarınn!!!", 0, waitTimerLeft + 1500, 3500);
                KafestekiCivcivler.Config.CanTakeDamage = true;
            }
        }

        public override int UpdateUIData()
        {
            base.UpdateUIData();
            return int_0;
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            if (PisBenBenKaynak != null && !PisBenBenKaynak.IsLiving && KafestekiCivcivler != null && !KafestekiCivcivler.IsLiving)
            {
                base.Game.IsWin = true;
            }
            else
            {
                base.Game.IsWin = false;
            }
        }

        public GCGCT1164()
        {
            PisBenBen = 7131;
            Kafes = 7132;
            ZehirliYumurtalar = 7133;
        }
    }
}
