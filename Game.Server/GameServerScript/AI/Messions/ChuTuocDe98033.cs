using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using Game.Logic;
using Game.Logic.Actions;
using Bussiness;
using Game.Server.Rooms;

namespace GameServerScript.AI.Messions
{
    public class ChuTuocDe98033 : AMissionControl
    {
        private SimpleBoss boss = null;
        private int bossID = 51001;

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
            int[] resources = { bossID };
            int[] gameOverResource = { bossID };
            Game.LoadResources(resources);
            Game.LoadNpcGameOverResources(gameOverResource);
            Game.AddLoadingFile(2, "image/game/effect/15/305b.swf", "asset.game.fifteen.305b");
            Game.AddLoadingFile(2, "image/game/effect/15/305d.swf", "asset.game.fifteen.305d");
            Game.SetMap(1243);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
            LivingConfig living = Game.BaseLivingConfig();
            living.IsFly = true; //roi copy qua di
            boss = Game.CreateBoss(bossID, 805, 277, -1, 1, "born", living);
            boss.SetRelateDemagemRect(boss.NpcInfo.X, boss.NpcInfo.Y, boss.NpcInfo.Width, boss.NpcInfo.Height);
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();

        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
        }

        public override bool CanGameOver()
        {
            if (boss != null && boss.IsLiving == false)
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

            if (boss != null && boss.IsLiving == false)
            {
                Game.IsWin = true;
            }
            else
            {
                Game.IsWin = false;
            }
            List<Player> players = Game.GetAllFightPlayers();
            foreach (Player p in players)
            {
                p.PlayerDetail.UpdatePveResult("yearmonter", p.TotalDameLiving, Game.IsWin);
            }
        }
    }
}
