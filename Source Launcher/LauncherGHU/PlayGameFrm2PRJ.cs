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
	public class PlayGameFrm2PRJ : Form
	{
		public static bool IsUpdate;

		public string NameServer = null;

		private string string_0;

		private Point point_0;

		private SelectServer selectServer_Control;

		private Recharge recharge_Control = null;

		private Account account_Control = null;

		private PlayGameEmmbed playGame_Control = null;

		private System.Threading.Timer timer_0;

		public const int WM_NCLBUTTONDOWN = 161;

		public const int HT_CAPTION = 2;

		private Thread thread_0;

		private static Process process_0;

		private IContainer icontainer_0 = null;

		private MenuStrip menuStrip1;

		private MenuStrip txtRue;

		private ToolStripMenuItem doimaychuBtn;

		private ToolStripMenuItem tienichMenu;

		private System.Windows.Forms.Timer timer_1;

		private ToolStripMenuItem bangcongthucBtn;

		private System.Windows.Forms.Timer timer_2;

		private Panel panel_Playgame;

		private ToolStripMenuItem toolStripMenuItem_0;

		public ToolStripMenuItem naptheItem;

		private ToolStripMenuItem toolStripMenuItem_1;

		private ToolStripMenuItem chonmaychuItemMenuTxt;

		private ToolStripMenuItem exitBtn;

		private ToolStripMenuItem miniBtn;

		private ToolStripMenuItem maxminBtn;
        private IContainer components;
        private static PlayGameFrm2PRJ YTEQkcbJ6LqfZoMbVWD;

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassStyle |= 131072;
				return createParams;
			}
		}

		public PlayGameFrm2PRJ(int serverid, string nameserver)
		{
			if (Environment.OSVersion.Version.Major >= 6)
			{
				SetProcessDPIAware();
			}
			InitializeComponent();
			timer_0 = new System.Threading.Timer(ControlMgr.stopCheats, null, 15000, 15000);
			LoginMgr.ServerID = serverid;
			NameServer = nameserver;
			method_1();
			tienichMenu.Text = LoginMgr.Username;
			Text = ApplicationConfig.ServerTitle + " - " + LoginMgr.Username;
			doimaychuBtn.Visible = false;
			method_2();
			method_0();
		}

		private void method_0()
		{
			playGame_Control = new PlayGameEmmbed(this);
			playGame_Control.Dock = DockStyle.Fill;
			playGame_Control.Location = new Point(0, 32);
			playGame_Control.Name = "playGame_Control";
			playGame_Control.Size = new Size(1000, 644);
			playGame_Control.TabIndex = 10;
			playGame_Control.Visible = true;
			panel_Playgame.Controls.Add(playGame_Control);
		}

		private void method_1()
		{
			string_0 = ControlMgr.GetCoin();
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

		private void menuStrip1_MouseDown(object sender, MouseEventArgs e)
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
			if (!base.InvokeRequired)
			{
				if (isLogout)
				{
					LoginMgr.Logout();
				}
				Close();
				thread_0 = new Thread(method_6);
				thread_0.SetApartmentState(ApartmentState.STA);
				thread_0.Start();
			}
			else
			{
				Invoke((MethodInvoker)delegate
				{
					CloseAndBackLogin(isLogout);
				});
			}
		}

		private void method_6()
		{
			Application.Run(new formLogin());
		}

		public void SetTitleForm(string nameserver)
		{
			NameServer = nameserver;
			Text = $"{ApplicationConfig.ServerTitle} | {LoginMgr.Username} - {NameServer}";
		}

		[DllImport("user32.dll")]
		private static extern int SetWindowText(IntPtr intptr_0, string string_1);

		public void InstallGameWebBrowse()
		{
			playGame_Control.BringToFront();
			doimaychuBtn.Text = "Đổi Máy Chủ";
			chonmaychuItemMenuTxt.Text = "Đổi Máy Chủ";
			doimaychuBtn.Visible = true;
			string[] flashConfigs = ControlMgr.getFlashConfigs();
			//string url = flashConfigs[0] + "?enterCode=" + ApplicationConfig.KeyCodeLauncher + "&" + flashConfigs[1];
			string url = ApplicationConfig.LoadingSwf + "?enterCode=" + ApplicationConfig.KeyCodeLauncher + "&" + flashConfigs[1];
			try
			{
				playGame_Control.PlayLink(url);
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

		private void PlayGameFrm2PRJ_Activated(object sender, EventArgs e)
		{
		}

		private void method_7(object sender, EventArgs e)
		{
			if (MessageBox.Show("Bạn có muốn đăng xuất không ?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				if (playGame_Control != null)
				{
					playGame_Control.CloseFlash();
				}
				CloseAndBackLogin(isLogout: true);
			}
		}

		private void toolStripMenuItem_1_Click(object sender, EventArgs e)
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
				method_8();
			}
			recharge_Control.BringToFront();
			doimaychuBtn.Text = "Quay Lại";
			chonmaychuItemMenuTxt.Text = "Quay Lại";
		}

		private void method_8()
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

		private void toolStripMenuItem_0_Click(object sender, EventArgs e)
		{
			if (account_Control == null)
			{
				method_9();
			}
			account_Control.BringToFront();
			doimaychuBtn.Text = "Quay Lại";
			chonmaychuItemMenuTxt.Text = "Quay Lại";
		}

		private void method_9()
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

		private void chonmaychuItemMenuTxt_Click(object sender, EventArgs e)
		{
			if (chonmaychuItemMenuTxt.Text == "Đổi Máy Chủ")
			{
				selectServer_Control.BringToFront();
				doimaychuBtn.Text = "Quay Lại";
				chonmaychuItemMenuTxt.Text = "Quay Lại";
			}
			else
			{
				playGame_Control.BringToFront();
				doimaychuBtn.Text = "Đổi Máy Chủ";
				chonmaychuItemMenuTxt.Text = "Đổi Máy Chủ";
			}
		}

		private void method_10(object sender, EventArgs e)
		{
			doimaychuBtn.Text = "Đổi Máy Chủ";
		}

		private void method_11(object sender, EventArgs e)
		{
			ControlMgr.stopCheats();
		}

		private void method_12(object sender, EventArgs e)
		{
			Application.Run(new PlayGameFrmFIB(LoginMgr.ServerID, NameServer));
		}

		private void bangcongthucBtn_Click(object sender, EventArgs e)
		{
			CongThucFrm congThucFrm = new CongThucFrm();
			congThucFrm.Show();
			congThucFrm.Location = new Point(base.Location.X + 830, base.Location.Y + 30);
			congThucFrm.TopMost = true;
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

		private void method_13(object sender, EventArgs e)
		{
			selectServer_Control.BringToFront();
		}

		private void PlayGameFrm2PRJ_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (playGame_Control != null)
			{
				playGame_Control.CloseFlash();
			}
		}

		private void maxminBtn_Click(object sender, EventArgs e)
		{
			if (base.WindowState != 0)
			{
				base.WindowState = FormWindowState.Normal;
			}
			else
			{
				base.WindowState = FormWindowState.Maximized;
			}
			if (playGame_Control != null)
			{
				playGame_Control.SetWindow();
			}
		}

		private void miniBtn_Click(object sender, EventArgs e)
		{
			base.WindowState = FormWindowState.Minimized;
		}

		private void exitBtn_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Bạn có muốn đăng xuất không ?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				CloseAndBackLogin(isLogout: true);
			}
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayGameFrm2PRJ));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.doimaychuBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.exitBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.maxminBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.miniBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.tienichMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_0 = new System.Windows.Forms.ToolStripMenuItem();
            this.naptheItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_1 = new System.Windows.Forms.ToolStripMenuItem();
            this.chonmaychuItemMenuTxt = new System.Windows.Forms.ToolStripMenuItem();
            this.bangcongthucBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.txtRue = new System.Windows.Forms.MenuStrip();
            this.timer_1 = new System.Windows.Forms.Timer(this.components);
            this.timer_2 = new System.Windows.Forms.Timer(this.components);
            this.panel_Playgame = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Sienna;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.doimaychuBtn,
            this.exitBtn,
            this.maxminBtn,
            this.miniBtn,
            this.tienichMenu,
            this.bangcongthucBtn});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStrip1.Size = new System.Drawing.Size(1333, 30);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.menuStrip1_MouseDown);
            // 
            // doimaychuBtn
            // 
            this.doimaychuBtn.BackColor = System.Drawing.Color.Sienna;
            this.doimaychuBtn.ForeColor = System.Drawing.Color.White;
            this.doimaychuBtn.Name = "doimaychuBtn";
            this.doimaychuBtn.Size = new System.Drawing.Size(108, 26);
            this.doimaychuBtn.Text = "Đổi Máy Chủ";
            this.doimaychuBtn.Click += new System.EventHandler(this.doimaychuBtn_Click);
            // 
            // exitBtn
            // 
            this.exitBtn.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.exitBtn.Image = ((System.Drawing.Image)(resources.GetObject("exitBtn.Image")));
            this.exitBtn.Name = "exitBtn";
            this.exitBtn.Size = new System.Drawing.Size(34, 26);
            this.exitBtn.Click += new System.EventHandler(this.exitBtn_Click);
            // 
            // maxminBtn
            // 
            this.maxminBtn.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.maxminBtn.Image = ((System.Drawing.Image)(resources.GetObject("maxminBtn.Image")));
            this.maxminBtn.Name = "maxminBtn";
            this.maxminBtn.Size = new System.Drawing.Size(34, 26);
            this.maxminBtn.Click += new System.EventHandler(this.maxminBtn_Click);
            // 
            // miniBtn
            // 
            this.miniBtn.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.miniBtn.Image = ((System.Drawing.Image)(resources.GetObject("miniBtn.Image")));
            this.miniBtn.Name = "miniBtn";
            this.miniBtn.Size = new System.Drawing.Size(34, 26);
            this.miniBtn.Click += new System.EventHandler(this.miniBtn_Click);
            // 
            // tienichMenu
            // 
            this.tienichMenu.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tienichMenu.BackColor = System.Drawing.Color.Sienna;
            this.tienichMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_0,
            this.naptheItem,
            this.toolStripMenuItem_1,
            this.chonmaychuItemMenuTxt});
            this.tienichMenu.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tienichMenu.ForeColor = System.Drawing.Color.White;
            this.tienichMenu.Name = "tienichMenu";
            this.tienichMenu.Size = new System.Drawing.Size(82, 26);
            this.tienichMenu.Text = "Tiện Ích";
            // 
            // toolStripMenuItem_0
            // 
            this.toolStripMenuItem_0.BackColor = System.Drawing.Color.Sienna;
            this.toolStripMenuItem_0.ForeColor = System.Drawing.Color.White;
            this.toolStripMenuItem_0.Name = "toolStripMenuItem_0";
            this.toolStripMenuItem_0.Size = new System.Drawing.Size(232, 26);
            this.toolStripMenuItem_0.Text = "Thông tin tài khoản";
            this.toolStripMenuItem_0.Click += new System.EventHandler(this.toolStripMenuItem_0_Click);
            // 
            // naptheItem
            // 
            this.naptheItem.BackColor = System.Drawing.Color.Sienna;
            this.naptheItem.ForeColor = System.Drawing.Color.White;
            this.naptheItem.Name = "naptheItem";
            this.naptheItem.Size = new System.Drawing.Size(232, 26);
            this.naptheItem.Text = "Nạp Thẻ";
            this.naptheItem.Click += new System.EventHandler(this.naptheItem_Click);
            // 
            // toolStripMenuItem_1
            // 
            this.toolStripMenuItem_1.BackColor = System.Drawing.Color.Sienna;
            this.toolStripMenuItem_1.ForeColor = System.Drawing.Color.White;
            this.toolStripMenuItem_1.Name = "toolStripMenuItem_1";
            this.toolStripMenuItem_1.Size = new System.Drawing.Size(232, 26);
            this.toolStripMenuItem_1.Text = "Xóa Cache";
            this.toolStripMenuItem_1.Click += new System.EventHandler(this.toolStripMenuItem_1_Click);
            // 
            // chonmaychuItemMenuTxt
            // 
            this.chonmaychuItemMenuTxt.BackColor = System.Drawing.Color.Sienna;
            this.chonmaychuItemMenuTxt.ForeColor = System.Drawing.Color.White;
            this.chonmaychuItemMenuTxt.Name = "chonmaychuItemMenuTxt";
            this.chonmaychuItemMenuTxt.Size = new System.Drawing.Size(232, 26);
            this.chonmaychuItemMenuTxt.Text = "Đổi Máy Chủ";
            this.chonmaychuItemMenuTxt.Click += new System.EventHandler(this.chonmaychuItemMenuTxt_Click);
            // 
            // bangcongthucBtn
            // 
            this.bangcongthucBtn.BackColor = System.Drawing.Color.Sienna;
            this.bangcongthucBtn.ForeColor = System.Drawing.Color.White;
            this.bangcongthucBtn.Name = "bangcongthucBtn";
            this.bangcongthucBtn.Size = new System.Drawing.Size(132, 26);
            this.bangcongthucBtn.Text = "Bảng Công Thức";
            this.bangcongthucBtn.Click += new System.EventHandler(this.bangcongthucBtn_Click);
            // 
            // txtRue
            // 
            this.txtRue.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.txtRue.Location = new System.Drawing.Point(0, 0);
            this.txtRue.Name = "txtRue";
            this.txtRue.Size = new System.Drawing.Size(200, 24);
            this.txtRue.TabIndex = 0;
            // 
            // timer_1
            // 
            this.timer_1.Interval = 10000;
            // 
            // timer_2
            // 
            this.timer_2.Interval = 10;
            this.timer_2.Tick += new System.EventHandler(this.timer_2_Tick);
            // 
            // panel_Playgame
            // 
            this.panel_Playgame.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel_Playgame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Playgame.Location = new System.Drawing.Point(0, 30);
            this.panel_Playgame.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel_Playgame.Name = "panel_Playgame";
            this.panel_Playgame.Size = new System.Drawing.Size(1333, 796);
            this.panel_Playgame.TabIndex = 2;
            // 
            // PlayGameFrm2PRJ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1333, 826);
            this.Controls.Add(this.panel_Playgame);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "PlayGameFrm2PRJ";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chơi Game - Gunny Lộc Phát";
            this.Activated += new System.EventHandler(this.PlayGameFrm2PRJ_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PlayGameFrm2PRJ_FormClosed);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		internal static void unyS4ub9LLnykFOpgIU()
		{
		}

		internal static bool KBHq5eb8CoLfWBRg6gk()
		{
			return YTEQkcbJ6LqfZoMbVWD == null;
		}
	}
}
