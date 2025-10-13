using System;
using System.Collections;
using System.Collections.Generic; // List<> kullanabilmek için eklendi
using System.IO;
using System.Net;
using System.Threading;
using System.Timers;
using Bussiness;
using Game.Server.GameObjects;
using Game.Server.Managers;
using Game.Service.actions;
using SqlDataProvider.Data;

namespace Game.Service
{
    // Token: 0x02000005 RID: 5
    internal class Program
    {
        // --- YENÝ EKLENEN KISIM: Ayarlar ve Listeler ---

        // Rastgele ödül sisteminde kullanýlacak ödül yapýsý
        public class RastgeleOdul
        {
            public int ItemID { get; set; }
            public int Sayi { get; set; }
            public string KazanmaMesaji { get; set; }
        }

        // Rastgele verilecek ödüllerin listesi. Burayý dilediðin gibi düzenleyebilirsin.
        private static readonly List<RastgeleOdul> _rastgeleOduller = new List<RastgeleOdul>
        {
            new RastgeleOdul { ItemID = 11025, Sayi = 2, KazanmaMesaji = "Þanslý saat! Herkes 2 adet 'Büyük Hoparlör' kazandý!" },
            new RastgeleOdul { ItemID = 11003, Sayi = 5, KazanmaMesaji = "Tebrikler! Sunucuya özel hediye olarak 5 adet 'Level 3 Güç Taþý' kazandýnýz!" },
            new RastgeleOdul { ItemID = 200501, Sayi = 10, KazanmaMesaji = "Sürpriz! Anlýk ödül olarak tüm oyunculara 10 adet 'Onur Özü' gönderildi!" },
            new RastgeleOdul { ItemID = 11925, Sayi = 1, KazanmaMesaji = "Ne kadar þanslýsýnýz! Bu saate özel olarak herkese 1 adet 'Rastgele Kart Kutusu' hediye!" }
        };

        // Rastgele gönderilecek sunucu mesajlarý listesi. Burayý dilediðin gibi düzenleyebilirsin.
        private static readonly List<string> _sunucuMesajlari = new List<string>
        {
            "[Duyuru] Sunucumuzda hile, bug veya 3. parti yazýlým kullanýmý kalýcý olarak yasaklanma sebebidir. Lütfen adil bir oyun ortamý için kurallara uyun.",
            "[Ýpucu] Günlük görevleri tamamlayarak deðerli ödüller kazanabileceðinizi unutmayýn!",
            "[Hatýrlatma] Takým arkadaþlarýnýzla iletiþim kurmak zaferin anahtarýdýr!",
            "[Duyuru] Herhangi bir sorunla karþýlaþýrsanýz oyun yöneticilerine bildirmekten çekinmeyin. Keyifli oyunlar!",
            "[Bilgi] Unutma, en güçlü silah bilgidir! Sitemizdeki rehberlere göz atarak oyununu geliþtirebilirsin."
        };

        // Rastgele olaylarý tetiklemek için kullanýlacak Random nesnesi
        private static readonly Random _random = new Random();

        // --- Mevcut Kod ---

        // Token: 0x0600000E RID: 14
        private static void Timer_Olayi(object source, ElapsedEventArgs e)
        {
            foreach (GamePlayer gamePlayer in WorldMgr.GetAllPlayers())
            {
                gamePlayer.UpdateFightPower();
                gamePlayer.SavePlayerInfo();
                
            }
            new WebClient().DownloadString("http://109.122.6.15/request/celeblist/createallceleb.ashx");
            Console.WriteLine("Onur listesi güncellendi!");
            GamePlayer[] allPlayers2 = WorldMgr.GetAllPlayers();
            for (int i = 0; i < allPlayers2.Length; i++)
            {
                allPlayers2[i].SendMessage("Onur Listesi güncellenmiþtir!");
            }
        }

        // Token: 0x0600000F RID: 15 RVA: 0x000020A5 File Offset: 0x000002A5
        public static void TimerBaslat()
        {
            Program.kontrol_araligi.Elapsed += Program.Timer_Olayi;
            Program.kontrol_araligi.AutoReset = true;
            Program.kontrol_araligi.Enabled = true;
        }

        // Token: 0x06000010 RID: 16 RVA: 0x000025E4 File Offset: 0x000007E4
        
   

