using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Bunifu.Framework;
using BunifuAnimatorNS;

namespace ns0
{
	[DebuggerStepThrough]
	internal class DecorationControl : UserControl
	{
		[CompilerGenerated]
		private DecorationType decorationType_0;

		[CompilerGenerated]
		private Control control_0;

		[CompilerGenerated]
		private Padding padding_0;

		[CompilerGenerated]
		private Bitmap bitmap_0;

		[CompilerGenerated]
		private byte[] byte_0;

		[CompilerGenerated]
		private int int_0;

		[CompilerGenerated]
		private Bitmap bitmap_1;

		[CompilerGenerated]
		private float float_0;

		private System.Windows.Forms.Timer timer_0;

		private bool bool_0;

		[CompilerGenerated]
		private EventHandler<NonLinearTransfromNeededEventArg> eventHandler_0;

		internal static DecorationControl S59bZYIsXU0QC939S550;

		public DecorationType DecorationType
		{
			[CompilerGenerated]
			get
			{
				return decorationType_0;
			}
			[CompilerGenerated]
			set
			{
				decorationType_0 = value;
			}
		}

		public Control DecoratedControl
		{
			[CompilerGenerated]
			get
			{
				return control_0;
			}
			[CompilerGenerated]
			set
			{
				control_0 = value;
			}
		}

		public new Padding Padding
		{
			[CompilerGenerated]
			get
			{
				return padding_0;
			}
			[CompilerGenerated]
			set
			{
				padding_0 = value;
			}
		}

		public Bitmap CtrlBmp
		{
			[CompilerGenerated]
			get
			{
				return bitmap_0;
			}
			[CompilerGenerated]
			set
			{
				bitmap_0 = value;
			}
		}

		public byte[] CtrlPixels
		{
			[CompilerGenerated]
			get
			{
				return byte_0;
			}
			[CompilerGenerated]
			set
			{
				byte_0 = value;
			}
		}

		public int CtrlStride
		{
			[CompilerGenerated]
			get
			{
				return int_0;
			}
			[CompilerGenerated]
			set
			{
				int_0 = value;
			}
		}

		public Bitmap Frame
		{
			[CompilerGenerated]
			get
			{
				return bitmap_1;
			}
			[CompilerGenerated]
			set
			{
				bitmap_1 = value;
			}
		}

		public float CurrentTime
		{
			[CompilerGenerated]
			get
			{
				return float_0;
			}
			[CompilerGenerated]
			set
			{
				float_0 = value;
			}
		}

		public event EventHandler<NonLinearTransfromNeededEventArg> Event_0
		{
			[CompilerGenerated]
			add
			{
				EventHandler<NonLinearTransfromNeededEventArg> eventHandler = eventHandler_0;
				EventHandler<NonLinearTransfromNeededEventArg> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler<NonLinearTransfromNeededEventArg> value2 = (EventHandler<NonLinearTransfromNeededEventArg>)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref eventHandler_0, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler<NonLinearTransfromNeededEventArg> eventHandler = eventHandler_0;
				EventHandler<NonLinearTransfromNeededEventArg> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler<NonLinearTransfromNeededEventArg> value2 = (EventHandler<NonLinearTransfromNeededEventArg>)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref eventHandler_0, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
		}

		public DecorationControl(DecorationType decorationType_1, Control control_1)
		{
			DecorationType = decorationType_1;
			DecoratedControl = control_1;
			control_1.VisibleChanged += method_2;
			control_1.ParentChanged += method_2;
			control_1.LocationChanged += method_2;
			control_1.Paint += method_1;
			SetStyle(ControlStyles.Selectable, value: false);
			SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, value: true);
			method_0();
			timer_0 = new System.Windows.Forms.Timer();
			timer_0.Interval = 100;
			timer_0.Tick += timer_0_Tick;
			timer_0.Enabled = true;
		}

		private void method_0()
		{
			DecorationType decorationType = DecorationType;
			if (decorationType == DecorationType.BottomMirror)
			{
				Padding = new Padding(0, 0, 0, 20);
			}
		}

		private void timer_0_Tick(object sender, EventArgs e)
		{
			DecorationType decorationType = DecorationType;
			if (decorationType == DecorationType.BottomMirror || decorationType == DecorationType.Custom)
			{
				Invalidate();
			}
		}

