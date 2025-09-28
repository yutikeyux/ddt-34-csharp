using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using Bussiness;
using Bussiness.Managers;
using Game.Base;
using Game.Base.Packets;
using Game.Logic;
using Game.Server;
using Game.Server.GameObjects;
using Game.Server.Managers;

using Game.Server.Packets;
using Game.Server.Rooms;
using SqlDataProvider.Data;

namespace Game.Service.actions
{
	public class ServerManagementForm : Form
	{
		private SqlConnection Baglanti_Db = new SqlConnection(ConfigurationManager.AppSettings.Get("conString"));

		private SqlConnection Baglanti_Membership = new SqlConnection("Data Source=WIN-8H6JMQBVGP2/GUNNYTURKEY;Initial Catalog=Db_Membership;Persist Security Info=True;User ID=sa;Password=56855685");

		private SqlConnection Baglanti_Membership_2 = new SqlConnection("Data Source=WIN-8H6JMQBVGP2/GUNNYTURKEY;Initial Catalog=Db_Membership;Persist Security Info=True;User ID=sa;Password=56855685");

		private static string link = "http://127.0.0.1/Request/";

		private IContainer components;

		private Label onlineTxt;

		private Timer UpdateUI;

		private CheckBox checkBox3;

		private GroupBox groupBox3;

		private TextBox textBox4;

		private TextBox textBox2;

		private TextBox textBox3;

		private TextBox textBox5;

		private Button button8;

		internal Label label2;

		internal Label label5;

		internal Label label3;

		internal Label label6;

		private ListBox lixtBox1;

		private Button button9;

		private Button button10;

		private Button button15;

		private TextBox textBox1;

		private Button button16;

		internal Label label9;

		private TextBox textBox6;

		private GroupBox groupBox6;

		private Label label13;

		private Label label27;

		private Label label24;

		private Label nickName;

		private Label label12;

		private Label label31;

		private Label label4;

		private Label label23;

		private Label label11;

		private Label label35;

		private Label label22;

		private Label label7;

		private Label label21;

		private Label label39;

		private Label label8;

		private Label label20;

		private Label label42;

		private Label label43;

		private Label label19;

		private Label label45;

		private Button button17;

		private TextBox textBox7;

		private TextBox textBox8;

		private Button button19;

		private GroupBox groupBox7;

		internal Label label15;

		internal Label label14;

		internal Label label17;

		internal Label label18;

		private Label label25;

		internal Label label10;

		private GroupBox groupBox5;

		private GroupBox groupBox4;

		private GroupBox groupBox8;

		internal Label label26;

		private Button button21;

		private TextBox textBox10;

		private TextBox textBox9;

		internal Label label16;

		private ComboBox comboBox1;

		private Button button4;

		private TextBox textBox11;

		private GroupBox groupBox9;

		private TextBox textBox12;

		private Button button5;

		private ComboBox comboBox2;

		private GroupBox groupBox1;

		private Button button1;

		private ComboBox comboBox3;

		private DataGridView dataGridView1;

		private ComboBox comboBox4;

		private Button button2;

		private GroupBox groupBox2;

		private TextBox textBox13;
        private Label label1;
        private Label label28;
        private Label label29;
        private Label label30;
        private Label label32;
        private Label label33;
        private Button button3;

		public string nickname { get; set; }

		public GamePlayer player { get; set; }

		public string NULL { get; set; }

		public ServerManagementForm()
		{
			InitializeComponent();
		}

		public void updateState()
		{
			try
			{
				GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
				GamePlayer[] array = allPlayers;
				List<BaseRoom> allUsingRoom = RoomMgr.GetAllUsingRoom();
				int num = ((allPlayers != null) ? allPlayers.Length : 0);
				double num2 = GC.GetTotalMemory(forceFullCollection: false);
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
				onlineTxt.Text = num.ToString();
				label25.Text = Math.Round(num2 / 1024.0 / 1024.0) + "MB";
				lixtBox1.Items.Clear();
				for (int i = 0; i < array.Length; i++)
				{
					GamePlayer gamePlayer = allPlayers[i];
					Console.WriteLine(gamePlayer.PlayerCharacter.UserName + " - [" + gamePlayer.PlayerCharacter.NickName + "] seviye: " + gamePlayer.PlayerCharacter.Grade);
					lixtBox1.Items.Add(gamePlayer.PlayerCharacter.NickName);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.StackTrace);
			}
		}

		private void UpdateUI_Tick(object sender, EventArgs e)
		{
			try
			{
				updateState();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.StackTrace);
			}
		}

		private void reloadBtn_Click(object sender, EventArgs e)
		{
			try
			{
				AllReload();
				MessageBox.Show("Tüm Veritabanı Güncellenmiştir.");
			}
			catch (Exception)
			{
				MessageBox.Show("Tüm Veritabanı Güncellenmiştir.");
				Console.WriteLine("Tüm Veritabanı Güncellenmiştir.");
				Console.WriteLine("Ula böle sistem kimsede yok 1 tuşla hepsini güncelleme :)");
			}
		}

		private static void AllReload()
		{
			throw new NotImplementedException();
		}

		private void PlayerList_Enter(object sender, EventArgs e)
		{
			Console.WriteLine("Click");
		}

		private void button8_Click(object sender, EventArgs e)
		{
			int validDate = 0;
			int gold = 0;
			int money = 0;
			int strengthenLevel = 0;
			int attackCompose = 0;
			int defendCompose = 0;
			int agilityCompose = 0;
			int luckCompose = 0;
			int templateID = int.Parse(textBox3.Text.ToString());
			string title = textBox2.Text.ToString();
			string content = textBox4.Text.ToString();
			textBox5.Text.ToString();
			int count = int.Parse(textBox5.Text.ToString());
			bool isBinds = bool.Parse(textBox6.Text.ToString());
			GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
			foreach (GamePlayer gamePlayer in allPlayers)
			{
				PlayerInfo playerInfo = new PlayerInfo();
				playerInfo = gamePlayer.PlayerCharacter;
				new PlayerBussiness().SendMailAndItem(title, content, playerInfo.ID, templateID, count, validDate, gold, money, strengthenLevel, attackCompose, defendCompose, agilityCompose, luckCompose, isBinds);
				string msg = $"[ Online Oyuncu Etkinliği Sistemi ] Hediye Ödüller Gönderilmiştir.";
				gamePlayer.SendMessage(msg);
			}
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			player = WorldMgr.GetClientByPlayerNickName(lixtBox1.GetItemText(lixtBox1.SelectedItem));
			nickName.Text = player.PlayerCharacter.NickName;
			label4.Text = player.PlayerCharacter.UserName;
			label7.Text = player.PlayerCharacter.Grade.ToString();
			label8.Text = player.PlayerCharacter.Money.ToString();
			label19.Text = player.PlayerCharacter.Attack.ToString();
			label20.Text = player.PlayerCharacter.Defence.ToString();
			label21.Text = player.PlayerCharacter.Luck.ToString();
			label22.Text = player.PlayerCharacter.Agility.ToString();
			label23.Text = player.PlayerCharacter.hp.ToString();
			label24.Text = player.PlayerCharacter.FightPower.ToString();
		}

