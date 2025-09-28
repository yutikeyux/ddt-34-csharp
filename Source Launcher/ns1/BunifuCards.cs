using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Bunifu.Framework.Lib;

namespace ns1
{
	[DebuggerStepThrough]
	[ProvideProperty("BunifuFramework", typeof(Control))]
	public class BunifuCards : Panel
	{
		private int int_0 = 5;

		private int int_1 = 20;

		private IContainer icontainer_0;

		private PictureBox topLine;

		private PictureBox BottomLine;

		private PictureBox leftLine;

		private PictureBox rightLine;

		internal static BunifuCards kLx5xOIU2rXeb8wTxCbt;

		public Color color
		{
			get
			{
				return topLine.BackColor;
			}
			set
			{
				topLine.BackColor = value;
			}
		}

		public int BorderRadius
		{
			get
			{
				return int_0;
			}
			set
			{
				int_0 = value;
				Elipse.Apply(this, int_0);
			}
		}

		public bool RightSahddow
		{
			get
			{
				return rightLine.Visible;
			}
			set
			{
				rightLine.Visible = value;
			}
		}

		public bool LeftSahddow
		{
			get
			{
				return leftLine.Visible;
			}
			set
			{
				leftLine.Visible = value;
			}
		}

		public bool BottomSahddow
		{
			get
			{
				return BottomLine.Visible;
			}
			set
			{
				BottomLine.Visible = value;
			}
		}

		public int ShadowDepth
		{
			get
			{
				return int_1;
			}
			set
			{
				int_1 = value;
				method_0();
			}
		}

		public BunifuCards()
		{
			InitializeComponent();
			base.Controls.Add(topLine);
			topLine.Dock = DockStyle.Top;
			topLine.Height = 5;
			topLine.BackColor = Color.Tomato;
			base.Controls.Add(BottomLine);
			BottomLine.Dock = DockStyle.Bottom;
			BottomLine.Height = 3;
			base.Controls.Add(leftLine);
			leftLine.Dock = DockStyle.Left;
			leftLine.Width = 2;
			base.Controls.Add(rightLine);
			rightLine.Dock = DockStyle.Right;
			rightLine.Width = 2;
			topLine.BringToFront();
			Elipse.Apply(this, BorderRadius);
			method_0();
		}

		private void BunifuCards_BackColorChanged(object sender, EventArgs e)
		{
			method_0();
		}

		private void method_0()
		{
			int r = BackColor.R;
			r = ((r - int_1 >= 0) ? (r - int_1) : 0);
			int g = BackColor.G;
			g = ((g - int_1 >= 0) ? (g - int_1) : 0);
			int b = BackColor.B;
			b = ((b - int_1 >= 0) ? (b - int_1) : 0);
			BottomLine.BackColor = Color.FromArgb(r, g, b);
		}

		private void BunifuCards_Resize(object sender, EventArgs e)
		{
			Elipse.Apply(this, int_0);
		}

		private void BottomLine_BackColorChanged(object sender, EventArgs e)
		{
			Color color2 = (leftLine.BackColor = (rightLine.BackColor = BottomLine.BackColor));
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
			topLine = new System.Windows.Forms.PictureBox();
			BottomLine = new System.Windows.Forms.PictureBox();
			leftLine = new System.Windows.Forms.PictureBox();
			rightLine = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)topLine).BeginInit();
			((System.ComponentModel.ISupportInitialize)BottomLine).BeginInit();
			((System.ComponentModel.ISupportInitialize)leftLine).BeginInit();
			((System.ComponentModel.ISupportInitialize)rightLine).BeginInit();
			SuspendLayout();
			topLine.Location = new System.Drawing.Point(0, 0);
			topLine.Name = "topLine";
			topLine.Size = new System.Drawing.Size(100, 50);
			topLine.TabIndex = 0;
			topLine.TabStop = false;
			BottomLine.Location = new System.Drawing.Point(0, 0);
			BottomLine.Name = "BottomLine";
			BottomLine.Size = new System.Drawing.Size(100, 50);
			BottomLine.TabIndex = 0;
			BottomLine.TabStop = false;
			BottomLine.BackColorChanged += new System.EventHandler(BottomLine_BackColorChanged);
			leftLine.Location = new System.Drawing.Point(0, 0);
			leftLine.Name = "leftLine";
			leftLine.Size = new System.Drawing.Size(100, 50);
			leftLine.TabIndex = 0;
			leftLine.TabStop = false;
			leftLine.Visible = false;
			rightLine.Location = new System.Drawing.Point(0, 0);
			rightLine.Name = "rightLine";
			rightLine.Size = new System.Drawing.Size(100, 50);
			rightLine.TabIndex = 0;
			rightLine.TabStop = false;
			BackColor = System.Drawing.Color.White;
			base.Name = "BunifuPaperPanel";
			base.Size = new System.Drawing.Size(605, 397);
			base.BackColorChanged += new System.EventHandler(BunifuCards_BackColorChanged);
			base.Resize += new System.EventHandler(BunifuCards_Resize);
			((System.ComponentModel.ISupportInitialize)topLine).EndInit();
			((System.ComponentModel.ISupportInitialize)BottomLine).EndInit();
			((System.ComponentModel.ISupportInitialize)leftLine).EndInit();
			((System.ComponentModel.ISupportInitialize)rightLine).EndInit();
			ResumeLayout(false);
		}

		internal static void Y2tZmRIUAlbCnHjvsvT9()
		{
		}

		internal static bool QD98RBIUytlH4yY59ua0()
		{
			return kLx5xOIU2rXeb8wTxCbt == null;
		}
	}
}
