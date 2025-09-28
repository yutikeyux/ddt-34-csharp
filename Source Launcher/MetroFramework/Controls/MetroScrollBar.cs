using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Security;
using System.Windows.Forms;
using MetroFramework.Components;
using MetroFramework.Drawing;
using MetroFramework.Interfaces;
using MetroFramework.Native;

namespace MetroFramework.Controls
{
	[Designer("MetroFramework.Design.Controls.MetroScrollBarDesigner, MetroFramework.Design, Version=1.4.0.0, Culture=neutral, PublicKeyToken=5f91a84759bf584a")]
	[DefaultProperty("Value")]
	[DefaultEvent("Scroll")]
	public class MetroScrollBar : Control, IMetroControl
	{
		public delegate void ScrollValueChangedDelegate(object sender, int newValue);

		private EventHandler<MetroPaintEventArgs> eventHandler_0;

		private EventHandler<MetroPaintEventArgs> eventHandler_1;

		private EventHandler<MetroPaintEventArgs> eventHandler_2;

		private MetroColorStyle metroColorStyle_0;

		private MetroThemeStyle metroThemeStyle_0;

		private MetroStyleManager metroStyleManager_0;

		private bool bool_0;

		private bool bool_1;

		private bool bool_2;

		private ScrollEventHandler scrollEventHandler_0;

		private bool bool_3 = true;

		private bool bool_4 = true;

		private bool bool_5;

		private Rectangle rectangle_0;

		private Rectangle rectangle_1;

		private bool bool_6;

		private bool bool_7;

		private bool bool_8;

		private int int_0 = 6;

		private int int_1;

		private int int_2;

		private int int_3;

		private int int_4;

		private int int_5;

		private int int_6;

		private readonly Timer timer_0 = new Timer();

		private int int_7 = 10;

		private bool bool_9;

		private bool bool_10;

		private bool bool_11;

		private bool bool_12;

		private MetroScrollOrientation metroScrollOrientation_0 = MetroScrollOrientation.Vertical;

		private ScrollOrientation scrollOrientation_0 = ScrollOrientation.VerticalScroll;

		private int int_8;

		private int int_9 = 100;

		private int int_10 = 1;

		private int int_11 = 10;

		private int int_12;

		private ScrollValueChangedDelegate scrollValueChangedDelegate_0;

		private bool bool_13;

		private Timer timer_1;

		private static MetroScrollBar uUUvkLVRUK8PvQEUEZU;

		[Category("Metro Appearance")]
		[DefaultValue(MetroColorStyle.Default)]
		public MetroColorStyle Style
		{
			get
			{
				if (!base.DesignMode && metroColorStyle_0 == MetroColorStyle.Default)
				{
					if (StyleManager == null || metroColorStyle_0 != 0)
					{
						if (StyleManager == null && metroColorStyle_0 == MetroColorStyle.Default)
						{
							return MetroColorStyle.Blue;
						}
						return metroColorStyle_0;
					}
					return StyleManager.Style;
				}
				return metroColorStyle_0;
			}
			set
			{
				metroColorStyle_0 = value;
			}
		}

