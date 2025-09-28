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
	public class BunifuVTrackbar : UserControl
	{
		private int int_0 = 100;

		private int int_1;

		[CompilerGenerated]
		private EventHandler eventHandler_0;

		private int int_2;

		private int int_3;

		private IContainer icontainer_0;

		private Panel bg;

		private Panel slider;

		private static BunifuVTrackbar LRKBdIImYjVNOwuu2SF1;

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
					double num = base.Height;
					double num2 = slider.Height;
					_ = slider.Top;
					double d = (double)int_1 * (num - num2) / (double)MaximumValue;
					slider.Top = (int)Math.Truncate(d);
				}
				else
				{
					MessageBox.Show("Cannot exceed maximum Value");
				}
			}
		}

		public int BorderRadius
		{
			get
			{
				return int_2;
			}
			set
			{
				int_2 = value;
				Elipse.Apply(bg, int_2);
			}
		}

		public int SliderRadius
		{
			get
			{
				return int_3;
			}
			set
			{
				int_3 = value;
				Elipse.Apply(slider, int_3);
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
				slider.Top = (base.Height - 15) * int_1 / int_0;
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

		public BunifuVTrackbar()
		{
			InitializeComponent();
			Bunifu.Framework.License.Check(this);
		}

		private void BunifuVTrackbar_Load(object sender, EventArgs e)
		{
			if (base.DesignMode)
			{
				Bunifu.Framework.License.Check(this);
			}
		}

		private void slider_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				int num = e.Y + slider.Top;
				if (num < 0)
				{
					slider.Top = 0;
					int_1 = 0;
				}
				else if (num + slider.Height > base.Height)
				{
					slider.Top = base.Height - slider.Height;
					int_1 = MaximumValue;
				}
				else
				{
					slider.Top = num;
					double num2 = base.Height;
					double num3 = slider.Height;
					double num4 = slider.Top;
					double d = (double)MaximumValue * num4 / (num2 - num3);
					int_1 = (int)Math.Truncate(d);
				}
				if (eventHandler_0 != null)
				{
					eventHandler_0(this, new EventArgs());
				}
			}
		}

		private void bg_Paint(object sender, PaintEventArgs e)
		{
		}

		private void BunifuVTrackbar_Resize(object sender, EventArgs e)
		{
			base.Width = slider.Width + 10;
			bg.Height = base.Height;
			bg.Top = 0;
			Elipse.Apply(bg, int_2);
			Elipse.Apply(slider, int_3);
		}

		private void bg_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
			{
				return;
			}
			int num = e.Y;
			if (num >= 0 && num + slider.Height <= base.Height)
			{
				slider.Top = num;
				double num2 = base.Height;
				double num3 = slider.Height;
				double num4 = slider.Top;
				double d = (double)MaximumValue * num4 / (num2 - num3);
				int_1 = (int)Math.Truncate(d);
				if (eventHandler_0 != null)
				{
					eventHandler_0(this, new EventArgs());
				}
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
			slider = new System.Windows.Forms.Panel();
			SuspendLayout();
			bg.BackColor = System.Drawing.Color.DarkGray;
			bg.Cursor = System.Windows.Forms.Cursors.Hand;
			bg.Location = new System.Drawing.Point(9, 8);
			bg.Name = "bg";
			bg.Size = new System.Drawing.Size(10, 408);
			bg.TabIndex = 0;
			bg.Paint += new System.Windows.Forms.PaintEventHandler(bg_Paint);
			bg.MouseDown += new System.Windows.Forms.MouseEventHandler(bg_MouseDown);
			slider.BackColor = System.Drawing.Color.SeaGreen;
			slider.Cursor = System.Windows.Forms.Cursors.Hand;
			slider.Location = new System.Drawing.Point(4, 0);
			slider.Name = "slider";
			slider.Size = new System.Drawing.Size(20, 20);
			slider.TabIndex = 1;
			slider.MouseMove += new System.Windows.Forms.MouseEventHandler(slider_MouseMove);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackColor = System.Drawing.Color.Transparent;
			base.Controls.Add(slider);
			base.Controls.Add(bg);
			base.Name = "BunifuVTrackbar";
			base.Size = new System.Drawing.Size(28, 415);
			base.Load += new System.EventHandler(BunifuVTrackbar_Load);
			base.Resize += new System.EventHandler(BunifuVTrackbar_Resize);
			ResumeLayout(false);
		}

		internal static void BKN75yImHSqAMLIM5RE6()
		{
		}

		internal static bool QssGy5ImFyZwpI1801Hk()
		{
			return LRKBdIImYjVNOwuu2SF1 == null;
		}
	}
}
