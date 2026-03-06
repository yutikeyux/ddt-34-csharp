using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System.Collections.Generic;
using System.Drawing;

namespace GameServerScript.AI.Messions
{
    public class DTGT1166 : AMissionControl
    {
        private const int BOSS_RED_ID = 6123;
        private const int NPC_FRIEND_ID = 6121;
        private const int NPC_ENEMY_ID = 6122;
        private const int NPC_SUPPORT_ID = 6124;
        private const int NPC_COMMANDER_ID = 6114;

        private SimpleBoss bossRed;
        private SimpleBoss bossCommander;
        private PhysicalObj targetObject;

        // KAZANMA KOŞULU: En az 2 dost bogo bitiş çizgisine ulaşmalı.
        private const int REQUIRED_FRIEND_COUNT = 2;

        // Düşman sayısı veya koşulu (Oyunun bitmesi için hala bir tetikleyici gerekli)
        private const int REQUIRED_ENEMY_COUNT = 5;

        private int _finishedFriends = 0;
        private int _finishedEnemies = 0;

        private List<SimpleNpc> listFriends = new List<SimpleNpc>();
        private List<SimpleNpc> listEnemies = new List<SimpleNpc>();

        private List<Point> pathPoints;

        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 900) return 3;
            if (score > 825) return 2;
            if (score > 725) return 1;
            return 0;
        }

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            base.Game.AddLoadingFile(2, "image/game/effect/6/danti.swf", "asset.game.six.danti");
            base.Game.AddLoadingFile(2, "image/game/effect/6/qunti.swf", "asset.game.six.qunti");
            base.Game.AddLoadingFile(2, "image/game/effect/6/zhaozi.swf", "asset.game.six.zhaozi");
            base.Game.AddLoadingFile(2, "image/game/effect/6/danjia.swf", "asset.game.six.danjia");
            base.Game.AddLoadingFile(2, "image/game/effect/6/qunjia.swf", "asset.game.six.qunjia");

            int[] npcIds = { BOSS_RED_ID, NPC_FRIEND_ID, NPC_ENEMY_ID, NPC_SUPPORT_ID, NPC_COMMANDER_ID };
            base.Game.LoadResources(npcIds);
            base.Game.LoadNpcGameOverResources(npcIds);
            base.Game.SetMap(1166);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
            _finishedFriends = 0;
            _finishedEnemies = 0;
            listFriends.Clear();
            listEnemies.Clear();

            bossRed = base.Game.CreateBoss(BOSS_RED_ID, 1910, 1080, -1, 1, "");
            bossRed.Delay = 1;
            bossRed.Config.CanTakeDamage = false;

            bossCommander = base.Game.CreateBoss(NPC_COMMANDER_ID, 460, 1080, -1, 1, "");
            bossCommander.Config.CanTakeDamage = false;
            bossCommander.Config.IsTurn = false;
            bossCommander.MoveTo(450, 1080, "walk", 500);
            bossCommander.PlayMovie("go", 1500, 0);
            bossCommander.Say("Dost bogoları kurtar, Düşman bogoları engelle!", 0, 2000);

            targetObject = base.Game.Createlayerboss(1100, 1080, "font", "game.living.Living190", "stand", 1, 0);

            bossRed.CallFuction(SpawnNpcs, 2500);
            base.Game.PveGameDelay = 0;
        }

        private void SpawnNpcs()
        {
            int startStep = 10;
            for (int i = 0; i < 5; i++)
            {
                int posX = 300 - (i * 50);
                SimpleNpc npc = base.Game.CreateNpc(NPC_ENEMY_ID, posX, 1080, 1, 1);
                npc.Config.IsFly = true;
                npc.Config.MaxStepMove = 6;
                npc.Config.MinBlood = 1;
                npc.Config.FirstStepMove = startStep;

                listEnemies.Add(npc);
                startStep -= 2;
            }

            startStep = 9;
            for (int i = 0; i < 5; i++)
            {
                int posX = 50 - (i * 50);
                SimpleNpc npc = base.Game.CreateNpc(NPC_FRIEND_ID, posX, 1080, 1, 1);
                npc.Config.IsFly = true;
                npc.Config.IsHelper = true;
                npc.Config.MaxStepMove = 4;
                npc.Config.MinBlood = 1;
                npc.Config.FirstStepMove = startStep;

                listFriends.Add(npc);
                startStep -= 2;
            }
        }

        public override bool CanGameOver()
        {
            base.CanGameOver();
            if (listFriends.Count == 0 && listEnemies.Count == 0) return false;

            int currentFinishedFriends = 0;
            int currentFinishedEnemies = 0;

            foreach (SimpleNpc npc in listFriends)
            {
                if (npc != null && npc.IsLiving && npc.Config.CompleteStep) currentFinishedFriends++;
            }

            foreach (SimpleNpc npc in listEnemies)
            {
                if (npc != null && npc.IsLiving && npc.Config.CompleteStep) currentFinishedEnemies++;
            }

            _finishedFriends = currentFinishedFriends;
            _finishedEnemies = currentFinishedEnemies;

            // Kazanma koşulu: 2 Dost Bogo tamamlarsa oyun biter.
            if (_finishedFriends >= REQUIRED_FRIEND_COUNT)

            {
                bossCommander.Say("Harika! Senin yazacağın kodun amına koyim utku!", 0, 2000);
                return true;
            }
            // Düşmanlar şartı sağlarsa (örneğin hepsi tamamlarsa) oyun biter ve OnGameOver'da kaybettirilir.
            if (_finishedEnemies >= REQUIRED_ENEMY_COUNT)
            {
                return true;
            }
            return false;
        }

        public override int UpdateUIData() => _finishedFriends;

        public override void OnGameOver()
        {
            base.OnGameOver();

            // Eğer en az 2 dost bogo hedefe ulaştıysa KAZANIR, değilse KAYBEDER.
            if (_finishedFriends >= REQUIRED_FRIEND_COUNT)
            {
                base.Game.IsWin = true;
            }
            else
            {
                base.Game.IsWin = false;
            }
        }

        public DTGT1166()
        {
            pathPoints = new List<Point> { new Point(620, 1080) };
        }
    }
}