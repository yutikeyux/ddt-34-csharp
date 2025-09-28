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
	[Designer("MetroFramework.Design.Controls.MetroLinkDesigner, MetroFramework.Design, Version=1.4.0.0, Culture=neutral, PublicKeyToken=5f91a84759bf584a")]
	[ToolboxBitmap(typeof(LinkLabel))]
	public class MetroLink : Button, IMetroControl
	{
		private bool bool_0;

		private EventHandler<MetroPaintEventArgs> eventHandler_0;

		private EventHandler<MetroPaintEventArgs> eventHandler_1;

		private EventHandler<MetroPaintEventArgs> eventHandler_2;

		private MetroColorStyle metroColorStyle_0;

		private MetroThemeStyle metroThemeStyle_0;

		private MetroStyleManager metroStyleManager_0;

		private bool bool_1;

		private bool OudpSqvggy;

		private bool bool_2;

		private Image image_0;

		private Image image_1;

		private int int_0 = 16;

		private MetroLinkSize metroLinkSize_0;

		private MetroLinkWeight metroLinkWeight_0 = MetroLinkWeight.Bold;

		private bool bool_3;

		private bool bool_4;

		private bool bool_5;

		private Color color_0;

		private Image image_2;

		private Image image_3;

		private Image image_4;

		private Image image_5;

		internal static MetroLink xOpF7SMSkBRi996fCBV;

		[Category("Metro Appearance")]
		[DefaultValue(false)]
		public bool DisplayFocus
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
					if (StyleManager != null || metroThemeStyle_0 != 0)
					{
						return metroThemeStyle_0;
					}
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
				return bool_1;
			}
			set
			{
				bool_1 = value;
			}
		}

		[Category("Metro Appearance")]
		[DefaultValue(false)]
		public bool UseCustomForeColor
		{
			get
			{
				return OudpSqvggy;
			}
			set
			{
				OudpSqvggy = value;
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
		[Browsable(false)]
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
		[DefaultValue(null)]
		public new virtual Image Image
		{
			get
			{
				return image_0;
			}
			set
			{
				image_0 = value;
				method_1();
			}
		}

		[DefaultValue(null)]
		[Category("Metro Appearance")]
		public Image NoFocusImage
		{
			get
			{
				return image_1;
			}
			set
			{
				image_1 = value;
			}
		}

		[Category("Metro Appearance")]
		[DefaultValue(16)]
		public int ImageSize
		{
			get
			{
				return int_0;
			}
			set
			{
				int_0 = value;
				Invalidate();
			}
		}

		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
				if (AutoSize && image_0 != null)
				{
					base.Width = TextRenderer.MeasureText(value, MetroFonts.Link(metroLinkSize_0, metroLinkWeight_0)).Width;
					base.Width += int_0 + 2;
				}
			}
		}

		[Category("Metro Appearance")]
		[DefaultValue(MetroLinkSize.Small)]
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

		[Category("Metro Appearance")]
		[DefaultValue(MetroLinkWeight.Bold)]
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

		public MetroLink()
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, value: true);
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			try
			{
				Color color = BackColor;
				if (!bool_1)
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
			if (!OudpSqvggy)
			{
				if (bool_3 && !bool_4 && base.Enabled)
				{
					color_0 = MetroPaint.ForeColor.Link.Normal(Theme);
				}
				else if (bool_3 && bool_4 && base.Enabled)
				{
					color_0 = MetroPaint.ForeColor.Link.Press(Theme);
				}
				else if (base.Enabled)
				{
					color_0 = ((!bool_2) ? MetroPaint.ForeColor.Link.Hover(Theme) : MetroPaint.GetStyleColor(Style));
				}
				else
				{
					color_0 = MetroPaint.ForeColor.Link.Disabled(Theme);
				}
			}
			else
			{
				color_0 = ForeColor;
			}
			TextRenderer.DrawText(e.Graphics, Text, MetroFonts.Link(metroLinkSize_0, metroLinkWeight_0), base.ClientRectangle, color_0, MetroPaint.GetTextFormatFlags(TextAlign));
			OnCustomPaintForeground(new MetroPaintEventArgs(Color.Empty, color_0, e.Graphics));
			if (bool_0 && bool_5)
			{
				ControlPaint.DrawFocusRectangle(e.Graphics, base.ClientRectangle);
			}
			if (image_0 != null)
			{
				method_0(e.Graphics);
			}
		}

		private void method_0(Graphics graphics_0)
		{
			if (Image == null)
			{
				return;
			}
			int num = int_0;
			int num2 = int_0;
			if (int_0 == 0)
			{
				num = image_0.Width;
				num2 = image_0.Height;
			}
			Point location = new Point(2, (base.ClientRectangle.Height - int_0) / 2);
			int num3 = 0;
			switch (base.ImageAlign)
			{
			case ContentAlignment.BottomLeft:
				location = new Point(num3, base.ClientRectangle.Height - num2 - num3);
				break;
			case ContentAlignment.MiddleRight:
				location = new Point(base.ClientRectangle.Width - num - num3, (base.ClientRectangle.Height - num2) / 2);
				break;
			case ContentAlignment.BottomRight:
				location = new Point(base.ClientRectangle.Width - num - num3, base.ClientRectangle.Height - num2 - num3);
				break;
			case ContentAlignment.BottomCenter:
				location = new Point((base.ClientRectangle.Width - num) / 2, base.ClientRectangle.Height - num2 - num3);
				break;
			case ContentAlignment.MiddleCenter:
				location = new Point((base.ClientRectangle.Width - num) / 2, (base.ClientRectangle.Height - num2) / 2);
				break;
			case ContentAlignment.MiddleLeft:
				location = new Point(num3, (base.ClientRectangle.Height - num2) / 2);
				break;
			case ContentAlignment.TopLeft:
				location = new Point(num3, num3);
				break;
			case ContentAlignment.TopCenter:
				location = new Point((base.ClientRectangle.Width - num) / 2, num3);
				break;
			case ContentAlignment.TopRight:
				location = new Point(base.ClientRectangle.Width - num - num3, num3);
				break;
			}
			location.Y++;
			if (image_1 != null)
			{
				if (Theme == MetroThemeStyle.Dark)
				{
					graphics_0.DrawImage((!bool_3 || bool_4) ? image_1 : image_5, new Rectangle(location, new Size(num, num2)));
				}
				else
				{
					graphics_0.DrawImage((bool_3 && !bool_4) ? image_0 : image_1, new Rectangle(location, new Size(num, num2)));
				}
			}
			else if (Theme == MetroThemeStyle.Dark)
			{
				graphics_0.DrawImage((!bool_3 || bool_4) ? image_3 : image_5, new Rectangle(location, new Size(num, num2)));
			}
			else
			{
				graphics_0.DrawImage((!bool_3 || bool_4) ? image_2 : image_4, new Rectangle(location, new Size(num, num2)));
			}
		}

		private void method_1()
		{
			if (image_0 != null)
			{
				image_4 = image_0;
				image_5 = ApplyInvert(new Bitmap(image_0));
				image_3 = ApplyLight(new Bitmap(image_5));
				image_2 = ApplyLight(new Bitmap(image_4));
			}
		}

		public Bitmap ApplyInvert(Bitmap bitmapImage)
		{
			for (int i = 0; i < bitmapImage.Height; i++)
			{
				for (int j = 0; j < bitmapImage.Width; j++)
				{
					Color pixel = bitmapImage.GetPixel(j, i);
					byte a = pixel.A;
					byte red = (byte)(255 - pixel.R);
					byte green = (byte)(255 - pixel.G);
					byte blue = (byte)(255 - pixel.B);
					bitmapImage.SetPixel(j, i, Color.FromArgb(a, red, green, blue));
				}
			}
			return bitmapImage;
		}

		public Bitmap ApplyLight(Bitmap bitmapImage)
		{
			for (int i = 0; i < bitmapImage.Height; i++)
			{
				for (int j = 0; j < bitmapImage.Width; j++)
				{
					Color pixel = bitmapImage.GetPixel(j, i);
					byte alpha = pixel.A;
					if (pixel.A <= byte.MaxValue && pixel.A >= 100)
					{
						alpha = 90;
					}
					byte r = pixel.R;
					byte g = pixel.G;
					byte b = pixel.B;
					bitmapImage.SetPixel(j, i, Color.FromArgb(alpha, r, g, b));
				}
			}
			return bitmapImage;
		}

		protected override void OnGotFocus(EventArgs e)
		{
			bool_5 = true;
			bool_4 = false;
			Invalidate();
			base.OnGotFocus(e);
		}

		protected override void OnLostFocus(EventArgs e)
		{
			bool_5 = false;
			bool_3 = false;
			bool_4 = false;
			Invalidate();
			base.OnLostFocus(e);
		}

		protected override void OnEnter(EventArgs e)
		{
			bool_5 = true;
			bool_4 = true;
			Invalidate();
			base.OnEnter(e);
		}

		protected override void OnLeave(EventArgs e)
		{
			bool_5 = false;
			bool_3 = false;
			bool_4 = false;
			Invalidate();
			base.OnLeave(e);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Space)
			{
				bool_3 = true;
				bool_4 = true;
				Invalidate();
			}
			base.OnKeyDown(e);
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			if (!bool_5)
			{
				bool_3 = false;
				bool_4 = false;
			}
			Invalidate();
			base.OnKeyUp(e);
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			bool_3 = true;
			Invalidate();
			base.OnMouseEnter(e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				bool_4 = true;
				Invalidate();
				if (base.Name == "lnkClear" && base.Parent.GetType().Name == "MetroTextBox")
				{
					PerformClick();
				}
				if (base.Name == "lnkClear" && base.Parent.GetType().Name == "SearchControl")
				{
					PerformClick();
				}
			}
			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			bool_4 = false;
			Invalidate();
			base.OnMouseUp(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			bool_3 = false;
			bool_4 = false;
			Invalidate();
			base.OnMouseLeave(e);
		}

		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			Invalidate();
		}

		internal static bool cYh6JFMt5kwKqG6sOEk()
		{
			return xOpF7SMSkBRi996fCBV == null;
		}

		internal static void XF2jf2M8kbiGJflnhLV()
		{
		}
	}
}
