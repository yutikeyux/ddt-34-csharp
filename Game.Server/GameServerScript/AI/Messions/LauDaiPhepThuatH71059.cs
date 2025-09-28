using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System.Collections.Generic;

namespace GameServerScript.AI.Messions
{
    public class LauDaiPhepThuatH71059 : AMissionControl
    {
        private List<SimpleNpc> someNpc = new List<SimpleNpc>();

        private int dieRedCount;

        private int[] npcIDs = { 71068, 71067 };

        private int[] birthX = { 635, 750, 900, 1115 };

        public LivingConfig config = (LivingConfig)null;

        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 1870)
            {
                return 3;
            }
            if (score > 1825)
            {
                return 2;
            }
            if (score > 1780)
            {
                return 1;
            }
            return 0;
        }

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            int[] resources = { npcIDs[0], npcIDs[1] };
            int[] gameOverResources = { npcIDs[1], npcIDs[0], npcIDs[0], npcIDs[0] };
            base.Game.LoadResources(resources);
            base.Game.LoadNpcGameOverResources(gameOverResources);
            base.Game.SetMap(1464);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
            config = Game.BaseLivingConfig();
            config.IsFly = true;

            someNpc.Add(this.Game.CreateNpc(this.npcIDs[0], base.Game.Random.Next(970, 1070), base.Game.Random.Next(510, 520), 1, -1));
            someNpc.Add(this.Game.CreateNpc(this.npcIDs[0], base.Game.Random.Next(1071, 1171), base.Game.Random.Next(521, 530), 1, -1));
            someNpc.Add(this.Game.CreateNpc(this.npcIDs[0], base.Game.Random.Next(1172, 1272), base.Game.Random.Next(531, 640), 1, -1));
            someNpc.Add(this.Game.CreateNpc(this.npcIDs[0], base.Game.Random.Next(1273, 1373), base.Game.Random.Next(541, 650), 1, -1));
            someNpc.Add(this.Game.CreateNpc(this.npcIDs[0], base.Game.Random.Next(1374, 1415), base.Game.Random.Next(551, 660), 1, -1));

            someNpc.Add(this.Game.CreateNpc(this.npcIDs[1], base.Game.Random.Next(570, 670), base.Game.Random.Next(510, 520), 1, -1, config));
            someNpc.Add(this.Game.CreateNpc(this.npcIDs[1], base.Game.Random.Next(671, 771), base.Game.Random.Next(521, 530), 1, -1, config));
            someNpc.Add(this.Game.CreateNpc(this.npcIDs[1], base.Game.Random.Next(772, 872), base.Game.Random.Next(531, 640), 1, -1, config));
            someNpc.Add(this.Game.CreateNpc(this.npcIDs[1], base.Game.Random.Next(873, 973), base.Game.Random.Next(541, 650), 1, -1, config));
            someNpc.Add(this.Game.CreateNpc(this.npcIDs[1], base.Game.Random.Next(974, 1115), base.Game.Random.Next(551, 660), 1, -1, config));
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
            if (base.Game.GetLivedLivings().Count == 0)
            {
                base.Game.PveGameDelay = 0;
            }
            if (base.Game.TurnIndex <= 1 || base.Game.CurrentPlayer.Delay <= base.Game.PveGameDelay || base.Game.GetLivedLivings().Count >= 10)
            {
                return;
            }
            for (int i = 0; i < 10 - base.Game.GetLivedLivings().Count; i++)
            {
                if (someNpc.Count == base.Game.MissionInfo.TotalCount)
                {
                    break;
                }
                int num = birthX[base.Game.Random.Next(0, birthX.Length)];
                int x = this.Game.Random.Next(572, 1115);
                int y = this.Game.Random.Next(510, 650);
                if (base.Game.Random.Next(0, npcIDs.Length) == 1 && GetNpcCountByID(npcIDs[1]) < 10)
                    someNpc.Add(base.Game.CreateNpc(npcIDs[1], x, y, 1, 1, config));
                else
                    someNpc.Add(base.Game.CreateNpc(npcIDs[0], Game.Random.Next(1164, 1465), 911, 1, 1));
            }
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
        }

        public override bool CanGameOver()
        {
            bool result = true;
            base.CanGameOver();
            dieRedCount = 0;
            foreach (SimpleNpc item in someNpc)
            {
                if (item.IsLiving)
                {
                    result = false;
                }
                else
                {
                    dieRedCount++;
                }
            }
            if (result && dieRedCount == base.Game.MissionInfo.TotalCount)
            {
                base.Game.IsWin = true;
                return true;
            }
            return false;
        }

        public override int UpdateUIData()
        {
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

        protected int GetNpcCountByID(int Id)
        {
            int Count = 0;
            foreach (SimpleNpc item in someNpc)
            {
                if (item.NpcInfo.ID == Id)
                {
                    Count++;
                }
            }
            return Count;
        }
    }
}
