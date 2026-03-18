using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System.Collections.Generic;

namespace GameServerScript.AI.Messions
{
    public class UZ17001 : AMissionControl
    {
        private List<SimpleNpc> someNpc = new List<SimpleNpc>();

        private int dieRedCount;

        
        private int[] npcIDs = { 10015, 71087 };

        // Sadece haritanın en sağındaki doğuş noktaları (Sol taraf kaldırıldı)
        private int[] birthX = { 1206, 1275, 1342, 1410, 1475 };

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
            base.Game.SetMap(1316);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();

            // Oyun başında 10 NPC doğuruluyor (Hepsi sağdan, 5'i uçan 5'i yürüyen)

            // 5 Tane Uçan NPC (38204) - Sağ Üstte
            for (int i = 0; i < 5; i++)
            {
                int x = birthX[base.Game.Random.Next(0, birthX.Length)];
                // Y=210 Havada doğması için. Direction=1 yere düşmemesi için yerçekimi kapatma girişimi (AI'a bağlı)
                someNpc.Add(base.Game.CreateNpc(npcIDs[0], x, 210, -1, 1));
            }

            // 5 Tane Yürüyen NPC (71087) - Sağ Altta
            for (int i = 0; i < 5; i++)
            {
                int x = birthX[base.Game.Random.Next(0, birthX.Length)];
                // Y=506 Zeminde
                someNpc.Add(base.Game.CreateNpc(npcIDs[1], x, 506, -1, 1));
            }
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
            if (base.Game.GetLivedLivings().Count == 0)
            {
                base.Game.PveGameDelay = 0;
            }

            // Standart canlı kontrolü
            if (base.Game.TurnIndex <= 1 || base.Game.CurrentPlayer.Delay <= base.Game.PveGameDelay || base.Game.GetLivedLivings().Count >= 10)
            {
                return;
            }

            // Eksik NPC sayısı kadar döngü
            for (int i = 0; i < 10 - base.Game.GetLivedLivings().Count; i++)
            {
                if (someNpc.Count == base.Game.MissionInfo.TotalCount)
                {
                    break;
                }

                // Rastgele X koordinatı (Sadece sağ taraf)
                int index = base.Game.Random.Next(0, birthX.Length);
                int NpcX = birthX[index];

                // Rastgele NPC ID seç
                index = base.Game.Random.Next(0, npcIDs.Length);
                int selectedNpcId = npcIDs[index];

                // Seçilen ID'ye göre konum belirleme
                if (selectedNpcId == npcIDs[0]) // 38204 Uçan NPC
                {
                    // Sağ üstte doğacak (Havada)
                    // Direction -1 veriyoruz ki sola (oyuncuya) baksın ve uçsun.
                    // Not: NPC'nin DB ayarlarında "Fly" özelliği açık olmalıdır, yoksa yere düşebilir.
                    someNpc.Add(base.Game.CreateNpc(selectedNpcId, NpcX, 210, -1, 1));
                }
                else // 71087 Yürüyen NPC
                {
                    // Sağ aşağıda doğacak (Zeminde)
                    someNpc.Add(base.Game.CreateNpc(selectedNpcId, NpcX, 506, -1, 1));
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