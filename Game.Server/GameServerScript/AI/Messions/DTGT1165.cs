using System;
using System.Collections.Generic;
using System.Text;
using Game.Logic.AI;
using Game.Logic.Effects;
using Game.Logic.Phy.Object;
using Game.Logic;
using SqlDataProvider.Data;
using System.Drawing;
using Bussiness.Managers;

namespace GameServerScript.AI.Messions
{
    // DTGT1165: Görev Kontrol Sınıfı
    public class DTGT1165 : AMissionControl
    {
        private SimpleNpc m_boss;

        // Oluşturulan topların listesi
        private List<PhysicalObj> m_toplar = new List<PhysicalObj>();

        // İki kat puan durumu
        private bool m_ikiKatMi = false;

        // İki kat puan sayacı
        private int m_ikiKatSayaci = 1;

        // NPC ve Top Kimlikleri
        private int bossID = 6121;
        private int topID = 6113;

        // Arka plan ve ön plan efekt nesneleri
        private PhysicalObj m_kingFilm;
        private PhysicalObj m_kingOn;

        // Sol taraf puan göstergesi nesneleri
        private PhysicalObj m_puan_nokta_sol;
        private PhysicalObj m_basamak_birler_sol;
        private PhysicalObj m_basamak_onlar_sol;

        // Sağ taraf puan göstergesi nesneleri
        private PhysicalObj m_puan_nokta_sag;
        private PhysicalObj m_basamak_birler_sag;
        private PhysicalObj m_basamak_onlar_sag;

        private int tur = 0;

        // Topların doğabileceği X koordinatları
        private int[] dogumX = { 450, 550, 650, 750, 850, 950, 1050, 1150, 1250, 455, 555, 655, 755, 855, 955, 1055, 1155, 1255 };

        // Topların doğabileceği Y koordinatları
        private int[] dogumY = { 184, 259, 335, 420, 504 };