		private void button9_Click(object sender, EventArgs e)
		{
			try
			{
				GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
				GamePlayer[] array = allPlayers;
				List<BaseRoom> allUsingRoom = RoomMgr.GetAllUsingRoom();
				if (allPlayers != null)
				{
					int num = allPlayers.Length;
				}
				GC.GetTotalMemory(forceFullCollection: false);
				int num2 = 0;
				int num3 = 0;
				foreach (BaseRoom item in allUsingRoom)
				{
					if (!item.IsEmpty)
					{
						num2++;
						if (item.IsPlaying)
						{
							num3++;
						}
					}
				}
				lixtBox1.Items.Clear();
				for (int i = 0; i < array.Length; i++)
				{
					GamePlayer gamePlayer = allPlayers[i];
					Console.WriteLine(gamePlayer.PlayerCharacter.UserName + " - [" + gamePlayer.PlayerCharacter.NickName + "] seviye: " + gamePlayer.PlayerCharacter.Grade);
					lixtBox1.Items.Add(gamePlayer.PlayerCharacter.NickName);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.StackTrace);
			}
		}

		private void button10_Click(object sender, EventArgs e)
		{
			DateTime date = new DateTime(2050, 7, 2);
			if (player == null)
			{
				return;
			}
			using ManageBussiness manageBussiness = new ManageBussiness();
			manageBussiness.ForbidPlayerByNickName(player.PlayerCharacter.NickName, date, isExist: false);
			MessageBox.Show(player.PlayerCharacter.NickName + " Oyuncu Banlandı !");
			GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
			foreach (GamePlayer gamePlayer in allPlayers)
			{
				string msg = $"Oyuncumuz <{player.PlayerCharacter.NickName}> oyun kurallarına aykırı gelirken yakaladık ve BANLADIK. Sizde böyle olmak istemiyorsanız kurallara uyunuz.";
				gamePlayer.SendMessage(msg);
			}
		}

		private void button11_Click(object sender, EventArgs e)
		{
			Process.Start("http://127.0.0.1/Request/activelist.ashx");
			MessageBox.Show("(Zaman Sınırlı) Güncellendi [Başarılı]");
		}

		private void button12_Click(object sender, EventArgs e)
		{
			Process.Start("http://127.0.0.1/Request/CelebList/CreateAllCeleb.ashx");
			MessageBox.Show("(Onur Listesi) Güncellendi [Başarılı]");
		}

		private void button13_Click(object sender, EventArgs e)
		{
			Process.Start("http://127.0.0.1/Request/LoadPVEItems.ashx");
			MessageBox.Show("(Droplar) Güncellendi [Başarılı]");
		}

		private void button14_Click(object sender, EventArgs e)
		{
			Process.Start("http://127.0.0.1/Request/questlist.ashx");
			MessageBox.Show("(Görevler) Güncellendi [Başarılı]");
		}

		private void button16_Click(object sender, EventArgs e)
		{
			DateTime date = new DateTime(2050, 7, 2);
			using (ManageBussiness manageBussiness = new ManageBussiness())
			{
				manageBussiness.ForbidPlayerByNickName(textBox1.Text, date, isExist: true);
			}
			GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
			int num = 0;
			if (num < allPlayers.Length)
			{
				GamePlayer gamePlayer = allPlayers[num];
				MessageBox.Show("Oyuncumuz Banı Açılmıştır ");
				string msg = $"Oyuncumuz <{textBox1.Text}> Banı Açılmıştır ";
				gamePlayer.SendMessage(msg);
			}
		}

		private void button10_Click_1(object sender, EventArgs e)
		{
			DateTime date = new DateTime(2050, 7, 2);
			if (player == null)
			{
				return;
			}
			using ManageBussiness manageBussiness = new ManageBussiness();
			manageBussiness.ForbidPlayerByNickName(nickName.Text, date, isExist: false);
			MessageBox.Show(player.PlayerCharacter.NickName + " Oyuncu Banlandı !");
			GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
			foreach (GamePlayer gamePlayer in allPlayers)
			{
				string msg = $"Oyuncumuz <{player.PlayerCharacter.NickName}> oyun kurallarına aykırı gelirken yakaladık ve BANLADIK. Sizde böyle olmak istemiyorsanız kurallara uyunuz.";
				gamePlayer.SendMessage(msg);
			}
		}

		private void button15_Click(object sender, EventArgs e)
		{
			if (player == null)
			{
				return;
			}
			using ManageBussiness manageBussiness = new ManageBussiness();
			manageBussiness.KitoffUserByNickName(nickName.Text, " Kick yedi");
			MessageBox.Show(player.PlayerCharacter.NickName + " kicklendi !");
			GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
			for (int i = 0; i < allPlayers.Length; i++)
			{
				allPlayers[i].SendMessage(string.Format("Oyuncu [{0}] Oyunculara rahatsızlığından dolayı kick yemiştir"));
			}
		}

