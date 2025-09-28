using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace BunifuAnimatorNS
{
	[DebuggerStepThrough]
	public class DoubleBitmapForm : Form, IFakeControl
	{
		private Bitmap bitmap_0;

		private Bitmap bitmap_1;

		[CompilerGenerated]
		private EventHandler<TransfromNeededEventArg> eventHandler_0;

		private Padding padding_0;

		private Control control_0;

		private Point point_0;

		[CompilerGenerated]
		private EventHandler<PaintEventArgs> eventHandler_1;

		[CompilerGenerated]
		private EventHandler<PaintEventArgs> eventHandler_2;

		private IContainer icontainer_0;

		private static DoubleBitmapForm KPNoN9IsZyCL1lb1dkPt;

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams obj = base.CreateParams;
				obj.Style = int.MinValue;
				obj.ExStyle |= 134217856;
				obj.X = base.Location.X;
				obj.Y = base.Location.Y;
				return obj;
			}
		}

		public Bitmap BgBmp
		{
			get
			{
				return bitmap_0;
			}
			set
			{
				bitmap_0 = value;
			}
		}

		public Bitmap Frame
		{
			get
			{
				return bitmap_1;
			}
			set
			{
				bitmap_1 = value;
			}
		}

		public event EventHandler<TransfromNeededEventArg> TransfromNeeded
		{
			[CompilerGenerated]
			add
			{
				EventHandler<TransfromNeededEventArg> eventHandler = eventHandler_0;
				EventHandler<TransfromNeededEventArg> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler<TransfromNeededEventArg> value2 = (EventHandler<TransfromNeededEventArg>)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref eventHandler_0, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler<TransfromNeededEventArg> eventHandler = eventHandler_0;
				EventHandler<TransfromNeededEventArg> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler<TransfromNeededEventArg> value2 = (EventHandler<TransfromNeededEventArg>)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref eventHandler_0, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
		}

		public event EventHandler<PaintEventArgs> FramePainting
		{
			[CompilerGenerated]
			add
			{
				EventHandler<PaintEventArgs> eventHandler = eventHandler_1;
				EventHandler<PaintEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler<PaintEventArgs> value2 = (EventHandler<PaintEventArgs>)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref eventHandler_1, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler<PaintEventArgs> eventHandler = eventHandler_1;
				EventHandler<PaintEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler<PaintEventArgs> value2 = (EventHandler<PaintEventArgs>)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref eventHandler_1, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
		}

		public event EventHandler<PaintEventArgs> FramePainted
		{
			[CompilerGenerated]
			add
			{
				EventHandler<PaintEventArgs> eventHandler = eventHandler_2;
				EventHandler<PaintEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler<PaintEventArgs> value2 = (EventHandler<PaintEventArgs>)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref eventHandler_2, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler<PaintEventArgs> eventHandler = eventHandler_2;
				EventHandler<PaintEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler<PaintEventArgs> value2 = (EventHandler<PaintEventArgs>)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref eventHandler_2, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
		}

		public DoubleBitmapForm()
		{
			InitializeComponent();
			base.Visible = false;
			SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, value: true);
			base.TopMost = true;
			base.FormBorderStyle = FormBorderStyle.None;
			base.WindowState = FormWindowState.Maximized;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics graphics = e.Graphics;
			OnFramePainting(e);
			try
			{
				graphics.DrawImage(bitmap_0, -base.Location.X, -base.Location.Y);
				if (bitmap_1 != null)
				{
					TransfromNeededEventArg transfromNeededEventArg = new TransfromNeededEventArg();
					Rectangle rectangle3 = (transfromNeededEventArg.ClientRectangle = (transfromNeededEventArg.ClipRectangle = new Rectangle(control_0.Bounds.Left - padding_0.Left, control_0.Bounds.Top - padding_0.Top, control_0.Bounds.Width + padding_0.Horizontal, control_0.Bounds.Height + padding_0.Vertical)));
					method_0(transfromNeededEventArg);
					graphics.SetClip(transfromNeededEventArg.ClipRectangle);
					graphics.Transform = transfromNeededEventArg.Matrix;
					Point location = control_0.Location;
					graphics.DrawImage(bitmap_1, location.X - padding_0.Left, location.Y - padding_0.Top);
				}
				OnFramePainted(e);
			}
			catch
			{
			}
		}

		private void method_0(TransfromNeededEventArg transfromNeededEventArg_0)
		{
			if (eventHandler_0 != null)
			{
				eventHandler_0(this, transfromNeededEventArg_0);
			}
		}

		protected virtual void OnFramePainting(PaintEventArgs e)
		{
			if (eventHandler_1 != null)
			{
				eventHandler_1(this, e);
			}
		}

		protected virtual void OnFramePainted(PaintEventArgs e)
		{
			if (eventHandler_2 != null)
			{
				eventHandler_2(this, e);
			}
		}

		public void InitParent(Control control, Padding padding)
		{
			control_0 = control;
			base.Location = new Point(0, 0);
			base.Size = Screen.PrimaryScreen.Bounds.Size;
			control.VisibleChanged += method_1;
			padding_0 = padding;
		}

		private void method_1(object sender, EventArgs e)
		{
			point_0 = (sender as Control).Location;
			_ = (sender as Control).Size;
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
			SuspendLayout();
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(284, 262);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.Name = "DoubleBitmapForm";
			base.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			Text = "DoubleBitmapForm";
			ResumeLayout(false);
		}

		internal static void m4fc8kIswiiSuCGTJDlY()
		{
		}

		internal static bool Dk5hBcIsdyRSc87bYcJe()
		{
			return KPNoN9IsZyCL1lb1dkPt == null;
		}
	}
}
