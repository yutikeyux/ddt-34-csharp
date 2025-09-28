using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using Bunifu.Framework;
using Bunifu.Framework.Lib;

namespace ns1
{
	[ProvideProperty("BunifuFramework", typeof(Control))]
	[DebuggerStepThrough]
	[DefaultEvent("ProgessChanged")]
	public class BunifuCircleProgressbar : UserControl
	{
		private int int_0 = 5;

		private int int_1 = 8;

		private Color color_0 = Color.SeaGreen;

		private Color color_1 = Color.Gainsboro;

		private int GgauvnXoaQ;

		private int int_2 = 100;

		private int int_3;

		[CompilerGenerated]
		private EventHandler eventHandler_0;

		private bool bool_0;

		private int int_4 = -90;

		private int int_5 = 5;

		private IContainer icontainer_0;

		private Label lblpass;

		private System.Windows.Forms.Timer timer_0;

		private static BunifuCircleProgressbar XZG31wIOV3dl8nmG9vHB;

		public int GetPassentage => int.Parse(lblpass.Text.Replace("%", ""));

		public Color ProgressColor
		{
			get
			{
				return color_0;
			}
			set
			{
				color_0 = value;
				Invalidate();
				method_2(GgauvnXoaQ);
			}
		}

		public bool LabelVisible
		{
			get
			{
				return lblpass.Visible;
			}
			set
			{
				lblpass.Visible = value;
			}
		}

		public Color ProgressBackColor
		{
			get
			{
				return color_1;
			}
			set
			{
				color_1 = value;
				Invalidate();
				method_2(GgauvnXoaQ);
			}
		}

		public int Value
		{
			get
			{
				return GgauvnXoaQ;
			}
			set
			{
				if (value <= int_2)
				{
					GgauvnXoaQ = value;
					lblpass.Text = (int)((double)GgauvnXoaQ / (double)int_2 * 100.0) + "%";
					method_2(GgauvnXoaQ);
				}
				else
				{
					MessageBox.Show("Maximum Value Exceeded");
				}
			}
		}

		public int LineThickness
		{
			get
			{
				return int_0;
			}
			set
			{
				int_0 = value;
				Invalidate();
				method_2(GgauvnXoaQ);
			}
		}

		public int LineProgressThickness
		{
			get
			{
				return int_1;
			}
			set
			{
				int_1 = value;
				Invalidate();
				method_2(GgauvnXoaQ);
			}
		}

		public int MaxValue
		{
			get
			{
				return int_2;
			}
			set
			{
				int_2 = value;
				Invalidate();
				method_2(GgauvnXoaQ);
			}
		}

		public int animationIterval
		{
			get
			{
				return int_5;
			}
			set
			{
				int_5 = value;
			}
		}

		public bool animated
		{
			get
			{
				return bool_0;
			}
			set
			{
				bool_0 = value;
				if (bool_0)
				{
					timer_0.Start();
					return;
				}
				timer_0.Stop();
				int_4 = -90;
				method_2(GgauvnXoaQ);
			}
		}

		public int animationSpeed
		{
			get
			{
				return timer_0.Interval;
			}
			set
			{
				timer_0.Interval = value;
			}
		}

		public event EventHandler ProgressChanged
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

		public BunifuCircleProgressbar()
		{
			InitializeComponent();
			lblpass.Font = Font;
			lblpass.ForeColor = ForeColor;
			lblpass.Top = base.Height / 2 - lblpass.Height / 2;
			lblpass.Left = base.Width / 2 - lblpass.Width / 2;
			base.Width = base.Height;
			_ = LicenseManager.UsageMode;
			Bunifu.Framework.License.Check(this);
			GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(this, true, null);
		}

		private int method_0(int int_6)
		{
			return 360 * int_6 / int_2;
		}

		private void method_1()
		{
			if (eventHandler_0 != null)
			{
				eventHandler_0(this, null);
			}
		}