		private void button9_Click_1(object sender, EventArgs e)
		{
			try
			{
				updateState();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.StackTrace);
			}
		}

		private void button17_Click(object sender, EventArgs e)
		{
			GC.Collect();
			MessageBox.Show("Ram temizlendi !");
			updateState();
		}

		private void button18_Click(object sender, EventArgs e)
		{
			Process.Start("http://127.0.0.1/Request/NPCInfoList.ashx");
			MessageBox.Show("(NPC'Ler) Güncellendi [Başarılı]");
		}

		private void button19_Click(object sender, EventArgs e)
		{
			if (GameServer.KeepRunning)
			{
				string text = textBox8.Text;
				string text2 = textBox7.Text;
				SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.AppSettings.Get("conString"));
				sqlConnection.Open();
				SqlCommand sqlCommand = new SqlCommand("update Sys_Users_Detail set NickName ='" + text2 + "' WHERE NickName=('" + text + "')");
				sqlCommand.Connection = sqlConnection;
				sqlCommand.ExecuteNonQuery();
				sqlConnection.Close();
				MessageBox.Show("Nick Değişme İşleminiz Gerçekleşmiştir !");
				textBox8.Text = null;
				textBox7.Text = null;
			}
			else
			{
				MessageBox.Show("Bilgileri Kontrol Ediniz ! ");
			}
		}

		private void button20_Click(object sender, EventArgs e)
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
			Console.WriteLine("Tek bir tıkla tüm veritabanı'nı güncelledin daha ne istiyorsun :)");
			MessageBox.Show("Veritabanı Güncellenmiştir.");
		}

		private void button21_Click(object sender, EventArgs e)
		{
			string text = textBox10.Text;
			SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.AppSettings.Get("conString"));
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlConnection;
			sqlCommand.CommandText = "SELECT ActiveIP FROM Sys_Users_Detail WHERE NickName='" + text + "'";
			sqlConnection.Open();
			SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
			while (sqlDataReader.Read())
			{
				textBox9.Text = NULL;
				textBox9.Text = sqlDataReader["ActiveIP"].ToString();
			}
			sqlConnection.Close();
		}

		private void button4_Click_1(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(comboBox1.Text))
			{
				GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
				foreach (GamePlayer gamePlayer in allPlayers)
				{
					if (comboBox1.Text == "Kupon")
					{
						gamePlayer.AddMoney(int.Parse(textBox11.Text));
					}
					else if (comboBox1.Text == "Exp")
					{
						gamePlayer.AddGP(int.Parse(textBox11.Text));
					}
					else if (comboBox1.Text == "Onur")
					{
						gamePlayer.AddHonor(int.Parse(textBox11.Text));
					}
					else if (comboBox1.Text == "Kart Ruhu")
					{
						//gamePlayer.AddCardSoul(int.Parse(textBox11.Text));
					}
					else if (comboBox1.Text == "Bağlı Kupon")
					{
						gamePlayer.AddGiftToken(int.Parse(textBox11.Text));
					}
					else if (comboBox1.Text == "Mükafat")
					{
						gamePlayer.AddOffer(int.Parse(textBox11.Text));
					}
					gamePlayer.SendMessage("Tüm Çevrimiçi Oyunculara [" + textBox11.Text + "] [" + comboBox1.Text + "] Gönderilmiştir ^_^");
				}
			}
			else
			{
				MessageBox.Show("Lütfen Seçim Yapınız.!", "Uyarı");
			}
		}

		private void button5_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(comboBox2.Text))
			{
				if (comboBox2.Text == "Küçük Hoparlör")
				{
					GSPacketIn gSPacketIn = new GSPacketIn(71);
					gSPacketIn.WriteInt(0);
					gSPacketIn.WriteString("Sistem");
					gSPacketIn.WriteString(textBox12.Text);
					GameServer.Instance.LoginServer.SendPacket(gSPacketIn);
					Herkes(gSPacketIn);
				}
				else if (comboBox2.Text == "Büyük Hoparlör")
				{
					GSPacketIn gSPacketIn2 = new GSPacketIn(72);
					gSPacketIn2.WriteInt(0);
					gSPacketIn2.WriteInt(0);
					gSPacketIn2.WriteString("Sistem");
					gSPacketIn2.WriteString(textBox12.Text);
					GameServer.Instance.LoginServer.SendPacket(gSPacketIn2);
					Herkes(gSPacketIn2);
				}
				else if (comboBox2.Text == "Sarı Mesaj")
				{
					Herkes_2(eMessageType.ChatNormal, textBox12.Text);
				}
				else if (comboBox2.Text == "Sistem Mesajı")
				{
					Herkes_2(eMessageType.ChatERROR, textBox12.Text);
				}
				else if (comboBox2.Text == "Kırmızı Mesaj")
				{
					GSPacketIn gSPacketIn3 = new GSPacketIn(73, 0);
					gSPacketIn3.WriteInt(1);
					gSPacketIn3.WriteInt(0);
					gSPacketIn3.WriteString("Sistem");
					gSPacketIn3.WriteString(textBox12.Text);
					gSPacketIn3.WriteString("Yönetim");
					GameServer.Instance.LoginServer.SendPacket(gSPacketIn3);
					Herkes(gSPacketIn3);
				}
				else if (comboBox2.Text == "Mor Mesaj")
				{
					new ManageBussiness().SystemNotice(textBox12.Text);
				}
				else if (comboBox2.Text == "Admin Mesajı")
				{
					Herkes_2(eMessageType.ALERT, textBox12.Text);
				}
			}
			else
			{
				MessageBox.Show("Lütfen Seçim Yapınız.!", "Uyarı");
			}
		}

		public static void Herkes(GSPacketIn Paket)
		{
			GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
			for (int i = 0; i < allPlayers.Length; i++)
			{
				allPlayers[i].SendTCP(Paket);
			}
		}

		public static void Herkes_2(eMessageType Msj, string Msj2)
		{
			GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
			for (int i = 0; i < allPlayers.Length; i++)
			{
				allPlayers[i].Out.SendMessage(Msj, Msj2);
			}
		}

		private void zaza1(string zaza)
		{
			try
			{
				string address = link + zaza;
				new WebClient().DownloadString(address);
				Console.WriteLine("Güncelleme Başarılı: " + DateTime.Now.ToString());
			}
			catch
			{
				Console.WriteLine("Güncelleme Başarısız.!");
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(comboBox3.Text))
			{
				if (comboBox3.Text == "Tüm Veritabanını Güncelle")
				{
					if (BallMgr.ReLoad())
					{
						Console.WriteLine("Ball info Güncelleniyor !");
						zaza1("Balllist.ashx");
					}
					Console.WriteLine("Ball info Güncellendi !");
					if (MapMgr.ReLoadMap())
					{
						Console.WriteLine("Map info Güncelleniyor !");
						zaza1("MapServerList.ashx");
					}
					Console.WriteLine("Map info Güncellendi !");
					if (MapMgr.ReLoadMapServer())
					{
						Console.WriteLine("mapserver info Güncelleniyor !");
						zaza1("MapServerList.ashx");
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
						zaza1("ShopItemList.ashx");
					}
					Console.WriteLine("shop info Güncellendi !");
					if (QuestMgr.ReLoad())
					{
						Console.WriteLine("quest info Güncelleniyor !");
						zaza1("QuestList.ashx");
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
						zaza1("ConsortiaAllyList.ashx");
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
						zaza1("NPCInfoList.ashx");
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
					Console.WriteLine("Veritabanı Başarıyla Güncellendi !");
					MessageBox.Show("Veritabanı Güncellenmiştir.");
				}
				else if (comboBox3.Text == "Görev Güncelle")
				{
					zaza1("QuestList.ashx");
					MessageBox.Show("(Görevler) Güncellendi [Başarılı]");
				}
				else if (comboBox3.Text == "Onur Listesi Güncelle")
				{
					zaza1("CelebList/CreateAllCeleb.ashx");
					MessageBox.Show("(Onur Listesi) Güncellendi [Başarılı]");
				}
				else if (comboBox3.Text == "Etkinlikleri Güncelle")
				{
					zaza1("ActiveList.ashx");
					MessageBox.Show("(Zaman Sınırlı) Güncellendi [Başarılı]");
				}
				else if (comboBox3.Text == "Pve_İnfo Güncelle")
				{
					zaza1("LoadPVEItems.ashx");
					MessageBox.Show("(Droplar) Güncellendi [Başarılı]");
				}
				else if (comboBox3.Text == "Templatelist Güncelle")
				{
					zaza1("TemplateAlllist.ashx");
					MessageBox.Show("Templist Güncellendi. [Başarılı]");
				}
				else if (comboBox3.Text == "Füzyon Güncelle")
				{
					if (FusionMgr.ReLoad())
					{
						Console.WriteLine("fusion Güncelleniyor...");
					}
					Console.WriteLine("fusion Güncellendi.!");
					MessageBox.Show("Füzyon Güncellendi.");
				}
				else if (comboBox3.Text == "Shop Güncelle")
				{
					if (ShopMgr.ReLoad())
					{
						zaza1("ShopItemList.ashx");
						Console.WriteLine("Shop Güncelleniyor...");
					}
					Console.WriteLine("Shop Güncellendi.!");
					MessageBox.Show("Shop Güncellendi.");
				}
				else if (comboBox3.Text == "Mission Güncelle")
				{
					if (MissionInfoMgr.Reload())
					{
						Console.WriteLine("Şartlar Güncelleniyor...");
					}
					Console.WriteLine("Şartlar Güncellendi.!");
					MessageBox.Show("Şartlar Güncellendi.");
				}
				else if (comboBox3.Text == "Npc Güncelle")
				{
					if (NPCInfoMgr.ReLoad())
					{
						zaza1("NPCInfoList.ashx");
						Console.WriteLine("Keşifler Güncelleniyor !");
					}
					Console.WriteLine("Keşifler Güncellendi !");
					MessageBox.Show("Keşifler Güncellenmiştir.");
				}
				else if (comboBox3.Text == "Ball Güncelle")
				{
					if (BallMgr.ReLoad())
					{
						zaza1("Balllist.ashx");
						Console.WriteLine("Ball Güncelleniyor...");
					}
					Console.WriteLine("Ball Güncellendi.!");
					MessageBox.Show("Ball Güncellendi.");
				}
				else if (comboBox3.Text == "Ball Config Güncelle")
				{
					if (BallConfigMgr.ReLoad())
					{
						zaza1("BombConfig.ashx");
						Console.WriteLine("Ball Config Güncelleniyor...");
					}
					Console.WriteLine("Ball Config Güncellendi.!");
					MessageBox.Show("Ball Config Güncellendi.");
				}
				else if (comboBox3.Text == "Yazıları Güncelle")
				{
					if (LanguageMgr.Reload(""))
					{
						Console.WriteLine("language Güncelleniyor...");
					}
					Console.WriteLine("language Güncellendi.!");
					MessageBox.Show("Yazılar Güncellendi.");
				}
				else if (comboBox3.Text == "Goldları Güncelle")
				{
					if (GoldEquipMgr.ReLoad())
					{
						zaza1("GoldEquipTemplateLoad.ashx");
						Console.WriteLine("Goldlar Güncelleniyor...");
					}
					Console.WriteLine("Goldlar Güncellendi.!");
					MessageBox.Show("Goldlar Güncellendi.");
				}
				else if (comboBox3.Text == "Mapları Güncelle")
				{
					if (MapMgr.ReLoadMap())
					{
						Console.WriteLine("Map info Güncelleniyor !");
					}
					if (MapMgr.ReLoadMapServer())
					{
						Console.WriteLine("mapserver info Güncelleniyor !");
					}
					zaza1("MapServerList.ashx");
					Console.WriteLine("Maplar Güncellendi !");
					MessageBox.Show("Maplar Başarıyla Güncellendi !", "Bilgi");
				}
				else if (comboBox3.Text == "Dropları Güncelle")
				{
					if (DropMgr.ReLoad())
					{
						zaza1("LoadPVEItems.ashx");
						Console.WriteLine("Droplar Güncelleniyor !");
					}
					Console.WriteLine("Droplar Güncellendi !");
					MessageBox.Show("Droplar Başarıyla Güncellendi !", "Bilgi");
				}
				else if (comboBox3.Text == "EventAward Güncelle")
				{
					if (EventAwardMgr.ReLoad())
					{
						Console.WriteLine("EventAward Güncelleniyor !");
					}
					Console.WriteLine("EventAward Güncellendi !");
					MessageBox.Show("EventAward Başarıyla Güncellendi !", "Bilgi");
				}
			}
			else
			{
				MessageBox.Show("Lütfen Seçim Yapınız.!", "Uyarı");
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			dataGridView1.ForeColor = Color.Black;
			if (comboBox4.Text == "Banlı Hesaplar")
			{
				Baglanti_Db.Open();
				DbDataAdapter dbDataAdapter = new SqlDataAdapter("Select NickName[Nick], ForbidReason[Ban Sebebi] From Sys_Users_Detail Where IsExist='" + 0 + "'", Baglanti_Db);
				DataTable dataTable = new DataTable();
				dbDataAdapter.Fill(dataTable);
				dataGridView1.DataSource = dataTable;
				Baglanti_Db.Close();
			}
		}

		private void button3_Click(object sender, EventArgs e)
		{
			SqlCommand sqlCommand = new SqlCommand("select UserId From Mem_Users Where UserName ='" + textBox13.Text + "'", Baglanti_Membership);
			Baglanti_Membership.Open();
			SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
			while (sqlDataReader.Read())
			{
				string text = sqlDataReader["UserId"].ToString();
				Baglanti_Membership_2.Open();
				new SqlCommand("Update Mem_UserInfo Set Password ='e10adc3949ba59abbe56e057f20f883e' Where UserId='" + text + "'", Baglanti_Membership_2).ExecuteNonQuery();
				Baglanti_Membership_2.Close();
			}
			Baglanti_Membership.Close();
			MessageBox.Show("Kişinin Şifresi Başarıyla Değiştirilmiştir, Yeni Şifre: 123456", "Bilgi");
		}

		private void ServerManagementForm_Load(object sender, EventArgs e)
        {

            if (HydroFilter.IsActive)
            {
                label28.Text = "Aktif";
                label28.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                label28.Text = "Pasif";
                label28.ForeColor = System.Drawing.Color.Red;
            }

            label30.Text = HydroFilter.ConnectionCount.ToString();
            label33.Text = HydroFilter.BlockedConnections.ToString();
        }

        protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.onlineTxt = new System.Windows.Forms.Label();
            this.UpdateUI = new System.Windows.Forms.Timer(this.components);
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.button8 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lixtBox1 = new System.Windows.Forms.ListBox();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button16 = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.nickName = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.button17 = new System.Windows.Forms.Button();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.button19 = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.button21 = new System.Windows.Forms.Button();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox13 = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.groupBox3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // onlineTxt
            // 
            this.onlineTxt.AutoSize = true;
            this.onlineTxt.BackColor = System.Drawing.Color.DarkSlateBlue;
            this.onlineTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.onlineTxt.ForeColor = System.Drawing.Color.Yellow;
            this.onlineTxt.Location = new System.Drawing.Point(150, 15);
            this.onlineTxt.Name = "onlineTxt";
            this.onlineTxt.Size = new System.Drawing.Size(27, 20);
            this.onlineTxt.TabIndex = 0;
            this.onlineTxt.Text = "00";
            // 
            // UpdateUI
            // 
            this.UpdateUI.Enabled = true;
            this.UpdateUI.Interval = 10000;
            this.UpdateUI.Tick += new System.EventHandler(this.UpdateUI_Tick);
            // 
            // checkBox3
            // 
            this.checkBox3.Location = new System.Drawing.Point(104, 201);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(104, 24);
            this.checkBox3.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBox3);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(1492, 16);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(10, 10);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Акции и дебаг";
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.SystemColors.Window;
            this.textBox4.Location = new System.Drawing.Point(111, 92);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(207, 26);
            this.textBox4.TabIndex = 22;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.SystemColors.Window;
            this.textBox2.Location = new System.Drawing.Point(111, 60);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(207, 26);
            this.textBox2.TabIndex = 23;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.SystemColors.Window;
            this.textBox3.Location = new System.Drawing.Point(111, 28);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(207, 26);
            this.textBox3.TabIndex = 24;
            // 
            // textBox5
            // 
            this.textBox5.BackColor = System.Drawing.SystemColors.Window;
            this.textBox5.Location = new System.Drawing.Point(111, 123);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(207, 26);
            this.textBox5.TabIndex = 25;
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.Color.LightBlue;
            this.button8.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button8.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.button8.Location = new System.Drawing.Point(111, 188);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(207, 32);
            this.button8.TabIndex = 26;
            this.button8.Text = "Gönder";
            this.button8.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button8.UseVisualStyleBackColor = false;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Magenta;
            this.label2.Location = new System.Drawing.Point(24, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 20);
            this.label2.TabIndex = 28;
            this.label2.Text = "İtem ID :";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Magenta;
            this.label9.Location = new System.Drawing.Point(24, 156);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 20);
            this.label9.TabIndex = 35;
            this.label9.Text = "Bağlımı :";
            // 
            // textBox6
            // 
            this.textBox6.BackColor = System.Drawing.SystemColors.Window;
            this.textBox6.Location = new System.Drawing.Point(111, 156);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(207, 26);
            this.textBox6.TabIndex = 34;
            this.textBox6.Text = "True";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Magenta;
            this.label6.Location = new System.Drawing.Point(24, 92);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 20);
            this.label6.TabIndex = 33;
            this.label6.Text = "İçerik :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Magenta;
            this.label5.Location = new System.Drawing.Point(24, 123);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 20);
            this.label5.TabIndex = 31;
            this.label5.Text = "Adet :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Magenta;
            this.label3.Location = new System.Drawing.Point(24, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 20);
            this.label3.TabIndex = 29;
            this.label3.Text = "Başlık :";
            // 
            // lixtBox1
            // 
            this.lixtBox1.BackColor = System.Drawing.Color.Black;
            this.lixtBox1.CausesValidation = false;
            this.lixtBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lixtBox1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lixtBox1.FormattingEnabled = true;
            this.lixtBox1.ItemHeight = 20;
            this.lixtBox1.Location = new System.Drawing.Point(12, 71);
            this.lixtBox1.Name = "lixtBox1";
            this.lixtBox1.Size = new System.Drawing.Size(190, 504);
            this.lixtBox1.TabIndex = 29;
            this.lixtBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // button9
            // 
            this.button9.BackColor = System.Drawing.Color.LightBlue;
            this.button9.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button9.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.button9.Location = new System.Drawing.Point(12, 582);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(190, 41);
            this.button9.TabIndex = 31;
            this.button9.Text = "Güncelle !";
            this.button9.UseVisualStyleBackColor = false;
            this.button9.Click += new System.EventHandler(this.button9_Click_1);
            // 
            // button10
            // 
            this.button10.BackColor = System.Drawing.Color.Lime;
            this.button10.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.button10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button10.ForeColor = System.Drawing.Color.Red;
            this.button10.Location = new System.Drawing.Point(220, 25);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(164, 38);
            this.button10.TabIndex = 31;
            this.button10.Text = "Banla !";
            this.button10.UseVisualStyleBackColor = false;
            this.button10.Click += new System.EventHandler(this.button10_Click_1);
            // 
            // button15
            // 
            this.button15.BackColor = System.Drawing.Color.Chartreuse;
            this.button15.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.button15.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button15.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button15.ForeColor = System.Drawing.Color.Red;
            this.button15.Location = new System.Drawing.Point(221, 69);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(164, 38);
            this.button15.TabIndex = 38;
            this.button15.Text = "Kickle !";
            this.button15.UseVisualStyleBackColor = false;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // textBox1
            // 
            this.textBox1.AccessibleName = "";
            this.textBox1.BackColor = System.Drawing.SystemColors.Window;
            this.textBox1.Location = new System.Drawing.Point(220, 136);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(166, 20);
            this.textBox1.TabIndex = 33;
            // 
            // button16
            // 
            this.button16.BackColor = System.Drawing.Color.Chartreuse;
            this.button16.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.button16.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button16.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button16.ForeColor = System.Drawing.Color.Red;
            this.button16.Location = new System.Drawing.Point(220, 167);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(166, 30);
            this.button16.TabIndex = 39;
            this.button16.Text = "Banı Aç !";
            this.button16.UseVisualStyleBackColor = false;
            this.button16.Click += new System.EventHandler(this.button16_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.Color.Red;
            this.groupBox6.Controls.Add(this.label13);
            this.groupBox6.Controls.Add(this.label27);
            this.groupBox6.Controls.Add(this.label24);
            this.groupBox6.Controls.Add(this.nickName);
            this.groupBox6.Controls.Add(this.label12);
            this.groupBox6.Controls.Add(this.label31);
            this.groupBox6.Controls.Add(this.label4);
            this.groupBox6.Controls.Add(this.label23);
            this.groupBox6.Controls.Add(this.label11);
            this.groupBox6.Controls.Add(this.label35);
            this.groupBox6.Controls.Add(this.label22);
            this.groupBox6.Controls.Add(this.label7);
            this.groupBox6.Controls.Add(this.label21);
            this.groupBox6.Controls.Add(this.label39);
            this.groupBox6.Controls.Add(this.label8);
            this.groupBox6.Controls.Add(this.label20);
            this.groupBox6.Controls.Add(this.label42);
            this.groupBox6.Controls.Add(this.label43);
            this.groupBox6.Controls.Add(this.label19);
            this.groupBox6.Controls.Add(this.label45);
            this.groupBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox6.ForeColor = System.Drawing.Color.White;
            this.groupBox6.Location = new System.Drawing.Point(220, 203);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(176, 471);
            this.groupBox6.TabIndex = 61;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Oyuncu Bilgileri";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label13.ForeColor = System.Drawing.Color.White;
            this.label13.Location = new System.Drawing.Point(13, 33);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(35, 16);
            this.label13.TabIndex = 44;
            this.label13.Text = "Nick";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label27.ForeColor = System.Drawing.Color.White;
            this.label27.Location = new System.Drawing.Point(14, 253);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(82, 16);
            this.label27.TabIndex = 58;
            this.label27.Text = "Savaşma G.";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label24.ForeColor = System.Drawing.Color.White;
            this.label24.Location = new System.Drawing.Point(83, 253);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(82, 16);
            this.label24.TabIndex = 59;
            this.label24.Text = "Savaşma G.";
            // 
            // nickName
            // 
            this.nickName.AutoSize = true;
            this.nickName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.nickName.ForeColor = System.Drawing.Color.White;
            this.nickName.Location = new System.Drawing.Point(83, 33);
            this.nickName.Name = "nickName";
            this.nickName.Size = new System.Drawing.Size(35, 16);
            this.nickName.TabIndex = 40;
            this.nickName.Text = "Nick";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(14, 57);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(39, 16);
            this.label12.TabIndex = 45;
            this.label12.Text = "K.Adı";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label31.ForeColor = System.Drawing.Color.White;
            this.label31.Location = new System.Drawing.Point(14, 227);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(32, 16);
            this.label31.TabIndex = 57;
            this.label31.Text = "Can";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(83, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 16);
            this.label4.TabIndex = 41;
            this.label4.Text = "K.Adı";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label23.ForeColor = System.Drawing.Color.White;
            this.label23.Location = new System.Drawing.Point(83, 227);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(32, 16);
            this.label23.TabIndex = 56;
            this.label23.Text = "Can";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(14, 79);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 16);
            this.label11.TabIndex = 46;
            this.label11.Text = "Level";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label35.ForeColor = System.Drawing.Color.White;
            this.label35.Location = new System.Drawing.Point(14, 200);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(39, 16);
            this.label35.TabIndex = 51;
            this.label35.Text = "Şans";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label22.ForeColor = System.Drawing.Color.White;
            this.label22.Location = new System.Drawing.Point(83, 200);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(39, 16);
            this.label22.TabIndex = 55;
            this.label22.Text = "Şans";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(83, 79);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 16);
            this.label7.TabIndex = 42;
            this.label7.Text = "Level";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label21.ForeColor = System.Drawing.Color.White;
            this.label21.Location = new System.Drawing.Point(83, 173);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(58, 16);
            this.label21.TabIndex = 54;
            this.label21.Text = "Çeviklik ";
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label39.ForeColor = System.Drawing.Color.White;
            this.label39.Location = new System.Drawing.Point(14, 173);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(58, 16);
            this.label39.TabIndex = 50;
            this.label39.Text = "Çeviklik ";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(83, 101);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 16);
            this.label8.TabIndex = 47;
            this.label8.Text = "Kupon";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label20.ForeColor = System.Drawing.Color.White;
            this.label20.Location = new System.Drawing.Point(83, 148);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(65, 16);
            this.label20.TabIndex = 53;
            this.label20.Text = "Savunma";
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label42.ForeColor = System.Drawing.Color.White;
            this.label42.Location = new System.Drawing.Point(14, 101);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(46, 16);
            this.label42.TabIndex = 43;
            this.label42.Text = "Kupon";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label43.ForeColor = System.Drawing.Color.White;
            this.label43.Location = new System.Drawing.Point(14, 148);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(65, 16);
            this.label43.TabIndex = 49;
            this.label43.Text = "Savunma";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label19.ForeColor = System.Drawing.Color.White;
            this.label19.Location = new System.Drawing.Point(83, 123);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(46, 16);
            this.label19.TabIndex = 52;
            this.label19.Text = "Saldırı";
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label45.ForeColor = System.Drawing.Color.White;
            this.label45.Location = new System.Drawing.Point(14, 123);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(46, 16);
            this.label45.TabIndex = 48;
            this.label45.Text = "Saldırı";
            // 
            // button17
            // 
            this.button17.BackColor = System.Drawing.Color.LightBlue;
            this.button17.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.button17.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button17.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button17.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.button17.Location = new System.Drawing.Point(12, 633);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(190, 41);
            this.button17.TabIndex = 62;
            this.button17.Text = "Ram\'ı Temizle !";
            this.button17.UseVisualStyleBackColor = false;
            this.button17.Click += new System.EventHandler(this.button17_Click);
            // 
            // textBox7
            // 
            this.textBox7.BackColor = System.Drawing.SystemColors.Window;
            this.textBox7.Location = new System.Drawing.Point(10, 104);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(308, 26);
            this.textBox7.TabIndex = 64;
            // 
            // textBox8
            // 
            this.textBox8.BackColor = System.Drawing.SystemColors.Window;
            this.textBox8.Location = new System.Drawing.Point(10, 51);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(308, 26);
            this.textBox8.TabIndex = 65;
            // 
            // button19
            // 
            this.button19.BackColor = System.Drawing.Color.LightBlue;
            this.button19.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.button19.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button19.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button19.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.button19.Location = new System.Drawing.Point(10, 137);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(308, 32);
            this.button19.TabIndex = 66;
            this.button19.Text = "Değiştir ";
            this.button19.UseVisualStyleBackColor = false;
            this.button19.Click += new System.EventHandler(this.button19_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.label15);
            this.groupBox7.Controls.Add(this.label14);
            this.groupBox7.Controls.Add(this.textBox7);
            this.groupBox7.Controls.Add(this.button19);
            this.groupBox7.Controls.Add(this.textBox8);
            this.groupBox7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox7.ForeColor = System.Drawing.Color.White;
            this.groupBox7.Location = new System.Drawing.Point(654, 247);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(336, 183);
            this.groupBox7.TabIndex = 19;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Oyuncu Nick Değiştirme";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.ForeColor = System.Drawing.Color.Red;
            this.label15.Location = new System.Drawing.Point(6, 81);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(94, 20);
            this.label15.TabIndex = 68;
            this.label15.Text = "Yeni Nick :";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ForeColor = System.Drawing.Color.Red;
            this.label14.Location = new System.Drawing.Point(12, 31);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 20);
            this.label14.TabIndex = 67;
            this.label14.Text = "Nick :";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label17.ForeColor = System.Drawing.Color.White;
            this.label17.Location = new System.Drawing.Point(9, 15);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(103, 20);
            this.label17.TabIndex = 71;
            this.label17.Text = "Online Sayısı:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label18.ForeColor = System.Drawing.Color.Cornsilk;
            this.label18.Location = new System.Drawing.Point(9, 40);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(113, 20);
            this.label18.TabIndex = 72;
            this.label18.Text = "Ram Kullanımı:";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.BackColor = System.Drawing.Color.DarkSlateBlue;
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label25.ForeColor = System.Drawing.Color.Yellow;
            this.label25.Location = new System.Drawing.Point(150, 40);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(27, 20);
            this.label25.TabIndex = 73;
            this.label25.Text = "00";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(223, 113);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(43, 20);
            this.label10.TabIndex = 74;
            this.label10.Text = "Nick:";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.textBox6);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.textBox3);
            this.groupBox5.Controls.Add(this.button8);
            this.groupBox5.Controls.Add(this.textBox2);
            this.groupBox5.Controls.Add(this.textBox4);
            this.groupBox5.Controls.Add(this.textBox5);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.ForeColor = System.Drawing.Color.White;
            this.groupBox5.Location = new System.Drawing.Point(654, 15);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(336, 226);
            this.groupBox5.TabIndex = 28;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Online İtem Etkinliği";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.textBox11);
            this.groupBox4.Controls.Add(this.button4);
            this.groupBox4.Controls.Add(this.comboBox1);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.ForeColor = System.Drawing.Color.White;
            this.groupBox4.Location = new System.Drawing.Point(404, 87);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(231, 101);
            this.groupBox4.TabIndex = 27;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Online Etkinliği";
            // 
            // textBox11
            // 
            this.textBox11.AccessibleName = "";
            this.textBox11.BackColor = System.Drawing.SystemColors.InfoText;
            this.textBox11.ForeColor = System.Drawing.SystemColors.Info;
            this.textBox11.Location = new System.Drawing.Point(6, 62);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(122, 26);
            this.textBox11.TabIndex = 34;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.LightBlue;
            this.button4.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button4.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.button4.Location = new System.Drawing.Point(134, 57);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(91, 32);
            this.button4.TabIndex = 27;
            this.button4.Text = "Gönder";
            this.button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click_1);
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.comboBox1.ForeColor = System.Drawing.SystemColors.Info;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Kupon",
            "Exp",
            "Onur",
            "Kart Ruhu",
            "Bağlı Kupon",
            "Mükafat"});
            this.comboBox1.Location = new System.Drawing.Point(6, 28);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(122, 28);
            this.comboBox1.TabIndex = 0;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.label16);
            this.groupBox8.Controls.Add(this.textBox9);
            this.groupBox8.Controls.Add(this.label26);
            this.groupBox8.Controls.Add(this.button21);
            this.groupBox8.Controls.Add(this.textBox10);
            this.groupBox8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox8.ForeColor = System.Drawing.Color.White;
            this.groupBox8.Location = new System.Drawing.Point(654, 436);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(336, 187);
            this.groupBox8.TabIndex = 75;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Oyuncu İP Adresi Bulma";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.ForeColor = System.Drawing.Color.Magenta;
            this.label16.Location = new System.Drawing.Point(12, 78);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(92, 20);
            this.label16.TabIndex = 69;
            this.label16.Text = "İP Adresi :";
            // 
            // textBox9
            // 
            this.textBox9.BackColor = System.Drawing.SystemColors.Window;
            this.textBox9.Location = new System.Drawing.Point(16, 104);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(302, 26);
            this.textBox9.TabIndex = 68;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.ForeColor = System.Drawing.Color.Magenta;
            this.label26.Location = new System.Drawing.Point(12, 26);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(53, 20);
            this.label26.TabIndex = 67;
            this.label26.Text = "Nick :";
            // 
            // button21
            // 
            this.button21.BackColor = System.Drawing.Color.LightBlue;
            this.button21.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.button21.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button21.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button21.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.button21.Location = new System.Drawing.Point(16, 136);
            this.button21.Name = "button21";
            this.button21.Size = new System.Drawing.Size(302, 32);
            this.button21.TabIndex = 66;
            this.button21.Text = "İP Adresini Göster";
            this.button21.UseVisualStyleBackColor = false;
            this.button21.Click += new System.EventHandler(this.button21_Click);
            // 
            // textBox10
            // 
            this.textBox10.BackColor = System.Drawing.SystemColors.Window;
            this.textBox10.Location = new System.Drawing.Point(16, 49);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(302, 26);
            this.textBox10.TabIndex = 65;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.textBox12);
            this.groupBox9.Controls.Add(this.button5);
            this.groupBox9.Controls.Add(this.comboBox2);
            this.groupBox9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox9.ForeColor = System.Drawing.Color.White;
            this.groupBox9.Location = new System.Drawing.Point(402, 272);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(231, 155);
            this.groupBox9.TabIndex = 76;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Yönetim Mesajı";
            // 
            // textBox12
            // 
            this.textBox12.AccessibleName = "";
            this.textBox12.BackColor = System.Drawing.SystemColors.Window;
            this.textBox12.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox12.Location = new System.Drawing.Point(6, 62);
            this.textBox12.Multiline = true;
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new System.Drawing.Size(219, 82);
            this.textBox12.TabIndex = 34;
            this.textBox12.Text = "Mesaj Yaz...";
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.LightBlue;
            this.button5.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button5.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.button5.Location = new System.Drawing.Point(134, 26);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(91, 32);
            this.button5.TabIndex = 27;
            this.button5.Text = "Gönder";
            this.button5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "Büyük Hoparlör",
            "Küçük Hoparlör",
            "Sistem Mesajı",
            "Sarı Mesaj",
            "Mor Mesaj",
            "Kırmızı Mesaj",
            "Admin Mesajı"});
            this.comboBox2.Location = new System.Drawing.Point(6, 28);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(122, 28);
            this.comboBox2.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.comboBox3);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(404, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(231, 69);
            this.groupBox1.TabIndex = 77;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Verileri Güncelle";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.LightBlue;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.button1.Location = new System.Drawing.Point(134, 26);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(91, 32);
            this.button1.TabIndex = 27;
            this.button1.Text = "Güncelle";
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBox3
            // 
            this.comboBox3.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.comboBox3.ForeColor = System.Drawing.SystemColors.Info;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Items.AddRange(new object[] {
            "Seçim Yapınız",
            "Tüm Veritabanını Güncelle",
            "Görev Güncelle",
            "Onur Listesi Güncelle",
            "Etkinlikleri Güncelle",
            "Pve_İnfo Güncelle",
            "Templatelist Güncelle",
            "Füzyon Güncelle",
            "Shop Güncelle",
            "Mission Güncelle",
            "Npc Güncelle",
            "Ball Güncelle",
            "Ball Config Güncelle",
            "Yazıları Güncelle",
            "Goldları Güncelle",
            "EventAward Güncelle",
            "Dropları Güncelle",
            "Mapları Güncelle"});
            this.comboBox3.Location = new System.Drawing.Point(6, 28);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(122, 28);
            this.comboBox3.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(404, 433);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(231, 216);
            this.dataGridView1.TabIndex = 78;
            // 
            // comboBox4
            // 
            this.comboBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Items.AddRange(new object[] {
            "Banlı Hesaplar"});
            this.comboBox4.Location = new System.Drawing.Point(406, 655);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(107, 24);
            this.comboBox4.TabIndex = 79;
            this.comboBox4.Text = "Banlı Hesaplar";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.LightBlue;
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.button2.Location = new System.Drawing.Point(520, 655);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(115, 25);
            this.button2.TabIndex = 80;
            this.button2.Text = "Göster";
            this.button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox13);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(402, 194);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(231, 69);
            this.groupBox2.TabIndex = 81;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Şifre Değiştir";
            // 
            // textBox13
            // 
            this.textBox13.AccessibleName = "";
            this.textBox13.BackColor = System.Drawing.SystemColors.Window;
            this.textBox13.Location = new System.Drawing.Point(6, 29);
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new System.Drawing.Size(122, 26);
            this.textBox13.TabIndex = 35;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.LightBlue;
            this.button3.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button3.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.button3.Location = new System.Drawing.Point(134, 26);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(91, 32);
            this.button3.TabIndex = 27;
            this.button3.Text = "Değiştir";
            this.button3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(656, 636);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 20);
            this.label1.TabIndex = 82;
            this.label1.Text = "HydroFilter durum:";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label28.Location = new System.Drawing.Point(797, 636);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(0, 20);
            this.label28.TabIndex = 83;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.Location = new System.Drawing.Point(656, 659);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(116, 20);
            this.label29.TabIndex = 84;
            this.label29.Text = "Bağlantı Sayısı:";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.Location = new System.Drawing.Point(797, 660);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(60, 20);
            this.label30.TabIndex = 85;
            this.label30.Text = "label30";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.Location = new System.Drawing.Point(660, 688);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(123, 20);
            this.label32.TabIndex = 86;
            this.label32.Text = "Bloklama Sayısı:";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.Location = new System.Drawing.Point(797, 688);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(60, 20);
            this.label33.TabIndex = 87;
            this.label33.Text = "label33";
            // 
            // ServerManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.WindowText;
            this.ClientSize = new System.Drawing.Size(1027, 730);
            this.Controls.Add(this.label33);
            this.Controls.Add(this.label32);
            this.Controls.Add(this.label30);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.comboBox4);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.button17);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.button15);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button16);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.onlineTxt);
            this.Controls.Add(this.lixtBox1);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox3);
            this.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ServerManagementForm";
            this.ShowIcon = false;
            this.Text = "GunnyTurkey";
            this.Load += new System.EventHandler(this.ServerManagementForm_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
	}
}
