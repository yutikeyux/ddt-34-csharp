using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Bunifu.Framework;
using Bunifu.Framework.Lib;

namespace ns1
{
	[DefaultEvent("Click")]
	[DebuggerStepThrough]
	[ProvideProperty("BunifuFramework", typeof(Control))]
	public class BunifuSwitch : UserControl
	{
		private bool bool_0 = true;

		private Color color_0 = Color.SeaGreen;

		private Color color_1 = Color.DarkGray;

		private int int_0;

		private int int_1;

		private int int_2;

		private IContainer icontainer_0;

		private Panel panel1;

		private BunifuCustomLabel customLabel1;

		internal static BunifuSwitch EmpNUZImEoTFCKgGXxmx;

		public int BorderRadius
		{
			get
			{
				return int_2;
			}
			set
			{
				int_2 = value;
				Elipse.Apply(this, int_2);
			}
		}

		public bool Value
		{
			get
			{
				return bool_0;
			}
			set
			{
				if (value)
				{
					panel1.Dock = DockStyle.Left;
					panel1.BackColor = color_0;
					customLabel1.Dock = DockStyle.Right;
					customLabel1.Text = "On";
				}
				else
				{
					panel1.Dock = DockStyle.Right;
					panel1.BackColor = color_1;
					customLabel1.Dock = DockStyle.Left;
					customLabel1.Text = "Off";
				}
				bool_0 = value;
			}
		}

		public Color Textcolor
		{
			get
			{
				return customLabel1.ForeColor;
			}
			set
			{
				customLabel1.ForeColor = value;
			}
		}

		public Color Oncolor
		{
			get
			{
				return color_0;
			}
			set
			{
				if (bool_0)
				{
					panel1.BackColor = value;
				}
				color_0 = value;
			}
		}

		public Color Onoffcolor
		{
			get
			{
				return color_1;
			}
			set
			{
				if (!bool_0)
				{
					panel1.BackColor = value;
				}
				color_1 = value;
			}
		}

		public BunifuSwitch()
		{
			FupdbrqKeF();
			int_0 = base.Width;
			int_1 = base.Height;
			Bunifu.Framework.License.Check(this);
		}

		private void BunifuSwitch_Click(object sender, EventArgs e)
		{
			Value = !Value;
		}

		private void BunifuSwitch_Resize(object sender, EventArgs e)
		{
			Elipse.Apply(this, int_2);
			base.Width = int_0;
			base.Height = int_1;
		}

		private void BunifuSwitch_ForeColorChanged(object sender, EventArgs e)
		{
			customLabel1.ForeColor = ForeColor;
		}

		private void BunifuSwitch_Load(object sender, EventArgs e)
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

		private void FupdbrqKeF()
		{
			panel1 = new Panel();
			customLabel1 = new BunifuCustomLabel();
			SuspendLayout();
			panel1.BackColor = Color.SeaGreen;
			panel1.Cursor = Cursors.Hand;
			panel1.Dock = DockStyle.Left;
			panel1.Enabled = false;
			panel1.Location = new Point(0, 0);
			panel1.Name = "panel1";
			panel1.Size = new Size(37, 19);
			panel1.TabIndex = 0;
			panel1.Click += BunifuSwitch_Click;
			customLabel1.Dock = DockStyle.Right;
			customLabel1.Enabled = false;
			customLabel1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
			customLabel1.ForeColor = Color.FromArgb(224, 224, 224);
			customLabel1.Location = new Point(26, 0);
			customLabel1.Name = "customLabel1";
			customLabel1.Size = new Size(25, 19);
			customLabel1.TabIndex = 1;
			customLabel1.Text = "On";
			customLabel1.TextAlign = ContentAlignment.MiddleCenter;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.FromArgb(64, 64, 64);
			base.Controls.Add(customLabel1);
			base.Controls.Add(panel1);
			Cursor = Cursors.Hand;
			ForeColor = Color.FromArgb(224, 224, 224);
			base.Name = "BunifuSwitch";
			base.Size = new Size(51, 19);
			base.Load += BunifuSwitch_Load;
			base.ForeColorChanged += BunifuSwitch_ForeColorChanged;
			base.Click += BunifuSwitch_Click;
			base.Resize += BunifuSwitch_Resize;
			ResumeLayout(performLayout: false);
		}

		internal static void kjYImPIm2ZOwV5YB9ME7()
		{
		}

		internal static bool OhAb4vImWuCjvQ6N3Suy()
		{
			return EmpNUZImEoTFCKgGXxmx == null;
		}
	}
}
