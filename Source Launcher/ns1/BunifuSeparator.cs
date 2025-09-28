using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Bunifu.Framework;

namespace ns1
{
	[ProvideProperty("BunifuFramework", typeof(Control))]
	[DebuggerStepThrough]
	public class BunifuSeparator : UserControl
	{
		private bool bool_0;

		private IContainer icontainer_0;

		private PictureBox pictureBox1;

		internal static BunifuSeparator KMekpnIG8I6T01xt2414;

		public bool Vertical
		{
			get
			{
				return bool_0;
			}
			set
			{
				if (value != bool_0)
				{
					bool_0 = value;
					int num = pictureBox1.Height;
					int num2 = pictureBox1.Width;
					pictureBox1.Height = num2;
					pictureBox1.Width = num;
					OnResize(new EventArgs());
				}
			}
		}

		public Color LineColor
		{
			get
			{
				return pictureBox1.BackColor;
			}
			set
			{
				pictureBox1.BackColor = value;
			}
		}

		public int Transparency
		{
			get
			{
				return pictureBox1.BackColor.A;
			}
			set
			{
				pictureBox1.BackColor = Color.FromArgb(value, pictureBox1.BackColor.R, pictureBox1.BackColor.G, pictureBox1.BackColor.B);
			}
		}

		public int LineThickness
		{
			get
			{
				if (!Vertical)
				{
					return pictureBox1.Height;
				}
				return pictureBox1.Width;
			}
			set
			{
				if (Vertical)
				{
					pictureBox1.Width = value;
				}
				else
				{
					pictureBox1.Height = value;
				}
			}
		}

		public BunifuSeparator()
		{
			InitializeComponent();
			OnResize(new EventArgs());
			_ = LicenseManager.UsageMode;
			Bunifu.Framework.License.Check(this);
		}

		private void BunifuSeparator_BackColorChanged(object sender, EventArgs e)
		{
			if (BackColor != Color.Transparent)
			{
				throw new Exception("Invalid Value");
			}
		}

		private void BunifuSeparator_Resize(object sender, EventArgs e)
		{
			if (Vertical)
			{
				pictureBox1.Top = 0;
				pictureBox1.Height = base.Height;
				pictureBox1.Left = base.Width / 2 - pictureBox1.Width / 2;
			}
			else
			{
				pictureBox1.Left = 0;
				pictureBox1.Width = base.Width;
				pictureBox1.Top = base.Height / 2 - pictureBox1.Height / 2;
			}
		}

		private void BunifuSeparator_Load(object sender, EventArgs e)
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
			pictureBox1 = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
			SuspendLayout();
			pictureBox1.BackColor = System.Drawing.Color.DimGray;
			pictureBox1.Location = new System.Drawing.Point(0, 15);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new System.Drawing.Size(639, 1);
			pictureBox1.TabIndex = 0;
			pictureBox1.TabStop = false;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackColor = System.Drawing.Color.Transparent;
			base.Controls.Add(pictureBox1);
			base.Name = "BunifuSeparator";
			base.Size = new System.Drawing.Size(639, 35);
			base.Load += new System.EventHandler(BunifuSeparator_Load);
			base.BackColorChanged += new System.EventHandler(BunifuSeparator_BackColorChanged);
			base.Resize += new System.EventHandler(BunifuSeparator_Resize);
			((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
			ResumeLayout(false);
		}

		internal static void KPMfncIG9p6x15Npb0hH()
		{
		}

		internal static bool crqQUOIGKGbBkK8BrPOk()
		{
			return KMekpnIG8I6T01xt2414 == null;
		}
	}
}
