using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Bunifu.Framework;

namespace ns1
{
	[ProvideProperty("BunifuFramework", typeof(Control))]
	[DefaultEvent("Click")]
	[DebuggerStepThrough]
	public class BunifuTileButton : UserControl
	{
		private int int_0 = 50;

		private Color color_0 = Color.SeaGreen;

		private Color color_1 = Color.MediumSeaGreen;

		private IContainer icontainer_0;

		private PictureBox img;

		private Label lbl;

		internal static BunifuTileButton QwdCBRImBBoCvnurG8dg;

		public Image Image
		{
			get
			{
				return img.Image;
			}
			set
			{
				img.Image = value;
			}
		}

		public string LabelText
		{
			get
			{
				return lbl.Text;
			}
			set
			{
				lbl.Text = value;
			}
		}

		public int LabelPosition
		{
			get
			{
				return lbl.Height;
			}
			set
			{
				lbl.Height = value;
			}
		}

		public int ImageZoom
		{
			get
			{
				return int_0;
			}
			set
			{
				int_0 = value;
				method_0();
			}
		}

		public int ImagePosition
		{
			get
			{
				return img.Top;
			}
			set
			{
				img.Top = value;
				method_0();
			}
		}

		public Color color
		{
			get
			{
				return color_0;
			}
			set
			{
				color_0 = value;
				BackColor = color_0;
			}
		}

		public Color colorActive
		{
			get
			{
				return color_1;
			}
			set
			{
				color_1 = value;
			}
		}

		public BunifuTileButton()
		{
			InitializeComponent();
			Bunifu.Framework.License.Check(this);
			method_0();
		}

		private void BunifuTileButton_Resize(object sender, EventArgs e)
		{
			method_0();
		}

		protected override void OnClick(EventArgs e)
		{
			BackColor = color_0;
			base.OnClick(e);
		}

		private void lbl_Click(object sender, EventArgs e)
		{
			BackColor = color_0;
			base.OnClick(e);
		}

		private void BunifuTileButton_MouseEnter(object sender, EventArgs e)
		{
			BackColor = color_1;
		}

		private void BunifuTileButton_MouseLeave(object sender, EventArgs e)
		{
			BackColor = color_0;
		}

		private void lbl_MouseLeave(object sender, EventArgs e)
		{
			BackColor = color_0;
		}

		private void lbl_MouseEnter(object sender, EventArgs e)
		{
			BackColor = color_1;
		}

		private void img_SizeChanged(object sender, EventArgs e)
		{
			method_0();
		}

		private void BunifuTileButton_FontChanged(object sender, EventArgs e)
		{
			lbl.Font = Font;
			method_0();
		}

		private void BunifuTileButton_ForeColorChanged(object sender, EventArgs e)
		{
			lbl.ForeColor = ForeColor;
		}

		private void method_0()
		{
			double num = base.Width;
			double num2 = 100.0;
			double value = num / num2 * (double)int_0;
			img.Width = int.Parse(Math.Round(value, 0).ToString());
			img.Height = img.Width;
			img.Left = base.Width / 2 - img.Width / 2;
		}

		private void BunifuTileButton_Load(object sender, EventArgs e)
		{
			if (base.DesignMode)
			{
				Bunifu.Framework.License.Check(this);
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ns1.BunifuTileButton));
			img = new System.Windows.Forms.PictureBox();
			lbl = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)img).BeginInit();
			SuspendLayout();
			img.Cursor = System.Windows.Forms.Cursors.Hand;
			img.Enabled = false;
			img.Image = (System.Drawing.Image)resources.GetObject("img.Image");
			img.Location = new System.Drawing.Point(33, 20);
			img.Margin = new System.Windows.Forms.Padding(6);
			img.Name = "img";
			img.Size = new System.Drawing.Size(64, 56);
			img.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			img.TabIndex = 0;
			img.TabStop = false;
			img.SizeChanged += new System.EventHandler(img_SizeChanged);
			lbl.Cursor = System.Windows.Forms.Cursors.Hand;
			lbl.Dock = System.Windows.Forms.DockStyle.Bottom;
			lbl.Font = new System.Drawing.Font("Century Gothic", 15.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lbl.ForeColor = System.Drawing.Color.White;
			lbl.Location = new System.Drawing.Point(0, 88);
			lbl.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			lbl.Name = "lbl";
			lbl.Size = new System.Drawing.Size(128, 41);
			lbl.TabIndex = 1;
			lbl.Text = "Tile 1";
			lbl.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			lbl.Click += new System.EventHandler(lbl_Click);
			lbl.MouseEnter += new System.EventHandler(lbl_MouseEnter);
			lbl.MouseLeave += new System.EventHandler(lbl_MouseLeave);
			base.AutoScaleDimensions = new System.Drawing.SizeF(12f, 24f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackColor = System.Drawing.Color.SeaGreen;
			base.Controls.Add(lbl);
			base.Controls.Add(img);
			Cursor = System.Windows.Forms.Cursors.Hand;
			Font = new System.Drawing.Font("Century Gothic", 15.75f);
			ForeColor = System.Drawing.Color.White;
			base.Margin = new System.Windows.Forms.Padding(6);
			base.Name = "BunifuTileButton";
			base.Size = new System.Drawing.Size(128, 129);
			base.Load += new System.EventHandler(BunifuTileButton_Load);
			base.FontChanged += new System.EventHandler(BunifuTileButton_FontChanged);
			base.ForeColorChanged += new System.EventHandler(BunifuTileButton_ForeColorChanged);
			base.MouseEnter += new System.EventHandler(BunifuTileButton_MouseEnter);
			base.MouseLeave += new System.EventHandler(BunifuTileButton_MouseLeave);
			base.Resize += new System.EventHandler(BunifuTileButton_Resize);
			((System.ComponentModel.ISupportInitialize)img).EndInit();
			ResumeLayout(false);
		}

		internal static void gt5JAvImU3lmoaDG8ir6()
		{
		}

		internal static bool a3qiLDImOXO84rgBqwTt()
		{
			return QwdCBRImBBoCvnurG8dg == null;
		}
	}
}
