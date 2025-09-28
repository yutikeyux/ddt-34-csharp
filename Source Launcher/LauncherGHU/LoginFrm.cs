using System;
using System.Collections.Generic;
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
using AutoUpdaterDotNET;
using FontAwesome.Sharp;
using LauncherGHU.API;
using LauncherGHU.Properties;
using LoadLauncher;
using MaterialSkin;
using MaterialSkin.Controls;
using MetroFramework;
using MetroFramework.Controls;
using ns1;

namespace LauncherGHU
{
	public class LoginFrm : Form
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct Struct4
		{
			public static Color color_0;

			public static Color color_1;

			public static Color color_2;

			public static Color color_3;

			public static Color color_4;

			public static Color color_5;

			internal static object Gjh19YLu3rLcsaO3y3a;

			static Struct4()
			{
				color_0 = Color.FromArgb(172, 126, 241);
				color_1 = Color.FromArgb(249, 118, 176);
				color_2 = Color.FromArgb(253, 138, 114);
				color_3 = Color.FromArgb(95, 77, 221);
				color_4 = Color.FromArgb(249, 88, 155);
				color_5 = Color.FromArgb(24, 161, 251);
			}

			internal static void swERUcLE7T3hsWCKx6c()
			{
			}

			internal static bool P7kkMJLkleCHQmGbpqs()
			{
				return Gjh19YLu3rLcsaO3y3a == null;
			}
		}

		private IconButton iconButton_0;

		private Panel panel_0;

		private string string_0 = "";

		public const int WM_NCLBUTTONDOWN = 161;

		public const int HT_CAPTION = 2;

		private Thread thread_0;

		private RegisterForm registerForm_0;

		private ForgetForm forgetForm_0;

		private IContainer icontainer_0 = null;

		private BackgroundWorker backgroundWorker_0;

		private OpenFileDialog openFileDialog_0;

		private BunifuGradientPanel bunipanelLeft;

		private BunifuGradientPanel bunifuGradientPanel2;

		private IconButton loginBtn;

		private IconButton forgotbtn;

		private IconButton regBtn;

		private PictureBox logoBtn;

		private BunifuGradientPanel menuRighttop;

		private IconPictureBox exitBtn;

		private IconPictureBox miniBtn;

		private Panel loginPanelRight;

		private MaterialRaisedButton dangnhapBtn;

		private MaterialSingleLineTextField matkhauTxt;

		private MaterialSingleLineTextField usernameTxt;

		private IconButton fanpageBtn;

		private ToolTip toolTip_0;

		private Panel panelLogo;

		private IconPictureBox deleteCacheBtn;

		private PictureBox pictureBox1;

		private Label verTxt;

		private PictureBox homeBrowser;

		private CheckBox khonghiennua;

		private GroupBox selectUser;

		private System.Windows.Forms.Timer timer_0;

		private IconButton thongbaolbTxt;

		private MaterialCheckBox saveinfo;

		private MetroComboBox modeFlash;

		private MetroComboBox accountsaveList;

