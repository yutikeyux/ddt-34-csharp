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
	[ToolboxBitmap(typeof(ComboBox))]
	public class MetroComboBox : ComboBox, IMetroControl
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

		private MetroComboBoxSize tbPfuYvslW = MetroComboBoxSize.Medium;

		private MetroComboBoxWeight metroComboBoxWeight_0 = MetroComboBoxWeight.Regular;

		private string string_0 = "";

		private bool bool_4;

		private bool bool_5;

		private bool bool_6;

		private bool bool_7;

		internal static MetroComboBox fasJb5avccDqsAeVFS4;

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

		[Browsable(false)]
		[DefaultValue(System.Windows.Forms.DrawMode.OwnerDrawFixed)]
		public new DrawMode DrawMode
		{
			get
			{
				return System.Windows.Forms.DrawMode.OwnerDrawFixed;
			}
			set
			{
				base.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			}
		}

		[Browsable(false)]
		[DefaultValue(ComboBoxStyle.DropDownList)]
		public new ComboBoxStyle DropDownStyle
		{
			get
			{
				return ComboBoxStyle.DropDownList;
			}
			set
			{
				base.DropDownStyle = ComboBoxStyle.DropDownList;
			}
		}

		[Category("Metro Appearance")]
		[DefaultValue(MetroComboBoxSize.Medium)]
		public MetroComboBoxSize FontSize
		{
			get
			{
				return tbPfuYvslW;
			}
			set
			{
				tbPfuYvslW = value;
			}
		}

		[Category("Metro Appearance")]
		[DefaultValue(MetroComboBoxWeight.Regular)]
		public MetroComboBoxWeight FontWeight
		{
			get
			{
				return metroComboBoxWeight_0;
			}
			set
			{
				metroComboBoxWeight_0 = value;
			}
		}

		[DefaultValue("")]
		[Category("Metro Appearance")]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		public string PromptText
		{
			get
			{
				return string_0;
			}
			set
			{
				string_0 = ((string)(object)value).Trim();
				Invalidate();
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

		public MetroComboBox()
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, value: true);
			base.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			base.DropDownStyle = ComboBoxStyle.DropDownList;
			bool_4 = SelectedIndex == -1;
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
			base.ItemHeight = GetPreferredSize(System.Drawing.Size.Empty).Height;
			Color color;
			Color color2;
			if (!bool_5 || bool_6 || !base.Enabled)
			{
				if (bool_5 && bool_6 && base.Enabled)
				{
					color = MetroPaint.ForeColor.ComboBox.Press(Theme);
					color2 = MetroPaint.BorderColor.ComboBox.Press(Theme);
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
			}
			else
			{
				color = MetroPaint.ForeColor.ComboBox.Hover(Theme);
				color2 = MetroPaint.BorderColor.ComboBox.Hover(Theme);
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
			TextRenderer.DrawText(bounds: new Rectangle(2, 2, base.Width - 20, base.Height - 4), dc: e.Graphics, text: Text, font: MetroFonts.ComboBox(tbPfuYvslW, metroComboBoxWeight_0), foreColor: color, flags: TextFormatFlags.VerticalCenter);
			OnCustomPaintForeground(new MetroPaintEventArgs(Color.Empty, color, e.Graphics));
			if (bool_3 && bool_7)
			{
				ControlPaint.DrawFocusRectangle(e.Graphics, base.ClientRectangle);
			}
			if (bool_4)
			{
				method_1(e.Graphics);
			}
		}

		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			if (e.Index >= 0)
			{
				Color color = BackColor;
				if (!bool_0)
				{
					color = MetroPaint.BackColor.Form(Theme);
				}
				Color foreColor;
				if (e.State != (DrawItemState.NoAccelerator | DrawItemState.NoFocusRect) && e.State != 0)
				{
					using (SolidBrush brush = new SolidBrush(MetroPaint.GetStyleColor(Style)))
					{
						e.Graphics.FillRectangle(brush, new Rectangle(e.Bounds.Left, e.Bounds.Top, e.Bounds.Width, e.Bounds.Height));
					}
					foreColor = MetroPaint.ForeColor.Tile.Normal(Theme);
				}
				else
				{
					using (SolidBrush brush2 = new SolidBrush(color))
					{
						e.Graphics.FillRectangle(brush2, new Rectangle(e.Bounds.Left, e.Bounds.Top, e.Bounds.Width, e.Bounds.Height));
					}
					foreColor = MetroPaint.ForeColor.Link.Normal(Theme);
				}
				Rectangle bounds = new Rectangle(0, e.Bounds.Top, e.Bounds.Width, e.Bounds.Height);
				TextRenderer.DrawText(e.Graphics, GetItemText(base.Items[e.Index]), MetroFonts.ComboBox(tbPfuYvslW, metroComboBoxWeight_0), bounds, foreColor, TextFormatFlags.VerticalCenter);
			}
			else
			{
				base.OnDrawItem(e);
			}
		}

		private void method_0()
		{
			using Graphics graphics_ = CreateGraphics();
			method_1(graphics_);
		}

		private void method_1(Graphics graphics_0)
		{
			Color backColor = BackColor;
			if (!bool_0)
			{
				backColor = MetroPaint.BackColor.Form(Theme);
			}
			TextRenderer.DrawText(bounds: new Rectangle(2, 2, base.Width - 20, base.Height - 4), dc: graphics_0, text: string_0, font: MetroFonts.ComboBox(tbPfuYvslW, metroComboBoxWeight_0), foreColor: SystemColors.GrayText, backColor: backColor, flags: TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter);
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

		public override Size GetPreferredSize(Size proposedSize)
		{
			base.GetPreferredSize(proposedSize);
			using Graphics dc = CreateGraphics();
			string text = ((((string)(object)Text).Length <= 0) ? "MeasureText" : Text);
			proposedSize = new Size(int.MaxValue, int.MaxValue);
			Size result = TextRenderer.MeasureText(dc, text, MetroFonts.ComboBox(tbPfuYvslW, metroComboBoxWeight_0), proposedSize, TextFormatFlags.VerticalCenter | TextFormatFlags.LeftAndRightPadding);
			result.Height += 4;
			return result;
		}

		protected override void OnSelectedIndexChanged(EventArgs e)
		{
			base.OnSelectedIndexChanged(e);
			bool_4 = SelectedIndex == -1;
			Invalidate();
		}

		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);
			if ((m.Msg == 15 || m.Msg == 8465) && bool_4)
			{
				method_0();
			}
		}

		internal static bool UKt4Dra9RDydYb2avEi()
		{
			return fasJb5avccDqsAeVFS4 == null;
		}

		internal static void aiutcLawWRH9hrHpbE3()
		{
		}
	}
}
