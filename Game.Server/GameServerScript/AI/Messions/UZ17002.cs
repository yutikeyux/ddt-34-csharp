using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.Messions
{
    public class UZ17002 : AMissionControl
    {
        private SimpleBoss simpleBoss_0; // Birinci Boss (Yerde)
        private SimpleBoss simpleBoss_1; // İkinci Boss (Havada)

        private PhysicalObj physicalObj_0;
        private PhysicalObj physicalObj_1;

        private int int_0; // Skor/Kazanma durumu

        // Boss ID Tanımlamaları
        private int boss1Id = 40065;   // Birinci Boss ID
        private int boss2Id = 2001006; // İkinci Boss ID

        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 1150) return 3;
            if (score > 925) return 2;
            if (score > 700) return 1;
            return 0;
        }

        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();

            // Gerekli görsel kaynakları yükle
            base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
            base.Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.hongpaoxiaoemoAsset");
            base.Game.AddLoadingFile(2, "image/game/effect/5/heip.swf", "asset.game.4.heip");
            base.Game.AddLoadingFile(2, "image/game/effect/5/tang.swf", "asset.game.4.tang");
            base.Game.AddLoadingFile(2, "image/game/effect/5/lanhuo.swf", "asset.game.4.lanhuo");

            // Her iki boss için de kaynakları yükle
            int[] npcIds = new int[] { boss1Id, boss2Id };
            base.Game.LoadResources(npcIds);
            base.Game.LoadNpcGameOverResources(npcIds);

            // Harita ID
            base.Game.SetMap(1469);
        }

        public override void OnStartGame()
        {
            base.OnStartGame();

            // Giriş sinematikleri
            physicalObj_0 = base.Game.Createlayer(0, 0, "moive", "game.asset.living.BossBgAsset", "out", 1, 1);
            physicalObj_1 = base.Game.Createlayer(850, 258, "front", "game.asset.living.hongpaoxiaoemoAsset", "out", 1, 1);

            // --- 1. BOSS YARATILIYOR (YERDE) ---
            LivingConfig livingConfig = base.Game.BaseLivingConfig();
            livingConfig.IsFly = false; // Yerde doğacak

            // ID: 40065, X:1000, Y:500 (Zemin), Yön: -1 (Sola bakar)
            simpleBoss_0 = base.Game.CreateBoss(boss1Id, 1000, 500, -1, 1, "", livingConfig);
            simpleBoss_0.SetRelateDemagemRect(simpleBoss_0.NpcInfo.X, simpleBoss_0.NpcInfo.Y, simpleBoss_0.NpcInfo.Width, simpleBoss_0.NpcInfo.Height);
            simpleBoss_0.Say("Uzun zamandır bunu bekliyordum!", 0, 1000);

            // Giriş animasyonlarını oynat
            physicalObj_0.PlayMovie("in", 4000, 0);
            physicalObj_1.PlayMovie("in", 4000, 0);
            physicalObj_0.PlayMovie("out", 7000, 0);
            physicalObj_1.PlayMovie("out", 7200, 0);
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            // Fiziksel objeleri temizle
            if (physicalObj_0 != null)
            {
                base.Game.RemovePhysicalObj(physicalObj_0, true);
                physicalObj_0 = null;
            }
            if (physicalObj_1 != null)
            {
                base.Game.RemovePhysicalObj(physicalObj_1, true);
                physicalObj_1 = null;
            }
        }

        public override void OnDied()
        {
            base.OnDied();

            // Birinci boss öldüğünde ve ikinci boss henüz yaratılmadıysa çalışır
            if (simpleBoss_0 != null && !simpleBoss_0.IsLiving && simpleBoss_1 == null)
            {
                // İstediğiniz gibi "die" animasyonunu oynatır (PlayMovie)
                simpleBoss_0.PlayMovie("die", 0, 0);

                // 3 saniye sonra ikinci bossu doğuracak fonksiyonu çağır
                simpleBoss_0.CallFuction(new LivingCallBackHandle(SpawnSecondBoss), simpleBoss_0, 3000);
            }
        }

        // İkinci Bossu Doğuran Metot
        private void SpawnSecondBoss(Living living)
        {
            // --- 2. BOSS YARATILIYOR (HAVADA) ---
            LivingConfig livingConfig = base.Game.BaseLivingConfig();
            livingConfig.IsFly = true; // Havada uçacak

            // ID: 2001006, X:1000, Y:300 (Hava), Yön: -1, Aksiyon: "born" (Doğuş animasyonu)
            simpleBoss_1 = base.Game.CreateBoss(boss2Id, 1000, 300, -1, 1, "born", livingConfig);
            simpleBoss_1.SetRelateDemagemRect(simpleBoss_1.NpcInfo.X, simpleBoss_1.NpcInfo.Y, simpleBoss_1.NpcInfo.Width, simpleBoss_1.NpcInfo.Height);
            simpleBoss_1.Say("Şimdi gerçek savaş başlıyor!", 0, 1000);
        }

        public override bool CanGameOver()
        {
            base.CanGameOver();

            // Eğer birinci boss öldü ama ikinci boss henüz doğmadıysa oyunu bitirme (bekle)
            if (simpleBoss_0 != null && !simpleBoss_0.IsLiving && simpleBoss_1 == null)
            {
                return false;
            }

            // İkinci boss öldüyse oyunu kazanır
            if (simpleBoss_1 != null && !simpleBoss_1.IsLiving)
            {
                int_0++;
                return true;
            }

            // Süre kontrolü
            if (base.Game.TurnIndex > 200)
            {
                return true;
            }
            return false;
        }

        public override int UpdateUIData()
        {
            base.UpdateUIData();
            return int_0;
        }

        public override void OnGameOver()
        {
            base.OnGameOver();

            // İkinci boss öldüyse oyunu kazanmış say
            if (simpleBoss_1 != null && !simpleBoss_1.IsLiving)
            {
                base.Game.IsWin = true;
            }
            else
            {
                base.Game.IsWin = false;
            }
        }

        public UZ17002()
        {
            // Constructor
        }
    }
}