		private void method_2(int int_6)
		{
			Bitmap bitmap = new Bitmap(base.Size.Width, base.Size.Height);
			Graphics graphics = Graphics.FromImage(bitmap);
			graphics.SmoothingMode = SmoothingMode.HighQuality;
			graphics.Clear(Color.Transparent);
			Rectangle rect = new Rectangle(10, 10, base.Width - 20, base.Width - 20);
			Pen pen = new Pen(color_1);
			pen.Width = int_0;
			graphics.DrawArc(pen, rect, 0f, 360f);
			Graphics graphics2 = Graphics.FromImage(bitmap);
			graphics2.SmoothingMode = graphics.SmoothingMode;
			graphics2.DrawArc(rect: new Rectangle(10, 10, base.Width - 20, base.Width - 20), pen: new Pen(color_0)
			{
				Width = int_1
			}, startAngle: int_4, sweepAngle: method_0(int_6));
			BackgroundImage = bitmap;
			method_1();
		}

		private void method_3(object sender, PaintEventArgs e)
		{
		}

		private void BunifuCircleProgressbar_Resize(object sender, EventArgs e)
		{
			Invalidate();
			lblpass.Top = base.Height / 2 - lblpass.Height / 2;
			lblpass.Left = base.Width / 2 - lblpass.Width / 2;
			base.Width = base.Height;
			method_2(GgauvnXoaQ);
		}

		private void BunifuCircleProgressbar_FontChanged(object sender, EventArgs e)
		{
			lblpass.Font = Font;
			lblpass.ForeColor = ForeColor;
			lblpass.Top = base.Height / 2 - lblpass.Height / 2;
			lblpass.Left = base.Width / 2 - lblpass.Width / 2;
		}

		private void BunifuCircleProgressbar_ForeColorChanged(object sender, EventArgs e)
		{
			lblpass.Font = Font;
			lblpass.ForeColor = ForeColor;
			lblpass.Top = base.Height / 2 - lblpass.Height / 2;
			lblpass.Left = base.Width / 2 - lblpass.Width / 2;
		}

		private void method_4(object sender, EventArgs e)
		{
			Elipse.Apply(lblpass, lblpass.Height);
			lblpass.Top = base.Height / 2 - lblpass.Height / 2;
			lblpass.Left = base.Width / 2 - lblpass.Width / 2;
		}

		private void BunifuCircleProgressbar_Load(object sender, EventArgs e)
		{
			if (base.DesignMode)
			{
				Bunifu.Framework.License.Check(this);
			}
		}

		private void timer_0_Tick(object sender, EventArgs e)
		{
			if (Value != MaxValue && Value > 0)
			{
				int_4 += animationIterval;
				method_2(GgauvnXoaQ);
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
			icontainer_0 = new System.ComponentModel.Container();
			lblpass = new System.Windows.Forms.Label();
			timer_0 = new System.Windows.Forms.Timer(icontainer_0);
			SuspendLayout();
			lblpass.AutoSize = true;
			lblpass.BackColor = System.Drawing.Color.Transparent;
			lblpass.Location = new System.Drawing.Point(76, 72);
			lblpass.Name = "lblpass";
			lblpass.Size = new System.Drawing.Size(67, 39);
			lblpass.TabIndex = 0;
			lblpass.Text = "0%";
			timer_0.Interval = 300;
			timer_0.Tick += new System.EventHandler(timer_0_Tick);
			base.AutoScaleDimensions = new System.Drawing.SizeF(20f, 39f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackColor = System.Drawing.Color.White;
			base.Controls.Add(lblpass);
			Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25f);
			ForeColor = System.Drawing.Color.SeaGreen;
			base.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
			base.Name = "BunifuCircleProgressbar";
			base.Size = new System.Drawing.Size(205, 201);
			base.Load += new System.EventHandler(BunifuCircleProgressbar_Load);
			base.FontChanged += new System.EventHandler(BunifuCircleProgressbar_FontChanged);
			base.ForeColorChanged += new System.EventHandler(BunifuCircleProgressbar_ForeColorChanged);
			base.Resize += new System.EventHandler(BunifuCircleProgressbar_Resize);
			ResumeLayout(false);
			PerformLayout();
		}

		internal static void gqUh1kIOFvoHUvxXqVC6()
		{
		}

		internal static bool mWpNoyIOn2lR4GN2f0DJ()
		{
			return XZG31wIOV3dl8nmG9vHB == null;
		}
	}
}
