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
	[DefaultEvent("Click")]
	[ToolboxBitmap(typeof(Button))]
	[Designer("MetroFramework.Design.Controls.MetroButtonDesigner, MetroFramework.Design, Version=1.4.0.0, Culture=neutral, PublicKeyToken=5f91a84759bf584a")]
	public class MetroButton : Button, IMetroControl
	{
		private EventHandler<MetroPaintEventArgs> miOfqfBcKp;

		private EventHandler<MetroPaintEventArgs> eventHandler_0;

		private EventHandler<MetroPaintEventArgs> eventHandler_1;

		private MetroColorStyle metroColorStyle_0;

		private MetroThemeStyle metroThemeStyle_0;

		private MetroStyleManager metroStyleManager_0;

		private bool bool_0;

		private bool bool_1;

		private bool bool_2;

		private bool bool_3;

		private bool bool_4;

		private MetroButtonSize metroButtonSize_0;

		private MetroButtonWeight metroButtonWeight_0 = MetroButtonWeight.Bold;

		private bool bool_5;

		private bool bool_6;

		private bool bool_7;

		internal static MetroButton ADR4OAaP3od0PVuA6fD;

		[DefaultValue(MetroColorStyle.Default)]
		[Category("Metro Appearance")]
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

		[DefaultValue(false)]
		[Category("Metro Appearance")]
		public bool Highlight
		{
			get
			{
				return bool_4;
			}
			set
			{
				bool_4 = value;
			}
		}

		[DefaultValue(MetroButtonSize.Small)]
		[Category("Metro Appearance")]
		public MetroButtonSize FontSize
		{
			get
			{
				return metroButtonSize_0;
			}
			set
			{
				metroButtonSize_0 = value;
			}
		}

		[DefaultValue(MetroButtonWeight.Bold)]
		[Category("Metro Appearance")]
		public MetroButtonWeight FontWeight
		{
			get
			{
				return metroButtonWeight_0;
			}
			set
			{
				metroButtonWeight_0 = value;
			}
		}

		[Category("Metro Appearance")]
		public event EventHandler<MetroPaintEventArgs> CustomPaintBackground
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				miOfqfBcKp = (EventHandler<MetroPaintEventArgs>)Delegate.Combine(miOfqfBcKp, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				miOfqfBcKp = (EventHandler<MetroPaintEventArgs>)Delegate.Remove(miOfqfBcKp, value);
			}
		}

		[Category("Metro Appearance")]
		public event EventHandler<MetroPaintEventArgs> CustomPaint
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
		public event EventHandler<MetroPaintEventArgs> CustomPaintForeground
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

		protected virtual void OnCustomPaintBackground(MetroPaintEventArgs e)
		{
			if (GetStyle(ControlStyles.UserPaint) && miOfqfBcKp != null)
			{
				miOfqfBcKp(this, e);
			}
		}

		protected virtual void OnCustomPaint(MetroPaintEventArgs e)
		{
			if (GetStyle(ControlStyles.UserPaint) && eventHandler_0 != null)
			{
				eventHandler_0(this, e);
			}
		}

		protected virtual void OnCustomPaintForeground(MetroPaintEventArgs e)
		{
			if (GetStyle(ControlStyles.UserPaint) && eventHandler_1 != null)
			{
				eventHandler_1(this, e);
			}
		}

		public MetroButton()
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, value: true);
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			try
			{
				Color color = BackColor;
				if (!bool_5 || bool_6 || !base.Enabled)
				{
					if (bool_5 && bool_6 && base.Enabled)
					{
						color = MetroPaint.BackColor.Button.Press(Theme);
					}
					else if (base.Enabled)
					{
						if (!bool_0)
						{
							color = MetroPaint.BackColor.Button.Normal(Theme);
						}
					}
					else
					{
						color = MetroPaint.BackColor.Button.Disabled(Theme);
					}
				}
				else
				{
					color = MetroPaint.BackColor.Button.Hover(Theme);
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
			Color color;
			Color foreColor;
			if (bool_5 && !bool_6 && base.Enabled)
			{
				color = MetroPaint.BorderColor.Button.Hover(Theme);
				foreColor = MetroPaint.ForeColor.Button.Hover(Theme);
			}
			else if (bool_5 && bool_6 && base.Enabled)
			{
				color = MetroPaint.BorderColor.Button.Press(Theme);
				foreColor = MetroPaint.ForeColor.Button.Press(Theme);
			}
			else if (base.Enabled)
			{
				color = MetroPaint.BorderColor.Button.Normal(Theme);
				foreColor = (bool_1 ? ForeColor : (bool_2 ? MetroPaint.GetStyleColor(Style) : MetroPaint.ForeColor.Button.Normal(Theme)));
			}
			else
			{
				color = MetroPaint.BorderColor.Button.Disabled(Theme);
				foreColor = MetroPaint.ForeColor.Button.Disabled(Theme);
			}
			using (Pen pen = new Pen(color))
			{
				Rectangle rect = new Rectangle(0, 0, base.Width - 1, base.Height - 1);
				e.Graphics.DrawRectangle(pen, rect);
			}
			if (Highlight && !bool_5 && !bool_6 && base.Enabled)
			{
				using Pen pen2 = MetroPaint.GetStylePen(Style);
				Rectangle rect2 = new Rectangle(0, 0, base.Width - 1, base.Height - 1);
				e.Graphics.DrawRectangle(pen2, rect2);
				rect2 = new Rectangle(1, 1, base.Width - 3, base.Height - 3);
				e.Graphics.DrawRectangle(pen2, rect2);
			}
			TextRenderer.DrawText(e.Graphics, Text, MetroFonts.Button(metroButtonSize_0, metroButtonWeight_0), base.ClientRectangle, foreColor, MetroPaint.GetTextFormatFlags(TextAlign));
			OnCustomPaintForeground(new MetroPaintEventArgs(Color.Empty, foreColor, e.Graphics));
			if (bool_3 && bool_7)
			{
				ControlPaint.DrawFocusRectangle(e.Graphics, base.ClientRectangle);
			}
		}

		protected override void OnGotFocus(EventArgs e)
		{
			bool_7 = true;
			bool_5 = true;
			Invalidate();
			base.OnGotFocus(e);
		}

		protected override void OnLostFocus(EventArgs e)
		{
			bool_7 = false;
			bool_5 = false;
			bool_6 = false;
			Invalidate();
			base.OnLostFocus(e);
		}

		protected override void OnEnter(EventArgs e)
		{
			bool_7 = true;
			bool_5 = true;
			Invalidate();
			base.OnEnter(e);
		}

		protected override void OnLeave(EventArgs e)
		{
			bool_7 = false;
			bool_5 = false;
			bool_6 = false;
			Invalidate();
			base.OnLeave(e);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Space)
			{
				bool_5 = true;
				bool_6 = true;
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
			bool_5 = true;
			Invalidate();
			base.OnMouseEnter(e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				bool_6 = true;
				Invalidate();
			}
			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			bool_6 = false;
			Invalidate();
			base.OnMouseUp(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			if (!bool_7)
			{
				bool_5 = false;
			}
			Invalidate();
			base.OnMouseLeave(e);
		}

		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			Invalidate();
		}

		internal static bool GSvdRpaIbjbKoqofHyi()
		{
			return ADR4OAaP3od0PVuA6fD == null;
		}

		internal static void XjB9D5axWX1DRdBuA5l()
		{
		}
	}
}
