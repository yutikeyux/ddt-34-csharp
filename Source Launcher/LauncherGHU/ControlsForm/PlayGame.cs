using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using AutoUpdaterDotNET;
using hoiuclib;
using LauncherGHU.Properties;

namespace LauncherGHU.ControlsForm
{
	public class PlayGame : UserControl
	{
		private cedruscontrol playgame_control;

		private Form form_0;

		private IContainer icontainer_0 = null;

		private Panel bar;

		private Panel panelPlay;

		private AxCode code;

		internal static PlayGame geShphGF1YEStuvf8wF;

		public PlayGame(Form frm)
		{
			InitializeComponent();
			form_0 = frm;
			try
			{
				Stream manifestResourceStream = GetType().Assembly.GetManifestResourceStream("LauncherGHU.cedrus.cedrus.ghu");
				if (manifestResourceStream == null)
				{
					MessageBox.Show("Resource not found");
				}

				code = new AxCode(manifestResourceStream);
				method_0();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message.ToString(), "Lỗi");
				ControlMgr.UpLogLauncher(ex.ToString() + " Now Version : " + Assembly.GetEntryAssembly().GetName().Version);
			}
		}

		public void SetScale(SizeF scale)
		{
			playgame_control.Scale(scale);
			playgame_control.Select();
			playgame_control.Focus();
		}

		public void SetZoom(int zoom)
		{
			playgame_control.FlashMethod_Zoom(zoom);
			playgame_control.Select();
			playgame_control.Focus();
		}

		public void SetScaleMode(int scale)
		{
			playgame_control.FlashProperty_ScaleMode = scale;
			playgame_control.Select();
			playgame_control.Focus();
		}

