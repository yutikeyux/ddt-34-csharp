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
	[ToolboxBitmap(typeof(DateTimePicker))]
	public class MetroDateTime : DateTimePicker, IMetroControl
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

		private bool bool_3;

		private MetroDateTimeSize metroDateTimeSize_0 = MetroDateTimeSize.Medium;

		private MetroDateTimeWeight metroDateTimeWeight_0 = MetroDateTimeWeight.Regular;

		private bool bool_4;

		private bool bool_5;

		private bool bool_6;

		internal static MetroDateTime UUV0OCeXnPqZFCFCQvt;

		[Category("Metro Appearance")]
		[DefaultValue(MetroColorStyle.Default)]
		public MetroColorStyle Style
		{
			get
			{
				if (!base.DesignMode && metroColorStyle_0 == MetroColorStyle.Default)
				{
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

		[Category("Metro Appearance")]
		[DefaultValue(false)]
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

		[Category("Metro Appearance")]
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

		[DefaultValue(false)]
		[Category("Metro Appearance")]
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

		[Category("Metro Appearance")]
		[DefaultValue(MetroDateTimeSize.Medium)]
		public MetroDateTimeSize FontSize
		{
			get
			{
				return metroDateTimeSize_0;
			}
			set
			{
				metroDateTimeSize_0 = value;
			}
		}

		[Category("Metro Appearance")]
		[DefaultValue(MetroDateTimeWeight.Regular)]
		public MetroDateTimeWeight FontWeight
		{
			get
			{
				return metroDateTimeWeight_0;
			}
			set
			{
				metroDateTimeWeight_0 = value;
			}
		}

		[Browsable(false)]
		[DefaultValue(false)]
		public new bool ShowUpDown
		{
			get
			{
				return base.ShowUpDown;
			}
			set
			{
				base.ShowUpDown = false;
			}
		}

		[Browsable(false)]
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
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

		public MetroDateTime()
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, value: true);
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
				if (color.A == byte.MaxValue && BackgroundImage == null)
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
			MinimumSize = new Size(0, GetPreferredSize(System.Drawing.Size.Empty).Height);
			Color color;
			Color color2;
			if (bool_4 && !bool_5 && base.Enabled)
			{
				color = MetroPaint.ForeColor.ComboBox.Hover(Theme);
				color2 = MetroPaint.GetStyleColor(Style);
			}
			else if (bool_4 && bool_5 && base.Enabled)
			{
				color = MetroPaint.ForeColor.ComboBox.Press(Theme);
				color2 = MetroPaint.GetStyleColor(Style);
			}
			else if (!base.Enabled)
			{
				color = MetroPaint.ForeColor.ComboBox.Disabled(Theme);
				color2 = MetroPaint.BorderColor.ComboBox.Disabled(Theme);
			}
			else
			{
				color = MetroPaint.ForeColor.ComboBox.Normal(Theme);
				color2 = MetroPaint.BorderColor.ComboBox.Normal(Theme);
			}
			using (Pen pen = new Pen(color2))
			{
				Rectangle rect = new Rectangle(0, 0, base.Width - 1, base.Height - 1);
				e.Graphics.DrawRectangle(pen, rect);
			}
			using (SolidBrush brush = new SolidBrush(color))
			{
				e.Graphics.FillPolygon(brush, new Point[3]
				{
					new Point(base.Width - 20, base.Height / 2 - 2),
					new Point(base.Width - 9, base.Height / 2 - 2),
					new Point(base.Width - 15, base.Height / 2 + 4)
				});
			}
			int num = 0;
			if (base.ShowCheckBox)
			{
				num = 15;
				using (Pen pen2 = new Pen(color2))
				{
					Rectangle rect2 = new Rectangle(3, base.Height / 2 - 6, 12, 12);
					e.Graphics.DrawRectangle(pen2, rect2);
				}
				if (base.Checked)
				{
					Color styleColor = MetroPaint.GetStyleColor(Style);
					using SolidBrush brush2 = new SolidBrush(styleColor);
					Rectangle rect3 = new Rectangle(5, base.Height / 2 - 4, 9, 9);
					e.Graphics.FillRectangle(brush2, rect3);
				}
				else
				{
					color = MetroPaint.ForeColor.ComboBox.Disabled(Theme);
				}
			}
			TextRenderer.DrawText(bounds: new Rectangle(2 + num, 2, base.Width - 20, base.Height - 4), dc: e.Graphics, text: Text, font: MetroFonts.DateTime(metroDateTimeSize_0, metroDateTimeWeight_0), foreColor: color, flags: TextFormatFlags.VerticalCenter);
			OnCustomPaintForeground(new MetroPaintEventArgs(Color.Empty, color, e.Graphics));
			if (bool_3 && bool_6)
			{
				ControlPaint.DrawFocusRectangle(e.Graphics, base.ClientRectangle);
			}
		}

		protected override void OnValueChanged(EventArgs eventargs)
		{
			base.OnValueChanged(eventargs);
			Invalidate();
		}

		protected override void OnGotFocus(EventArgs e)
		{
			bool_6 = true;
			bool_4 = true;
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
			bool_4 = true;
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
			if (e.KeyCode == Keys.Space)
			{
				bool_4 = true;
				bool_5 = true;
				Invalidate();
			}
			base.OnKeyDown(e);
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			Invalidate();
			base.OnKeyUp(e);
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
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			bool_5 = false;
			Invalidate();
			base.OnMouseUp(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			if (!bool_6)
			{
				bool_4 = false;
			}
			Invalidate();
			base.OnMouseLeave(e);
		}

		public override Size GetPreferredSize(Size proposedSize)
		{
			base.GetPreferredSize(proposedSize);
			using Graphics dc = CreateGraphics();
			string text = ((((string)(object)Text).Length > 0) ? Text : "MeasureText");
			proposedSize = new Size(int.MaxValue, int.MaxValue);
			Size result = TextRenderer.MeasureText(dc, text, MetroFonts.DateTime(metroDateTimeSize_0, metroDateTimeWeight_0), proposedSize, TextFormatFlags.VerticalCenter | TextFormatFlags.LeftAndRightPadding);
			result.Height += 10;
			return result;
		}

		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);
		}

		internal static bool l6hhJBea1ukCRRCDo6U()
		{
			return UUV0OCeXnPqZFCFCQvt == null;
		}

		internal static void koKqN9eVQx2OchUWF0M()
		{
		}
	}
}
