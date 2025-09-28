using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using FontAwesome.Sharp;
using LauncherGHU.ControlsForm;
using LauncherGHU.Properties;
using ns1;

namespace LauncherGHU
{
	public class PlayGameFrmFIB : Form
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct Struct5
		{
			public static Color color_0;

			public static Color color_1;

			public static Color color_2;

			public static Color color_3;

			public static Color color_4;

			public static Color color_5;

			internal static object R7lCc3LRoqvkaL2c0yR;

			static Struct5()
			{
				color_0 = Color.FromArgb(172, 126, 241);
				color_1 = Color.FromArgb(249, 118, 176);
				color_2 = Color.FromArgb(253, 138, 114);
				color_3 = Color.FromArgb(95, 77, 221);
				color_4 = Color.FromArgb(249, 88, 155);
				color_5 = Color.FromArgb(24, 161, 251);
			}

			internal static void Q2BrlqLjLIlN17MNhWi()
			{
			}

			internal static bool FwanFZLAYLHNh8ikLaw()
			{
				return R7lCc3LRoqvkaL2c0yR == null;
			}
		}

		public static class User32Interop
		{
			internal static object t8glmfLrtN3C5MF9ScP;

			public static char ToAscii(Keys key, Keys modifiers)
			{
				StringBuilder stringBuilder = new StringBuilder(2);
				int num = ToAscii_1((uint)key, 0u, smethod_0(modifiers), stringBuilder, 0u);
				if (num != 1)
				{
					throw new Exception("Invalid key");
				}
				return stringBuilder[0];
			}

			private static byte[] smethod_0(Keys keys_0)
			{
				byte[] array = new byte[256];
				foreach (Keys value in Enum.GetValues(typeof(Keys)))
				{
					if ((keys_0 & value) == value)
					{
						try
						{
							array[(int)value] = 128;
						}
						catch
						{
						}
					}
				}
				return array;
			}

			[DllImport("user32.dll", EntryPoint = "ToAscii")]
			private static extern int ToAscii_1(uint uint_0, uint uint_1, byte[] byte_0, [Out] StringBuilder stringBuilder_0, uint uint_2);

			internal static bool gCDXuDLQ7sUMVm8DwxA()
			{
				return t8glmfLrtN3C5MF9ScP == null;
			}
		}

		private static Process process_0;

		private IconButton iconButton_0;

		private IconPictureBox iconPictureBox_0;

		private Label label_0;

		private Panel panel_0;

		public static bool IsUpdate;

		private string string_0 = null;

		private string string_1 = "High";

		private bool bool_0 = true;

		private System.Drawing.Point point_0;

		private System.Drawing.Size size_0;

		private System.Drawing.Size size_1;

		private System.Drawing.Size size_2;

		private System.Drawing.Point point_1;

		private PlayGame playGame_Control_1;

		private SelectServer selectServer_Control;

		private Recharge recharge_Control = null;

		private Account account_Control = null;

		private System.Threading.Timer timer_0;

		public const int WM_NCLBUTTONDOWN = 161;

		public const int HT_CAPTION = 2;

		private Thread thread_0;

		private Thread thread_1;

		private IContainer icontainer_0 = null;

		private ToolTip toolTip_0;

		public IconButton sukientuanBtn;

		private IconButton dangxuatBtn;

		private IconButton doimaychuBtn;

		private IconButton doithongtinBtn;

		public IconButton naptheBtn;

		private IconButton playBtn;

		private System.Windows.Forms.Timer timer_1;

		private SaveFileDialog saveFileDialog_0;

		private Panel panel_Playgame;

		private IconPictureBox giaodienthapbtn;

		private IconPictureBox maxBtn;

		private IconPictureBox reloadBtn;

		private IconPictureBox deleteCacheBtn;

		private IconPictureBox congthucBtn;

		private IconPictureBox miniBtn;

		private IconPictureBox exitBtn;

		private Label menuLefttxt;

		private Panel qualityPanel;

		private Label thapLb;

		private RadioButton caoR;

		private RadioButton thuongR;

		private Label thuongLb;

		private RadioButton thapR;

		private Label caoLb;