		[DefaultValue(MetroThemeStyle.Default)]
		[Category("Metro Appearance")]
		public MetroThemeStyle Theme
		{
			get
			{
				if (!base.DesignMode && metroThemeStyle_0 == MetroThemeStyle.Default)
				{
					if (StyleManager != null && metroThemeStyle_0 == MetroThemeStyle.Default)
					{
						return StyleManager.Theme;
					}
					if (StyleManager == null && metroThemeStyle_0 == MetroThemeStyle.Default)
					{
						return MetroThemeStyle.Light;
					}
					return metroThemeStyle_0;
				}
				return metroThemeStyle_0;
			}
			set
			{
				metroThemeStyle_0 = value;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public MetroStyleManager StyleManager
		{
			get
			{
				return metroStyleManager_0;
			}
			set
			{
				metroStyleManager_0 = value;
			}
		}

		[DefaultValue(false)]
		[Category("Metro Appearance")]
		public bool UseCustomBackColor
		{
			get
			{
				return bool_0;
			}
			set
			{
				bool_0 = value;
			}
		}

		[Category("Metro Appearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[DefaultValue(false)]
		public bool UseCustomForeColor
		{
			get
			{
				return bool_1;
			}
			set
			{
				bool_1 = value;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue(false)]
		[Category("Metro Appearance")]
		public bool UseStyleColors
		{
			get
			{
				return bool_2;
			}
			set
			{
				bool_2 = value;
			}
		}

		[Browsable(false)]
		[Category("Metro Behaviour")]
		[DefaultValue(false)]
		public bool UseSelectable
		{
			get
			{
				return GetStyle(ControlStyles.Selectable);
			}
			set
			{
				SetStyle(ControlStyles.Selectable, value);
			}
		}

		public int MouseWheelBarPartitions
		{
			get
			{
				return int_7;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value", "MouseWheelBarPartitions has to be greather than zero");
				}
				int_7 = value;
			}
		}

		[DefaultValue(false)]
		[Category("Metro Appearance")]
		public bool UseBarColor
		{
			get
			{
				return bool_11;
			}
			set
			{
				bool_11 = value;
			}
		}

		[Category("Metro Appearance")]
		public int ScrollbarSize
		{
			get
			{
				if (Orientation != MetroScrollOrientation.Vertical)
				{
					return base.Height;
				}
				return base.Width;
			}
			set
			{
				if (Orientation == MetroScrollOrientation.Vertical)
				{
					base.Width = value;
				}
				else
				{
					base.Height = value;
				}
			}
		}

		[DefaultValue(false)]
		[Category("Metro Appearance")]
		public bool HighlightOnWheel
		{
			get
			{
				return bool_12;
			}
			set
			{
				bool_12 = value;
			}
		}

		public MetroScrollOrientation Orientation
		{
			get
			{
				return metroScrollOrientation_0;
			}
			set
			{
				if (value != metroScrollOrientation_0)
				{
					metroScrollOrientation_0 = value;
					if (value == MetroScrollOrientation.Vertical)
					{
						scrollOrientation_0 = ScrollOrientation.VerticalScroll;
					}
					else
					{
						scrollOrientation_0 = ScrollOrientation.HorizontalScroll;
					}
					base.Size = new Size(base.Height, base.Width);
					method_2();
				}
			}
		}

		public int Minimum
		{
			get
			{
				return int_8;
			}
			set
			{
				if (int_8 != value && value >= 0 && value < int_9)
				{
					int_8 = value;
					if (int_12 < value)
					{
						int_12 = value;
					}
					if (int_11 > int_9 - int_8)
					{
						int_11 = int_9 - int_8;
					}
					method_2();
					if (int_12 < value)
					{
						bool_13 = true;
						Value = value;
					}
					else
					{
						method_9(method_5());
						Refresh();
					}
				}
			}
		}

		public int Maximum
		{
			get
			{
				return int_9;
			}
			set
			{
				if (value != int_9 && value >= 1 && value > int_8)
				{
					int_9 = value;
					if (int_11 > int_9 - int_8)
					{
						int_11 = int_9 - int_8;
					}
					method_2();
					if (int_12 > value)
					{
						bool_13 = true;
						Value = int_9;
					}
					else
					{
						method_9(method_5());
						Refresh();
					}
				}
			}
		}

		[DefaultValue(1)]
		public int SmallChange
		{
			get
			{
				return int_10;
			}
			set
			{
				if (value != int_10 && value >= 1 && value < int_11)
				{
					int_10 = value;
					method_2();
				}
			}
		}

		[DefaultValue(5)]
		public int LargeChange
		{
			get
			{
				return int_11;
			}
			set
			{
				if (value != int_11 && value >= int_10 && value >= 2)
				{
					if (value <= int_9 - int_8)
					{
						int_11 = value;
					}
					else
					{
						int_11 = int_9 - int_8;
					}
					method_2();
				}
			}
		}

		[DefaultValue(0)]
		[Browsable(false)]
		public int Value
		{
			get
			{
				return int_12;
			}
			set
			{
				if (int_12 == value || value < int_8 || value > int_9)
				{
					return;
				}
				int_12 = value;
				method_9(method_5());
				method_0(ScrollEventType.ThumbPosition, -1, value, scrollOrientation_0);
				if (!bool_13 && bool_12)
				{
					if (!bool_9)
					{
						bool_9 = true;
					}
					if (timer_1 != null)
					{
						timer_1.Stop();
						timer_1.Start();
					}
					else
					{
						timer_1 = new Timer();
						timer_1.Interval = 1000;
						timer_1.Tick += timer_1_Tick;
						timer_1.Start();
					}
				}
				else
				{
					bool_13 = false;
				}
				Refresh();
			}
		}

		[Category("Metro Appearance")]
		public event EventHandler<MetroPaintEventArgs> CustomPaintBackground
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				eventHandler_0 = (EventHandler<MetroPaintEventArgs>)Delegate.Combine(eventHandler_0, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				eventHandler_0 = (EventHandler<MetroPaintEventArgs>)Delegate.Remove(eventHandler_0, value);
			}
		}

		[Category("Metro Appearance")]
		public event EventHandler<MetroPaintEventArgs> CustomPaint
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				eventHandler_1 = (EventHandler<MetroPaintEventArgs>)Delegate.Combine(eventHandler_1, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				eventHandler_1 = (EventHandler<MetroPaintEventArgs>)Delegate.Remove(eventHandler_1, value);
			}
		}

		[Category("Metro Appearance")]
		public event EventHandler<MetroPaintEventArgs> CustomPaintForeground
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				eventHandler_2 = (EventHandler<MetroPaintEventArgs>)Delegate.Combine(eventHandler_2, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				eventHandler_2 = (EventHandler<MetroPaintEventArgs>)Delegate.Remove(eventHandler_2, value);
			}
		}

		public event ScrollEventHandler Scroll
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				scrollEventHandler_0 = (ScrollEventHandler)Delegate.Combine(scrollEventHandler_0, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				scrollEventHandler_0 = (ScrollEventHandler)Delegate.Remove(scrollEventHandler_0, value);
			}
		}