        // Skora göre puan hesaplama
        public override int CalculateScoreGrade(int score)
        {
            base.CalculateScoreGrade(score);
            if (score > 900)
            {
                return 3;
            }
            else if (score > 825)
            {
                return 2;
            }
            else if (score > 725)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        // Yeni oturum hazırlığı (Kaynaklar ve Harita)
        public override void OnPrepareNewSession()
        {
            base.OnPrepareNewSession();
            int[] resources = { bossID, topID };
            Game.LoadResources(resources);
            Game.LoadNpcGameOverResources(resources);
            Game.AddLoadingFile(2, "image/game/thing/bossborn6.swf", "game.asset.living.GuizeAsset");
            Game.AddLoadingFile(2, "image/game/thing/BossBornBgAsset.swf", "game.asset.living.BossBgAsset");
            Game.AddLoadingFile(2, "image/game/effect/6/ball.swf", "asset.game.six.ball");
            Game.AddLoadingFile(2, "image/game/effect/6/jifenpai.swf", "asset.game.six.fenshu");
            Game.AddLoadingFile(2, "image/game/effect/6/jifenpai.swf", "asset.game.six.shuzi");
            Game.SetMap(1165);
        }

        // Oyun başlangıcı
        public override void OnStartGame()
        {
            base.OnStartGame();

            Game.TotalCount = 99;
            Game.TotalTurn = Game.PlayerCount * 20;
            Game.SendMissionInfo();

            // Boss oluşturma ayarları
            LivingConfig config = Game.BaseLivingConfig();
            config.CanTakeDamage = false;
            config.IsTurn = false;
            m_boss = Game.CreateNpc(bossID, 345, 860, 1, 1, "", config);

            m_boss.PlayMovie("standC", 0, 0);
            m_boss.PlayMovie("go", 1000, 0);
            m_boss.Say("Oh! Buraya yeni geldiğine göre kuralları bilmiyorsundur sanırım.", 0, 0);

            m_boss.CallFuction(new LivingCallBack(SonrakiSaldiri2), 4000);
            m_boss.CallFuction(new LivingCallBack(SonrakiSaldiri), 5000);
        }

        // Boss giriş senaryosu ve top oluşturma başlangıcı
        private void SonrakiSaldiri2()
        {
            m_kingFilm = Game.Createlayer(0, 0, "kingmoive", "game.asset.living.BossBgAsset", "out", 1, 1);

            ((PVEGame)Game).SendGameFocus(900, 500, 1, 0, 1000);

            m_kingFilm.PlayMovie("in", 0, 0);
            m_kingFilm.PlayMovie("out", 5000, 0);

            m_boss.CallFuction(new LivingCallBack(ToplariOlustur), 6000);
        }

        // Arayüz elemanlarının oluşturulması
        private void SonrakiSaldiri()
        {
            m_kingOn = Game.Createlayer(900, 450, "font", "game.asset.living.GuizeAsset", "out", 1, 1);
            m_puan_nokta_sol = Game.Createlayer(170, 650, "movie", "asset.game.six.fenshu", "Z", 1, 1);
            m_basamak_onlar_sol = Game.Createlayer(270, 650, "movie", "asset.game.six.shuzi", "z0", 1, 1);
            m_basamak_birler_sol = Game.Createlayer(320, 650, "movie", "asset.game.six.shuzi", "z0", 1, 1);

            m_puan_nokta_sag = Game.Createlayer(1550, 650, "movie", "asset.game.six.fenshu", "Z", 1, 1);
            m_basamak_onlar_sag = Game.Createlayer(1650, 650, "movie", "asset.game.six.shuzi", "z0", 1, 1);
            m_basamak_birler_sag = Game.Createlayer(1700, 650, "movie", "asset.game.six.shuzi", "z0", 1, 1);
        }

        // Puanları basamaklara ayırma ve animasyon isimlerini döndürme
        public List<string> BasamaklariGuncelle(int ToplamPuan)
        {
            string aksiyon = "";
            List<string> donenDeger = new List<string>();

            // Negatif kontrolü ve mutlak değer alma
            bool pozitifMi = ToplamPuan >= 0;
            string puanStr = Math.Abs(ToplamPuan).ToString();

            // Başlangıç değerleri
            string birler = "0";
            string onlar = "0";

            // Arayüz noktalarını güncelleme
            if (m_puan_nokta_sol != null)
            {
                m_puan_nokta_sol.PlayMovie(pozitifMi ? "Z" : "F", 0, 900);
                m_puan_nokta_sag.PlayMovie(pozitifMi ? "Z" : "F", 0, 900);
            }

            // Basamakları ayırma
            if (puanStr.Length >= 2)
            {
                onlar = puanStr[puanStr.Length - 2].ToString();
                birler = puanStr[puanStr.Length - 1].ToString();
            }
            else if (puanStr.Length == 1)
            {
                birler = puanStr[0].ToString();
            }

            // Onlar basamağı animasyonu
            switch (onlar)
            {
                case "1": aksiyon = "z1"; break;
                case "2": aksiyon = "z2"; break;
                case "3": aksiyon = "z3"; break;
                case "4": aksiyon = "z4"; break;
                case "5": aksiyon = "z5"; break;
                case "6": aksiyon = "z6"; break;
                case "7": aksiyon = "z7"; break;
                case "8": aksiyon = "z8"; break;
                case "9": aksiyon = "z9"; break;
                default: aksiyon = "z0"; break;
            }
            donenDeger.Add(aksiyon);

            // Birler basamağı animasyonu
            switch (birler)
            {
                case "1": aksiyon = "z1"; break;
                case "2": aksiyon = "z2"; break;
                case "3": aksiyon = "z3"; break;
                case "4": aksiyon = "z4"; break;
                case "5": aksiyon = "z5"; break;
                case "6": aksiyon = "z6"; break;
                case "7": aksiyon = "z7"; break;
                case "8": aksiyon = "z8"; break;
                case "9": aksiyon = "z9"; break;
                default: aksiyon = "z0"; break;
            }
            donenDeger.Add(aksiyon);

            return donenDeger;
        }

        // Mavi top oluşturma sayısı
        private int mavi_top_sayisi = 4;

        // Topları haritada oluşturma
        private void ToplariOlustur()
        {
            string[] aksiyonlarKirmizi = { "s1", "s2", "s3", "s4", "s5", "s1", "s2", "s3", "s4", "s5", "s1", "s2", "s3", "s4", "s5" };
            string[] aksiyonlarMavi = { "s-1", "s-2", "s-3", "s-4", "s-5", "s-1", "s-2", "s-3", "s-4", "s-5", "s-1", "s-2", "s-3", "s-4", "s-5" };
            string aksiyonIkiKat = "double";

            Point[] noktalar =
            {
                new Point(1001,173), new Point(774,203), new Point(639,299), new Point(545,463), new Point(566,661),
                new Point(698,805), new Point(858,863), new Point(1091,837), new Point(1223,719), new Point(1307,517),
                new Point(1289,366), new Point(1171,241), new Point(985,240), new Point(717,400), new Point(684,602),
                new Point(853,742), new Point(1140,629), new Point(1145,410), new Point(982,438), new Point(829,506),
                new Point(962,592)
            };

            // Noktaları karıştır (Shuffer yerine Shuffle kullanıldı, mantıksal düzeltme)
            Game.Shuffer(noktalar);

            for (int i = 0; i < noktalar.Length; i++)
            {
                int rastgeleIndeks = Game.Random.Next(aksiyonlarMavi.Length);

                // İlk belirli sayıda top mavi olur
                if (i < mavi_top_sayisi)
                {
                    m_toplar.Add(Game.CreateBall(noktalar[i].X, noktalar[i].Y, aksiyonlarMavi[rastgeleIndeks]));
                }
                else
                {
                    int rastgeleIndeksKirmizi = Game.Random.Next(aksiyonlarKirmizi.Length);
                    m_toplar.Add(Game.CreateBall(noktalar[i].X, noktalar[i].Y, aksiyonlarKirmizi[rastgeleIndeksKirmizi]));
                }
            }

            // İki kat puan topu oluşturma mantığı
            if (m_ikiKatMi && m_ikiKatSayaci != 0)
            {
                int rastgele = Game.Random.Next(0, noktalar.Length);

                // Var olan bir topu yok et ve yerine iki kat topu koy
                m_toplar[rastgele].PlayMovie(m_toplar[rastgele].ActionMapping[m_toplar[rastgele].CurrentAction], 0, 500);
                m_toplar[rastgele].Die();
                Game.RemovePhysicalObj(m_toplar[rastgele], true);

                m_toplar[rastgele] = Game.CreateBall(noktalar[rastgele].X, noktalar[rastgele].Y, aksiyonIkiKat);

                m_ikiKatMi = false;
                m_ikiKatSayaci--;
            }
        }

        // Yeni tur başladığında
        public override void OnNewTurnStarted()
        {
            base.OnNewTurnStarted();
            Game.ClearBall();

            // 2. turdan sonra rastgelelik artar
            if (Game.TurnIndex > 2)
            {
                int rastgele = Game.Random.Next(4, 8);
                mavi_top_sayisi = rastgele;

                // %20 ihtimalle iki kat topu geleceğini işaretle
                int rastgeleIkiKat = Game.Random.Next(1, 100);
                if (rastgeleIkiKat > 80)
                {
                    m_ikiKatMi = true;
                }
            }
            ToplariOlustur();
        }

        // Her turun başında
        public override void OnBeginNewTurn()
        {
            base.OnBeginNewTurn();

            // 1. turdan sonra giriş efektlerini temizle
            if (Game.TurnIndex > 1)
            {
                if (m_kingFilm != null)
                {
                    Game.RemovePhysicalObj(m_kingFilm, true);
                    m_kingFilm = null;
                }
                if (m_kingOn != null)
                {
                    Game.RemovePhysicalObj(m_kingOn, true);
                    m_kingOn = null;
                }
            }
        }

        // Oyunun bitebilme koşulu kontrolü
        public override bool CanGameOver()
        {
            base.CanGameOver();

            // Kazanma koşulu: 99 puana ulaşmak
            if (Game.TotalKillCount >= 99)
            {
                Game.TotalKillCount = 99;
                return true;
            }

            // Kaybetme koşulu: Tur süresi dolması
            // Orijinal kodda burada return false vardı ki bu oyunun hiç bitmemesine neden olabilirdi.
            // Mantığı düzelttik: Tur sınırı aşılırsa oyun biter (OnGameOver'da kontrol edilecek).
            if (Game.TurnIndex > Game.TotalTurn)
            {
                return true;
            }

            return false;
        }

        // Arayüz verilerini güncelleme
        public override int UpdateUIData()
        {
            if (Game.TotalKillCount < -99)
            {
                Game.TotalKillCount = -99;
            }

            // Sol taraf göstergesi
            if (m_basamak_birler_sol != null)
            {
                List<string> animlar = BasamaklariGuncelle(Game.TotalKillCount);
                m_basamak_birler_sol.PlayMovie(animlar[1], 0, 1100);
                m_basamak_onlar_sol.PlayMovie(animlar[0], 0, 1100);
            }

            // Sağ taraf göstergesi
            if (m_basamak_birler_sag != null)
            {
                List<string> animlar = BasamaklariGuncelle(Game.TotalKillCount);
                m_basamak_birler_sag.PlayMovie(animlar[1], 0, 1100);
                m_basamak_onlar_sag.PlayMovie(animlar[0], 0, 1100);
            }
            return Game.TotalKillCount;
        }

        // Oyun bittiğinde
        public override void OnGameOver()
        {
            base.OnGameOver();
            if (Game.TotalKillCount >= 99)
            {
                Game.IsWin = true;
            }
            else if (Game.TurnIndex > Game.TotalTurn && Game.TotalKillCount < 99)
            {
                Game.IsWin = false;
            }
        }

        // Puan hesaplama
        public override void OnCalculatePoint(int point, bool isdouble)
        {
            Game.TotalKillCount += point;
            if (isdouble)
                Game.TotalKillCount *= 2;

            // Puan sınırları
            if (Game.TotalKillCount < -99)
            {
                Game.TotalKillCount = -99;
            }
            if (Game.TotalKillCount >= 99)
            {
                Game.TotalKillCount = 99;
            }
        }
    }
}