using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using Bussiness;


//Civciv kolay Etap 3
namespace GameServerScript.AI.Messions
{
    public class GCGCD1163 : AMissionControl
    {
        private SimpleBoss ÇiftlikEVÝ = null;

        private int ÞiþmanTavuk = 7021;

        private int HorozAbi = 7022;

        private int ÇiftlikEvi = 7023;

        private int kill = 0;
        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 1750)
            {
                return 3;
            }
            else if (score > 1675)
            {
                return 2;
            }
            else if (score > 1600)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            int[] resources = { ÇiftlikEvi, ÞiþmanTavuk, HorozAbi };
            int[] gameOverResource = { ÇiftlikEvi };
            Game.LoadResources(resources);
            Game.LoadNpcGameOverResources(gameOverResource);
            Game.AddLoadingFile(2, "image/game/effect/7/cao.swf", "asset.game.seven.cao");
            Game.AddLoadingFile(2, "image/game/effect/7/jinquhd.swf", "asset.game.seven.jinquhd");
            Game.SetMap(1163);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
            Game.IsBossWar = "7103";
            ÇiftlikEVÝ = Game.CreateBoss(ÇiftlikEvi, 275, 950, 1, 1, "");
            ÇiftlikEVÝ.FallFrom(338, 950, "", 0, 0, 1000);
            ÇiftlikEVÝ.SetRelateDemagemRect(ÇiftlikEVÝ.NpcInfo.X, ÇiftlikEVÝ.NpcInfo.Y, ÇiftlikEVÝ.NpcInfo.Width, ÇiftlikEVÝ.NpcInfo.Height);

        }

        public override void OnNewTurnStarted()
        {
            //base.OnBeginNewTurn();


        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
        }

        public override bool CanGameOver()
        {

            if (ÇiftlikEVÝ != null && ÇiftlikEVÝ.IsLiving == false)
            {
                kill++;
                return true;
            }
            return false;
        }

        public override int UpdateUIData()
        {
            base.UpdateUIData();
            return kill;
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            if (ÇiftlikEVÝ != null && ÇiftlikEVÝ.IsLiving == false)
            {
                Game.IsWin = true;
            }
            else
            {
                Game.IsWin = false;
            }
        }
    }
}