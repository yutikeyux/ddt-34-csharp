using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using MetroFramework.Components;
using MetroFramework.Drawing;
using MetroFramework.Interfaces;

namespace MetroFramework.Controls
{
	[DefaultEvent("Scroll")]
	[ToolboxBitmap(typeof(TrackBar))]
	public class MetroTrackBar : Control, IMetroControl
	{
		private EventHandler<MetroPaintEventArgs> eventHandler_0;

		private EventHandler<MetroPaintEventArgs> eventHandler_1;

		private EventHandler<MetroPaintEventArgs> eventHandler_2;

		private MetroColorStyle metroColorStyle_0;

		private MetroThemeStyle metroThemeStyle_0;

		private MetroStyleManager metroStyleManager_0;

		private bool bool_0;

		private bool bool_1;

		private bool bool_2;

		private EventHandler eventHandler_3;

		private ScrollEventHandler scrollEventHandler_0;

		private bool bool_3;

		private int int_0 = 50;

		private int int_1;

		private int int_2 = 100;

		private int int_3 = 1;

		private int int_4 = 5;

		private int int_5 = 10;

		private bool bool_4;

		private bool bool_5;

		private bool bool_6;

		private static MetroTrackBar h7K7Y8Y4XAZCwJUdeQh;

		[Category("Metro Appearance")]
		[DefaultValue(MetroColorStyle.Default)]
		public MetroColorStyle Style
		{
			get
			{
				if (base.DesignMode || metroColorStyle_0 != 0)
				{
					return metroColorStyle_0;
				}
				if (StyleManager != null && metroColorStyle_0 == MetroColorStyle.Default)
				{
					return StyleManager.Style;
				}
				if (StyleManager != null || metroColorStyle_0 != 0)
				{
					return metroColorStyle_0;
				}
				return MetroColorStyle.Blue;
			}
			set
			{
				metroColorStyle_0 = value;
			}
		}

