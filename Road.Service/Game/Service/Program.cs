using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Timers;
using Bussiness;
using Game.Server.Managers;
using Game.Service.actions;

namespace Game.Service
{
    internal sealed class Program
    {
        // ── Statik Alanlar ────────────────────────────────────────────────────────────

        // ArrayList yerine generic List<T>: boxing/unboxing yok, daha az GC baskısı
        private static readonly List<IAction> _actions = new List<IAction>();

        // Timer türünü tam nitelendirilmiş isimle belirtiyoruz: Timer çakışmasını önler
        private static readonly System.Timers.Timer _onurListesiTimer = new System.Timers.Timer(1800000.0); // 30 dk
        private static readonly System.Timers.Timer _discordTimer = new System.Timers.Timer(1800000.0); // 30 dk
        private static readonly System.Timers.Timer _dinamikOlayTimer = new System.Timers.Timer(300000.0); // 5 dk

        private static readonly Random _random = new Random();

        // ── Giriş Noktası ────────────────────────────────────────────────────────────

        [MTAThread]
        private static void Main(string[] args)
        {
            Console.WriteLine("Başlatılıyor...");

            AppDomain.CurrentDomain.SetupInformation.PrivateBinPath =
                "." + Path.DirectorySeparatorChar.ToString() + "lib";

            Thread.CurrentThread.Name = "MAIN";

            RegisterActions();
            StartTimers();

            if (args.Length == 0)
                args = new string[] { "--start" };

            string actionName;
            Hashtable parameters; // IAction.OnAction(Hashtable) imzasıyla uyumlu kalıyoruz

            try
            {
                ParseParameters(args, out actionName, out parameters);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            IAction action = GetAction(actionName);
            if (action != null)
                action.OnAction(parameters);
            else
                ShowSyntax();
        }

        // ── Aksiyon Yönetimi ─────────────────────────────────────────────────────────

        private static void RegisterActions()
        {
            _actions.Add(new ServiceRun());
            _actions.Add(new ConsoleStart());
            _actions.Add(new ServiceInstall());
            _actions.Add(new ServiceUninstall());
            _actions.Add(new ServiceStart());
            _actions.Add(new ServiceStop());
        }

        private static IAction GetAction(string name)
        {
            // foreach üzerinde IAction — boxing yok (generic List)
            foreach (IAction action in _actions)
            {
                if (action.Name.Equals(name, StringComparison.Ordinal))
                    return action;
            }
            return null;
        }

        public static void ShowSyntax()
        {
            Console.WriteLine("Syntax: RoadServer.exe {action} [param1=value1] [param2=value2] ...");
            Console.WriteLine("Possible actions:");
            foreach (IAction action in _actions)
            {
                if (action.Syntax != null && action.Description != null)
                    Console.WriteLine(string.Format("  {0,-20}\t{1}", action.Syntax, action.Description));
            }
        }

        // ── Parametre Ayrıştırma ──────────────────────────────────────────────────────
        // Hashtable bırakıldı: IAction.OnAction(Hashtable) imzasıyla uyumluluk zorunlu

        private static void ParseParameters(string[] args, out string actionName, out Hashtable parameters)
        {
            parameters = new Hashtable();
            actionName = null;

            if (!args[0].StartsWith("--"))
                throw new ArgumentException("First argument must be the action");

            actionName = args[0];

            for (int i = 1; i < args.Length; i++)
            {
                string arg = args[i];

                if (arg.StartsWith("--"))
                    throw new ArgumentException("At least two actions given; only one action allowed!");

                if (arg.StartsWith("-"))
                {
                    int eq = arg.IndexOf('=');
                    if (eq == -1)
                    {
                        parameters[arg] = string.Empty;
                    }
                    else
                    {
                        string key = arg.Substring(0, eq);
                        string value = (eq + 1 < arg.Length) ? arg.Substring(eq + 1) : string.Empty;
                        parameters[key] = value;
                    }
                }
            }
        }

        // ── Timer Başlatma ────────────────────────────────────────────────────────────

        private static void StartTimers()
        {
            _onurListesiTimer.Elapsed += OnurListesiTimer_Elapsed;
            _onurListesiTimer.AutoReset = true;
            _onurListesiTimer.Enabled = true;

            _discordTimer.Elapsed += DiscordTimer_Elapsed;
            _discordTimer.AutoReset = true;
            _discordTimer.Enabled = true;

            _dinamikOlayTimer.Elapsed += DinamikOlayTimer_Elapsed;
            _dinamikOlayTimer.AutoReset = true;
            _dinamikOlayTimer.Enabled = true;
        }

        // ── Timer Olayları ────────────────────────────────────────────────────────────

        /// <summary>
        /// Her 30 dakikada bir:
        ///   1) Tüm oyuncuların savaş gücünü günceller ve kaydeder.
        ///   2) Onur listesi web servisini tetikler.
        /// WebClient tek bir using bloğunda oluşturulup hemen dispose edilir;
        /// böylece bağlantı kaynakları anında serbest bırakılır.
        /// </summary>
        private static void OnurListesiTimer_Elapsed(object source, ElapsedEventArgs e)
        {
            GamePlayer[] players = WorldMgr.GetAllPlayers();

            foreach (GamePlayer player in players)
            {
                player.UpdateFightPower();
                player.SavePlayerInfo();
            }

            try
            {
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadString("https://trbombom.com/ddt-quest-s1/celeblist/createallceleb.ashx");
                }
                Console.WriteLine("Onur listesi güncellendi!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("[HATA] Onur listesi güncellenemedi: " + ex.Message);
            }
        }

        /// <summary>
        /// Her 30 dakikada bir Discord linkini oyunculara göndermek için kullanılır.
        /// Şu an devre dışı; etkinleştirmek için aşağıdaki satırları uncomment yapın.
        /// </summary>
        private static void DiscordTimer_Elapsed(object source, ElapsedEventArgs e)
        {
            // GamePlayer[] players = WorldMgr.GetAllPlayers();
            // foreach (GamePlayer player in players)
            //     player.SendMessage("Discord sunucumuza katılın: discord.gg/bombom");
            // Console.WriteLine("Discord linki chate gönderildi.");
        }

        /// <summary>
        /// Her 5 dakikada bir dinamik olayları kontrol eder.
        /// Oyuncu yoksa erken çıkar; gereksiz işlem yapmaz.
        /// </summary>
        private static void DinamikOlayTimer_Elapsed(object source, ElapsedEventArgs e)
        {
            GamePlayer[] players = WorldMgr.GetAllPlayers();
            if (players.Length == 0)
                return;

            int roll = _random.Next(1, 101);

            // Sunucu mesajı (aktif etmek için uncomment yapın):
            // if (roll <= 25)
            // {
            //     string msg = _sunucuMesajlari[_random.Next(_sunucuMesajlari.Length)];
            //     foreach (GamePlayer player in players)
            //         player.SendMessage(msg);
            //     Console.WriteLine("Sunucu mesajı gönderildi: " + msg);
            // }

            // Hafta sonu ödül sistemi (aktif etmek için uncomment yapın):
            // bool haftaSonu = DateTime.Now.DayOfWeek == DayOfWeek.Saturday
            //               || DateTime.Now.DayOfWeek == DayOfWeek.Sunday;
            // if (haftaSonu && roll <= 5)
            //     VerRastgeleOdul(players);
        }

        // ── Rastgele Ödül Sistemi (Devre Dışı) ───────────────────────────────────────
        // Etkinleştirmek için bu metodu ve _rastgeleOduller dizisini uncomment yapın.

        // private static void VerRastgeleOdul(GamePlayer[] players)
        // {
        //     RastgeleOdul odul = _rastgeleOduller[_random.Next(_rastgeleOduller.Length)];
        //     Console.WriteLine("Rastgele ödül: " + odul.KazanmaMesaji);
        //     PlayerBussiness pb = new PlayerBussiness();
        //     foreach (GamePlayer player in players)
        //     {
        //         player.SendMessage(odul.KazanmaMesaji);
        //         pb.SendMailAndItem(
        //             "Tebrikler, bu saatte online olduğun için ödül kazandın! İyi oyunlar.",
        //             "Online Etkinlik Sistemi",
        //             player.PlayerCharacter.ID,
        //             odul.ItemID, odul.Sayi,
        //             0, 0, 0, 0, 0, 0, 0, 0, true);
        //     }
        // }

        // private static readonly RastgeleOdul[] _rastgeleOduller = new RastgeleOdul[]
        // {
        //     new RastgeleOdul { ItemID = 11101, Sayi = 5,  KazanmaMesaji = "Şanslı saat! Herkes 5 adet 'Küçük Hoparlör' kazandı!" },
        //     new RastgeleOdul { ItemID = 11102, Sayi = 3,  KazanmaMesaji = "Şanslı saat! Herkes 3 adet 'Büyük Hoparlör' kazandı!" },
        //     new RastgeleOdul { ItemID = 11019, Sayi = 5,  KazanmaMesaji = "Şanslı saat! Herkes 5 adet '1. Seviye Güçlendirme Taşı' kazandı!" },
        //     new RastgeleOdul { ItemID = 11021, Sayi = 4,  KazanmaMesaji = "Şanslı saat! Herkes 4 adet '2. Seviye Güçlendirme Taşı' kazandı!" },
        //     new RastgeleOdul { ItemID = 11022, Sayi = 3,  KazanmaMesaji = "Şanslı saat! Herkes 3 adet '3. Seviye Güçlendirme Taşı' kazandı!" },
        //     new RastgeleOdul { ItemID = 11023, Sayi = 2,  KazanmaMesaji = "Şanslı saat! Herkes 2 adet '4. Seviye Güçlendirme Taşı' kazandı!" },
        //     new RastgeleOdul { ItemID = 11024, Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet '5. Seviye Güçlendirme Taşı' kazandı!" },
        //     new RastgeleOdul { ItemID = 11020, Sayi = 3,  KazanmaMesaji = "Şanslı saat! Herkes 3 adet 'Kutsallık Sembolü' kazandı!" },
        //     new RastgeleOdul { ItemID = 311199,Sayi = 4,  KazanmaMesaji = "Şanslı saat! Herkes 4 adet '1. Seviye Saldırı İncisi' kazandı!" },
        //     new RastgeleOdul { ItemID = 312199,Sayi = 4,  KazanmaMesaji = "Şanslı saat! Herkes 4 adet '1. Seviye Savunma İncisi' kazandı!" },
        //     new RastgeleOdul { ItemID = 313199,Sayi = 4,  KazanmaMesaji = "Şanslı saat! Herkes 4 adet '1. Seviye Nitelik İncisi' kazandı!" },
        //     new RastgeleOdul { ItemID = 311299,Sayi = 3,  KazanmaMesaji = "Şanslı saat! Herkes 3 adet '2. Seviye Saldırı İncisi' kazandı!" },
        //     new RastgeleOdul { ItemID = 312299,Sayi = 3,  KazanmaMesaji = "Şanslı saat! Herkes 3 adet '2. Seviye Savunma İncisi' kazandı!" },
        //     new RastgeleOdul { ItemID = 313299,Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet '2. Seviye Nitelik İncisi' kazandı!" },
        //     new RastgeleOdul { ItemID = 311399,Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet '3. Seviye Saldırı İncisi' kazandı!" },
        //     new RastgeleOdul { ItemID = 312399,Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet '3. Seviye Savunma İncisi' kazandı!" },
        //     new RastgeleOdul { ItemID = 313399,Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet '3. Seviye Nitelik İncisi' kazandı!" },
        //     new RastgeleOdul { ItemID = 311499,Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet '4. Seviye Saldırı İncisi' kazandı!" },
        //     new RastgeleOdul { ItemID = 312499,Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet '4. Seviye Savunma İncisi' kazandı!" },
        //     new RastgeleOdul { ItemID = 313499,Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet '4. Seviye Nitelik İncisi' kazandı!" },
        //     new RastgeleOdul { ItemID = 40001, Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet '1. Seviye Eğitim İksiri' kazandı!" },
        //     new RastgeleOdul { ItemID = 40002, Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet '2. Seviye Eğitim İksiri' kazandı!" },
        //     new RastgeleOdul { ItemID = 40003, Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet '3. Seviye Eğitim İksiri' kazandı!" },
        //     new RastgeleOdul { ItemID = 40004, Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet '4. Seviye Eğitim İksiri' kazandı!" },
        //     new RastgeleOdul { ItemID = 315001,Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet '1. Seviye Kutsal Taşı Yenile' kazandı!" },
        //     new RastgeleOdul { ItemID = 315002,Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet '2. Seviye Kutsal Taşı Yenile' kazandı!" },
        //     new RastgeleOdul { ItemID = 315003,Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet '3. Seviye Kutsal Taşı Yenile' kazandı!" },
        //     new RastgeleOdul { ItemID = 315004,Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet '4. Seviye Kutsal Taşı Yenile' kazandı!" },
        //     new RastgeleOdul { ItemID = 315005,Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet '5. Seviye Kutsal Taşı Yenile' kazandı!" },
        //     new RastgeleOdul { ItemID = 315006,Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet '6. Seviye Kutsal Taşı Yenile' kazandı!" },
        //     new RastgeleOdul { ItemID = 8002,  Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet 'Şans Künyesi' kazandı!" },
        //     new RastgeleOdul { ItemID = 8003,  Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet 'Çeviklik Künyesi' kazandı!" },
        //     new RastgeleOdul { ItemID = 8004,  Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet 'Canavar Saldırısı' kazandı!" },
        //     new RastgeleOdul { ItemID = 8006,  Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet 'Sağlam Zincir' kazandı!" },
        //     new RastgeleOdul { ItemID = 9002,  Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet 'Kanatlı Yüzük' kazandı!" },
        //     new RastgeleOdul { ItemID = 9003,  Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet 'Şans Yüzüğü' kazandı!" },
        //     new RastgeleOdul { ItemID = 9005,  Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet 'Okyanus Yüzüğü' kazandı!" },
        //     new RastgeleOdul { ItemID = 9006,  Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet 'Alevli Yüzük' kazandı!" },
        //     new RastgeleOdul { ItemID = 20101, Sayi = 25, KazanmaMesaji = "Şanslı saat! Herkes 25 adet 'Kırmızı Sihirli Karınca Kart Kutusu' kazandı!" },
        //     new RastgeleOdul { ItemID = 20102, Sayi = 25, KazanmaMesaji = "Şanslı saat! Herkes 25 adet 'Mavi Sihirli Karınca Kart Kutusu' kazandı!" },
        //     new RastgeleOdul { ItemID = 20103, Sayi = 25, KazanmaMesaji = "Şanslı saat! Herkes 25 adet 'Sihirli Karınca Kraliçesi Kart Kutusu' kazandı!" },
        //     new RastgeleOdul { ItemID = 20104, Sayi = 25, KazanmaMesaji = "Şanslı saat! Herkes 25 adet 'Pembe Bogolu Kart Kutusu' kazandı!" },
        //     new RastgeleOdul { ItemID = 20105, Sayi = 25, KazanmaMesaji = "Şanslı saat! Herkes 25 adet 'Bogo Eliti Kart Kutusu' kazandı!" },
        //     new RastgeleOdul { ItemID = 20106, Sayi = 25, KazanmaMesaji = "Şanslı saat! Herkes 25 adet 'Bogo Lideri Kart Kutusu' kazandı!" },
        //     new RastgeleOdul { ItemID = 20107, Sayi = 25, KazanmaMesaji = "Şanslı saat! Herkes 25 adet 'Bogo Kralı Kart Kutusu' kazandı!" },
        //     new RastgeleOdul { ItemID = 20108, Sayi = 25, KazanmaMesaji = "Şanslı saat! Herkes 25 adet 'Kabile Savaşçısı Kart Kutusu' kazandı!" },
        //     new RastgeleOdul { ItemID = 20109, Sayi = 25, KazanmaMesaji = "Şanslı saat! Herkes 25 adet 'Kabile Askeri Kart Kutusu' kazandı!" },
        //     new RastgeleOdul { ItemID = 20110, Sayi = 25, KazanmaMesaji = "Şanslı saat! Herkes 25 adet 'Kabile Kardeşinin Abi Kart Kutusu' kazandı!" },
        //     new RastgeleOdul { ItemID = 20111, Sayi = 25, KazanmaMesaji = "Şanslı saat! Herkes 25 adet 'Kabile Kardeşinin Kardeş Kart Kutusu' kazandı!" },
        //     new RastgeleOdul { ItemID = 20112, Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet 'Kabile Reisi Kart Kutusu' kazandı!" },
        //     new RastgeleOdul { ItemID = 20113, Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet 'Sihirli Kartal Kart Kutusu' kazandı!" },
        //     new RastgeleOdul { ItemID = 20114, Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet 'Sihirli Kurt Kart Kutusu' kazandı!" },
        //     new RastgeleOdul { ItemID = 20115, Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet 'Kırmızı Cüppeli Şeytan Kart Kutusu' kazandı!" },
        //     new RastgeleOdul { ItemID = 20116, Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet 'Minotar Kart Kutusu' kazandı!" },
        //     new RastgeleOdul { ItemID = 20117, Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet 'İlahi Fırtına Kart Kutusu' kazandı!" },
        //     new RastgeleOdul { ItemID = 20118, Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet 'Bom Kart Kutusu' kazandı!" },
        //     new RastgeleOdul { ItemID = 20119, Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet 'Öfke Ateşi Kart Kutusu' kazandı!" },
        //     new RastgeleOdul { ItemID = 20120, Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet 'Yıldırım Kart Kutusu' kazandı!" },
        //     new RastgeleOdul { ItemID = 20121, Sayi = 1,  KazanmaMesaji = "Şanslı saat! Herkes 1 adet 'Barabran Kölesi Kart Kutusu' kazandı!" },
        // };

        // ── Sunucu Mesajları (Devre Dışı) ─────────────────────────────────────────────
        // private static readonly string[] _sunucuMesajlari = new string[]
        // {
        //     "Duyuru! Sunucumuzda hile, bug veya 3. parti yazılım kullanımı kalıcı ban sebebidir.",
        //     "İpucu: Günlük görevleri tamamlayarak değerli ödüller kazanabilirsiniz!",
        //     "Hatırlatma! Discord'umuza katılarak etkinliklerden haberdar olun: discord.gg/bombom",
        //     "Duyuru! Sorun yaşarsanız oyun yöneticilerine bildirin. Keyifli oyunlar!",
        //     "Bilgi: En güçlü silah bilgidir! Sitemizdeki rehberlere göz atarak gelişin."
        // };

        // ── İç Sınıf ─────────────────────────────────────────────────────────────────

        public sealed class RastgeleOdul
        {
            public int ItemID { get; set; }
            public int Sayi { get; set; }
            public string KazanmaMesaji { get; set; }
        }
    }
}