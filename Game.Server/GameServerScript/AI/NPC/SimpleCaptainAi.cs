using Game.Logic.AI;
using Game.Logic.Phy.Object;
using System.Collections.Generic;
using System.Drawing;

namespace GameServerScript.AI.NPC
{
    public class SimpleCaptainAi : ABrain
    {
        // Saldırı aşamasını takip eden değişken (0, 1, 2 döngüsü)
        private int currentAttackPhase;

        public int currentCount;
        public int Dander;

        // Çağrılacak yardımcı NPC'nin ID'si
        private int minionNpcId;

        public List<SimpleNpc> Children;

        // Sohbet metinleri (Türkçeleştirildi)
        private static string[] GroundSlamMessages;   // string_0
        private static string[] ShootMessages;        // string_1
        private static string[] TauntMessages;        // string_2 (Kullanılmıyor ancak kodda var)
        private static string[] SummonMessages;       // string_3
        private static string[] HurtMessages;         // string_4 (Kullanılmıyor ancak kodda var)
        private static string[] JumpMessages;         // string_5 (Kullanılmıyor ancak kodda var)
        private static string[] ProximityMessages;    // string_6

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            base.Body.CurrentDamagePlus = 2f;
            base.Body.CurrentShootMinus = 2f;

            // NPC'nin çarpışma alanını (Rect) ayarlama
            base.Body.SetRect(((SimpleBoss)base.Body).NpcInfo.X, ((SimpleBoss)base.Body).NpcInfo.Y, ((SimpleBoss)base.Body).NpcInfo.Width, ((SimpleBoss)base.Body).NpcInfo.Height);

            if (base.Body.Direction == -1)
            {
                base.Body.SetRect(((SimpleBoss)base.Body).NpcInfo.X, ((SimpleBoss)base.Body).NpcInfo.Y, ((SimpleBoss)base.Body).NpcInfo.Width, ((SimpleBoss)base.Body).NpcInfo.Height);
            }
            else
            {
                base.Body.SetRect(-((SimpleBoss)base.Body).NpcInfo.X - ((SimpleBoss)base.Body).NpcInfo.Width, ((SimpleBoss)base.Body).NpcInfo.Y, ((SimpleBoss)base.Body).NpcInfo.Width, ((SimpleBoss)base.Body).NpcInfo.Height);
            }
        }

        public override void OnCreated()
        {
            base.OnCreated();
        }

        public override void OnStartAttacking()
        {
            base.Body.Direction = base.Game.FindlivingbyDir(base.Body);
            bool isPlayerInZone = false;
            int maxDistance = 0;

            // Oyuncuların belirli bir bölgede olup olmadığını kontrol et
            foreach (Player allFightPlayer in base.Game.GetAllFightPlayers())
            {
                if (allFightPlayer.IsLiving && allFightPlayer.X > 500 && allFightPlayer.X < 1050)
                {
                    int distance = (int)base.Body.Distance(allFightPlayer.X, allFightPlayer.Y);
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                    }
                    isPlayerInZone = true;
                }
            }

            // Eğer oyuncu tehlikeli bölgedeyse (yakınsa) özel saldırı
            if (isPlayerInZone)
            {
                PerformProximityAttack(500, 1050);
            }
            // Değilse sıralı saldırı desenlerini uygula
            else if (currentAttackPhase == 0)
            {
                PerformGroundSlamAttack();
                currentAttackPhase++;
            }
            else if (currentAttackPhase == 1)
            {
                PerformMoveAndShootAttack();
                currentAttackPhase++;
            }
            else
            {
                PerformSummonMinions();
                currentAttackPhase = 0;
            }
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

        /// <summary>
        /// Yakın mesafedeki oyunculara alan hasarı verir.
        /// </summary>
        private void PerformProximityAttack(int minRange, int maxRange)
        {
            PlayShakeEffect(3);
            int messageIndex = base.Game.Random.Next(0, ProximityMessages.Length);
            base.Body.Say(ProximityMessages[messageIndex], 1, 1000);
            base.Body.CurrentDamagePlus = 9999f;
            base.Body.PlayMovie("beat2", 3000, 0);
            base.Body.RangeAttacking(minRange, maxRange, "cry", 5000, null);
        }

        /// <summary>
        /// Yere düşüp yüksek hasarlı deprem saldırısı yapar.
        /// </summary>
        private void PerformGroundSlamAttack()
        {
            PlayShakeEffect(3);
            base.Body.CurrentDamagePlus = 4f;
            int messageIndex = base.Game.Random.Next(0, GroundSlamMessages.Length);
            base.Body.Say(GroundSlamMessages[messageIndex], 1, 0);
            base.Body.FallFrom(base.Body.X, 509, null, 1000, 1, 12);
            base.Body.PlayMovie("beat2", 1000, 0);
            base.Body.RangeAttacking(base.Body.X - 1000, base.Body.X + 1000, "cry", 4000, null);
        }