		private IconButton clearAllaccountBtn;
        private IContainer components;
        internal static LoginFrm FNPxmtOg994h7ENE3WF;

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ClassStyle |= 131072;
				return createParams;
			}
		}

		[DllImport("Gdi32.dll")]
		private static extern IntPtr CreateRoundRectRgn(int int_0, int int_1, int int_2, int int_3, int int_4, int int_5);

		public LoginFrm()
		{
			if (Environment.OSVersion.Version.Major >= 6)
			{
				SetProcessDPIAware();
			}
			InitializeComponent();
			if (GetWindowsScaling() >= 125)
			{
				loginBtn.Font = new Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
				regBtn.Font = new Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
				fanpageBtn.Font = new Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
				forgotbtn.Font = new Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			}
			saveinfo.Checked = Settings.Default.saveinfo;
			verTxt.Text = "Launcher " + ApplicationConfig.ServerDomainName + " ver " + ControlMgr.CurrentVersion;
			panel_0 = new Panel();
			panel_0.Size = new System.Drawing.Size(7, 67);
			bunipanelLeft.Controls.Add(panel_0);
			homeBrowser.Dock = DockStyle.Fill;
			homeBrowser.Visible = true;
			//loginPanelRight.Visible = false;
			try
			{
				homeBrowser.Load(ApplicationConfig.UrlHome + "launcher/update/bg.jpg");
			}
			catch (Exception ex)
			{
				ControlMgr.UpLogLauncher(ex.ToString() + "-(LoginFrm())- Now Version : " + Assembly.GetEntryAssembly().GetName().Version);
			}
			homeBrowser.BringToFront();
			menuRighttop.Visible = false;
			khonghiennua.Visible = true;
			khonghiennua.BringToFront();
			try
			{
				AutoUpdater.Start(ApplicationConfig.UrlHome + "launcher/version.xml");
			}
			catch (Exception ex2)
			{
				ControlMgr.UpLogLauncher(ex2.ToString() + "-(LoginFrm())- Now Version : " + Assembly.GetEntryAssembly().GetName().Version);
			}
			khonghiennua.Checked = Settings.Default.khonghienthianhlogo;
			if (Settings.Default.khonghienthianhlogo)
			{
				menuRighttop.Visible = true;
				homeBrowser.Visible = false;
				//loginPanelRight.Visible = true;
				//loginPanelRight.Dock = DockStyle.Fill;
				usernameTxt.Width = matkhauTxt.Width;
				khonghiennua.Visible = false;
			}
			try
			{
				method_0();
			}
			catch (Exception ex3)
			{
				ControlMgr.UpLogLauncher(ex3.ToString() + "-(LoginFrm())- Now Version : " + Assembly.GetEntryAssembly().GetName().Version);
			}
			((ListControl)(object)modeFlash).SelectedIndex = Settings.Default.ModeFlash;
			base.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, base.Width, base.Height, 20, 20));
			base.AcceptButton = dangnhapBtn;
			//this.Show();
		}

		public static String GetHashHMACSHA256(String text, String key)
		{
			Byte[] textBytes = Encoding.UTF8.GetBytes(text);
			Byte[] keyBytes = Encoding.UTF8.GetBytes(key);

			Byte[] hashBytes;

			using (System.Security.Cryptography.HMACSHA256 hash = new System.Security.Cryptography.HMACSHA256(keyBytes))
				hashBytes = hash.ComputeHash(textBytes);
			return Convert.ToBase64String(hashBytes);
		}

		private void method_0()
		{
			try
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				string path = Path.GetTempPath() + "\\" + ControlMgr.smethod_0("AssemblyDataGhu") + "\\";
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				string path2 = Path.GetTempPath() + ControlMgr.smethod_0("AssemblyDataGhu") + "\\" + ControlMgr.smethod_0("user");
				if (!File.Exists(path2))
				{
					File.WriteAllText(path2, "");
				}
				string[] array = null;
				string_0 = File.ReadAllText(path2);
				string[] array2 = string_0.Split(';');
				if (string.IsNullOrEmpty(string_0))
				{
					dictionary.Add("Chưa có tài khoản nào được lưu", "0");
				}
				else
				{
					dictionary.Add("Chọn tài khoản đã lưu", "0");
					if (array2.Length >= 1)
					{
						clearAllaccountBtn.Visible = true;
						string[] array3 = array2;
						foreach (string text in array3)
						{
							array = text.Split('|');
							string key = Encoding.ASCII.DecodeBase64(array[0]);
							string value = Encoding.UTF32.DecodeBase64(array[1]);
							if (!dictionary.ContainsKey(key))
							{
								dictionary.Add(key, value);
							}
						}
					}
				}
				((ComboBox)(object)accountsaveList).Items.Clear();
				((ComboBox)(object)accountsaveList).DataSource = new BindingSource(dictionary, null);
				((ListControl)(object)accountsaveList).DisplayMember = "Key";
				((ListControl)(object)accountsaveList).ValueMember = "Value";
			}
			catch (Exception ex)
			{
				ControlMgr.UpLogLauncher(ex.ToString() + "-(LoadUserSaved())- Now Version : " + Assembly.GetEntryAssembly().GetName().Version);
				clearAllaccountBtn_Click(null, null);
			}
		}

		public static int GetWindowsScaling()
		{
			return (int)((double)(100 * Screen.PrimaryScreen.Bounds.Width) / SystemParameters.PrimaryScreenWidth);
		}

		[DllImport("user32.dll")]
		private static extern bool SetProcessDPIAware();

		private void method_1(object object_0, Color color_0)
		{
			if (object_0 != null)
			{
				method_2();
				iconButton_0 = (IconButton)object_0;
				iconButton_0.BackColor = Color.FromArgb(234, 109, 9);
				iconButton_0.ForeColor = color_0;
			}
		}

		private void method_2()
		{
			if (iconButton_0 != null)
			{
				iconButton_0.BackColor = Color.Transparent;
				iconButton_0.ForeColor = Color.White;
				iconButton_0.IconColor = Color.White;
			}
		}

		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

		[DllImport("user32.dll")]
		public static extern bool ReleaseCapture();

		private void LoginFrm_MouseDown(object sender, MouseEventArgs e)
		{
			method_3(e);
		}

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
			if (System.Windows.Forms.MessageBox.Show("Bạn có chắc là muốn thoát Launcher?", "Thoát Launcher", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
			{
				timer_0.Start();
			}
		}

		private void backgroundWorker_0_DoWork(object sender, DoWorkEventArgs e)
		{
			e.Cancel = true;
		}

		public void OpenPlayGame()
		{
			if (base.InvokeRequired)
			{
				Invoke((MethodInvoker)delegate
				{
					OpenPlayGame();
				});
				return;
			}
			Close();
			thread_0 = new Thread(method_5);
			thread_0.SetApartmentState(ApartmentState.STA);
			thread_0.Start();
		}

		private void method_5()
		{
			System.Windows.Forms.Application.Run(new Form1());
		}

		private void method_6(object sender, EventArgs e)
		{
			regBtn.PerformClick();
		}

		private void method_7(object sender, EventArgs e)
		{
			forgotbtn.PerformClick();
		}

		private void method_8()
		{
			method_2();
			panel_0.Visible = false;
			//loginPanelRight.Visible = false;
			homeBrowser.Visible = true;
			menuRighttop.Visible = false;
			khonghiennua.Visible = true;
		}

		private void bunifuGradientPanel2_MouseDown(object sender, MouseEventArgs e)
		{
			method_3(e);
		}

		private void panelLogo_MouseDown(object sender, MouseEventArgs e)
		{
			method_3(e);
		}

		private void loginBtn_Click(object sender, EventArgs e)
		{
			method_1(sender, Color.White);
			menuRighttop.Visible = true;
			homeBrowser.Visible = false;
			//loginPanelRight.Visible = true;
			//loginPanelRight.Dock = DockStyle.Fill;
			usernameTxt.Width = matkhauTxt.Width;
			khonghiennua.Visible = false;
		}

		private void regBtn_Click(object sender, EventArgs e)
		{
            method_1(sender, Color.White);
            registerForm_0 = new RegisterForm(this);
            registerForm_0.ShowDialog();
            //ControlMgr.OpenWebsite(ApplicationConfig.RegisterUrl);
        }

		private void forgotbtn_Click(object sender, EventArgs e)
		{
            method_1(sender, Color.White);
            forgetForm_0 = new ForgetForm(this);
            forgetForm_0.ShowDialog();
            //ControlMgr.OpenWebsite(ApplicationConfig.UrlLostPassword);
        }

        private void logoBtn_Click(object sender, EventArgs e)
		{
			method_8();
		}

		private void method_9(object sender, EventArgs e)
		{
			loginBtn.BackColor = Color.FromArgb(234, 109, 9);
		}

		private void method_10(object sender, EventArgs e)
		{
			loginBtn.BackColor = Color.Transparent;
		}

		private void method_11(object sender, EventArgs e)
		{
			regBtn.BackColor = Color.FromArgb(234, 109, 9);
		}

		private void method_12(object sender, EventArgs e)
		{
			regBtn.BackColor = Color.Transparent;
		}

		private void method_13(object sender, EventArgs e)
		{
			forgotbtn.BackColor = Color.FromArgb(234, 109, 9);
		}

		private void method_14(object sender, EventArgs e)
		{
			forgotbtn.BackColor = Color.Transparent;
		}

		private void menuRighttop_MouseDown(object sender, MouseEventArgs e)
		{
			method_3(e);
		}

		private void exitBtn_MouseEnter(object sender, EventArgs e)
		{
			exitBtn.BackColor = Color.FromArgb(242, 90, 56);
		}

		private void exitBtn_MouseLeave(object sender, EventArgs e)
		{
			exitBtn.BackColor = Color.Transparent;
		}

		private void miniBtn_Click(object sender, EventArgs e)
		{
			base.WindowState = FormWindowState.Minimized;
		}

		private void miniBtn_MouseEnter(object sender, EventArgs e)
		{
			miniBtn.BackColor = Color.FromArgb(255, 211, 155);
		}

		private void miniBtn_MouseLeave(object sender, EventArgs e)
		{
			miniBtn.BackColor = Color.Transparent;
		}

		private void exitBtn_Click(object sender, EventArgs e)
		{
			if (System.Windows.Forms.MessageBox.Show("Bạn có chắc là muốn thoát Launcher?", "Thoát Launcher", MessageBoxButtons.YesNo, MessageBoxIcon.Hand) == DialogResult.Yes)
			{
				//timer_0.Start();
				this.Close();
			}
		}

		private void dangnhapBtn_Click(object sender, EventArgs e)
		{
			string text = ControlMgr.xoabug(usernameTxt.Text).ToLower();
			string text2 = matkhauTxt.Text;
			string text3 = (LoginMgr.PasswordReal = ControlMgr.smethod_0(text2).ToUpper());
			if (text.Length >= 4)
			{
				if (text2.Length >= 6 && text2.Length <= 100)
				{
					string[] string_0 = AccountAPI.Login(text, text2);
					if (string_0[0] == "1")
					{
						string text4 = Encoding.ASCII.EncodeBase64(usernameTxt.Text);
						string text5 = Encoding.UTF32.EncodeBase64(matkhauTxt.Text);
						string path = Path.GetTempPath() + ControlMgr.smethod_0("AssemblyDataGhu") + "\\" + ControlMgr.smethod_0("user");
						LoginMgr.Login(string_0[1], string_0[2], ((ListControl)(object)modeFlash).SelectedIndex);
						if (saveinfo.Checked)
						{
							if (!(this.string_0 == ""))
							{
								bool flag = false;
								bool flag2 = false;
								string text6 = text4 + "|" + text5;
								string[] array = this.string_0.Split(';');
								string[] array2 = array;
								foreach (string text7 in array2)
								{
									if (text6 == text7)
									{
										flag = true;
									}
									string[] array3 = text7.Split('|');
									if (text4 == array3[0] && text5 != array3[1])
									{
										flag2 = true;
										this.string_0 = File.ReadAllText(path);
										this.string_0 = this.string_0.Replace(text7, text6);
										File.WriteAllText(path, this.string_0);
									}
								}
								if (!flag2)
								{
									string[] array4 = array;
									foreach (string text8 in array4)
									{
										if (text6 != text8 && !flag)
										{
											File.AppendAllText(path, ";" + text6);
											flag = true;
										}
									}
								}
							}
							else
							{
								File.AppendAllText(path, text4 + "|" + text5);
							}
						}
						Settings.Default.saveinfo = saveinfo.Checked;
						Settings.Default.ModeFlash = ((ListControl)(object)modeFlash).SelectedIndex;
						Settings.Default.Save();
						OpenPlayGame();
					}
					else
					{
						thongbaolbTxt.Invoke((MethodInvoker)delegate
						{
							thongbaolbTxt.Visible = true;
							thongbaolbTxt.Text = string_0[1];
						});
					}
				}
				else
				{
					thongbaolbTxt.Visible = true;
					thongbaolbTxt.Text = "Vui lòng nhập mật khẩu hợp lệ!";
				}
			}
			else
			{
				thongbaolbTxt.Visible = true;
				thongbaolbTxt.Text = "Vui lòng nhập ta\u0300i khoa\u0309n hợp lệ!";
			}
		}

		private void method_15(object sender, EventArgs e)
		{
			if (bunipanelLeft.Visible)
			{
				base.Width -= bunipanelLeft.Width;
				bunipanelLeft.Visible = false;
			}
			else
			{
				base.Width += bunipanelLeft.Width;
				bunipanelLeft.Visible = true;
			}
		}

		private void fanpageBtn_Click(object sender, EventArgs e)
		{
			ControlMgr.OpenWebsite(ApplicationConfig.FanpageLink);
		}

		private void matkhauTxt_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				dangnhapBtn_Click(sender, e);
			}
		}

		private void deleteCacheBtn_Click(object sender, EventArgs e)
		{
			//if (System.Windows.Forms.MessageBox.Show("Bạn có muốn xóa toàn bộ cache không ?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			//{
			//	Settings.Default.Save();
			//	Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 4351");
			//}
			if (System.Windows.Forms.MessageBox.Show("Bạn có muốn xóa toàn bộ cache không ?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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

		private void deleteCacheBtn_MouseEnter(object sender, EventArgs e)
		{
			deleteCacheBtn.BackColor = Color.FromArgb(255, 211, 155);
		}

		private void deleteCacheBtn_MouseLeave(object sender, EventArgs e)
		{
			deleteCacheBtn.BackColor = Color.Transparent;
		}

		private void LoginFrm_Load(object sender, EventArgs e)
		{
		}

		private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
		{
			method_3(e);
		}

		private void khonghiennua_CheckedChanged(object sender, EventArgs e)
		{
			Settings.Default.khonghienthianhlogo = khonghiennua.Checked;
			Settings.Default.Save();
		}

		private void timer_0_Tick(object sender, EventArgs e)
		{
			base.Opacity -= 0.014;
			if (base.Opacity <= 0.01)
			{
				timer_0.Stop();
				System.Windows.Forms.Application.Exit();
			}
		}

		private void clearAllaccountBtn_Click(object sender, EventArgs e)
		{
			string path = Path.GetTempPath() + ControlMgr.smethod_0("AssemblyDataGhu") + "\\" + ControlMgr.smethod_0("user");
			File.WriteAllText(path, "");
			((ComboBox)(object)accountsaveList).DataSource = null;
			((ComboBox)(object)accountsaveList).Items.Clear();
			method_0();
		}

		private void accountsaveList_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (((ListControl)(object)accountsaveList).SelectedIndex > 0)
			{
				usernameTxt.Text = ((Control)(object)accountsaveList).Text.ToString();
				matkhauTxt.Text = ((ListControl)(object)accountsaveList).SelectedValue.ToString();
				dangnhapBtn.PerformClick();
			}
		}

		public void LoginByForm(string username, string password, Form frm)
		{
			if (frm is RegisterForm)
			{
				registerForm_0.Close();
			}
			if (frm is ForgetForm)
			{
				forgetForm_0.Close();
			}
			usernameTxt.Text = username;
			matkhauTxt.Text = password;
			dangnhapBtn_Click(null, null);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginFrm));
            this.backgroundWorker_0 = new System.ComponentModel.BackgroundWorker();
            this.openFileDialog_0 = new System.Windows.Forms.OpenFileDialog();
            this.toolTip_0 = new System.Windows.Forms.ToolTip(this.components);
            this.timer_0 = new System.Windows.Forms.Timer(this.components);
            this.bunipanelLeft = new ns1.BunifuGradientPanel();
            this.fanpageBtn = new FontAwesome.Sharp.IconButton();
            this.verTxt = new System.Windows.Forms.Label();
            this.forgotbtn = new FontAwesome.Sharp.IconButton();
            this.regBtn = new FontAwesome.Sharp.IconButton();
            this.loginBtn = new FontAwesome.Sharp.IconButton();
            this.panelLogo = new System.Windows.Forms.Panel();
            this.logoBtn = new System.Windows.Forms.PictureBox();
            this.bunifuGradientPanel2 = new ns1.BunifuGradientPanel();
            this.menuRighttop = new ns1.BunifuGradientPanel();
            this.deleteCacheBtn = new FontAwesome.Sharp.IconPictureBox();
            this.miniBtn = new FontAwesome.Sharp.IconPictureBox();
            this.exitBtn = new FontAwesome.Sharp.IconPictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            //this.loginPanelRight = new System.Windows.Forms.Panel();
            this.modeFlash = new MetroFramework.Controls.MetroComboBox();
            this.thongbaolbTxt = new FontAwesome.Sharp.IconButton();
            this.clearAllaccountBtn = new FontAwesome.Sharp.IconButton();
            this.selectUser = new System.Windows.Forms.GroupBox();
            this.accountsaveList = new MetroFramework.Controls.MetroComboBox();
            this.usernameTxt = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.saveinfo = new MaterialSkin.Controls.MaterialCheckBox();
            this.matkhauTxt = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.dangnhapBtn = new MaterialSkin.Controls.MaterialRaisedButton();
            this.homeBrowser = new System.Windows.Forms.PictureBox();
            this.khonghiennua = new System.Windows.Forms.CheckBox();
            this.bunipanelLeft.SuspendLayout();
            this.panelLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoBtn)).BeginInit();
            this.bunifuGradientPanel2.SuspendLayout();
            this.menuRighttop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.deleteCacheBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.miniBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.exitBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            //this.loginPanelRight.SuspendLayout();
            this.selectUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.homeBrowser)).BeginInit();
            this.SuspendLayout();
            // 
            // backgroundWorker_0
            // 
            this.backgroundWorker_0.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_0_DoWork);
            // 
            // openFileDialog_0
            // 
            this.openFileDialog_0.FileName = "openFileDialog1";
            // 
            // timer_0
            // 
            this.timer_0.Interval = 10;
            this.timer_0.Tick += new System.EventHandler(this.timer_0_Tick);
            // 
            // bunipanelLeft
            // 
            this.bunipanelLeft.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bunipanelLeft.BackgroundImage")));
            this.bunipanelLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bunipanelLeft.Controls.Add(this.fanpageBtn);
            this.bunipanelLeft.Controls.Add(this.verTxt);
            this.bunipanelLeft.Controls.Add(this.forgotbtn);
            this.bunipanelLeft.Controls.Add(this.regBtn);
            this.bunipanelLeft.Controls.Add(this.loginBtn);
            this.bunipanelLeft.Controls.Add(this.panelLogo);
            this.bunipanelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.bunipanelLeft.GradientBottomLeft = System.Drawing.Color.DarkGoldenrod;
            this.bunipanelLeft.GradientBottomRight = System.Drawing.Color.SaddleBrown;
            this.bunipanelLeft.GradientTopLeft = System.Drawing.Color.Crimson;
            this.bunipanelLeft.GradientTopRight = System.Drawing.Color.Navy;
            this.bunipanelLeft.Location = new System.Drawing.Point(0, 0);
            this.bunipanelLeft.Margin = new System.Windows.Forms.Padding(2);
            this.bunipanelLeft.Name = "bunipanelLeft";
            this.bunipanelLeft.Quality = 10;
            this.bunipanelLeft.Size = new System.Drawing.Size(262, 307);
            this.bunipanelLeft.TabIndex = 0;
            this.bunipanelLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LoginFrm_MouseDown);
            // 
            // fanpageBtn
            // 
            this.fanpageBtn.BackColor = System.Drawing.Color.Transparent;
            this.fanpageBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.fanpageBtn.FlatAppearance.BorderSize = 0;
            this.fanpageBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(109)))), ((int)(((byte)(9)))));
            this.fanpageBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.fanpageBtn.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fanpageBtn.ForeColor = System.Drawing.Color.White;
            this.fanpageBtn.IconChar = FontAwesome.Sharp.IconChar.FacebookSquare;
            this.fanpageBtn.IconColor = System.Drawing.SystemColors.ButtonFace;
            this.fanpageBtn.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.fanpageBtn.IconSize = 40;
            this.fanpageBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.fanpageBtn.Location = new System.Drawing.Point(0, 232);
            this.fanpageBtn.Margin = new System.Windows.Forms.Padding(2);
            this.fanpageBtn.Name = "fanpageBtn";
            this.fanpageBtn.Size = new System.Drawing.Size(261, 49);
            this.fanpageBtn.TabIndex = 7;
            this.fanpageBtn.TabStop = false;
            this.fanpageBtn.Text = "     FANPAGE";
            this.toolTip_0.SetToolTip(this.fanpageBtn, "Truy cập Fanpage");
            this.fanpageBtn.UseVisualStyleBackColor = false;
            this.fanpageBtn.Click += new System.EventHandler(this.fanpageBtn_Click);
            // 
            // verTxt
            // 
            this.verTxt.AutoSize = true;
            this.verTxt.BackColor = System.Drawing.Color.Transparent;
            this.verTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.verTxt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.verTxt.Location = new System.Drawing.Point(5, 290);
            this.verTxt.Name = "verTxt";
            this.verTxt.Size = new System.Drawing.Size(42, 13);
            this.verTxt.TabIndex = 6;
            this.verTxt.Text = "Version";
            // 
            // forgotbtn
            // 
            this.forgotbtn.BackColor = System.Drawing.Color.Transparent;
            this.forgotbtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.forgotbtn.FlatAppearance.BorderSize = 0;
            this.forgotbtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(109)))), ((int)(((byte)(9)))));
            this.forgotbtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.forgotbtn.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.forgotbtn.ForeColor = System.Drawing.Color.White;
            this.forgotbtn.IconChar = FontAwesome.Sharp.IconChar.Keycdn;
            this.forgotbtn.IconColor = System.Drawing.SystemColors.ButtonFace;
            this.forgotbtn.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.forgotbtn.IconSize = 40;
            this.forgotbtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.forgotbtn.Location = new System.Drawing.Point(1, 183);
            this.forgotbtn.Margin = new System.Windows.Forms.Padding(2);
            this.forgotbtn.Name = "forgotbtn";
            this.forgotbtn.Size = new System.Drawing.Size(261, 49);
            this.forgotbtn.TabIndex = 4;
            this.forgotbtn.TabStop = false;
            this.forgotbtn.Text = "     QUÊN MẬT KHẨU";
            this.toolTip_0.SetToolTip(this.forgotbtn, "Quên mật khẩu");
            this.forgotbtn.UseVisualStyleBackColor = false;
            this.forgotbtn.Click += new System.EventHandler(this.forgotbtn_Click);
            // 
            // regBtn
            // 
            this.regBtn.BackColor = System.Drawing.Color.Transparent;
            this.regBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.regBtn.FlatAppearance.BorderSize = 0;
            this.regBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(109)))), ((int)(((byte)(9)))));
            this.regBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.regBtn.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.regBtn.ForeColor = System.Drawing.Color.White;
            this.regBtn.IconChar = FontAwesome.Sharp.IconChar.Registered;
            this.regBtn.IconColor = System.Drawing.SystemColors.ButtonFace;
            this.regBtn.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.regBtn.IconSize = 40;
            this.regBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.regBtn.Location = new System.Drawing.Point(1, 134);
            this.regBtn.Margin = new System.Windows.Forms.Padding(2);
            this.regBtn.Name = "regBtn";
            this.regBtn.Size = new System.Drawing.Size(261, 49);
            this.regBtn.TabIndex = 3;
            this.regBtn.TabStop = false;
            this.regBtn.Text = "     ĐĂNG KÝ";
            this.toolTip_0.SetToolTip(this.regBtn, "Đăng ký");
            this.regBtn.UseVisualStyleBackColor = false;
            this.regBtn.Click += new System.EventHandler(this.regBtn_Click);
            // 
            // loginBtn
            // 
            this.loginBtn.BackColor = System.Drawing.Color.Transparent;
            this.loginBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.loginBtn.FlatAppearance.BorderSize = 0;
            this.loginBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(109)))), ((int)(((byte)(9)))));
            this.loginBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loginBtn.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loginBtn.ForeColor = System.Drawing.Color.White;
            this.loginBtn.IconChar = FontAwesome.Sharp.IconChar.SignInAlt;
            this.loginBtn.IconColor = System.Drawing.SystemColors.ButtonFace;
            this.loginBtn.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.loginBtn.IconSize = 40;
            this.loginBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.loginBtn.Location = new System.Drawing.Point(1, 85);
            this.loginBtn.Margin = new System.Windows.Forms.Padding(2);
            this.loginBtn.Name = "loginBtn";
            this.loginBtn.Size = new System.Drawing.Size(261, 49);
            this.loginBtn.TabIndex = 2;
            this.loginBtn.TabStop = false;
            this.loginBtn.Text = "     ĐĂNG NHẬP";
            this.toolTip_0.SetToolTip(this.loginBtn, "Đăng nhập");
            this.loginBtn.UseVisualStyleBackColor = false;
            this.loginBtn.Click += new System.EventHandler(this.loginBtn_Click);
            // 
            // panelLogo
            // 
            this.panelLogo.BackColor = System.Drawing.Color.Transparent;
            this.panelLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panelLogo.Controls.Add(this.logoBtn);
            this.panelLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelLogo.Location = new System.Drawing.Point(0, 0);
            this.panelLogo.Margin = new System.Windows.Forms.Padding(2);
            this.panelLogo.Name = "panelLogo";
            this.panelLogo.Size = new System.Drawing.Size(262, 99);
            this.panelLogo.TabIndex = 1;
            this.panelLogo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelLogo_MouseDown);
            // 
            // logoBtn
            // 
            this.logoBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.logoBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.logoBtn.Image = ((System.Drawing.Image)(resources.GetObject("logoBtn.Image")));
            this.logoBtn.Location = new System.Drawing.Point(35, 2);
            this.logoBtn.Name = "logoBtn";
            this.logoBtn.Size = new System.Drawing.Size(201, 76);
            this.logoBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.logoBtn.TabIndex = 0;
            this.logoBtn.TabStop = false;
            this.toolTip_0.SetToolTip(this.logoBtn, "Trang chủ");
            this.logoBtn.Click += new System.EventHandler(this.logoBtn_Click);
            // 
            // bunifuGradientPanel2
            // 
            this.bunifuGradientPanel2.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.bunifuGradientPanel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bunifuGradientPanel2.BackgroundImage")));
            this.bunifuGradientPanel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bunifuGradientPanel2.Controls.Add(this.menuRighttop);
            //this.bunifuGradientPanel2.Controls.Add(this.loginPanelRight);
            this.bunifuGradientPanel2.Controls.Add(this.homeBrowser);
            this.bunifuGradientPanel2.Controls.Add(this.khonghiennua);
            this.bunifuGradientPanel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.bunifuGradientPanel2.GradientBottomLeft = System.Drawing.Color.Orange;
            this.bunifuGradientPanel2.GradientBottomRight = System.Drawing.Color.Gold;
            this.bunifuGradientPanel2.GradientTopLeft = System.Drawing.Color.SandyBrown;
            this.bunifuGradientPanel2.GradientTopRight = System.Drawing.Color.LightSalmon;
            this.bunifuGradientPanel2.Location = new System.Drawing.Point(255, 0);
            this.bunifuGradientPanel2.Margin = new System.Windows.Forms.Padding(2);
            this.bunifuGradientPanel2.Name = "bunifuGradientPanel2";
            this.bunifuGradientPanel2.Quality = 10;
            this.bunifuGradientPanel2.Size = new System.Drawing.Size(536, 307);
            this.bunifuGradientPanel2.TabIndex = 1;
            this.bunifuGradientPanel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.bunifuGradientPanel2_MouseDown);
            // 
            // menuRighttop
            // 
            this.menuRighttop.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("menuRighttop.BackgroundImage")));
            this.menuRighttop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.menuRighttop.Controls.Add(this.deleteCacheBtn);
            this.menuRighttop.Controls.Add(this.miniBtn);
            this.menuRighttop.Controls.Add(this.exitBtn);
            this.menuRighttop.Controls.Add(this.pictureBox1);
            this.menuRighttop.Dock = System.Windows.Forms.DockStyle.Top;
            this.menuRighttop.GradientBottomLeft = System.Drawing.Color.DarkOrchid;
            this.menuRighttop.GradientBottomRight = System.Drawing.Color.SandyBrown;
            this.menuRighttop.GradientTopLeft = System.Drawing.Color.DarkGoldenrod;
            this.menuRighttop.GradientTopRight = System.Drawing.Color.Olive;
            this.menuRighttop.Location = new System.Drawing.Point(0, 0);
            this.menuRighttop.Name = "menuRighttop";
            this.menuRighttop.Quality = 10;
            this.menuRighttop.Size = new System.Drawing.Size(536, 40);
            this.menuRighttop.TabIndex = 6;
            this.menuRighttop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.menuRighttop_MouseDown);
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
            this.deleteCacheBtn.IconSize = 20;
            this.deleteCacheBtn.Location = new System.Drawing.Point(447, 5);
            this.deleteCacheBtn.Margin = new System.Windows.Forms.Padding(5);
            this.deleteCacheBtn.Name = "deleteCacheBtn";
            this.deleteCacheBtn.Size = new System.Drawing.Size(24, 20);
            this.deleteCacheBtn.TabIndex = 13;
            this.deleteCacheBtn.TabStop = false;
            this.toolTip_0.SetToolTip(this.deleteCacheBtn, "Xóa Cache");
            this.deleteCacheBtn.Click += new System.EventHandler(this.deleteCacheBtn_Click);
            this.deleteCacheBtn.MouseEnter += new System.EventHandler(this.deleteCacheBtn_MouseEnter);
            this.deleteCacheBtn.MouseLeave += new System.EventHandler(this.deleteCacheBtn_MouseLeave);
            // 
            // miniBtn
            // 
            this.miniBtn.BackColor = System.Drawing.Color.Transparent;
            this.miniBtn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.miniBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.miniBtn.IconChar = FontAwesome.Sharp.IconChar.Minus;
            this.miniBtn.IconColor = System.Drawing.Color.White;
            this.miniBtn.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.miniBtn.IconSize = 20;
            this.miniBtn.Location = new System.Drawing.Point(475, 5);
            this.miniBtn.Name = "miniBtn";
            this.miniBtn.Size = new System.Drawing.Size(24, 20);
            this.miniBtn.TabIndex = 3;
            this.miniBtn.TabStop = false;
            this.toolTip_0.SetToolTip(this.miniBtn, "Ẩn launcher");
            this.miniBtn.Click += new System.EventHandler(this.miniBtn_Click);
            this.miniBtn.MouseEnter += new System.EventHandler(this.miniBtn_MouseEnter);
            this.miniBtn.MouseLeave += new System.EventHandler(this.miniBtn_MouseLeave);
            // 
            // exitBtn
            // 
            this.exitBtn.BackColor = System.Drawing.Color.Transparent;
            this.exitBtn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.exitBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.exitBtn.IconChar = FontAwesome.Sharp.IconChar.Times;
            this.exitBtn.IconColor = System.Drawing.Color.White;
            this.exitBtn.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.exitBtn.IconSize = 20;
            this.exitBtn.Location = new System.Drawing.Point(503, 5);
            this.exitBtn.Name = "exitBtn";
            this.exitBtn.Size = new System.Drawing.Size(24, 20);
            this.exitBtn.TabIndex = 2;
            this.exitBtn.TabStop = false;
            this.toolTip_0.SetToolTip(this.exitBtn, "Thoát launcher");
            this.exitBtn.Click += new System.EventHandler(this.exitBtn_Click);
            this.exitBtn.MouseEnter += new System.EventHandler(this.exitBtn_MouseEnter);
            this.exitBtn.MouseLeave += new System.EventHandler(this.exitBtn_MouseLeave);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(91, -7);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(330, 57);
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            // 
            // loginPanelRight
            // 
            //this.loginPanelRight.BackColor = System.Drawing.Color.SandyBrown;
            //this.loginPanelRight.Controls.Add(this.modeFlash);
            //this.loginPanelRight.Controls.Add(this.thongbaolbTxt);
            //this.loginPanelRight.Controls.Add(this.clearAllaccountBtn);
            //this.loginPanelRight.Controls.Add(this.selectUser);
            //this.loginPanelRight.Controls.Add(this.usernameTxt);
            //this.loginPanelRight.Controls.Add(this.saveinfo);
            //this.loginPanelRight.Controls.Add(this.matkhauTxt);
            //this.loginPanelRight.Controls.Add(this.dangnhapBtn);
            //this.loginPanelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.loginPanelRight.ForeColor = System.Drawing.Color.Maroon;
            //this.loginPanelRight.Location = new System.Drawing.Point(0, 0);
            //this.loginPanelRight.Name = "loginPanelRight";
            //this.loginPanelRight.Size = new System.Drawing.Size(536, 307);
            //this.loginPanelRight.TabIndex = 8;
            //this.loginPanelRight.Visible = false;
            //this.loginPanelRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LoginFrm_MouseDown);
            // 
            // modeFlash
            // 
            this.modeFlash.BackColor = System.Drawing.Color.White;
            this.modeFlash.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.modeFlash.ForeColor = System.Drawing.Color.Maroon;
            this.modeFlash.FormattingEnabled = true;
            this.modeFlash.ItemHeight = 23;
            this.modeFlash.Items.AddRange(new object[] {
            "Chế Độ Client GunnyLocPhat",
			"Chế Độ Flash GunnyLocPhat"});
            this.modeFlash.Location = new System.Drawing.Point(45, 194);
            this.modeFlash.Name = "modeFlash";
            this.modeFlash.Size = new System.Drawing.Size(447, 29);
            this.modeFlash.Style = MetroFramework.MetroColorStyle.Orange;
            this.modeFlash.TabIndex = 20;
            this.modeFlash.UseSelectable = true;
            // 
            // thongbaolbTxt
            // 
            this.thongbaolbTxt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.thongbaolbTxt.FlatAppearance.BorderSize = 0;
            this.thongbaolbTxt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.thongbaolbTxt.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.thongbaolbTxt.IconChar = FontAwesome.Sharp.IconChar.ExclamationCircle;
            this.thongbaolbTxt.IconColor = System.Drawing.Color.Maroon;
            this.thongbaolbTxt.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.thongbaolbTxt.IconSize = 16;
            this.thongbaolbTxt.Location = new System.Drawing.Point(-1, 280);
            this.thongbaolbTxt.Name = "thongbaolbTxt";
            this.thongbaolbTxt.Size = new System.Drawing.Size(273, 25);
            this.thongbaolbTxt.TabIndex = 19;
            this.thongbaolbTxt.Text = " Xóa hết tài khoản đã lưu";
            this.thongbaolbTxt.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip_0.SetToolTip(this.thongbaolbTxt, "Xóa tất cả tài khoản đã lưu");
            this.thongbaolbTxt.UseVisualStyleBackColor = true;
            this.thongbaolbTxt.Visible = false;
            // 
            // clearAllaccountBtn
            // 
            this.clearAllaccountBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.clearAllaccountBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearAllaccountBtn.IconChar = FontAwesome.Sharp.IconChar.TrashAlt;
            this.clearAllaccountBtn.IconColor = System.Drawing.Color.Black;
            this.clearAllaccountBtn.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.clearAllaccountBtn.IconSize = 16;
            this.clearAllaccountBtn.Location = new System.Drawing.Point(369, 114);
            this.clearAllaccountBtn.Name = "clearAllaccountBtn";
            this.clearAllaccountBtn.Size = new System.Drawing.Size(160, 25);
            this.clearAllaccountBtn.TabIndex = 18;
            this.clearAllaccountBtn.Text = "  Xóa hết tài khoản đã lưu";
            this.clearAllaccountBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip_0.SetToolTip(this.clearAllaccountBtn, "Xóa tất cả tài khoản đã lưu");
            this.clearAllaccountBtn.UseVisualStyleBackColor = true;
            this.clearAllaccountBtn.Visible = false;
            this.clearAllaccountBtn.Click += new System.EventHandler(this.clearAllaccountBtn_Click);
            // 
            // selectUser
            // 
            this.selectUser.Controls.Add(this.accountsaveList);
            this.selectUser.Font = new System.Drawing.Font("Tahoma", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectUser.Location = new System.Drawing.Point(14, 43);
            this.selectUser.Name = "selectUser";
            this.selectUser.Size = new System.Drawing.Size(514, 69);
            this.selectUser.TabIndex = 18;
            this.selectUser.TabStop = false;
            this.selectUser.Text = "Đăng nhập bằng tài khoản đã lưu :";
            // 
            // accountsaveList
            // 
            this.accountsaveList.FormattingEnabled = true;
            this.accountsaveList.ItemHeight = 23;
            this.accountsaveList.Location = new System.Drawing.Point(30, 24);
            this.accountsaveList.Name = "accountsaveList";
            this.accountsaveList.Size = new System.Drawing.Size(448, 29);
            this.accountsaveList.TabIndex = 0;
            this.accountsaveList.UseSelectable = true;
            this.accountsaveList.SelectedIndexChanged += new System.EventHandler(this.accountsaveList_SelectedIndexChanged);
            // 
            // usernameTxt
            // 
            this.usernameTxt.Depth = 0;
            this.usernameTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.usernameTxt.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.usernameTxt.Hint = "Tài khoản";
            this.usernameTxt.Location = new System.Drawing.Point(45, 133);
            this.usernameTxt.Margin = new System.Windows.Forms.Padding(2);
            this.usernameTxt.MaxLength = 32767;
            this.usernameTxt.MouseState = MaterialSkin.MouseState.HOVER;
            this.usernameTxt.Name = "usernameTxt";
            this.usernameTxt.PasswordChar = '\0';
            this.usernameTxt.SelectedText = "";
            this.usernameTxt.SelectionLength = 0;
            this.usernameTxt.SelectionStart = 0;
            this.usernameTxt.Size = new System.Drawing.Size(448, 23);
            this.usernameTxt.TabIndex = 0;
            this.usernameTxt.TabStop = false;
            this.usernameTxt.UseSystemPasswordChar = false;
            // 
            // saveinfo
            // 
            this.saveinfo.AutoSize = true;
            this.saveinfo.Depth = 0;
            this.saveinfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveinfo.Font = new System.Drawing.Font("Roboto", 10F);
            this.saveinfo.ForeColor = System.Drawing.Color.White;
            this.saveinfo.Location = new System.Drawing.Point(369, 277);
            this.saveinfo.Margin = new System.Windows.Forms.Padding(0);
            this.saveinfo.MouseLocation = new System.Drawing.Point(-1, -1);
            this.saveinfo.MouseState = MaterialSkin.MouseState.HOVER;
            this.saveinfo.Name = "saveinfo";
            this.saveinfo.Ripple = true;
            this.saveinfo.Size = new System.Drawing.Size(111, 30);
            this.saveinfo.TabIndex = 6;
            this.saveinfo.Text = "Lưu thông tin";
            this.toolTip_0.SetToolTip(this.saveinfo, "Lưu thông tin đăng nhập");
            this.saveinfo.UseVisualStyleBackColor = true;
            // 
            // matkhauTxt
            // 
            this.matkhauTxt.Depth = 0;
            this.matkhauTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.matkhauTxt.ForeColor = System.Drawing.Color.White;
            this.matkhauTxt.Hint = "Mật khẩu";
            this.matkhauTxt.Location = new System.Drawing.Point(45, 164);
            this.matkhauTxt.Margin = new System.Windows.Forms.Padding(2);
            this.matkhauTxt.MaxLength = 32767;
            this.matkhauTxt.MouseState = MaterialSkin.MouseState.HOVER;
            this.matkhauTxt.Name = "matkhauTxt";
            this.matkhauTxt.PasswordChar = '\0';
            this.matkhauTxt.SelectedText = "";
            this.matkhauTxt.SelectionLength = 0;
            this.matkhauTxt.SelectionStart = 0;
            this.matkhauTxt.Size = new System.Drawing.Size(448, 23);
            this.matkhauTxt.TabIndex = 1;
            this.matkhauTxt.TabStop = false;
            this.matkhauTxt.UseSystemPasswordChar = true;
            this.matkhauTxt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.matkhauTxt_KeyDown);
            // 
            // dangnhapBtn
            // 
            this.dangnhapBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dangnhapBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.dangnhapBtn.BackColor = System.Drawing.Color.White;
            this.dangnhapBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dangnhapBtn.Depth = 0;
            this.dangnhapBtn.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.dangnhapBtn.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dangnhapBtn.Icon = null;
            this.dangnhapBtn.Location = new System.Drawing.Point(44, 231);
            this.dangnhapBtn.Margin = new System.Windows.Forms.Padding(2);
            this.dangnhapBtn.MouseState = MaterialSkin.MouseState.HOVER;
            this.dangnhapBtn.Name = "dangnhapBtn";
            this.dangnhapBtn.Primary = true;
            this.dangnhapBtn.Size = new System.Drawing.Size(448, 45);
            this.dangnhapBtn.TabIndex = 2;
            this.dangnhapBtn.Text = "ĐĂNG NHẬP";
            this.dangnhapBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.dangnhapBtn.UseVisualStyleBackColor = false;
            this.dangnhapBtn.Click += new System.EventHandler(this.dangnhapBtn_Click);
            // 
            // homeBrowser
            // 
            this.homeBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.homeBrowser.Location = new System.Drawing.Point(0, 0);
            this.homeBrowser.Name = "homeBrowser";
            this.homeBrowser.Size = new System.Drawing.Size(536, 307);
            this.homeBrowser.TabIndex = 10;
            this.homeBrowser.TabStop = false;
            this.homeBrowser.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LoginFrm_MouseDown);
            // 
            // khonghiennua
            // 
            this.khonghiennua.AutoSize = true;
            this.khonghiennua.Location = new System.Drawing.Point(11, 286);
            this.khonghiennua.Name = "khonghiennua";
            this.khonghiennua.Size = new System.Drawing.Size(155, 17);
            this.khonghiennua.TabIndex = 11;
            this.khonghiennua.Text = "Không hiển thị lại ảnh nữa !";
            this.khonghiennua.UseVisualStyleBackColor = true;
            this.khonghiennua.CheckedChanged += new System.EventHandler(this.khonghiennua_CheckedChanged);
            // 
            // LoginFrm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(791, 307);
            this.ControlBox = false;
            this.Controls.Add(this.bunipanelLeft);
            this.Controls.Add(this.bunifuGradientPanel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginFrm";
            this.Opacity = 0.95D;
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " ";
            this.Load += new System.EventHandler(this.LoginFrm_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LoginFrm_MouseDown);
            this.bunipanelLeft.ResumeLayout(false);
            this.bunipanelLeft.PerformLayout();
            this.panelLogo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logoBtn)).EndInit();
            this.bunifuGradientPanel2.ResumeLayout(false);
            this.bunifuGradientPanel2.PerformLayout();
            this.menuRighttop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.deleteCacheBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.miniBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.exitBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            //this.loginPanelRight.ResumeLayout(false);
            //this.loginPanelRight.PerformLayout();
            this.selectUser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.homeBrowser)).EndInit();
            this.ResumeLayout(false);

		}

		[CompilerGenerated]
		private void method_16()
		{
			OpenPlayGame();
		}

		internal static void JgJoBTONvOX12hGNxhd()
		{
		}

		internal static bool T4mjl4Oqu9ydp9SZsKU()
		{
			return FNPxmtOg994h7ENE3WF == null;
		}
	}
}
