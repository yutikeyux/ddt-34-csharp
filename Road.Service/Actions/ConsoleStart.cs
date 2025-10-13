using Bussiness;
using Bussiness.Managers;
using Game.Base;
using Game.Logic;
using Game.Server;
using Game.Server.Managers;
using Game.Server.Packets;
using Game.Server.Rooms;
using System.Windows.Forms;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace Game.Service.actions
{
    public class ConsoleStart : IAction
    {
        private delegate int ConsoleCtrlDelegate(ConsoleEvent ctrlType);

        private enum ConsoleEvent
        {
            Ctrl_C,
            Ctrl_Break,
            Close,
            Logoff,
            Shutdown
        }

        private GameServerConfig config;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static System.Threading.Timer _timer;

        private static int _count;

        private static ConsoleCtrlDelegate handler;

        public string Name => "--start";

        public string Syntax => "--start [-config=./config/serverconfig.xml]";

        public string Description => "Starts the DOL server in console mode";

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        private static extern bool ReadConsoleW(IntPtr hConsoleInput, [Out] byte[] lpBuffer, uint nNumberOfCharsToRead, out uint lpNumberOfCharsRead, IntPtr lpReserved);
		public static int _count2 { get; set; }

		public static System.Threading.Timer _timer2 { get; set; }
		private static void onlinesayac(object state)
		{
			_count2--;
			Console.WriteLine($"Komut verildi. {_count2} dakika sonra mesaj gönderilecek !");
			GameClient[] allClients = GameServer.Instance.GetAllClients();
			int num = ((allClients != null) ? allClients.Length : 0);
			GameClient[] array = allClients;
			foreach (GameClient gameClient in array)
			{
			}
			if (_count2 == 0)
			{
				GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
				GamePlayer[] allPlayers2 = WorldMgr.GetAllPlayers();
				foreach (GamePlayer gamePlayer in allPlayers2)
				{
					gamePlayer.SendMessage($"Sistem : Şuanda oyunda {num} kişi online !. [BomBomRia 2026]");
				}
				Console.WriteLine("Online sayısı gönderildi");
				_count2 = 31;
			}
		}

		public static IntPtr GetWin32InputHandle()
        {
            return GetStdHandle(-10);
        }
        public static void NewForm()
        {
            Application.Run(new ServerManagementForm());
        }
        public void OnAction(Hashtable parameters)
        {
            bool flag = true;
            Console.Title = "Road Service";
            Console.ForegroundColor = ConsoleColor.Green;
            GameServer.CreateInstance(config = new GameServerConfig());
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Road Server Başlatılıyor...");
            GameServer.Instance.Start();
            Console.ForegroundColor = ConsoleColor.Cyan;
           
            GameServer.KeepRunning = true;
            FusionCombined.ListCombinedFusion();

            if (!flag)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Server Başarısız !!");
            }
            else
            {
                Console.WriteLine("Server Online!");
                Console.WriteLine("Yönetim Konsolunu Açmak İçin (admin) Yazıp Enter Tuşuna Basınız .");
            }
            ConsoleClient client = new ConsoleClient();
            new Thread(NewForm).Start();
            while (GameServer.KeepRunning)
			{
				if (!flag)
				{
					continue;
				}
				try
				{
					handler = ConsoleCtrHandler;
					SetConsoleCtrlHandler(handler, add: true);
					Console.Write("=> ");
					string text = Console.ReadLine();
					string text2 = text.Split(' ')[0];
					if (text2 == null)
					{
						goto IL_0f54;
					}
					if (!(text2 == "admin"))
					{
						if (text2 == "reset")
						{
							Console.Clear();
							continue;
						}
						goto IL_0f54;
					}
					Console.Clear();
					Console.ForegroundColor = ConsoleColor.Cyan;
					Console.WriteLine("BomBomRia Yönetim Konsolu.");
					Console.WriteLine("Lütfen numara seçin ;");
					Console.WriteLine("1.  Mesaj Gönder.");
					Console.WriteLine("2.  Nick e Ban At.");
					Console.WriteLine("3.  Kullanıcı Adına Ban At.");
					Console.WriteLine("4.  Oyunu 1 Dakika Sonra Kapat (!).");
					Console.WriteLine("5.  Oyunu Hemen Kapat (!).");
					Console.WriteLine("6.  Oyun Online Durumu Ve Ram Kullanımı.");
					Console.WriteLine("7.  Nick e Kick At.");
					Console.WriteLine("8.  Oyuncu Ban Kaldırma.");
					Console.WriteLine("9.  Oyun İçine Online Oyuncu Sayısını Otomatik Gönder.");
					Console.WriteLine("10. Oyun Veritabanı Değişiklerini Kaydet.");
					Console.WriteLine("11. Online (Kupon) Etkinliği.");
					Console.WriteLine("12. Online (Onur Özü) Etkinliği.");
					Console.WriteLine("13. Online (Kart Ruhu) Etkinliği.");
					Console.WriteLine("14. Online (Exp GP) Etkinliği.");
					Console.WriteLine("15. Online (İtem) Etkinliği.");
					Console.WriteLine("16. Özel Mesaj Gönderme.");
					Console.WriteLine("17. Özel (Road) Sistemini Aç.");
					Console.Write("Seçiminizi Girin : ");
					string text3 = Console.ReadLine();
					switch (text3)
					{
						case "1":
							{
								Console.Clear();
								Console.Write("Mesajınızı Giriniz : ");
								string arg2 = Console.ReadLine();
								GamePlayer[] allPlayers12 = WorldMgr.GetAllPlayers();
								for (int num20 = 0; num20 < allPlayers12.Length; num20++)
								{
									allPlayers12[num20].SendMessage($"Yönetim : {arg2}");
								}
								Console.WriteLine("Mesaj gönderildi.");
								goto end_IL_00de;
							}
						case "2":
							{
								Console.Clear();
								Console.WriteLine("Banlanacak oyuncunun Nick'i : ");
								string text9 = Console.ReadLine();
								Console.WriteLine("Ban sebebi: ");
								string text10 = Console.ReadLine();
								Console.WriteLine("Ban açılış yılı gir: ");
								int num16 = int.Parse(Console.ReadLine());
								Console.WriteLine("Ban açılış ayı gir: ");
								int num17 = int.Parse(Console.ReadLine());
								Console.WriteLine("Ban açılış günü gir: ");
								int num18 = int.Parse(Console.ReadLine());
								DateTime date3 = new DateTime(num16, num17, num18);
								using (ManageBussiness manageBussiness5 = new ManageBussiness())
								{
									manageBussiness5.ForbidPlayerByNickName(text9, date3, isExist: false, text10);
								}
								GamePlayer[] allPlayers11 = WorldMgr.GetAllPlayers();
								foreach (GamePlayer gamePlayer8 in allPlayers11)
								{
									string msg5 = $"Oyuncumuz <{text9}> oyun kurallarına aykırı gelirken yakaladık ve BANLADIK. Sizde böyle olmak istemiyorsanız kurallara uyunuz. Ban sebebi : ({text10}). Ban açılış tarihi: ({num16}.{num17}.{num18})";
									gamePlayer8.SendMessage(msg5);
								}
								Console.WriteLine("Oyuncu " + text9 + " banlandı ve kicklendi.");
								break;
							}
						case "3":
							{
								Console.Clear();
								Console.WriteLine("Banlanacak kullanıcı adı: ");
								string text7 = Console.ReadLine();
								Console.WriteLine("Ban sebebi gir: ");
								string text8 = Console.ReadLine();
								Console.WriteLine("Ban açılış yılı gir: ");
								int num12 = int.Parse(Console.ReadLine());
								Console.WriteLine("Ban açılış ayı gir: ");
								int num13 = int.Parse(Console.ReadLine());
								Console.WriteLine("Ban açılış günü: ");
								int num14 = int.Parse(Console.ReadLine());
								DateTime date2 = new DateTime(num12, num13, num14);
								using (ManageBussiness manageBussiness4 = new ManageBussiness())
								{
									manageBussiness4.ForbidPlayerByUserName(text7, date2, isExist: false, text8);
								}
								GamePlayer[] allPlayers10 = WorldMgr.GetAllPlayers();
								foreach (GamePlayer gamePlayer7 in allPlayers10)
								{
									string msg4 = $"<{text7}> Kullanıcı adlı oyuncumuz oyun kurallarını çiğnediğinden oyundan uzaklaştırılmıştır. Ban sebebi : ({text8}). Ban açılış tarihi: ({num12}.{num13}.{num14})";
									gamePlayer7.SendMessage(msg4);
								}
								Console.WriteLine("Oyuncu banlandı !.");
								break;
							}
						case "4":
							{
								GamePlayer[] allPlayers13 = WorldMgr.GetAllPlayers();
								foreach (GamePlayer gamePlayer9 in allPlayers13)
								{
									gamePlayer9.SendMessage("Admin Tarafından Oyundan Atıldınız.!");
									gamePlayer9.Disconnect();
								}
								GameServer.KeepRunning = false;
								Console.WriteLine("Oyun Kontrollu Bir Şekilde Kapanmıştır.!");
								goto end_IL_00de;
							}
						case "5":
							GameServer.KeepRunning = false;
							goto end_IL_00de;
						case "17":
							new Thread(NewForm).Start();
							goto default;
						default:
							switch (text3)
							{
								case "16":
									{
										Console.Clear();
										Console.Write("Mesajınızı Giriniz : ");
										string text6 = Console.ReadLine();
										GamePlayer[] allPlayers4 = WorldMgr.GetAllPlayers();
										for (int k = 0; k < allPlayers4.Length; k++)
										{
											allPlayers4[k].Out.SendMessage(eMessageType.ALERT, "[ YÖNETİM ]: " + text6);
										}
										Console.WriteLine("Mesaj gönderildi.");
										goto end_IL_00de;
									}
								case "6":
									{
										GameClient[] allClients = GameServer.Instance.GetAllClients();
										int num = ((allClients != null) ? allClients.Length : 0);
										GamePlayer[] allPlayers3 = WorldMgr.GetAllPlayers();
										if (allPlayers3 != null)
										{
											int num2 = allPlayers3.Length;
										}
										List<BaseRoom> allUsingRoom = RoomMgr.GetAllUsingRoom();
										int num3 = 0;
										int num4 = 0;
										foreach (BaseRoom item in allUsingRoom)
										{
											if (!item.IsEmpty)
											{
												num3++;
												if (item.IsPlaying)
												{
													num4++;
												}
											}
										}
										double num5 = GC.GetTotalMemory(forceFullCollection: false);
										Console.WriteLine($"Online Oyuncu : {num}");
										Console.WriteLine($"Savaştaki oyuncu : {num3} odada , {num4} kişi savaşta");
										Console.WriteLine($"Oyun ram kullanimi:{num5 / 1024.0 / 1024.0} MB");
										goto end_IL_00de;
									}
								case "7":
									{
										Console.Clear();
										Console.WriteLine("Dc yiyecek oyuncunun nicki : ");
										string text5 = Console.ReadLine();
										Console.WriteLine("Dc Sebebini gir : ");
										string arg = Console.ReadLine();
										using (ManageBussiness manageBussiness2 = new ManageBussiness())
										{
											manageBussiness2.KitoffUserByNickName(text5, " ");
										}
										GamePlayer[] allPlayers2 = WorldMgr.GetAllPlayers();
										for (int j = 0; j < allPlayers2.Length; j++)
										{
											allPlayers2[j].SendMessage($"Oyuncu [{text5}] Oyunculara rahatsızlığından dolayı kick yemiştir. Kick sebebi :  ({arg})");
										}
										goto end_IL_00de;
									}
								case "8":
									{
										Console.Clear();
										Console.WriteLine("Banı Açılacak Oyuncunun Nick'i : ");
										string text4 = Console.ReadLine();
										DateTime date = new DateTime(2050, 7, 2);
										using (ManageBussiness manageBussiness = new ManageBussiness())
										{
											manageBussiness.ForbidPlayerByNickName(text4, date, isExist: true);
										}
										GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
										foreach (GamePlayer gamePlayer in allPlayers)
										{
											string msg = $"Oyuncumuz <{text4}> banı sonra ermiştir. ";
											gamePlayer.SendMessage(msg);
										}
										Console.WriteLine("Oyuncu " + text4 + " banı açıldı");
										break;
									}
							}
							switch (text3)
							{
								case "9":
									_count2 = 31;
									_timer2 = new System.Threading.Timer(onlinesayac, null, 0, 60000);
									goto end_IL_00de;
								case "11":
									{
										Console.Write("Gönderilecek kupon miktarı : ");
										int num6 = int.Parse(Console.ReadLine());
										GamePlayer[] allPlayers6 = WorldMgr.GetAllPlayers();
										foreach (GamePlayer gamePlayer3 in allPlayers6)
										{
											gamePlayer3.AddMoney(num6);
											gamePlayer3.SendMessage("Tebrikler ! Bütün online oyunculara " + num6 + " kupon gönderildi. Online etkinliği sona ermiştir...");
										}
										Console.WriteLine("Kupon gönderme başarılı (Gönderilen {0})", num6);
										goto end_IL_00de;
									}
								case "12":
									{
										Console.Write("Gönderilecek onur miktarı : ");
										int num8 = int.Parse(Console.ReadLine());
										GamePlayer[] allPlayers8 = WorldMgr.GetAllPlayers();
										foreach (GamePlayer gamePlayer5 in allPlayers8)
										{
											gamePlayer5.AddHonor(num8);
											gamePlayer5.SendMessage("Tebrikler ! Bütün online oyunculara " + num8 + " onur gönderildi. Online etkinliği sona ermiştir...");
										}
										Console.WriteLine("Onur gönderme başarılı (Gönderilen {0})", num8);
										goto end_IL_00de;
									}
								case "13":
									{
										Console.Write("Gönderilecek Kart Ruhu miktarı : ");
										int num10 = int.Parse(Console.ReadLine());
										GamePlayer[] allPlayers9 = WorldMgr.GetAllPlayers();
										foreach (GamePlayer gamePlayer6 in allPlayers9)
										{
											//gamePlayer6.AddCardSoul(num10);
											gamePlayer6.SendMessage("Tebrikler ! Bütün online oyunculara " + num10 + " Kart Ruhu gönderildi. Online etkinliği sona ermiştir...");
										}
										Console.WriteLine("Kart Ruhu gönderme başarılı (Gönderilen {0})", num10);
										goto end_IL_00de;
									}
								case "14":
									{
										Console.Write("Gönderilecek EXP miktarı : ");
										int num7 = int.Parse(Console.ReadLine());
										GamePlayer[] allPlayers7 = WorldMgr.GetAllPlayers();
										foreach (GamePlayer gamePlayer4 in allPlayers7)
										{
											gamePlayer4.AddGP(num7);
											gamePlayer4.SendMessage("Tebrikler ! Bütün online oyunculara " + num7 + " EXP (GP) gönderildi. Online etkinliği sona ermiştir...");
										}
										Console.WriteLine("EXP (GP) gönderme başarılı (Gönderilen {0})", num7);
										goto end_IL_00de;
									}
								case "15":
									{
										Console.Write("Başlık:");
										string title = Console.ReadLine();
										Console.Write("İçerik:");
										string content = Console.ReadLine();
										Console.Write("İtem İD:");
										int templateID = int.Parse(Console.ReadLine());
										Console.Write("Adet:");
										int count = int.Parse(Console.ReadLine());
										Console.Write("Gün:");
										int validDate = int.Parse(Console.ReadLine());
										Console.Write("Altın:");
										int gold = int.Parse(Console.ReadLine());
										Console.Write("Kupon:");
										int money = int.Parse(Console.ReadLine());
										Console.Write("Level:");
										int strengthenLevel = int.Parse(Console.ReadLine());
										Console.Write("Atak:");
										int attackCompose = int.Parse(Console.ReadLine());
										Console.Write("Defans:");
										int defendCompose = int.Parse(Console.ReadLine());
										Console.Write("Çeviklik:");
										int agilityCompose = int.Parse(Console.ReadLine());
										Console.Write("Şans:");
										int luckCompose = int.Parse(Console.ReadLine());
										Console.Write("Baglı (True-False):");
										bool isBinds = bool.Parse(Console.ReadLine());
										GamePlayer[] allPlayers5 = WorldMgr.GetAllPlayers();
										foreach (GamePlayer gamePlayer2 in allPlayers5)
										{
											PlayerInfo playerInfo = new PlayerInfo();
											playerInfo = gamePlayer2.PlayerCharacter;
											new PlayerBussiness().SendMailAndItem(title, content, playerInfo.ID, templateID, count, validDate, gold, money, strengthenLevel, attackCompose, defendCompose, agilityCompose, luckCompose, isBinds);
											string msg2 = $"[ Online Oyuncu Etkinliği Sistemi ] Hediye Ödüller Gönderilmiştir.";
											gamePlayer2.SendMessage(msg2);
                                            using ManageBussiness manageBussiness3 = new ManageBussiness();
											string msg3 = $"Sistem Yöneticisi : Hediye Ödüller Gönderilmiştir.";
											manageBussiness3.SystemNotice(msg3);
										}
										break;
									}
							}
							if (text3 == "10")
							{
								if (BallMgr.ReLoad())
								{
									Console.WriteLine("Ball info Güncelleniyor !");
								}
								Console.WriteLine("Ball info Güncellendi !");
								if (MapMgr.ReLoadMap())
								{
									Console.WriteLine("Map info Güncelleniyor !");
								}
								Console.WriteLine("Map info Güncellendi !");
								if (MapMgr.ReLoadMapServer())
								{
									Console.WriteLine("mapserver info Güncelleniyor !");
								}
								Console.WriteLine("mapserver Güncellendi !");
								if (PropItemMgr.Reload())
								{
									Console.WriteLine("prop info Güncelleniyor !");
								}
								Console.WriteLine("prop info Güncellendi !");
								if (ItemMgr.ReLoad())
								{
									Console.WriteLine("item info Güncelleniyor !");
								}
								Console.WriteLine("item info Güncellendi !");
								if (ShopMgr.ReLoad())
								{
									Console.WriteLine("shop info Güncelleniyor !");
								}
								Console.WriteLine("shop info Güncellendi !");
								if (QuestMgr.ReLoad())
								{
									Console.WriteLine("quest info Güncelleniyor !");
								}
								Console.WriteLine("quest info Güncellendi !");
								if (FusionMgr.ReLoad())
								{
									Console.WriteLine("fusion info Güncelleniyor !");
								}
								Console.WriteLine("fusion info Güncellendi !");
								if (ConsortiaMgr.ReLoad())
								{
									Console.WriteLine("consortiaMgr info Güncelleniyor !");
								}
								Console.WriteLine("consortiaMgr info Güncellendi !");
								if (RateMgr.ReLoad())
								{
									Console.WriteLine("Rate Rate Güncelleniyor !");
								}
								Console.WriteLine("Rate Rate Güncellendi !");
								if (NPCInfoMgr.ReLoad())
								{
									Console.WriteLine("NPCInfo Güncelleniyor !");
								}
								Console.WriteLine("NPCInfo Güncellendi !");
								if (FightRateMgr.ReLoad())
								{
									Console.WriteLine("FightRateMgr Güncelleniyor !");
								}
								Console.WriteLine("FightRateMgr Güncellendi !");
								if (AwardMgr.ReLoad())
								{
									Console.WriteLine("dailyaward Güncelleniyor !");
								}
								Console.WriteLine("dailyaward Güncellendi !");
								if (LanguageMgr.Reload(""))
								{
									Console.WriteLine("language Güncelleniyor !");
								}
								Console.WriteLine("language Güncellendi !");
							}
							goto end_IL_00de;
					}
					goto IL_0f54;
				IL_0f54:
					if (text.Length <= 0)
					{
						continue;
					}
					if (text[0] == '/')
					{
						text = text.Remove(0, 1);
						text = text.Insert(0, "&");
					}
					try
					{
						if (!CommandMgr.HandleCommandNoPlvl(client, text))
						{
							Console.WriteLine("Bilinmeyen komut: " + text);
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.ToString());
					}
				end_IL_00de:;
				}
				catch (Exception value)
				{
					Console.WriteLine(value);
				}
			}
			if (GameServer.Instance != null)
			{
				GameServer.Instance.Stop();
			}
			LogManager.Shutdown();
		}

		private static void ShutDownCallBack(object state)
        {
            _count--;
            Console.WriteLine($"Server will shutdown after {_count} mins!");
            GameClient[] allClients = GameServer.Instance.GetAllClients();
            GameClient[] array = allClients;
            foreach (GameClient c in array)
            {
                if (c.Out != null)
                {
                    c.Out.SendMessage(eMessageType.GM_NOTICE, string.Format("{0}{1}{2}", LanguageMgr.GetTranslation("Game.Service.actions.ShutDown1"), _count, LanguageMgr.GetTranslation("Game.Service.actions.ShutDown2")));
                }
            }
            if (_count == 0)
            {
                _timer.Dispose();
                _timer = null;
                GameServer.Instance.Stop();
                Console.WriteLine("Server has stopped!");
                GameServer.KeepRunning = false;
                Environment.Exit(0);
                return;
            }
        }

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int SetConsoleCtrlHandler(ConsoleCtrlDelegate HandlerRoutine, bool add);

        private static int ConsoleCtrHandler(ConsoleEvent e)
        {
            SetConsoleCtrlHandler(handler, add: false);
            if (GameServer.Instance != null)
            {
                GameServer.Instance.Stop();
            }
            return 0;
        }
    }
}
