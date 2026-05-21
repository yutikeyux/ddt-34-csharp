using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Bussiness;
using Bussiness.Managers;
using Game.Base;
using Game.Logic;
using Game.Server;
using Game.Server.Managers;
using Game.Server.Rooms;
using log4net;
using SqlDataProvider.Data;

namespace Game.Service.actions
{
    public class ConsoleStart : IAction
    {
        // ── IAction Özellikleri ───────────────────────────────────────────────────────

        public string Name => "--start";
        public string Syntax => "--start [-config=./config/serverconfig.xml]";
        public string Description => "Starts the DOL server in console mode";

        // ── Win32 İçe Aktarımları ─────────────────────────────────────────────────────

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        private static extern bool ReadConsoleW(
            IntPtr hConsoleInput,
            [Out] byte[] lpBuffer,
            uint nNumberOfCharsToRead,
            out uint lpNumberOfCharsRead,
            IntPtr lpReserved);

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int SetConsoleCtrlHandler(ConsoleCtrlDelegate handlerRoutine, bool add);

        // ── Statik Alanlar ────────────────────────────────────────────────────────────

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static System.Threading.Timer _timer;
        private static int _count;

        // Online sayaç timer'ı — property yerine düz field (property gereği yok)
        private static System.Threading.Timer _timer2;
        private static int _count2;

        private static ConsoleCtrlDelegate _handler;

        // ── Yapılandırma ──────────────────────────────────────────────────────────────

        private GameServerConfig _config;

        // ── Yardımcı Metodlar ─────────────────────────────────────────────────────────

        public static IntPtr GetWin32InputHandle()
        {
            return GetStdHandle(-10);
        }

        public static void NewForm()
        {
            Application.Run(new ServerManagementForm());
        }

        // ── Online Sayaç Timer Callback ───────────────────────────────────────────────

        /// <summary>
        /// Her dakika çalışır; sayaç sıfırlandığında online oyuncu sayısını
        /// tüm oyunculara gönderir ve sayacı sıfırlar.
        /// </summary>
        private static void OnlineSayacCallback(object state)
        {
            _count2--;
            Console.WriteLine(string.Format("Komut verildi. {0} dakika sonra mesaj gönderilecek!", _count2));

            if (_count2 != 0)
                return;

            GameClient[] allClients = GameServer.Instance.GetAllClients();
            int onlineCount = (allClients != null) ? allClients.Length : 0;

            GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
            for (int i = 0; i < allPlayers.Length; i++)
            {
                allPlayers[i].SendMessage(string.Format(
                    "Sistem : Şuanda oyunda {0} kişi online! |TrBombom 2027|", onlineCount));
            }

            Console.WriteLine("Online sayısı gönderildi.");
            _count2 = 1;
        }

        // ── Kapatma Timer Callback ────────────────────────────────────────────────────

        private static void ShutDownCallback(object state)
        {
            _count--;
            Console.WriteLine(string.Format("Server will shutdown after {0} mins!", _count));

            foreach (GameClient client in GameServer.Instance.GetAllClients())
            {
                if (client.Out != null)
                {
                    client.Out.SendMessage(eMessageType.GM_NOTICE, string.Format("{0}{1}{2}",
                        LanguageMgr.GetTranslation("Game.Service.actions.ShutDown1", Array.Empty<object>()),
                        _count,
                        LanguageMgr.GetTranslation("Game.Service.actions.ShutDown2", Array.Empty<object>())));
                }
            }

            if (_count != 0)
                return;

            _timer.Dispose();
            _timer = null;
            GameServer.Instance.Stop();
            Console.WriteLine("Server has stopped!");
            GameServer.KeepRunning = false;
            Environment.Exit(0);
        }

        // ── Console Ctrl Handler ──────────────────────────────────────────────────────

        private static int ConsoleCtrHandler(ConsoleEvent e)
        {
            SetConsoleCtrlHandler(_handler, false);
            if (GameServer.Instance != null)
                GameServer.Instance.Stop();
            return 0;
        }

        // ── Ana Aksiyon ───────────────────────────────────────────────────────────────

