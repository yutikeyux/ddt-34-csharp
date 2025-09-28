using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using Bunifu.Framework;
using Bunifu.Framework.Lib;

namespace ns1
{
	[DefaultEvent("ValueChanged")]
	[DebuggerStepThrough]
	[ProvideProperty("BunifuFramework", typeof(Control))]
	public class BunifuSlider : UserControl
	{
		private int int_0 = 100;

		private int int_1;

		[CompilerGenerated]
		private EventHandler eventHandler_0;

		private int nUvKmYsrab;

		[CompilerGenerated]
		private EventHandler eventHandler_1;

		private IContainer icontainer_0;

		private Panel bg;

		private Panel slider;

		private Panel panel1;

		private static BunifuSlider IcSvqwImLZT4CnEx9bPD;

		public int Value
		{
			get
			{
				return int_1;
			}
			set
			{
				if (value <= int_0)
				{
					int_1 = value;
					double num = base.Width;
					double num2 = slider.Width;
					_ = slider.Left;
					double d = (double)int_1 * (num - num2) / (double)MaximumValue;
					slider.Left = (int)Math.Truncate(d);
					panel1.Width = slider.Left + slider.Width / 2;
				}
				else
				{
					MessageBox.Show("Cannot exceed maximum Value");
				}
			}
		}

		public int MaximumValue
		{
			get
			{
				return int_0;
			}
			set
			{
				int_0 = value;
				double num = base.Width;
				double num2 = slider.Width;
				_ = slider.Left;
				double d = (double)int_1 * (num - num2) / (double)MaximumValue;
				slider.Left = (int)Math.Truncate(d);
				panel1.Width = slider.Left + slider.Width / 2;
			}
		}

		public Color IndicatorColor
		{
			get
			{
				return slider.BackColor;
			}
			set
			{
				slider.BackColor = value;
				panel1.BackColor = slider.BackColor;
			}
		}

		public int BorderRadius
		{
			get
			{
				return nUvKmYsrab;
			}
			set
			{
				nUvKmYsrab = value;
				Elipse.Apply(bg, nUvKmYsrab);
				Elipse.Apply(slider, nUvKmYsrab);
			}
		}

		public Color BackgroudColor
		{
			get
			{
				return bg.BackColor;
			}
			set
			{
				bg.BackColor = value;
			}
		}

		public event EventHandler ValueChanged
		{
			[CompilerGenerated]
			add
			{
				EventHandler eventHandler = eventHandler_0;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref eventHandler_0, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler eventHandler = eventHandler_0;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref eventHandler_0, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
		}

		public event EventHandler ValueChangeComplete
		{
			[CompilerGenerated]
			add
			{
				EventHandler eventHandler = eventHandler_1;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref eventHandler_1, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler eventHandler = eventHandler_1;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref eventHandler_1, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
		}

		public BunifuSlider()
		{
			InitializeComponent();
			panel1.Width = slider.Left + slider.Width / 2;
			Bunifu.Framework.License.Check(this);
		}

		private void slider_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				int num = e.X + slider.Left;
				if (num < 0)
				{
					slider.Left = 0;
					int_1 = 0;
				}
				else if (num + slider.Width > base.Width)
				{
					slider.Left = base.Width - slider.Width;
					int_1 = MaximumValue;
				}
				else
				{
					slider.Left = num;
					double num2 = base.Width;
					double num3 = slider.Width;
					double num4 = slider.Left;
					double d = (double)MaximumValue * num4 / (num2 - num3);
					int_1 = (int)Math.Truncate(d);
				}
				panel1.Width = slider.Left + slider.Width / 2;
				if (eventHandler_0 != null)
				{
					eventHandler_0(this, new EventArgs());
				}
			}
		}

		private void BunifuSlider_Load(object sender, EventArgs e)
		{
			if (base.DesignMode)
			{
				Bunifu.Framework.License.Check(this);
			}
		}

		private void bg_Paint(object sender, PaintEventArgs e)
		{
		}

		private void BunifuSlider_Resize(object sender, EventArgs e)
		{
			base.Height = slider.Height + 10;
			bg.Width = base.Width;
			bg.Left = 0;
			Elipse.Apply(bg, nUvKmYsrab);
			Elipse.Apply(slider, nUvKmYsrab);
		}

		private void bg_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
			{
				return;
			}
			int num = e.X;
			if (num > 0 && num + slider.Width < base.Width)
			{
				slider.Left = num;
				double num2 = base.Width;
				double num3 = slider.Width;
				double num4 = slider.Left;
				double d = (double)MaximumValue * num4 / (num2 - num3);
				int_1 = (int)Math.Truncate(d);
				panel1.Width = slider.Left + slider.Width / 2;
				if (eventHandler_0 != null)
				{
					eventHandler_0(this, new EventArgs());
				}
			}
		}

		private void slider_MouseUp(object sender, MouseEventArgs e)
		{
			if (eventHandler_1 != null)
			{
				eventHandler_1(this, new EventArgs());
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
			bg = new System.Windows.Forms.Panel();
			panel1 = new System.Windows.Forms.Panel();
			slider = new System.Windows.Forms.Panel();
			bg.SuspendLayout();
			SuspendLayout();
			bg.BackColor = System.Drawing.Color.DarkGray;
			bg.Controls.Add(panel1);
			bg.Cursor = System.Windows.Forms.Cursors.Hand;
			bg.Location = new System.Drawing.Point(3, 8);
			bg.Name = "bg";
			bg.Size = new System.Drawing.Size(408, 10);
			bg.TabIndex = 0;
			bg.Paint += new System.Windows.Forms.PaintEventHandler(bg_Paint);
			bg.MouseDown += new System.Windows.Forms.MouseEventHandler(bg_MouseDown);
			panel1.BackColor = System.Drawing.Color.SeaGreen;
			panel1.Cursor = System.Windows.Forms.Cursors.Hand;
			panel1.Dock = System.Windows.Forms.DockStyle.Left;
			panel1.Enabled = false;
			panel1.Location = new System.Drawing.Point(0, 0);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(0, 10);
			panel1.TabIndex = 2;
			slider.BackColor = System.Drawing.Color.SeaGreen;
			slider.Cursor = System.Windows.Forms.Cursors.Hand;
			slider.Location = new System.Drawing.Point(0, 3);
			slider.Name = "slider";
			slider.Size = new System.Drawing.Size(20, 20);
			slider.TabIndex = 1;
			slider.MouseMove += new System.Windows.Forms.MouseEventHandler(slider_MouseMove);
			slider.MouseUp += new System.Windows.Forms.MouseEventHandler(slider_MouseUp);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackColor = System.Drawing.Color.Transparent;
			base.Controls.Add(slider);
			base.Controls.Add(bg);
			base.Name = "BunifuSlider";
			base.Size = new System.Drawing.Size(415, 28);
			base.Load += new System.EventHandler(BunifuSlider_Load);
			base.Resize += new System.EventHandler(BunifuSlider_Resize);
			bg.ResumeLayout(false);
			ResumeLayout(false);
		}

		internal static void avxtrBImXtIBvanLw4Ae()
		{
		}

		internal static bool Oj3Up3ImsDHSlA0ChAcs()
		{
			return IcSvqwImLZT4CnEx9bPD == null;
		}
	}
}
