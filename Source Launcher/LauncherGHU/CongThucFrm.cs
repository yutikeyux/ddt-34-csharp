using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using FontAwesome.Sharp;
using LauncherGHU.Properties;
using ns1;

namespace LauncherGHU
{
	public class CongThucFrm : Form
	{
		public const int WM_NCLBUTTONDOWN = 161;

		public const int HT_CAPTION = 2;

		private IContainer icontainer_0 = null;

		private BunifuGradientPanel menuRighttop;

		private IconPictureBox exitBtn;

		private PictureBox pictureBox1;

		private static CongThucFrm wn6NYj7r4MO8spfH7AD;

		public CongThucFrm()
		{
			InitializeComponent();
		}

		private void exitBtn_MouseEnter(object sender, EventArgs e)
		{
			exitBtn.BackColor = Color.FromArgb(255, 211, 155);
		}

		private void exitBtn_MouseLeave(object sender, EventArgs e)
		{
			exitBtn.BackColor = Color.Transparent;
		}

		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

		[DllImport("user32.dll")]
		public static extern bool ReleaseCapture();

		private void method_0(MouseEventArgs mouseEventArgs_0)
		{
			if (mouseEventArgs_0.Button == MouseButtons.Left)
			{
				ReleaseCapture();
				SendMessage(base.Handle, 161, 2, 0);
			}
		}

		private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
		{
			method_0(e);
		}

		private void exitBtn_Click(object sender, EventArgs e)
		{
			Close();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LauncherGHU.CongThucFrm));
			menuRighttop = new ns1.BunifuGradientPanel();
			exitBtn = new FontAwesome.Sharp.IconPictureBox();
			pictureBox1 = new System.Windows.Forms.PictureBox();
			menuRighttop.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)exitBtn).BeginInit();
			((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
			SuspendLayout();
			menuRighttop.BackgroundImage = (System.Drawing.Image)resources.GetObject("menuRighttop.BackgroundImage");
			menuRighttop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			menuRighttop.Controls.Add(exitBtn);
			menuRighttop.Dock = System.Windows.Forms.DockStyle.Top;
			menuRighttop.GradientBottomLeft = System.Drawing.SystemColors.Desktop;
			menuRighttop.GradientBottomRight = System.Drawing.Color.Sienna;
			menuRighttop.GradientTopLeft = System.Drawing.Color.DarkGreen;
			menuRighttop.GradientTopRight = System.Drawing.Color.DarkOrange;
			menuRighttop.Location = new System.Drawing.Point(0, 0);
			menuRighttop.Margin = new System.Windows.Forms.Padding(4);
			menuRighttop.Name = "menuRighttop";
			menuRighttop.Quality = 10;
			menuRighttop.Size = new System.Drawing.Size(379, 19);
			menuRighttop.TabIndex = 9;
			menuRighttop.MouseDown += new System.Windows.Forms.MouseEventHandler(pictureBox1_MouseDown);
			exitBtn.BackColor = System.Drawing.Color.Transparent;
			exitBtn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			exitBtn.Cursor = System.Windows.Forms.Cursors.Hand;
			exitBtn.IconChar = FontAwesome.Sharp.IconChar.Times;
			exitBtn.IconColor = System.Drawing.Color.White;
			exitBtn.IconSize = 15;
			exitBtn.Location = new System.Drawing.Point(359, 1);
			exitBtn.Margin = new System.Windows.Forms.Padding(4);
			exitBtn.Name = "exitBtn";
			exitBtn.Size = new System.Drawing.Size(20, 15);
			exitBtn.TabIndex = 2;
			exitBtn.TabStop = false;
			exitBtn.Click += new System.EventHandler(exitBtn_Click);
			exitBtn.MouseEnter += new System.EventHandler(exitBtn_MouseEnter);
			exitBtn.MouseLeave += new System.EventHandler(exitBtn_MouseLeave);
			pictureBox1.BackgroundImage = LauncherGHU.Properties.Resources.gunny;
			pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			pictureBox1.Location = new System.Drawing.Point(0, 19);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new System.Drawing.Size(379, 134);
			pictureBox1.TabIndex = 10;
			pictureBox1.TabStop = false;
			pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(pictureBox1_MouseDown);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			base.ClientSize = new System.Drawing.Size(379, 153);
			base.Controls.Add(pictureBox1);
			base.Controls.Add(menuRighttop);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.Name = "CongThucFrm";
			Text = "Công Thức";
			menuRighttop.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)exitBtn).EndInit();
			((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
			ResumeLayout(false);
		}

		internal static void CPmd1YbPJotxJLth5JY()
		{
		}

		internal static bool MuX21P7Q3SFvF2stFtM()
		{
			return wn6NYj7r4MO8spfH7AD == null;
		}
	}
}