        public void OnAction(Hashtable parameters)
        {
            Console.Title = "TrBombom Main Service";
            Console.ForegroundColor = ConsoleColor.Green;

            GameServer.CreateInstance(_config = new GameServerConfig());

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Road Başlatılıyor...");
            GameServer.Instance.Start();
            GameServer.KeepRunning = true;

            FusionCombined.ListCombinedFusion();

            Console.WriteLine("Server Online!");

            ConsoleClient consoleClient = new ConsoleClient();
            new Thread(ConsoleStart.NewForm).Start();

            // Ctrl+C / kapat sinyalini yakala
            _handler = new ConsoleCtrlDelegate(ConsoleCtrHandler);
            SetConsoleCtrlHandler(_handler, true);

            // ── Ana Konsol Döngüsü ────────────────────────────────────────────────────
            while (GameServer.KeepRunning)
            {
                try
                {
                    Console.Write("=> ");
                    string input = Console.ReadLine();

                    if (string.IsNullOrEmpty(input))
                        continue;

                    string cmd = input.Split(' ')[0];

                    switch (cmd)
                    {
                        case "reset":
                            Console.Clear();
                            continue;

                        case "admin":
                            HandleAdminMenu(consoleClient, input);
                            continue;
                    }

                    // /komut → &komut dönüşümü
                    if (input[0] == '/')
                        input = "&" + input.Substring(1);

                    if (!CommandMgr.HandleCommandNoPlvl(consoleClient, input))
                        Console.WriteLine("Bilinmeyen komut: " + input);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            if (GameServer.Instance != null)
                GameServer.Instance.Stop();

            LogManager.Shutdown();
        }

        // ── Yönetim Menüsü ────────────────────────────────────────────────────────────

        /// <summary>
        /// "admin" komutu girildiğinde yönetim menüsünü gösterir ve seçimi işler.
        /// OnAction'dan ayrı bir metoda taşındı: tek sorumluluk, daha kolay bakım.
        /// </summary>
        private static void HandleAdminMenu(ConsoleClient client, string originalInput)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Yönetim Konsolu.");
            Console.WriteLine("Lütfen numara seçin:");
            Console.WriteLine(" 1.  Mesaj Gönder");
            Console.WriteLine(" 2.  Nick'e Ban At");
            Console.WriteLine(" 3.  Kullanıcı Adına Ban At");
            Console.WriteLine(" 4.  Oyunu 1 Dakika Sonra Kapat (!)");
            Console.WriteLine(" 5.  Oyunu Hemen Kapat (!)");
            Console.WriteLine(" 6.  Online Durumu ve RAM Kullanımı");
            Console.WriteLine(" 7.  Nick'e Kick At");
            Console.WriteLine(" 8.  Oyuncu Ban Kaldırma");
            Console.WriteLine(" 9.  Online Oyuncu Sayısını Otomatik Gönder");
            Console.WriteLine("10.  Veritabanı Değişikliklerini Kaydet");
            Console.WriteLine("11.  Online (Kupon) Etkinliği");
            Console.WriteLine("12.  Online (Onur Özü) Etkinliği");
            Console.WriteLine("13.  Online (Kart Ruhu) Etkinliği");
            Console.WriteLine("14.  Online (Exp GP) Etkinliği");
            Console.WriteLine("15.  Online (İtem) Etkinliği");
            Console.WriteLine("16.  Özel Mesaj Gönder");
            Console.WriteLine("17.  Özel (Road) Sistemini Aç");
            Console.Write("Seçiminizi Girin: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    SendBroadcastMessage();
                    break;

                case "2":
                    BanByNick();
                    break;

                case "3":
                    BanByUsername();
                    break;

                case "4":
                    KickAndShutdown();
                    break;

                case "5":
                    GameServer.KeepRunning = false;
                    break;

                case "6":
                    ShowServerStatus();
                    break;

                case "7":
                    KickPlayer();
                    break;

                case "8":
                    UnbanPlayer();
                    break;

                case "9":
                    StartOnlineSayac();
                    break;

                case "10":
                    ReloadAllManagers();
                    break;

                case "11":
                    SendKuponEvent();
                    break;

                case "12":
                    SendOnurEvent();
                    break;

                case "13":
                    SendKartRuhuEvent();
                    break;

                case "14":
                    SendExpGpEvent();
                    break;

                case "15":
                    SendItemEvent();
                    break;

                case "16":
                    SendAlertMessage();
                    break;

                case "17":
                    new Thread(ConsoleStart.NewForm).Start();
                    break;

                default:
                    Console.WriteLine("Geçersiz seçim.");
                    break;
            }
        }

        // ── Yönetim Komut Metodları ───────────────────────────────────────────────────

        private static void SendBroadcastMessage()
        {
            Console.Clear();
            Console.Write("Mesajınızı Giriniz: ");
            string msg = Console.ReadLine();
            foreach (GamePlayer player in WorldMgr.GetAllPlayers())
                player.SendMessage("Yönetim : " + msg);
            Console.WriteLine("Mesaj gönderildi.");
        }

        private static void SendAlertMessage()
        {
            Console.Clear();
            Console.Write("Mesajınızı Giriniz: ");
            string msg = Console.ReadLine();
            foreach (GamePlayer player in WorldMgr.GetAllPlayers())
                player.Out.SendMessage(eMessageType.ALERT, "[YÖNETİM]: " + msg);
            Console.WriteLine("Mesaj gönderildi.");
        }

        private static void BanByNick()
        {
            Console.Clear();
            Console.WriteLine("Banlanacak oyuncunun Nick'i: ");
            string nick = Console.ReadLine();
            Console.WriteLine("Ban sebebi: ");
            string reason = Console.ReadLine();
            DateTime banEnd = ReadDateFromConsole();

            using (ManageBussiness mb = new ManageBussiness())
                mb.ForbidPlayerByNickName(nick, banEnd, false, reason);

            string broadcastMsg = string.Format(
                "Oyuncumuz <{0}> oyun kurallarına aykırı davranışı nedeniyle BANLANMIŞTIR. " +
                "Ban sebebi: ({1}). Açılış: ({2:dd.MM.yyyy})", nick, reason, banEnd);

            foreach (GamePlayer player in WorldMgr.GetAllPlayers())
                player.SendMessage(broadcastMsg);

            Console.WriteLine("Oyuncu " + nick + " banlandı ve kicklendi.");
        }

        private static void BanByUsername()
        {
            Console.Clear();
            Console.WriteLine("Banlanacak kullanıcı adı: ");
            string username = Console.ReadLine();
            Console.WriteLine("Ban sebebi: ");
            string reason = Console.ReadLine();
            DateTime banEnd = ReadDateFromConsole();

            using (ManageBussiness mb = new ManageBussiness())
                mb.ForbidPlayerByUserName(username, banEnd, false, reason);

            string broadcastMsg = string.Format(
                "<{0}> kullanıcı adlı oyuncu kuralları çiğnediğinden uzaklaştırıldı. " +
                "Ban sebebi: ({1}). Açılış: ({2:dd.MM.yyyy})", username, reason, banEnd);

            foreach (GamePlayer player in WorldMgr.GetAllPlayers())
                player.SendMessage(broadcastMsg);

            Console.WriteLine("Oyuncu banlandı.");
        }

        private static void KickAndShutdown()
        {
            foreach (GamePlayer player in WorldMgr.GetAllPlayers())
            {
                player.SendMessage("Admin Tarafından Oyundan Atıldınız!");
                player.Disconnect();
            }
            GameServer.KeepRunning = false;
            Console.WriteLine("Oyun kontrollü bir şekilde kapandı.");
        }

        private static void ShowServerStatus()
        {
            GameClient[] clients = GameServer.Instance.GetAllClients();
            int onlineCount = (clients != null) ? clients.Length : 0;

            List<BaseRoom> rooms = RoomMgr.GetAllUsingRoom();
            int usedRooms = 0;
            int playingRooms = 0;

            foreach (BaseRoom room in rooms)
            {
                if (!room.IsEmpty)
                {
                    usedRooms++;
                    if (room.IsPlaying) playingRooms++;
                }
            }

            double ramMB = (double)GC.GetTotalMemory(false) / 1024.0 / 1024.0;

            Console.WriteLine(string.Format("Online oyuncu     : {0}", onlineCount));
            Console.WriteLine(string.Format("Dolu oda / savaşta: {0} oda, {1} kişi", usedRooms, playingRooms));
            Console.WriteLine(string.Format("RAM kullanımı     : {0:F2} MB", ramMB));
        }

        private static void KickPlayer()
        {
            Console.Clear();
            Console.WriteLine("Kick yiyecek oyuncunun nick'i: ");
            string nick = Console.ReadLine();
            Console.WriteLine("Kick sebebi: ");
            string reason = Console.ReadLine();

            using (ManageBussiness mb = new ManageBussiness())
                mb.KitoffUserByNickName(nick, " ");

            string broadcastMsg = string.Format(
                "Oyuncu [{0}] rahatsızlık verdiği için kicklenmiştir. Sebep: ({1})", nick, reason);

            foreach (GamePlayer player in WorldMgr.GetAllPlayers())
                player.SendMessage(broadcastMsg);
        }

        private static void UnbanPlayer()
        {
            Console.Clear();
            Console.WriteLine("Banı açılacak oyuncunun Nick'i: ");
            string nick = Console.ReadLine();
            DateTime futureDate = new DateTime(2050, 7, 2);

            using (ManageBussiness mb = new ManageBussiness())
                mb.ForbidPlayerByNickName(nick, futureDate, true);

            foreach (GamePlayer player in WorldMgr.GetAllPlayers())
                player.SendMessage("Oyuncumuz <" + nick + "> banı sona ermiştir.");

            Console.WriteLine("Oyuncu " + nick + " banı açıldı.");
        }

        private static void StartOnlineSayac()
        {
            _count2 = 31;
            _timer2 = new System.Threading.Timer(OnlineSayacCallback, null, 0, 60000);
        }

        private static void SendKuponEvent()
        {
            Console.Write("Gönderilecek kupon miktarı: ");
            int amount = int.Parse(Console.ReadLine());
            foreach (GamePlayer player in WorldMgr.GetAllPlayers())
            {
                player.AddMoney(amount);
                player.SendMessage(string.Format(
                    "Tebrikler! Tüm online oyunculara {0} kupon gönderildi. Etkinlik sona erdi.", amount));
            }
            Console.WriteLine("Kupon gönderildi: " + amount);
        }

        private static void SendOnurEvent()
        {
            Console.Write("Gönderilecek onur miktarı: ");
            int amount = int.Parse(Console.ReadLine());
            foreach (GamePlayer player in WorldMgr.GetAllPlayers())
            {
                player.AddHonor(amount);
                player.SendMessage(string.Format(
                    "Tebrikler! Tüm online oyunculara {0} onur gönderildi. Etkinlik sona erdi.", amount));
            }
            Console.WriteLine("Onur gönderildi: " + amount);
        }

        private static void SendKartRuhuEvent()
        {
            Console.Write("Gönderilecek Kart Ruhu miktarı: ");
            int amount = int.Parse(Console.ReadLine());
            foreach (GamePlayer player in WorldMgr.GetAllPlayers())
            {
                player.SendMessage(string.Format(
                    "Tebrikler! Tüm online oyunculara {0} Kart Ruhu gönderildi. Etkinlik sona erdi.", amount));
            }
            Console.WriteLine("Kart Ruhu gönderildi: " + amount);
        }

        private static void SendExpGpEvent()
        {
            Console.Write("Gönderilecek EXP (GP) miktarı: ");
            int amount = int.Parse(Console.ReadLine());
            foreach (GamePlayer player in WorldMgr.GetAllPlayers())
            {
                player.AddGP(amount);
                player.SendMessage(string.Format(
                    "Tebrikler! Tüm online oyunculara {0} EXP (GP) gönderildi. Etkinlik sona erdi.", amount));
            }
            Console.WriteLine("EXP (GP) gönderildi: " + amount);
        }

        private static void SendItemEvent()
        {
            Console.Write("Başlık: "); string title = Console.ReadLine();
            Console.Write("İçerik: "); string content = Console.ReadLine();
            Console.Write("İtem ID: "); int templateID = int.Parse(Console.ReadLine());
            Console.Write("Adet: "); int count = int.Parse(Console.ReadLine());
            Console.Write("Gün: "); int validDate = int.Parse(Console.ReadLine());
            Console.Write("Altın: "); int gold = int.Parse(Console.ReadLine());
            Console.Write("Kupon: "); int money = int.Parse(Console.ReadLine());
            Console.Write("Level: "); int strengthenLvl = int.Parse(Console.ReadLine());
            Console.Write("Atak: "); int attackCompose = int.Parse(Console.ReadLine());
            Console.Write("Defans: "); int defendCompose = int.Parse(Console.ReadLine());
            Console.Write("Çeviklik: "); int agilityCompose = int.Parse(Console.ReadLine());
            Console.Write("Şans: "); int luckCompose = int.Parse(Console.ReadLine());
            Console.Write("Bağlı (True/False): "); bool isBinds = bool.Parse(Console.ReadLine());

            PlayerBussiness pb = new PlayerBussiness();

            foreach (GamePlayer player in WorldMgr.GetAllPlayers())
            {
                pb.SendMailAndItem(title, content, player.PlayerCharacter.ID,
                    templateID, count, validDate, gold, money,
                    strengthenLvl, attackCompose, defendCompose, agilityCompose, luckCompose, isBinds);

                player.SendMessage("[ Online Oyuncu Etkinliği ] Hediye ödüller gönderilmiştir.");

                using (ManageBussiness mb = new ManageBussiness())
                    mb.SystemNotice("Sistem Yöneticisi: Hediye ödüller gönderilmiştir.");
            }
        }

        private static void ReloadAllManagers()
        {
            ReloadManager("Ball", () => BallMgr.ReLoad());
            ReloadManager("Map", () => MapMgr.ReLoadMap());
            ReloadManager("MapServer", () => MapMgr.ReLoadMapServer());
            ReloadManager("PropItem", () => PropItemMgr.Reload());
            ReloadManager("Item", () => ItemMgr.ReLoad());
            ReloadManager("Shop", () => ShopMgr.ReLoad());
            ReloadManager("Quest", () => QuestMgr.ReLoad());
            ReloadManager("Fusion", () => FusionMgr.ReLoad());
            ReloadManager("Consortia", () => ConsortiaMgr.ReLoad());
            ReloadManager("Rate", () => RateMgr.ReLoad());
            ReloadManager("NPCInfo", () => NPCInfoMgr.ReLoad());
            ReloadManager("FightRate", () => FightRateMgr.ReLoad());
            ReloadManager("DailyAward", () => AwardMgr.ReLoad());
            ReloadManager("Language", () => LanguageMgr.Reload(""));
        }

        /// <summary>
        /// Tek bir manager yeniden yükleme adımını gerçekleştirir ve sonucu loglar.
        /// Tüm reload çağrılarının aynı try/catch + log kalıbını tekrar etmesini önler.
        /// </summary>
        private static void ReloadManager(string name, Func<bool> reloadFunc)
        {
            try
            {
                bool result = reloadFunc();
                Console.WriteLine(string.Format("{0} {1}.", name, result ? "güncelleniyor" : "güncellendi"));
                if (result)
                    Console.WriteLine(name + " güncellendi.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("[HATA] {0} güncellenemedi: {1}", name, ex.Message));
            }
        }

        // ── Yardımcı: Konsoldan Tarih Okuma ──────────────────────────────────────────

        /// <summary>
        /// Kullanıcıdan yıl/ay/gün girerek DateTime oluşturur.
        /// Tekrarlanan Console.ReadLine + int.Parse bloklarını ortadan kaldırır.
        /// </summary>
        private static DateTime ReadDateFromConsole()
        {
            Console.Write("Ban açılış yılı : "); int year = int.Parse(Console.ReadLine());
            Console.Write("Ban açılış ayı  : "); int month = int.Parse(Console.ReadLine());
            Console.Write("Ban açılış günü : "); int day = int.Parse(Console.ReadLine());
            return new DateTime(year, month, day);
        }

        // ── Delegate / Enum ───────────────────────────────────────────────────────────

        private delegate int ConsoleCtrlDelegate(ConsoleEvent ctrlType);

        private enum ConsoleEvent
        {
            Ctrl_C = 0,
            Ctrl_Break = 1,
            Close = 2,
            Logoff = 5,
            Shutdown = 6
        }
    }
}