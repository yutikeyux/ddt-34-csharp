using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using LauncherGHU.ControlsForm;

namespace LauncherGHU
{
	public class PlayGameFrm2FIB : Form
	{
		public static bool IsUpdate;

		private string string_0 = null;

		private string string_1;

		private System.Threading.Timer timer_0;

		private Point point_0;

		private PlayGame playGame_Control;

		private SelectServer selectServer_Control;

		private Recharge recharge_Control = null;

		private Account account_Control = null;

		public const int WM_NCLBUTTONDOWN = 161;

		public const int HT_CAPTION = 2;

		private Thread thread_0;

		private IContainer icontainer_0 = null;

		private MenuStrip menuStrip;

		private ToolStripMenuItem highToolStripMenuItem;

		private ToolStripMenuItem lowToolStripMenuItem;

		private ToolStripMenuItem normalToolStripMenuItem;

		private ToolStripMenuItem menuQuality;

		private ToolStripComboBox toolStripComboBoxZoom;

		private MenuStrip txtRue;

		private ToolStripMenuItem doimaychuBtn;

		private ToolStripMenuItem tienichMenu;

		public ToolStripMenuItem naptheItem;

		private ToolStripMenuItem toolStripMenuItem_0;

		private ToolStripMenuItem toolStripMenuItem_1;

		private ToolStripMenuItem chonmaychuItemMenuTxt;

		private System.Windows.Forms.Timer timer_1;

		private System.Windows.Forms.Timer timer_2;

		private Panel panel_Playgame;

		public ToolStripMenuItem sukienTuanItem;

		private ToolStripMenuItem exitBtn;

		private ToolStripMenuItem maxminBtn;

		private ToolStripMenuItem miniBtn;

