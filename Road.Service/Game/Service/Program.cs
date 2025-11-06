using Bussiness;
using Game.Server.Managers;
using Game.Service.actions;
using SqlDataProvider.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Timers;

namespace Game.Service
{
	// Token: 0x02000006 RID: 6
	internal class Program
	{
		// Token: 0x06000012 RID: 18 RVA: 0x000022C4 File Offset: 0x000004C4
		[MTAThread]
		private static void Main(string[] args)
		{
			Program.TimerBaslat();
			Program.TimerBaslat3();
			Program.TimerBaslatDinamik();
			Console.WriteLine("Başlatılıyor...");
			AppDomain.CurrentDomain.SetupInformation.PrivateBinPath = "." + Path.DirectorySeparatorChar.ToString() + "lib";
			Thread.CurrentThread.Name = "MAIN";
			Program.RegisterActions();
			bool flag = args.Length == 0;
			if (flag)
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
			bool flag2 = action != null;
			if (flag2)
			{
				action.OnAction(parameters);
			}
			else
			{
				Program.ShowSyntax();
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000023A4 File Offset: 0x000005A4
		private static void RegisterActions()
		{
			Program.RegisterAction(new ServiceRun());
			Program.RegisterAction(new ConsoleStart());
			Program.RegisterAction(new ServiceInstall());
			Program.RegisterAction(new ServiceUninstall());
			Program.RegisterAction(new ServiceStart());
			Program.RegisterAction(new ServiceStop());
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000023F4 File Offset: 0x000005F4
		private static void RegisterAction(IAction action)
		{
			bool flag = action == null;
			if (flag)
			{
				throw new ArgumentException("Action can't be bull", "actioni");
			}
			Program._actions.Add(action);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002428 File Offset: 0x00000628
		public static void ShowSyntax()
		{
			Console.WriteLine("Syntax: RoadServer.exe {action} [param1=value1] [param2=value2] ...");
			Console.WriteLine("Possible actions:");
			foreach (object obj in Program._actions)
			{
				IAction action = (IAction)obj;
				bool flag = action.Syntax != null && action.Description != null;
				if (flag)
				{
					Console.WriteLine(string.Format("{0,-20}\t{1}", action.Syntax, action.Description));
				}
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000024D0 File Offset: 0x000006D0
		private static IAction GetAction(string name)
		{
			foreach (object obj in Program._actions)
			{
				IAction action = (IAction)obj;
				bool flag = action.Name.Equals(name);
				if (flag)
				{
					return action;
				}
			}
			return null;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002548 File Offset: 0x00000748
		private static void ParseParameters(string[] args, out string actionName, out Hashtable parameters)
		{
			parameters = new Hashtable();
			actionName = null;
			bool flag = !args[0].StartsWith("--");
			if (flag)
			{
				throw new ArgumentException("First argument must be the action");
			}
			actionName = args[0];
			bool flag2 = args.Length != 1;
			if (flag2)
			{
				for (int i = 1; i < args.Length; i++)
				{
					string text = args[i];
					bool flag3 = text.StartsWith("--");
					if (flag3)
					{
						throw new ArgumentException("At least two actions given and only one action allowed!");
					}
					bool flag4 = text.StartsWith("-");
					if (flag4)
					{
						int num = text.IndexOf('=');
						bool flag5 = num == -1;
						if (flag5)
						{
							parameters.Add(text, "");
						}
						else
						{
							string key = text.Substring(0, num);
							string value = "";
							bool flag6 = num + 1 < text.Length;
							if (flag6)
							{
								value = text.Substring(num + 1);
							}
							parameters.Add(key, value);
						}
					}
				}
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002650 File Offset: 0x00000850
		static Program()
		{
			Program._actions = new ArrayList();
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00003108 File Offset: 0x00001308
		private static void Timer_Olayi(object source, ElapsedEventArgs e)
		{
			foreach (GamePlayer gamePlayer in WorldMgr.GetAllPlayers())
			{
				gamePlayer.UpdateFightPower();
				gamePlayer.SavePlayerInfo();
			}
			new WebClient().DownloadString("http://109.122.6.15/request/celeblist/createallceleb.ashx");
			Console.WriteLine("Onur listesi güncellendi!");
								//GamePlayer[] allPlayers2 = WorldMgr.GetAllPlayers();
								//for (int j = 0; j < allPlayers2.Length; j++)
								//{
								//allPlayers2[j].SendMessage("Onur Listesi güncellendi!");
								//}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00003189 File Offset: 0x00001389
		public static void TimerBaslat()
		{
			Program.kontrol_araligi.Elapsed += Program.Timer_Olayi;
			Program.kontrol_araligi.AutoReset = true;
			Program.kontrol_araligi.Enabled = true;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000031BC File Offset: 0x000013BC
		private static void Timer_Olayi3(object source, ElapsedEventArgs e)
		{
			GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
			for (int i = 0; i < allPlayers.Length; i++)
			{
				allPlayers[i].SendMessage("Discord sunucumuza katılarak etkinliklerden ve çekilişlerden haberdar olabilirsiniz: https://discord.gg/trbombom");
			}
			Console.WriteLine("Discord linki chate gönderildi.");
		}

        // Token: 0x0600001C RID: 28 RVA: 0x00003200 File Offset: 0x00001400
        private static void DinamikOlaylariKontrolEt(object source, ElapsedEventArgs e)
        {
            GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
            if (allPlayers.Length == 0)
            {
                return;
            }

			// Mevcut tarihin hafta sonu olup olmadığını kontrol et (Cumartesi veya Pazar)
			bool haftaSonuMu = DateTime.Now.DayOfWeek == DayOfWeek.Saturday ||
							   DateTime.Now.DayOfWeek == DayOfWeek.Sunday ||
							   DateTime.Now.DayOfWeek == DayOfWeek.Monday;
					

            int num = Program._random.Next(1, 101);

            // Sadece hafta sonu ise ödül verme mantığını çalıştır
            if (haftaSonuMu && num <= 5)
            {
                Program.RastgeleOdul rastgeleOdul = Program._rastgeleOduller[Program._random.Next(Program._rastgeleOduller.Count)];
                Console.WriteLine("Rastgele ödül zamanı! '" + rastgeleOdul.KazanmaMesaji + "' ödülü tüm oyunculara gönderiliyor.");
                foreach (GamePlayer gamePlayer in allPlayers)
                {
                    gamePlayer.SendMessage(rastgeleOdul.KazanmaMesaji);
                    new PlayerBussiness().SendMailAndItem("Tebrikler, sunucunun bu saatinde bu ödüle denk geldin! Bu ödülleri sadece haftasonu ve Pazartesi günü elde edebilirsiniz. İyi oyunlar.!", "TrBombom Online Etkinlik Sistemi", gamePlayer.PlayerCharacter.ID, rastgeleOdul.ItemID, rastgeleOdul.Sayi, 0, 0, 0, 0, 0, 0, 0, 0, true);

                }
                return;
            }

            // ... geri kalan kodunuz ...
            // Sunucu mesajları (her gün gönderilir)
            if (num <= 25)
            {
                string text = Program._sunucuMesajlari[Program._random.Next(Program._sunucuMesajlari.Count)];
                Console.WriteLine("Rastgele sunucu mesajı gönderiliyor: '" + text + "'");
                GamePlayer[] array2 = allPlayers;
                for (int i = 0; i < array2.Length; i++)
                {
                    array2[i].SendMessage(text);
                }
            }
        }

        // Token: 0x0600001D RID: 29 RVA: 0x00003357 File Offset: 0x00001557
        public static void TimerBaslat3()
		{
			Program.kontrol_araligi3.Elapsed += Program.Timer_Olayi3;
			Program.kontrol_araligi3.AutoReset = true;
			Program.kontrol_araligi3.Enabled = true;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00003389 File Offset: 0x00001589
		public static void TimerBaslatDinamik()
		{
			Program.dinamik_olay_timeri.Elapsed += Program.DinamikOlaylariKontrolEt;
			Program.dinamik_olay_timeri.AutoReset = true;
			Program.dinamik_olay_timeri.Enabled = true;
		}

		// Token: 0x04000005 RID: 5
		private static ArrayList _actions;

		// Token: 0x04000006 RID: 6
		private static readonly List<Program.RastgeleOdul> _rastgeleOduller = new List<Program.RastgeleOdul>
		{
			new Program.RastgeleOdul
			{
				ItemID = 11101,
				Sayi = 5,
				KazanmaMesaji = "Şanslı saat! Herkes 5 adet 'Küçük Hoparlör' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 11102,
				Sayi = 2,
				KazanmaMesaji = "Şanslı saat! Herkes 2 adet 'Büyük Hoparlör' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 11019,
				Sayi = 5,
				KazanmaMesaji = "Şanslı saat! Herkes 5 adet '1. Seviye Güçlendirme Taşı' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 11021,
				Sayi = 4,
				KazanmaMesaji = "Şanslı saat! Herkes 4 adet '2. Seviye Güçlendirme Taşı' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 11022,
				Sayi = 3,
				KazanmaMesaji = "Şanslı saat! Herkes 3 adet '3. Seviye Güçlendirme Taşı' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 11023,
				Sayi = 2,
				KazanmaMesaji = "Şanslı saat! Herkes 2 adet '4. Seviye Güçlendirme Taşı' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 11024,
				Sayi = 1,
				KazanmaMesaji = "Şanslı saat! Herkes 1 adet '5. Seviye Güçlendirme Taşı' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 11020,
				Sayi = 5,
				KazanmaMesaji = "Şanslı saat! Herkes 5 adet 'Kutsallık Sembolü' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 311199,
				Sayi = 4,
				KazanmaMesaji = "Şanslı saat! Herkes 4 adet '1. Seviye Saldırı İncisi' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 312199,
				Sayi = 4,
				KazanmaMesaji = "Şanslı saat! Herkes 4 adet '1. Seviye Savunma İncisi' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 313199,
				Sayi = 4,
				KazanmaMesaji = "Şanslı saat! Herkes 4 adet '1. Seviye Nitelik İncisi' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 311299,
				Sayi = 3,
				KazanmaMesaji = "Şanslı saat! Herkes 3 adet '2. Seviye Saldırı İncisi' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 312299,
				Sayi = 3,
				KazanmaMesaji = "Şanslı saat! Herkes 3 adet '2. Seviye Savunma İncisi' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 313299,
				Sayi = 3,
				KazanmaMesaji = "Şanslı saat! Herkes 3 adet '2. Seviye Nitelik İncisi' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 311399,
				Sayi = 2,
				KazanmaMesaji = "Şanslı saat! Herkes 2 adet '3. Seviye Saldırı İncisi' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 312399,
				Sayi = 2,
				KazanmaMesaji = "Şanslı saat! Herkes 2 adet '3. Seviye Savunma İncisi' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 313399,
				Sayi = 2,
				KazanmaMesaji = "Şanslı saat! Herkes 2 adet '3. Seviye Nitelik İncisi' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 311499,
				Sayi = 1,
				KazanmaMesaji = "Şanslı saat! Herkes 1 adet '4. Seviye Saldırı İncisi' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 312499,
				Sayi = 1,
				KazanmaMesaji = "Şanslı saat! Herkes 1 adet '4. Seviye Savunma İncisi' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 313499,
				Sayi = 1,
				KazanmaMesaji = "Şanslı saat! Herkes 1 adet '4. Seviye Nitelik İncisi' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 40001,
				Sayi = 4,
				KazanmaMesaji = "Şanslı saat! Herkes 4 adet '1. Seviye Eğitim İksiri' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 40002,
				Sayi = 3,
				KazanmaMesaji = "Şanslı saat! Herkes 3 adet '2. Seviye Eğitim İksiri' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 40003,
				Sayi = 2,
				KazanmaMesaji = "Şanslı saat! Herkes 2 adet '3. Seviye Eğitim İksiri' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 40004,
				Sayi = 1,
				KazanmaMesaji = "Şanslı saat! Herkes 1 adet '4. Seviye Eğitim İksiri' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 315001,
				Sayi = 60,
				KazanmaMesaji = "Şanslı saat! Herkes 60 adet '1. Seviye Kutsal Taşı Yenile' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 315002,
				Sayi = 50,
				KazanmaMesaji = "Şanslı saat! Herkes 50 adet '2. Seviye Kutsal Taşı Yenile' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 315003,
				Sayi = 40,
				KazanmaMesaji = "Şanslı saat! Herkes 40 adet '3. Seviye Kutsal Taşı Yenile' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 315004,
				Sayi = 30,
				KazanmaMesaji = "Şanslı saat! Herkes 30 adet '4. Seviye Kutsal Taşı Yenile' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 315005,
				Sayi = 20,
				KazanmaMesaji = "Şanslı saat! Herkes 20 adet '5. Seviye Kutsal Taşı Yenile' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 315006,
				Sayi = 10,
				KazanmaMesaji = "Şanslı saat! Herkes 10 adet '6. Seviye Kutsal Taşı Yenile' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 8002,
				Sayi = 1,
				KazanmaMesaji = "Şanslı saat! Herkes 1 adet 'Şans Künyesi' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 8003,
				Sayi = 1,
				KazanmaMesaji = "Şanslı saat! Herkes 1 adet 'Çeviklik Künyesi' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 8004,
				Sayi = 1,
				KazanmaMesaji = "Şanslı saat! Herkes 1 adet 'Canavar Saldırısı' kazandı!"
			},
            new Program.RastgeleOdul
            {
                ItemID = 8006,
                Sayi = 1,
                KazanmaMesaji = "Şanslı saat! Herkes 1 adet 'Sağlam Zincir' kazandı!"
            },
            new Program.RastgeleOdul
			{
				ItemID = 9002,
				Sayi = 1,
				KazanmaMesaji = "Şanslı saat! Herkes 1 adet 'Kanatlı Yüzük' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 9003,
				Sayi = 1,
				KazanmaMesaji = "Şanslı saat! Herkes 1 adet 'Şans Yüzüğü' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 9005,
				Sayi = 1,
				KazanmaMesaji = "Şanslı saat! Herkes 1 adet 'Okyanus Yüzüğü' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 9006,
				Sayi = 1,
				KazanmaMesaji = "Şanslı saat! Herkes 1 adet 'Alevli Yüzük' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 20101,
				Sayi = 50,
				KazanmaMesaji = "Şanslı saat! Herkes 1 adet 'Kırmızı Sihirli Karınca Kart Kutusu' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 20102,
				Sayi = 50,
				KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Mavi Sihirli Karınca Kart Kutusu' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 20103,
				Sayi = 50,
				KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Sihirli Karınca Kraliçesi Kart Kutusu' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 20104,
				Sayi = 50,
				KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Pembe Bogolu Kart Kutusu' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 20105,
				Sayi = 50,
				KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Bogo Eliti Kart Kutusu' kazandı!"
            },
			new Program.RastgeleOdul
			{
				ItemID = 20106,
				Sayi = 50,
				KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Bogo Lideri Kart Kutusu' kazandı!"
            },
			new Program.RastgeleOdul
			{
				ItemID = 20107,
				Sayi = 50,
				KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Bogo Kralı Kart Kutusu' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 20108,
				Sayi = 50,
				KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Kabile Savaşçısı Kart Kutusu' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 20109,
				Sayi = 50,
				KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Kabile Askeri Kart Kutusu' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 20110,
				Sayi = 50,
				KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Kabile Kardeşinin Abi Kart Kutusu' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 20111,
				Sayi = 50,
				KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Kabile Kardeşinin Kardeş Kart Kutusu' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 20112,
				Sayi = 50,
				KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Kabile Reisi Kart Kutusu' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 20113,
				Sayi = 50,
				KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Sihirli Kartal Kart Kutusu' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 20114,
				Sayi = 50,
				KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Sihirli Kurt Kart Kutusu' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 20115,
				Sayi = 50,
				KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Kırmızı Cüppeli Şeytan Kart Kutusu' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 20116,
				Sayi = 50,
				KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Minotar Kart Kutusu' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 20117,
				Sayi = 50,
				KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'İlahi Fırtına Kart Kutusu' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 20118,
				Sayi = 50,
				KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Bom Kart Kutusu' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 20119,
				Sayi = 50,
				KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Öfke Ateşi Kart Kutusu' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 20120,
				Sayi = 50,
				KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Yıldırım Kart Kutusu' kazandı!"
			},
			new Program.RastgeleOdul
			{
				ItemID = 20121,
				Sayi = 50,
				KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Barabran Kölesi Kart Kutusu' kazandı!"
			},
            new Program.RastgeleOdul
            {
                ItemID = 20122,
                Sayi = 50,
                KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Barabran teknikeri kart kutusu' kazandı!"
            },
            new Program.RastgeleOdul
            {
                ItemID = 20123,
                Sayi = 50,
                KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Barabran savaşçısı kart kutusu' kazandı!"
            },
            new Program.RastgeleOdul
            {
                ItemID = 20124,
                Sayi = 50,
                KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Barabran askeri kart kutusu' kazandı!"
            },
            new Program.RastgeleOdul
            {
                ItemID = 20125,
                Sayi = 50,
                KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Barabran komandanı kart kutusu' kazandı!"
            },
            new Program.RastgeleOdul
            {
                ItemID = 20126,
                Sayi = 50,
                KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Kabuklu civciv kart kutusu' kazandı!"
            },
            new Program.RastgeleOdul
            {
                ItemID = 20127,
                Sayi = 50,
                KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Şişman tavuk kart kutusu' kazandı!"
            },
            new Program.RastgeleOdul
            {
                ItemID = 20128,
                Sayi = 50,
                KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'İnci boyunlu horoz kart kutusu' kazandı!"
            },
            new Program.RastgeleOdul
            {
                ItemID = 20129,
                Sayi = 50,
                KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Pis Benben kart kutusu' kazandı!"
            },
            new Program.RastgeleOdul
            {
                ItemID = 20130,
                Sayi = 50,
                KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Bogolu sporcu kart kutusu' kazandı!"
            },
            new Program.RastgeleOdul
            {
                ItemID = 20131,
                Sayi = 50,
                KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Bogo antrenörü kart kutusu' kazandı!"
            },
            new Program.RastgeleOdul
            {
                ItemID = 20132,
                Sayi = 50,
                KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Bogolu izleyici kart kutusu' kazandı!"
            },
            new Program.RastgeleOdul
            {
                ItemID = 20133,
                Sayi = 50,
                KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Bogolu boksör kralı kart kutusu' kazandı!"
            },
            new Program.RastgeleOdul
            {
                ItemID = 20134,
                Sayi = 50,
                KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Bogolu hakem kart kutusu' kazandı!"
            },
            new Program.RastgeleOdul
            {
                ItemID = 20135,
                Sayi = 50,
                KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Fıçı kart kutusu' kazandı!"
            },
            new Program.RastgeleOdul
            {
                ItemID = 20136,
                Sayi = 50,
                KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Newton elması kart kutusu' kazandı!"
            },
            new Program.RastgeleOdul
            {
                ItemID = 20137,
                Sayi = 50,
                KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'FA kutusu kart kutusu' kazandı!"
            },
            new Program.RastgeleOdul
            {
                ItemID = 20138,
                Sayi = 50,
                KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Keskin kılıç kart kutusu' kazandı!"
            },
            new Program.RastgeleOdul
            {
                ItemID = 20139,
                Sayi = 50,
                KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'SB TV kart kutusu' kazandı!"
            },
            new Program.RastgeleOdul
            {
                ItemID = 20140,
                Sayi = 50,
                KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Civciv birliği kart kutusu' kazandı!"
            },
            new Program.RastgeleOdul
            {
                ItemID = 20141,
                Sayi = 50,
                KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Kurt komando lideri kart kutusu' kazandı!"
            },
            new Program.RastgeleOdul
            {
                ItemID = 20142,
                Sayi = 50,
                KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Karasakal kart kutusu' kazandı!"
            },
            new Program.RastgeleOdul
            {
                ItemID = 20143,
                Sayi = 50,
                KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Dalov Albay kart kutusu' kazandı!"
            },
            new Program.RastgeleOdul
            {
                ItemID = 20144,
                Sayi = 50,
                KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Rigri kart kutusu' kazandı!"
            },
            new Program.RastgeleOdul
            {
                ItemID = 20145,
                Sayi = 50,
                KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Karanlık kraliyet kart kutusu' kazandı!"
            },
            new Program.RastgeleOdul
            {
                ItemID = 20146,
                Sayi = 50,
                KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Solan reis kart kutusu' kazandı!"
            },
            new Program.RastgeleOdul
            {
                ItemID = 20147,
                Sayi = 50,
                KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'Çift bomba kralı kart kutusu' kazandı!"
            },
            new Program.RastgeleOdul
            {
                ItemID = 20148,
                Sayi = 50,
                KazanmaMesaji = "Şanslı saat! Herkes 50 adet 'En iyi partner kart kutusu' kazandı!"
            },
        };

		// Token: 0x04000007 RID: 7
		private static readonly List<string> _sunucuMesajlari = new List<string>
		{
			"[Duyuru] Sunucumuzda hile, bug veya 3. parti yazılım kullanımı kalıcı olarak yasaklanma sebebidir. Lütfen adil bir oyun ortamı için kurallara uyun.",
			"[İpucu] Günlük görevleri tamamlayarak değerli ödüller kazanabileceğinizi unutmayın!",
			"[Hatırlatma] Takım arkadaşlarınızla iletişim kurmak zaferin anahtarıdır!",
			"[Duyuru] Herhangi bir sorunla karşılaşırsanız oyun yöneticilerine bildirmekten çekinmeyin. Keyifli oyunlar!",
			"[Bilgi] Unutma, en güçlü silah bilgidir! Sitemizdeki rehberlere göz atarak oyununu geliştirebilirsin."
		};

		// Token: 0x04000008 RID: 8
		private static readonly Random _random = new Random();

		// Token: 0x04000009 RID: 9
		public static System.Timers.Timer kontrol_araligi = new System.Timers.Timer(1800000.0);

		// Token: 0x0400000A RID: 10
		public static System.Timers.Timer kontrol_araligi2 = new System.Timers.Timer(60000.0);

		// Token: 0x0400000B RID: 11
		public static System.Timers.Timer kontrol_araligi3 = new System.Timers.Timer(1800000.0);

		// Token: 0x0400000C RID: 12
		public static System.Timers.Timer dinamik_olay_timeri = new System.Timers.Timer(300000.0);

		// Token: 0x0200000F RID: 15
		public class RastgeleOdul
		{
			// Token: 0x17000024 RID: 36
			// (get) Token: 0x060000B7 RID: 183 RVA: 0x0000EBE0 File Offset: 0x0000CDE0
			// (set) Token: 0x060000B8 RID: 184 RVA: 0x0000EBE8 File Offset: 0x0000CDE8
			public int ItemID { get; set; }

			// Token: 0x17000025 RID: 37
			// (get) Token: 0x060000B9 RID: 185 RVA: 0x0000EBF1 File Offset: 0x0000CDF1
			// (set) Token: 0x060000BA RID: 186 RVA: 0x0000EBF9 File Offset: 0x0000CDF9
			public int Sayi { get; set; }

			// Token: 0x17000026 RID: 38
			// (get) Token: 0x060000BB RID: 187 RVA: 0x0000EC02 File Offset: 0x0000CE02
			// (set) Token: 0x060000BC RID: 188 RVA: 0x0000EC0A File Offset: 0x0000CE0A
			public string KazanmaMesaji { get; set; }
		}
	}
}