		public event ScrollValueChangedDelegate ValueChanged
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				scrollValueChangedDelegate_0 = (ScrollValueChangedDelegate)Delegate.Combine(scrollValueChangedDelegate_0, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				scrollValueChangedDelegate_0 = (ScrollValueChangedDelegate)Delegate.Remove(scrollValueChangedDelegate_0, value);
			}
		}

		protected virtual void OnCustomPaintBackground(MetroPaintEventArgs e)
		{
			if (GetStyle(ControlStyles.UserPaint) && eventHandler_0 != null)
			{
				eventHandler_0(this, e);
			}
		}

		protected virtual void OnCustomPaint(MetroPaintEventArgs e)
		{
			if (GetStyle(ControlStyles.UserPaint) && eventHandler_1 != null)
			{
				eventHandler_1(this, e);
			}
		}

		protected virtual void OnCustomPaintForeground(MetroPaintEventArgs e)
		{
			if (GetStyle(ControlStyles.UserPaint) && eventHandler_2 != null)
			{
				eventHandler_2(this, e);
			}
		}

		private void method_0(ScrollEventType scrollEventType_0, int int_13, int int_14, ScrollOrientation scrollOrientation_1)
		{
			if (int_13 != int_14 && scrollValueChangedDelegate_0 != null)
			{
				scrollValueChangedDelegate_0(this, int_12);
			}
			if (scrollEventHandler_0 == null)
			{
				return;
			}
			if (scrollOrientation_1 == ScrollOrientation.HorizontalScroll)
			{
				if (scrollEventType_0 != ScrollEventType.EndScroll && bool_4)
				{
					scrollEventType_0 = ScrollEventType.First;
				}
				else if (!bool_4 && scrollEventType_0 == ScrollEventType.EndScroll)
				{
					bool_4 = true;
				}
			}
			else if (scrollEventType_0 != ScrollEventType.EndScroll && bool_3)
			{
				scrollEventType_0 = ScrollEventType.First;
			}
			else if (!bool_4 && scrollEventType_0 == ScrollEventType.EndScroll)
			{
				bool_3 = true;
			}
			scrollEventHandler_0(this, new ScrollEventArgs(scrollEventType_0, int_13, int_14, scrollOrientation_1));
		}