		public void PlayURLMovie()
		{
			try
			{
				if (true)// playgame_control.FlashProperty_ReadyState == 4)
				{
					playgame_control.Dispose();
					if (playgame_control.IsDisposed)
					{
						method_1();
					}
				}

				string[] flashConfigs = ControlMgr.getFlashConfigs();
				playgame_control.FlashProperty_AllowFullscreen = true;
				playgame_control.FlashProperty_ScaleMode = 2;
				playgame_control.FlashMethod_Zoom(100);
				playgame_control.FlashProperty_FlashVars = "enterCode=" + ApplicationConfig.KeyCodeLauncher + "&" + flashConfigs[1];
				//playgame_control.FlashProperty_Movie = flashConfigs[0];
				playgame_control.FlashProperty_Movie = ApplicationConfig.LoadingSwf;
				playgame_control.FlashProperty_Quality2 = "High";
				playgame_control.Select();
				playgame_control.Focus();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message.ToString(), "Lỗi");
				if (playgame_control.IsDisposed || playgame_control == null)
				{
					method_0();
				}
				ControlMgr.UpLogLauncher(ex.ToString() + " Now Version : " + Assembly.GetEntryAssembly().GetName().Version);
			}
		}

		public void SetQuality(string quality)
		{
			playgame_control.FlashProperty_Quality2 = quality;
			playgame_control.Select();
			playgame_control.Focus();
		}

		public void CallFlash(string type)
		{
			if (type == "CallOpenTopEventWeek")
			{
				string text = playgame_control.FlashMethod_CallFunction("<invoke name=\"" + type + "\" returntype=\"xml\"><arguments></arguments></invoke>");
				if (text == "")
				{
					MessageBox.Show("Vui lòng vào game để mở Bảng Xếp Hạng Sự Kiện Tuần !", "Thông báo");
				}
			}
		}

		private void method_0()
		{
			try
			{
				method_1();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Khởi tạo tài nguyên không thành công , vui lòng bật lại ứng dụng");
				ControlMgr.UpLogLauncher(ex.ToString() + "- (SetupPlayGameControl)- Now Version : " + Assembly.GetEntryAssembly().GetName().Version);
				Application.Restart();
			}
		}

		private void method_1()
		{			
			playgame_control = new cedruscontrol(code);
			playgame_control.BackColor = SystemColors.AppWorkspace;
			playgame_control.Context = null;
			playgame_control.Dock = DockStyle.Fill;
			playgame_control.FlashProperty_AlignMode = 0;
			playgame_control.FlashProperty_AllowFullscreen = false;
			playgame_control.FlashProperty_AllowScriptAccess = "";
			playgame_control.FlashProperty_BackgroundColor = -1;
			playgame_control.FlashProperty_Base = "";
			playgame_control.FlashProperty_BGColor = "";
			playgame_control.FlashProperty_DeviceFont = false;
			playgame_control.FlashProperty_EmbedMovie = false;
			playgame_control.FlashProperty_FlashVars = "";
			playgame_control.FlashProperty_FrameNum = -1;
			playgame_control.FlashProperty_Loop = true;
			playgame_control.FlashProperty_Menu = false;
			playgame_control.FlashProperty_Movie = "";
			playgame_control.FlashProperty_MovieData = "";
			playgame_control.FlashProperty_Playing = true;
			playgame_control.FlashProperty_Quality = 1;
			playgame_control.FlashProperty_Quality2 = "High";
			playgame_control.FlashProperty_SAlign = "";
			playgame_control.FlashProperty_Scale = "ShowAll";
			playgame_control.FlashProperty_ScaleMode = 0;
			playgame_control.FlashProperty_Stacking = "";
			playgame_control.FlashProperty_SWRemote = "";
			playgame_control.FlashProperty_WMode = "Direct";
			playgame_control.ForeColor = SystemColors.ActiveCaption;
			playgame_control.Location = new Point(0, 0);
			playgame_control.Name = "playgame_control";
			playgame_control.Size = new Size(1000, 600);
			playgame_control.StandardMenu = false;
			playgame_control.TabIndex = 0;
			playgame_control.Text = "cedruscontrol1";
			playgame_control.TransparentMode = false;
			playgame_control.UseFlashCursor = true;
			playgame_control.OnIsInputKey += method_2;
			playgame_control.OnFlashCall += playgame_control_OnFlashCall;
			panelPlay.Controls.Add(playgame_control);
			base.ActiveControl = playgame_control;
			playgame_control.Select();
			playgame_control.Focus();
		}

		private void playgame_control_OnFlashCall(object object_0, string string_0)
		{
			if (string_0.IndexOf("Gunhoiucnapthe") > -1)
			{
				if (form_0 is PlayGameFrmFIB)
				{
					(form_0 as PlayGameFrmFIB).naptheBtn.PerformClick();
				}
				if (form_0 is PlayGameFrm2FIB)
				{
					(form_0 as PlayGameFrm2FIB).naptheItem.PerformClick();
				}
			}
			if (string_0.IndexOf("launcher") > -1)
			{
				MessageBox.Show("Bạn đang sử dụng Launcher \nNếu có phiên bản mới, hệ thống sẽ tự động cập nhật !", "Thông báo");
				AutoUpdater.Start(ApplicationConfig.UrlApi + "version.xml");
			}
			if (string_0.IndexOf("DisableSktuanBtn") > -1)
			{
				if (form_0 is PlayGameFrmFIB)
				{
					(form_0 as PlayGameFrmFIB).sukientuanBtn.Enabled = false;
				}
				if (form_0 is PlayGameFrm2FIB)
				{
					(form_0 as PlayGameFrm2FIB).sukienTuanItem.Enabled = false;
				}
			}
			if (string_0.IndexOf("EnableSktuanBtn") > -1)
			{
				if (form_0 is PlayGameFrmFIB)
				{
					(form_0 as PlayGameFrmFIB).sukientuanBtn.Enabled = true;
				}
				if (form_0 is PlayGameFrm2FIB)
				{
					(form_0 as PlayGameFrm2FIB).sukienTuanItem.Enabled = true;
				}
			}
		}

		private void method_2(object object_0, Keys keys_0, ref bool bool_0)
		{
			if (keys_0 == Keys.Tab || keys_0 == Keys.Return || keys_0 == Keys.Up || keys_0 == Keys.Down || keys_0 == Keys.Left || keys_0 == Keys.Right)
			{
				bool_0 = true;
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
			panelPlay.BackColor = System.Drawing.SystemColors.ActiveCaption;
			panelPlay.Dock = System.Windows.Forms.DockStyle.Fill;
			panelPlay.Location = new System.Drawing.Point(0, 0);
			panelPlay.Name = "panelPlay";
			panelPlay.Size = new System.Drawing.Size(1000, 600);
			panelPlay.TabIndex = 1;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(panelPlay);
			base.Controls.Add(bar);
			base.Name = "PlayGame";
			base.Size = new System.Drawing.Size(1000, 625);
			ResumeLayout(false);
		}

		internal static void ElC33bGpIH7GfQr5Wkl()
		{
		}

		internal static bool hRbi9WG4Jof5UbTgOdE()
		{
			return geShphGF1YEStuvf8wF == null;
		}
	}
}
