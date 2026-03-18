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
	// Token: 0x02000007 RID: 7
	public class ConsoleStart : IAction
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000020 RID: 32 RVA: 0x000033C4 File Offset: 0x000015C4
		public string Name
		{
			get
			{
				return "--start";
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000021 RID: 33 RVA: 0x000033DC File Offset: 0x000015DC
		public string Syntax
		{
			get
			{
				return "--start [-config=./config/serverconfig.xml]";
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000022 RID: 34 RVA: 0x000033F4 File Offset: 0x000015F4
		public string Description
		{
			get
			{
				return "Starts the DOL server in console mode";
			}
		}

		// Token: 0x06000023 RID: 35
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr GetStdHandle(int nStdHandle);

		// Token: 0x06000024 RID: 36
		[DllImport("kernel32.dll")]
		private static extern bool ReadConsoleW(IntPtr hConsoleInput, [Out] byte[] lpBuffer, uint nNumberOfCharsToRead, out uint lpNumberOfCharsRead, IntPtr lpReserved);

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000025 RID: 37 RVA: 0x0000340B File Offset: 0x0000160B
		// (set) Token: 0x06000026 RID: 38 RVA: 0x00003412 File Offset: 0x00001612
		public static int _count2 { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000027 RID: 39 RVA: 0x0000341A File Offset: 0x0000161A
		// (set) Token: 0x06000028 RID: 40 RVA: 0x00003421 File Offset: 0x00001621
		public static System.Threading.Timer _timer2 { get; set; }

		// Token: 0x06000029 RID: 41 RVA: 0x0000342C File Offset: 0x0000162C
		private static void onlinesayac(object state)
		{
			ConsoleStart._count2--;
			Console.WriteLine(string.Format("Komut verildi. {0} dakika sonra mesaj gönderilecek !", ConsoleStart._count2));
			GameClient[] allClients = GameServer.Instance.GetAllClients();
			int num = (allClients != null) ? allClients.Length : 0;
			foreach (GameClient gameClient in allClients)
			{
			}
			bool flag = ConsoleStart._count2 == 0;
			if (flag)
			{
				WorldMgr.GetAllPlayers();
				GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
				for (int j = 0; j < allPlayers.Length; j++)
				{
					allPlayers[j].SendMessage(string.Format("Sistem : Şuanda oyunda {0} kişi online !. |TrBombom 2027|", num));
				}
				Console.WriteLine("Online sayısı gönderildi");
				ConsoleStart._count2 = 1;
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000034F4 File Offset: 0x000016F4
		public static IntPtr GetWin32InputHandle()
		{
			return ConsoleStart.GetStdHandle(-10);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x0000350D File Offset: 0x0000170D
		public static void NewForm()
		{
			Application.Run(new ServerManagementForm());
		}

		// Token: 0x0600002C RID: 44 RVA: 0x0000351C File Offset: 0x0000171C
		public void OnAction(Hashtable parameters)
		{
			bool flag = true;
			Console.Title = "TrBombom Main Service";
			Console.ForegroundColor = ConsoleColor.Green;
			GameServer.CreateInstance(this.config = new GameServerConfig());
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("Road Başlatılıyor");
			GameServer.Instance.Start();
			Console.ForegroundColor = ConsoleColor.Cyan;
			GameServer.KeepRunning = true;
			FusionCombined.ListCombinedFusion();
			bool flag2 = !flag;
			if (flag2)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Server Başarısız!!");
			}
			else
			{
				Console.WriteLine("Server Online!");
			}
			ConsoleClient client = new ConsoleClient();
			new Thread(new ThreadStart(ConsoleStart.NewForm)).Start();
			while (GameServer.KeepRunning)
			{
				bool flag3 = flag;
				if (flag3)
				{
					try
					{
						ConsoleStart.handler = new ConsoleStart.ConsoleCtrlDelegate(ConsoleStart.ConsoleCtrHandler);
						ConsoleStart.SetConsoleCtrlHandler(ConsoleStart.handler, true);
						Console.Write("=> ");
						string text = Console.ReadLine();
						string text2 = text.Split(new char[]
						{
							' '
						})[0];
						bool flag4 = text2 != null;
						if (flag4)
						{
							bool flag5 = !(text2 == "admin");
							if (flag5)
							{
								bool flag6 = text2 == "reset";
								if (flag6)
								{
									Console.Clear();
									continue;
								}
							}
							else
							{
								Console.Clear();
								Console.ForegroundColor = ConsoleColor.Cyan;
								Console.WriteLine("Yönetim Konsolu.");
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
								string a = text3;
								bool flag7 = a == "1";
								if (flag7)
								{
									Console.Clear();
									Console.Write("Mesajınızı Giriniz : ");
									string str = Console.ReadLine();
									GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
									for (int i = 0; i < allPlayers.Length; i++)
									{
										allPlayers[i].SendMessage("Yönetim : " + str);
									}
									Console.WriteLine("Mesaj gönderildi.");
									continue;
								}
								bool flag8 = !(a == "2");
								if (flag8)
								{
									bool flag9 = !(a == "3");
									if (flag9)
									{
										bool flag10 = a == "4";
										if (flag10)
										{
											foreach (GamePlayer gamePlayer in WorldMgr.GetAllPlayers())
											{
												gamePlayer.SendMessage("Admin Tarafından Oyundan Atıldınız.!");
												gamePlayer.Disconnect();
											}
											GameServer.KeepRunning = false;
											Console.WriteLine("Oyun Kontrollu Bir Şekilde Kapanmıştır.!");
											continue;
										}
										bool flag11 = a == "5";
										if (flag11)
										{
											GameServer.KeepRunning = false;
											continue;
										}
										bool flag12 = a == "17";
										if (flag12)
										{
											new Thread(new ThreadStart(ConsoleStart.NewForm)).Start();
										}
										string a2 = text3;
										bool flag13 = a2 == "16";
										if (flag13)
										{
											Console.Clear();
											Console.Write("Mesajınızı Giriniz : ");
											string str2 = Console.ReadLine();
											GamePlayer[] allPlayers3 = WorldMgr.GetAllPlayers();
											for (int k = 0; k < allPlayers3.Length; k++)
											{
												allPlayers3[k].Out.SendMessage(eMessageType.ALERT, "[YÖNETİM]: " + str2);
											}
											Console.WriteLine("Mesaj gönderildi.");
											continue;
										}
										bool flag14 = a2 == "6";
										if (flag14)
										{
											GameClient[] allClients = GameServer.Instance.GetAllClients();
											int num = (allClients != null) ? allClients.Length : 0;
											GamePlayer[] allPlayers4 = WorldMgr.GetAllPlayers();
											bool flag15 = allPlayers4 != null;
											if (flag15)
											{
												int num2 = allPlayers4.Length;
											}
											List<BaseRoom> allUsingRoom = RoomMgr.GetAllUsingRoom();
											int num3 = 0;
											int num4 = 0;
											foreach (BaseRoom baseRoom in allUsingRoom)
											{
												bool flag16 = !baseRoom.IsEmpty;
												if (flag16)
												{
													num3++;
													bool isPlaying = baseRoom.IsPlaying;
													if (isPlaying)
													{
														num4++;
													}
												}
											}
											double num5 = (double)GC.GetTotalMemory(false);
											Console.WriteLine(string.Format("Online Oyuncu : {0}", num));
											Console.WriteLine(string.Format("Savaştaki oyuncu : {0} odada , {1} kişi savaşta", num3, num4));
											Console.WriteLine(string.Format("Oyun ram kullanimi:{0} MB", num5 / 1024.0 / 1024.0));
											continue;
										}
										bool flag17 = a2 == "7";
										if (flag17)
										{
											Console.Clear();
											Console.WriteLine("Dc yiyecek oyuncunun nicki : ");
											string text4 = Console.ReadLine();
											Console.WriteLine("Dc Sebebini gir : ");
											string text5 = Console.ReadLine();
											using (ManageBussiness manageBussiness = new ManageBussiness())
											{
												manageBussiness.KitoffUserByNickName(text4, " ");
											}
											GamePlayer[] allPlayers5 = WorldMgr.GetAllPlayers();
											for (int l = 0; l < allPlayers5.Length; l++)
											{
												allPlayers5[l].SendMessage(string.Concat(new string[]
												{
													"Oyuncu [",
													text4,
													"] Oyunculara rahatsızlığından dolayı kick yemiştir. Kick sebebi :  (",
													text5,
													")"
												}));
											}
											continue;
										}
										bool flag18 = a2 == "8";
										if (flag18)
										{
											Console.Clear();
											Console.WriteLine("Banı Açılacak Oyuncunun Nick'i : ");
											string text6 = Console.ReadLine();
											DateTime date = new DateTime(2050, 7, 2);
											using (ManageBussiness manageBussiness2 = new ManageBussiness())
											{
												manageBussiness2.ForbidPlayerByNickName(text6, date, true);
											}
											foreach (GamePlayer gamePlayer2 in WorldMgr.GetAllPlayers())
											{
												string msg = "Oyuncumuz <" + text6 + "> banı sonra ermiştir. ";
												gamePlayer2.SendMessage(msg);
											}
											Console.WriteLine("Oyuncu " + text6 + " banı açıldı");
										}
										string a3 = text3;
										bool flag19 = a3 == "9";
										if (flag19)
										{
											ConsoleStart._count2 = 31;
											ConsoleStart._timer2 = new System.Threading.Timer(new TimerCallback(ConsoleStart.onlinesayac), null, 0, 60000);
											continue;
										}
										bool flag20 = a3 == "11";
										if (flag20)
										{
											Console.Write("Gönderilecek kupon miktarı : ");
											int num6 = int.Parse(Console.ReadLine());
											foreach (GamePlayer gamePlayer3 in WorldMgr.GetAllPlayers())
											{
												gamePlayer3.AddMoney(num6);
												gamePlayer3.SendMessage("Tebrikler ! Bütün online oyunculara " + num6.ToString() + " kupon gönderildi. Online etkinliği sona ermiştir...");
											}
											Console.WriteLine("Kupon gönderme başarılı (Gönderilen {0})", num6);
											continue;
										}
										bool flag21 = a3 == "12";
										if (flag21)
										{
											Console.Write("Gönderilecek onur miktarı : ");
											int num7 = int.Parse(Console.ReadLine());
											foreach (GamePlayer gamePlayer4 in WorldMgr.GetAllPlayers())
											{
												gamePlayer4.AddHonor(num7);
												gamePlayer4.SendMessage("Tebrikler ! Bütün online oyunculara " + num7.ToString() + " onur gönderildi. Online etkinliği sona ermiştir...");
											}
											Console.WriteLine("Onur gönderme başarılı (Gönderilen {0})", num7);
											continue;
										}
										bool flag22 = a3 == "13";
										if (flag22)
										{
											Console.Write("Gönderilecek Kart Ruhu miktarı : ");
											int num9 = int.Parse(Console.ReadLine());
											GamePlayer[] allPlayers9 = WorldMgr.GetAllPlayers();
											for (int num10 = 0; num10 < allPlayers9.Length; num10++)
											{
												allPlayers9[num10].SendMessage("Tebrikler ! Bütün online oyunculara " + num9.ToString() + " Kart Ruhu gönderildi. Online etkinliği sona ermiştir...");
											}
											Console.WriteLine("Kart Ruhu gönderme başarılı (Gönderilen {0})", num9);
											continue;
										}
										bool flag23 = !(a3 == "14");
										if (flag23)
										{
											bool flag24 = a3 == "15";
											if (flag24)
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
												foreach (GamePlayer gamePlayer5 in WorldMgr.GetAllPlayers())
												{
													PlayerInfo playerInfo = new PlayerInfo();
													playerInfo = gamePlayer5.PlayerCharacter;
													new PlayerBussiness().SendMailAndItem(title, content, playerInfo.ID, templateID, count, validDate, gold, money, strengthenLevel, attackCompose, defendCompose, agilityCompose, luckCompose, isBinds);
													string msg2 = "[ Online Oyuncu Etkinliği Sistemi ] Hediye Ödüller Gönderilmiştir.";
													gamePlayer5.SendMessage(msg2);
													using (ManageBussiness manageBussiness3 = new ManageBussiness())
													{
														string msg3 = "Sistem Yöneticisi : Hediye Ödüller Gönderilmiştir.";
														manageBussiness3.SystemNotice(msg3);
													}
												}
											}
											bool flag25 = text3 == "10";
											if (flag25)
											{
												bool flag26 = BallMgr.ReLoad();
												if (flag26)
												{
													Console.WriteLine("Ball info Güncelleniyor !");
												}
												Console.WriteLine("Ball info Güncellendi !");
												bool flag27 = MapMgr.ReLoadMap();
												if (flag27)
												{
													Console.WriteLine("Map info Güncelleniyor !");
												}
												Console.WriteLine("Map info Güncellendi !");
												bool flag28 = MapMgr.ReLoadMapServer();
												if (flag28)
												{
													Console.WriteLine("mapserver info Güncelleniyor !");
												}
												Console.WriteLine("mapserver Güncellendi !");
												bool flag29 = PropItemMgr.Reload();
												if (flag29)
												{
													Console.WriteLine("prop info Güncelleniyor !");
												}
												Console.WriteLine("prop info Güncellendi !");
												bool flag30 = ItemMgr.ReLoad();
												if (flag30)
												{
													Console.WriteLine("item info Güncelleniyor !");
												}
												Console.WriteLine("item info Güncellendi !");
												bool flag31 = ShopMgr.ReLoad();
												if (flag31)
												{
													Console.WriteLine("shop info Güncelleniyor !");
												}
												Console.WriteLine("shop info Güncellendi !");
												bool flag32 = QuestMgr.ReLoad();
												if (flag32)
												{
													Console.WriteLine("quest info Güncelleniyor !");
												}
												Console.WriteLine("quest info Güncellendi !");
												bool flag33 = FusionMgr.ReLoad();
												if (flag33)
												{
													Console.WriteLine("fusion info Güncelleniyor !");
												}
												Console.WriteLine("fusion info Güncellendi !");
												bool flag34 = ConsortiaMgr.ReLoad();
												if (flag34)
												{
													Console.WriteLine("consortiaMgr info Güncelleniyor !");
												}
												Console.WriteLine("consortiaMgr info Güncellendi !");
												bool flag35 = RateMgr.ReLoad();
												if (flag35)
												{
													Console.WriteLine("Rate Rate Güncelleniyor !");
												}
												Console.WriteLine("Rate Rate Güncellendi !");
												bool flag36 = NPCInfoMgr.ReLoad();
												if (flag36)
												{
													Console.WriteLine("NPCInfo Güncelleniyor !");
												}
												Console.WriteLine("NPCInfo Güncellendi !");
												bool flag37 = FightRateMgr.ReLoad();
												if (flag37)
												{
													Console.WriteLine("FightRateMgr Güncelleniyor !");
												}
												Console.WriteLine("FightRateMgr Güncellendi !");
												bool flag38 = AwardMgr.ReLoad();
												if (flag38)
												{
													Console.WriteLine("dailyaward Güncelleniyor !");
												}
												Console.WriteLine("dailyaward Güncellendi !");
												bool flag39 = LanguageMgr.Reload("");
												if (flag39)
												{
													Console.WriteLine("language Güncelleniyor !");
												}
												Console.WriteLine("language Güncellendi !");
											}
											continue;
										}
										Console.Write("Gönderilecek EXP miktarı : ");
										int num12 = int.Parse(Console.ReadLine());
										foreach (GamePlayer gamePlayer6 in WorldMgr.GetAllPlayers())
										{
											gamePlayer6.AddGP(num12);
											gamePlayer6.SendMessage("Tebrikler ! Bütün online oyunculara " + num12.ToString() + " EXP (GP) gönderildi. Online etkinliği sona ermiştir...");
										}
										Console.WriteLine("EXP (GP) gönderme başarılı (Gönderilen {0})", num12);
										continue;
									}
									else
									{
										Console.Clear();
										Console.WriteLine("Banlanacak kullanıcı adı: ");
										string text7 = Console.ReadLine();
										Console.WriteLine("Ban sebebi gir: ");
										string text8 = Console.ReadLine();
										Console.WriteLine("Ban açılış yılı gir: ");
										int num14 = int.Parse(Console.ReadLine());
										Console.WriteLine("Ban açılış ayı gir: ");
										int num15 = int.Parse(Console.ReadLine());
										Console.WriteLine("Ban açılış günü: ");
										int num16 = int.Parse(Console.ReadLine());
										DateTime date2 = new DateTime(num14, num15, num16);
										using (ManageBussiness manageBussiness4 = new ManageBussiness())
										{
											manageBussiness4.ForbidPlayerByUserName(text7, date2, false, text8);
										}
										foreach (GamePlayer gamePlayer7 in WorldMgr.GetAllPlayers())
										{
											string msg4 = string.Format("<{0}> Kullanıcı adlı oyuncumuz oyun kurallarını çiğnediğinden oyundan uzaklaştırılmıştır. Ban sebebi : ({1}). Ban açılış tarihi: ({2}.{3}.{4})", new object[]
											{
												text7,
												text8,
												num14,
												num15,
												num16
											});
											gamePlayer7.SendMessage(msg4);
										}
										Console.WriteLine("Oyuncu banlandı !.");
									}
								}
								else
								{
									Console.Clear();
									Console.WriteLine("Banlanacak oyuncunun Nick'i : ");
									string text9 = Console.ReadLine();
									Console.WriteLine("Ban sebebi: ");
									string text10 = Console.ReadLine();
									Console.WriteLine("Ban açılış yılı gir: ");
									int num18 = int.Parse(Console.ReadLine());
									Console.WriteLine("Ban açılış ayı gir: ");
									int num19 = int.Parse(Console.ReadLine());
									Console.WriteLine("Ban açılış günü gir: ");
									int num20 = int.Parse(Console.ReadLine());
									DateTime date3 = new DateTime(num18, num19, num20);
									using (ManageBussiness manageBussiness5 = new ManageBussiness())
									{
										manageBussiness5.ForbidPlayerByNickName(text9, date3, false, text10);
									}
									foreach (GamePlayer gamePlayer8 in WorldMgr.GetAllPlayers())
									{
										string msg5 = string.Format("Oyuncumuz <{0}> oyun kurallarına aykırı gelirken yakaladık ve BANLADIK. Sizde böyle olmak istemiyorsanız kurallara uyunuz. Ban sebebi : ({1}). Ban açılış tarihi: ({2}.{3}.{4})", new object[]
										{
											text9,
											text10,
											num18,
											num19,
											num20
										});
										gamePlayer8.SendMessage(msg5);
									}
									Console.WriteLine("Oyuncu " + text9 + " banlandı ve kicklendi.");
								}
							}
						}
						bool flag40 = text.Length > 0;
						if (flag40)
						{
							bool flag41 = text[0] == '/';
							if (flag41)
							{
								text = text.Remove(0, 1);
								text = text.Insert(0, "&");
							}
							try
							{
								bool flag42 = !CommandMgr.HandleCommandNoPlvl(client, text);
								if (flag42)
								{
									Console.WriteLine("Bilinmeyen komut: " + text);
								}
							}
							catch (Exception ex)
							{
								Console.WriteLine(ex.ToString());
							}
						}
					}
					catch (Exception value)
					{
						Console.WriteLine(value);
					}
				}
			}
			bool flag43 = GameServer.Instance != null;
			if (flag43)
			{
				GameServer.Instance.Stop();
			}
			LogManager.Shutdown();
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000045FC File Offset: 0x000027FC
		private static void ShutDownCallBack(object state)
		{
			ConsoleStart._count--;
			Console.WriteLine(string.Format("Server will shutdown after {0} mins!", ConsoleStart._count));
			foreach (GameClient gameClient in GameServer.Instance.GetAllClients())
			{
				bool flag = gameClient.Out != null;
				if (flag)
				{
					gameClient.Out.SendMessage(eMessageType.GM_NOTICE, string.Format("{0}{1}{2}", LanguageMgr.GetTranslation("Game.Service.actions.ShutDown1", Array.Empty<object>()), ConsoleStart._count, LanguageMgr.GetTranslation("Game.Service.actions.ShutDown2", Array.Empty<object>())));
				}
			}
			bool flag2 = ConsoleStart._count == 0;
			if (flag2)
			{
				ConsoleStart._timer.Dispose();
				ConsoleStart._timer = null;
				GameServer.Instance.Stop();
				Console.WriteLine("Server has stopped!");
				GameServer.KeepRunning = false;
				Environment.Exit(0);
			}
		}

		// Token: 0x0600002E RID: 46
		[DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall)]
		private static extern int SetConsoleCtrlHandler(ConsoleStart.ConsoleCtrlDelegate HandlerRoutine, bool add);

		// Token: 0x0600002F RID: 47 RVA: 0x000046E4 File Offset: 0x000028E4
		private static int ConsoleCtrHandler(ConsoleStart.ConsoleEvent e)
		{
			ConsoleStart.SetConsoleCtrlHandler(ConsoleStart.handler, false);
			bool flag = GameServer.Instance != null;
			if (flag)
			{
				GameServer.Instance.Stop();
			}
			return 0;
		}

		// Token: 0x0400000F RID: 15
		private GameServerConfig config;

		// Token: 0x04000010 RID: 16
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Token: 0x04000011 RID: 17
		private static System.Threading.Timer _timer;

		// Token: 0x04000012 RID: 18
		private static int _count;

		// Token: 0x04000013 RID: 19
		private static ConsoleStart.ConsoleCtrlDelegate handler;

		// Token: 0x02000010 RID: 16
		// (Invoke) Token: 0x060000BF RID: 191
		private delegate int ConsoleCtrlDelegate(ConsoleStart.ConsoleEvent ctrlType);

		// Token: 0x02000011 RID: 17
		private enum ConsoleEvent
		{
			// Token: 0x04000083 RID: 131
			Ctrl_C,
			// Token: 0x04000084 RID: 132
			Ctrl_Break,
			// Token: 0x04000085 RID: 133
			Close,
			// Token: 0x04000086 RID: 134
			Logoff,
			// Token: 0x04000087 RID: 135
			Shutdown
		}
	}
}
