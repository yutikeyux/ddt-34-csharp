using System.Collections.Generic;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Effects;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.Messions
{
    public class PDHAT1142 : AMissionControl
    {
        private List<SimpleNpc> NpcListesi;

        private SimpleBoss KALEDUVARI;

        private SimpleBoss FIÇIBOMBA;

        private int FýçýBomba;

        private int ZombiGoblin;

        private int KALE;

        private int YANMACANI;

        private int DOÐACAKZOMBÝGOBLÝNSAYISI;

        private int KIRILANDUVAR;

        public int DuvarBoþluðu
        {
            get
            {
                return KIRILANDUVAR;
            }
            set
            {
                KIRILANDUVAR = value;
            }
        }

        public SimpleBoss DuvarBombasý
        {
            get
            {
                return FIÇIBOMBA;
            }
            set
            {
                FIÇIBOMBA = value;
            }
        }

        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 1750)
            {
                return 3;
            }
            if (score > 1675)
            {
                return 2;
            }
            if (score > 1600)
            {
                return 1;
            }
            return 0;
        }

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            int[] npcIDleri = new int[3]
            {
                KALE,
                ZombiGoblin,
                FýçýBomba
            };
            base.Game.LoadResources(npcIDleri);
            base.Game.LoadNpcGameOverResources(npcIDleri);
            base.Game.AddLoadingFile(2, "image/game/effect/4/gate.swf", "game.asset.Gate");
            base.Game.SetMap(1142);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
            LivingConfig livingConfig = base.Game.BaseLivingConfig();
            livingConfig.CanTakeDamage = false;
            KALEDUVARI = base.Game.CreateBoss(KALE, 1520, 350, -1, 1, "", livingConfig);
            base.Game.SendHideBlood(KALEDUVARI, 0);
            BombayýÇaðýr();
            base.Game.SendFreeFocus(1500, 250, 1, 2000, 3000);
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            if (FIÇIBOMBA != null && FIÇIBOMBA.IsLiving && FIÇIBOMBA.X >= 600 && base.Game.FindAllNpcLiving().Length == 0 && base.Game.CurrentTurnLiving is Player)
            {
                GobliniÇaðýr();
            }
            if (FIÇIBOMBA != null && !FIÇIBOMBA.IsLiving)
            {
                base.Game.RemoveLiving(FIÇIBOMBA, sendToClient: true);
                BombayýÇaðýr();
            }
            if (base.Game.CurrentTurnLiving is Player && FIÇIBOMBA != null && FIÇIBOMBA.IsLiving && (int)KALEDUVARI.Properties1 == 1)
            {
                FIÇIBOMBA.PlayMovie("standB", 1200, 0);
            }
        }

        private void BombayýÇaðýr()
        {
            LivingConfig livingConfig = base.Game.BaseLivingConfig();
            livingConfig.IsHelper = true;
            FIÇIBOMBA = base.Game.CreateBoss(FýçýBomba, 321, 746, 1, 0, "", livingConfig);
            FIÇIBOMBA.AddEffect(new ContinueReduceBloodEffect(2, YANMACANI, FIÇIBOMBA), 0);
        }

        private void GobliniÇaðýr()
        {
            int num = 0;
            NpcListesi = new List<SimpleNpc>();
            for (int i = 0; i < DOÐACAKZOMBÝGOBLÝNSAYISI; i++)
            {
                if (num > 0)
                {
                    int num2 = base.Game.Random.Next(5, 110);
                    NpcListesi.Add(base.Game.CreateNpc(ZombiGoblin, FIÇIBOMBA.X - num2, FIÇIBOMBA.Y, 0));
                }
                else
                {
                    NpcListesi.Add(base.Game.CreateNpc(ZombiGoblin, FIÇIBOMBA.X - 20, FIÇIBOMBA.Y, 0));
                }
                num++;
            }
        }

        public override bool CanGameOver()
        {
            base.CanGameOver();
            if (KIRILANDUVAR >= base.Game.MissionInfo.TotalCount)
            {
                return true;
            }
            if (base.Game.TotalTurn > base.Game.MissionInfo.TotalTurn)
            {
                return true;
            }
            return false;
        }

        public override int UpdateUIData()
        {
            base.UpdateUIData();
            return KIRILANDUVAR;
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            if (KIRILANDUVAR >= base.Game.MissionInfo.TotalCount)
            {
                base.Game.IsWin = true;
            }
            else
            {
                base.Game.IsWin = false;
            }
        }

        public PDHAT1142()
        {
            NpcListesi = new List<SimpleNpc>();
            FýçýBomba = 4101;
            ZombiGoblin = 4103;
            KALE = 4104;
            YANMACANI = 500;
            DOÐACAKZOMBÝGOBLÝNSAYISI = 1;
        }
    }
}