        /// <summary>
        /// Rastgele bir noktaya yürür ve ardından ateş eder.
        /// </summary>
        private void PerformMoveAndShootAttack()
        {
            PlayShakeEffect(3);
            int messageIndex = base.Game.Random.Next(0, ShootMessages.Length);
            base.Body.Say(ShootMessages[messageIndex], 1, 0);
            int targetX = base.Game.Random.Next(670, 880);

            // Yürüdükten sonra ateş etme fonksiyonunu tetikle
            base.Body.MoveTo(targetX, base.Body.Y, "walk", 1000, "", 4, OnMoveCompleteShootPlayer);
            base.Body.ChangeDirection(base.Game.FindlivingbyDir(base.Body), 9000);
        }

        /// <summary>
        /// Takviye kuvvet (minyon) çağırır.
        /// </summary>
        private void PerformSummonMinions()
        {
            PlayShakeEffect(3);
            base.Body.JumpTo(base.Body.X, base.Body.Y - 300, "Jump", 1000, 1);
            int messageIndex = base.Game.Random.Next(0, SummonMessages.Length);
            base.Body.Say(SummonMessages[messageIndex], 1, 3300);
            base.Body.PlayMovie("call", 3500, 0);
            base.Body.CallFuction(CreateChild, 4000);
        }

        /// <summary>
        /// Hareket tamamlandıktan sonra rastgele bir oyuncuya ateş eder.
        /// </summary>
        private void OnMoveCompleteShootPlayer()
        {
            Player player = base.Game.FindRandomPlayer();
            base.Body.SetRect(0, 0, 0, 0);

            if (player != null)
            {
                // Oyuncunun yönüne göre yönel
                if (player.X > base.Body.X) // Orijinal kodda base.Body.Y idi, mantıksal olarak X olmalı
                {
                    base.Body.ChangeDirection(1, 500);
                }
                else
                {
                    base.Body.ChangeDirection(-1, 500);
                }

                base.Body.CurrentDamagePlus = 2f;

                int targetX = base.Game.Random.Next(player.X - 50, player.X + 50);

                // Birinci atış
                if (base.Body.ShootPoint(targetX, player.Y, 61, 1000, 10000, 1, 1f, 2200))
                {
                    base.Body.PlayMovie("beat", 1700, 0);
                }
                // İkinci atış
                if (base.Body.ShootPoint(targetX, player.Y, 61, 1000, 10000, 1, 1f, 3200))
                {
                    base.Body.PlayMovie("beat", 2700, 0);
                }
            }
        }

        /// <summary>
        /// Boss'un sağa sola sallanma animasyonunu oynatır.
        /// </summary>
        private void PlayShakeEffect(int count)
        {
            int direction = base.Body.Direction;
            for (int i = 0; i < count; i++)
            {
                base.Body.ChangeDirection(-direction, i * 200 + 100);
                base.Body.ChangeDirection(direction, (i + 1) * 100 + i * 200);
            }
        }

        private Point[] birthPoints = { new Point(600, 539), new Point(950, 539) };

        public void CreateChild()
        {
            ((SimpleBoss)Body).CreateChild(minionNpcId, birthPoints, 8, 2, 1);
        }

        public SimpleCaptainAi()
        {
            minionNpcId = 1009;
            Children = new List<SimpleNpc>();
        }

        static SimpleCaptainAi()
        {
            // Sohbet metinlerinin Türkçeleştirilmesi
            GroundSlamMessages = new string[3]
            {
                "Büyük Yer <br/> Sarsıntısı .... <br/> Sarsılın......",
                "Kendinizi ölüme gönderiyorsunuz!",
                "Yenilmez Süper Deprem <br/> ...... GRR ... GRR ......"
            };

            ShootMessages = new string[2]
            {
                "Bakın şimdi size ateş edeceğim.",
                "Yüzünüzü patlatacağım!"
            };

            TauntMessages = new string[3]
            {
                "Aşağı in, hapse git tatlım!",
                "Beni yenebileceğini mi sanıyorsun?",
                "Beni yenmeyi istemeden önce biraz daha antrenman yap."
            };

            SummonMessages = new string[2]
            {
                "Muhafızlar! <br/> Koruyun beni!!",
                "Sizler, Acemiler! <br/> Bana yardım edin!"
            };

            HurtMessages = new string[3]
            {
                "Ah kahretsin...",
                "Bana ateş etmeye cüret mi ediyorsun GRRR...",
                "Çok acıttı, seni affetmeyeceğim!!!"
            };

            JumpMessages = new string[3]
            {
                "Bana mı ateş ediyorsun? Dene bakalım, <br/> bana ateş et!",
                "Yukarı çıkın! Yükseğe zıplayın!",
                "Yüksek! <br/> Daha yükseğe!"
            };

            ProximityMessages = new string[4]
            {
                "Yakınımda hayatta kalacağını mı sanıyorsun?",
                "Ölümün kucağına atlayan bir aptal.",
                "Bana bu kadar yakın ne arıyorsun ha?",
                "Benimle uğraşacak yaşta bile değilsin!!!"
            };
        }
    }
}