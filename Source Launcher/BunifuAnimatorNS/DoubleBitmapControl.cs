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
	[ProvideProperty("BunifuFramework", typeof(Control))]
	public class DoubleBitmapControl : Control, IFakeControl
	{
		private Bitmap bitmap_0;

		private Bitmap bitmap_1;

		[CompilerGenerated]
		private EventHandler<TransfromNeededEventArg> eventHandler_0;

		[CompilerGenerated]
		private EventHandler<PaintEventArgs> eventHandler_1;

		[CompilerGenerated]
		private EventHandler<PaintEventArgs> eventHandler_2;

		private IContainer icontainer_0;

		internal static DoubleBitmapControl VUmgiOIsS6ne2qhMVGgC;

		Bitmap IFakeControl.BgBmp
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

		Bitmap IFakeControl.Frame
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

		public event EventHandler<PaintEventArgs> FramePainted
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

		public DoubleBitmapControl()
		{
			method_1();
			base.Visible = false;
			SetStyle(ControlStyles.Selectable, value: false);
			SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, value: true);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics graphics = e.Graphics;
			OnFramePainting(e);
			try
			{
				graphics.DrawImage(bitmap_0, 0, 0);
				if (bitmap_1 != null)
				{
					TransfromNeededEventArg transfromNeededEventArg = new TransfromNeededEventArg
					{
						ClientRectangle = new Rectangle(0, 0, base.Width, base.Height)
					};
					transfromNeededEventArg.ClipRectangle = transfromNeededEventArg.ClientRectangle;
					method_0(transfromNeededEventArg);
					graphics.SetClip(transfromNeededEventArg.ClipRectangle);
					graphics.Transform = transfromNeededEventArg.Matrix;
					graphics.DrawImage(bitmap_1, 0, 0);
				}
			}
			catch
			{
			}
			OnFramePainted(e);
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
			if (eventHandler_2 != null)
			{
				eventHandler_2(this, e);
			}
		}

		protected virtual void OnFramePainted(PaintEventArgs e)
		{
			if (eventHandler_1 != null)
			{
				eventHandler_1(this, e);
			}
		}

		public void InitParent(Control control, Padding padding)
		{
			base.Parent = control.Parent;
			int childIndex = control.Parent.Controls.GetChildIndex(control);
			control.Parent.Controls.SetChildIndex(this, childIndex);
			base.Bounds = new Rectangle(control.Left - padding.Left, control.Top - padding.Top, control.Size.Width + padding.Left + padding.Right, control.Size.Height + padding.Top + padding.Bottom);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && icontainer_0 != null)
			{
				icontainer_0.Dispose();
			}
			base.Dispose(disposing);
		}

		private void method_1()
		{
			icontainer_0 = new Container();
		}

		internal static void LlCLwfIshMyVyRIEKdCY()
		{
		}

		internal static bool r793p4IstL8jWR99xlLP()
		{
			return VUmgiOIsS6ne2qhMVGgC == null;
		}
	}
}