		[Category("Metro Appearance")]
		[DefaultValue(MetroThemeStyle.Default)]
		public MetroThemeStyle Theme
		{
			get
			{
				if (base.DesignMode || metroThemeStyle_0 != 0)
				{
					return metroThemeStyle_0;
				}
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

		[DefaultValue(false)]
		[Browsable(false)]
		[Category("Metro Appearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
		[Category("Metro Appearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[DefaultValue(false)]
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
		[DefaultValue(true)]
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

		[Category("Metro Appearance")]
		[DefaultValue(false)]
		public bool DisplayFocus
		{
			get
			{
				return bool_3;
			}
			set
			{
				bool_3 = value;
			}
		}

		[DefaultValue(50)]
		public int Value
		{
			get
			{
				return int_0;
			}
			set
			{
				if (!((value >= int_1) & (value <= int_2)))
				{
					throw new ArgumentOutOfRangeException("Value is outside appropriate range (min, max)");
				}
				int_0 = value;
				method_0();
				Invalidate();
			}
		}

		[DefaultValue(0)]
		public int Minimum
		{
			get
			{
				return int_1;
			}
			set
			{
				if (value < int_2)
				{
					int_1 = value;
					if (int_0 < int_1)
					{
						int_0 = int_1;
						if (eventHandler_3 != null)
						{
							eventHandler_3(this, new EventArgs());
						}
					}
					Invalidate();
					return;
				}
				throw new ArgumentOutOfRangeException("Minimal value is greather than maximal one");
			}
		}

		[DefaultValue(100)]
		public int Maximum
		{
			get
			{
				return int_2;
			}
			set
			{
				if (value <= int_1)
				{
					throw new ArgumentOutOfRangeException("Maximal value is lower than minimal one");
				}
				int_2 = value;
				if (int_0 > int_2)
				{
					int_0 = int_2;
					if (eventHandler_3 != null)
					{
						eventHandler_3(this, new EventArgs());
					}
				}
				Invalidate();
			}
		}

		[DefaultValue(1)]
		public int SmallChange
		{
			get
			{
				return int_3;
			}
			set
			{
				int_3 = value;
			}
		}

		[DefaultValue(5)]
		public int LargeChange
		{
			get
			{
				return int_4;
			}
			set
			{
				int_4 = value;
			}
		}

		[DefaultValue(10)]
		public int MouseWheelBarPartitions
		{
			get
			{
				return int_5;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("MouseWheelBarPartitions has to be greather than zero");
				}
				int_5 = value;
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

		public event EventHandler ValueChanged
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				eventHandler_3 = (EventHandler)Delegate.Combine(eventHandler_3, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				eventHandler_3 = (EventHandler)Delegate.Remove(eventHandler_3, value);
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

		private void method_0()
		{
			if (eventHandler_3 != null)
			{
				eventHandler_3(this, EventArgs.Empty);
			}
		}

		private void method_1(ScrollEventType scrollEventType_0, int int_6)
		{
			if (scrollEventHandler_0 != null)
			{
				scrollEventHandler_0(this, new ScrollEventArgs(scrollEventType_0, int_6));
			}
		}

		public MetroTrackBar(int min, int max, int value)
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.Selectable | ControlStyles.UserMouse | ControlStyles.SupportsTransparentBackColor | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, value: true);
			BackColor = Color.Transparent;
			Minimum = min;
			Maximum = max;
			Value = value;
		}

		public MetroTrackBar()
			: this(0, 100, 50)
		{
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			try
			{
				Color color = BackColor;
				if (!bool_0)
				{
					color = MetroPaint.BackColor.Form(Theme);
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
			Color color_;
			Color color_2;
			if (bool_4 && !bool_5 && base.Enabled)
			{
				color_ = MetroPaint.BackColor.TrackBar.Thumb.Hover(Theme);
				color_2 = MetroPaint.BackColor.TrackBar.Bar.Hover(Theme);
			}
			else if (!bool_4 || !bool_5 || !base.Enabled)
			{
				if (base.Enabled)
				{
					color_ = MetroPaint.BackColor.TrackBar.Thumb.Normal(Theme);
					color_2 = MetroPaint.BackColor.TrackBar.Bar.Normal(Theme);
				}
				else
				{
					color_ = MetroPaint.BackColor.TrackBar.Thumb.Disabled(Theme);
					color_2 = MetroPaint.BackColor.TrackBar.Bar.Disabled(Theme);
				}
			}
			else
			{
				color_ = MetroPaint.BackColor.TrackBar.Thumb.Press(Theme);
				color_2 = MetroPaint.BackColor.TrackBar.Bar.Press(Theme);
			}
			method_2(e.Graphics, color_, color_2);
			if (bool_3 && bool_6)
			{
				ControlPaint.DrawFocusRectangle(e.Graphics, base.ClientRectangle);
			}
		}

		private void method_2(Graphics graphics_0, Color color_0, Color color_1)
		{
			int num = (int_0 - int_1) * (base.Width - 6) / (int_2 - int_1);
			using (SolidBrush brush = new SolidBrush(color_0))
			{
				Rectangle rect = new Rectangle(0, base.Height / 2 - 2, num, 4);
				graphics_0.FillRectangle(brush, rect);
				Rectangle rect2 = new Rectangle(num, base.Height / 2 - 8, 6, 16);
				graphics_0.FillRectangle(brush, rect2);
			}
			using SolidBrush brush2 = new SolidBrush(color_1);
			Rectangle rect3 = new Rectangle(num + 7, base.Height / 2 - 2, base.Width - num + 7, 4);
			graphics_0.FillRectangle(brush2, rect3);
		}

		protected override void OnGotFocus(EventArgs e)
		{
			bool_6 = true;
			Invalidate();
			base.OnGotFocus(e);
		}

		protected override void OnLostFocus(EventArgs e)
		{
			bool_6 = false;
			bool_4 = false;
			bool_5 = false;
			Invalidate();
			base.OnLostFocus(e);
		}

		protected override void OnEnter(EventArgs e)
		{
			bool_6 = true;
			Invalidate();
			base.OnEnter(e);
		}

		protected override void OnLeave(EventArgs e)
		{
			bool_6 = false;
			bool_4 = false;
			bool_5 = false;
			Invalidate();
			base.OnLeave(e);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			bool_4 = true;
			bool_5 = true;
			Invalidate();
			base.OnKeyDown(e);
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			bool_4 = false;
			bool_5 = false;
			Invalidate();
			base.OnKeyUp(e);
			switch (e.KeyCode)
			{
			case Keys.Prior:
				method_3(Value + int_4);
				method_1(ScrollEventType.LargeIncrement, Value);
				break;
			case Keys.Next:
				method_3(Value - int_4);
				method_1(ScrollEventType.LargeDecrement, Value);
				break;
			case Keys.End:
				Value = int_2;
				break;
			case Keys.Home:
				Value = int_1;
				break;
			case Keys.Up:
			case Keys.Right:
				method_3(Value + int_3);
				method_1(ScrollEventType.SmallIncrement, Value);
				break;
			case Keys.Left:
			case Keys.Down:
				method_3(Value - int_3);
				method_1(ScrollEventType.SmallDecrement, Value);
				break;
			}
			if (Value == int_1)
			{
				method_1(ScrollEventType.First, Value);
			}
			if (Value == int_2)
			{
				method_1(ScrollEventType.Last, Value);
			}
			Point point = PointToClient(System.Windows.Forms.Cursor.Position);
			OnMouseMove(new MouseEventArgs(System.Windows.Forms.MouseButtons.None, 0, point.X, point.Y, 0));
		}

		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (!((keyData == Keys.Tab) | (Control.ModifierKeys == Keys.Shift)))
			{
				OnKeyDown(new KeyEventArgs(keyData));
				return true;
			}
			return base.ProcessDialogKey(keyData);
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			bool_4 = true;
			Invalidate();
			base.OnMouseEnter(e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				bool_5 = true;
				Invalidate();
			}
			base.OnMouseDown(e);
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				base.Capture = true;
				method_1(ScrollEventType.ThumbTrack, int_0);
				method_0();
				OnMouseMove(e);
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (base.Capture & (e.Button == System.Windows.Forms.MouseButtons.Left))
			{
				ScrollEventType scrollEventType_ = ScrollEventType.ThumbPosition;
				int num = e.Location.X;
				float num2 = (float)(int_2 - int_1) / (float)(base.ClientSize.Width - 3);
				int_0 = (int)((float)num * num2 + (float)int_1);
				if (int_0 <= int_1)
				{
					int_0 = int_1;
					scrollEventType_ = ScrollEventType.First;
				}
				else if (int_0 >= int_2)
				{
					int_0 = int_2;
					scrollEventType_ = ScrollEventType.Last;
				}
				method_1(scrollEventType_, int_0);
				method_0();
				Invalidate();
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			bool_5 = false;
			Invalidate();
			base.OnMouseUp(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			bool_4 = false;
			Invalidate();
			base.OnMouseLeave(e);
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			int num = e.Delta / 120 * (int_2 - int_1) / int_5;
			method_3(Value + num);
		}

		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			Invalidate();
		}

		private void method_3(int int_6)
		{
			if (int_6 < int_1)
			{
				Value = int_1;
			}
			else if (int_6 <= int_2)
			{
				Value = int_6;
			}
			else
			{
				Value = int_2;
			}
		}

		internal static bool FGKghSYHuhjSWmGRale()
		{
			return h7K7Y8Y4XAZCwJUdeQh == null;
		}

		internal static void X6d7gjYtMXFAMdrWeD5()
		{
		}
	}
}
