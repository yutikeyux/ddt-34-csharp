using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using ns0;

namespace BunifuAnimatorNS
{
	[DebuggerStepThrough]
	public class Controller
	{
		protected Bitmap ctrlBmp;

		[CompilerGenerated]
		private float float_0;

		[CompilerGenerated]
		private float float_1;

		[CompilerGenerated]
		private EventHandler<TransfromNeededEventArg> eventHandler_0;

		[CompilerGenerated]
		private EventHandler<NonLinearTransfromNeededEventArg> eventHandler_1;

		[CompilerGenerated]
		private EventHandler<PaintEventArgs> eventHandler_2;

		[CompilerGenerated]
		private EventHandler<PaintEventArgs> eventHandler_3;

		[CompilerGenerated]
		private EventHandler<MouseEventArgs> eventHandler_4;

		[CompilerGenerated]
		private Control control_0;

		[CompilerGenerated]
		private Control control_1;

		private Point[] point_0;

		private byte[] byte_0;

		protected Rectangle CustomClipRect;

		private AnimateMode animateMode_0;

		private Animation animation_0;

		internal static Controller OdsFarIL21B6ywY9lLYb;

		protected Bitmap BgBmp
		{
			get
			{
				return (DoubleBitmap as IFakeControl).BgBmp;
			}
			set
			{
				(DoubleBitmap as IFakeControl).BgBmp = value;
			}
		}

