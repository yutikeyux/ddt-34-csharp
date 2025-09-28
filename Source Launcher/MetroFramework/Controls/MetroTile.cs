using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using MetroFramework.Components;
using MetroFramework.Drawing;
using MetroFramework.Interfaces;

namespace MetroFramework.Controls
{
	[Designer("MetroFramework.Design.Controls.MetroTileDesigner, MetroFramework.Design, Version=1.4.0.0, Culture=neutral, PublicKeyToken=5f91a84759bf584a")]
	[ToolboxBitmap(typeof(Button))]
	public class MetroTile : Button, IContainerControl, IMetroControl
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

		private Control control_0;

		private bool bool_3 = true;

		private int int_0;

		private Image image_0;

		private bool bool_4;

		private ContentAlignment contentAlignment_0 = ContentAlignment.TopLeft;

		private MetroTileTextSize metroTileTextSize_0 = MetroTileTextSize.Medium;

		private MetroTileTextWeight metroTileTextWeight_0;

		private bool bool_5;

		private bool bool_6;

		private bool bool_7;

		internal static MetroTile nZHIqjC2gVBVwLdRSwG;

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

		[Browsable(false)]
		public Control ActiveControl
		{
			get
			{
				return control_0;
			}
			set
			{
				control_0 = value;
			}
		}

		[DefaultValue(true)]
		[Category("Metro Appearance")]
		public bool PaintTileCount
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

		[DefaultValue(0)]
		public int TileCount
		{
			get
			{
				return int_0;
			}
			set
			{
				int_0 = value;
			}
		}

		[DefaultValue(ContentAlignment.BottomLeft)]
		public new ContentAlignment TextAlign
		{
			get
			{
				return base.TextAlign;
			}
			set
			{
				base.TextAlign = value;
			}
		}

		[DefaultValue(null)]
		[Category("Metro Appearance")]
		public Image TileImage
		{
			get
			{
				return image_0;
			}
			set
			{
				image_0 = value;
			}
		}

		[DefaultValue(false)]
		[Category("Metro Appearance")]
		public bool UseTileImage
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

		[DefaultValue(ContentAlignment.TopLeft)]
		[Category("Metro Appearance")]
		public ContentAlignment TileImageAlign
		{
			get
			{
				return contentAlignment_0;
			}
			set
			{
				contentAlignment_0 = value;
			}
		}

		[DefaultValue(MetroTileTextSize.Medium)]
		[Category("Metro Appearance")]
		public MetroTileTextSize TileTextFontSize
		{
			get
			{
				return metroTileTextSize_0;
			}
			set
			{
				metroTileTextSize_0 = value;
				Refresh();
			}
		}

		[Category("Metro Appearance")]
		[DefaultValue(MetroTileTextWeight.Light)]
		public MetroTileTextWeight TileTextFontWeight
		{
			get
			{
				return metroTileTextWeight_0;
			}
			set
			{
				metroTileTextWeight_0 = value;
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

		public bool ActivateControl(Control ctrl)
		{
			if (!base.Controls.Contains(ctrl))
			{
				return false;
			}
			ctrl.Select();
			control_0 = ctrl;
			return true;
		}

		public MetroTile()
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, value: true);
			TextAlign = ContentAlignment.BottomLeft;
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			try
			{
				Color color = BackColor;
				if (!bool_0)
				{
					color = MetroPaint.GetStyleColor(Style);
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
			Color color = MetroPaint.BorderColor.Button.Normal(Theme);
			Color foreColor = ((bool_5 && !bool_6 && base.Enabled) ? MetroPaint.ForeColor.Tile.Hover(Theme) : ((bool_5 && bool_6 && base.Enabled) ? MetroPaint.ForeColor.Tile.Press(Theme) : (base.Enabled ? MetroPaint.ForeColor.Tile.Normal(Theme) : MetroPaint.ForeColor.Tile.Disabled(Theme))));
			if (bool_1)
			{
				foreColor = ForeColor;
			}
			if (bool_6 || bool_5 || bool_7)
			{
				using Pen pen = new Pen(color);
				pen.Width = 3f;
				Rectangle rect = new Rectangle(1, 1, base.Width - 3, base.Height - 3);
				e.Graphics.DrawRectangle(pen, rect);
			}
			e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
			e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
			if (bool_4 && image_0 != null)
			{
				Rectangle rect2 = contentAlignment_0 switch
				{
					ContentAlignment.MiddleCenter => new Rectangle(new Point(base.Width / 2 - TileImage.Width / 2, base.Height / 2 - TileImage.Height / 2), new Size(TileImage.Width, TileImage.Height)), 
					ContentAlignment.MiddleLeft => new Rectangle(new Point(0, base.Height / 2 - TileImage.Height / 2), new Size(TileImage.Width, TileImage.Height)), 
					ContentAlignment.TopLeft => new Rectangle(new Point(0, 0), new Size(TileImage.Width, TileImage.Height)), 
					ContentAlignment.TopCenter => new Rectangle(new Point(base.Width / 2 - TileImage.Width / 2, 0), new Size(TileImage.Width, TileImage.Height)), 
					ContentAlignment.TopRight => new Rectangle(new Point(base.Width - TileImage.Width, 0), new Size(TileImage.Width, TileImage.Height)), 
					ContentAlignment.BottomLeft => new Rectangle(new Point(0, base.Height - TileImage.Height), new Size(TileImage.Width, TileImage.Height)), 
					ContentAlignment.MiddleRight => new Rectangle(new Point(base.Width - TileImage.Width, base.Height / 2 - TileImage.Height / 2), new Size(TileImage.Width, TileImage.Height)), 
					ContentAlignment.BottomRight => new Rectangle(new Point(base.Width - TileImage.Width, base.Height - TileImage.Height), new Size(TileImage.Width, TileImage.Height)), 
					ContentAlignment.BottomCenter => new Rectangle(new Point(base.Width / 2 - TileImage.Width / 2, base.Height - TileImage.Height), new Size(TileImage.Width, TileImage.Height)), 
					_ => new Rectangle(new Point(0, 0), new Size(TileImage.Width, TileImage.Height)), 
				};
				e.Graphics.DrawImage(TileImage, rect2);
			}
			if (TileCount > 0 && bool_3)
			{
				Size size = TextRenderer.MeasureText(((int)TileCount).ToString(), MetroFonts.TileCount);
				e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
				TextRenderer.DrawText(e.Graphics, ((int)TileCount).ToString(), MetroFonts.TileCount, new Point(base.Width - size.Width, 0), foreColor);
				e.Graphics.TextRenderingHint = TextRenderingHint.SystemDefault;
			}
			TextRenderer.MeasureText(Text, MetroFonts.Tile(metroTileTextSize_0, metroTileTextWeight_0));
			TextFormatFlags flags = MetroPaint.GetTextFormatFlags(TextAlign) | TextFormatFlags.LeftAndRightPadding | TextFormatFlags.EndEllipsis;
			Rectangle clientRectangle = base.ClientRectangle;
			if (!bool_6)
			{
				clientRectangle.Inflate(-2, -10);
			}
			else
			{
				clientRectangle.Inflate(-4, -12);
			}
			TextRenderer.DrawText(e.Graphics, Text, MetroFonts.Tile(metroTileTextSize_0, metroTileTextWeight_0), clientRectangle, foreColor, flags);
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

		internal static bool VlKyQHCy7SFr6eQxO6U()
		{
			return nZHIqjC2gVBVwLdRSwG == null;
		}

		internal static void Y47XmuCrattfNYx75QK()
		{
		}
	}
}
