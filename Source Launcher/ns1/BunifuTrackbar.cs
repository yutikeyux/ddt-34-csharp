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
	[ProvideProperty("BunifuFramework", typeof(Control))]
	[DebuggerStepThrough]
	[DefaultEvent("ValueChanged")]
	public class BunifuTrackbar : UserControl
	{
		private int int_0 = 100;

		private int int_1;

		[CompilerGenerated]
		private EventHandler eventHandler_0;

		private int int_2;

		private int int_3;

		private Drag drag_0 = new Drag();

		private IContainer icontainer_0;

		private Panel bg;

		private Panel slider;

		internal static BunifuTrackbar GRSR0DImj9a5BX20ebkT;

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
					slider.Left = (base.Width - 15) * int_1 / int_0;
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
				slider.Left = (base.Width - slider.Width) * int_1 / int_0;
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

		public BunifuTrackbar()
		{
			InitializeComponent();
		}

		private void BunifuTrackbar_Load(object sender, EventArgs e)
		{
			Bunifu.Framework.License.Check(this);
		}

		private void slider_MouseMove(object sender, MouseEventArgs e)
		{
			int left = slider.Left;
			if (left < 0 || left + slider.Width > base.Width)
			{
				return;
			}
			drag_0.MoveObject(Horizontal: true, Vertical: false);
			int num = (int_1 = int_0 * slider.Left / (base.Width - slider.Width));
			if (num >= 0)
			{
				if (num > int_0)
				{
					Value = MaximumValue;
				}
			}
			else
			{
				Value = 0;
			}
			if (eventHandler_0 != null)
			{
				eventHandler_0(this, new EventArgs());
			}
		}

		private void bg_Paint(object sender, PaintEventArgs e)
		{
		}

		private void BunifuTrackbar_Resize(object sender, EventArgs e)
		{
			base.Height = slider.Height + 10;
			bg.Width = base.Width;
			bg.Left = 0;
			Elipse.Apply(bg, int_2);
			Elipse.Apply(slider, int_3);
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
				Value = int_0 * slider.Left / (base.Width - 15);
				if (eventHandler_0 != null)
				{
					eventHandler_0(this, new EventArgs());
				}
			}
		}

		private void slider_MouseUp(object sender, MouseEventArgs e)
		{
			drag_0.Release();
		}

		private void slider_MouseDown(object sender, MouseEventArgs e)
		{
			drag_0.Grab(slider);
		}

		private void bg_MouseMove(object sender, MouseEventArgs e)
		{
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
			bg.Location = new System.Drawing.Point(0, 8);
			bg.Name = "bg";
			bg.Size = new System.Drawing.Size(415, 10);
			bg.TabIndex = 0;
			bg.Paint += new System.Windows.Forms.PaintEventHandler(bg_Paint);
			bg.MouseDown += new System.Windows.Forms.MouseEventHandler(bg_MouseDown);
			bg.MouseMove += new System.Windows.Forms.MouseEventHandler(bg_MouseMove);
			slider.BackColor = System.Drawing.Color.SeaGreen;
			slider.Cursor = System.Windows.Forms.Cursors.Hand;
			slider.Location = new System.Drawing.Point(0, 3);
			slider.Name = "slider";
			slider.Size = new System.Drawing.Size(20, 20);
			slider.TabIndex = 1;
			slider.MouseDown += new System.Windows.Forms.MouseEventHandler(slider_MouseDown);
			slider.MouseMove += new System.Windows.Forms.MouseEventHandler(slider_MouseMove);
			slider.MouseUp += new System.Windows.Forms.MouseEventHandler(slider_MouseUp);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackColor = System.Drawing.Color.Transparent;
			base.Controls.Add(slider);
			base.Controls.Add(bg);
			base.Name = "BunifuTrackbar";
			base.Size = new System.Drawing.Size(415, 28);
			base.Load += new System.EventHandler(BunifuTrackbar_Load);
			base.Resize += new System.EventHandler(BunifuTrackbar_Resize);
			ResumeLayout(false);
		}

		internal static void F7A3QuImzO24SLpIlxaX()
		{
		}

		internal static bool zXbM6AImrGgDrTcdlTOh()
		{
			return GRSR0DImj9a5BX20ebkT == null;
		}
	}
}
