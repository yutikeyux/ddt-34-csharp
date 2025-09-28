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
	[DefaultEvent("RangeChanged")]
	[DebuggerStepThrough]
	[ProvideProperty("BunifuFramework", typeof(Control))]
	public class BunifuRange : UserControl
	{
		private int int_0 = 100;

		private int int_1;

		private int int_2;

		[CompilerGenerated]
		private EventHandler eventHandler_0;

		[CompilerGenerated]
		private EventHandler eventHandler_1;

		[CompilerGenerated]
		private EventHandler eventHandler_2;

		private int int_3;

		private IContainer icontainer_0;

		private Panel bg;

		private Panel slider;

		private Panel slider2;

		private Panel FILL;

		private static BunifuRange fRb3FNIGXGec6ybsrSFq;

		public int BorderRadius
		{
			get
			{
				return int_3;
			}
			set
			{
				int_3 = value;
				Elipse.Apply(bg, int_3);
				Elipse.Apply(slider, int_3);
				Elipse.Apply(slider2, int_3);
			}
		}

		public int RangeMax
		{
			get
			{
				return int_2;
			}
			set
			{
				int left = slider2.Left;
				if (value <= int_0)
				{
					int_2 = value;
					slider2.Left = (base.Width - 15) * int_2 / int_0;
					if (slider2.Left < slider.Left)
					{
						slider2.Left = left;
						throw new Exception("Minium Value Reached");
					}
					FILL.Left = slider.Left + slider.Width / 2;
					FILL.Width = slider2.Left + slider2.Width / 2 - FILL.Left;
					return;
				}
				throw new Exception("Maximum Value Reached");
			}
		}

		public int RangeMin
		{
			get
			{
				return int_1;
			}
			set
			{
				int left = slider.Left;
				if (value > int_0)
				{
					throw new Exception("Minium Value Reached");
				}
				int_1 = value;
				slider.Left = (base.Width - 15) * int_1 / int_0;
				if (slider.Left > slider2.Left)
				{
					slider.Left = left;
					throw new Exception("Minium Value Reached");
				}
				FILL.Left = slider.Left + slider.Width / 2;
				FILL.Width = slider2.Left + slider2.Width / 2 - FILL.Left;
			}
		}

		public int MaximumRange
		{
			get
			{
				return int_0;
			}
			set
			{
				int_0 = value;
				RangeMax = int_0 * slider2.Left / (base.Width - 15);
				RangeMin = int_0 * slider.Left / (base.Width - 15);
				FILL.Left = slider.Left + slider.Width / 2;
				FILL.Width = slider2.Left + slider2.Width / 2 - FILL.Left;
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
				Panel panel = slider;
				Panel fILL = FILL;
				Color color2 = (slider2.BackColor = value);
				Color color5 = (panel.BackColor = (fILL.BackColor = color2));
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

		public event EventHandler RangeChanged
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

		public event EventHandler RangeMaxChanged
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

		public event EventHandler RangeMinChanged
		{
			[CompilerGenerated]
			add
			{
				EventHandler eventHandler = eventHandler_2;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref eventHandler_2, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler eventHandler = eventHandler_2;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref eventHandler_2, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
		}

		public BunifuRange()
		{
			InitializeComponent();
			RangeMax = int_0 * slider2.Left / (base.Width - 15);
			RangeMin = int_0 * slider.Left / (base.Width - 15);
			FILL.Left = slider.Left + slider.Width / 2;
			FILL.Width = slider2.Left + slider2.Width / 2 - FILL.Left;
			Bunifu.Framework.License.Check(this);
		}

		private void BunifuRange_Load(object sender, EventArgs e)
		{
			if (base.DesignMode)
			{
				Bunifu.Framework.License.Check(this);
			}
		}

		private void slider_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
			{
				return;
			}
			int num = e.X + slider.Left;
			if (num < slider2.Left && num > 0 && num + slider.Width < base.Width)
			{
				slider.Left = num;
				FILL.Left = slider.Left + slider.Width / 2;
				FILL.Width = slider2.Left + slider2.Width / 2 - FILL.Left;
				RangeMin = int_0 * slider.Left / (base.Width - 15);
				if (eventHandler_0 != null)
				{
					eventHandler_0(this, new EventArgs());
				}
				if (eventHandler_2 != null)
				{
					eventHandler_2(this, new EventArgs());
				}
			}
		}

		private void slider2_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
			{
				return;
			}
			int num = e.X + slider2.Left;
			if (num > slider.Left && num + slider2.Width < base.Width)
			{
				slider2.Left = num;
				FILL.Left = slider.Left + slider.Width / 2;
				FILL.Width = slider2.Left + slider2.Width / 2 - FILL.Left;
				RangeMax = int_0 * slider2.Left / (base.Width - 15);
				if (eventHandler_0 != null)
				{
					eventHandler_0(this, new EventArgs());
				}
				if (eventHandler_1 != null)
				{
					eventHandler_1(this, new EventArgs());
				}
			}
		}

		private void bg_Paint(object sender, PaintEventArgs e)
		{
		}

		private void BunifuRange_Resize(object sender, EventArgs e)
		{
			base.Height = slider.Height + 10;
			bg.Width = base.Width;
			bg.Left = 0;
			Elipse.Apply(bg, int_3);
			Elipse.Apply(slider, int_3);
			Elipse.Apply(slider2, int_3);
		}

		private void bg_MouseDown(object sender, MouseEventArgs e)
		{
		}

		private void mvajtXkib4(object sender, EventArgs e)
		{
			FILL.Height = bg.Height + 1;
			FILL.Top = -1;
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
			FILL = new System.Windows.Forms.Panel();
			slider = new System.Windows.Forms.Panel();
			slider2 = new System.Windows.Forms.Panel();
			bg.SuspendLayout();
			SuspendLayout();
			bg.BackColor = System.Drawing.Color.DarkGray;
			bg.Controls.Add(FILL);
			bg.Location = new System.Drawing.Point(3, 8);
			bg.Name = "bg";
			bg.Size = new System.Drawing.Size(408, 10);
			bg.TabIndex = 0;
			bg.Paint += new System.Windows.Forms.PaintEventHandler(bg_Paint);
			bg.MouseDown += new System.Windows.Forms.MouseEventHandler(bg_MouseDown);
			bg.Resize += new System.EventHandler(mvajtXkib4);
			FILL.BackColor = System.Drawing.Color.SeaGreen;
			FILL.Cursor = System.Windows.Forms.Cursors.Hand;
			FILL.Location = new System.Drawing.Point(18, -5);
			FILL.Name = "FILL";
			FILL.Size = new System.Drawing.Size(183, 20);
			FILL.TabIndex = 3;
			slider.BackColor = System.Drawing.Color.SeaGreen;
			slider.Cursor = System.Windows.Forms.Cursors.Hand;
			slider.Location = new System.Drawing.Point(0, 3);
			slider.Name = "slider";
			slider.Size = new System.Drawing.Size(20, 20);
			slider.TabIndex = 1;
			slider.MouseMove += new System.Windows.Forms.MouseEventHandler(slider_MouseMove);
			slider2.BackColor = System.Drawing.Color.SeaGreen;
			slider2.Cursor = System.Windows.Forms.Cursors.Hand;
			slider2.Location = new System.Drawing.Point(197, 3);
			slider2.Name = "slider2";
			slider2.Size = new System.Drawing.Size(20, 20);
			slider2.TabIndex = 2;
			slider2.MouseMove += new System.Windows.Forms.MouseEventHandler(slider2_MouseMove);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackColor = System.Drawing.Color.Transparent;
			base.Controls.Add(slider2);
			base.Controls.Add(slider);
			base.Controls.Add(bg);
			base.Name = "BunifuRange";
			base.Size = new System.Drawing.Size(415, 28);
			base.Load += new System.EventHandler(BunifuRange_Load);
			base.Resize += new System.EventHandler(BunifuRange_Resize);
			bg.ResumeLayout(false);
			ResumeLayout(false);
		}

		internal static void y6w6pUIGoxRdvBNeTWrf()
		{
		}

		internal static bool f1r4nkIGaZLkFJqDTR9B()
		{
			return fRb3FNIGXGec6ybsrSFq == null;
		}
	}
}
