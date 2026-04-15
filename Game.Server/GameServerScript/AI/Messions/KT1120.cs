using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System.Collections.Generic;

namespace GameServerScript.AI.Messions
{
    public class KT1120 : AMissionControl
    {
        private List<SimpleNpc> someNpc = new List<SimpleNpc>();

        private int KırmızıKarıncaÖlümSayısı;

        private int[] KarıncaIDleri = { 2101, 2102 };

        private int[] DoğumNoktaları = { 52, 115, 183, 253, 320, 1206, 1275, 1342, 1410, 1475 };

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
            int[] resources = { KarıncaIDleri[0], KarıncaIDleri[1] };
            int[] gameOverResources = { KarıncaIDleri[1], KarıncaIDleri[0], KarıncaIDleri[0], KarıncaIDleri[0] };
            base.Game.LoadResources(resources);
            base.Game.LoadNpcGameOverResources(gameOverResources);
            base.Game.SetMap(1120);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
            int index = base.Game.Random.Next(0, KarıncaIDleri.Length);
            someNpc.Add(base.Game.CreateNpc(KarıncaIDleri[index], 52, 206, 1, 1));
            index = base.Game.Random.Next(0, KarıncaIDleri.Length);
            someNpc.Add(base.Game.CreateNpc(KarıncaIDleri[index], 100, 207, 1, 1));
            index = base.Game.Random.Next(0, KarıncaIDleri.Length);
            someNpc.Add(base.Game.CreateNpc(KarıncaIDleri[index], 155, 208, 1, 1));
            index = base.Game.Random.Next(0, KarıncaIDleri.Length);
            someNpc.Add(base.Game.CreateNpc(KarıncaIDleri[index], 210, 207, 1, 1));
            index = base.Game.Random.Next(0, KarıncaIDleri.Length);
            someNpc.Add(base.Game.CreateNpc(KarıncaIDleri[index], 253, 207, 1, 1));
            index = base.Game.Random.Next(0, KarıncaIDleri.Length);
            someNpc.Add(base.Game.CreateNpc(KarıncaIDleri[index], 1275, 208, -1, -1)); //sağdaki karıncalar oyuncuya dönük olması için -1
            index = base.Game.Random.Next(0, KarıncaIDleri.Length);
            someNpc.Add(base.Game.CreateNpc(KarıncaIDleri[index], 1325, 206, -1, -1)); //sağdaki karıncalar oyuncuya dönük olması için -1
            index = base.Game.Random.Next(0, KarıncaIDleri.Length);
            someNpc.Add(base.Game.CreateNpc(KarıncaIDleri[index], 1360, 208, -1, -1)); //sağdaki karıncalar oyuncuya dönük olması için -1
            index = base.Game.Random.Next(0, KarıncaIDleri.Length);
            someNpc.Add(base.Game.CreateNpc(KarıncaIDleri[index], 1410, 206, -1, -1)); //sağdaki karıncalar oyuncuya dönük olması için -1
            index = base.Game.Random.Next(0, KarıncaIDleri.Length);
            someNpc.Add(base.Game.CreateNpc(KarıncaIDleri[index], 1475, 208, -1, -1)); //sağdaki karıncalar oyuncuya dönük olması için -1
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
                int index = base.Game.Random.Next(0, DoğumNoktaları.Length);
                int NpcX = DoğumNoktaları[index];
                index = base.Game.Random.Next(0, KarıncaIDleri.Length);
                if (index == 1 && GetNpcCountByID(KarıncaIDleri[1]) < 10)
                {
                    if (NpcX > 700)
                    {
                        someNpc.Add(base.Game.CreateNpc(KarıncaIDleri[1], NpcX, 506, -1, 1));
                    }
                    else
                    {
                        someNpc.Add(base.Game.CreateNpc(KarıncaIDleri[1], NpcX, 506, 1, 1));
                    }
                }
                else if (NpcX > 700)
                {
                    someNpc.Add(base.Game.CreateNpc(KarıncaIDleri[1], NpcX, 506, -1, 1));
                }
                else
                {
                    someNpc.Add(base.Game.CreateNpc(KarıncaIDleri[1], NpcX, 506, 1, 1));
                }
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
            KırmızıKarıncaÖlümSayısı = 0;
            foreach (SimpleNpc item in someNpc)
            {
                if (item.IsLiving)
                {
                    result = false;
                }
                else
                {
                    KırmızıKarıncaÖlümSayısı++;
                }
            }
            if (result && KırmızıKarıncaÖlümSayısı == base.Game.MissionInfo.TotalCount)
            {
                base.Game.IsWin = true;
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
            return base.Game.TotalKillCount;
        }

        public override void OnGameOver()
        {
            base.OnGameOver();
            if (base.Game.GetLivedLivings().Count == 0)
            {
                base.Game.IsWin = true;
                List<LoadingFileInfo> loadingFileInfos = new List<LoadingFileInfo>();
                loadingFileInfos.Add(new LoadingFileInfo(2, "image/map/2/show2", ""));
                base.Game.SendLoadResource(loadingFileInfos);
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
