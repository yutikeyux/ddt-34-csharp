using System.Collections.Generic;
using System.Drawing;
using Game.Logic;
using Game.Logic.AI;
using Game.Logic.Phy.Object;

namespace GameServerScript.AI.NPC
{
    public class UzayZamanBoss : ABrain
    {
        // Sıra takibi için sayaç
        private int int_0;

        // Hedef oyuncu
        private Player m_target;

        // Görsel efektler için liste
        private List<PhysicalObj> list_0;

        // Konuşma metinleri
        private string[] string_0; // Normal saldırı
        private string[] string_1; // Alan hasarı
        private string[] string_2; // Kaos atağı

        public override void OnBeginSelfTurn()
        {
            base.OnBeginSelfTurn();
        }

        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();
            // Her tur başında hasar çarpanını sıfırla
            m_body.CurrentDamagePlus = 1f;
            m_body.CurrentShootMinus = 1f;

            // Önceki turlardan kalan efektleri temizle
            if (list_0 != null)
            {
                foreach (PhysicalObj item in list_0)
                {
                    base.Game.RemovePhysicalObj(item, true);
                }
            }
            list_0 = new List<PhysicalObj>();
        }

        public override void OnCreated()
        {
            base.OnCreated();
            // Patronun yakın dövüş mesafesi
            base.Body.MaxBeatDis = 200;
        }

        public override void OnStartAttacking()
        {
            base.OnStartAttacking();
            int_0++;

            // 3 saldırı tipi arasında döngü
            switch (int_0)
            {
                case 1:
                    // Basit tekli saldırı
                    method_0();
                    break;
                case 2:
                    // Alan hasarı (Tüm harita)
                    method_2();
                    break;
                case 3:
                    // Işınlanma ve ağır hasar
                    method_4();
                    // Döngüyü başa al
                    int_0 = 0;
                    break;
            }
        }

        // --- SALDIRI 1: Basit Tekli Vuruş ---
        private void method_0()
        {
            m_target = base.Game.FindRandomPlayer();
            if (m_target != null)
            {
                // Rastgele bir şey söyle
                ((SimpleBoss)base.Body).RandomSay(string_0, 0, 1000, 0);

                // Hedefin yanına uç (Uzay-zaman teması için 'fly' animasyonu)
                base.Body.MoveTo(m_target.X, m_target.Y - 50, "fly", 1500, method_1, 10);
            }
        }

        // Vuruş animasyonu ve hasar
        private void method_1()
        {
            if (m_target != null && m_target.IsLiving)
            {
                base.Body.ChangeDirection(m_target, 100);
                base.Body.Beat(m_target, "beatA", 100, 1, 500, 1, 1);
            }
        }

        // --- SALDIRI 2: Alan Hasarı (Zaman Darbesi) ---
        private void method_2()
        {
            // Merkeze git
            base.Body.MoveTo(900, 500, "fly", 1000, method_3, 10);
        }

        private void method_3()
        {
            ((SimpleBoss)base.Body).RandomSay(string_1, 0, 1000, 0);
            base.Body.CurrentDamagePlus = 1.5f; // Biraz fazla hasar

            // Animasyonu oynat
            base.Body.PlayMovie("beatB", 1500, 0);

            // Tüm haritaya hasar ver (X: -1000'den +2000'e kadar)
            base.Body.RangeAttacking(base.Body.X - 2000, base.Body.X + 2000, "cry", 3000, directDamage: true);
        }

        // --- SALDIRI 3: Kaos Işınlanması ---
        private void method_4()
        {
            ((SimpleBoss)base.Body).RandomSay(string_2, 0, 1000, 0);

            // Rastgele bir konuma geç
            int randomX = base.Game.Random.Next(300, 1500);
            int randomY = base.Game.Random.Next(300, 600);

            // Işınlanma efekti (Görsel)
            PhysicalObj portal = ((PVEGame)base.Game).Createlayer(base.Body.X, base.Body.Y, "", "asset.game.4.lanhuo", "", 1, 1);
            list_0.Add(portal);

            // Işınlan ve saldır
            base.Body.JumpTo(randomX, randomY, "fly", 1500, 100, 10, method_5);
        }

        private void method_5()
        {
            // Işınlandığı yerde patlama
            base.Body.PlayMovie("beatC", 500, 0);

            // Etrafa yayılan hasar
            base.Body.RangeAttacking(base.Body.X - 200, base.Body.X + 200, "cry", 1500, directDamage: true);
        }

        public override void OnDie()
        {
            base.OnDie();
            base.Body.Say("Zaman... beni terk ediyor...", 0, 2000, 0);
        }

        public override void OnStopAttacking()
        {
            base.OnStopAttacking();
        }

        // Yapıcı metot (Değişkenlerin başlatılması)
        public UzayZamanBoss()
        {
            list_0 = new List<PhysicalObj>();

            // Normal saldırı sözleri
            string_0 = new string[3]
            {
                "Zamanın akıyor, durmayacak!",
                "Seni uzayın derinliklerine göndereceğim.",
                "Kaçış yok, kaderin bu."
            };

            // Alan hasarı sözleri
            string_1 = new string[3]
            {
                "ZAMAN DURSUN!",
                "Boyutlar arası geçiş başlıyor!",
                "Bu patlamayı durduramazsın!"
            };

            // Kaos atağı sözleri
            string_2 = new string[3]
            {
                "Beklemediğin yerden geleceğim!",
                "Işık hızıyla hareket ediyorum.",
                "Gözlerin beni takip edemez!"
            };
        }
    }
}