		private void method_1(object sender, PaintEventArgs e)
		{
			if (!bool_0)
			{
				Invalidate();
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			CtrlBmp = vmethod_0(DecoratedControl);
			CtrlPixels = method_4(CtrlBmp);
			if (Frame != null)
			{
				Frame.Dispose();
			}
			Frame = vmethod_1();
			if (Frame != null)
			{
				e.Graphics.DrawImage(Frame, Point.Empty);
			}
		}

		private void method_2(object sender, EventArgs e)
		{
			method_3();
		}

		private void method_3()
		{
			base.Parent = DecoratedControl.Parent;
			base.Visible = DecoratedControl.Visible;
			base.Location = new Point(DecoratedControl.Left - Padding.Left, DecoratedControl.Top - Padding.Top);
			if (base.Parent != null)
			{
				int childIndex = base.Parent.Controls.GetChildIndex(DecoratedControl);
				base.Parent.Controls.SetChildIndex(this, childIndex + 1);
			}
			Size size = new Size(DecoratedControl.Width + Padding.Left + Padding.Right, DecoratedControl.Height + Padding.Top + Padding.Bottom);
			if (size != base.Size)
			{
				base.Size = size;
			}
		}

		protected virtual Bitmap vmethod_0(Control control_1)
		{
			Bitmap bitmap = new Bitmap(base.Width, base.Height);
			if (!control_1.IsDisposed)
			{
				bool_0 = true;
				control_1.DrawToBitmap(bitmap, new Rectangle(Padding.Left, Padding.Top, control_1.Width, control_1.Height));
				bool_0 = false;
			}
			return bitmap;
		}

		private byte[] method_4(Bitmap bitmap_2)
		{
			Rectangle rect = new Rectangle(0, 0, bitmap_2.Width, bitmap_2.Height);
			BitmapData bitmapData = bitmap_2.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			IntPtr scan = bitmapData.Scan0;
			int num = bitmap_2.Width * bitmap_2.Height * 4;
			byte[] array = new byte[num];
			Marshal.Copy(scan, array, 0, num);
			bitmap_2.UnlockBits(bitmapData);
			return array;
		}

		protected virtual Bitmap vmethod_1()
		{
			Bitmap bitmap = null;
			if (CtrlBmp == null)
			{
				return null;
			}
			try
			{
				bitmap = new Bitmap(base.Width, base.Height);
				Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
				BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
				IntPtr scan = bitmapData.Scan0;
				int num = bitmap.Width * bitmap.Height * 4;
				byte[] array = new byte[num];
				Marshal.Copy(scan, array, 0, num);
				NonLinearTransfromNeededEventArg nonLinearTransfromNeededEventArg = new NonLinearTransfromNeededEventArg
				{
					CurrentTime = CurrentTime,
					ClientRectangle = base.ClientRectangle,
					Pixels = array,
					Stride = bitmapData.Stride,
					SourcePixels = CtrlPixels,
					SourceClientRectangle = new Rectangle(Padding.Left, Padding.Top, DecoratedControl.Width, DecoratedControl.Height),
					SourceStride = CtrlStride
				};
				try
				{
					if (eventHandler_0 != null)
					{
						eventHandler_0(this, nonLinearTransfromNeededEventArg);
					}
					else
					{
						nonLinearTransfromNeededEventArg.UseDefaultTransform = true;
					}
					if (nonLinearTransfromNeededEventArg.UseDefaultTransform)
					{
						DecorationType decorationType = DecorationType;
						if (decorationType == DecorationType.BottomMirror)
						{
							Class11.smethod_8(nonLinearTransfromNeededEventArg);
						}
					}
				}
				catch
				{
				}
				Marshal.Copy(array, 0, scan, num);
				bitmap.UnlockBits(bitmapData);
				return bitmap;
			}
			catch
			{
				return bitmap;
			}
		}

		protected override void Dispose(bool disposing)
		{
			timer_0.Stop();
			timer_0.Dispose();
			base.Dispose(disposing);
		}

		private void method_5()
		{
			SuspendLayout();
			base.Name = "DecorationControl";
			base.Load += DecorationControl_Load;
			ResumeLayout(performLayout: false);
		}

		private void DecorationControl_Load(object sender, EventArgs e)
		{
			if (base.DesignMode)
			{
				License.Check(this);
			}
		}

		internal static void m4VAygIsM2Zm0e4QaUpT()
		{
		}

		internal static bool MvUMPaIsa6r1wucYmReq()
		{
			return S59bZYIsXU0QC939S550 == null;
		}
	}
}
