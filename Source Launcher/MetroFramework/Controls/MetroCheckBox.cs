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
	[Designer("MetroFramework.Design.Controls.MetroCheckBoxDesigner, MetroFramework.Design, Version=1.4.0.0, Culture=neutral, PublicKeyToken=5f91a84759bf584a")]
	[ToolboxBitmap(typeof(CheckBox))]
	public class MetroCheckBox : CheckBox, IMetroControl
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

		private MetroCheckBoxSize metroCheckBoxSize_0;

		private MetroCheckBoxWeight metroCheckBoxWeight_0 = MetroCheckBoxWeight.Regular;

		private bool bool_4;

		private bool bool_5;

		private bool bool_6;

		internal static MetroCheckBox dFOQTOaVf8PTyGqLHUH;

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
					if (StyleManager == null && metroColorStyle_0 == MetroColorStyle.Default)
					{
						return MetroColorStyle.Blue;
					}
					return metroColorStyle_0;
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

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		[DefaultValue(false)]
		[Category("Metro Appearance")]
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

		[DefaultValue(false)]
		[Category("Metro Behaviour")]
		[Browsable(false)]
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

		[DefaultValue(MetroCheckBoxSize.Small)]
		[Category("Metro Appearance")]
		public MetroCheckBoxSize FontSize
		{
			get
			{
				return metroCheckBoxSize_0;
			}
			set
			{
				metroCheckBoxSize_0 = value;
			}
		}

		[Category("Metro Appearance")]
		[DefaultValue(MetroCheckBoxWeight.Regular)]
		public MetroCheckBoxWeight FontWeight
		{
			get
			{
				return metroCheckBoxWeight_0;
			}
			set
			{
				metroCheckBoxWeight_0 = value;
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

		public MetroCheckBox()
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
					if (base.Parent is MetroTile)
					{
						color = MetroPaint.GetStyleColor(Style);
					}
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
			Color foreColor;
			Color color;
			if (!bool_1)
			{
				if (bool_4 && !bool_5 && base.Enabled)
				{
					foreColor = MetroPaint.ForeColor.CheckBox.Hover(Theme);
					color = MetroPaint.BorderColor.CheckBox.Hover(Theme);
				}
				else if (bool_4 && bool_5 && base.Enabled)
				{
					foreColor = MetroPaint.ForeColor.CheckBox.Press(Theme);
					color = MetroPaint.BorderColor.CheckBox.Press(Theme);
				}
				else if (!base.Enabled)
				{
					foreColor = MetroPaint.ForeColor.CheckBox.Disabled(Theme);
					color = MetroPaint.BorderColor.CheckBox.Disabled(Theme);
				}
				else
				{
					foreColor = (bool_2 ? MetroPaint.GetStyleColor(Style) : MetroPaint.ForeColor.CheckBox.Normal(Theme));
					color = MetroPaint.BorderColor.CheckBox.Normal(Theme);
				}
			}
			else
			{
				foreColor = ForeColor;
				color = ((bool_4 && !bool_5 && base.Enabled) ? MetroPaint.BorderColor.CheckBox.Hover(Theme) : ((bool_4 && bool_5 && base.Enabled) ? MetroPaint.BorderColor.CheckBox.Press(Theme) : ((!base.Enabled) ? MetroPaint.BorderColor.CheckBox.Disabled(Theme) : MetroPaint.BorderColor.CheckBox.Normal(Theme))));
			}
			Rectangle bounds = new Rectangle(16, 0, base.Width - 16, base.Height);
			Rectangle rect = new Rectangle(0, base.Height / 2 - 6, 12, 12);
			using (Pen pen = new Pen(color))
			{
				switch (base.CheckAlign)
				{
				case ContentAlignment.BottomRight:
					rect = new Rectangle(base.Width - 13, base.Height - 13, 12, 12);
					bounds = new Rectangle(0, 0, base.Width - 16, base.Height);
					break;
				case ContentAlignment.BottomCenter:
					rect = new Rectangle(base.Width / 2 - 6, base.Height - 13, 12, 12);
					bounds = new Rectangle(16, -10, base.Width - 8, base.Height);
					break;
				case ContentAlignment.BottomLeft:
					rect = new Rectangle(0, base.Height - 13, 12, 12);
					break;
				case ContentAlignment.MiddleRight:
					rect = new Rectangle(base.Width - 13, base.Height / 2 - 6, 12, 12);
					bounds = new Rectangle(0, 0, base.Width - 16, base.Height);
					break;
				case ContentAlignment.MiddleCenter:
					rect = new Rectangle(base.Width / 2 - 6, base.Height / 2 - 6, 12, 12);
					break;
				case ContentAlignment.MiddleLeft:
					rect = new Rectangle(0, base.Height / 2 - 6, 12, 12);
					break;
				case ContentAlignment.TopLeft:
					rect = new Rectangle(0, 0, 12, 12);
					break;
				case ContentAlignment.TopCenter:
					rect = new Rectangle(base.Width / 2 - 6, 0, 12, 12);
					bounds = new Rectangle(16, rect.Top + rect.Height - 5, base.Width - 8, base.Height);
					break;
				case ContentAlignment.TopRight:
					rect = new Rectangle(base.Width - 13, 0, 12, 12);
					bounds = new Rectangle(0, 0, base.Width - 16, base.Height);
					break;
				}
				e.Graphics.DrawRectangle(pen, rect);
			}
			if (base.Checked)
			{
				Color color2 = ((base.CheckState == System.Windows.Forms.CheckState.Indeterminate) ? color : MetroPaint.GetStyleColor(Style));
				using SolidBrush brush = new SolidBrush(color2);
				Rectangle rect2 = new Rectangle(rect.Left + 2, rect.Top + 2, 9, 9);
				e.Graphics.FillRectangle(brush, rect2);
			}
			TextRenderer.DrawText(e.Graphics, Text, MetroFonts.CheckBox(metroCheckBoxSize_0, metroCheckBoxWeight_0), bounds, foreColor, MetroPaint.GetTextFormatFlags(TextAlign));
			OnCustomPaintForeground(new MetroPaintEventArgs(Color.Empty, foreColor, e.Graphics));
			if (bool_3 && bool_6)
			{
				ControlPaint.DrawFocusRectangle(e.Graphics, base.ClientRectangle);
			}
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

		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			Invalidate();
		}

		protected override void OnCheckedChanged(EventArgs e)
		{
			base.OnCheckedChanged(e);
			Invalidate();
		}

		public override Size GetPreferredSize(Size proposedSize)
		{
			base.GetPreferredSize(proposedSize);
			using Graphics dc = CreateGraphics();
			proposedSize = new Size(int.MaxValue, int.MaxValue);
			Size result = TextRenderer.MeasureText(dc, Text, MetroFonts.CheckBox(metroCheckBoxSize_0, metroCheckBoxWeight_0), proposedSize, MetroPaint.GetTextFormatFlags(TextAlign));
			result.Width += 16;
			if (base.CheckAlign == ContentAlignment.TopCenter || base.CheckAlign == ContentAlignment.BottomCenter)
			{
				result.Height += 16;
				return result;
			}
			return result;
		}

		internal static bool lnNZg7anbSXDRRpdxDG()
		{
			return dFOQTOaVf8PTyGqLHUH == null;
		}

		internal static void ttSBEbaFQBpcRfGorF1()
		{
		}
	}
}