		private Label label4;
        private IContainer components;
        public IconButton iconButton1;
        private Panel bunipanelLeft;
        private IconPictureBox reloadCoin;
        private Label label3;
        private Label label7;
        private IconPictureBox iconPictureBox1;
        private Label coinTxt;
        private Label label1;
        private IconPictureBox anMenuleftBtn;
        private Panel menuRighttop;
        internal static PlayGameFrmFIB LDVqCbfmZEYcYpQluFv;

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassStyle |= 131072;
				return createParams;
			}
		}

		public PlayGameFrmFIB(int serverid, string nameserver)
		{
			if (Environment.OSVersion.Version.Major >= 6)
			{
				SetProcessDPIAware();
			}
			InitializeComponent();
			method_3();
			timer_0 = new System.Threading.Timer(ControlMgr.stopCheats, null, 15000, 15000);
			LoginMgr.ServerID = serverid;
			string_0 = nameserver;
			if (GetWindowsScaling() >= 125)
			{
				playBtn.Font = (doithongtinBtn.Font = (naptheBtn.Font = (doimaychuBtn.Font = (sukientuanBtn.Font = (dangxuatBtn.Font = new Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0))))));
				label7.Font = (label3.Font = new Font("Tahoma", 10f, System.Drawing.FontStyle.Underline, GraphicsUnit.Point, 0));
				label1.Font = (coinTxt.Font = new Font("Tahoma", 10f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0));
				thapR.Font = (thuongR.Font = (caoR.Font = new Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, GraphicsUnit.Point, 0)));
				thapLb.Font = (label4.Font = (thuongLb.Font = (caoLb.Font = new Font("Tahoma", 6.5f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0))));
			}
			method_21();
			method_4();
			method_2();
		}

		private void method_0()
		{
			if (recharge_Control == null)
			{
				recharge_Control = new Recharge();
				recharge_Control.Dock = DockStyle.Fill;
				recharge_Control.Location = new System.Drawing.Point(0, 32);
				recharge_Control.Name = "recharge_Control";
				recharge_Control.Size = new System.Drawing.Size(1000, 644);
				recharge_Control.TabIndex = 0;
				recharge_Control.Visible = true;
				panel_Playgame.Controls.Add(recharge_Control);
			}
		}

		private void method_1()
		{
			if (account_Control == null)
			{
				account_Control = new Account();
				account_Control.Dock = DockStyle.Fill;
				account_Control.Location = new System.Drawing.Point(0, 32);
				account_Control.Name = "account_Control";
				account_Control.Size = new System.Drawing.Size(1000, 644);
				account_Control.TabIndex = 0;
				account_Control.Visible = true;
				panel_Playgame.Controls.Add(account_Control);
			}
		}

		private void method_2()
		{
			try
			{
				playGame_Control_1 = new PlayGame(this);
				playGame_Control_1.Dock = DockStyle.Fill;
				playGame_Control_1.Location = new System.Drawing.Point(0, 32);
				playGame_Control_1.Name = "playGame_Control";
				playGame_Control_1.Size = new System.Drawing.Size(1000, 644);
				playGame_Control_1.TabIndex = 10;
				playGame_Control_1.Visible = true;
				panel_Playgame.Controls.Add(playGame_Control_1);
			}
			catch (Exception ex)
			{
				ControlMgr.UpLogLauncher(ex.ToString() + "- (SetupPlayGameControl1)- Now Version : " + Assembly.GetEntryAssembly().GetName().Version);
				Settings.Default.ErrorLoadDll = true;
				Settings.Default.Save();
				try
				{
					string text = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location) + "\\";
					ControlMgr.CreateFileIfNotFound("gunhoiuc", text, "ghu");
					Directory.SetCurrentDirectory(text);
					playGame_Control_1 = new PlayGame(this);
					playGame_Control_1.Dock = DockStyle.Fill;
					playGame_Control_1.Location = new System.Drawing.Point(0, 32);
					playGame_Control_1.Name = "playGame_Control";
					playGame_Control_1.Size = new System.Drawing.Size(1000, 644);
					playGame_Control_1.TabIndex = 10;
					playGame_Control_1.Visible = true;
					panel_Playgame.Controls.Add(playGame_Control_1);
				}
				catch (Exception ex2)
				{
					ControlMgr.UpLogLauncher(ex2.ToString() + "- (SetupPlayGameControl2) Now Version : " + Assembly.GetEntryAssembly().GetName().Version);
					System.Windows.MessageBox.Show("Không thể khởi tạo tài nguyên", "Lỗi");
				}
			}
			base.ActiveControl = playGame_Control_1;
		}

		private void method_3()
		{
			selectServer_Control = new SelectServer(this);
			selectServer_Control.Dock = DockStyle.Fill;
			selectServer_Control.Location = new System.Drawing.Point(0, 32);
			selectServer_Control.Name = "selectServer_Control";
			selectServer_Control.Size = new System.Drawing.Size(1000, 644);
			selectServer_Control.TabIndex = 10;
			selectServer_Control.Visible = true;
			panel_Playgame.Controls.Add(selectServer_Control);
		}

		private void method_4()
		{
			point_0 = iconPictureBox1.Location;
			size_0 = iconPictureBox1.Size;
			size_1 = bunipanelLeft.Size;
			point_1 = anMenuleftBtn.Location;
			size_2 = base.Size;
			base.WindowState = FormWindowState.Normal;
			panel_0 = new Panel();
			panel_0.Size = new System.Drawing.Size(5, 50);
			bunipanelLeft.Controls.Add(panel_0);
			base.FormBorderStyle = FormBorderStyle.None;
			BackColor = Color.Gold;
			base.TransparencyKey = Color.Gold;
			label1.Text = LoginMgr.Username;
			toolTip_0.SetToolTip(dangxuatBtn, "Đăng xuất");
			toolTip_0.SetToolTip(deleteCacheBtn, "Xóa cache");
			toolTip_0.SetToolTip(naptheBtn, "Nạp Thẻ");
			toolTip_0.SetToolTip(miniBtn, "Ẩn Launcher");
			toolTip_0.SetToolTip(exitBtn, "Thoát khỏi Launcher");
			SetStyle(ControlStyles.ResizeRedraw, value: true);
			if (LoginMgr.ServerID != 0)
			{
				InstallGameWebBrowse();
			}
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 132)
			{
				System.Drawing.Point p = new System.Drawing.Point(m.LParam.ToInt32());
				p = PointToClient(p);
				if (p.Y < 32)
				{
					m.Result = (IntPtr)2;
					return;
				}
				if (p.X >= base.ClientSize.Width - 16 && p.Y >= base.ClientSize.Height - 16)
				{
					m.Result = (IntPtr)17;
					return;
				}
			}
			base.WndProc(ref m);
		}

		[DllImport("user32.dll")]
		private static extern bool SetProcessDPIAware();

		public static int GetWindowsScaling()
		{
			return (int)((double)(100 * Screen.PrimaryScreen.Bounds.Width) / SystemParameters.PrimaryScreenWidth);
		}

		public void InstallGameWebBrowse()
		{
			try
			{
				doimaychuBtn.Visible = true;
				sukientuanBtn.Visible = true;
				if (!sukientuanBtn.Enabled)
				{
					sukientuanBtn.Enabled = true;
				}
				if (playGame_Control_1 == null)
				{
					method_2();
				}
				playGame_Control_1.PlayURLMovie();
				playGame_Control_1.BringToFront();
				qualityPanel.Visible = true;
				playBtn.PerformClick();
				selectServer_Control.Dispose();
			}
			catch (Exception ex)
			{
				ControlMgr.UpLogLauncher(ex.Message.ToString() + "-(InstallGameWebBrowse)- Now Version : " + Assembly.GetEntryAssembly().GetName().Version);
				System.Windows.Forms.MessageBox.Show("Xảy ra lỗi tài nguyên , vui lòng khởi động lại Launcher");
				System.Windows.Forms.Application.Restart();
			}
		}

		public void SetTitleForm(string nameserver)
		{
			menuLefttxt.Text = $"{ApplicationConfig.ServerTitle} | {LoginMgr.Username} - {nameserver}";
		}

		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

		[DllImport("user32.dll")]
		public static extern bool ReleaseCapture();

		private void method_5(MouseEventArgs mouseEventArgs_0)
		{
			if (mouseEventArgs_0.Button == MouseButtons.Left)
			{
				ReleaseCapture();
				SendMessage(base.Handle, 161, 2, 0);
			}
		}

		private void PlayGameFrmFIB_MouseDown(object sender, MouseEventArgs e)
		{
			method_5(e);
		}

		private void method_6(object sender, EventArgs e)
		{
			base.WindowState = FormWindowState.Minimized;
		}

		private void method_7(object sender, EventArgs e)
		{
			if (System.Windows.Forms.MessageBox.Show("Bạn có chắc là muốn thoát?", "Thoát ứng dụng", MessageBoxButtons.YesNo, MessageBoxIcon.Hand) == DialogResult.Yes)
			{
				timer_1.Start();
			}
		}

		private void method_8(object sender, EventArgs e)
		{
			if (System.Windows.Forms.MessageBox.Show("Bạn có muốn xóa toàn bộ cache và đăng xuất ?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
				thread_0 = new Thread(method_9);
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

		private void method_9()
		{
			System.Windows.Forms.Application.Run(new formLogin());
		}

		private void method_10(object sender, EventArgs e)
		{
			if (System.Windows.Forms.MessageBox.Show("Bạn có chắc là muốn thoát đăng nhập?", "Thoát đăng nhập", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				CloseAndBackLogin(isLogout: true);
			}
		}

		private void method_11(object sender, EventArgs e)
		{
			InstallGameWebBrowse();
		}

		private void method_12(object sender, EventArgs e)
		{
			base.WindowState = FormWindowState.Maximized;
		}

		private void PlayGameFrmFIB_Load(object sender, EventArgs e)
		{
		}

		private void method_13(object sender, EventArgs e)
		{
			label1.ForeColor = Color.White;
		}

		private void method_14(object sender, EventArgs e)
		{
			label1.ForeColor = Color.Orange;
		}

		[DllImport("user32.dll")]
		public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

		private void method_15(object sender, EventArgs e)
		{
			if (base.WindowState != FormWindowState.Maximized)
			{
				base.WindowState = FormWindowState.Maximized;
			}
			else
			{
				base.WindowState = FormWindowState.Normal;
			}
		}

		private void method_16(object sender, EventArgs e)
		{
			ControlMgr.OpenWebsite(ApplicationConfig.FanpageLink);
		}

		private void miniBtn_Click(object sender, EventArgs e)
		{
			base.WindowState = FormWindowState.Minimized;
		}

		private void exitBtn_Click(object sender, EventArgs e)
		{
			if (System.Windows.Forms.MessageBox.Show("Bạn có chắc là muốn thoát?", "Thoát ứng dụng", MessageBoxButtons.YesNo, MessageBoxIcon.Hand) == DialogResult.Yes)
			{
				timer_1.Start();
			}
		}

		private void anMenuleftBtn_Click(object sender, EventArgs e)
		{
			if (!bool_0)
			{
				bool_0 = true;
				iconPictureBox1.Size = size_0;
				iconPictureBox1.Location = point_0;
				anMenuleftBtn.Location = point_1;
				Label label = label3;
				label7.Visible = true;
				label.Visible = true;
				bunipanelLeft.Size = size_1;
				method_17(bool_1: false);
			}
			else
			{
				bool_0 = false;
				iconPictureBox1.Size = new System.Drawing.Size(45, 45);
				iconPictureBox1.Location = new System.Drawing.Point(-8, 46);
				anMenuleftBtn.Location = new System.Drawing.Point(4, 4);
				Label label2 = label3;
				label7.Visible = false;
				label2.Visible = false;
				bunipanelLeft.Size = new System.Drawing.Size(40, 657);
				method_17(bool_1: true);
			}
		}

		private void method_17(bool bool_1)
		{
			int num = 7;
			if (!bool_1)
			{
				playBtn.Location = new System.Drawing.Point(playBtn.Location.X + num, playBtn.Location.Y);
				naptheBtn.Location = new System.Drawing.Point(naptheBtn.Location.X + num, naptheBtn.Location.Y);
				doithongtinBtn.Location = new System.Drawing.Point(doithongtinBtn.Location.X + num, doithongtinBtn.Location.Y);
				sukientuanBtn.Location = new System.Drawing.Point(sukientuanBtn.Location.X + num, sukientuanBtn.Location.Y);
				doimaychuBtn.Location = new System.Drawing.Point(doimaychuBtn.Location.X + num, doimaychuBtn.Location.Y);
				dangxuatBtn.Location = new System.Drawing.Point(dangxuatBtn.Location.X + num, dangxuatBtn.Location.Y);
				base.Size = size_2;
			}
			else
			{
				playBtn.Location = new System.Drawing.Point(playBtn.Location.X - num, playBtn.Location.Y);
				naptheBtn.Location = new System.Drawing.Point(naptheBtn.Location.X - num, naptheBtn.Location.Y);
				doithongtinBtn.Location = new System.Drawing.Point(doithongtinBtn.Location.X - num, doithongtinBtn.Location.Y);
				sukientuanBtn.Location = new System.Drawing.Point(sukientuanBtn.Location.X - num, sukientuanBtn.Location.Y);
				doimaychuBtn.Location = new System.Drawing.Point(doimaychuBtn.Location.X - num, doimaychuBtn.Location.Y);
				dangxuatBtn.Location = new System.Drawing.Point(dangxuatBtn.Location.X - num, dangxuatBtn.Location.Y);
				base.Size = new System.Drawing.Size(base.Width - 165, base.Height);
			}
		}

		private void miniBtn_MouseEnter(object sender, EventArgs e)
		{
			miniBtn.BackColor = Color.FromArgb(242, 214, 73);
		}

		private void miniBtn_MouseLeave(object sender, EventArgs e)
		{
			miniBtn.BackColor = Color.Transparent;
		}

		private void exitBtn_MouseEnter(object sender, EventArgs e)
		{
			exitBtn.BackColor = Color.FromArgb(242, 90, 56);
		}

		private void exitBtn_MouseLeave(object sender, EventArgs e)
		{
			exitBtn.BackColor = Color.Transparent;
		}

		private void playBtn_Click(object sender, EventArgs e)
		{
			method_18(sender, Color.White);
			if (LoginMgr.ServerID != 0)
			{
				playGame_Control_1.BringToFront();
				qualityPanel.Visible = true;
				playGame_Control_1.Select();
				playGame_Control_1.Focus();
			}
			else
			{
				qualityPanel.Visible = false;
				selectServer_Control.BringToFront();
			}
		}

		private void doimaychuBtn_Click(object sender, EventArgs e)
		{
			method_18(sender, Color.White);
			if (selectServer_Control.IsDisposed)
			{
				method_3();
			}
			selectServer_Control.BringToFront();
		}

		private void method_18(object object_0, Color color_0)
		{
			if (object_0 != null)
			{
				method_19();
				iconButton_0 = (IconButton)object_0;
				iconButton_0.BackColor = Color.FromArgb(158, 189, 255);
				iconButton_0.ForeColor = color_0;
			}
		}

		private void method_19()
		{
			if (iconButton_0 != null)
			{
				iconButton_0.BackColor = Color.Transparent;
				iconButton_0.ForeColor = Color.White;
				iconButton_0.IconColor = Color.White;
			}
		}

		private void naptheBtn_Click(object sender, EventArgs e)
		{
            //if (recharge_Control == null)
            //{
            //    method_0();
            //}
            //method_18(sender, Color.White);
            //recharge_Control.BringToFront();
            ControlMgr.OpenWebsite(ApplicationConfig.UrlRecharge);
        }

		private void method_20(object sender, EventArgs e)
		{
			method_18(sender, Color.White);
		}

		private void dangxuatBtn_Click(object sender, EventArgs e)
		{
			if (System.Windows.Forms.MessageBox.Show("Bạn có chắc là muốn thoát đăng nhập?", "Thoát đăng nhập", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				CloseAndBackLogin(isLogout: true);
			}
		}

		private void method_21()
		{
			string coin = ControlMgr.GetCoin();
			coinTxt.Text = "Coin : " + coin;
		}

		private void deleteCacheBtn_Click(object sender, EventArgs e)
		{
			//if (System.Windows.Forms.MessageBox.Show("Bạn có muốn xóa toàn bộ cache và đăng xuất không ?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			//{
			//	Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 4351");
			//	CloseAndBackLogin(isLogout: true);
			//}
			if (System.Windows.Forms.MessageBox.Show("Bạn có muốn xóa toàn bộ cache và đăng xuất không ?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				//Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 4351");
				//Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 255");
				//DirectoryInfo di = new DirectoryInfo(this.APPLICATION_DATA_FOLDER + "\\Adobe\\Flash Player");
				//DirectoryInfo di2 = new DirectoryInfo(this.APPLICATION_DATA_FOLDER + "\\Macromedia\\Flash Player");
				//this.emptyFolder(di);
				//this.emptyFolder(di2);
				string AdobeFlash = this.APPLICATION_DATA_FOLDER + "\\Adobe\\Flash Player";
				string MacromediaFlash = this.APPLICATION_DATA_FOLDER + "\\Macromedia\\Flash Player";
				if (Directory.Exists(AdobeFlash))
				{
					Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 255");
					RemoveDirectories(AdobeFlash);
				}
				if (Directory.Exists(MacromediaFlash))
				{
					Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 255");
					RemoveDirectories(MacromediaFlash);
				}
				CloseAndBackLogin(isLogout: true);
			}
		}

		private void RemoveDirectories(string strpath)
		{
			foreach (FileSystemInfo file in new DirectoryInfo(strpath).GetFiles())
				file.Delete();
			foreach (DirectoryInfo directory in new DirectoryInfo(strpath).GetDirectories())
				directory.Delete(true);
		}

		private string APPLICATION_DATA_FOLDER = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

		private void emptyFolder(DirectoryInfo di)
		{
			FileInfo[] files = di.GetFiles();
			for (int i = 0; i < files.Length; i++)
			{
				files[i].Delete();
			}
			DirectoryInfo[] directories = di.GetDirectories();
			for (int i = 0; i < directories.Length; i++)
			{
				directories[i].Delete(true);
			}
		}

		private void congthucBtn_Click(object sender, EventArgs e)
		{
			CongThucFrm congThucFrm = new CongThucFrm();
			congThucFrm.Show();
			congThucFrm.Location = new System.Drawing.Point(base.Location.X + 830, base.Location.Y + 30);
			congThucFrm.TopMost = true;
		}

		private void doithongtinBtn_Click(object sender, EventArgs e)
		{
            if (account_Control == null)
            {
                method_1();
            }
            method_18(sender, Color.White);
            account_Control.BringToFront();
            //ControlMgr.OpenWebsite(ApplicationConfig.AccountUrl);
        }

		private void label7_Click(object sender, EventArgs e)
		{
			doithongtinBtn.PerformClick();
		}

		private void label7_MouseEnter(object sender, EventArgs e)
		{
			label7.ForeColor = Color.FromArgb(255, 246, 58);
		}

		private void label7_MouseLeave(object sender, EventArgs e)
		{
			label7.ForeColor = Color.FromArgb(236, 184, 20);
		}

		private void congthucBtn_MouseEnter(object sender, EventArgs e)
		{
			congthucBtn.BackColor = Color.FromArgb(255, 211, 155);
		}

		private void congthucBtn_MouseLeave(object sender, EventArgs e)
		{
			congthucBtn.BackColor = Color.Transparent;
		}

		private void deleteCacheBtn_MouseEnter(object sender, EventArgs e)
		{
			deleteCacheBtn.BackColor = Color.FromArgb(255, 211, 155);
		}

		private void deleteCacheBtn_MouseLeave(object sender, EventArgs e)
		{
			deleteCacheBtn.BackColor = Color.Transparent;
		}

		private void method_22(object sender, EventArgs e)
		{
			deleteCacheBtn.BackColor = Color.FromArgb(255, 211, 155);
		}

		private void method_23(object sender, EventArgs e)
		{
			deleteCacheBtn.BackColor = Color.Transparent;
		}

		private void method_24(object sender, EventArgs e)
		{
			deleteCacheBtn_Click(sender, e);
		}

		private void method_25(object sender, EventArgs e)
		{
			if (bunipanelLeft.Visible)
			{
				base.Width -= bunipanelLeft.Width;
				bunipanelLeft.Visible = false;
			}
			else
			{
				bunipanelLeft.Visible = false;
				anMenuleftBtn.IconColor = Color.White;
			}
		}

		private void caoR_CheckedChanged(object sender, EventArgs e)
		{
			playGame_Control_1.SetQuality("High");
		}

		private void thuongR_CheckedChanged(object sender, EventArgs e)
		{
			playGame_Control_1.SetQuality("Medium");
		}

		private void thapR_CheckedChanged(object sender, EventArgs e)
		{
			playGame_Control_1.SetQuality("Low");
		}

		private void thuongLb_Click(object sender, EventArgs e)
		{
			thuongR.Checked = true;
			thuongR_CheckedChanged(sender, e);
		}

		private void thapLb_Click(object sender, EventArgs e)
		{
			thapR.Checked = true;
			thapR_CheckedChanged(sender, e);
		}

		private void caoLb_Click(object sender, EventArgs e)
		{
			caoR.Checked = true;
			caoR_CheckedChanged(sender, e);
		}

		private void sukientuanBtn_Click(object sender, EventArgs e)
		{
			playBtn.PerformClick();
			playGame_Control_1.CallFlash("CallOpenTopEventWeek");
		}

		private void reloadBtn_MouseEnter(object sender, EventArgs e)
		{
			reloadBtn.BackColor = Color.FromArgb(255, 211, 155);
		}

		private void reloadBtn_MouseLeave(object sender, EventArgs e)
		{
			reloadBtn.BackColor = Color.Transparent;
		}

		private void reloadBtn_Click(object sender, EventArgs e)
		{
			if (System.Windows.Forms.MessageBox.Show("Bạn có muốn tải lại trang ?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				InstallGameWebBrowse();
			}
		}

		private void PlayGameFrmFIB_Activated(object sender, EventArgs e)
		{
			if (playGame_Control_1 != null)
			{
				playGame_Control_1.Focus();
				playGame_Control_1.Select();
			}
		}

		private void label3_MouseEnter(object sender, EventArgs e)
		{
			label3.ForeColor = Color.FromArgb(255, 246, 58);
		}

		private void label3_MouseLeave(object sender, EventArgs e)
		{
			label3.ForeColor = Color.FromArgb(236, 184, 20);
		}

		private void maxBtn_Click(object sender, EventArgs e)
		{
			if (base.WindowState != 0)
			{
				base.WindowState = FormWindowState.Normal;
			}
			else
			{
				base.WindowState = FormWindowState.Maximized;
			}
		}

		private void maxBtn_MouseEnter(object sender, EventArgs e)
		{
			maxBtn.BackColor = Color.FromArgb(4, 191, 157);
		}

		private void maxBtn_MouseLeave(object sender, EventArgs e)
		{
			maxBtn.BackColor = Color.Transparent;
		}

		private void reloadCoin_Click(object sender, EventArgs e)
		{
			method_21();
		}

		private void giaodienthapbtn_Click(object sender, EventArgs e)
		{
			if (System.Windows.Forms.MessageBox.Show("Bạn có muốn chuyển qua chế độ máy thấp không, thao tác sẽ reload lại game?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				OpenFrm2();
			}
		}

		private void giaodienthapbtn_MouseEnter(object sender, EventArgs e)
		{
			giaodienthapbtn.BackColor = Color.FromArgb(255, 211, 155);
		}

		private void giaodienthapbtn_MouseLeave(object sender, EventArgs e)
		{
			giaodienthapbtn.BackColor = Color.Transparent;
		}

		public void OpenFrm2()
		{
			if (base.InvokeRequired)
			{
				Invoke((MethodInvoker)delegate
				{
					OpenFrm2();
				});
				return;
			}
			Close();
			thread_1 = new Thread(method_26);
			thread_1.SetApartmentState(ApartmentState.STA);
			thread_1.Start();
		}

		private void method_26()
		{
			System.Windows.Forms.Application.Run(new PlayGameFrm2FIB(LoginMgr.ServerID, string_0));
		}

		private void anMenuleftBtn_MouseEnter(object sender, EventArgs e)
		{
			anMenuleftBtn.IconColor = Color.FromArgb(0, 244, 201);
		}

		private void anMenuleftBtn_MouseLeave(object sender, EventArgs e)
		{
			anMenuleftBtn.IconColor = Color.White;
		}

		private void reloadCoin_MouseEnter(object sender, EventArgs e)
		{
			reloadCoin.IconColor = Color.FromArgb(0, 244, 201);
		}

		private void reloadCoin_MouseLeave(object sender, EventArgs e)
		{
			reloadCoin.IconColor = Color.White;
		}

		private void timer_1_Tick(object sender, EventArgs e)
		{
			base.Opacity -= 0.014;
			if (base.Opacity <= 0.01)
			{
				timer_1.Stop();
				System.Windows.Forms.Application.Exit();
			}
		}

		private void label3_Click(object sender, EventArgs e)
		{
			doithongtinBtn.PerformClick();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayGameFrmFIB));
            this.toolTip_0 = new System.Windows.Forms.ToolTip(this.components);
            this.panel_Playgame = new System.Windows.Forms.Panel();
            this.timer_1 = new System.Windows.Forms.Timer(this.components);
            this.saveFileDialog_0 = new System.Windows.Forms.SaveFileDialog();
            this.menuRighttop = new System.Windows.Forms.Panel();
            this.qualityPanel = new System.Windows.Forms.Panel();
            this.thapLb = new System.Windows.Forms.Label();
            this.caoR = new System.Windows.Forms.RadioButton();
            this.thuongR = new System.Windows.Forms.RadioButton();
            this.thuongLb = new System.Windows.Forms.Label();
            this.thapR = new System.Windows.Forms.RadioButton();
            this.caoLb = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.menuLefttxt = new System.Windows.Forms.Label();
            this.giaodienthapbtn = new FontAwesome.Sharp.IconPictureBox();
            this.exitBtn = new FontAwesome.Sharp.IconPictureBox();
            this.maxBtn = new FontAwesome.Sharp.IconPictureBox();
            this.miniBtn = new FontAwesome.Sharp.IconPictureBox();
            this.reloadBtn = new FontAwesome.Sharp.IconPictureBox();
            this.congthucBtn = new FontAwesome.Sharp.IconPictureBox();
            this.deleteCacheBtn = new FontAwesome.Sharp.IconPictureBox();
            this.bunipanelLeft = new System.Windows.Forms.Panel();
            this.reloadCoin = new FontAwesome.Sharp.IconPictureBox();
            this.iconButton1 = new FontAwesome.Sharp.IconButton();
            this.label3 = new System.Windows.Forms.Label();
            this.sukientuanBtn = new FontAwesome.Sharp.IconButton();
            this.label7 = new System.Windows.Forms.Label();
            this.playBtn = new FontAwesome.Sharp.IconButton();
            this.iconPictureBox1 = new FontAwesome.Sharp.IconPictureBox();
            this.coinTxt = new System.Windows.Forms.Label();
            this.dangxuatBtn = new FontAwesome.Sharp.IconButton();
            this.label1 = new System.Windows.Forms.Label();
            this.naptheBtn = new FontAwesome.Sharp.IconButton();
            this.anMenuleftBtn = new FontAwesome.Sharp.IconPictureBox();
            this.doimaychuBtn = new FontAwesome.Sharp.IconButton();
            this.doithongtinBtn = new FontAwesome.Sharp.IconButton();
            this.menuRighttop.SuspendLayout();
            this.qualityPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.giaodienthapbtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.exitBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.miniBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reloadBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.congthucBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deleteCacheBtn)).BeginInit();
            this.bunipanelLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.reloadCoin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.anMenuleftBtn)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_Playgame
            // 
            this.panel_Playgame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Playgame.Location = new System.Drawing.Point(218, 35);
            this.panel_Playgame.Name = "panel_Playgame";
            this.panel_Playgame.Size = new System.Drawing.Size(1001, 641);
            this.panel_Playgame.TabIndex = 0;
            // 
            // timer_1
            // 
            this.timer_1.Interval = 10;
            this.timer_1.Tick += new System.EventHandler(this.timer_1_Tick);
            // 
            // menuRighttop
            // 
            this.menuRighttop.BackgroundImage = global::Properties.Resources.vPuCElj1TUxuMPvd;
            this.menuRighttop.Controls.Add(this.qualityPanel);
            this.menuRighttop.Controls.Add(this.menuLefttxt);
            this.menuRighttop.Controls.Add(this.giaodienthapbtn);
            this.menuRighttop.Controls.Add(this.exitBtn);
            this.menuRighttop.Controls.Add(this.maxBtn);
            this.menuRighttop.Controls.Add(this.miniBtn);
            this.menuRighttop.Controls.Add(this.reloadBtn);
            this.menuRighttop.Controls.Add(this.congthucBtn);
            this.menuRighttop.Controls.Add(this.deleteCacheBtn);
            this.menuRighttop.Dock = System.Windows.Forms.DockStyle.Top;
            this.menuRighttop.Location = new System.Drawing.Point(218, 0);
            this.menuRighttop.Name = "menuRighttop";
            this.menuRighttop.Size = new System.Drawing.Size(1001, 35);
            this.menuRighttop.TabIndex = 23;
            this.menuRighttop.Paint += new System.Windows.Forms.PaintEventHandler(this.menuRighttop_Paint);
            this.menuRighttop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.menuRighttop_MouseDown);
            // 
            // qualityPanel
            // 
            this.qualityPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.qualityPanel.BackColor = System.Drawing.Color.Transparent;
            this.qualityPanel.Controls.Add(this.thapLb);
            this.qualityPanel.Controls.Add(this.caoR);
            this.qualityPanel.Controls.Add(this.thuongR);
            this.qualityPanel.Controls.Add(this.thuongLb);
            this.qualityPanel.Controls.Add(this.thapR);
            this.qualityPanel.Controls.Add(this.caoLb);
            this.qualityPanel.Controls.Add(this.label4);
            this.qualityPanel.Location = new System.Drawing.Point(446, 3);
            this.qualityPanel.Name = "qualityPanel";
            this.qualityPanel.Size = new System.Drawing.Size(282, 26);
            this.qualityPanel.TabIndex = 26;
            this.qualityPanel.Visible = false;
            // 
            // thapLb
            // 
            this.thapLb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.thapLb.AutoSize = true;
            this.thapLb.BackColor = System.Drawing.Color.Transparent;
            this.thapLb.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.thapLb.ForeColor = System.Drawing.Color.White;
            this.thapLb.Location = new System.Drawing.Point(108, 4);
            this.thapLb.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.thapLb.Name = "thapLb";
            this.thapLb.Size = new System.Drawing.Size(37, 16);
            this.thapLb.TabIndex = 19;
            this.thapLb.Text = "Thấp";
            this.thapLb.Click += new System.EventHandler(this.thapLb_Click);
            // 
            // caoR
            // 
            this.caoR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.caoR.AutoSize = true;
            this.caoR.BackColor = System.Drawing.Color.Transparent;
            this.caoR.Checked = true;
            this.caoR.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.caoR.FlatAppearance.BorderSize = 50;
            this.caoR.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.caoR.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.caoR.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.caoR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.caoR.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.caoR.ForeColor = System.Drawing.Color.Maroon;
            this.caoR.Location = new System.Drawing.Point(231, 8);
            this.caoR.Margin = new System.Windows.Forms.Padding(4);
            this.caoR.Name = "caoR";
            this.caoR.Size = new System.Drawing.Size(13, 12);
            this.caoR.TabIndex = 17;
            this.caoR.TabStop = true;
            this.caoR.UseVisualStyleBackColor = true;
            this.caoR.CheckedChanged += new System.EventHandler(this.caoR_CheckedChanged);
            // 
            // thuongR
            // 
            this.thuongR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.thuongR.AutoSize = true;
            this.thuongR.BackColor = System.Drawing.Color.Transparent;
            this.thuongR.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.thuongR.FlatAppearance.BorderSize = 50;
            this.thuongR.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.thuongR.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.thuongR.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.thuongR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.thuongR.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.thuongR.ForeColor = System.Drawing.Color.Maroon;
            this.thuongR.Location = new System.Drawing.Point(156, 7);
            this.thuongR.Margin = new System.Windows.Forms.Padding(4);
            this.thuongR.Name = "thuongR";
            this.thuongR.Size = new System.Drawing.Size(13, 12);
            this.thuongR.TabIndex = 16;
            this.thuongR.UseVisualStyleBackColor = false;
            this.thuongR.CheckedChanged += new System.EventHandler(this.thuongR_CheckedChanged);
            // 
            // thuongLb
            // 
            this.thuongLb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.thuongLb.AutoSize = true;
            this.thuongLb.BackColor = System.Drawing.Color.Transparent;
            this.thuongLb.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.thuongLb.ForeColor = System.Drawing.Color.White;
            this.thuongLb.Location = new System.Drawing.Point(173, 5);
            this.thuongLb.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.thuongLb.Name = "thuongLb";
            this.thuongLb.Size = new System.Drawing.Size(52, 16);
            this.thuongLb.TabIndex = 20;
            this.thuongLb.Text = "Thường";
            this.thuongLb.Click += new System.EventHandler(this.thuongLb_Click);
            // 
            // thapR
            // 
            this.thapR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.thapR.AutoSize = true;
            this.thapR.BackColor = System.Drawing.Color.Transparent;
            this.thapR.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.thapR.FlatAppearance.BorderSize = 50;
            this.thapR.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.thapR.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.thapR.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.thapR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.thapR.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.thapR.ForeColor = System.Drawing.Color.Maroon;
            this.thapR.Location = new System.Drawing.Point(90, 6);
            this.thapR.Margin = new System.Windows.Forms.Padding(4);
            this.thapR.Name = "thapR";
            this.thapR.Size = new System.Drawing.Size(13, 12);
            this.thapR.TabIndex = 15;
            this.thapR.UseVisualStyleBackColor = false;
            this.thapR.CheckedChanged += new System.EventHandler(this.thapR_CheckedChanged);
            // 
            // caoLb
            // 
            this.caoLb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.caoLb.AutoSize = true;
            this.caoLb.BackColor = System.Drawing.Color.Transparent;
            this.caoLb.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.caoLb.ForeColor = System.Drawing.Color.White;
            this.caoLb.Location = new System.Drawing.Point(247, 5);
            this.caoLb.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.caoLb.Name = "caoLb";
            this.caoLb.Size = new System.Drawing.Size(30, 16);
            this.caoLb.TabIndex = 21;
            this.caoLb.Text = "Cao";
            this.caoLb.Click += new System.EventHandler(this.caoLb_Click);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(3, 3);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 16);
            this.label4.TabIndex = 18;
            this.label4.Text = "Chất lượng :";
            // 
            // menuLefttxt
            // 
            this.menuLefttxt.AutoSize = true;
            this.menuLefttxt.BackColor = System.Drawing.Color.Transparent;
            this.menuLefttxt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.menuLefttxt.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuLefttxt.ForeColor = System.Drawing.Color.White;
            this.menuLefttxt.Location = new System.Drawing.Point(6, 5);
            this.menuLefttxt.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.menuLefttxt.Name = "menuLefttxt";
            this.menuLefttxt.Size = new System.Drawing.Size(224, 19);
            this.menuLefttxt.TabIndex = 1;
            this.menuLefttxt.Text = "Siêu Gunny | Hồi Ức Tuổi Thơ";
            this.menuLefttxt.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PlayGameFrmFIB_MouseDown);
            // 
            // giaodienthapbtn
            // 
            this.giaodienthapbtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.giaodienthapbtn.BackColor = System.Drawing.Color.Transparent;
            this.giaodienthapbtn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.giaodienthapbtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.giaodienthapbtn.IconChar = FontAwesome.Sharp.IconChar.BatteryQuarter;
            this.giaodienthapbtn.IconColor = System.Drawing.Color.White;
            this.giaodienthapbtn.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.giaodienthapbtn.IconSize = 23;
            this.giaodienthapbtn.Location = new System.Drawing.Point(743, 3);
            this.giaodienthapbtn.Margin = new System.Windows.Forms.Padding(5);
            this.giaodienthapbtn.Name = "giaodienthapbtn";
            this.giaodienthapbtn.Size = new System.Drawing.Size(32, 23);
            this.giaodienthapbtn.TabIndex = 25;
            this.giaodienthapbtn.TabStop = false;
            this.toolTip_0.SetToolTip(this.giaodienthapbtn, "Chuyển sang giao diện thấp");
            this.giaodienthapbtn.Click += new System.EventHandler(this.giaodienthapbtn_Click);
            this.giaodienthapbtn.MouseEnter += new System.EventHandler(this.giaodienthapbtn_MouseEnter);
            this.giaodienthapbtn.MouseLeave += new System.EventHandler(this.giaodienthapbtn_MouseLeave);
            // 
            // exitBtn
            // 
            this.exitBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exitBtn.BackColor = System.Drawing.Color.Transparent;
            this.exitBtn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.exitBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.exitBtn.IconChar = FontAwesome.Sharp.IconChar.Times;
            this.exitBtn.IconColor = System.Drawing.Color.White;
            this.exitBtn.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.exitBtn.IconSize = 23;
            this.exitBtn.Location = new System.Drawing.Point(966, 3);
            this.exitBtn.Margin = new System.Windows.Forms.Padding(5);
            this.exitBtn.Name = "exitBtn";
            this.exitBtn.Size = new System.Drawing.Size(32, 23);
            this.exitBtn.TabIndex = 2;
            this.exitBtn.TabStop = false;
            this.exitBtn.Click += new System.EventHandler(this.exitBtn_Click);
            this.exitBtn.MouseEnter += new System.EventHandler(this.exitBtn_MouseEnter);
            this.exitBtn.MouseLeave += new System.EventHandler(this.exitBtn_MouseLeave);
            // 
            // maxBtn
            // 
            this.maxBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.maxBtn.BackColor = System.Drawing.Color.Transparent;
            this.maxBtn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.maxBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.maxBtn.IconChar = FontAwesome.Sharp.IconChar.WindowMaximize;
            this.maxBtn.IconColor = System.Drawing.Color.White;
            this.maxBtn.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.maxBtn.IconSize = 23;
            this.maxBtn.Location = new System.Drawing.Point(929, 3);
            this.maxBtn.Margin = new System.Windows.Forms.Padding(5);
            this.maxBtn.Name = "maxBtn";
            this.maxBtn.Size = new System.Drawing.Size(32, 23);
            this.maxBtn.TabIndex = 23;
            this.maxBtn.TabStop = false;
            this.toolTip_0.SetToolTip(this.maxBtn, "Phóng tó / thu nhỏ");
            this.maxBtn.Click += new System.EventHandler(this.maxBtn_Click);
            this.maxBtn.MouseEnter += new System.EventHandler(this.maxBtn_MouseEnter);
            this.maxBtn.MouseLeave += new System.EventHandler(this.maxBtn_MouseLeave);
            // 
            // miniBtn
            // 
            this.miniBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.miniBtn.BackColor = System.Drawing.Color.Transparent;
            this.miniBtn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.miniBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.miniBtn.IconChar = FontAwesome.Sharp.IconChar.Minus;
            this.miniBtn.IconColor = System.Drawing.Color.White;
            this.miniBtn.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.miniBtn.IconSize = 23;
            this.miniBtn.Location = new System.Drawing.Point(891, 3);
            this.miniBtn.Margin = new System.Windows.Forms.Padding(5);
            this.miniBtn.Name = "miniBtn";
            this.miniBtn.Size = new System.Drawing.Size(32, 23);
            this.miniBtn.TabIndex = 3;
            this.miniBtn.TabStop = false;
            this.miniBtn.Click += new System.EventHandler(this.miniBtn_Click);
            this.miniBtn.MouseEnter += new System.EventHandler(this.miniBtn_MouseEnter);
            this.miniBtn.MouseLeave += new System.EventHandler(this.miniBtn_MouseLeave);
            // 
            // reloadBtn
            // 
            this.reloadBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.reloadBtn.BackColor = System.Drawing.Color.Transparent;
            this.reloadBtn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.reloadBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.reloadBtn.IconChar = FontAwesome.Sharp.IconChar.Sync;
            this.reloadBtn.IconColor = System.Drawing.Color.White;
            this.reloadBtn.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.reloadBtn.IconSize = 23;
            this.reloadBtn.Location = new System.Drawing.Point(779, 3);
            this.reloadBtn.Margin = new System.Windows.Forms.Padding(5);
            this.reloadBtn.Name = "reloadBtn";
            this.reloadBtn.Size = new System.Drawing.Size(32, 23);
            this.reloadBtn.TabIndex = 22;
            this.reloadBtn.TabStop = false;
            this.toolTip_0.SetToolTip(this.reloadBtn, "Tải lại game");
            this.reloadBtn.Click += new System.EventHandler(this.reloadBtn_Click);
            this.reloadBtn.MouseEnter += new System.EventHandler(this.reloadBtn_MouseEnter);
            this.reloadBtn.MouseLeave += new System.EventHandler(this.reloadBtn_MouseLeave);
            // 
            // congthucBtn
            // 
            this.congthucBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.congthucBtn.BackColor = System.Drawing.Color.Transparent;
            this.congthucBtn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.congthucBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.congthucBtn.IconChar = FontAwesome.Sharp.IconChar.Eye;
            this.congthucBtn.IconColor = System.Drawing.Color.White;
            this.congthucBtn.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.congthucBtn.IconSize = 23;
            this.congthucBtn.Location = new System.Drawing.Point(854, 3);
            this.congthucBtn.Margin = new System.Windows.Forms.Padding(5);
            this.congthucBtn.Name = "congthucBtn";
            this.congthucBtn.Size = new System.Drawing.Size(32, 23);
            this.congthucBtn.TabIndex = 11;
            this.congthucBtn.TabStop = false;
            this.toolTip_0.SetToolTip(this.congthucBtn, "Bảng công thức");
            this.congthucBtn.Click += new System.EventHandler(this.congthucBtn_Click);
            this.congthucBtn.MouseEnter += new System.EventHandler(this.congthucBtn_MouseEnter);
            this.congthucBtn.MouseLeave += new System.EventHandler(this.congthucBtn_MouseLeave);
            // 
            // deleteCacheBtn
            // 
            this.deleteCacheBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteCacheBtn.BackColor = System.Drawing.Color.Transparent;
            this.deleteCacheBtn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.deleteCacheBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.deleteCacheBtn.IconChar = FontAwesome.Sharp.IconChar.Eraser;
            this.deleteCacheBtn.IconColor = System.Drawing.Color.White;
            this.deleteCacheBtn.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.deleteCacheBtn.IconSize = 23;
            this.deleteCacheBtn.Location = new System.Drawing.Point(817, 3);
            this.deleteCacheBtn.Margin = new System.Windows.Forms.Padding(5);
            this.deleteCacheBtn.Name = "deleteCacheBtn";
            this.deleteCacheBtn.Size = new System.Drawing.Size(32, 23);
            this.deleteCacheBtn.TabIndex = 12;
            this.deleteCacheBtn.TabStop = false;
            this.toolTip_0.SetToolTip(this.deleteCacheBtn, "Xóa Cache");
            this.deleteCacheBtn.Click += new System.EventHandler(this.deleteCacheBtn_Click);
            this.deleteCacheBtn.MouseEnter += new System.EventHandler(this.deleteCacheBtn_MouseEnter);
            this.deleteCacheBtn.MouseLeave += new System.EventHandler(this.deleteCacheBtn_MouseLeave);
            // 
            // bunipanelLeft
            // 
            this.bunipanelLeft.BackgroundImage = global::Properties.Resources.vPuCElj1TUxuMPvd;
            this.bunipanelLeft.Controls.Add(this.reloadCoin);
            this.bunipanelLeft.Controls.Add(this.iconButton1);
            this.bunipanelLeft.Controls.Add(this.label3);
            this.bunipanelLeft.Controls.Add(this.sukientuanBtn);
            this.bunipanelLeft.Controls.Add(this.label7);
            this.bunipanelLeft.Controls.Add(this.playBtn);
            this.bunipanelLeft.Controls.Add(this.iconPictureBox1);
            this.bunipanelLeft.Controls.Add(this.coinTxt);
            this.bunipanelLeft.Controls.Add(this.dangxuatBtn);
            this.bunipanelLeft.Controls.Add(this.label1);
            this.bunipanelLeft.Controls.Add(this.naptheBtn);
            this.bunipanelLeft.Controls.Add(this.anMenuleftBtn);
            this.bunipanelLeft.Controls.Add(this.doimaychuBtn);
            this.bunipanelLeft.Controls.Add(this.doithongtinBtn);
            this.bunipanelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.bunipanelLeft.Location = new System.Drawing.Point(0, 0);
            this.bunipanelLeft.Name = "bunipanelLeft";
            this.bunipanelLeft.Size = new System.Drawing.Size(218, 676);
            this.bunipanelLeft.TabIndex = 10;
            // 
            // reloadCoin
            // 
            this.reloadCoin.BackColor = System.Drawing.Color.Transparent;
            this.reloadCoin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.reloadCoin.IconChar = FontAwesome.Sharp.IconChar.SyncAlt;
            this.reloadCoin.IconColor = System.Drawing.Color.White;
            this.reloadCoin.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.reloadCoin.IconSize = 22;
            this.reloadCoin.Location = new System.Drawing.Point(197, 40);
            this.reloadCoin.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.reloadCoin.Name = "reloadCoin";
            this.reloadCoin.Size = new System.Drawing.Size(22, 26);
            this.reloadCoin.TabIndex = 12;
            this.reloadCoin.TabStop = false;
            this.toolTip_0.SetToolTip(this.reloadCoin, "Cập nhật Coin");
            this.reloadCoin.Click += new System.EventHandler(this.reloadCoin_Click);
            this.reloadCoin.MouseEnter += new System.EventHandler(this.reloadCoin_MouseEnter);
            this.reloadCoin.MouseLeave += new System.EventHandler(this.reloadCoin_MouseLeave);
            // 
            // iconButton1
            // 
            this.iconButton1.BackColor = System.Drawing.Color.Transparent;
            this.iconButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.iconButton1.FlatAppearance.BorderSize = 0;
            this.iconButton1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.iconButton1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(189)))), ((int)(((byte)(255)))));
            this.iconButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButton1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iconButton1.ForeColor = System.Drawing.Color.White;
            this.iconButton1.IconChar = FontAwesome.Sharp.IconChar.Readme;
            this.iconButton1.IconColor = System.Drawing.SystemColors.ButtonFace;
            this.iconButton1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconButton1.IconSize = 30;
            this.iconButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButton1.Location = new System.Drawing.Point(0, 276);
            this.iconButton1.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.iconButton1.Name = "iconButton1";
            this.iconButton1.Size = new System.Drawing.Size(227, 45);
            this.iconButton1.TabIndex = 26;
            this.iconButton1.TabStop = false;
            this.iconButton1.Text = "Nhắn Tin Hỗ Trợ";
            this.toolTip_0.SetToolTip(this.iconButton1, "Đổi Xu");
            this.iconButton1.UseVisualStyleBackColor = false;
            this.iconButton1.Click += new System.EventHandler(this.iconButton1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label3.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(184)))), ((int)(((byte)(20)))));
            this.label3.Location = new System.Drawing.Point(16, 102);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 18);
            this.label3.TabIndex = 11;
            this.label3.Tag = "";
            this.label3.Text = "Tài khoản";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            this.label3.MouseEnter += new System.EventHandler(this.label3_MouseEnter);
            this.label3.MouseLeave += new System.EventHandler(this.label3_MouseLeave);
            // 
            // sukientuanBtn
            // 
            this.sukientuanBtn.BackColor = System.Drawing.Color.Transparent;
            this.sukientuanBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sukientuanBtn.FlatAppearance.BorderSize = 0;
            this.sukientuanBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.sukientuanBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(189)))), ((int)(((byte)(255)))));
            this.sukientuanBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sukientuanBtn.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sukientuanBtn.ForeColor = System.Drawing.Color.White;
            this.sukientuanBtn.IconChar = FontAwesome.Sharp.IconChar.LevelUpAlt;
            this.sukientuanBtn.IconColor = System.Drawing.SystemColors.ButtonFace;
            this.sukientuanBtn.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.sukientuanBtn.IconSize = 30;
            this.sukientuanBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.sukientuanBtn.Location = new System.Drawing.Point(3, 440);
            this.sukientuanBtn.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.sukientuanBtn.Name = "sukientuanBtn";
            this.sukientuanBtn.Size = new System.Drawing.Size(227, 45);
            this.sukientuanBtn.TabIndex = 25;
            this.sukientuanBtn.TabStop = false;
            this.sukientuanBtn.Text = "SỰ KIỆN TUẦN";
            this.toolTip_0.SetToolTip(this.sukientuanBtn, "Sự kiện tuần");
            this.sukientuanBtn.UseVisualStyleBackColor = false;
            this.sukientuanBtn.Visible = false;
            this.sukientuanBtn.Click += new System.EventHandler(this.sukientuanBtn_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label7.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(184)))), ((int)(((byte)(20)))));
            this.label7.Location = new System.Drawing.Point(120, 102);
            this.label7.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 18);
            this.label7.TabIndex = 9;
            this.label7.Tag = "";
            this.label7.Text = "Đổi mật khẩu";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            this.label7.MouseEnter += new System.EventHandler(this.label7_MouseEnter);
            this.label7.MouseLeave += new System.EventHandler(this.label7_MouseLeave);
            // 
            // playBtn
            // 
            this.playBtn.BackColor = System.Drawing.Color.Transparent;
            this.playBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.playBtn.FlatAppearance.BorderSize = 0;
            this.playBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.playBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(189)))), ((int)(((byte)(255)))));
            this.playBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.playBtn.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playBtn.ForeColor = System.Drawing.Color.White;
            this.playBtn.IconChar = FontAwesome.Sharp.IconChar.PlayCircle;
            this.playBtn.IconColor = System.Drawing.SystemColors.ButtonFace;
            this.playBtn.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.playBtn.IconSize = 30;
            this.playBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.playBtn.Location = new System.Drawing.Point(2, 141);
            this.playBtn.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.playBtn.Name = "playBtn";
            this.playBtn.Size = new System.Drawing.Size(227, 45);
            this.playBtn.TabIndex = 18;
            this.playBtn.TabStop = false;
            this.playBtn.Text = "CHƠI GAME";
            this.toolTip_0.SetToolTip(this.playBtn, "Chơi game");
            this.playBtn.UseVisualStyleBackColor = false;
            this.playBtn.Click += new System.EventHandler(this.playBtn_Click);
            // 
            // iconPictureBox1
            // 
            this.iconPictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.iconPictureBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.iconPictureBox1.IconChar = FontAwesome.Sharp.IconChar.UserCircle;
            this.iconPictureBox1.IconColor = System.Drawing.Color.White;
            this.iconPictureBox1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconPictureBox1.IconSize = 89;
            this.iconPictureBox1.Location = new System.Drawing.Point(3, 7);
            this.iconPictureBox1.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.iconPictureBox1.Name = "iconPictureBox1";
            this.iconPictureBox1.Size = new System.Drawing.Size(89, 92);
            this.iconPictureBox1.TabIndex = 9;
            this.iconPictureBox1.TabStop = false;
            this.toolTip_0.SetToolTip(this.iconPictureBox1, "BemBemGunny");
            // 
            // coinTxt
            // 
            this.coinTxt.AutoSize = true;
            this.coinTxt.BackColor = System.Drawing.Color.Transparent;
            this.coinTxt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.coinTxt.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coinTxt.ForeColor = System.Drawing.Color.MistyRose;
            this.coinTxt.Location = new System.Drawing.Point(93, 43);
            this.coinTxt.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.coinTxt.Name = "coinTxt";
            this.coinTxt.Size = new System.Drawing.Size(37, 16);
            this.coinTxt.TabIndex = 10;
            this.coinTxt.Tag = "";
            this.coinTxt.Text = "Coin ";
            // 
            // dangxuatBtn
            // 
            this.dangxuatBtn.BackColor = System.Drawing.Color.Transparent;
            this.dangxuatBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dangxuatBtn.FlatAppearance.BorderSize = 0;
            this.dangxuatBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.dangxuatBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(189)))), ((int)(((byte)(255)))));
            this.dangxuatBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dangxuatBtn.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dangxuatBtn.ForeColor = System.Drawing.Color.White;
            this.dangxuatBtn.IconChar = FontAwesome.Sharp.IconChar.SignOutAlt;
            this.dangxuatBtn.IconColor = System.Drawing.SystemColors.ButtonFace;
            this.dangxuatBtn.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.dangxuatBtn.IconSize = 30;
            this.dangxuatBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.dangxuatBtn.Location = new System.Drawing.Point(3, 628);
            this.dangxuatBtn.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.dangxuatBtn.Name = "dangxuatBtn";
            this.dangxuatBtn.Size = new System.Drawing.Size(227, 45);
            this.dangxuatBtn.TabIndex = 21;
            this.dangxuatBtn.TabStop = false;
            this.dangxuatBtn.Text = "ĐĂNG XUẤT";
            this.dangxuatBtn.UseVisualStyleBackColor = false;
            this.dangxuatBtn.Click += new System.EventHandler(this.dangxuatBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.MistyRose;
            this.label1.Location = new System.Drawing.Point(93, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 16);
            this.label1.TabIndex = 9;
            this.label1.Tag = "";
            this.label1.Text = "Tên tài khoản";
            // 
            // naptheBtn
            // 
            this.naptheBtn.BackColor = System.Drawing.Color.Transparent;
            this.naptheBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.naptheBtn.FlatAppearance.BorderSize = 0;
            this.naptheBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.naptheBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(189)))), ((int)(((byte)(255)))));
            this.naptheBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.naptheBtn.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.naptheBtn.ForeColor = System.Drawing.Color.White;
            this.naptheBtn.IconChar = FontAwesome.Sharp.IconChar.DollarSign;
            this.naptheBtn.IconColor = System.Drawing.SystemColors.ButtonFace;
            this.naptheBtn.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.naptheBtn.IconSize = 30;
            this.naptheBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.naptheBtn.Location = new System.Drawing.Point(0, 225);
            this.naptheBtn.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.naptheBtn.Name = "naptheBtn";
            this.naptheBtn.Size = new System.Drawing.Size(227, 45);
            this.naptheBtn.TabIndex = 19;
            this.naptheBtn.TabStop = false;
            this.naptheBtn.Text = "ĐỔI XU";
            this.toolTip_0.SetToolTip(this.naptheBtn, "Đổi Xu");
            this.naptheBtn.UseVisualStyleBackColor = false;
            this.naptheBtn.Click += new System.EventHandler(this.naptheBtn_Click);
            // 
            // anMenuleftBtn
            // 
            this.anMenuleftBtn.BackColor = System.Drawing.Color.Transparent;
            this.anMenuleftBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.anMenuleftBtn.IconChar = FontAwesome.Sharp.IconChar.Bars;
            this.anMenuleftBtn.IconColor = System.Drawing.Color.White;
            this.anMenuleftBtn.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.anMenuleftBtn.IconSize = 25;
            this.anMenuleftBtn.Location = new System.Drawing.Point(192, 2);
            this.anMenuleftBtn.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.anMenuleftBtn.Name = "anMenuleftBtn";
            this.anMenuleftBtn.Size = new System.Drawing.Size(25, 25);
            this.anMenuleftBtn.TabIndex = 8;
            this.anMenuleftBtn.TabStop = false;
            this.toolTip_0.SetToolTip(this.anMenuleftBtn, "Ẩn thanh Menu");
            this.anMenuleftBtn.Click += new System.EventHandler(this.anMenuleftBtn_Click);
            this.anMenuleftBtn.MouseEnter += new System.EventHandler(this.anMenuleftBtn_MouseEnter);
            this.anMenuleftBtn.MouseLeave += new System.EventHandler(this.anMenuleftBtn_MouseLeave);
            // 
            // doimaychuBtn
            // 
            this.doimaychuBtn.BackColor = System.Drawing.Color.Transparent;
            this.doimaychuBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.doimaychuBtn.FlatAppearance.BorderSize = 0;
            this.doimaychuBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.doimaychuBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(189)))), ((int)(((byte)(255)))));
            this.doimaychuBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.doimaychuBtn.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.doimaychuBtn.ForeColor = System.Drawing.Color.White;
            this.doimaychuBtn.IconChar = FontAwesome.Sharp.IconChar.ExchangeAlt;
            this.doimaychuBtn.IconColor = System.Drawing.SystemColors.ButtonFace;
            this.doimaychuBtn.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.doimaychuBtn.IconSize = 30;
            this.doimaychuBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.doimaychuBtn.Location = new System.Drawing.Point(3, 486);
            this.doimaychuBtn.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.doimaychuBtn.Name = "doimaychuBtn";
            this.doimaychuBtn.Size = new System.Drawing.Size(227, 45);
            this.doimaychuBtn.TabIndex = 20;
            this.doimaychuBtn.TabStop = false;
            this.doimaychuBtn.Text = "ĐỔI MÁY CHỦ";
            this.toolTip_0.SetToolTip(this.doimaychuBtn, "Đổi máy chủ");
            this.doimaychuBtn.UseVisualStyleBackColor = false;
            this.doimaychuBtn.Visible = false;
            this.doimaychuBtn.Click += new System.EventHandler(this.doimaychuBtn_Click);
            // 
            // doithongtinBtn
            // 
            this.doithongtinBtn.BackColor = System.Drawing.Color.Transparent;
            this.doithongtinBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.doithongtinBtn.FlatAppearance.BorderSize = 0;
            this.doithongtinBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.doithongtinBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(189)))), ((int)(((byte)(255)))));
            this.doithongtinBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.doithongtinBtn.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.doithongtinBtn.ForeColor = System.Drawing.Color.White;
            this.doithongtinBtn.IconChar = FontAwesome.Sharp.IconChar.UserCog;
            this.doithongtinBtn.IconColor = System.Drawing.SystemColors.ButtonFace;
            this.doithongtinBtn.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.doithongtinBtn.IconSize = 30;
            this.doithongtinBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.doithongtinBtn.Location = new System.Drawing.Point(2, 179);
            this.doithongtinBtn.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.doithongtinBtn.Name = "doithongtinBtn";
            this.doithongtinBtn.Size = new System.Drawing.Size(227, 45);
            this.doithongtinBtn.TabIndex = 23;
            this.doithongtinBtn.TabStop = false;
            this.doithongtinBtn.Text = "ĐỔI THÔNG TIN";
            this.toolTip_0.SetToolTip(this.doithongtinBtn, "Nạp thẻ");
            this.doithongtinBtn.UseVisualStyleBackColor = false;
            this.doithongtinBtn.Click += new System.EventHandler(this.doithongtinBtn_Click);
            // 
            // PlayGameFrmFIB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1219, 676);
            this.ControlBox = false;
            this.Controls.Add(this.panel_Playgame);
            this.Controls.Add(this.menuRighttop);
            this.Controls.Add(this.bunipanelLeft);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "PlayGameFrmFIB";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chơi Game - Gunny Lộc Phát";
            this.Activated += new System.EventHandler(this.PlayGameFrmFIB_Activated);
            this.Load += new System.EventHandler(this.PlayGameFrmFIB_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PlayGameFrmFIB_MouseDown);
            this.menuRighttop.ResumeLayout(false);
            this.menuRighttop.PerformLayout();
            this.qualityPanel.ResumeLayout(false);
            this.qualityPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.giaodienthapbtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.exitBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.miniBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reloadBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.congthucBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deleteCacheBtn)).EndInit();
            this.bunipanelLeft.ResumeLayout(false);
            this.bunipanelLeft.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.reloadCoin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.anMenuleftBtn)).EndInit();
            this.ResumeLayout(false);

		}

		[CompilerGenerated]
		private void method_27()
		{
			OpenFrm2();
		}

		internal static bool CetDB3fg2r9wkN8qAPd()
		{
			return LDVqCbfmZEYcYpQluFv == null;
		}

        private void iconButton1_Click(object sender, EventArgs e)
        {
			ControlMgr.OpenWebsite(ApplicationConfig.FanpageLink);
		}

        private void menuRighttop_Paint(object sender, PaintEventArgs e)
        {

		}

        private void menuRighttop_MouseDown(object sender, MouseEventArgs e)
        {
			method_5(e);
		}
    }
}
