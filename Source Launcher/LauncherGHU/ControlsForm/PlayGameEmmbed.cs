using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using LauncherGHU.Properties;

namespace LauncherGHU.ControlsForm
{
	public class PlayGameEmmbed : UserControl
	{
		private Form form_0;

		private static Process process_0;

		public static int GWL_STYLE;

		public static int WS_CHILD;

		public static int WS_BORDER;

		public static int WS_DLGFRAME;

		public static int WS_CAPTION;

		private IContainer icontainer_0 = null;

		private Panel bar;

		private Panel panelPlay;

		private static PlayGameEmmbed klrnJeGX5fpY2FLjnq0;

		public PlayGameEmmbed(Form frm)
		{
			InitializeComponent();
			form_0 = frm;
		}

		public void CloseFlash()
		{
			try
			{
				if (process_0 != null)
				{
					process_0.CloseMainWindow();
					process_0.Close();
				}
			}
			catch (Exception ex)
			{
				ControlMgr.UpLogLauncher(ex.ToString() + " Now Version : " + Assembly.GetEntryAssembly().GetName().Version);
			}
		}

		[DllImport("user32")]
		public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

		[DllImport("user32.dll")]
		private static extern bool ShowWindow(IntPtr intptr_0, int int_0);

		public void PlayLink(string url)
		{
			try
			{
				CloseFlash();
				string fileName = Path.GetTempPath() + ControlMgr.smethod_0("AssemblyDataGhu") + $"\\{ApplicationConfig.ResxFileName}.exe";
				string text = "\"";
				process_0 = new Process();
				ProcessStartInfo startInfo = new ProcessStartInfo(fileName, text + url + text);
				process_0.StartInfo = startInfo;
				process_0.EnableRaisingEvents = true;
				process_0.Start();
				while (process_0.MainWindowHandle == IntPtr.Zero)
				{
					Application.DoEvents();
				}
				IntPtr mainWindowHandle = process_0.MainWindowHandle;
				int windowLong = GetWindowLong(mainWindowHandle, GWL_STYLE);
				SetWindowLong(mainWindowHandle, GWL_STYLE, windowLong & ~WS_CAPTION);
				while (process_0.MainWindowHandle == IntPtr.Zero)
				{
					Application.DoEvents();
				}
				SetParent(process_0.MainWindowHandle, panelPlay.Handle);
				Thread.Sleep(1000);
				ShowWindow(process_0.MainWindowHandle, 3);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Lỗi không thể khởi tạo tài nguyên, vui lòng sử dụng chế độ khác !", "Lỗi");
				ControlMgr.UpLogLauncher(ex.Message.ToString() + "- (PlayLink)- Now Version : " + Assembly.GetEntryAssembly().GetName().Version);
			}
		}

		[DllImport("user32.dll")]
		private static extern int SetWindowText(IntPtr intptr_0, string string_0);

		public void SetWindow()
		{
			if (process_0 != null)
			{
				SetParent(process_0.MainWindowHandle, panelPlay.Handle);
				Thread.Sleep(500);
				ShowWindow(process_0.MainWindowHandle, 3);
			}
		}

		[DllImport("user32.dll")]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("user32.dll")]
		public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

		[DllImport("user32.dll")]
		public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

		[DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
		private static extern IntPtr FindWindow_1(IntPtr intptr_0, string string_0);

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
			bar = new System.Windows.Forms.Panel();
			panelPlay = new System.Windows.Forms.Panel();
			SuspendLayout();
			bar.BackgroundImage = LauncherGHU.Properties.Resources.bar;
			bar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			bar.Dock = System.Windows.Forms.DockStyle.Bottom;
			bar.Location = new System.Drawing.Point(0, 600);
			bar.Name = "bar";
			bar.Size = new System.Drawing.Size(1000, 25);
			bar.TabIndex = 0;
			panelPlay.BackColor = System.Drawing.Color.White;
			panelPlay.Dock = System.Windows.Forms.DockStyle.Fill;
			panelPlay.Location = new System.Drawing.Point(0, 0);
			panelPlay.Name = "panelPlay";
			panelPlay.Size = new System.Drawing.Size(1000, 600);
			panelPlay.TabIndex = 1;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(panelPlay);
			base.Controls.Add(bar);
			base.Name = "PlayGameEmmbed";
			base.Size = new System.Drawing.Size(1000, 625);
			ResumeLayout(false);
		}

		static PlayGameEmmbed()
		{
			process_0 = null;
			GWL_STYLE = -16;
			WS_CHILD = 1073741824;
			WS_BORDER = 8388608;
			WS_DLGFRAME = 4194304;
			WS_CAPTION = WS_BORDER | WS_DLGFRAME;
		}

		internal static void fFACO2GMapeN4i17u8E()
		{
		}

		internal static bool AJuj0aGa9NVj7jfcTBW()
		{
			return klrnJeGX5fpY2FLjnq0 == null;
		}
	}
}