		public Bitmap Frame
		{
			get
			{
				return (DoubleBitmap as IFakeControl).Frame;
			}
			set
			{
				(DoubleBitmap as IFakeControl).Frame = value;
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
			private set
			{
				float_0 = value;
			}
		}

		protected float TimeStep
		{
			[CompilerGenerated]
			get
			{
				return float_1;
			}
			[CompilerGenerated]
			private set
			{
				float_1 = value;
			}
		}

		public Control DoubleBitmap
		{
			[CompilerGenerated]
			get
			{
				return control_0;
			}
			[CompilerGenerated]
			private set
			{
				control_0 = value;
			}
		}

		public Control AnimatedControl
		{
			[CompilerGenerated]
			get
			{
				return control_1;
			}
			[CompilerGenerated]
			set
			{
				control_1 = value;
			}
		}

		public bool IsCompleted
		{
			get
			{
				if (TimeStep >= 0f && !(CurrentTime < animation_0.MaxTime))
				{
					return true;
				}
				if (TimeStep <= 0f)
				{
					return CurrentTime <= animation_0.MinTime;
				}
				return false;
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

		public event EventHandler<NonLinearTransfromNeededEventArg> NonLinearTransfromNeeded
		{
			[CompilerGenerated]
			add
			{
				EventHandler<NonLinearTransfromNeededEventArg> eventHandler = eventHandler_1;
				EventHandler<NonLinearTransfromNeededEventArg> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler<NonLinearTransfromNeededEventArg> value2 = (EventHandler<NonLinearTransfromNeededEventArg>)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref eventHandler_1, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler<NonLinearTransfromNeededEventArg> eventHandler = eventHandler_1;
				EventHandler<NonLinearTransfromNeededEventArg> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler<NonLinearTransfromNeededEventArg> value2 = (EventHandler<NonLinearTransfromNeededEventArg>)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref eventHandler_1, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
		}

		public event EventHandler<PaintEventArgs> FramePainting
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

		public event EventHandler<PaintEventArgs> FramePainted
		{
			[CompilerGenerated]
			add
			{
				EventHandler<PaintEventArgs> eventHandler = eventHandler_3;
				EventHandler<PaintEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler<PaintEventArgs> value2 = (EventHandler<PaintEventArgs>)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref eventHandler_3, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler<PaintEventArgs> eventHandler = eventHandler_3;
				EventHandler<PaintEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler<PaintEventArgs> value2 = (EventHandler<PaintEventArgs>)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref eventHandler_3, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
		}

		public event EventHandler<MouseEventArgs> MouseDown
		{
			[CompilerGenerated]
			add
			{
				EventHandler<MouseEventArgs> eventHandler = eventHandler_4;
				EventHandler<MouseEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler<MouseEventArgs> value2 = (EventHandler<MouseEventArgs>)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref eventHandler_4, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler<MouseEventArgs> eventHandler = eventHandler_4;
				EventHandler<MouseEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler<MouseEventArgs> value2 = (EventHandler<MouseEventArgs>)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref eventHandler_4, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
		}

		public Controller(Control control, AnimateMode mode, Animation animation, float timeStep, Rectangle controlClipRect)
		{
			if (control is Form)
			{
				DoubleBitmap = new DoubleBitmapForm();
			}
			else
			{
				DoubleBitmap = new DoubleBitmapControl();
			}
			(DoubleBitmap as IFakeControl).FramePainting += OnFramePainting;
			(DoubleBitmap as IFakeControl).FramePainted += OnFramePainting;
			(DoubleBitmap as IFakeControl).TransfromNeeded += OnTransfromNeeded;
			DoubleBitmap.MouseDown += OnMouseDown;
			animation_0 = animation;
			AnimatedControl = control;
			animateMode_0 = mode;
			CustomClipRect = controlClipRect;
			if (mode == AnimateMode.Show || mode == AnimateMode.BeginUpdate)
			{
				timeStep = 0f - timeStep;
			}
			TimeStep = timeStep * ((animation.TimeCoeff == 0f) ? 1f : animation.TimeCoeff);
			if (TimeStep == 0f)
			{
				timeStep = 0.01f;
			}
			try
			{
				switch (mode)
				{
				case AnimateMode.Show:
					BgBmp = GetBackground(control);
					(DoubleBitmap as IFakeControl).InitParent(control, animation.Padding);
					DoubleBitmap.Visible = true;
					DoubleBitmap.Refresh();
					control.Visible = true;
					ctrlBmp = GetForeground(control);
					break;
				case AnimateMode.Hide:
					BgBmp = GetBackground(control);
					(DoubleBitmap as IFakeControl).InitParent(control, animation.Padding);
					ctrlBmp = GetForeground(control);
					DoubleBitmap.Visible = true;
					control.Visible = false;
					break;
				case AnimateMode.Update:
				case AnimateMode.BeginUpdate:
					(DoubleBitmap as IFakeControl).InitParent(control, animation.Padding);
					BgBmp = GetBackground(control, includeForeground: true);
					DoubleBitmap.Visible = true;
					break;
				}
			}
			catch
			{
				Dispose();
			}
			CurrentTime = ((timeStep > 0f) ? animation.MinTime : animation.MaxTime);
		}

		public void Dispose()
		{
			if (ctrlBmp != null)
			{
				BgBmp.Dispose();
			}
			if (ctrlBmp != null)
			{
				ctrlBmp.Dispose();
			}
			if (Frame != null)
			{
				Frame.Dispose();
			}
			AnimatedControl = null;
			Hide();
		}

		public void Hide()
		{
			if (DoubleBitmap == null)
			{
				return;
			}
			try
			{
				DoubleBitmap.BeginInvoke((MethodInvoker)delegate
				{
					if (DoubleBitmap.Visible)
					{
						DoubleBitmap.Hide();
					}
					DoubleBitmap.Parent = null;
				});
			}
			catch
			{
			}
		}

		protected virtual Rectangle GetBounds()
		{
			return new Rectangle(AnimatedControl.Left - animation_0.Padding.Left, AnimatedControl.Top - animation_0.Padding.Top, AnimatedControl.Size.Width + animation_0.Padding.Left + animation_0.Padding.Right, AnimatedControl.Size.Height + animation_0.Padding.Top + animation_0.Padding.Bottom);
		}

		protected virtual Rectangle ControlRectToMyRect(Rectangle rect)
		{
			return new Rectangle(animation_0.Padding.Left + rect.Left, animation_0.Padding.Top + rect.Top, rect.Width + animation_0.Padding.Left + animation_0.Padding.Right, rect.Height + animation_0.Padding.Top + animation_0.Padding.Bottom);
		}

		protected virtual void OnMouseDown(object sender, MouseEventArgs e)
		{
			if (eventHandler_4 != null)
			{
				eventHandler_4(this, e);
			}
		}

		protected virtual void OnFramePainting(object sender, PaintEventArgs e)
		{
			Bitmap frame = Frame;
			Frame = null;
			if (animateMode_0 != AnimateMode.BeginUpdate)
			{
				Frame = OnNonLinearTransfromNeeded();
				if (frame != Frame)
				{
					frame?.Dispose();
				}
				float num = CurrentTime + TimeStep;
				if (num > animation_0.MaxTime)
				{
					num = animation_0.MaxTime;
				}
				if (num < animation_0.MinTime)
				{
					num = animation_0.MinTime;
				}
				CurrentTime = num;
				if (eventHandler_2 != null)
				{
					eventHandler_2(this, e);
				}
			}
		}

		protected virtual void OnFramePainted(object sender, PaintEventArgs e)
		{
			if (eventHandler_3 != null)
			{
				eventHandler_3(this, e);
			}
		}

		protected virtual Bitmap GetBackground(Control ctrl, bool includeForeground = false, bool clip = false)
		{
			if (ctrl is Form)
			{
				return method_0(ctrl, includeForeground, clip);
			}
			Rectangle bounds = GetBounds();
			int num = bounds.Width;
			int num2 = bounds.Height;
			if (num == 0)
			{
				num = 1;
			}
			if (num2 == 1)
			{
				num2 = 1;
			}
			Bitmap bitmap = new Bitmap(num, num2);
			PaintEventArgs paintEventArgs = new PaintEventArgs(clipRect: new Rectangle(0, 0, bitmap.Width, bitmap.Height), graphics: Graphics.FromImage(bitmap));
			if (clip)
			{
				if (!(CustomClipRect == default(Rectangle)))
				{
					paintEventArgs.Graphics.SetClip(CustomClipRect);
				}
				else
				{
					paintEventArgs.Graphics.SetClip(new Rectangle(0, 0, num, num2));
				}
			}
			for (int num3 = ctrl.Parent.Controls.Count - 1; num3 >= 0; num3--)
			{
				Control control = ctrl.Parent.Controls[num3];
				if (control == ctrl && !includeForeground)
				{
					break;
				}
				if (control.Visible && !control.IsDisposed && control.Bounds.IntersectsWith(bounds))
				{
					using Bitmap bitmap2 = new Bitmap(control.Width, control.Height);
					control.DrawToBitmap(bitmap2, new Rectangle(0, 0, control.Width, control.Height));
					paintEventArgs.Graphics.DrawImage(bitmap2, control.Left - bounds.Left, control.Top - bounds.Top, control.Width, control.Height);
				}
				if (control == ctrl)
				{
					break;
				}
			}
			paintEventArgs.Graphics.Dispose();
			return bitmap;
		}

		private Bitmap method_0(Control control_2, bool bool_0, bool bool_1)
		{
			Size size = Screen.PrimaryScreen.Bounds.Size;
			Graphics g = DoubleBitmap.CreateGraphics();
			Bitmap bitmap = new Bitmap(size.Width, size.Height, g);
			Graphics.FromImage(bitmap).CopyFromScreen(0, 0, 0, 0, size);
			return bitmap;
		}

		protected virtual Bitmap GetForeground(Control ctrl)
		{
			Bitmap bitmap = null;
			if (!ctrl.IsDisposed)
			{
				if (ctrl.Parent != null)
				{
					bitmap = new Bitmap(DoubleBitmap.Width, DoubleBitmap.Height);
					ctrl.DrawToBitmap(bitmap, new Rectangle(ctrl.Left - DoubleBitmap.Left, ctrl.Top - DoubleBitmap.Top, ctrl.Width, ctrl.Height));
				}
				else
				{
					bitmap = new Bitmap(ctrl.Width + animation_0.Padding.Horizontal, ctrl.Height + animation_0.Padding.Vertical);
					ctrl.DrawToBitmap(bitmap, new Rectangle(animation_0.Padding.Left, animation_0.Padding.Top, ctrl.Width, ctrl.Height));
				}
			}
			return bitmap;
		}

		protected virtual void OnTransfromNeeded(object sender, TransfromNeededEventArg e)
		{
			try
			{
				if (CustomClipRect != default(Rectangle))
				{
					e.ClipRectangle = ControlRectToMyRect(CustomClipRect);
				}
				e.CurrentTime = CurrentTime;
				if (eventHandler_0 != null)
				{
					eventHandler_0(this, e);
				}
				else
				{
					e.UseDefaultMatrix = true;
				}
				if (e.UseDefaultMatrix)
				{
					Class11.smethod_0(e, animation_0);
					Class11.smethod_7(e, animation_0);
					Class11.smethod_1(e, animation_0);
				}
			}
			catch
			{
			}
		}

		protected virtual Bitmap OnNonLinearTransfromNeeded()
		{
			Bitmap bitmap = null;
			if (ctrlBmp != null)
			{
				if (eventHandler_1 == null && !animation_0.IsNonLinearTransformNeeded)
				{
					return ctrlBmp;
				}
				try
				{
					bitmap = (Bitmap)ctrlBmp.Clone();
					Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
					BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
					IntPtr scan = bitmapData.Scan0;
					int num = bitmap.Width * bitmap.Height * 4;
					byte[] array = new byte[num];
					Marshal.Copy(scan, array, 0, num);
					NonLinearTransfromNeededEventArg nonLinearTransfromNeededEventArg = new NonLinearTransfromNeededEventArg
					{
						CurrentTime = CurrentTime,
						ClientRectangle = DoubleBitmap.ClientRectangle,
						Pixels = array,
						Stride = bitmapData.Stride
					};
					if (eventHandler_1 == null)
					{
						nonLinearTransfromNeededEventArg.UseDefaultTransform = true;
					}
					else
					{
						eventHandler_1(this, nonLinearTransfromNeededEventArg);
					}
					if (nonLinearTransfromNeededEventArg.UseDefaultTransform)
					{
						Class11.smethod_2(nonLinearTransfromNeededEventArg, animation_0);
						Class11.smethod_3(nonLinearTransfromNeededEventArg, animation_0, ref point_0, ref byte_0);
						Class11.smethod_5(nonLinearTransfromNeededEventArg, animation_0);
						Class11.smethod_4(nonLinearTransfromNeededEventArg, animation_0);
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
			return null;
		}

		public void EndUpdate()
		{
			Bitmap background = GetBackground(AnimatedControl, includeForeground: true, clip: true);
			if (animation_0.AnimateOnlyDifferences)
			{
				Class11.smethod_6(background, BgBmp);
			}
			ctrlBmp = background;
			animateMode_0 = AnimateMode.Update;
		}

		internal void method_1()
		{
			if (animateMode_0 != AnimateMode.BeginUpdate)
			{
				DoubleBitmap.Invalidate();
			}
		}

		[CompilerGenerated]
		private void method_2()
		{
			if (DoubleBitmap.Visible)
			{
				DoubleBitmap.Hide();
			}
			DoubleBitmap.Parent = null;
		}

		internal static bool gw06a2ILyeKBSP2J0I3x()
		{
			return OdsFarIL21B6ywY9lLYb == null;
		}
	}
}