        // Token: 0x06000011 RID: 17 RVA: 0x00002664 File Offset: 0x00000864
        private static void Timer_Olayi3(object source, ElapsedEventArgs e)
        {
            GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
            for (int i = 0; i < allPlayers.Length; i++)
            {
                allPlayers[i].SendMessage("Discord sunucumuza katýlarak etkinliklerden ve çekiliþlerden haberdar olabilirsiniz: https://discord.gg/bombomria");
            }
            Console.WriteLine("Discord linki chate gönderildi.");
        }

        // --- YENÝ EKLENEN KISIM: Dinamik Olaylar Timer'ý ---

        private static void DinamikOlaylariKontrolEt(object source, ElapsedEventArgs e)
        {
            GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
            if (allPlayers.Length == 0) return; // Sunucuda oyuncu yoksa hiçbir þey yapma

            int rastgeleSayi = _random.Next(1, 101); // 1 ile 100 arasýnda bir sayý tut

            // %5 ihtimalle rastgele ödül ver. Bu oraný deðiþtirebilirsin (örn: <= 2 yaparsan %2 olur)
            if (rastgeleSayi <= 5)
            {
                // Ödül listesinden rastgele bir ödül seç
                RastgeleOdul odul = _rastgeleOduller[_random.Next(_rastgeleOduller.Count)];

                Console.WriteLine("Rastgele ödül zamaný! '" + odul.KazanmaMesaji + "' ödülü tüm oyunculara gönderiliyor.");

                // Tüm oyunculara hem mesaj gönder hem de ödülü yolla
                foreach (GamePlayer player in allPlayers)
                {
                    player.SendMessage(odul.KazanmaMesaji);
                    new PlayerBussiness().SendMailAndItem("Sürpriz Hediye!", "Tebrikler, sunucunun bu saatinde bu ödüle denk geldin! Ýyi oyunlar.", player.PlayerCharacter.ID, odul.ItemID, odul.Sayi, 0, 0, 0, 0, 0, 0, 0, 0, true);
                }
            }
            // %20 ihtimalle rastgele bir sunucu mesajý gönder.
            else if (rastgeleSayi <= 25) // 5 (ödül) + 20 (mesaj) = 25
            {
                // Mesaj listesinden rastgele bir mesaj seç
                string mesaj = _sunucuMesajlari[_random.Next(_sunucuMesajlari.Count)];

                Console.WriteLine("Rastgele sunucu mesajý gönderiliyor: '" + mesaj + "'");

                // Tüm oyunculara mesajý gönder
                foreach (GamePlayer player in allPlayers)
                {
                    player.SendMessage(mesaj);
                }
            }
        }

        // Token: 0x06000012 RID: 18 RVA: 0x000020D3 File Offset: 0x000002D3


        // Token: 0x06000013 RID: 19 RVA: 0x00002101 File Offset: 0x00000301
        public static void TimerBaslat3()
        {
            Program.kontrol_araligi3.Elapsed += Program.Timer_Olayi3;
            Program.kontrol_araligi3.AutoReset = true;
            Program.kontrol_araligi3.Enabled = true;
        }

        // --- YENÝ EKLENEN KISIM: Dinamik Olaylar Baþlatma Metodu ---
        public static void TimerBaslatDinamik()
        {
            Program.dinamik_olay_timeri.Elapsed += Program.DinamikOlaylariKontrolEt;
            Program.dinamik_olay_timeri.AutoReset = true;
            Program.dinamik_olay_timeri.Enabled = true;
        }

