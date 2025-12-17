// Gerekli kütüphaneler ve sistem sınıfları
using System;
using System.Collections.Generic;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Effects;
using Game.Logic.Phy.Object;

// AI kodunun bulunduğu namespace
namespace GameServerScript.AI.NPC
{
    /// <summary>
    /// Bu sınıf, "TwelveSimpleFlyThirdBoss" adlı NPC'nin yapay zekasını ve davranışlarını kontrol eder.
    /// ABrain sınıfından miras alır.
    /// </summary>
    public class TwelveSimpleFlyThirdBoss : ABrain
    {
        #region Constants (Sabitler)

        // Oyuncuların ışınlanacağı X koordinatları
        private static readonly int[] TELEPORT_X_POSITIONS = new int[] { 725, 840, 1065, 1200 };

        // Oyuncuların ışınlanacağı ilk Y koordinatı
        private const int INITIAL_TELEPORT_Y = 600;

        // Oyuncuların saldırı sonrası ışınlanacağı son Y koordinatı
        private const int FINAL_ATTACK_Y_POS = 910;

        // Döndürme için toplam açı (5 tur x 360 derece)
        private const int TOTAL_ROTATION_DEGREES = 200;

        // Döndürme hızı
        private const int ROTATION_SPEED = 30;

        // Zamanlama gecikmeleri (milisaniye cinsinden)
        private const int PREPARE_ATTACK_DELAY = 100;
        private const int CREATE_EFFECTS_DELAY = 500;
        private const int SPIN_DELAY = 100;

        #endregion

        #region Fields (Alanlar)

        // Boss'un konuşma metinleri
        private static readonly string[] BOSS_QUOTES = new string[]
        {
            "Anafor! Sonsuzluk sizi yutar!"
        };

        #endregion

        #region ABrain Override Methods (Geçersiz Kılınan Metotlar)

        /// <summary>
        /// Her yeni tur başladığında çağrılır. Boss'un temel saldırı değerlerini sıfırlar.
        /// </summary>
        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            // Saldırı ve savunma bonuslarını normal seviyeye getir
            base.Body.CurrentDamagePlus = 1.0f;
            base.Body.CurrentShootMinus = 1.0f;
        }

        /// <summary>
        /// Boss'un kendi turu başladığında çağrılır.
        /// </summary>
        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        /// <summary>
        /// Boss nesnesi oluşturulduğunda çağrılır.
        /// </summary>
        public override void OnCreated()
        {
            base.OnCreated();
        }

        /// <summary>
        /// Boss saldırıyı bıraktığında çağrılır.
        /// </summary>
        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

        /// <summary>
        /// Boss saldırmaya başladığında ana saldırı dizisini tetikler.
        /// </summary>
        public override void OnStartAttacking()
        {
            base.OnStartAttacking();
            // Saldırı dizisini başlat
            this.StartTeleportSequence();
        }

        #endregion

        #region Private Methods (Özel Metotlar - Saldırı Dizisi)

        /// <summary>
        /// Saldırı dizisinin ilk adımı: Boss rastgele bir konuma hareket eder.
        /// </summary>
        private void StartTeleportSequence()
        {
            // Boss için harita üzerinde rastgele bir X ve Y konumu belirle
            int randomX = Game.Random.Next(600, 1300);
            int randomY = Game.Random.Next(300, 400);

            // Boss'u belirtilen konuma "fly" animasyonuyla hareket ettir
            Body.MoveTo(randomX, randomY, "fly", 1000, "fly", 8);

            // Hareket bittikten sonra bir sonraki adımı (saldırıyı hazırlama) çağır
            Body.CallFuction(new LivingCallBack(this.PrepareAttack), 0);
        }

        /// <summary>
        /// Saldırıyı hazırlama adımı: Boss animasyon oynatır ve bir şeyler söyler.
        /// </summary>
        private void PrepareAttack()
        {
            // "beatD" animasyonunu oynat
            Body.PlayMovie("beatD", 0, 1);

            // Belirlenmiş konuşma metinlerinden rastgele birini söyle
            Body.Say(BOSS_QUOTES[Game.Random.Next(0, BOSS_QUOTES.Length)], 1000, 0);

            // Bir sonraki adımı (ışınlanma efektlerini oluşturma) gecikmeli olarak çağır
            Body.CallFuction(new LivingCallBack(this.CreateTeleportEffects), PREPARE_ATTACK_DELAY);
        }

