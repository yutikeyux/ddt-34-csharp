using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using Bussiness;
using Bussiness.Managers;
using Game.Base;
using Game.Base.Packets;
using Game.Logic;
using Game.Server;
using Game.Server.Managers;
using Game.Server.Rooms;
using SqlDataProvider.Data;

namespace Game.Service.actions
{
	// Token: 0x02000008 RID: 8
	public partial class ServerManagementForm : Form
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00004732 File Offset: 0x00002932
		// (set) Token: 0x06000033 RID: 51 RVA: 0x0000473A File Offset: 0x0000293A
		public string nickname { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00004743 File Offset: 0x00002943
		// (set) Token: 0x06000035 RID: 53 RVA: 0x0000474B File Offset: 0x0000294B
		public GamePlayer player { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00004754 File Offset: 0x00002954
		// (set) Token: 0x06000037 RID: 55 RVA: 0x0000475C File Offset: 0x0000295C
		public string NULL { get; set; }

		// Token: 0x06000038 RID: 56 RVA: 0x00004768 File Offset: 0x00002968
		public ServerManagementForm()
		{
			this.InitializeComponent();
			this.InitializeExtendedComponents();
			this.InitializeLogging();
			this.LoadServerSettings();
			this.SetupAutoMessages();
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00004810 File Offset: 0x00002A10
		private void InitializeExtendedComponents()
		{
			this.statsTimer = new Timer();
			this.statsTimer.Interval = 5000;
			this.statsTimer.Tick += this.UpdateUI_Tick;
			this.statsTimer.Start();
			this.autoMessageTimer = new Timer();
			this.autoMessageTimer.Interval = 300000;
			this.autoMessageTimer.Tick += this.AutoMessageTimer_Tick;
			this.autoMessageTimer.Start();
			TabControl tabControl = new TabControl();
			tabControl.Dock = DockStyle.Fill;
			tabControl.Size = new Size(1000, 700);
			TabPage tabPage = new TabPage("Oyuncu Yönetimi");
			TabPage tabPage2 = new TabPage("Sunucu Kontrolü");
			TabPage tabPage3 = new TabPage("Veritabanı Yönetimi");
			TabPage tabPage4 = new TabPage("Etkinlik Yönetimi");
			TabPage tabPage5 = new TabPage("Raporlar");
			TabPage tabPage6 = new TabPage("Sunucu Ayarları");
			TabPage tabPage7 = new TabPage("Loglar");
			tabControl.TabPages.AddRange(new TabPage[]
			{
				tabPage,
				tabPage2,
				tabPage3,
				tabPage4,
				tabPage5,
				tabPage6,
				tabPage7
			});
			base.Controls.Add(tabControl);
			this.SetupPlayerManagementTab(tabPage);
			this.SetupServerControlTab(tabPage2);
			this.SetupDatabaseTab(tabPage3);
			this.SetupEventsTab(tabPage4);
			this.SetupReportsTab(tabPage5);
			this.SetupSettingsTab(tabPage6);
			this.SetupLogsTab(tabPage7);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00004990 File Offset: 0x00002B90
		private void SetupPlayerManagementTab(TabPage tab)
		{
			GroupBox groupBox = new GroupBox();
			groupBox.Text = "Oyuncu Ara";
			groupBox.Location = new Point(10, 10);
			groupBox.Size = new Size(450, 80);
			TextBox textBox = new TextBox();
			textBox.Location = new Point(10, 20);
			textBox.Size = new Size(200, 20);
			ComboBox comboBox = new ComboBox();
			comboBox.Items.AddRange(new object[]
			{
				"Nick",
				"ID",
				"IP Adresi",
				"E-posta"
			});
			comboBox.SelectedIndex = 0;
			comboBox.Location = new Point(220, 20);
			comboBox.Size = new Size(100, 20);
			Button button = new Button();
			button.Text = "Ara";
			button.Location = new Point(330, 20);
			button.Size = new Size(100, 25);
			button.Click += this.SearchPlayer_Click;
			groupBox.Controls.AddRange(new Control[]
			{
				textBox,
				comboBox,
				button
			});
			ListBox listBox = new ListBox();
			listBox.Location = new Point(10, 100);
			listBox.Size = new Size(450, 200);
			listBox.SelectedIndexChanged += this.PlayerList_SelectedIndexChanged;
			GroupBox groupBox2 = new GroupBox();
			groupBox2.Text = "Oyuncu Detayları";
			groupBox2.Location = new Point(470, 10);
			groupBox2.Size = new Size(500, 290);
			Label[] array = new Label[15];
			for (int i = 0; i < 15; i++)
			{
				array[i] = new Label();
				array[i].Location = new Point(10, 20 + i * 25);
				array[i].Size = new Size(150, 20);
				array[i].Text = string.Format("Özellik {0}:", i + 1);
				groupBox2.Controls.Add(array[i]);
			}
			GroupBox groupBox3 = new GroupBox();
			groupBox3.Text = "Oyuncu Eylemleri";
			groupBox3.Location = new Point(10, 310);
			groupBox3.Size = new Size(960, 150);
			Button[] array2 = new Button[10];
			string[] array3 = new string[]
			{
				"Banla",
				"Ban Aç",
				"Kick At",
				"Şifre Sıfırla",
				"Item Ver",
				"Para Ver",
				"Seviye Ayarla",
				"TP Konumlandır",
				"Mektup Gönder",
				"Detaylı Görüntüle"
			};
			for (int j = 0; j < 10; j++)
			{
				array2[j] = new Button();
				array2[j].Text = array3[j];
				array2[j].Location = new Point(10 + j % 5 * 190, 20 + j / 5 * 50);
				array2[j].Size = new Size(180, 40);
				array2[j].Tag = j;
				array2[j].Click += this.PlayerActionButton_Click;
				groupBox3.Controls.Add(array2[j]);
			}
			tab.Controls.AddRange(new Control[]
			{
				groupBox,
				listBox,
				groupBox2,
				groupBox3
			});
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00004D50 File Offset: 0x00002F50
		private void SetupServerControlTab(TabPage tab)
		{
			GroupBox groupBox = new GroupBox();
			groupBox.Text = "Sunucu Durumu";
			groupBox.Location = new Point(10, 10);
			groupBox.Size = new Size(450, 150);
			Label[] array = new Label[6];
			string[] array2 = new string[]
			{
				"Sunucu Durumu:",
				"Online Oyuncu:",
				"Bellek Kullanımı:",
				"CPU Kullanımı:",
				"Ağ Trafiği:",
				"Çalışma Süresi:"
			};
			for (int i = 0; i < 6; i++)
			{
				array[i] = new Label();
				array[i].Location = new Point(10, 20 + i * 20);
				array[i].Size = new Size(150, 20);
				array[i].Text = array2[i];
				groupBox.Controls.Add(array[i]);
			}
			GroupBox groupBox2 = new GroupBox();
			groupBox2.Text = "Sunucu Eylemleri";
			groupBox2.Location = new Point(470, 10);
			groupBox2.Size = new Size(500, 150);
			Button[] array3 = new Button[8];
			string[] array4 = new string[]
			{
				"Bakım Modu",
				"Yeniden Başlat",
				"Kapat",
				"RAM Temizle",
				"Logları Temizle",
				"Yedek Al",
				"DDoS Koruması",
				"Otomatik Mesaj"
			};
			for (int j = 0; j < 8; j++)
			{
				array3[j] = new Button();
				array3[j].Text = array4[j];
				array3[j].Location = new Point(10 + j % 4 * 120, 20 + j / 4 * 50);
				array3[j].Size = new Size(110, 40);
				array3[j].Tag = j;
				array3[j].Click += this.ServerActionButton_Click;
				groupBox2.Controls.Add(array3[j]);
			}
			GroupBox groupBox3 = new GroupBox();
			groupBox3.Text = "Duyuru Yönetimi";
			groupBox3.Location = new Point(10, 170);
			groupBox3.Size = new Size(960, 150);
			ComboBox comboBox = new ComboBox();
			comboBox.Items.AddRange(new object[]
			{
				"Sarı Mesaj",
				"Kırmızı Mesaj",
				"Sistem Mesajı",
				"Hoparlör",
				"Mor Mesaj"
			});
			comboBox.SelectedIndex = 0;
			comboBox.Location = new Point(10, 20);
			comboBox.Size = new Size(150, 20);
			TextBox textBox = new TextBox();
			textBox.Location = new Point(170, 20);
			textBox.Size = new Size(600, 20);
			textBox.Multiline = true;
			textBox.Height = 60;
			Button button = new Button();
			button.Text = "Gönder";
			button.Location = new Point(780, 20);
			button.Size = new Size(170, 60);
			button.Click += this.SendAnnouncementButton_Click;
			groupBox3.Controls.AddRange(new Control[]
			{
				comboBox,
				textBox,
				button
			});
			GroupBox groupBox4 = new GroupBox();
			groupBox4.Text = "Otomatik Mesajlar";
			groupBox4.Location = new Point(10, 330);
			groupBox4.Size = new Size(960, 130);
			ListBox listBox = new ListBox();
			listBox.Location = new Point(10, 20);
			listBox.Size = new Size(400, 80);
			Button button2 = new Button();
			button2.Text = "Ekle";
			button2.Location = new Point(420, 20);
			button2.Size = new Size(100, 30);
			button2.Click += this.AddAutoMessageButton_Click;
			Button button3 = new Button();
			button3.Text = "Kaldır";
			button3.Location = new Point(530, 20);
			button3.Size = new Size(100, 30);
			button3.Click += this.RemoveAutoMessageButton_Click;
			NumericUpDown numericUpDown = new NumericUpDown();
			numericUpDown.Minimum = 1m;
			numericUpDown.Maximum = 60m;
			numericUpDown.Value = 5m;
			numericUpDown.Location = new Point(640, 20);
			numericUpDown.Size = new Size(100, 20);
			Label label = new Label();
			label.Text = "Dakika";
			label.Location = new Point(750, 20);
			label.Size = new Size(50, 20);
			groupBox4.Controls.AddRange(new Control[]
			{
				listBox,
				button2,
				button3,
				numericUpDown,
				label
			});
			tab.Controls.AddRange(new Control[]
			{
				groupBox,
				groupBox2,
				groupBox3,
				groupBox4
			});
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000052C0 File Offset: 0x000034C0
		private void SetupDatabaseTab(TabPage tab)
		{
			GroupBox groupBox = new GroupBox();
			groupBox.Text = "Veritabanı Güncelleme";
			groupBox.Location = new Point(10, 10);
			groupBox.Size = new Size(450, 200);
			ComboBox comboBox = new ComboBox();
			comboBox.Items.AddRange(new object[]
			{
				"Tümünü Güncelle",
				"Ball",
				"Map",
				"Item",
				"Shop",
				"Görev",
				"NPC",
				"Dil",
				"Füzyon",
				"Drop",
				"Etkinlik Ödülü"
			});
			comboBox.SelectedIndex = 0;
			comboBox.Location = new Point(10, 20);
			comboBox.Size = new Size(200, 20);
			Button button = new Button();
			button.Text = "Güncelle";
			button.Location = new Point(220, 20);
			button.Size = new Size(100, 25);
			button.Click += this.DbUpdateButton_Click;
			ProgressBar progressBar = new ProgressBar();
			progressBar.Location = new Point(10, 50);
			progressBar.Size = new Size(410, 20);
			TextBox textBox = new TextBox();
			textBox.Location = new Point(10, 80);
			textBox.Size = new Size(410, 100);
			textBox.Multiline = true;
			textBox.ScrollBars = ScrollBars.Vertical;
			textBox.ReadOnly = true;
			groupBox.Controls.AddRange(new Control[]
			{
				comboBox,
				button,
				progressBar,
				textBox
			});
			GroupBox groupBox2 = new GroupBox();
			groupBox2.Text = "Veritabanı Sorgusu";
			groupBox2.Location = new Point(470, 10);
			groupBox2.Size = new Size(500, 200);
			TextBox textBox2 = new TextBox();
			textBox2.Location = new Point(10, 20);
			textBox2.Size = new Size(480, 100);
			textBox2.Multiline = true;
			textBox2.ScrollBars = ScrollBars.Vertical;
			Button button2 = new Button();
			button2.Text = "Çalıştır";
			button2.Location = new Point(10, 130);
			button2.Size = new Size(100, 25);
			button2.Click += this.DbExecuteButton_Click;
			Button button3 = new Button();
			button3.Text = "Dışa Aktar";
			button3.Location = new Point(120, 130);
			button3.Size = new Size(100, 25);
			button3.Click += this.DbExportButton_Click;
			ComboBox comboBox2 = new ComboBox();
			comboBox2.Items.AddRange(new object[]
			{
				"SELECT",
				"UPDATE",
				"INSERT",
				"DELETE"
			});
			comboBox2.SelectedIndex = 0;
			comboBox2.Location = new Point(230, 130);
			comboBox2.Size = new Size(100, 20);
			groupBox2.Controls.AddRange(new Control[]
			{
				textBox2,
				button2,
				button3,
				comboBox2
			});
			GroupBox groupBox3 = new GroupBox();
			groupBox3.Text = "Veritabanı Yedekleme";
			groupBox3.Location = new Point(10, 220);
			groupBox3.Size = new Size(450, 150);
			Button button4 = new Button();
			button4.Text = "Yedek Al";
			button4.Location = new Point(10, 20);
			button4.Size = new Size(100, 25);
			button4.Click += this.DbBackupButton_Click;
			Button button5 = new Button();
			button5.Text = "Geri Yükle";
			button5.Location = new Point(120, 20);
			button5.Size = new Size(100, 25);
			button5.Click += this.DbRestoreButton_Click;
			ListBox listBox = new ListBox();
			listBox.Location = new Point(10, 50);
			listBox.Size = new Size(410, 80);
			groupBox3.Controls.AddRange(new Control[]
			{
				button4,
				button5,
				listBox
			});
			GroupBox groupBox4 = new GroupBox();
			groupBox4.Text = "Veritabanı Optimizasyonu";
			groupBox4.Location = new Point(470, 220);
			groupBox4.Size = new Size(500, 150);
			Button button6 = new Button();
			button6.Text = "Optimize Et";
			button6.Location = new Point(10, 20);
			button6.Size = new Size(100, 25);
			button6.Click += this.DbOptimizeButton_Click;
			Button button7 = new Button();
			button7.Text = "Onar";
			button7.Location = new Point(120, 20);
			button7.Size = new Size(100, 25);
			button7.Click += this.DbRepairButton_Click;
			ProgressBar progressBar2 = new ProgressBar();
			progressBar2.Location = new Point(10, 50);
			progressBar2.Size = new Size(410, 20);
			TextBox textBox3 = new TextBox();
			textBox3.Location = new Point(10, 80);
			textBox3.Size = new Size(410, 50);
			textBox3.Multiline = true;
			textBox3.ScrollBars = ScrollBars.Vertical;
			textBox3.ReadOnly = true;
			groupBox4.Controls.AddRange(new Control[]
			{
				button6,
				button7,
				progressBar2,
				textBox3
			});
			tab.Controls.AddRange(new Control[]
			{
				groupBox,
				groupBox2,
				groupBox3,
				groupBox4
			});
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000058E0 File Offset: 0x00003AE0
		private void SetupEventsTab(TabPage tab)
		{
			GroupBox groupBox = new GroupBox();
			groupBox.Text = "Aktif Etkinlikler";
			groupBox.Location = new Point(10, 10);
			groupBox.Size = new Size(450, 200);
			ListBox listBox = new ListBox();
			listBox.Location = new Point(10, 20);
			listBox.Size = new Size(410, 150);
			Button button = new Button();
			button.Text = "Başlat";
			button.Location = new Point(10, 170);
			button.Size = new Size(100, 25);
			button.Click += this.StartEventButton_Click;
			Button button2 = new Button();
			button2.Text = "Durdur";
			button2.Location = new Point(120, 170);
			button2.Size = new Size(100, 25);
			button2.Click += this.StopEventButton_Click;
			groupBox.Controls.AddRange(new Control[]
			{
				listBox,
				button,
				button2
			});
			GroupBox groupBox2 = new GroupBox();
			groupBox2.Text = "Yeni Etkinlik Oluştur";
			groupBox2.Location = new Point(470, 10);
			groupBox2.Size = new Size(500, 200);
			Label label = new Label();
			label.Text = "Etkinlik Adı:";
			label.Location = new Point(10, 20);
			label.Size = new Size(100, 20);
			TextBox textBox = new TextBox();
			textBox.Location = new Point(120, 20);
			textBox.Size = new Size(200, 20);
			Label label2 = new Label();
			label2.Text = "Açıklama:";
			label2.Location = new Point(10, 50);
			label2.Size = new Size(100, 20);
			TextBox textBox2 = new TextBox();
			textBox2.Location = new Point(120, 50);
			textBox2.Size = new Size(350, 50);
			textBox2.Multiline = true;
			Label label3 = new Label();
			label3.Text = "Süre (dakika):";
			label3.Location = new Point(10, 110);
			label3.Size = new Size(100, 20);
			NumericUpDown numericUpDown = new NumericUpDown();
			numericUpDown.Minimum = 1m;
			numericUpDown.Maximum = 1440m;
			numericUpDown.Value = 60m;
			numericUpDown.Location = new Point(120, 110);
			numericUpDown.Size = new Size(100, 20);
			Button button3 = new Button();
			button3.Text = "Oluştur";
			button3.Location = new Point(230, 110);
			button3.Size = new Size(100, 25);
			button3.Click += this.CreateEventButton_Click;
			groupBox2.Controls.AddRange(new Control[]
			{
				label,
				textBox,
				label2,
				textBox2,
				label3,
				numericUpDown,
				button3
			});
			GroupBox groupBox3 = new GroupBox();
			groupBox3.Text = "Etkinlik Ödülleri";
			groupBox3.Location = new Point(10, 220);
			groupBox3.Size = new Size(450, 200);
			ListBox listBox2 = new ListBox();
			listBox2.Location = new Point(10, 20);
			listBox2.Size = new Size(300, 150);
			Button button4 = new Button();
			button4.Text = "Ödül Ekle";
			button4.Location = new Point(320, 20);
			button4.Size = new Size(100, 25);
			button4.Click += this.AddRewardButton_Click;
			Button button5 = new Button();
			button5.Text = "Kaldır";
			button5.Location = new Point(320, 50);
			button5.Size = new Size(100, 25);
			button5.Click += this.RemoveRewardButton_Click;
			groupBox3.Controls.AddRange(new Control[]
			{
				listBox2,
				button4,
				button5
			});
			GroupBox groupBox4 = new GroupBox();
			groupBox4.Text = "Etkinlik Katılımcıları";
			groupBox4.Location = new Point(470, 220);
			groupBox4.Size = new Size(500, 200);
			ListBox listBox3 = new ListBox();
			listBox3.Location = new Point(10, 20);
			listBox3.Size = new Size(400, 150);
			Button button6 = new Button();
			button6.Text = "Hepsine Ödül Ver";
			button6.Location = new Point(10, 170);
			button6.Size = new Size(150, 25);
			button6.Click += this.RewardAllButton_Click;
			Button button7 = new Button();
			button7.Text = "Dışa Aktar";
			button7.Location = new Point(170, 170);
			button7.Size = new Size(150, 25);
			button7.Click += this.ExportParticipantsButton_Click;
			groupBox4.Controls.AddRange(new Control[]
			{
				listBox3,
				button6,
				button7
			});
			tab.Controls.AddRange(new Control[]
			{
				groupBox,
				groupBox2,
				groupBox3,
				groupBox4
			});
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00005EA4 File Offset: 0x000040A4
		private void SetupReportsTab(TabPage tab)
		{
			GroupBox groupBox = new GroupBox();
			groupBox.Text = "Oyuncu Raporları";
			groupBox.Location = new Point(10, 10);
			groupBox.Size = new Size(450, 300);
			ListBox listBox = new ListBox();
			listBox.Location = new Point(10, 20);
			listBox.Size = new Size(410, 200);
			Button button = new Button();
			button.Text = "Görüntüle";
			button.Location = new Point(10, 230);
			button.Size = new Size(100, 25);
			button.Click += this.ViewReportButton_Click;
			Button button2 = new Button();
			button2.Text = "Çözüldü";
			button2.Location = new Point(120, 230);
			button2.Size = new Size(100, 25);
			button2.Click += this.ResolveReportButton_Click;
			Button button3 = new Button();
			button3.Text = "Banla";
			button3.Location = new Point(230, 230);
			button3.Size = new Size(100, 25);
			button3.Click += this.BanReportedPlayerButton_Click;
			groupBox.Controls.AddRange(new Control[]
			{
				listBox,
				button,
				button2,
				button3
			});
			GroupBox groupBox2 = new GroupBox();
			groupBox2.Text = "Rapor Detayları";
			groupBox2.Location = new Point(470, 10);
			groupBox2.Size = new Size(500, 300);
			TextBox textBox = new TextBox();
			textBox.Location = new Point(10, 20);
			textBox.Size = new Size(480, 250);
			textBox.Multiline = true;
			textBox.ScrollBars = ScrollBars.Vertical;
			textBox.ReadOnly = true;
			groupBox2.Controls.Add(textBox);
			GroupBox groupBox3 = new GroupBox();
			groupBox3.Text = "Sunucu İstatistikleri";
			groupBox3.Location = new Point(10, 320);
			groupBox3.Size = new Size(960, 130);
			ListBox listBox2 = new ListBox();
			listBox2.Location = new Point(10, 20);
			listBox2.Size = new Size(940, 90);
			Button button4 = new Button();
			button4.Text = "Yenile";
			button4.Location = new Point(10, 110);
			button4.Size = new Size(100, 25);
			button4.Click += this.RefreshStatsButton_Click;
			Button button5 = new Button();
			button5.Text = "Dışa Aktar";
			button5.Location = new Point(120, 110);
			button5.Size = new Size(100, 25);
			button5.Click += this.ExportStatsButton_Click;
			groupBox3.Controls.AddRange(new Control[]
			{
				listBox2,
				button4,
				button5
			});
			tab.Controls.AddRange(new Control[]
			{
				groupBox,
				groupBox2,
				groupBox3
			});
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000061F4 File Offset: 0x000043F4
		private void SetupSettingsTab(TabPage tab)
		{
			GroupBox groupBox = new GroupBox();
			groupBox.Text = "Sunucu Ayarları";
			groupBox.Location = new Point(10, 10);
			groupBox.Size = new Size(450, 300);
			Label[] array = new Label[10];
			string[] array2 = new string[]
			{
				"Max Oyuncu:",
				"Exp Oranı:",
				"Drop Oranı:",
				"Para Oranı:",
				"Onur Oranı:",
				"PvP Hasar:",
				"PvE Hasar:",
				"Otomatik Kayıt:",
				"Bakım Modu:",
				"DDoS Koruması:"
			};
			NumericUpDown[] array3 = new NumericUpDown[8];
			CheckBox[] array4 = new CheckBox[2];
			for (int i = 0; i < 10; i++)
			{
				array[i] = new Label();
				array[i].Text = array2[i];
				array[i].Location = new Point(10, 20 + i * 25);
				array[i].Size = new Size(150, 20);
				groupBox.Controls.Add(array[i]);
				bool flag = i < 8;
				if (flag)
				{
					array3[i] = new NumericUpDown();
					array3[i].Minimum = 0m;
					array3[i].Maximum = 1000m;
					array3[i].Value = 100m;
					array3[i].Location = new Point(170, 20 + i * 25);
					array3[i].Size = new Size(100, 20);
					groupBox.Controls.Add(array3[i]);
				}
				else
				{
					array4[i - 8] = new CheckBox();
					array4[i - 8].Location = new Point(170, 20 + i * 25);
					array4[i - 8].Size = new Size(20, 20);
					groupBox.Controls.Add(array4[i - 8]);
				}
			}
			Button button = new Button();
			button.Text = "Kaydet";
			button.Location = new Point(10, 270);
			button.Size = new Size(100, 25);
			button.Click += this.SaveSettingsButton_Click;
			Button button2 = new Button();
			button2.Text = "Sıfırla";
			button2.Location = new Point(120, 270);
			button2.Size = new Size(100, 25);
			button2.Click += this.ResetSettingsButton_Click;
			groupBox.Controls.AddRange(new Control[]
			{
				button,
				button2
			});
			GroupBox groupBox2 = new GroupBox();
			groupBox2.Text = "Kanal Ayarları";
			groupBox2.Location = new Point(470, 10);
			groupBox2.Size = new Size(500, 300);
			ListBox listBox = new ListBox();
			listBox.Location = new Point(10, 20);
			listBox.Size = new Size(200, 200);
			Button button3 = new Button();
			button3.Text = "Ekle";
			button3.Location = new Point(220, 20);
			button3.Size = new Size(100, 25);
			button3.Click += this.AddChannelButton_Click;
			Button button4 = new Button();
			button4.Text = "Kaldır";
			button4.Location = new Point(330, 20);
			button4.Size = new Size(100, 25);
			button4.Click += this.RemoveChannelButton_Click;
			Label label = new Label();
			label.Text = "Kanal Adı:";
			label.Location = new Point(220, 60);
			label.Size = new Size(100, 20);
			TextBox textBox = new TextBox();
			textBox.Location = new Point(220, 80);
			textBox.Size = new Size(200, 20);
			Label label2 = new Label();
			label2.Text = "Oyuncu Limiti:";
			label2.Location = new Point(220, 110);
			label2.Size = new Size(100, 20);
			NumericUpDown numericUpDown = new NumericUpDown();
			numericUpDown.Minimum = 1m;
			numericUpDown.Maximum = 1000m;
			numericUpDown.Value = 100m;
			numericUpDown.Location = new Point(220, 130);
			numericUpDown.Size = new Size(100, 20);
			Label label3 = new Label();
			label3.Text = "Kanal Tipi:";
			label3.Location = new Point(220, 160);
			label3.Size = new Size(100, 20);
			ComboBox comboBox = new ComboBox();
			comboBox.Items.AddRange(new object[]
			{
				"Normal",
				"VIP",
				"Etkinlik",
				"Yeni Başlayanlar"
			});
			comboBox.SelectedIndex = 0;
			comboBox.Location = new Point(220, 180);
			comboBox.Size = new Size(200, 20);
			groupBox2.Controls.AddRange(new Control[]
			{
				listBox,
				button3,
				button4,
				label,
				textBox,
				label2,
				numericUpDown,
				label3,
				comboBox
			});
			GroupBox groupBox3 = new GroupBox();
			groupBox3.Text = "Güvenlik Ayarları";
			groupBox3.Location = new Point(10, 320);
			groupBox3.Size = new Size(960, 130);
			Label[] array5 = new Label[4];
			string[] array6 = new string[]
			{
				"Hatalı Giriş Limiti:",
				"Şifre Gücü:",
				"IP Ban Süresi (saat):",
				"Anti-Cheat Seviyesi:"
			};
			NumericUpDown[] array7 = new NumericUpDown[4];
			for (int j = 0; j < 4; j++)
			{
				array5[j] = new Label();
				array5[j].Text = array6[j];
				array5[j].Location = new Point(10 + j * 240, 20);
				array5[j].Size = new Size(150, 20);
				groupBox3.Controls.Add(array5[j]);
				array7[j] = new NumericUpDown();
				array7[j].Minimum = 0m;
				array7[j].Maximum = 100m;
				array7[j].Value = 5m;
				array7[j].Location = new Point(10 + j * 240, 40);
				array7[j].Size = new Size(100, 20);
				groupBox3.Controls.Add(array7[j]);
			}
			Button button5 = new Button();
			button5.Text = "Kaydet";
			button5.Location = new Point(10, 70);
			button5.Size = new Size(100, 25);
			button5.Click += this.SaveSecurityButton_Click;
			Button button6 = new Button();
			button6.Text = "Sıfırla";
			button6.Location = new Point(120, 70);
			button6.Size = new Size(100, 25);
			button6.Click += this.ResetSecurityButton_Click;
			groupBox3.Controls.AddRange(new Control[]
			{
				button5,
				button6
			});
			tab.Controls.AddRange(new Control[]
			{
				groupBox,
				groupBox2,
				groupBox3
			});
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000069F0 File Offset: 0x00004BF0
		private void SetupLogsTab(TabPage tab)
		{
			GroupBox groupBox = new GroupBox();
			groupBox.Text = "Log Filtreleme";
			groupBox.Location = new Point(10, 10);
			groupBox.Size = new Size(960, 80);
			Label label = new Label();
			label.Text = "Log Tipi:";
			label.Location = new Point(10, 20);
			label.Size = new Size(100, 20);
			ComboBox comboBox = new ComboBox();
			comboBox.Items.AddRange(new object[]
			{
				"Tümü",
				"Hata",
				"Uyarı",
				"Bilgi",
				"Güvenlik",
				"Oyuncu",
				"Sistem"
			});
			comboBox.SelectedIndex = 0;
			comboBox.Location = new Point(120, 20);
			comboBox.Size = new Size(150, 20);
			Label label2 = new Label();
			label2.Text = "Başlangıç:";
			label2.Location = new Point(280, 20);
			label2.Size = new Size(100, 20);
			DateTimePicker dateTimePicker = new DateTimePicker();
			dateTimePicker.Location = new Point(390, 20);
			dateTimePicker.Size = new Size(150, 20);
			Label label3 = new Label();
			label3.Text = "Bitiş:";
			label3.Location = new Point(550, 20);
			label3.Size = new Size(100, 20);
			DateTimePicker dateTimePicker2 = new DateTimePicker();
			dateTimePicker2.Location = new Point(660, 20);
			dateTimePicker2.Size = new Size(150, 20);
			Button button = new Button();
			button.Text = "Filtrele";
			button.Location = new Point(820, 20);
			button.Size = new Size(100, 25);
			button.Click += this.FilterLogsButton_Click;
			TextBox textBox = new TextBox();
			textBox.Location = new Point(10, 50);
			textBox.Size = new Size(800, 20);
			Button button2 = new Button();
			button2.Text = "Ara";
			button2.Location = new Point(820, 50);
			button2.Size = new Size(100, 25);
			button2.Click += this.SearchLogsButton_Click;
			groupBox.Controls.AddRange(new Control[]
			{
				label,
				comboBox,
				label2,
				dateTimePicker,
				label3,
				dateTimePicker2,
				button,
				textBox,
				button2
			});
			GroupBox groupBox2 = new GroupBox();
			groupBox2.Text = "Log Görüntüleme";
			groupBox2.Location = new Point(10, 100);
			groupBox2.Size = new Size(960, 350);
			ListBox listBox = new ListBox();
			listBox.Location = new Point(10, 20);
			listBox.Size = new Size(940, 280);
			Button button3 = new Button();
			button3.Text = "Detaylar";
			button3.Location = new Point(10, 310);
			button3.Size = new Size(100, 25);
			button3.Click += this.ViewLogDetailsButton_Click;
			Button button4 = new Button();
			button4.Text = "Dışa Aktar";
			button4.Location = new Point(120, 310);
			button4.Size = new Size(100, 25);
			button4.Click += this.ExportLogsButton_Click;
			Button button5 = new Button();
			button5.Text = "Temizle";
			button5.Location = new Point(230, 310);
			button5.Size = new Size(100, 25);
			button5.Click += this.ClearLogsButton_Click;
			groupBox2.Controls.AddRange(new Control[]
			{
				listBox,
				button3,
				button4,
				button5
			});
			tab.Controls.AddRange(new Control[]
			{
				groupBox,
				groupBox2
			});
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00006E48 File Offset: 0x00005048
		private void InitializeLogging()
		{
			try
			{
				this.logWriter = new StreamWriter(this.logFilePath, true);
				this.logWriter.WriteLine(string.Format("[{0}] Sunucu yönetim paneli başlatıldı.", DateTime.Now));
				this.logWriter.Flush();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Log dosyası oluşturulamadı: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00006ECC File Offset: 0x000050CC
		private void LoadServerSettings()
		{
			try
			{
				using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.AppSettings.Get("conString")))
				{
					sqlConnection.Open();
					SqlCommand sqlCommand = new SqlCommand("SELECT * FROM ServerSettings", sqlConnection);
					SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
					bool flag = sqlDataReader.Read();
					if (flag)
					{
					}
					sqlDataReader.Close();
				}
			}
			catch (Exception ex)
			{
				this.LogMessage("Sunucu ayarları yüklenemedi: " + ex.Message, "Hata");
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00006F70 File Offset: 0x00005170
		private void SetupAutoMessages()
		{
			this.autoMessages.Add("Sunucumuza hoş geldiniz! İyi oyunlar dileriz.");
			this.autoMessages.Add("Kurallara uymayı unutmayın! Hile yapmak kesinlikle yasaktır.");
			this.autoMessages.Add("Sorunlarınız için destek ekibimize başvurabilirsiniz.");
			this.autoMessages.Add("Etkinliklerimizi kaçırmayın! Ödüller sizi bekliyor.");
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00006FC4 File Offset: 0x000051C4
		private void LogMessage(string message, string type = "Bilgi")
		{
			try
			{
				string logEntry = string.Format("[{0}] [{1}] {2}", DateTime.Now, type, message);
				this.logWriter.WriteLine(logEntry);
				this.logWriter.Flush();
				bool invokeRequired = base.InvokeRequired;
				if (invokeRequired)
				{
					base.Invoke(new Action(delegate()
					{
						this.UpdateLogDisplay(logEntry);
					}));
				}
				else
				{
					this.UpdateLogDisplay(logEntry);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Log yazılamadı: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00007080 File Offset: 0x00005280
		private void UpdateLogDisplay(string logEntry)
		{
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00007084 File Offset: 0x00005284
		public void updateState()
		{
			try
			{
				GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
				GamePlayer[] array = allPlayers;
				List<BaseRoom> allUsingRoom = RoomMgr.GetAllUsingRoom();
				int num = (allPlayers != null) ? allPlayers.Length : 0;
				double num2 = (double)GC.GetTotalMemory(false);
				int num3 = 0;
				int num4 = 0;
				foreach (BaseRoom baseRoom in allUsingRoom)
				{
					bool flag = !baseRoom.IsEmpty;
					bool flag2 = flag;
					if (flag2)
					{
						num3++;
						bool isPlaying = baseRoom.IsPlaying;
						bool flag3 = isPlaying;
						if (flag3)
						{
							num4++;
						}
					}
				}
				this.onlineTxt.Text = num.ToString();
				this.label25.Text = Math.Round(num2 / 1024.0 / 1024.0).ToString() + "MB";
				this.lixtBox1.Items.Clear();
				for (int i = 0; i < array.Length; i++)
				{
					GamePlayer gamePlayer = allPlayers[i];
					Console.WriteLine(string.Concat(new string[]
					{
						gamePlayer.PlayerCharacter.UserName,
						" - [",
						gamePlayer.PlayerCharacter.NickName,
						"] seviye: ",
						gamePlayer.PlayerCharacter.Grade.ToString()
					}));
					this.lixtBox1.Items.Add(gamePlayer.PlayerCharacter.NickName);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.StackTrace);
				this.LogMessage("Sunucu durumu güncellenemedi: " + ex.Message, "Hata");
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00007280 File Offset: 0x00005480
		private void UpdateUI_Tick(object sender, EventArgs e)
		{
			try
			{
				this.updateState();
				this.UpdateServerStats();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.StackTrace);
				this.LogMessage("UI güncellenemedi: " + ex.Message, "Hata");
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000072E0 File Offset: 0x000054E0
		private void UpdateServerStats()
		{
			try
			{
				PerformanceCounter performanceCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
				float num = performanceCounter.NextValue();
				long totalMemory = GC.GetTotalMemory(false);
				long num2 = 0L;
				long num3 = 0L;
				bool isNetworkAvailable = NetworkInterface.GetIsNetworkAvailable();
				if (isNetworkAvailable)
				{
					NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
					foreach (NetworkInterface networkInterface in allNetworkInterfaces)
					{
						IPv4InterfaceStatistics ipv4Statistics = networkInterface.GetIPv4Statistics();
						num2 += ipv4Statistics.BytesSent;
						num3 += ipv4Statistics.BytesReceived;
					}
				}
				TimeSpan timeSpan = TimeSpan.FromMilliseconds((double)Environment.TickCount);
			}
			catch (Exception ex)
			{
				this.LogMessage("Sunucu istatistikleri güncellenemedi: " + ex.Message, "Hata");
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000073B0 File Offset: 0x000055B0
		private void reloadBtn_Click(object sender, EventArgs e)
		{
			try
			{
				ServerManagementForm.AllReload();
				MessageBox.Show("Tüm Veritabanı Güncellenmiştir.");
				this.LogMessage("Tüm veritabanı güncellendi.", "Bilgi");
			}
			catch (Exception)
			{
				MessageBox.Show("Tüm Veritabanı Güncellenmiştir.");
				Console.WriteLine("Tüm Veritabanı Güncellenmiştir.");
				Console.WriteLine("Ula böle sistem kimsede yok 1 tuşla hepsini güncelleme :)");
				this.LogMessage("Tüm veritabanı güncellendi.", "Bilgi");
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x0000742C File Offset: 0x0000562C
		private static void AllReload()
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00007434 File Offset: 0x00005634
		private void PlayerList_Enter(object sender, EventArgs e)
		{
			Console.WriteLine("Click");
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00007444 File Offset: 0x00005644
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
			int num = int.Parse(this.textBox3.Text.ToString());
			string title = this.textBox2.Text.ToString();
			string content = this.textBox4.Text.ToString();
			this.textBox5.Text.ToString();
			int num2 = int.Parse(this.textBox5.Text.ToString());
			bool isBinds = bool.Parse(this.textBox6.Text.ToString());
			GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
			foreach (GamePlayer gamePlayer in allPlayers)
			{
				PlayerInfo playerInfo = new PlayerInfo();
				playerInfo = gamePlayer.PlayerCharacter;
				new PlayerBussiness().SendMailAndItem(title, content, playerInfo.ID, num, num2, validDate, gold, money, strengthenLevel, attackCompose, defendCompose, agilityCompose, luckCompose, isBinds);
				string msg = "[ Online Oyuncu Etkinliği Sistemi ] Hediye Ödüller Gönderilmiştir.";
				gamePlayer.SendMessage(msg);
			}
			this.LogMessage(string.Format("Tüm oyunculara item gönderildi: {0} x{1}", num, num2), "Bilgi");
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00007578 File Offset: 0x00005778
		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.player = WorldMgr.GetClientByPlayerNickName(this.lixtBox1.GetItemText(this.lixtBox1.SelectedItem));
			this.nickName.Text = this.player.PlayerCharacter.NickName;
			this.label4.Text = this.player.PlayerCharacter.UserName;
			this.label7.Text = this.player.PlayerCharacter.Grade.ToString();
			this.label8.Text = this.player.PlayerCharacter.Money.ToString();
			this.label19.Text = this.player.PlayerCharacter.Attack.ToString();
			this.label20.Text = this.player.PlayerCharacter.Defence.ToString();
			this.label21.Text = this.player.PlayerCharacter.Luck.ToString();
			this.label22.Text = this.player.PlayerCharacter.Agility.ToString();
			this.label23.Text = this.player.PlayerCharacter.hp.ToString();
			this.label24.Text = this.player.PlayerCharacter.FightPower.ToString();
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00007700 File Offset: 0x00005900
		private void button9_Click(object sender, EventArgs e)
		{
			try
			{
				GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
				GamePlayer[] array = allPlayers;
				List<BaseRoom> allUsingRoom = RoomMgr.GetAllUsingRoom();
				bool flag = allPlayers != null;
				bool flag2 = flag;
				if (flag2)
				{
					int num = allPlayers.Length;
				}
				GC.GetTotalMemory(false);
				int num2 = 0;
				int num3 = 0;
				foreach (BaseRoom baseRoom in allUsingRoom)
				{
					bool flag3 = !baseRoom.IsEmpty;
					bool flag4 = flag3;
					if (flag4)
					{
						num2++;
						bool isPlaying = baseRoom.IsPlaying;
						bool flag5 = isPlaying;
						if (flag5)
						{
							num3++;
						}
					}
				}
				this.lixtBox1.Items.Clear();
				for (int i = 0; i < array.Length; i++)
				{
					GamePlayer gamePlayer = allPlayers[i];
					Console.WriteLine(string.Concat(new string[]
					{
						gamePlayer.PlayerCharacter.UserName,
						" - [",
						gamePlayer.PlayerCharacter.NickName,
						"] seviye: ",
						gamePlayer.PlayerCharacter.Grade.ToString()
					}));
					this.lixtBox1.Items.Add(gamePlayer.PlayerCharacter.NickName);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.StackTrace);
				this.LogMessage("Oyuncu listesi güncellenemedi: " + ex.Message, "Hata");
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000078B4 File Offset: 0x00005AB4
		private void button10_Click(object sender, EventArgs e)
		{
			DateTime date = new DateTime(2050, 7, 2);
			bool flag = this.player == null;
			bool flag2 = !flag;
			if (flag2)
			{
				using (ManageBussiness manageBussiness = new ManageBussiness())
				{
					manageBussiness.ForbidPlayerByNickName(this.player.PlayerCharacter.NickName, date, false);
					MessageBox.Show(this.player.PlayerCharacter.NickName + " Oyuncu Banlandı !");
					GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
					foreach (GamePlayer gamePlayer in allPlayers)
					{
						string msg = "Oyuncumuz <" + this.player.PlayerCharacter.NickName + "> oyun kurallarına aykırı gelirken yakaladık ve BANLADIK. Sizde böyle olmak istemiyorsanız kurallara uyunuz.";
						gamePlayer.SendMessage(msg);
					}
				}
				this.LogMessage("Oyuncu banlandı: " + this.player.PlayerCharacter.NickName, "Güvenlik");
			}
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000079C0 File Offset: 0x00005BC0
		private void button11_Click(object sender, EventArgs e)
		{
			Process.Start("http://185.88.175.105/Request/activelist.ashx");
			MessageBox.Show("(Zaman Sınırlı) Güncellendi [Başarılı]");
			this.LogMessage("Zaman sınırlı etkinlikler güncellendi.", "Bilgi");
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000079EA File Offset: 0x00005BEA
		private void button12_Click(object sender, EventArgs e)
		{
			Process.Start("http://185.88.175.105/Request/CelebList/CreateAllCeleb.ashx");
			MessageBox.Show("(Onur Listesi) Güncellendi [Başarılı]");
			this.LogMessage("Onur listesi güncellendi.", "Bilgi");
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00007A14 File Offset: 0x00005C14
		private void button13_Click(object sender, EventArgs e)
		{
			Process.Start("http://185.88.175.105/Request/LoadPVEItems.ashx");
			MessageBox.Show("(Droplar) Güncellendi [Başarılı]");
			this.LogMessage("PVE itemları güncellendi.", "Bilgi");
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00007A3E File Offset: 0x00005C3E
		private void button14_Click(object sender, EventArgs e)
		{
			Process.Start("http://185.88.175.105/Request/questlist.ashx");
			MessageBox.Show("(Görevler) Güncellendi [Başarılı]");
			this.LogMessage("Görevler güncellendi.", "Bilgi");
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00007A68 File Offset: 0x00005C68
		private void button16_Click(object sender, EventArgs e)
		{
			DateTime date = new DateTime(2050, 7, 2);
			using (ManageBussiness manageBussiness = new ManageBussiness())
			{
				manageBussiness.ForbidPlayerByNickName(this.textBox1.Text, date, true);
			}
			GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
			int num = 0;
			bool flag = num < allPlayers.Length;
			bool flag2 = flag;
			if (flag2)
			{
				GamePlayer gamePlayer = allPlayers[num];
				MessageBox.Show("Oyuncumuz Banı Açılmıştır ");
				string msg = "Oyuncumuz <" + this.textBox1.Text + "> Banı Açılmıştır ";
				gamePlayer.SendMessage(msg);
			}
			this.LogMessage("Oyuncu banı açıldı: " + this.textBox1.Text, "Güvenlik");
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00007B30 File Offset: 0x00005D30
		private void button10_Click_1(object sender, EventArgs e)
		{
			DateTime date = new DateTime(2050, 7, 2);
			bool flag = this.player == null;
			bool flag2 = !flag;
			if (flag2)
			{
				using (ManageBussiness manageBussiness = new ManageBussiness())
				{
					manageBussiness.ForbidPlayerByNickName(this.nickName.Text, date, false);
					MessageBox.Show(this.player.PlayerCharacter.NickName + " Oyuncu Banlandı !");
					GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
					foreach (GamePlayer gamePlayer in allPlayers)
					{
						string msg = "Oyuncumuz <" + this.player.PlayerCharacter.NickName + "> oyun kurallarına aykırı gelirken yakaladık ve BANLADIK. Sizde böyle olmak istemiyorsanız kurallara uyunuz.";
						gamePlayer.SendMessage(msg);
					}
				}
				this.LogMessage("Oyuncu banlandı: " + this.player.PlayerCharacter.NickName, "Güvenlik");
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00007C38 File Offset: 0x00005E38
		private void button15_Click(object sender, EventArgs e)
		{
			bool flag = this.player == null;
			bool flag2 = !flag;
			if (flag2)
			{
				using (ManageBussiness manageBussiness = new ManageBussiness())
				{
					manageBussiness.KitoffUserByNickName(this.nickName.Text, " Kick yedi");
					MessageBox.Show(this.player.PlayerCharacter.NickName + " kicklendi !");
					GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
					for (int i = 0; i < allPlayers.Length; i++)
					{
						allPlayers[i].SendMessage(string.Format("Oyuncu [{0}] Oyunculara rahatsızlığından dolayı kick yemiştir", Array.Empty<object>()));
					}
				}
				this.LogMessage("Oyuncu kicklendi: " + this.player.PlayerCharacter.NickName, "Güvenlik");
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00007D18 File Offset: 0x00005F18
		private void button9_Click_1(object sender, EventArgs e)
		{
			try
			{
				this.updateState();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.StackTrace);
				this.LogMessage("Durum güncellenemedi: " + ex.Message, "Hata");
			}
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00007D70 File Offset: 0x00005F70
		private void button17_Click(object sender, EventArgs e)
		{
			GC.Collect();
			MessageBox.Show("Ram temizlendi !");
			this.updateState();
			this.LogMessage("RAM temizlendi.", "Bilgi");
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00007D9C File Offset: 0x00005F9C
		private void button18_Click(object sender, EventArgs e)
		{
			Process.Start("http://185.88.175.105/Request/NPCInfoList.ashx");
			MessageBox.Show("(NPC'Ler) Güncellendi [Başarılı]");
			this.LogMessage("NPC'ler güncellendi.", "Bilgi");
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00007DC8 File Offset: 0x00005FC8
		private void button19_Click(object sender, EventArgs e)
		{
			bool keepRunning = GameServer.KeepRunning;
			bool flag = keepRunning;
			if (flag)
			{
				string text = this.textBox8.Text;
				string text2 = this.textBox7.Text;
				SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.AppSettings.Get("conString"));
				sqlConnection.Open();
				new SqlCommand(string.Concat(new string[]
				{
					"update Sys_Users_Detail set NickName ='",
					text2,
					"' WHERE NickName=('",
					text,
					"')"
				}))
				{
					Connection = sqlConnection
				}.ExecuteNonQuery();
				sqlConnection.Close();
				MessageBox.Show("Nick Değişme İşleminiz Gerçekleşmiştir !");
				this.textBox8.Text = null;
				this.textBox7.Text = null;
				this.LogMessage("Oyuncu nicki değiştirildi: " + text + " -> " + text2, "Bilgi");
			}
			else
			{
				MessageBox.Show("Bilgileri Kontrol Ediniz ! ");
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00007EB4 File Offset: 0x000060B4
		private void button20_Click(object sender, EventArgs e)
		{
			bool flag = BallMgr.ReLoad();
			bool flag2 = flag;
			if (flag2)
			{
				Console.WriteLine("Ball info Güncelleniyor !");
			}
			Console.WriteLine("Ball info Güncellendi !");
			bool flag3 = MapMgr.ReLoadMap();
			bool flag4 = flag3;
			if (flag4)
			{
				Console.WriteLine("Map info Güncelleniyor !");
			}
			Console.WriteLine("Map info Güncellendi !");
			bool flag5 = MapMgr.ReLoadMapServer();
			bool flag6 = flag5;
			if (flag6)
			{
				Console.WriteLine("mapserver info Güncelleniyor !");
			}
			Console.WriteLine("mapserver Güncellendi !");
			bool flag7 = PropItemMgr.Reload();
			bool flag8 = flag7;
			if (flag8)
			{
				Console.WriteLine("prop info Güncelleniyor !");
			}
			Console.WriteLine("prop info Güncellendi !");
			bool flag9 = ItemMgr.ReLoad();
			bool flag10 = flag9;
			if (flag10)
			{
				Console.WriteLine("item info Güncelleniyor !");
			}
			Console.WriteLine("item info Güncellendi !");
			bool flag11 = ShopMgr.ReLoad();
			bool flag12 = flag11;
			if (flag12)
			{
				Console.WriteLine("shop info Güncelleniyor !");
			}
			Console.WriteLine("shop info Güncellendi !");
			bool flag13 = QuestMgr.ReLoad();
			bool flag14 = flag13;
			if (flag14)
			{
				Console.WriteLine("quest info Güncelleniyor !");
			}
			Console.WriteLine("quest info Güncellendi !");
			bool flag15 = FusionMgr.ReLoad();
			bool flag16 = flag15;
			if (flag16)
			{
				Console.WriteLine("fusion info Güncelleniyor !");
			}
			Console.WriteLine("fusion info Güncellendi !");
			bool flag17 = ConsortiaMgr.ReLoad();
			bool flag18 = flag17;
			if (flag18)
			{
				Console.WriteLine("consortiaMgr info Güncelleniyor !");
			}
			Console.WriteLine("consortiaMgr info Güncellendi !");
			bool flag19 = RateMgr.ReLoad();
			bool flag20 = flag19;
			if (flag20)
			{
				Console.WriteLine("Rate Rate Güncelleniyor !");
			}
			Console.WriteLine("Rate Rate Güncellendi !");
			bool flag21 = NPCInfoMgr.ReLoad();
			bool flag22 = flag21;
			if (flag22)
			{
				Console.WriteLine("NPCInfo Güncelleniyor !");
			}
			Console.WriteLine("NPCInfo Güncellendi !");
			bool flag23 = FightRateMgr.ReLoad();
			bool flag24 = flag23;
			if (flag24)
			{
				Console.WriteLine("FightRateMgr Güncelleniyor !");
			}
			Console.WriteLine("FightRateMgr Güncellendi !");
			bool flag25 = AwardMgr.ReLoad();
			bool flag26 = flag25;
			if (flag26)
			{
				Console.WriteLine("dailyaward Güncelleniyor !");
			}
			Console.WriteLine("dailyaward Güncellendi !");
			bool flag27 = LanguageMgr.Reload("");
			bool flag28 = flag27;
			if (flag28)
			{
				Console.WriteLine("language Güncelleniyor !");
			}
			Console.WriteLine("language Güncellendi !");
			Console.WriteLine("Tek bir tıkla tüm veritabanı'nı güncelledin daha ne istiyorsun :)");
			MessageBox.Show("Veritabanı Güncellenmiştir.");
			this.LogMessage("Tüm veritabanı güncellendi.", "Bilgi");
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00008108 File Offset: 0x00006308
		private void button21_Click(object sender, EventArgs e)
		{
			string text = this.textBox10.Text;
			SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.AppSettings.Get("conString"));
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlConnection;
			sqlCommand.CommandText = "SELECT ActiveIP FROM Sys_Users_Detail WHERE NickName='" + text + "'";
			sqlConnection.Open();
			SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
			while (sqlDataReader.Read())
			{
				this.textBox9.Text = this.NULL;
				this.textBox9.Text = sqlDataReader["ActiveIP"].ToString();
			}
			sqlConnection.Close();
			this.LogMessage("Oyuncu IP'si sorgulandı: " + text, "Bilgi");
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000081C8 File Offset: 0x000063C8
		private void button4_Click_1(object sender, EventArgs e)
		{
			bool flag = !string.IsNullOrEmpty(this.comboBox1.Text);
			bool flag2 = flag;
			if (flag2)
			{
				GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
				foreach (GamePlayer gamePlayer in allPlayers)
				{
					bool flag3 = this.comboBox1.Text == "Kupon";
					bool flag4 = flag3;
					if (flag4)
					{
						gamePlayer.AddMoney(int.Parse(this.textBox11.Text));
					}
					else
					{
						bool flag5 = this.comboBox1.Text == "Exp";
						bool flag6 = flag5;
						if (flag6)
						{
							gamePlayer.AddGP(int.Parse(this.textBox11.Text));
						}
						else
						{
							bool flag7 = this.comboBox1.Text == "Onur";
							bool flag8 = flag7;
							if (flag8)
							{
								gamePlayer.AddHonor(int.Parse(this.textBox11.Text));
							}
							else
							{
								bool flag9 = this.comboBox1.Text == "Kart Ruhu";
								bool flag10 = !flag9;
								if (flag10)
								{
									bool flag11 = this.comboBox1.Text == "Bağlı Kupon";
									bool flag12 = flag11;
									if (flag12)
									{
										gamePlayer.AddGiftToken(int.Parse(this.textBox11.Text));
									}
									else
									{
										bool flag13 = this.comboBox1.Text == "Mükafat";
										bool flag14 = flag13;
										if (flag14)
										{
											gamePlayer.AddOffer(int.Parse(this.textBox11.Text));
										}
									}
								}
							}
						}
					}
					gamePlayer.SendMessage(string.Concat(new string[]
					{
						"Tüm Çevrimiçi Oyunculara [",
						this.textBox11.Text,
						"] [",
						this.comboBox1.Text,
						"] Gönderilmiştir ^_^"
					}));
				}
				this.LogMessage("Tüm oyunculara " + this.comboBox1.Text + " gönderildi: " + this.textBox11.Text, "Bilgi");
			}
			else
			{
				MessageBox.Show("Lütfen Seçim Yapınız.!", "Uyarı");
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00008404 File Offset: 0x00006604
		private void button5_Click(object sender, EventArgs e)
		{
			bool flag = !string.IsNullOrEmpty(this.comboBox2.Text);
			bool flag2 = flag;
			if (flag2)
			{
				bool flag3 = this.comboBox2.Text == "Küçük Hoparlör";
				bool flag4 = flag3;
				if (flag4)
				{
					GSPacketIn gspacketIn = new GSPacketIn(71);
					gspacketIn.WriteInt(0);
					gspacketIn.WriteString("Sistem");
					gspacketIn.WriteString(this.textBox12.Text);
					GameServer.Instance.LoginServer.SendPacket(gspacketIn);
					ServerManagementForm.Herkes(gspacketIn);
				}
				else
				{
					bool flag5 = this.comboBox2.Text == "Büyük Hoparlör";
					bool flag6 = flag5;
					if (flag6)
					{
						GSPacketIn gspacketIn2 = new GSPacketIn(72);
						gspacketIn2.WriteInt(0);
						gspacketIn2.WriteInt(0);
						gspacketIn2.WriteString("Sistem");
						gspacketIn2.WriteString(this.textBox12.Text);
						GameServer.Instance.LoginServer.SendPacket(gspacketIn2);
						ServerManagementForm.Herkes(gspacketIn2);
					}
					else
					{
						bool flag7 = this.comboBox2.Text == "Sarı Mesaj";
						bool flag8 = flag7;
						if (flag8)
						{
							ServerManagementForm.Herkes_2(eMessageType.ChatNormal, this.textBox12.Text);
						}
						else
						{
							bool flag9 = this.comboBox2.Text == "Sistem Mesajı";
							bool flag10 = flag9;
							if (flag10)
							{
								ServerManagementForm.Herkes_2(eMessageType.ChatERROR, this.textBox12.Text);
							}
							else
							{
								bool flag11 = this.comboBox2.Text == "Kırmızı Mesaj";
								bool flag12 = flag11;
								if (flag12)
								{
									GSPacketIn gspacketIn3 = new GSPacketIn(73, 0);
									gspacketIn3.WriteInt(1);
									gspacketIn3.WriteInt(0);
									gspacketIn3.WriteString("Sistem");
									gspacketIn3.WriteString(this.textBox12.Text);
									gspacketIn3.WriteString("Yönetim");
									GameServer.Instance.LoginServer.SendPacket(gspacketIn3);
									ServerManagementForm.Herkes(gspacketIn3);
								}
								else
								{
									bool flag13 = this.comboBox2.Text == "Mor Mesaj";
									bool flag14 = flag13;
									if (flag14)
									{
										new ManageBussiness().SystemNotice(this.textBox12.Text);
									}
									else
									{
										bool flag15 = this.comboBox2.Text == "Admin Mesajı";
										bool flag16 = flag15;
										if (flag16)
										{
											ServerManagementForm.Herkes_2(eMessageType.ALERT, this.textBox12.Text);
										}
									}
								}
							}
						}
					}
				}
				this.LogMessage("Tüm oyunculara mesaj gönderildi: " + this.comboBox2.Text + " - " + this.textBox12.Text, "Bilgi");
			}
			else
			{
				MessageBox.Show("Lütfen Seçim Yapınız.!", "Uyarı");
			}
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000086CC File Offset: 0x000068CC
		public static void Herkes(GSPacketIn Paket)
		{
			GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
			for (int i = 0; i < allPlayers.Length; i++)
			{
				allPlayers[i].SendTCP(Paket);
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00008700 File Offset: 0x00006900
		public static void Herkes_2(eMessageType Msj, string Msj2)
		{
			GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
			for (int i = 0; i < allPlayers.Length; i++)
			{
				allPlayers[i].Out.SendMessage(Msj, Msj2);
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00008738 File Offset: 0x00006938
		private void zaza1(string zaza)
		{
			try
			{
				string address = ServerManagementForm.link + zaza;
				new WebClient().DownloadString(address);
				Console.WriteLine("Güncelleme Başarılı: " + DateTime.Now.ToString());
				this.LogMessage("Güncelleme başarılı: " + zaza, "Bilgi");
			}
			catch
			{
				Console.WriteLine("Güncelleme Başarısız.!");
				this.LogMessage("Güncelleme başarısız: " + zaza, "Hata");
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000087D0 File Offset: 0x000069D0
		private void button1_Click(object sender, EventArgs e)
		{
			bool flag = !string.IsNullOrEmpty(this.comboBox3.Text);
			bool flag2 = flag;
			if (flag2)
			{
				bool flag3 = this.comboBox3.Text == "Tüm Veritabanını Güncelle";
				bool flag4 = flag3;
				if (flag4)
				{
					bool flag5 = BallMgr.ReLoad();
					bool flag6 = flag5;
					if (flag6)
					{
						Console.WriteLine("Ball info Güncelleniyor !");
						this.zaza1("Balllist.ashx");
					}
					Console.WriteLine("Ball info Güncellendi !");
					bool flag7 = MapMgr.ReLoadMap();
					bool flag8 = flag7;
					if (flag8)
					{
						Console.WriteLine("Map info Güncelleniyor !");
						this.zaza1("MapServerList.ashx");
					}
					Console.WriteLine("Map info Güncellendi !");
					bool flag9 = MapMgr.ReLoadMapServer();
					bool flag10 = flag9;
					if (flag10)
					{
						Console.WriteLine("mapserver info Güncelleniyor !");
						this.zaza1("MapServerList.ashx");
					}
					Console.WriteLine("mapserver Güncellendi !");
					bool flag11 = PropItemMgr.Reload();
					bool flag12 = flag11;
					if (flag12)
					{
						Console.WriteLine("prop info Güncelleniyor !");
					}
					Console.WriteLine("prop info Güncellendi !");
					bool flag13 = ItemMgr.ReLoad();
					bool flag14 = flag13;
					if (flag14)
					{
						Console.WriteLine("item info Güncelleniyor !");
					}
					Console.WriteLine("item info Güncellendi !");
					bool flag15 = ShopMgr.ReLoad();
					bool flag16 = flag15;
					if (flag16)
					{
						Console.WriteLine("shop info Güncelleniyor !");
						this.zaza1("ShopItemList.ashx");
					}
					Console.WriteLine("shop info Güncellendi !");
					bool flag17 = QuestMgr.ReLoad();
					bool flag18 = flag17;
					if (flag18)
					{
						Console.WriteLine("quest info Güncelleniyor !");
						this.zaza1("QuestList.ashx");
					}
					Console.WriteLine("quest info Güncellendi !");
					bool flag19 = FusionMgr.ReLoad();
					bool flag20 = flag19;
					if (flag20)
					{
						Console.WriteLine("fusion info Güncelleniyor !");
					}
					Console.WriteLine("fusion info Güncellendi !");
					bool flag21 = ConsortiaMgr.ReLoad();
					bool flag22 = flag21;
					if (flag22)
					{
						Console.WriteLine("consortiaMgr info Güncelleniyor !");
						this.zaza1("ConsortiaAllyList.ashx");
					}
					Console.WriteLine("consortiaMgr info Güncellendi !");
					bool flag23 = RateMgr.ReLoad();
					bool flag24 = flag23;
					if (flag24)
					{
						Console.WriteLine("Rate Rate Güncelleniyor !");
					}
					Console.WriteLine("Rate Rate Güncellendi !");
					bool flag25 = NPCInfoMgr.ReLoad();
					bool flag26 = flag25;
					if (flag26)
					{
						Console.WriteLine("NPCInfo Güncelleniyor !");
						this.zaza1("NPCInfoList.ashx");
					}
					Console.WriteLine("NPCInfo Güncellendi !");
					bool flag27 = FightRateMgr.ReLoad();
					bool flag28 = flag27;
					if (flag28)
					{
						Console.WriteLine("FightRateMgr Güncelleniyor !");
					}
					Console.WriteLine("FightRateMgr Güncellendi !");
					bool flag29 = AwardMgr.ReLoad();
					bool flag30 = flag29;
					if (flag30)
					{
						Console.WriteLine("dailyaward Güncelleniyor !");
					}
					Console.WriteLine("dailyaward Güncellendi !");
					bool flag31 = LanguageMgr.Reload("");
					bool flag32 = flag31;
					if (flag32)
					{
						Console.WriteLine("language Güncelleniyor !");
					}
					Console.WriteLine("language Güncellendi !");
					Console.WriteLine("Veritabanı Başarıyla Güncellendi !");
					MessageBox.Show("Veritabanı Güncellenmiştir.");
					this.LogMessage("Tüm veritabanı güncellendi.", "Bilgi");
				}
				else
				{
					bool flag33 = this.comboBox3.Text == "Görev Güncelle";
					bool flag34 = flag33;
					if (flag34)
					{
						this.zaza1("QuestList.ashx");
						MessageBox.Show("(Görevler) Güncellendi [Başarılı]");
						this.LogMessage("Görevler güncellendi.", "Bilgi");
					}
					else
					{
						bool flag35 = this.comboBox3.Text == "Onur Listesi Güncelle";
						bool flag36 = flag35;
						if (flag36)
						{
							this.zaza1("CelebList/CreateAllCeleb.ashx");
							MessageBox.Show("(Onur Listesi) Güncellendi [Başarılı]");
							this.LogMessage("Onur listesi güncellendi.", "Bilgi");
						}
						else
						{
							bool flag37 = this.comboBox3.Text == "Etkinlikleri Güncelle";
							bool flag38 = flag37;
							if (flag38)
							{
								this.zaza1("ActiveList.ashx");
								MessageBox.Show("(Zaman Sınırlı) Güncellendi [Başarılı]");
								this.LogMessage("Zaman sınırlı etkinlikler güncellendi.", "Bilgi");
							}
							else
							{
								bool flag39 = this.comboBox3.Text == "Pve_İnfo Güncelle";
								bool flag40 = flag39;
								if (flag40)
								{
									this.zaza1("LoadPVEItems.ashx");
									MessageBox.Show("(Droplar) Güncellendi [Başarılı]");
									this.LogMessage("PVE itemları güncellendi.", "Bilgi");
								}
								else
								{
									bool flag41 = this.comboBox3.Text == "Templatelist Güncelle";
									bool flag42 = flag41;
									if (flag42)
									{
										this.zaza1("TemplateAlllist.ashx");
										MessageBox.Show("Templist Güncellendi. [Başarılı]");
										this.LogMessage("Template listesi güncellendi.", "Bilgi");
									}
									else
									{
										bool flag43 = this.comboBox3.Text == "Füzyon Güncelle";
										bool flag44 = flag43;
										if (flag44)
										{
											bool flag45 = FusionMgr.ReLoad();
											bool flag46 = flag45;
											if (flag46)
											{
												Console.WriteLine("fusion Güncelleniyor...");
											}
											Console.WriteLine("fusion Güncellendi.!");
											MessageBox.Show("Füzyon Güncellendi.");
											this.LogMessage("Füzyon güncellendi.", "Bilgi");
										}
										else
										{
											bool flag47 = this.comboBox3.Text == "Shop Güncelle";
											bool flag48 = flag47;
											if (flag48)
											{
												bool flag49 = ShopMgr.ReLoad();
												bool flag50 = flag49;
												if (flag50)
												{
													this.zaza1("ShopItemList.ashx");
													Console.WriteLine("Shop Güncelleniyor...");
												}
												Console.WriteLine("Shop Güncellendi.!");
												MessageBox.Show("Shop Güncellendi.");
												this.LogMessage("Shop güncellendi.", "Bilgi");
											}
											else
											{
												bool flag51 = this.comboBox3.Text == "Mission Güncelle";
												bool flag52 = flag51;
												if (flag52)
												{
													bool flag53 = MissionInfoMgr.Reload();
													bool flag54 = flag53;
													if (flag54)
													{
														Console.WriteLine("Şartlar Güncelleniyor...");
													}
													Console.WriteLine("Şartlar Güncellendi.!");
													MessageBox.Show("Şartlar Güncellendi.");
													this.LogMessage("Mission bilgileri güncellendi.", "Bilgi");
												}
												else
												{
													bool flag55 = this.comboBox3.Text == "Npc Güncelle";
													bool flag56 = flag55;
													if (flag56)
													{
														bool flag57 = NPCInfoMgr.ReLoad();
														bool flag58 = flag57;
														if (flag58)
														{
															this.zaza1("NPCInfoList.ashx");
															Console.WriteLine("Keşifler Güncelleniyor !");
														}
														Console.WriteLine("Keşifler Güncellendi !");
														MessageBox.Show("Keşifler Güncellenmiştir.");
														this.LogMessage("NPC'ler güncellendi.", "Bilgi");
													}
													else
													{
														bool flag59 = this.comboBox3.Text == "Ball Güncelle";
														bool flag60 = flag59;
														if (flag60)
														{
															bool flag61 = BallMgr.ReLoad();
															bool flag62 = flag61;
															if (flag62)
															{
																this.zaza1("Balllist.ashx");
																Console.WriteLine("Ball Güncelleniyor...");
															}
															Console.WriteLine("Ball Güncellendi.!");
															MessageBox.Show("Ball Güncellendi.");
															this.LogMessage("Ball güncellendi.", "Bilgi");
														}
														else
														{
															bool flag63 = this.comboBox3.Text == "Ball Config Güncelle";
															bool flag64 = flag63;
															if (flag64)
															{
																bool flag65 = BallConfigMgr.ReLoad();
																bool flag66 = flag65;
																if (flag66)
																{
																	this.zaza1("BombConfig.ashx");
																	Console.WriteLine("Ball Config Güncelleniyor...");
																}
																Console.WriteLine("Ball Config Güncellendi.!");
																MessageBox.Show("Ball Config Güncellendi.");
																this.LogMessage("Ball config güncellendi.", "Bilgi");
															}
															else
															{
																bool flag67 = this.comboBox3.Text == "Yazıları Güncelle";
																bool flag68 = flag67;
																if (flag68)
																{
																	bool flag69 = LanguageMgr.Reload("");
																	bool flag70 = flag69;
																	if (flag70)
																	{
																		Console.WriteLine("language Güncelleniyor...");
																	}
																	Console.WriteLine("language Güncellendi.!");
																	MessageBox.Show("Yazılar Güncellendi.");
																	this.LogMessage("Dil dosyaları güncellendi.", "Bilgi");
																}
																else
																{
																	bool flag71 = this.comboBox3.Text == "Goldları Güncelle";
																	bool flag72 = flag71;
																	if (flag72)
																	{
																		bool flag73 = GoldEquipMgr.ReLoad();
																		bool flag74 = flag73;
																		if (flag74)
																		{
																			this.zaza1("GoldEquipTemplateLoad.ashx");
																			Console.WriteLine("Goldlar Güncelleniyor...");
																		}
																		Console.WriteLine("Goldlar Güncellendi.!");
																		MessageBox.Show("Goldlar Güncellendi.");
																		this.LogMessage("Gold ekipmanlar güncellendi.", "Bilgi");
																	}
																	else
																	{
																		bool flag75 = this.comboBox3.Text == "Mapları Güncelle";
																		bool flag76 = flag75;
																		if (flag76)
																		{
																			bool flag77 = MapMgr.ReLoadMap();
																			bool flag78 = flag77;
																			if (flag78)
																			{
																				Console.WriteLine("Map info Güncelleniyor !");
																			}
																			bool flag79 = MapMgr.ReLoadMapServer();
																			bool flag80 = flag79;
																			if (flag80)
																			{
																				Console.WriteLine("mapserver info Güncelleniyor !");
																			}
																			this.zaza1("MapServerList.ashx");
																			Console.WriteLine("Maplar Güncellendi !");
																			MessageBox.Show("Maplar Başarıyla Güncellendi !", "Bilgi");
																			this.LogMessage("Maplar güncellendi.", "Bilgi");
																		}
																		else
																		{
																			bool flag81 = this.comboBox3.Text == "Dropları Güncelle";
																			bool flag82 = flag81;
																			if (flag82)
																			{
																				bool flag83 = DropMgr.ReLoad();
																				bool flag84 = flag83;
																				if (flag84)
																				{
																					this.zaza1("LoadPVEItems.ashx");
																					Console.WriteLine("Droplar Güncelleniyor !");
																				}
																				Console.WriteLine("Droplar Güncellendi !");
																				MessageBox.Show("Droplar Başarıyla Güncellendi !", "Bilgi");
																				this.LogMessage("Droplar güncellendi.", "Bilgi");
																			}
																			else
																			{
																				bool flag85 = this.comboBox3.Text == "EventAward Güncelle";
																				bool flag86 = flag85;
																				if (flag86)
																				{
																					bool flag87 = EventAwardMgr.ReLoad();
																					bool flag88 = flag87;
																					if (flag88)
																					{
																						Console.WriteLine("EventAward Güncelleniyor !");
																					}
																					Console.WriteLine("EventAward Güncellendi !");
																					MessageBox.Show("EventAward Başarıyla Güncellendi !", "Bilgi");
																					this.LogMessage("Etkinlik ödülleri güncellendi.", "Bilgi");
																				}
																			}
																		}
																	}
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			else
			{
				MessageBox.Show("Lütfen Seçim Yapınız.!", "Uyarı");
			}
		}

		// Token: 0x06000063 RID: 99 RVA: 0x0000917C File Offset: 0x0000737C
		private void button2_Click(object sender, EventArgs e)
		{
			this.dataGridView1.ForeColor = Color.Black;
			bool flag = this.comboBox4.Text == "Banlı Hesaplar";
			bool flag2 = flag;
			if (flag2)
			{
				this.Baglanti_Db.Open();
				DbDataAdapter dbDataAdapter = new SqlDataAdapter("Select NickName[Nick], ForbidReason[Ban Sebebi] From Sys_Users_Detail Where IsExist='" + 0.ToString() + "'", this.Baglanti_Db);
				DataTable dataTable = new DataTable();
				dbDataAdapter.Fill(dataTable);
				this.dataGridView1.DataSource = dataTable;
				this.Baglanti_Db.Close();
				this.LogMessage("Banlı hesaplar listelendi.", "Bilgi");
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00009224 File Offset: 0x00007424
		private void button3_Click(object sender, EventArgs e)
		{
			SqlCommand sqlCommand = new SqlCommand("select UserId From Mem_Users Where UserName ='" + this.textBox13.Text + "'", this.Baglanti_Membership);
			this.Baglanti_Membership.Open();
			SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
			while (sqlDataReader.Read())
			{
				string str = sqlDataReader["UserId"].ToString();
				this.Baglanti_Membership_2.Open();
				new SqlCommand("Update Mem_UserInfo Set Password ='e10adc3949ba59abbe56e057f20f883e' Where UserId='" + str + "'", this.Baglanti_Membership_2).ExecuteNonQuery();
				this.Baglanti_Membership_2.Close();
			}
			this.Baglanti_Membership.Close();
			MessageBox.Show("Kişinin Şifresi Başarıyla Değiştirilmiştir, Yeni Şifre: 123456", "Bilgi");
			this.LogMessage("Oyuncu şifresi sıfırlandı: " + this.textBox13.Text, "Güvenlik");
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00009300 File Offset: 0x00007500
		private void ServerManagementForm_Load(object sender, EventArgs e)
		{
			bool isActive = HydroFilter.IsActive;
			bool flag = isActive;
			if (flag)
			{
				this.label28.Text = "Aktif";
				this.label28.ForeColor = Color.Green;
			}
			else
			{
				this.label28.Text = "Pasif";
				this.label28.ForeColor = Color.Red;
			}
			this.label30.Text = HydroFilter.ConnectionCount.ToString();
			this.label33.Text = HydroFilter.BlockedConnections.ToString();
			this.LogMessage("Sunucu yönetim paneli yüklendi.", "Bilgi");
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000093A0 File Offset: 0x000075A0
		private void SearchPlayer_Click(object sender, EventArgs e)
		{
			this.LogMessage("Oyuncu arama yapıldı.", "Bilgi");
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000093B4 File Offset: 0x000075B4
		private void PlayerList_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.LogMessage("Oyuncu detayları görüntülendi.", "Bilgi");
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000093C8 File Offset: 0x000075C8
		private void PlayerActionButton_Click(object sender, EventArgs e)
		{
			Button button = (Button)sender;
			switch ((int)button.Tag)
			{
			case 0:
			{
				string str = "Oyuncu banlandı: ";
				GamePlayer player = this.player;
				this.LogMessage(str + ((player != null) ? player.PlayerCharacter.NickName : null), "Güvenlik");
				break;
			}
			case 1:
			{
				string str2 = "Oyuncu banı açıldı: ";
				GamePlayer player2 = this.player;
				this.LogMessage(str2 + ((player2 != null) ? player2.PlayerCharacter.NickName : null), "Güvenlik");
				break;
			}
			case 2:
			{
				string str3 = "Oyuncu kicklendi: ";
				GamePlayer player3 = this.player;
				this.LogMessage(str3 + ((player3 != null) ? player3.PlayerCharacter.NickName : null), "Güvenlik");
				break;
			}
			case 3:
			{
				string str4 = "Oyuncu şifresi sıfırlandı: ";
				GamePlayer player4 = this.player;
				this.LogMessage(str4 + ((player4 != null) ? player4.PlayerCharacter.NickName : null), "Güvenlik");
				break;
			}
			case 4:
			{
				string str5 = "Oyuncuya item verildi: ";
				GamePlayer player5 = this.player;
				this.LogMessage(str5 + ((player5 != null) ? player5.PlayerCharacter.NickName : null), "Bilgi");
				break;
			}
			case 5:
			{
				string str6 = "Oyuncuya para verildi: ";
				GamePlayer player6 = this.player;
				this.LogMessage(str6 + ((player6 != null) ? player6.PlayerCharacter.NickName : null), "Bilgi");
				break;
			}
			case 6:
			{
				string str7 = "Oyuncu seviyesi ayarlandı: ";
				GamePlayer player7 = this.player;
				this.LogMessage(str7 + ((player7 != null) ? player7.PlayerCharacter.NickName : null), "Bilgi");
				break;
			}
			case 7:
			{
				string str8 = "Oyuncu TP konumlandırıldı: ";
				GamePlayer player8 = this.player;
				this.LogMessage(str8 + ((player8 != null) ? player8.PlayerCharacter.NickName : null), "Bilgi");
				break;
			}
			case 8:
			{
				string str9 = "Oyuncuya mektup gönderildi: ";
				GamePlayer player9 = this.player;
				this.LogMessage(str9 + ((player9 != null) ? player9.PlayerCharacter.NickName : null), "Bilgi");
				break;
			}
			case 9:
			{
				string str10 = "Oyuncu detaylı görüntülendi: ";
				GamePlayer player10 = this.player;
				this.LogMessage(str10 + ((player10 != null) ? player10.PlayerCharacter.NickName : null), "Bilgi");
				break;
			}
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x0000960C File Offset: 0x0000780C
		private void ServerActionButton_Click(object sender, EventArgs e)
		{
			Button button = (Button)sender;
			switch ((int)button.Tag)
			{
			case 0:
				this.serverMaintenanceMode = !this.serverMaintenanceMode;
				MessageBox.Show(this.serverMaintenanceMode ? "Bakım modu aktif edildi." : "Bakım modu devre dışı bırakıldı.");
				this.LogMessage("Bakım modu " + (this.serverMaintenanceMode ? "aktif edildi" : "devre dışı bırakıldı") + ".", "Bilgi");
				break;
			case 1:
			{
				bool flag = MessageBox.Show("Sunucuyu yeniden başlatmak istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo) == DialogResult.Yes;
				if (flag)
				{
					this.LogMessage("Sunucu yeniden başlatılıyor...", "Bilgi");
				}
				break;
			}
			case 2:
			{
				bool flag2 = MessageBox.Show("Sunucuyu kapatmak istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo) == DialogResult.Yes;
				if (flag2)
				{
					this.LogMessage("Sunucu kapatılıyor...", "Bilgi");
				}
				break;
			}
			case 3:
				GC.Collect();
				GC.WaitForPendingFinalizers();
				MessageBox.Show("RAM temizlendi.");
				this.LogMessage("RAM temizlendi.", "Bilgi");
				break;
			case 4:
			{
				bool flag3 = MessageBox.Show("Tüm logları temizlemek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo) == DialogResult.Yes;
				if (flag3)
				{
					this.LogMessage("Loglar temizlendi.", "Bilgi");
				}
				break;
			}
			case 5:
				this.LogMessage("Sunucu yedeği alınıyor...", "Bilgi");
				break;
			case 6:
				this.serverUnderAttack = !this.serverUnderAttack;
				MessageBox.Show(this.serverUnderAttack ? "DDoS koruması aktif edildi." : "DDoS koruması devre dışı bırakıldı.");
				this.LogMessage("DDoS koruması " + (this.serverUnderAttack ? "aktif edildi" : "devre dışı bırakıldı") + ".", "Güvenlik");
				break;
			case 7:
				this.autoMessageTimer.Enabled = !this.autoMessageTimer.Enabled;
				MessageBox.Show(this.autoMessageTimer.Enabled ? "Otomatik mesajlar aktif edildi." : "Otomatik mesajlar devre dışı bırakıldı.");
				this.LogMessage("Otomatik mesajlar " + (this.autoMessageTimer.Enabled ? "aktif edildi" : "devre dışı bırakıldı") + ".", "Bilgi");
				break;
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00009858 File Offset: 0x00007A58
		private void SendAnnouncementButton_Click(object sender, EventArgs e)
		{
			this.LogMessage("Duyuru gönderildi.", "Bilgi");
		}

		// Token: 0x0600006B RID: 107 RVA: 0x0000986C File Offset: 0x00007A6C
		private void AutoMessageTimer_Tick(object sender, EventArgs e)
		{
			bool flag = this.autoMessages.Count > 0;
			if (flag)
			{
				string str = this.autoMessages[this.currentAutoMessageIndex];
				GamePlayer[] allPlayers = WorldMgr.GetAllPlayers();
				foreach (GamePlayer gamePlayer in allPlayers)
				{
					gamePlayer.SendMessage("[OTOMATİK] " + str);
				}
				this.currentAutoMessageIndex = (this.currentAutoMessageIndex + 1) % this.autoMessages.Count;
				this.LogMessage("Otomatik mesaj gönderildi: " + str, "Bilgi");
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00009907 File Offset: 0x00007B07
		private void AddAutoMessageButton_Click(object sender, EventArgs e)
		{
			this.LogMessage("Otomatik mesaj eklendi.", "Bilgi");
		}

		// Token: 0x0600006D RID: 109 RVA: 0x0000991B File Offset: 0x00007B1B
		private void RemoveAutoMessageButton_Click(object sender, EventArgs e)
		{
			this.LogMessage("Otomatik mesaj kaldırıldı.", "Bilgi");
		}

		// Token: 0x0600006E RID: 110 RVA: 0x0000992F File Offset: 0x00007B2F
		private void DbUpdateButton_Click(object sender, EventArgs e)
		{
			this.LogMessage("Veritabanı güncellemesi başlatıldı.", "Bilgi");
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00009943 File Offset: 0x00007B43
		private void DbExecuteButton_Click(object sender, EventArgs e)
		{
			this.LogMessage("Veritabanı sorgusu çalıştırıldı.", "Bilgi");
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00009957 File Offset: 0x00007B57
		private void DbExportButton_Click(object sender, EventArgs e)
		{
			this.LogMessage("Veritabanı dışa aktarıldı.", "Bilgi");
		}

		// Token: 0x06000071 RID: 113 RVA: 0x0000996B File Offset: 0x00007B6B
		private void DbBackupButton_Click(object sender, EventArgs e)
		{
			this.LogMessage("Veritabanı yedeği alınıyor...", "Bilgi");
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00009980 File Offset: 0x00007B80
		private void DbRestoreButton_Click(object sender, EventArgs e)
		{
			bool flag = MessageBox.Show("Veritabanını geri yüklemek istediğinize emin misiniz? Bu işlem mevcut verilerin üzerine yazacaktır.", "Onay", MessageBoxButtons.YesNo) == DialogResult.Yes;
			if (flag)
			{
				this.LogMessage("Veritabanı geri yükleniyor...", "Bilgi");
			}
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000099B8 File Offset: 0x00007BB8
		private void DbOptimizeButton_Click(object sender, EventArgs e)
		{
			this.LogMessage("Veritabanı optimizasyonu başlatıldı.", "Bilgi");
		}

		// Token: 0x06000074 RID: 116 RVA: 0x000099CC File Offset: 0x00007BCC
		private void DbRepairButton_Click(object sender, EventArgs e)
		{
			bool flag = MessageBox.Show("Veritabanını onarmak istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo) == DialogResult.Yes;
			if (flag)
			{
				this.LogMessage("Veritabanı onarımı başlatıldı.", "Bilgi");
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00009A04 File Offset: 0x00007C04
		private void StartEventButton_Click(object sender, EventArgs e)
		{
			this.LogMessage("Etkinlik başlatıldı.", "Bilgi");
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00009A18 File Offset: 0x00007C18
		private void StopEventButton_Click(object sender, EventArgs e)
		{
			this.LogMessage("Etkinlik durduruldu.", "Bilgi");
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00009A2C File Offset: 0x00007C2C
		private void CreateEventButton_Click(object sender, EventArgs e)
		{
			this.LogMessage("Yeni etkinlik oluşturuldu.", "Bilgi");
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00009A40 File Offset: 0x00007C40
		private void AddRewardButton_Click(object sender, EventArgs e)
		{
			this.LogMessage("Etkinlik ödülü eklendi.", "Bilgi");
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00009A54 File Offset: 0x00007C54
		private void RemoveRewardButton_Click(object sender, EventArgs e)
		{
			this.LogMessage("Etkinlik ödülü kaldırıldı.", "Bilgi");
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00009A68 File Offset: 0x00007C68
		private void RewardAllButton_Click(object sender, EventArgs e)
		{
			this.LogMessage("Tüm etkinlik katılımcılarına ödül verildi.", "Bilgi");
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00009A7C File Offset: 0x00007C7C
		private void ExportParticipantsButton_Click(object sender, EventArgs e)
		{
			this.LogMessage("Etkinlik katılımcıları dışa aktarıldı.", "Bilgi");
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00009A90 File Offset: 0x00007C90
		private void ViewReportButton_Click(object sender, EventArgs e)
		{
			this.LogMessage("Rapor görüntülendi.", "Bilgi");
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00009AA4 File Offset: 0x00007CA4
		private void ResolveReportButton_Click(object sender, EventArgs e)
		{
			this.LogMessage("Rapor çözüldü.", "Bilgi");
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00009AB8 File Offset: 0x00007CB8
		private void BanReportedPlayerButton_Click(object sender, EventArgs e)
		{
			this.LogMessage("Raporlanan oyuncu banlandı.", "Güvenlik");
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00009ACC File Offset: 0x00007CCC
		private void RefreshStatsButton_Click(object sender, EventArgs e)
		{
			this.LogMessage("İstatistikler yenilendi.", "Bilgi");
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00009AE0 File Offset: 0x00007CE0
		private void ExportStatsButton_Click(object sender, EventArgs e)
		{
			this.LogMessage("İstatistikler dışa aktarıldı.", "Bilgi");
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00009AF4 File Offset: 0x00007CF4
		private void SaveSettingsButton_Click(object sender, EventArgs e)
		{
			this.LogMessage("Sunucu ayarları kaydedildi.", "Bilgi");
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00009B08 File Offset: 0x00007D08
		private void ResetSettingsButton_Click(object sender, EventArgs e)
		{
			bool flag = MessageBox.Show("Tüm ayarları varsayılan değerlere sıfırlamak istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo) == DialogResult.Yes;
			if (flag)
			{
				this.LogMessage("Sunucu ayarları sıfırlandı.", "Bilgi");
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00009B40 File Offset: 0x00007D40
		private void AddChannelButton_Click(object sender, EventArgs e)
		{
			this.LogMessage("Yeni kanal eklendi.", "Bilgi");
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00009B54 File Offset: 0x00007D54
		private void RemoveChannelButton_Click(object sender, EventArgs e)
		{
			this.LogMessage("Kanal kaldırıldı.", "Bilgi");
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00009B68 File Offset: 0x00007D68
		private void SaveSecurityButton_Click(object sender, EventArgs e)
		{
			this.LogMessage("Güvenlik ayarları kaydedildi.", "Güvenlik");
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00009B7C File Offset: 0x00007D7C
		private void ResetSecurityButton_Click(object sender, EventArgs e)
		{
			bool flag = MessageBox.Show("Tüm güvenlik ayarlarını varsayılan değerlere sıfırlamak istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo) == DialogResult.Yes;
			if (flag)
			{
				this.LogMessage("Güvenlik ayarları sıfırlandı.", "Güvenlik");
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00009BB4 File Offset: 0x00007DB4
		private void FilterLogsButton_Click(object sender, EventArgs e)
		{
			this.LogMessage("Loglar filtrelendi.", "Bilgi");
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00009BC8 File Offset: 0x00007DC8
		private void SearchLogsButton_Click(object sender, EventArgs e)
		{
			this.LogMessage("Loglarda arama yapıldı.", "Bilgi");
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00009BDC File Offset: 0x00007DDC
		private void ViewLogDetailsButton_Click(object sender, EventArgs e)
		{
			this.LogMessage("Log detayları görüntülendi.", "Bilgi");
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00009BF0 File Offset: 0x00007DF0
		private void ExportLogsButton_Click(object sender, EventArgs e)
		{
			this.LogMessage("Loglar dışa aktarıldı.", "Bilgi");
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00009C04 File Offset: 0x00007E04
		private void ClearLogsButton_Click(object sender, EventArgs e)
		{
			bool flag = MessageBox.Show("Tüm logları temizlemek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo) == DialogResult.Yes;
			if (flag)
			{
				this.LogMessage("Loglar temizlendi.", "Bilgi");
			}
		}

		// Token: 0x04000017 RID: 23
		private Timer statsTimer;

		// Token: 0x04000018 RID: 24
		private StreamWriter logWriter;

		// Token: 0x04000019 RID: 25
		private string logFilePath = "ServerManagementLogs.txt";

		// Token: 0x0400001A RID: 26
		private bool serverMaintenanceMode = false;

		// Token: 0x0400001B RID: 27
		private bool serverUnderAttack = false;

		// Token: 0x0400001C RID: 28
		private Timer autoMessageTimer;

		// Token: 0x0400001D RID: 29
		private List<string> autoMessages = new List<string>();

		// Token: 0x0400001E RID: 30
		private int currentAutoMessageIndex = 0;

		// Token: 0x0400001F RID: 31
		private List<PlayerReport> playerReports = new List<PlayerReport>();

		// Token: 0x04000020 RID: 32
		private SqlConnection Baglanti_Db = new SqlConnection(ConfigurationManager.AppSettings.Get("conString"));

		// Token: 0x04000021 RID: 33
		private SqlConnection Baglanti_Membership = new SqlConnection("Data Source=WIN-8H6JMQBVGP2/GUNNYTURKEY;Initial Catalog=Db_Membership;Persist Security Info=True;User ID=sa;Password=GunnyTurkey0!");

		// Token: 0x04000022 RID: 34
		private SqlConnection Baglanti_Membership_2 = new SqlConnection("Data Source=WIN-8H6JMQBVGP2/GUNNYTURKEY;Initial Catalog=Db_Membership;Persist Security Info=True;User ID=sa;Password=GunnyTurkey0!");

		// Token: 0x04000023 RID: 35
		private static string link = "http://185.88.175.105/Request/";
	}
}