        // Token: 0x06000014 RID: 20 RVA: 0x0000269C File Offset: 0x0000089C
        [MTAThread]
        private static void Main(string[] args)
        {
            // Mevcut Timer'larýn baþlatýlmasý
            Program.TimerBaslat();
            Program.TimerBaslat3();

            // YENÝ EKLENEN DÝNAMÝK TÝMER'IN BAÞLATILMASI
            Program.TimerBaslatDinamik();

            Console.WriteLine("Tüm Timer'lar Baþlatýldý (Onur Listesi, Online Ödül, Discord ve Dinamik Olaylar).");

            AppDomain.CurrentDomain.SetupInformation.PrivateBinPath = "." + Path.DirectorySeparatorChar.ToString() + "lib";
            Thread.CurrentThread.Name = "MAIN";
            Program.RegisterActions();
            if (args.Length == 0)
            {
                args = new string[]
                {
                    "--start"
                };
            }
            string name;
            Hashtable parameters;
            try
            {
                Program.ParseParameters(args, out name, out parameters);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            IAction action = Program.GetAction(name);
            if (action != null)
            {
                action.OnAction(parameters);
                return;
            }
            Program.ShowSyntax();
        }

        // --- Geri Kalan Kod (Deðiþiklik Yok) ---

        // Token: 0x06000015 RID: 21 RVA: 0x0000212F File Offset: 0x0000032F
        private static void RegisterActions()
        {
            Program.RegisterAction(new ServiceRun());
            Program.RegisterAction(new ConsoleStart());
            Program.RegisterAction(new ServiceInstall());
            Program.RegisterAction(new ServiceUninstall());
            Program.RegisterAction(new ServiceStart());
            Program.RegisterAction(new ServiceStop());
        }

        // Token: 0x06000016 RID: 22 RVA: 0x0000216D File Offset: 0x0000036D
        private static void RegisterAction(IAction action)
        {
            if (action == null)
            {
                throw new ArgumentException("Action can't be bull", "actioni");
            }
            Program._actions.Add(action);
        }

        // Token: 0x06000017 RID: 23 RVA: 0x00002750 File Offset: 0x00000950
        public static void ShowSyntax()
        {
            Console.WriteLine("Syntax: RoadServer.exe {action} [param1=value1] [param2=value2] ...");
            Console.WriteLine("Possible actions:");
            foreach (object obj in Program._actions)
            {
                IAction action = (IAction)obj;
                if (action.Syntax != null && action.Description != null)
                {
                    Console.WriteLine(string.Format("{0,-20}\t{1}", action.Syntax, action.Description));
                }
            }
        }

        // Token: 0x06000018 RID: 24 RVA: 0x000027E0 File Offset: 0x000009E0
        private static IAction GetAction(string name)
        {
            foreach (object obj in Program._actions)
            {
                IAction action = (IAction)obj;
                if (action.Name.Equals(name))
                {
                    return action;
                }
            }
            return null;
        }

        // Token: 0x06000019 RID: 25 RVA: 0x00002848 File Offset: 0x00000A48
        private static void ParseParameters(string[] args, out string actionName, out Hashtable parameters)
        {
            parameters = new Hashtable();
            actionName = null;
            if (!args[0].StartsWith("--"))
            {
                throw new ArgumentException("First argument must be the action");
            }
            actionName = args[0];
            if (args.Length != 1)
            {
                for (int i = 1; i < args.Length; i++)
                {
                    string text = args[i];
                    if (text.StartsWith("--"))
                    {
                        throw new ArgumentException("At least two actions given and only one action allowed!");
                    }
                    if (text.StartsWith("-"))
                    {
                        int num = text.IndexOf('=');
                        if (num == -1)
                        {
                            parameters.Add(text, "");
                        }
                        else
                        {
                            string key = text.Substring(0, num);
                            string value = "";
                            if (num + 1 < text.Length)
                            {
                                value = text.Substring(num + 1);
                            }
                            parameters.Add(key, value);
                        }
                    }
                }
            }
        }

        // Token: 0x0600001A RID: 26 RVA: 0x0000218E File Offset: 0x0000038E
        public Program()
        {
        }

        // Token: 0x0600001B RID: 27 RVA: 0x0000290C File Offset: 0x00000B0C
        static Program()
        {
        }

        // Token: 0x04000003 RID: 3
        public static System.Timers.Timer kontrol_araligi = new System.Timers.Timer(1800000.0); // 30 dakika

        // Token: 0x04000004 RID: 4
        public static System.Timers.Timer kontrol_araligi2 = new System.Timers.Timer(60000.0); // 1 dakika

        // Token: 0x04000005 RID: 5
        public static System.Timers.Timer kontrol_araligi3 = new System.Timers.Timer(1800000.0); // 30 dakika

        // YENÝ EKLENEN TÝMER
        // Dinamik olaylar için timer. 5 dakikada bir çalýþýr (300000.0 milisaniye).
        // Bu süreyi kýsaltarak olaylarýn daha sýk olmasýný saðlayabilirsin.
        public static System.Timers.Timer dinamik_olay_timeri = new System.Timers.Timer(300000.0);

        // Token: 0x04000006 RID: 6
        private static ArrayList _actions = new ArrayList();
    }
}