        /// <summary>
        /// Işınlanma efektlerini oluşturma adımı: Oyuncuların gideceği yerlere kara delik efektleri ekler.
        /// </summary>
        private void CreateTeleportEffects()
        {
            // Önceden belirlenmiş tüm X konumları için döngü oluştur
            foreach (int x in TELEPORT_X_POSITIONS)
            {
                // Her konuma "heidong" (kara delik) efektini oluştur
                ((PVEGame)Game).Createlayer(x, INITIAL_TELEPORT_Y, "moive", "asset.game.nine.heidong", "in", 1, 0, true);
            }

            // Bir sonraki adımı (oyuncuları asıl ışınlatma) gecikmeli olarak çağır
            Body.CallFuction(new LivingCallBack(this.ExecutePlayerTeleport), CREATE_EFFECTS_DELAY);
        }

        /// <summary>
        /// Oyuncuları ışınlatma adımı: Tüm oyuncuları rastgele konumlara anında ışınlar.
        /// </summary>
        private void ExecutePlayerTeleport()
        {
            // Tüm canlı oyuncuları al
            List<Player> allPlayers = Game.GetAllLivingPlayers();

            // Her oyuncu için
            foreach (Player player in allPlayers)
            {
                // Oyuncuyu rastgele bir ışınlanma noktasına gönder
                int targetX = TELEPORT_X_POSITIONS[Game.Random.Next(0, TELEPORT_X_POSITIONS.Length)];
                player.BoltMove(targetX, INITIAL_TELEPORT_Y, 100);
            }

            // Kamerayı rastgele bir ışınlanma noktasına odakla
            int randomFocusX = TELEPORT_X_POSITIONS[Game.Random.Next(0, TELEPORT_X_POSITIONS.Length)];
            ((PVEGame)Game).SendFreeFocus(randomFocusX, INITIAL_TELEPORT_Y, 1, 0, 2000);

            // Bir sonraki adımı (oyuncuları döndürme) gecikmeli olarak çağır
            Body.CallFuction(new LivingCallBack(this.SpinPlayers), SPIN_DELAY);
        }

        /// <summary>
        /// Oyuncuları döndürme adımı: Tüm oyuncuları kendi etraflarında 5 kez döndürür.
        /// </summary>
        private void SpinPlayers()
        {
            // Tüm canlı oyuncuları al
            List<Player> allPlayers = Game.GetAllLivingPlayers();

            // Her oyuncu için
            foreach (Player player in allPlayers)
            {
                // Oyuncuyu 1800 derece (5 tam tur) döndür
                Game.SendLivingTurnRotation(player, TOTAL_ROTATION_DEGREES, ROTATION_SPEED, "");
            }

            // Döndürme işlemi bittikten sonra son adımı (saldıyı bitirme) çağır
            Body.CallFuction(new LivingCallBack(this.FinalizeAttack), 0);
        }

        /// <summary>
        /// Saldırı dizisinin son adımı: Oyuncuları son konumlarına ışınlar ve saldırır.
        /// </summary>
        private void FinalizeAttack()
        {
            // Tüm canlı oyuncuları al
            List<Player> allPlayers = Game.GetAllLivingPlayers();

            // Her oyuncu için
            foreach (Player player in allPlayers)
            {
                // NOT: Bu satır, döndürme animasyonu sonrası oyuncunun yönünü sıfırlamak için olabilir.
                // Görsel olarak ani bir "sekme" yaratabilir.
                Game.SendLivingTurnRotation(player, 0, 0, "");

                // Oyuncuyu yeni bir rastgele X konumuna ve sabit bir Y konumuna ışınla
                int targetX = TELEPORT_X_POSITIONS[Game.Random.Next(0, TELEPORT_X_POSITIONS.Length)];
                player.BoltMove(targetX, FINAL_ATTACK_Y_POS, 2000);

                // Oyuncunun yeni konumuna alan saldırısı yap
                Body.RangeAttacking(player.X - 100, player.X + 100, "cryA", 0, null);

                // Kamerayı saldırılan oyuncunun üzerine odakla
                ((PVEGame)Game).SendFreeFocus(player.X, FINAL_ATTACK_Y_POS, 1, 0, 2000);
            }
        }

        #endregion
    }
}