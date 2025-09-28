using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using MetroFramework.Components;
using MetroFramework.Drawing;
using MetroFramework.Interfaces;
using MetroFramework.Localization;

namespace MetroFramework.Controls
{
	[Designer("MetroFramework.Design.Controls.MetroToggleDesigner, MetroFramework.Design, Version=1.4.0.0, Culture=neutral, PublicKeyToken=5f91a84759bf584a")]
	[ToolboxBitmap(typeof(CheckBox))]
	public class MetroToggle : CheckBox, IMetroControl
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

		private MetroLocalize metroLocalize_0;

		private MetroLinkSize metroLinkSize_0;

		private MetroLinkWeight metroLinkWeight_0 = MetroLinkWeight.Regular;

		private bool bool_4 = true;

		private bool bool_5;

		private bool bool_6;

		private bool bool_7;

		internal static MetroToggle X7BOqQYBaGt1IBKDRIg;

		[DefaultValue(MetroColorStyle.Default)]
		[Category("Metro Appearance")]
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
				if (StyleManager == null && metroColorStyle_0 == MetroColorStyle.Default)
				{
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

		[Category("Metro Behaviour")]
		[DefaultValue(false)]
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

		[DefaultValue(MetroLinkSize.Small)]
		[Category("Metro Appearance")]
		public MetroLinkSize FontSize
		{
			get
			{
				return metroLinkSize_0;
			}
			set
			{
				metroLinkSize_0 = value;
			}
		}

		[DefaultValue(MetroLinkWeight.Regular)]
		[Category("Metro Appearance")]
		public MetroLinkWeight FontWeight
		{
			get
			{
				return metroLinkWeight_0;
			}
			set
			{
				metroLinkWeight_0 = value;
			}
		}

		[DefaultValue(true)]
		[Category("Metro Appearance")]
		public bool DisplayStatus
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

		[Browsable(false)]
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
			}
		}

		[Browsable(false)]
		public override string Text
		{
			get
			{
				if (base.Checked)
				{
					return metroLocalize_0.translate("StatusOn");
				}
				return metroLocalize_0.translate("StatusOff");
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

		public MetroToggle()
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, value: true);
			base.Name = "MetroToggle";
			metroLocalize_0 = new MetroLocalize(this);
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
			Color foreColor;
			Color color;
			if (bool_5 && !bool_6 && base.Enabled)
			{
				foreColor = MetroPaint.ForeColor.CheckBox.Hover(Theme);
				color = MetroPaint.BorderColor.CheckBox.Hover(Theme);
			}
			else if (bool_5 && bool_6 && base.Enabled)
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
			using (Pen pen = new Pen(color))
			{
				Rectangle rect = new Rectangle(DisplayStatus ? 30 : 0, 0, base.ClientRectangle.Width - ((!DisplayStatus) ? 1 : 31), base.ClientRectangle.Height - 1);
				e.Graphics.DrawRectangle(pen, rect);
			}
			Color color2 = ((!base.Checked) ? MetroPaint.BorderColor.CheckBox.Normal(Theme) : MetroPaint.GetStyleColor(Style));
			using (SolidBrush brush = new SolidBrush(color2))
			{
				Rectangle rect2 = new Rectangle((!DisplayStatus) ? 2 : 32, 2, base.ClientRectangle.Width - (DisplayStatus ? 34 : 4), base.ClientRectangle.Height - 4);
				e.Graphics.FillRectangle(brush, rect2);
			}
			Color color3 = BackColor;
			if (!bool_0)
			{
				color3 = MetroPaint.BackColor.Form(Theme);
			}
			using (SolidBrush brush2 = new SolidBrush(color3))
			{
				int num = (base.Checked ? (base.Width - 11) : (DisplayStatus ? 30 : 0));
				Rectangle rect3 = new Rectangle(num, 0, 11, base.ClientRectangle.Height);
				e.Graphics.FillRectangle(brush2, rect3);
			}
			using (SolidBrush brush3 = new SolidBrush(MetroPaint.BorderColor.CheckBox.Hover(Theme)))
			{
				int num2 = (base.Checked ? (base.Width - 10) : (DisplayStatus ? 30 : 0));
				Rectangle rect4 = new Rectangle(num2, 0, 10, base.ClientRectangle.Height);
				e.Graphics.FillRectangle(brush3, rect4);
			}
			if (DisplayStatus)
			{
				Rectangle bounds = new Rectangle(0, 0, 30, base.ClientRectangle.Height);
				TextRenderer.DrawText(e.Graphics, Text, MetroFonts.Link(metroLinkSize_0, metroLinkWeight_0), bounds, foreColor, MetroPaint.GetTextFormatFlags(TextAlign));
			}
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

		protected override void OnCheckedChanged(EventArgs e)
		{
			base.OnCheckedChanged(e);
			Invalidate();
		}

		public override Size GetPreferredSize(Size proposedSize)
		{
			Size preferredSize = base.GetPreferredSize(proposedSize);
			preferredSize.Width = (DisplayStatus ? 80 : 50);
			return preferredSize;
		}

		internal static bool or9M0xYOD2IbHMgktbJ()
		{
			return X7BOqQYBaGt1IBKDRIg == null;
		}
	}
}
