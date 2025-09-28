using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Windows.Forms;
using CircularProgressBar;
using WinFormAnimation;

namespace AutoUpdaterDotNET
{
	internal class DownloadUpdateDialog : Form
	{
		private readonly string string_0;

		private string string_1;

		private WebClient webClient_0;

		private IContainer icontainer_0 = null;

		private Label labelInformation;

		private OpenFileDialog openFileDialog_0;

		private Label currentVer;

		private Label newVer;

		private PictureBox pictureBox1;

		private Label label1;

		private global::CircularProgressBar.CircularProgressBar circularProgressBar1;

		internal static DownloadUpdateDialog OGDhCL7YRy46b7DMgFd;

		public DownloadUpdateDialog(string string_2)
		{
			InitializeComponent();
			string_0 = string_2;
		}

		private void DownloadUpdateDialog_Load(object sender, EventArgs e)
		{
			currentVer.Text = "Phiên bản hiện tại : " + AutoUpdater.InstalledVersion;
			newVer.Text = "Phiên bản mới : " + AutoUpdater.CurrentVersion;
			webClient_0 = new WebClient();
			Uri address = new Uri(string_0);
			string currentDirectory = Directory.GetCurrentDirectory();
			string_1 = System.IO.Path.Combine(currentDirectory, smethod_0(string_0));
			webClient_0.DownloadProgressChanged += webClient_0_DownloadProgressChanged;
			webClient_0.DownloadFileCompleted += webClient_0_DownloadFileCompleted;
			webClient_0.DownloadFileAsync(address, string_1);
		}

		private void webClient_0_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			circularProgressBar1.Value = e.ProgressPercentage;
			circularProgressBar1.Text = circularProgressBar1.Value.ToString();
		}

		private void webClient_0_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
		{
			if (e.Cancelled)
			{
				return;
			}
			ProcessStartInfo startInfo = new ProcessStartInfo
			{
				FileName = string_1,
				UseShellExecute = true
			};
			Process.Start(startInfo);
			Process currentProcess = Process.GetCurrentProcess();
			Process[] processesByName = Process.GetProcessesByName(currentProcess.ProcessName);
			foreach (Process process in processesByName)
			{
				if (process.Id != currentProcess.Id)
				{
					process.Kill();
				}
			}
			if (AutoUpdater.IsWinFormsApplication)
			{
				Application.Exit();
			}
			else
			{
				Environment.Exit(0);
			}
		}