		private void timer_1_Tick(object sender, EventArgs e)
		{
			bool_9 = false;
			Invalidate();
			timer_1.Stop();
		}

		public MetroScrollBar()
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.Selectable | ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, value: true);
			base.Width = 10;
			base.Height = 200;
			method_2();
			timer_0.Interval = 20;
			timer_0.Tick += timer_0_Tick;
		}

		public MetroScrollBar(MetroScrollOrientation orientation)
			: this()
		{
			Orientation = orientation;
		}

		public MetroScrollBar(MetroScrollOrientation orientation, int width)
			: this(orientation)
		{
			base.Width = width;
		}

		public bool HitTest(Point point)
		{
			return rectangle_1.Contains(point);
		}

		[SecuritySafeCritical]
		public void BeginUpdate()
		{
			WinApi.SendMessage(base.Handle, 11, param: false, 0);
			bool_5 = true;
		}

		[SecuritySafeCritical]
		public void EndUpdate()
		{
			WinApi.SendMessage(base.Handle, 11, param: true, 0);
			bool_5 = false;
			method_2();
			Refresh();
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			try
			{
				Color color = BackColor;
				if (!bool_0)
				{
					color = ((base.Parent == null) ? MetroPaint.BackColor.Form(Theme) : ((base.Parent is IMetroControl) ? MetroPaint.BackColor.Form(Theme) : base.Parent.BackColor));
				}
				if (color.A == byte.MaxValue)
				{
					e.Graphics.Clear(color);
					return;
				}
				base.OnPaintBackground(e);
				OnCustomPaintBackground(new MetroPaintEventArgs(color, Color.Empty, e.Graphics));
			}
			catch
			{
				Invalidate();
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			try
			{
				if (GetStyle(ControlStyles.AllPaintingInWmPaint))
				{
					OnPaintBackground(e);
				}
				OnCustomPaint(new MetroPaintEventArgs(Color.Empty, Color.Empty, e.Graphics));
				OnPaintForeground(e);
			}
			catch
			{
				Invalidate();
			}
		}

		protected virtual void OnPaintForeground(PaintEventArgs e)
		{
			Color color = (bool_0 ? BackColor : ((base.Parent == null) ? MetroPaint.BackColor.Form(Theme) : ((base.Parent is IMetroControl) ? MetroPaint.BackColor.Form(Theme) : base.Parent.BackColor)));
			Color color2;
			Color color_;
			if (bool_9 && !bool_10 && base.Enabled)
			{
				color2 = MetroPaint.BackColor.ScrollBar.Thumb.Hover(Theme);
				color_ = MetroPaint.BackColor.ScrollBar.Bar.Hover(Theme);
			}
			else if (bool_9 && bool_10 && base.Enabled)
			{
				color2 = MetroPaint.BackColor.ScrollBar.Thumb.Press(Theme);
				color_ = MetroPaint.BackColor.ScrollBar.Bar.Press(Theme);
			}
			else if (base.Enabled)
			{
				color2 = MetroPaint.BackColor.ScrollBar.Thumb.Normal(Theme);
				color_ = MetroPaint.BackColor.ScrollBar.Bar.Normal(Theme);
			}
			else
			{
				color2 = MetroPaint.BackColor.ScrollBar.Thumb.Disabled(Theme);
				color_ = MetroPaint.BackColor.ScrollBar.Bar.Disabled(Theme);
			}
			method_1(e.Graphics, color, color2, color_);
			OnCustomPaintForeground(new MetroPaintEventArgs(color, color2, e.Graphics));
		}

		private void method_1(Graphics graphics_0, Color color_0, Color color_1, Color color_2)
		{
			if (bool_11)
			{
				using SolidBrush brush = new SolidBrush(color_2);
				graphics_0.FillRectangle(brush, base.ClientRectangle);
			}
			using (SolidBrush brush2 = new SolidBrush(color_0))
			{
				Rectangle rect = new Rectangle(rectangle_1.X - 1, rectangle_1.Y - 1, rectangle_1.Width + 2, rectangle_1.Height + 2);
				graphics_0.FillRectangle(brush2, rect);
			}
			using SolidBrush brush3 = new SolidBrush(color_1);
			graphics_0.FillRectangle(brush3, rectangle_1);
		}

		protected override void OnGotFocus(EventArgs e)
		{
			Invalidate();
			base.OnGotFocus(e);
		}

		protected override void OnLostFocus(EventArgs e)
		{
			bool_9 = false;
			bool_10 = false;
			Invalidate();
			base.OnLostFocus(e);
		}

		protected override void OnEnter(EventArgs e)
		{
			Invalidate();
			base.OnEnter(e);
		}

		protected override void OnLeave(EventArgs e)
		{
			bool_9 = false;
			bool_10 = false;
			Invalidate();
			base.OnLeave(e);
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			int num = e.Delta / 120 * (int_9 - int_8) / int_7;
			if (Orientation == MetroScrollOrientation.Vertical)
			{
				Value -= num;
			}
			else
			{
				Value += num;
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				bool_10 = true;
				Invalidate();
			}
			base.OnMouseDown(e);
			Focus();
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				Point location = e.Location;
				if (rectangle_1.Contains(location))
				{
					bool_8 = true;
					int_5 = ((metroScrollOrientation_0 == MetroScrollOrientation.Vertical) ? (location.Y - rectangle_1.Y) : (location.X - rectangle_1.X));
					Invalidate(rectangle_1);
					return;
				}
				int_6 = ((metroScrollOrientation_0 == MetroScrollOrientation.Vertical) ? location.Y : location.X);
				if (int_6 < ((metroScrollOrientation_0 == MetroScrollOrientation.Vertical) ? rectangle_1.Y : rectangle_1.X))
				{
					bool_6 = true;
				}
				else
				{
					bool_7 = true;
				}
				method_10(bool_14: true);
			}
			else if (e.Button == System.Windows.Forms.MouseButtons.Right)
			{
				int_6 = ((metroScrollOrientation_0 == MetroScrollOrientation.Vertical) ? e.Y : e.X);
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			bool_10 = false;
			base.OnMouseUp(e);
			if (e.Button != System.Windows.Forms.MouseButtons.Left)
			{
				return;
			}
			if (!bool_8)
			{
				if (!bool_6)
				{
					if (bool_7)
					{
						bool_7 = false;
						method_8();
					}
				}
				else
				{
					bool_6 = false;
					method_8();
				}
			}
			else
			{
				bool_8 = false;
				method_0(ScrollEventType.EndScroll, -1, int_12, scrollOrientation_0);
			}
			Invalidate();
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			bool_9 = true;
			Invalidate();
			base.OnMouseEnter(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			bool_9 = false;
			Invalidate();
			base.OnMouseLeave(e);
			method_3();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				if (!bool_8)
				{
					return;
				}
				int num = int_12;
				int num2 = ((metroScrollOrientation_0 == MetroScrollOrientation.Vertical) ? e.Location.Y : e.Location.X);
				int num3 = ((metroScrollOrientation_0 == MetroScrollOrientation.Vertical) ? (num2 / base.Height / int_1) : (num2 / base.Width / int_0));
				if (num2 <= int_4 + int_5)
				{
					method_9(int_4);
					int_12 = int_8;
					Invalidate();
				}
				else if (num2 >= int_3 + int_5)
				{
					method_9(int_3);
					int_12 = int_9;
					Invalidate();
				}
				else
				{
					method_9(num2 - int_5);
					int num4;
					int num5;
					if (Orientation == MetroScrollOrientation.Vertical)
					{
						num4 = base.Height - num3;
						num5 = rectangle_1.Y;
					}
					else
					{
						num4 = base.Width - num3;
						num5 = rectangle_1.X;
					}
					float num6 = 0f;
					if (num4 != 0)
					{
						num6 = (float)num5 / (float)num4;
					}
					int_12 = Convert.ToInt32(num6 * (float)(int_9 - int_8) + (float)int_8);
				}
				if (num != int_12)
				{
					method_0(ScrollEventType.ThumbTrack, num, int_12, scrollOrientation_0);
					Refresh();
				}
			}
			else if (base.ClientRectangle.Contains(e.Location))
			{
				if (e.Button != 0)
				{
					return;
				}
				if (!rectangle_1.Contains(e.Location))
				{
					if (base.ClientRectangle.Contains(e.Location))
					{
						Invalidate();
					}
				}
				else
				{
					Invalidate(rectangle_1);
				}
			}
			else
			{
				method_3();
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			bool_9 = true;
			bool_10 = true;
			Invalidate();
			base.OnKeyDown(e);
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			bool_9 = false;
			bool_10 = false;
			Invalidate();
			base.OnKeyUp(e);
		}

		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			base.SetBoundsCore(x, y, width, height, specified);
			if (base.DesignMode)
			{
				method_2();
			}
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			method_2();
		}

		protected override bool ProcessDialogKey(Keys keyData)
		{
			Keys keys = Keys.Up;
			Keys keys2 = Keys.Down;
			if (Orientation == MetroScrollOrientation.Horizontal)
			{
				keys = Keys.Left;
				keys2 = Keys.Right;
			}
			if (keyData == keys)
			{
				Value -= int_10;
				return true;
			}
			if (keyData == keys2)
			{
				Value += int_10;
				return true;
			}
			switch (keyData)
			{
			case Keys.Prior:
				Value = method_4(bool_14: false, bool_15: true);
				return true;
			case Keys.Next:
				if (int_12 + int_11 > int_9)
				{
					Value = int_9;
				}
				else
				{
					Value += int_11;
				}
				return true;
			case Keys.Home:
				Value = int_8;
				return true;
			case Keys.End:
				Value = int_9;
				return true;
			default:
				return base.ProcessDialogKey(keyData);
			}
		}

		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			Invalidate();
		}

		private void method_2()
		{
			if (!bool_5)
			{
				if (Orientation == MetroScrollOrientation.Vertical)
				{
					int_0 = ((base.Width > 0) ? base.Width : 10);
					int_1 = method_6();
					rectangle_0 = base.ClientRectangle;
					rectangle_0.Inflate(-1, -1);
					rectangle_1 = new Rectangle(base.ClientRectangle.X, base.ClientRectangle.Y, int_0, int_1);
					int_5 = rectangle_1.Height / 2;
					int_2 = base.ClientRectangle.Bottom;
					int_3 = int_2 - rectangle_1.Height;
					int_4 = base.ClientRectangle.Y;
				}
				else
				{
					int_1 = ((base.Height > 0) ? base.Height : 10);
					int_0 = method_6();
					rectangle_0 = base.ClientRectangle;
					rectangle_0.Inflate(-1, -1);
					rectangle_1 = new Rectangle(base.ClientRectangle.X, base.ClientRectangle.Y, int_0, int_1);
					int_5 = rectangle_1.Width / 2;
					int_2 = base.ClientRectangle.Right;
					int_3 = int_2 - rectangle_1.Width;
					int_4 = base.ClientRectangle.X;
				}
				method_9(method_5());
				Refresh();
			}
		}

		private void method_3()
		{
			bool_6 = false;
			bool_7 = false;
			method_8();
			Refresh();
		}

		private void timer_0_Tick(object sender, EventArgs e)
		{
			method_10(bool_14: true);
		}

		private int method_4(bool bool_14, bool bool_15)
		{
			int num;
			if (bool_15)
			{
				num = int_12 - (bool_14 ? int_10 : int_11);
				if (num < int_8)
				{
					num = int_8;
				}
			}
			else
			{
				num = int_12 + (bool_14 ? int_10 : int_11);
				if (num > int_9)
				{
					num = int_9;
				}
			}
			return num;
		}

		private int method_5()
		{
			if (int_1 != 0 && int_0 != 0)
			{
				int num = ((metroScrollOrientation_0 == MetroScrollOrientation.Vertical) ? (int_5 / base.Height / int_1) : (int_5 / base.Width / int_0));
				int num2 = ((Orientation != MetroScrollOrientation.Vertical) ? (base.Width - num) : (base.Height - num));
				int num3 = int_9 - int_8;
				float num4 = 0f;
				if (num3 != 0)
				{
					num4 = ((float)int_12 - (float)int_8) / (float)num3;
				}
				return Math.Max(int_4, Math.Min(int_3, Convert.ToInt32(num4 * (float)num2)));
			}
			return 0;
		}

		private int method_6()
		{
			int num = ((metroScrollOrientation_0 == MetroScrollOrientation.Vertical) ? base.Height : base.Width);
			if (int_9 == 0 || int_11 == 0)
			{
				return num;
			}
			float val = (float)int_11 * (float)num / (float)int_9;
			return Convert.ToInt32(Math.Min(num, Math.Max(val, 10f)));
		}

		private void method_7()
		{
			if (timer_0.Enabled)
			{
				timer_0.Interval = 10;
				return;
			}
			timer_0.Interval = 600;
			timer_0.Start();
		}

		private void method_8()
		{
			timer_0.Stop();
		}

		private void method_9(int int_13)
		{
			if (Orientation == MetroScrollOrientation.Vertical)
			{
				rectangle_1.Y = int_13;
			}
			else
			{
				rectangle_1.X = int_13;
			}
		}

		private void method_10(bool bool_14)
		{
			int num = int_12;
			ScrollEventType scrollEventType_ = ScrollEventType.First;
			int num2;
			int num3;
			if (Orientation == MetroScrollOrientation.Vertical)
			{
				num2 = rectangle_1.Y;
				num3 = rectangle_1.Height;
			}
			else
			{
				num2 = rectangle_1.X;
				num3 = rectangle_1.Width;
			}
			if (bool_7 && num2 + num3 < int_6)
			{
				scrollEventType_ = ScrollEventType.LargeIncrement;
				int_12 = method_4(bool_14: false, bool_15: false);
				if (int_12 == int_9)
				{
					method_9(int_3);
					scrollEventType_ = ScrollEventType.Last;
				}
				else
				{
					method_9(Math.Min(int_3, method_5()));
				}
			}
			else if (bool_6 && num2 > int_6)
			{
				scrollEventType_ = ScrollEventType.LargeDecrement;
				int_12 = method_4(bool_14: false, bool_15: true);
				if (int_12 == int_8)
				{
					method_9(int_4);
					scrollEventType_ = ScrollEventType.First;
				}
				else
				{
					method_9(Math.Max(int_4, method_5()));
				}
			}
			if (num != int_12)
			{
				method_0(scrollEventType_, num, int_12, scrollOrientation_0);
				Invalidate();
				if (bool_14)
				{
					method_7();
				}
			}
		}

		internal static bool jNCotsVAx9RFpoywr3m()
		{
			return uUUvkLVRUK8PvQEUEZU == null;
		}

		internal static void RQRjownIhtTJwbNX4RJ()
		{
		}
	}
}
