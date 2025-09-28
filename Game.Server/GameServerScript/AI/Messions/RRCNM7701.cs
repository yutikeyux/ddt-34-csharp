using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System.Collections.Generic;
using System.Drawing;

namespace GameServerScript.AI.Messions
{
    public class RRCNM7701 : AMissionControl
    {
        private List<SimpleNpc> redNpc = new List<SimpleNpc>();

        private List<SimpleNpc> blueNpc = new List<SimpleNpc>();

        private List<Point> brithPointRed = new List<Point>
            {
                new Point(958, 950),
                new Point(1400, 950),
                new Point(1034, 950),
                new Point(1472, 950)
            };

        private List<Point> brithPointBlue = new List<Point>
            {
                new Point(1150, 950),
                new Point(1346, 950)
            };

        private PhysicalObj m_layer;

        private int redCount = 20;

        private int blueCount = 10;

        private int redTotalCount = 10;

        private int blueTotalCount = 5;

        private int dieRedCount;

        private int dieBlueCount;

        private int redNpcID = 7702;

        private int blueNpcID = 7701;

        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 930)
            {
                return 3;
            }
            else if (score > 850)
            {
                return 2;
            }
            else if (score > 775)
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
            int[] resources = { redNpcID, blueNpcID };
            Game.AddLoadingFile(2, "image/game/living/living176.swf", "game.living.Living176");
            Game.LoadResources(resources);
            Game.LoadNpcGameOverResources(resources);
            Game.SetMap(1161);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
            m_layer = base.Game.Createlayer(1200, 955, "kingmoive", "game.living.Living176", "in", 1, 0);
            SummonBlue(blueTotalCount);
            SummonRed(redTotalCount);
        }

        private void SummonRed(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Point point = ((i < brithPointRed.Count) ? brithPointRed[i] : brithPointRed[base.Game.Random.Next(brithPointRed.Count)]);
                redNpc.Add(base.Game.CreateNpc(redNpcID, point.X, point.Y, 0, -1));
            }
        }

        private void SummonBlue(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Point point = ((i < brithPointBlue.Count) ? brithPointBlue[i] : brithPointBlue[base.Game.Random.Next(brithPointBlue.Count)]);
                blueNpc.Add(base.Game.CreateNpc(redNpcID, point.X, point.Y, 0, -1));
            }
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
            if (dieRedCount < redTotalCount && redNpc.Count < redCount)
            {
                int count = ((redTotalCount - dieRedCount > redCount - redNpc.Count) ? (redCount - redNpc.Count) : (redTotalCount - dieRedCount));
                if (count > 0)
                {
                    SummonRed(count);
                }
            }
            if (dieBlueCount < blueTotalCount && blueNpc.Count < blueCount)
            {
                int count = ((blueTotalCount - dieBlueCount > blueCount - blueNpc.Count) ? (blueCount - blueNpc.Count) : (blueTotalCount - dieBlueCount));
                if (count > 0)
                {
                    SummonBlue(count);
                }
            }
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
        }

        public override bool CanGameOver()
        {
            base.CanGameOver();
            if (base.Game.GetLivedLivings().Count == 0)
            {
                base.Game.PveGameDelay = 0;
            }
            dieBlueCount = 0;
            dieRedCount = 0;
            foreach (SimpleNpc npc in redNpc)
            {
                if (npc.IsLiving)
                {
                    dieRedCount++;
                }
            }
            foreach (SimpleNpc npc in blueNpc)
            {
                if (npc.IsLiving)
                {
                    dieBlueCount++;
                }
            }
            if (blueNpc.Count >= blueCount && redNpc.Count >= redCount && base.Game.GetLivedLivings().Count <= 0)
            {
                return true;
            }
            if (base.Game.TurnIndex > base.Game.MissionInfo.TotalTurn)
            {
                return true;
            }
            return false;
        }

        public override int UpdateUIData()
        {
            base.UpdateUIData();
            return base.Game.TotalKillCount;
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            if (base.Game.GetLivedLivings().Count == 0)
            {
                base.Game.IsWin = true;
            }
            else
            {
                base.Game.IsWin = false;
            }
        }
    }
}