		private static string smethod_0(string string_2, string string_3 = "HEAD")
		{
			try
			{
				string text = string.Empty;
				Uri uri = new Uri(string_2);
				if (uri.Scheme.Equals(Uri.UriSchemeHttp) || uri.Scheme.Equals(Uri.UriSchemeHttps))
				{
					HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
					httpWebRequest.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
					httpWebRequest.Method = string_3;
					httpWebRequest.AllowAutoRedirect = false;
					HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
					if ((httpWebResponse.StatusCode.Equals(HttpStatusCode.Found) || httpWebResponse.StatusCode.Equals(HttpStatusCode.MovedPermanently) || httpWebResponse.StatusCode.Equals(HttpStatusCode.MovedPermanently)) && httpWebResponse.Headers["Location"] != null)
					{
						string string_4 = httpWebResponse.Headers["Location"];
						return smethod_0(string_4);
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

		private void DownloadUpdateDialog_FormClosing(object sender, FormClosingEventArgs e)
		{
			webClient_0.CancelAsync();
		}

		private void method_0(object sender, EventArgs e)
		{
		}

		private void currentVer_Click(object sender, EventArgs e)
		{
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoUpdaterDotNET.DownloadUpdateDialog));
			labelInformation = new System.Windows.Forms.Label();
			openFileDialog_0 = new System.Windows.Forms.OpenFileDialog();
			currentVer = new System.Windows.Forms.Label();
			newVer = new System.Windows.Forms.Label();
			pictureBox1 = new System.Windows.Forms.PictureBox();
			label1 = new System.Windows.Forms.Label();
			circularProgressBar1 = new global::CircularProgressBar.CircularProgressBar();
			((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
			SuspendLayout();
			resources.ApplyResources(labelInformation, "labelInformation");
			labelInformation.BackColor = System.Drawing.Color.Transparent;
			labelInformation.ForeColor = System.Drawing.Color.OrangeRed;
			labelInformation.Name = "labelInformation";
			openFileDialog_0.FileName = "update";
			resources.ApplyResources(currentVer, "currentVer");
			currentVer.BackColor = System.Drawing.Color.Transparent;
			currentVer.ForeColor = System.Drawing.Color.MidnightBlue;
			currentVer.Name = "currentVer";
			currentVer.Click += new System.EventHandler(currentVer_Click);
			resources.ApplyResources(newVer, "newVer");
			newVer.BackColor = System.Drawing.Color.Transparent;
			newVer.ForeColor = System.Drawing.Color.MidnightBlue;
			newVer.Name = "newVer";
			pictureBox1.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(pictureBox1, "pictureBox1");
			pictureBox1.Name = "pictureBox1";
			pictureBox1.TabStop = false;
			resources.ApplyResources(label1, "label1");
			label1.BackColor = System.Drawing.Color.Transparent;
			label1.ForeColor = System.Drawing.Color.Maroon;
			label1.Name = "label1";
			circularProgressBar1.AnimationFunction = WinFormAnimation.KnownAnimationFunctions.CubicEaseIn;
			circularProgressBar1.AnimationSpeed = 2000;
			circularProgressBar1.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(circularProgressBar1, "circularProgressBar1");
			circularProgressBar1.ForeColor = System.Drawing.Color.FromArgb(255, 128, 0);
			circularProgressBar1.InnerColor = System.Drawing.Color.Beige;
			circularProgressBar1.InnerMargin = 2;
			circularProgressBar1.InnerWidth = -1;
			circularProgressBar1.MarqueeAnimationSpeed = 1500;
			circularProgressBar1.Name = "circularProgressBar1";
			circularProgressBar1.OuterColor = System.Drawing.Color.DarkSlateBlue;
			circularProgressBar1.OuterMargin = -25;
			circularProgressBar1.OuterWidth = 26;
			circularProgressBar1.ProgressColor = System.Drawing.Color.FromArgb(255, 128, 0);
			circularProgressBar1.ProgressWidth = 25;
			circularProgressBar1.SecondaryFont = new System.Drawing.Font("Microsoft Sans Serif", 36f);
			circularProgressBar1.StartAngle = 270;
			circularProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			circularProgressBar1.SubscriptColor = System.Drawing.Color.FromArgb(166, 166, 166);
			circularProgressBar1.SubscriptMargin = new System.Windows.Forms.Padding(10, -35, 0, 0);
			circularProgressBar1.SubscriptText = "";
			circularProgressBar1.SuperscriptColor = System.Drawing.Color.DarkSlateBlue;
			circularProgressBar1.SuperscriptMargin = new System.Windows.Forms.Padding(10, 35, 0, 0);
			circularProgressBar1.SuperscriptText = "%";
			circularProgressBar1.TextMargin = new System.Windows.Forms.Padding(8, 8, 0, 0);
			circularProgressBar1.Value = 1;
			resources.ApplyResources(this, "$this");
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackColor = System.Drawing.SystemColors.ActiveCaptionText;
			base.Controls.Add(labelInformation);
			base.Controls.Add(circularProgressBar1);
			base.Controls.Add(pictureBox1);
			base.Controls.Add(label1);
			base.Controls.Add(newVer);
			base.Controls.Add(currentVer);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "DownloadUpdateDialog";
			base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(DownloadUpdateDialog_FormClosing);
			base.Load += new System.EventHandler(DownloadUpdateDialog_Load);
			((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		internal static void L5agql7HHDDxn1bhkCt()
		{
		}

		internal static bool MUsssP7FbyDi3wijkW1()
		{
			return OGDhCL7YRy46b7DMgFd == null;
		}
	}
}
