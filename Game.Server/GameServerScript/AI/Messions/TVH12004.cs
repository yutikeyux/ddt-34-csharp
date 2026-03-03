using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.Messions
{
    public class TVH12004 : AMissionControl
    {
        private int ChickenFriendID = 12213;

        private int BossID1 = 12214;

        private int BossID2 = 12215;

        private int BossID3 = 12216;

        private int NpcID1 = 12217;

        private int NpcID2 = 12218;

        private int NpcID3 = 12220;

        private int NpcID4 = 12219;

        private SimpleNpc chickenFriend;

        private SimpleBoss Boss1;

        private SimpleBoss Boss2;

        private SimpleBoss Boss3;

        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 900)
            {
                return 3;
            }
            if (score > 825)
            {
                return 2;
            }
            if (score <= 725)
            {
                return 0;
            }
            return 1;
        }

        // DÜZELTİLMİŞ CanGameOver METODU
        public override bool CanGameOver()
        {
            // 1. Oyun süresi dolduysa (örneğin 200 tur bittiyse) oyunu bitir.
            if (Game.TurnIndex > Game.TotalTurn)
                return true;

            // 2. Tüm oyuncular öldüyse oyunu bitir (Kaybetme durumu).
            if (Game.GetAllLivingPlayers().Count == 0)
                return true;

            // 3. Son Boss (Boss3) varsa ve öldüyse oyunu bitir (Kazanma durumu).
            // Boss3 henüz doğmadıysa (null ise) oyun devam etmelidir.
            if (Boss3 != null && !Boss3.IsLiving)
                return true;

            // Hiçbir koşul sağlanmıyorsa (Boss1 veya Boss2 yaşıyor, oyuncular yaşıyor) oyun devam etsin.
            return false;
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
        }

        public override void OnDied()
        {
            base.OnDied();
            // Boss1 öldüyse 4 saniye sonra Boss2'yi oluştur
            if (Boss1 != null)
            {
                if (!Boss1.IsLiving)
                    Boss1.CallFuction(CreateSecondBoss, 4000);
            }
            // Boss2 öldüyse 4 saniye sonra Boss3'ü (Son Boss) oluştur
            if (Boss2 != null)
            {
                if (!Boss2.IsLiving)
                    Boss2.CallFuction(CreateFinalBoss, 4000);
            }
            // Boss3 öldüyse 4 saniye bekle (Oyun bitiş animasyonu için)
            if (Boss3 != null)
            {
                if (!Boss3.IsLiving)
                    Game.WaitTime(4000);
            }
        }

        // DÜZELTİLMİŞ OnGameOver METODU
        public override void OnGameOver()
        {
            base.OnGameOver();

            // Kazanma koşulu sadece Boss3'ün ölmesiyle gerçekleşir.
            bool isWin = false;

            if (Boss3 != null && !Boss3.IsLiving)
            {
                isWin = true;
            }

            Game.IsWin = isWin;
        }

        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
        }

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            Game.AddLoadingFile(2, "image/game/effect/9/daodan.swf", "asset.game.nine.daodan");
            Game.AddLoadingFile(2, "image/game/effect/9/diancipao.swf", "asset.game.nine.diancipao");
            Game.AddLoadingFile(2, "image/game/effect/9/fengyin.swf", "asset.game.nine.fengyin");
            Game.AddLoadingFile(2, "image/game/effect/9/siwang.swf", "asset.game.nine.siwang");
            Game.AddLoadingFile(2, "image/game/effect/9/shexian.swf", "asset.game.nine.shexian");
            Game.AddLoadingFile(2, "image/game/effect/9/biaoji.swf", "asset.game.nine.biaoji");
            Game.AddLoadingFile(2, "image/game/effect/9/heidong.swf", "asset.game.nine.heidong");
            int[] int1 = new int[] { BossID1, BossID2, BossID3, NpcID1, NpcID2, NpcID3, ChickenFriendID, NpcID4 };
            Game.LoadResources(int1);
            Game.LoadNpcGameOverResources(int1);
            Game.SetMap(1210);

            // ÖNEMLİ: Toplam görev süresini(turn) buradan ayarlayabilirsiniz, 
            // ancak kodda belirtilmediği için varsayılan değer kullanılır.
        }

        public override void OnPrepareStartGame()
        {
            base.OnPrepareStartGame();
            CreateChickenFriend();
        }

        public override void OnStartGame()
        {
            base.OnStartGame();
            CreateFirstBoss();
            Game.SendFreeFocus(950, 300, 1, 1, 1);
        }

        private void CreateChickenFriend()
        {
            LivingConfig livingConfig = base.Game.BaseLivingConfig();
            livingConfig.IsFly = true;
            livingConfig.CanTakeDamage = false;
            livingConfig.IsHelper = true;
            livingConfig.IsTurn = false;
            livingConfig.CanCollied = false;
            livingConfig.isShowBlood = false;
            chickenFriend = Game.CreateNpc(ChickenFriendID, 219, 750, 1, 1, "", livingConfig);
        }

        private void CreateSecondBoss()
        {
            Game.SendFreeFocus(987, 342, 1, 1, 1);
            Boss1 = null; // Önceki boss'ı temizle
            LivingConfig livingConfig = base.Game.BaseLivingConfig();
            livingConfig.IsFly = true;
            livingConfig.CanCountKill = false;
            livingConfig.isBotom = 0;
            Boss2 = Game.CreateBoss(BossID2, 987, 342, -1, 1, "born", livingConfig);
        }

        private void CreateFinalBoss()
        {
            Game.SendFreeFocus(987, 342, 1, 1, 1);
            Boss2 = null; // Önceki boss'ı temizle
            LivingConfig livingConfig = Game.BaseLivingConfig();
            livingConfig.IsFly = true;
            livingConfig.CanCountKill = true; // Son boss öldüğünde sayılsın
            livingConfig.isBotom = 0;
            Boss3 = Game.CreateBoss(BossID3, 987, 342, -1, 1, "born", livingConfig);
        }

        private void CreateFirstBoss()
        {
            LivingConfig livingConfig = Game.BaseLivingConfig();
            livingConfig.IsFly = true;
            livingConfig.CanCountKill = false;
            livingConfig.isBotom = 0;
            Boss1 = Game.CreateBoss(BossID1, 950, 400, -1, 1, "born", livingConfig);
            Boss1.SetRelateDemagemRect(Boss1.NpcInfo.X, Boss1.NpcInfo.Y, Boss1.NpcInfo.Width, Boss1.NpcInfo.Height);
            Game.WaitTime(7000);
        }

        public override int UpdateUIData()
        {
            base.UpdateUIData();
            return Game.TotalKillCount;
        }
    }
}