using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using CircularProgressBar;
using LauncherGHU;
using Microsoft.VisualBasic.Devices;
using ns1;
using WinFormAnimation;

namespace LoadLauncher
{
	public class Form1 : Form
	{
		private string string_0;

		private WebClient webClient_0;

		private string string_1;

		private int int_0;

		private Thread thread_0;

		public const int WM_NCLBUTTONDOWN = 161;

		public const int HT_CAPTION = 2;

		private IContainer icontainer_0;

		private System.Windows.Forms.Timer timer_0;

		private PictureBox pictureBox1;

		private PictureBox pictureBox3;

		private PictureBox pictureBox2;

		private PictureBox pictureBox4;

		private PictureBox qxaQgoawi;

		private PictureBox pictureBox7;

		private BunifuCustomLabel bunifuCustomLabel1;

		private System.Windows.Forms.Timer timer_1;

		private global::CircularProgressBar.CircularProgressBar circularProgressBar1;
        private Label label1;
        private Panel bunifuGradientPanel2;
        private Panel bunifuGradientPanel1;
        private BunifuCustomLabel bunifuCustomLabel2;

        //private Label label_0;
        private IContainer components;

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams obj = base.CreateParams;
				obj.ClassStyle |= 131072;
				return obj;
			}
		}

		[DllImport("Gdi32.dll")]
		private static extern IntPtr CreateRoundRectRgn(int int_1, int int_2, int int_3, int int_4, int int_5, int int_6);

		public Form1()
		{
			if (Environment.OSVersion.Version.Major >= 6)
			{
				SetProcessDPIAware();
			}
			InitializeComponent();
			string_1 = new ComputerInfo().TotalPhysicalMemory.ToString();
			GetWindowsScaling();
			base.TopMost = true;
			ControlMgr.stopCheats();
			base.Opacity = 0.0;
			base.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, base.Width, base.Height, 50, 50));
			timer_0.Start();
		}

		private void method_0()
		{
		}

		[DllImport("user32.dll")]
		private static extern bool SetProcessDPIAware();

		private void method_1()
		{
			string[] array = ControlMgr.RequestWebForm(ApplicationConfig.UrlApi + "/serverlist.php", "").Split('|');
			if (array.Length == 0)
			{
				return;
			}
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string[] array3 = array2[i].Split(',');
				if (array3.Length >= 2 && !ControlMgr.ServerList.ContainsKey(int.Parse(array3[0])))
				{
					ServerInfo serverInfo = new ServerInfo();
					serverInfo.ServerID = int.Parse(array3[0]);
					serverInfo.ServerName = array3[1];
					serverInfo.XButton = int.Parse(array3[2]);
					serverInfo.YButton = int.Parse(array3[3]);
					ControlMgr.ServerList.Add(serverInfo.ServerID, serverInfo);
				}
			}
		}

		public static int GetWindowsScaling()
		{
			return (int)((double)(100 * Screen.PrimaryScreen.Bounds.Width) / SystemParameters.PrimaryScreenWidth);
		}

		private void method_2()
		{
			string[] array = "0|191|0|0|0|190|84|67|83|79|0|4|0|0|0|0|0|28|102|105|108|101|118|110|46|103|117|110|104|111|105|117|99|46|99|111|109|47|115|101|116|116|105|110|103|115|0|0|0|0|0|5|97|108|108|111|119|1|0|0|0|6|97|108|119|97|121|115|1|0|0|0|11|97|108|108|111|119|115|101|99|117|114|101|1|0|0|0|12|97|108|119|97|121|115|115|101|99|117|114|101|1|0|0|0|6|107|108|105|109|105|116|0|192|0|0|0|0|0|0|0|0|0|11|104|115|116|115|69|110|97|98|108|101|100|1|0|0|0|10|104|115|116|115|77|97|120|65|103|101|2|0|1|48|0|0|16|104|115|116|115|73|110|99|83|117|98|68|111|109|97|105|110|1|0|0|0|13|104|115|116|115|83|116|97|114|116|84|105|109|101|2|0|1|48|0".Split('|');
			byte[] array2 = new byte[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = byte.Parse(array[i]);
			}
			string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Macromedia\\Flash Player\\macromedia.com\\support\\flashplayer\\sys\\#filevn.gunhoiuc.com\\";
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			method_4(array2, text + "settings.sol");
		}

		private void method_3()
		{
			string[] array = "0|191|0|0|0|188|84|67|83|79|0|4|0|0|0|0|0|26|102|105|108|101|46|103|117|110|104|111|105|117|99|46|99|111|109|47|115|101|116|116|105|110|103|115|0|0|0|0|0|5|97|108|108|111|119|1|0|0|0|6|97|108|119|97|121|115|1|0|0|0|11|97|108|108|111|119|115|101|99|117|114|101|1|0|0|0|12|97|108|119|97|121|115|115|101|99|117|114|101|1|0|0|0|6|107|108|105|109|105|116|0|192|0|0|0|0|0|0|0|0|0|11|104|115|116|115|69|110|97|98|108|101|100|1|0|0|0|10|104|115|116|115|77|97|120|65|103|101|2|0|1|48|0|0|16|104|115|116|115|73|110|99|83|117|98|68|111|109|97|105|110|1|0|0|0|13|104|115|116|115|83|116|97|114|116|84|105|109|101|2|0|1|48|0".Split('|');
			byte[] array2 = new byte[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = byte.Parse(array[i]);
			}
			string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Macromedia\\Flash Player\\macromedia.com\\support\\flashplayer\\sys\\#file.gunhoiuc.com\\";
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			method_4(array2, text + "settings.sol");
		}

		private void method_4(byte[] byte_0, string string_2)
		{
			BinaryWriter binaryWriter = null;
			try
			{
				binaryWriter = new BinaryWriter(File.Create(string_2));
				binaryWriter.Write(byte_0);
				binaryWriter.Flush();
			}
			finally
			{
				binaryWriter.Close();
			}
		}

		private void timer_0_Tick(object sender, EventArgs e)
		{
			int_0++;
			circularProgressBar1.Value = int_0;
			circularProgressBar1.Text = circularProgressBar1.Value.ToString();
			if (int_0 == 1)
			{
				new Thread(method_0).Start();
				if (LoginMgr.ModeFlash != 0 && !method_6())
				{
					System.Windows.Forms.MessageBox.Show("Khởi tạo tài nguyên thất bại , vui lòng sử dụng chế độ khác", "Thông báo");
					return;
				}
			}
			if (int_0 == 50)
			{
				method_1();
				new Thread((ThreadStart)delegate
				{
					method_3();
				}).Start();
				new Thread((ThreadStart)delegate
				{
					method_2();
				}).Start();
			}
			if (int_0 == 100)
			{
				timer_0.Stop();
				timer_1.Start();
			}
			base.Opacity += 0.01;
		}

		private void method_5()
		{
			new WebClient();
			string webForm = ControlMgr.GetWebForm(ApplicationConfig.UrlApi + "/noflash.txt");
			webClient_0 = new WebClient();
			Uri address = new Uri(webForm);
			string currentDirectory = Directory.GetCurrentDirectory();
			string_0 = System.IO.Path.Combine(currentDirectory, smethod_0(webForm));
			webClient_0.DownloadFileCompleted += webClient_0_DownloadFileCompleted;
			webClient_0.DownloadFileAsync(address, string_0);
		}

		private bool method_6()
		{
			string path = System.IO.Path.GetTempPath() + ControlMgr.smethod_0("AssemblyDataGhu") + "\\";
			if (Directory.Exists(path))
			{
				ControlMgr.CreateFileIfNotFound(ApplicationConfig.ResxFileName, path, "exe");
				return true;
			}
			Directory.CreateDirectory(path);
			if (!Directory.Exists(path))
			{
				return false;
			}
			ControlMgr.CreateFileIfNotFound(ApplicationConfig.ResxFileName, path, "exe");
			return true;
		}

		private static string smethod_0(string string_2, string string_3 = "HEAD")
		{
			try
			{
				string text = string.Empty;
				Uri uri = new Uri(string_2);
				if (uri.Scheme.Equals(Uri.UriSchemeHttp) || uri.Scheme.Equals(Uri.UriSchemeHttps))
				{
					HttpWebRequest obj = (HttpWebRequest)WebRequest.Create(uri);
					obj.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
					obj.Method = string_3;
					obj.AllowAutoRedirect = false;
					HttpWebResponse httpWebResponse = (HttpWebResponse)obj.GetResponse();
					if ((httpWebResponse.StatusCode.Equals(HttpStatusCode.Found) || httpWebResponse.StatusCode.Equals(HttpStatusCode.MovedPermanently) || httpWebResponse.StatusCode.Equals(HttpStatusCode.MovedPermanently)) && httpWebResponse.Headers["Location"] != null)
					{
						return smethod_0(httpWebResponse.Headers["Location"]);
					}
					string text2 = httpWebResponse.Headers["content-disposition"];
					if (!string.IsNullOrEmpty(text2))
					{
						int num = text2.IndexOf("filename=", StringComparison.CurrentCultureIgnoreCase);
						if (num >= 0)
						{
							text = text2.Substring(num + "filename=".Length);
						}
						if (text.StartsWith("\"") && text.EndsWith("\""))
						{
							text = text.Substring(1, text.Length - 2);
						}
					}
				}
				if (string.IsNullOrEmpty(text))
				{
					text = System.IO.Path.GetFileName(uri.LocalPath);
				}
				return text;
			}
			catch (WebException)
			{
				return smethod_0(string_2, "GET");
			}
		}

		private void webClient_0_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
		{
			if (e.Cancelled)
			{
				return;
			}
			Process.Start(new ProcessStartInfo
			{
				FileName = string_0,
				UseShellExecute = true
			});
			Process currentProcess = Process.GetCurrentProcess();
			Process[] processesByName = Process.GetProcessesByName(currentProcess.ProcessName);
			foreach (Process process in processesByName)
			{
				if (process.Id != currentProcess.Id)
				{
					process.Kill();
				}
			}
		}

		private void timer_1_Tick(object sender, EventArgs e)
		{
			base.Opacity -= 0.014;
			if (base.Opacity <= 0.01)
			{
				timer_1.Stop();
				Hide();
				try
				{
					OpenPlayGame();
				}
				catch (Exception ex)
				{
					string obj = ex.Message.ToString();
					string text = "-(timer2_Tick StartLauncher)- Now Version : ";
					Version version = Assembly.GetEntryAssembly().GetName().Version;
					ControlMgr.UpLogLauncher(obj + text + ((version != null) ? version.ToString() : null));
				}
			}
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
			thread_0 = new Thread(method_7);
			thread_0.SetApartmentState(ApartmentState.STA);
			thread_0.Start();
		}

		private void method_7()
		{
			try
			{
				Dispose();
				if (LoginMgr.ModeFlash != 0)
				{
					if (Screen.PrimaryScreen.Bounds.Width <= 1200)
					{
						System.Windows.Forms.Application.Run(new PlayGameFrm2PRJ(LoginMgr.ServerID, ""));
					}
					if (Screen.PrimaryScreen.Bounds.Width > 1200)
					{
						System.Windows.Forms.Application.Run(new PlayGameFrmPRJ(LoginMgr.ServerID, ""));
					}
				}
				else
				{
					if (Screen.PrimaryScreen.Bounds.Width <= 1200)
					{
						System.Windows.Forms.Application.Run(new PlayGameFrm2FIB(LoginMgr.ServerID, ""));
					}
					if (Screen.PrimaryScreen.Bounds.Width > 1200)
					{
						System.Windows.Forms.Application.Run(new PlayGameFrmFIB(LoginMgr.ServerID, ""));
					}
				}
			}
			catch (Exception ex)
			{
				string obj = ex.Message.ToString();
				string text = "- (OpenPlayGameFrm)- Now Version : ";
				Version version = Assembly.GetEntryAssembly().GetName().Version;
				ControlMgr.UpLogLauncher(obj + text + ((version != null) ? version.ToString() : null));
				System.Windows.Forms.MessageBox.Show($"Lỗi : {ex.Message} . Hệ thống sẽ tự động khắc phục , nếu không được bạn hãy khởi động lại Launcher !");
				OpenPlayGame();
			}
		}

		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

		[DllImport("user32.dll")]
		public static extern bool ReleaseCapture();

		private void method_8(MouseEventArgs mouseEventArgs_0)
		{
			if (mouseEventArgs_0.Button == MouseButtons.Left)
			{
				ReleaseCapture();
				SendMessage(base.Handle, 161, 2, 0);
			}
		}

		private void pictureBox4_MouseDown(object sender, MouseEventArgs e)
		{
			method_8(e);
		}

		private void pictureBox8_MouseDown(object sender, MouseEventArgs e)
		{
			method_8(e);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.timer_0 = new System.Windows.Forms.Timer(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.bunifuGradientPanel2 = new System.Windows.Forms.Panel();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.bunifuCustomLabel1 = new ns1.BunifuCustomLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.qxaQgoawi = new System.Windows.Forms.PictureBox();
            this.timer_1 = new System.Windows.Forms.Timer(this.components);
            this.bunifuGradientPanel1 = new System.Windows.Forms.Panel();
            this.bunifuCustomLabel2 = new ns1.BunifuCustomLabel();
            this.circularProgressBar1 = new CircularProgressBar.CircularProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.bunifuGradientPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.qxaQgoawi)).BeginInit();
            this.bunifuGradientPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer_0
            // 
            this.timer_0.Interval = 5;
            this.timer_0.Tick += new System.EventHandler(this.timer_0_Tick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(23, 21);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(165, 102);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox3.BackgroundImage")));
            this.pictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox3.Location = new System.Drawing.Point(224, 4);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(553, 96);
            this.pictureBox3.TabIndex = 3;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox2.BackgroundImage")));
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Location = new System.Drawing.Point(262, 96);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(515, 332);
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            // 
            // bunifuGradientPanel2
            // 
            this.bunifuGradientPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(152)))), ((int)(((byte)(105)))));
            this.bunifuGradientPanel2.Controls.Add(this.pictureBox7);
            this.bunifuGradientPanel2.Controls.Add(this.pictureBox4);
            this.bunifuGradientPanel2.Controls.Add(this.bunifuCustomLabel1);
            this.bunifuGradientPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.bunifuGradientPanel2.Location = new System.Drawing.Point(0, 0);
            this.bunifuGradientPanel2.Name = "bunifuGradientPanel2";
            this.bunifuGradientPanel2.Size = new System.Drawing.Size(766, 71);
            this.bunifuGradientPanel2.TabIndex = 7;
            // 
            // pictureBox7
            // 
            this.pictureBox7.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox7.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox7.BackgroundImage")));
            this.pictureBox7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox7.Location = new System.Drawing.Point(638, -1);
            this.pictureBox7.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(140, 70);
            this.pictureBox7.TabIndex = 1;
            this.pictureBox7.TabStop = false;
            this.pictureBox7.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox4_MouseDown);
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox4.BackgroundImage = global::Properties.Resources.logo_remake;
            this.pictureBox4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox4.Location = new System.Drawing.Point(0, -35);
            this.pictureBox4.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(206, 134);
            this.pictureBox4.TabIndex = 0;
            this.pictureBox4.TabStop = false;
            this.pictureBox4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox4_MouseDown);
            // 
            // bunifuCustomLabel1
            // 
            this.bunifuCustomLabel1.AutoSize = true;
            this.bunifuCustomLabel1.BackColor = System.Drawing.Color.Transparent;
            this.bunifuCustomLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 17.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bunifuCustomLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(89)))), ((int)(((byte)(56)))));
            this.bunifuCustomLabel1.Location = new System.Drawing.Point(261, 19);
            this.bunifuCustomLabel1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.bunifuCustomLabel1.Name = "bunifuCustomLabel1";
            this.bunifuCustomLabel1.Size = new System.Drawing.Size(333, 29);
            this.bunifuCustomLabel1.TabIndex = 2;
            this.bunifuCustomLabel1.Text = "Siêu Gunny - Hồi Ức Trở Về";
            this.bunifuCustomLabel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox4_MouseDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Georgia", 25.75F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(88)))), ((int)(((byte)(77)))));
            this.label1.Location = new System.Drawing.Point(-6, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(197, 41);
            this.label1.TabIndex = 6;
            this.label1.Text = "Loading...";
            // 
            // qxaQgoawi
            // 
            this.qxaQgoawi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(219)))), ((int)(((byte)(188)))));
            this.qxaQgoawi.BackgroundImage = global::Properties.Resources.character;
            this.qxaQgoawi.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.qxaQgoawi.Location = new System.Drawing.Point(0, 69);
            this.qxaQgoawi.Margin = new System.Windows.Forms.Padding(2);
            this.qxaQgoawi.Name = "qxaQgoawi";
            this.qxaQgoawi.Size = new System.Drawing.Size(229, 331);
            this.qxaQgoawi.TabIndex = 1;
            this.qxaQgoawi.TabStop = false;
            this.qxaQgoawi.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox4_MouseDown);
            // 
            // timer_1
            // 
            this.timer_1.Interval = 10;
            this.timer_1.Tick += new System.EventHandler(this.timer_1_Tick);
            // 
            // bunifuGradientPanel1
            // 
            this.bunifuGradientPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(219)))), ((int)(((byte)(188)))));
            this.bunifuGradientPanel1.Controls.Add(this.bunifuCustomLabel2);
            this.bunifuGradientPanel1.Controls.Add(this.circularProgressBar1);
            this.bunifuGradientPanel1.Controls.Add(this.bunifuGradientPanel2);
            this.bunifuGradientPanel1.Controls.Add(this.qxaQgoawi);
            this.bunifuGradientPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bunifuGradientPanel1.Location = new System.Drawing.Point(0, 0);
            this.bunifuGradientPanel1.Name = "bunifuGradientPanel1";
            this.bunifuGradientPanel1.Size = new System.Drawing.Size(766, 403);
            this.bunifuGradientPanel1.TabIndex = 8;
            // 
            // bunifuCustomLabel2
            // 
            this.bunifuCustomLabel2.AutoSize = true;
            this.bunifuCustomLabel2.BackColor = System.Drawing.Color.Transparent;
            this.bunifuCustomLabel2.Font = new System.Drawing.Font("Malgun Gothic", 25F, System.Drawing.FontStyle.Bold);
            this.bunifuCustomLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(89)))), ((int)(((byte)(56)))));
            this.bunifuCustomLabel2.Location = new System.Drawing.Point(453, 131);
            this.bunifuCustomLabel2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.bunifuCustomLabel2.Name = "bunifuCustomLabel2";
            this.bunifuCustomLabel2.Size = new System.Drawing.Size(175, 46);
            this.bunifuCustomLabel2.TabIndex = 3;
            this.bunifuCustomLabel2.Text = "Loading...";
            // 
            // circularProgressBar1
            // 
            this.circularProgressBar1.AnimationFunction = WinFormAnimation.KnownAnimationFunctions.QuinticEaseOut;
            this.circularProgressBar1.AnimationSpeed = 2000;
            this.circularProgressBar1.BackColor = System.Drawing.Color.Transparent;
            this.circularProgressBar1.Font = new System.Drawing.Font("Microsoft Tai Le", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.circularProgressBar1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.circularProgressBar1.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.circularProgressBar1.InnerMargin = 2;
            this.circularProgressBar1.InnerWidth = -1;
            this.circularProgressBar1.Location = new System.Drawing.Point(437, 204);
            this.circularProgressBar1.MarqueeAnimationSpeed = 1500;
            this.circularProgressBar1.Name = "circularProgressBar1";
            this.circularProgressBar1.OuterColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(199)))), ((int)(((byte)(169)))));
            this.circularProgressBar1.OuterMargin = -25;
            this.circularProgressBar1.OuterWidth = 26;
            this.circularProgressBar1.ProgressColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(88)))), ((int)(((byte)(77)))));
            this.circularProgressBar1.ProgressWidth = 25;
            this.circularProgressBar1.SecondaryFont = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.circularProgressBar1.Size = new System.Drawing.Size(170, 159);
            this.circularProgressBar1.StartAngle = 270;
            this.circularProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.circularProgressBar1.SubscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(88)))), ((int)(((byte)(77)))));
            this.circularProgressBar1.SubscriptMargin = new System.Windows.Forms.Padding(5, -40, 0, 0);
            this.circularProgressBar1.SubscriptText = "%";
            this.circularProgressBar1.SuperscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.circularProgressBar1.SuperscriptMargin = new System.Windows.Forms.Padding(10, 35, 0, 0);
            this.circularProgressBar1.SuperscriptText = "";
            this.circularProgressBar1.TabIndex = 5;
            this.circularProgressBar1.Text = "0";
            this.circularProgressBar1.TextMargin = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.circularProgressBar1.Value = 45;
            this.circularProgressBar1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox4_MouseDown);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(766, 403);
            this.Controls.Add(this.bunifuGradientPanel1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Launcher SiêuGunny";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.bunifuGradientPanel2.ResumeLayout(false);
            this.bunifuGradientPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.qxaQgoawi)).EndInit();
            this.bunifuGradientPanel1.ResumeLayout(false);
            this.bunifuGradientPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		[CompilerGenerated]
		private void method_9()
		{
			method_3();
		}

		[CompilerGenerated]
		private void method_10()
		{
			method_2();
		}

		[CompilerGenerated]
		private void method_11()
		{
			OpenPlayGame();
		}
	}
}