		private static PlayGameFrm2FIB GP7T9hb2NsfeA1JGaQ3;

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassStyle |= 131072;
				return createParams;
			}
		}

		public PlayGameFrm2FIB(int serverid, string nameserver)
		{
			if (Environment.OSVersion.Version.Major >= 6)
			{
				SetProcessDPIAware();
			}
			InitializeComponent();
			timer_0 = new System.Threading.Timer(ControlMgr.stopCheats, null, 15000, 15000);
			LoginMgr.ServerID = serverid;
			string_0 = nameserver;
			method_0();
			tienichMenu.Text = LoginMgr.Username;
			Text = ApplicationConfig.ServerTitle + " - " + LoginMgr.Username;
			toolStripComboBoxZoom.Visible = false;
			doimaychuBtn.Visible = false;
			method_2();
			method_1();
			if (LoginMgr.ServerID != 0)
			{
				method_7();
			}
		}

		private void method_0()
		{
			string_1 = ControlMgr.GetCoin();
		}

		private void method_1()
		{
			try
			{
				playGame_Control = new PlayGame(this);
				playGame_Control.Dock = DockStyle.Fill;
				playGame_Control.Location = new Point(0, 32);
				playGame_Control.Name = "playGame_Control";
				playGame_Control.Size = new Size(1000, 644);
				playGame_Control.TabIndex = 10;
				playGame_Control.Visible = true;
				panel_Playgame.Controls.Add(playGame_Control);
			}
			catch (Exception ex)
			{
				ControlMgr.UpLogLauncher(ex.ToString() + "-(SetupPlayGameControl1)- Now Version : " + Assembly.GetEntryAssembly().GetName().Version);
			}
			base.ActiveControl = playGame_Control;
		}

		private void method_2()
		{
			selectServer_Control = new SelectServer(this);
			selectServer_Control.Dock = DockStyle.Fill;
			selectServer_Control.Location = new Point(0, 32);
			selectServer_Control.Name = "selectServer_Control";
			selectServer_Control.Size = new Size(1000, 644);
			selectServer_Control.TabIndex = 10;
			selectServer_Control.Visible = true;
			panel_Playgame.Controls.Add(selectServer_Control);
		}

		[DllImport("user32.dll")]
		private static extern bool SetProcessDPIAware();

		private void menuStrip_MouseDown(object sender, MouseEventArgs e)
		{
			method_3(e);
		}

		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

		[DllImport("user32.dll")]
		public static extern bool ReleaseCapture();

		private void method_3(MouseEventArgs mouseEventArgs_0)
		{
			if (mouseEventArgs_0.Button == MouseButtons.Left)
			{
				ReleaseCapture();
				SendMessage(base.Handle, 161, 2, 0);
			}
		}

		private void toolStripComboBoxZoom_SelectedIndexChanged(object sender, EventArgs e)
		{
			float num = (float)((double)(toolStripComboBoxZoom.SelectedIndex - 10) * 0.0500000007450581 + 1.0);
			double num2 = num;
			SizeF scale = new SizeF((float)num2, (float)num2);
			playGame_Control.SetScaleMode(2);
			playGame_Control.SetScale(scale);
			playGame_Control.SetZoom(100);
			double num3 = (double)num * 24.0;
			base.Size = new Size((int)((double)num * 1000.0), (int)((double)num * 600.0 + 27.0 + num3));
			txtRue.Height = (int)num3;
		}

		private void highToolStripMenuItem_Click(object sender, EventArgs e)
		{
			playGame_Control.SetQuality("High");
			menuQuality.Text = "Châ\u0301t Lươ\u0323ng Cao";
			playGame_Control.Focus();
			playGame_Control.Select();
		}

		private void lowToolStripMenuItem_Click(object sender, EventArgs e)
		{
			playGame_Control.SetQuality("Low");
			menuQuality.Text = "Châ\u0301t Lươ\u0323ng Thấp";
			playGame_Control.Focus();
			playGame_Control.Select();
		}

		private void normalToolStripMenuItem_Click(object sender, EventArgs e)
		{
			playGame_Control.SetQuality("Medium");
			menuQuality.Text = "Châ\u0301t Lươ\u0323ng Trung Bình";
			playGame_Control.Focus();
			playGame_Control.Select();
		}

		private void method_4(object sender, EventArgs e)
		{
			if (MessageBox.Show("Đóng Client ?", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				timer_2.Start();
			}
		}

		private void method_5(object sender, EventArgs e)
		{
			if (MessageBox.Show("Bạn có muốn xóa toàn bộ cache và đăng xuất không ?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 4351");
				CloseAndBackLogin(isLogout: true);
			}
		}

		public void CloseAndBackLogin(bool isLogout)
		{
			if (base.InvokeRequired)
			{
				Invoke((MethodInvoker)delegate
				{
					CloseAndBackLogin(isLogout);
				});
				return;
			}
			if (isLogout)
			{
				LoginMgr.Logout();
			}
			Close();
			thread_0 = new Thread(method_6);
			thread_0.SetApartmentState(ApartmentState.STA);
			thread_0.Start();
		}

		private void method_6()
		{
			Application.Run(new formLogin());
		}

		private void method_7()
		{
			if (LoginMgr.ServerID != 0)
			{
				InstallGameWebBrowse();
			}
			else
			{
				MessageBox.Show("Vui lòng chọn máy chủ.");
			}
		}

		public void SetTitleForm(string nameserver)
		{
			Text = $"{ApplicationConfig.ServerTitle} | {LoginMgr.Username} - {nameserver}";
		}

		public void InstallGameWebBrowse()
		{
			chonmaychuItemMenuTxt.Text = "Đổi Máy Chủ";
			doimaychuBtn.Text = "Đổi Máy Chủ";
			toolStripComboBoxZoom.SelectedIndex = 10;
			Text = ApplicationConfig.ServerTitle + " - " + LoginMgr.Username + " - " + string_0;
			toolStripComboBoxZoom.Visible = true;
			doimaychuBtn.Visible = true;
			try
			{
				playGame_Control.PlayURLMovie();
				playGame_Control.BringToFront();
			}
			catch (Exception ex)
			{
				ControlMgr.UpLogLauncher(ex.Message.ToString() + "-(InstallGameWebBrowse)- Now Version : " + Assembly.GetEntryAssembly().GetName().Version);
				MessageBox.Show("Xảy ra lỗi tài nguyên , vui lòng khởi động lại Launcher");
				Application.Restart();
			}
		}

		private void doimaychuBtn_Click(object sender, EventArgs e)
		{
			if (!(doimaychuBtn.Text == "Đổi Máy Chủ"))
			{
				playGame_Control.BringToFront();
				playGame_Control.Focus();
				playGame_Control.Select();
				doimaychuBtn.Text = "Đổi Máy Chủ";
				chonmaychuItemMenuTxt.Text = "Đổi Máy Chủ";
			}
			else
			{
				selectServer_Control.BringToFront();
				doimaychuBtn.Text = "Quay Lại";
				chonmaychuItemMenuTxt.Text = "Quay Lại";
			}
		}

		private void PlayGameFrm2FIB_Activated(object sender, EventArgs e)
		{
			playGame_Control.Focus();
			playGame_Control.Select();
		}

		private void method_8(object sender, EventArgs e)
		{
			if (MessageBox.Show("Bạn có muốn đăng xuất không ?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				CloseAndBackLogin(isLogout: true);
			}
		}

		private void hAmGloXtwT(object sender, EventArgs e)
		{
			if (MessageBox.Show("Bạn có muốn xóa toàn bộ cache và đăng xuất không ?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 4351");
				CloseAndBackLogin(isLogout: true);
			}
		}

		private void naptheItem_Click(object sender, EventArgs e)
		{
			if (recharge_Control == null)
			{
				method_9();
			}
			recharge_Control.BringToFront();
			doimaychuBtn.Text = "Quay Lại";
			chonmaychuItemMenuTxt.Text = "Quay Lại";
		}

		private void method_9()
		{
			if (recharge_Control == null)
			{
				recharge_Control = new Recharge();
				recharge_Control.Dock = DockStyle.Fill;
				recharge_Control.Location = new Point(0, 32);
				recharge_Control.Name = "recharge_Control";
				recharge_Control.Size = new Size(1000, 644);
				recharge_Control.TabIndex = 0;
				recharge_Control.Visible = true;
				panel_Playgame.Controls.Add(recharge_Control);
			}
		}

		private void method_10(object sender, EventArgs e)
		{
		}

		private void method_11(object sender, EventArgs e)
		{
		}

		private void method_12(object sender, EventArgs e)
		{
		}

		private void toolStripMenuItem_1_Click(object sender, EventArgs e)
		{
			if (account_Control == null)
			{
				method_13();
			}
			account_Control.BringToFront();
			doimaychuBtn.Text = "Quay Lại";
			chonmaychuItemMenuTxt.Text = "Quay Lại";
		}

		private void method_13()
		{
			if (account_Control == null)
			{
				account_Control = new Account();
				account_Control.Dock = DockStyle.Fill;
				account_Control.Location = new Point(0, 32);
				account_Control.Name = "account_Control";
				account_Control.Size = new Size(1000, 644);
				account_Control.TabIndex = 0;
				account_Control.Visible = true;
				panel_Playgame.Controls.Add(account_Control);
			}
		}

		private void toolStripComboBoxZoom_Click(object sender, EventArgs e)
		{
		}

		private void chonmaychuItemMenuTxt_Click(object sender, EventArgs e)
		{
			if (!(chonmaychuItemMenuTxt.Text == "Đổi Máy Chủ"))
			{
				playGame_Control.BringToFront();
				doimaychuBtn.Text = "Đổi Máy Chủ";
				chonmaychuItemMenuTxt.Text = "Đổi Máy Chủ";
			}
			else
			{
				selectServer_Control.BringToFront();
				doimaychuBtn.Text = "Quay Lại";
				chonmaychuItemMenuTxt.Text = "Quay Lại";
			}
		}

		private void method_14(object sender, EventArgs e)
		{
			doimaychuBtn.Text = "Đổi Máy Chủ";
		}

		private void method_15(object sender, EventArgs e)
		{
			ControlMgr.stopCheats();
		}

		private void method_16(object sender, EventArgs e)
		{
			Application.Run(new PlayGameFrmFIB(LoginMgr.ServerID, string_0));
		}

		private void method_17(object sender, EventArgs e)
		{
		}

		private void timer_2_Tick(object sender, EventArgs e)
		{
			base.Opacity -= 0.014;
			if (base.Opacity <= 0.01)
			{
				timer_2.Stop();
				Application.Exit();
			}
		}

		private void sukienTuanItem_Click(object sender, EventArgs e)
		{
			if (LoginMgr.ServerID == 0)
			{
				selectServer_Control.BringToFront();
				doimaychuBtn.Text = "Quay Lại";
				chonmaychuItemMenuTxt.Text = "Quay Lại";
			}
			else
			{
				playGame_Control.BringToFront();
				playGame_Control.CallFlash("CallOpenTopEventWeek");
			}
		}

		private void exitBtn_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Bạn có muốn đăng xuất không ?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				CloseAndBackLogin(isLogout: true);
			}
		}

		private void maxminBtn_Click(object sender, EventArgs e)
		{
			if (base.WindowState == FormWindowState.Normal)
			{
				base.WindowState = FormWindowState.Maximized;
			}
			else
			{
				base.WindowState = FormWindowState.Normal;
			}
		}

		private void miniBtn_Click(object sender, EventArgs e)
		{
			base.WindowState = FormWindowState.Minimized;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && icontainer_0 != null)
			{
				icontainer_0.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			icontainer_0 = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LauncherGHU.PlayGameFrm2FIB));
			menuStrip = new System.Windows.Forms.MenuStrip();
			menuQuality = new System.Windows.Forms.ToolStripMenuItem();
			highToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			lowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			normalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			doimaychuBtn = new System.Windows.Forms.ToolStripMenuItem();
			toolStripComboBoxZoom = new System.Windows.Forms.ToolStripComboBox();
			tienichMenu = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem_1 = new System.Windows.Forms.ToolStripMenuItem();
			naptheItem = new System.Windows.Forms.ToolStripMenuItem();
			sukienTuanItem = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem_0 = new System.Windows.Forms.ToolStripMenuItem();
			chonmaychuItemMenuTxt = new System.Windows.Forms.ToolStripMenuItem();
			txtRue = new System.Windows.Forms.MenuStrip();
			timer_1 = new System.Windows.Forms.Timer(icontainer_0);
			timer_2 = new System.Windows.Forms.Timer(icontainer_0);
			panel_Playgame = new System.Windows.Forms.Panel();
			exitBtn = new System.Windows.Forms.ToolStripMenuItem();
			maxminBtn = new System.Windows.Forms.ToolStripMenuItem();
			miniBtn = new System.Windows.Forms.ToolStripMenuItem();
			menuStrip.SuspendLayout();
			SuspendLayout();
			menuStrip.BackColor = System.Drawing.Color.Sienna;
			menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
			menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { menuQuality, doimaychuBtn, toolStripComboBoxZoom, exitBtn, maxminBtn, miniBtn, tienichMenu });
			menuStrip.Location = new System.Drawing.Point(0, 0);
			menuStrip.Name = "menuStrip";
			menuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			menuStrip.Size = new System.Drawing.Size(1000, 28);
			menuStrip.TabIndex = 1;
			menuStrip.Text = "menuStrip1";
			menuStrip.MouseDown += new System.Windows.Forms.MouseEventHandler(menuStrip_MouseDown);
			menuQuality.BackColor = System.Drawing.Color.Sienna;
			menuQuality.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3] { highToolStripMenuItem, lowToolStripMenuItem, normalToolStripMenuItem });
			menuQuality.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			menuQuality.ForeColor = System.Drawing.Color.White;
			menuQuality.ImageTransparentColor = System.Drawing.Color.Transparent;
			menuQuality.Name = "menuQuality";
			menuQuality.Size = new System.Drawing.Size(156, 24);
			menuQuality.Text = "Chất Lượng Trung Bình";
			highToolStripMenuItem.BackColor = System.Drawing.Color.Sienna;
			highToolStripMenuItem.ForeColor = System.Drawing.Color.White;
			highToolStripMenuItem.Name = "highToolStripMenuItem";
			highToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
			highToolStripMenuItem.Text = "Chất lượng cao";
			highToolStripMenuItem.Click += new System.EventHandler(highToolStripMenuItem_Click);
			lowToolStripMenuItem.BackColor = System.Drawing.Color.Sienna;
			lowToolStripMenuItem.ForeColor = System.Drawing.Color.White;
			lowToolStripMenuItem.Name = "lowToolStripMenuItem";
			lowToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
			lowToolStripMenuItem.Text = "Chất lượng thấp";
			lowToolStripMenuItem.Click += new System.EventHandler(lowToolStripMenuItem_Click);
			normalToolStripMenuItem.BackColor = System.Drawing.Color.Sienna;
			normalToolStripMenuItem.ForeColor = System.Drawing.Color.White;
			normalToolStripMenuItem.Name = "normalToolStripMenuItem";
			normalToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
			normalToolStripMenuItem.Text = "Chất lượng trung bình";
			normalToolStripMenuItem.Click += new System.EventHandler(normalToolStripMenuItem_Click);
			doimaychuBtn.ForeColor = System.Drawing.Color.White;
			doimaychuBtn.Name = "doimaychuBtn";
			doimaychuBtn.Size = new System.Drawing.Size(88, 24);
			doimaychuBtn.Text = "Đổi Máy Chủ";
			doimaychuBtn.Click += new System.EventHandler(doimaychuBtn_Click);
			toolStripComboBoxZoom.BackColor = System.Drawing.Color.Sienna;
			toolStripComboBoxZoom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			toolStripComboBoxZoom.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			toolStripComboBoxZoom.ForeColor = System.Drawing.Color.White;
			toolStripComboBoxZoom.Items.AddRange(new object[21]
			{
				"Kích thước 50%", "Kích thước 55%", "Kích thước 60%", "Kích thước 65%", "Kích thước 70%", "Kích thước 75%", "Kích thước 80%", "Kích thước 85%", "Kích thước 90%", "Kích thước 95%",
				"Kích thước 100%", "Kích thước 105%", "Kích thước 110%", "Kích thước 115%", "Kích thước 120%", "Kích thước 125%", "Kích thước 130%", "Kích thước 135%", "Kích thước 140%", "Kích thước 145%",
				"Kích thước 150%"
			});
			toolStripComboBoxZoom.Name = "toolStripComboBoxZoom";
			toolStripComboBoxZoom.Size = new System.Drawing.Size(135, 24);
			toolStripComboBoxZoom.SelectedIndexChanged += new System.EventHandler(toolStripComboBoxZoom_SelectedIndexChanged);
			toolStripComboBoxZoom.Click += new System.EventHandler(toolStripComboBoxZoom_Click);
			tienichMenu.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			tienichMenu.BackColor = System.Drawing.Color.Sienna;
			tienichMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[5] { toolStripMenuItem_1, naptheItem, sukienTuanItem, toolStripMenuItem_0, chonmaychuItemMenuTxt });
			tienichMenu.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			tienichMenu.ForeColor = System.Drawing.Color.White;
			tienichMenu.Name = "tienichMenu";
			tienichMenu.Size = new System.Drawing.Size(67, 24);
			tienichMenu.Text = "Tiện Ích";
			toolStripMenuItem_1.BackColor = System.Drawing.Color.Sienna;
			toolStripMenuItem_1.ForeColor = System.Drawing.Color.White;
			toolStripMenuItem_1.Name = "thôngTinTàiKhoảnToolStripMenuItem";
			toolStripMenuItem_1.Size = new System.Drawing.Size(196, 22);
			toolStripMenuItem_1.Text = "Thông tin tài khoản";
			toolStripMenuItem_1.Click += new System.EventHandler(toolStripMenuItem_1_Click);
			naptheItem.BackColor = System.Drawing.Color.Sienna;
			naptheItem.ForeColor = System.Drawing.Color.White;
			naptheItem.Name = "naptheItem";
			naptheItem.Size = new System.Drawing.Size(196, 22);
			naptheItem.Text = "Nạp Thẻ";
			naptheItem.Click += new System.EventHandler(naptheItem_Click);
			sukienTuanItem.BackColor = System.Drawing.Color.Sienna;
			sukienTuanItem.ForeColor = System.Drawing.Color.White;
			sukienTuanItem.Name = "sukienTuanItem";
			sukienTuanItem.Size = new System.Drawing.Size(196, 22);
			sukienTuanItem.Text = "Sự Kiện Tuần";
			sukienTuanItem.Click += new System.EventHandler(sukienTuanItem_Click);
			toolStripMenuItem_0.BackColor = System.Drawing.Color.Sienna;
			toolStripMenuItem_0.ForeColor = System.Drawing.Color.White;
			toolStripMenuItem_0.Name = "xóaCacheToolStripMenuItem";
			toolStripMenuItem_0.Size = new System.Drawing.Size(196, 22);
			toolStripMenuItem_0.Text = "Xóa Cache";
			toolStripMenuItem_0.Click += new System.EventHandler(hAmGloXtwT);
			chonmaychuItemMenuTxt.BackColor = System.Drawing.Color.Sienna;
			chonmaychuItemMenuTxt.ForeColor = System.Drawing.Color.White;
			chonmaychuItemMenuTxt.Name = "chonmaychuItemMenuTxt";
			chonmaychuItemMenuTxt.Size = new System.Drawing.Size(196, 22);
			chonmaychuItemMenuTxt.Text = "Đổi Máy Chủ";
			chonmaychuItemMenuTxt.Click += new System.EventHandler(chonmaychuItemMenuTxt_Click);
			txtRue.ImageScalingSize = new System.Drawing.Size(20, 20);
			txtRue.Location = new System.Drawing.Point(0, 0);
			txtRue.Name = "txtRue";
			txtRue.Size = new System.Drawing.Size(200, 24);
			txtRue.TabIndex = 0;
			timer_1.Interval = 10000;
			timer_2.Interval = 10;
			timer_2.Tick += new System.EventHandler(timer_2_Tick);
			panel_Playgame.BackColor = System.Drawing.SystemColors.AppWorkspace;
			panel_Playgame.Dock = System.Windows.Forms.DockStyle.Fill;
			panel_Playgame.Location = new System.Drawing.Point(0, 28);
			panel_Playgame.Name = "panel_Playgame";
			panel_Playgame.Size = new System.Drawing.Size(1000, 643);
			panel_Playgame.TabIndex = 2;
			exitBtn.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			exitBtn.Image = (System.Drawing.Image)resources.GetObject("exitBtn.Image");
			exitBtn.Name = "exitBtn";
			exitBtn.Size = new System.Drawing.Size(32, 24);
			exitBtn.Click += new System.EventHandler(exitBtn_Click);
			maxminBtn.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			maxminBtn.Image = (System.Drawing.Image)resources.GetObject("maxminBtn.Image");
			maxminBtn.Name = "maxminBtn";
			maxminBtn.Size = new System.Drawing.Size(32, 24);
			maxminBtn.Click += new System.EventHandler(maxminBtn_Click);
			miniBtn.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			miniBtn.Image = (System.Drawing.Image)resources.GetObject("miniBtn.Image");
			miniBtn.Name = "miniBtn";
			miniBtn.Size = new System.Drawing.Size(32, 24);
			miniBtn.Click += new System.EventHandler(miniBtn_Click);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(1000, 671);
			base.Controls.Add(panel_Playgame);
			base.Controls.Add(menuStrip);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.MainMenuStrip = menuStrip;
			base.Name = "PlayGameFrm2FIB";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Chơi Game - "+ ApplicationConfig.ServerTitle;
			base.Activated += new System.EventHandler(PlayGameFrm2FIB_Activated);
			menuStrip.ResumeLayout(false);
			menuStrip.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		internal static bool JrIyETby4AkqfNG8AtJ()
		{
			return GP7T9hb2NsfeA1JGaQ3 == null;
		}
	}
}
