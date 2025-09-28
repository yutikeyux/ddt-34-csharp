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
	[Designer("MetroFramework.Design.Controls.MetroTextBoxDesigner, MetroFramework.Design, Version=1.4.0.0, Culture=neutral, PublicKeyToken=5f91a84759bf584a")]
	public class MetroTextBox : Control, IMetroControl
	{
		public delegate void ButClick(object sender, EventArgs e);

		private class Class19 : TextBox
		{
			private bool bool_0;

			private string string_0 = "";

			private Color color_0 = MetroPaint.ForeColor.Button.Disabled(MetroThemeStyle.Dark);

			private Font font_0 = MetroFonts.WaterMark(MetroLabelSize.Small, MetroWaterMarkWeight.Italic);

			private static object j4yAfkCtF17lI8g5GPV;

			[Browsable(true)]
			[EditorBrowsable(EditorBrowsableState.Always)]
			[DefaultValue("")]
			public string String_0
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

			[SpecialName]
			public Color method_0()
			{
				return color_0;
			}

			[SpecialName]
			public void method_1(Color color_1)
			{
				color_0 = color_1;
				Invalidate();
			}

			[SpecialName]
			public Font method_2()
			{
				return font_0;
			}

			[SpecialName]
			public void method_3(Font font_1)
			{
				font_0 = font_1;
			}

			public Class19()
			{
				SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, value: true);
				bool_0 = ((string)(object)((string)(object)Text).Trim()).Length == 0;
			}

			private void method_4()
			{
				using Graphics graphics_ = CreateGraphics();
				method_5(graphics_);
			}

			private void method_5(Graphics graphics_0)
			{
				TextFormatFlags textFormatFlags = TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding;
				Rectangle clientRectangle = base.ClientRectangle;
				switch (base.TextAlign)
				{
				case HorizontalAlignment.Left:
					clientRectangle.Offset(1, 0);
					break;
				case HorizontalAlignment.Right:
					textFormatFlags |= TextFormatFlags.Right;
					clientRectangle.Offset(-2, 0);
					break;
				case HorizontalAlignment.Center:
					textFormatFlags |= TextFormatFlags.HorizontalCenter;
					clientRectangle.Offset(1, 0);
					break;
				}
				new SolidBrush(method_0());
				TextRenderer.DrawText(graphics_0, string_0, font_0, clientRectangle, color_0, BackColor, textFormatFlags);
			}

			protected override void OnPaint(PaintEventArgs pevent)
			{
				base.OnPaint(pevent);
				if (bool_0)
				{
					method_5(pevent.Graphics);
				}
			}

			protected override void OnCreateControl()
			{
				base.OnCreateControl();
			}

			protected override void OnTextAlignChanged(EventArgs e)
			{
				base.OnTextAlignChanged(e);
				Invalidate();
			}

			protected override void OnTextChanged(EventArgs e)
			{
				base.OnTextChanged(e);
				bool_0 = ((string)(object)((string)(object)Text).Trim()).Length == 0;
				Invalidate();
			}

			protected override void WndProc(ref Message m)
			{
				base.WndProc(ref m);
				if ((m.Msg == 15 || m.Msg == 8465) && bool_0 && !GetStyle(ControlStyles.UserPaint))
				{
					method_4();
				}
			}

			protected override void OnLostFocus(EventArgs e)
			{
				base.OnLostFocus(e);
			}

			internal static bool EFDMgjCDFbfWkch678s()
			{
				return j4yAfkCtF17lI8g5GPV == null;
			}

			internal static void MuDxLACTj4h0yMgxmTq()
			{
			}
		}

		public delegate void LUClear();

		[ToolboxItem(false)]
		public class MetroTextButton : Button, IMetroControl
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

			private bool bool_4;

			private Bitmap bitmap_0;

			internal static MetroTextButton jXwHEfCKLnt7Yo4sSJF;

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

			public new Image Image
			{
				get
				{
					return base.Image;
				}
				set
				{
					base.Image = value;
					if (value != null)
					{
						bitmap_0 = ApplyInvert(new Bitmap(value));
					}
				}
			}

			protected Size iconSize
			{
				get
				{
					if (Image == null)
					{
						return new Size(-1, -1);
					}
					Size size = Image.Size;
					double num = 14.0 / (double)size.Height;
					new Point(1, 1);
					return new Size((int)((double)size.Width * num), (int)((double)size.Height * num));
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

			protected override void OnCreateControl()
			{
				base.OnCreateControl();
				SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, value: true);
			}

			protected override void OnPaint(PaintEventArgs e)
			{
				MetroThemeStyle theme = Theme;
				MetroColorStyle style = Style;
				Color foreColor;
				Color color;
				if (base.Parent != null)
				{
					if (!(base.Parent is IMetroForm))
					{
						if (!(base.Parent is IMetroControl))
						{
							foreColor = MetroPaint.ForeColor.Button.Press(theme);
							color = MetroPaint.GetStyleColor(style);
						}
						else
						{
							theme = ((IMetroControl)base.Parent).Theme;
							style = ((IMetroControl)base.Parent).Style;
							foreColor = MetroPaint.ForeColor.Button.Press(theme);
							color = MetroPaint.GetStyleColor(style);
						}
					}
					else
					{
						theme = ((IMetroForm)base.Parent).Theme;
						style = ((IMetroForm)base.Parent).Style;
						foreColor = MetroPaint.ForeColor.Button.Press(theme);
						color = MetroPaint.GetStyleColor(style);
					}
				}
				else
				{
					foreColor = MetroPaint.ForeColor.Button.Press(theme);
					color = MetroPaint.BackColor.Form(theme);
				}
				if (bool_3 && !bool_4 && base.Enabled)
				{
					_ = color.R;
					_ = color.G;
					_ = color.B;
					color = ControlPaint.Light(color, 0.25f);
				}
				else if (bool_3 && bool_4 && base.Enabled)
				{
					foreColor = MetroPaint.ForeColor.Button.Press(theme);
					color = MetroPaint.GetStyleColor(style);
				}
				else if (!base.Enabled)
				{
					foreColor = MetroPaint.ForeColor.Button.Disabled(theme);
					color = MetroPaint.BackColor.Button.Disabled(theme);
				}
				else
				{
					foreColor = MetroPaint.ForeColor.Button.Press(theme);
				}
				e.Graphics.Clear(color);
				Font font = MetroFonts.Button(MetroButtonSize.Small, MetroButtonWeight.Bold);
				TextRenderer.DrawText(e.Graphics, Text, font, base.ClientRectangle, foreColor, color, TextFormatFlags.EndEllipsis | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
				method_0(e.Graphics);
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

			private void method_0(Graphics graphics_0)
			{
				if (Image != null)
				{
					Point location = new Point(2, (base.ClientRectangle.Height - iconSize.Height) / 2);
					int num = 5;
					switch (base.ImageAlign)
					{
					case ContentAlignment.BottomLeft:
						location = new Point(num, base.ClientRectangle.Height - iconSize.Height - num);
						break;
					case ContentAlignment.MiddleRight:
						location = new Point(base.ClientRectangle.Width - iconSize.Width - num, (base.ClientRectangle.Height - iconSize.Height) / 2);
						break;
					case ContentAlignment.BottomRight:
						location = new Point(base.ClientRectangle.Width - iconSize.Width - num, base.ClientRectangle.Height - iconSize.Height - num);
						break;
					case ContentAlignment.BottomCenter:
						location = new Point((base.ClientRectangle.Width - iconSize.Width) / 2, base.ClientRectangle.Height - iconSize.Height - num);
						break;
					case ContentAlignment.MiddleCenter:
						location = new Point((base.ClientRectangle.Width - iconSize.Width) / 2, (base.ClientRectangle.Height - iconSize.Height) / 2);
						break;
					case ContentAlignment.MiddleLeft:
						location = new Point(num, (base.ClientRectangle.Height - iconSize.Height) / 2);
						break;
					case ContentAlignment.TopLeft:
						location = new Point(num, num);
						break;
					case ContentAlignment.TopCenter:
						location = new Point((base.ClientRectangle.Width - iconSize.Width) / 2, num);
						break;
					case ContentAlignment.TopRight:
						location = new Point(base.ClientRectangle.Width - iconSize.Width - num, num);
						break;
					}
					graphics_0.DrawImage((Theme != MetroThemeStyle.Dark) ? (bool_4 ? Image : bitmap_0) : (bool_4 ? bitmap_0 : Image), new Rectangle(location, iconSize));
				}
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
				Invalidate();
				base.OnMouseLeave(e);
			}

			internal static bool vCBc6KCv6BQvtmSDDqs()
			{
				return jXwHEfCKLnt7Yo4sSJF == null;
			}
		}

		private EventHandler<MetroPaintEventArgs> eventHandler_0;

		private EventHandler<MetroPaintEventArgs> eventHandler_1;

		private EventHandler<MetroPaintEventArgs> eventHandler_2;

		private MetroColorStyle metroColorStyle_0;

		private MetroThemeStyle metroThemeStyle_0;

		private MetroStyleManager metroStyleManager_0;

		private bool bool_0;

		private bool bool_1;

		private bool bool_2;

		private Class19 class19_0;

		private MetroTextBoxSize metroTextBoxSize_0;

		private MetroTextBoxWeight metroTextBoxWeight_0 = MetroTextBoxWeight.Regular;

		private Image image_0;

		private bool bool_3;

		private bool bool_4;

		private MetroTextButton metroTextButton_0;

		private bool bool_5;

		private MetroLink lnkClear;

		private bool bool_6;

		private bool bool_7;

		private EventHandler eventHandler_3;

		private bool bool_8;

		private bool bool_9;

		private ButClick butClick_0;

		private LUClear luclear_0;

		internal static MetroTextBox oJc3FLCxWifZqivuDKe;

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

		[DefaultValue(MetroTextBoxSize.Small)]
		[Category("Metro Appearance")]
		public MetroTextBoxSize FontSize
		{
			get
			{
				return metroTextBoxSize_0;
			}
			set
			{
				metroTextBoxSize_0 = value;
				method_3();
			}
		}

		[DefaultValue(MetroTextBoxWeight.Regular)]
		[Category("Metro Appearance")]
		public MetroTextBoxWeight FontWeight
		{
			get
			{
				return metroTextBoxWeight_0;
			}
			set
			{
				metroTextBoxWeight_0 = value;
				method_3();
			}
		}

		[Obsolete("Use watermark")]
		[Category("Metro Appearance")]
		[DefaultValue("")]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		public string PromptText
		{
			get
			{
				return class19_0.String_0;
			}
			set
			{
				class19_0.String_0 = value;
			}
		}

		[DefaultValue("")]
		[Browsable(true)]
		[Category("Metro Appearance")]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public string WaterMark
		{
			get
			{
				return class19_0.String_0;
			}
			set
			{
				class19_0.String_0 = value;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Always)]
		[Category("Metro Appearance")]
		[Browsable(true)]
		[DefaultValue(null)]
		public Image Icon
		{
			get
			{
				return image_0;
			}
			set
			{
				image_0 = value;
				Refresh();
			}
		}

		[Category("Metro Appearance")]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DefaultValue(false)]
		public bool IconRight
		{
			get
			{
				return bool_3;
			}
			set
			{
				bool_3 = value;
				Refresh();
			}
		}

		[DefaultValue(false)]
		[Category("Metro Appearance")]
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public bool DisplayIcon
		{
			get
			{
				return bool_4;
			}
			set
			{
				bool_4 = value;
				Refresh();
			}
		}

		protected Size iconSize
		{
			get
			{
				if (!bool_4 || image_0 == null)
				{
					return new Size(-1, -1);
				}
				int num = ((image_0.Height > base.ClientRectangle.Height) ? base.ClientRectangle.Height : image_0.Height);
				Size size = image_0.Size;
				double num2 = (double)num / (double)size.Height;
				new Point(1, 1);
				return new Size((int)((double)size.Width * num2), (int)((double)size.Height * num2));
			}
		}

		protected int ButtonWidth
		{
			get
			{
				int result = 0;
				if (metroTextButton_0 != null)
				{
					result = (bool_5 ? metroTextButton_0.Width : 0);
				}
				return result;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Always)]
		[DefaultValue(false)]
		[Browsable(true)]
		[Category("Metro Appearance")]
		public bool ShowButton
		{
			get
			{
				return bool_5;
			}
			set
			{
				bool_5 = value;
				Refresh();
			}
		}

		[Category("Metro Appearance")]
		[DefaultValue(false)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		public bool ShowClearButton
		{
			get
			{
				return bool_6;
			}
			set
			{
				bool_6 = value;
				Refresh();
			}
		}

		[DefaultValue(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Category("Metro Appearance")]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public MetroTextButton CustomButton
		{
			get
			{
				return metroTextButton_0;
			}
			set
			{
				metroTextButton_0 = value;
				Refresh();
			}
		}

		[DefaultValue(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool WithError
		{
			get
			{
				return bool_7;
			}
			set
			{
				bool_7 = value;
				Invalidate();
			}
		}

		public override ContextMenu ContextMenu
		{
			get
			{
				return class19_0.ContextMenu;
			}
			set
			{
				ContextMenu = value;
				class19_0.ContextMenu = value;
			}
		}

		public override ContextMenuStrip ContextMenuStrip
		{
			get
			{
				return class19_0.ContextMenuStrip;
			}
			set
			{
				ContextMenuStrip = value;
				class19_0.ContextMenuStrip = value;
			}
		}

		[DefaultValue(false)]
		public bool Multiline
		{
			get
			{
				return class19_0.Multiline;
			}
			set
			{
				class19_0.Multiline = value;
			}
		}

		public override string Text
		{
			get
			{
				return class19_0.Text;
			}
			set
			{
				class19_0.Text = value;
			}
		}

		[Category("Metro Appearance")]
		public Color WaterMarkColor
		{
			get
			{
				return class19_0.method_0();
			}
			set
			{
				class19_0.method_1(value);
			}
		}

		[Category("Metro Appearance")]
		public Font WaterMarkFont
		{
			get
			{
				return class19_0.method_2();
			}
			set
			{
				class19_0.method_3(value);
			}
		}

		public string[] Lines
		{
			get
			{
				return class19_0.Lines;
			}
			set
			{
				class19_0.Lines = value;
			}
		}

		[Browsable(false)]
		public string SelectedText
		{
			get
			{
				return class19_0.SelectedText;
			}
			set
			{
				class19_0.Text = value;
			}
		}

		[DefaultValue(false)]
		public bool ReadOnly
		{
			get
			{
				return class19_0.ReadOnly;
			}
			set
			{
				class19_0.ReadOnly = value;
			}
		}

		public char PasswordChar
		{
			get
			{
				return class19_0.PasswordChar;
			}
			set
			{
				class19_0.PasswordChar = value;
			}
		}

		[DefaultValue(false)]
		public bool UseSystemPasswordChar
		{
			get
			{
				return class19_0.UseSystemPasswordChar;
			}
			set
			{
				class19_0.UseSystemPasswordChar = value;
			}
		}

		[DefaultValue(HorizontalAlignment.Left)]
		public HorizontalAlignment TextAlign
		{
			get
			{
				return class19_0.TextAlign;
			}
			set
			{
				class19_0.TextAlign = value;
			}
		}

		public int SelectionStart
		{
			get
			{
				return class19_0.SelectionStart;
			}
			set
			{
				class19_0.SelectionStart = value;
			}
		}

		public int SelectionLength
		{
			get
			{
				return class19_0.SelectionLength;
			}
			set
			{
				class19_0.SelectionLength = value;
			}
		}

		[DefaultValue(true)]
		public new bool TabStop
		{
			get
			{
				return class19_0.TabStop;
			}
			set
			{
				class19_0.TabStop = value;
			}
		}

		public int MaxLength
		{
			get
			{
				return class19_0.MaxLength;
			}
			set
			{
				class19_0.MaxLength = value;
			}
		}

		public ScrollBars ScrollBars
		{
			get
			{
				return class19_0.ScrollBars;
			}
			set
			{
				class19_0.ScrollBars = value;
			}
		}

		[DefaultValue(System.Windows.Forms.AutoCompleteMode.None)]
		public AutoCompleteMode AutoCompleteMode
		{
			get
			{
				return class19_0.AutoCompleteMode;
			}
			set
			{
				class19_0.AutoCompleteMode = value;
			}
		}

		[DefaultValue(System.Windows.Forms.AutoCompleteSource.None)]
		public AutoCompleteSource AutoCompleteSource
		{
			get
			{
				return class19_0.AutoCompleteSource;
			}
			set
			{
				class19_0.AutoCompleteSource = value;
			}
		}

		public AutoCompleteStringCollection AutoCompleteCustomSource
		{
			get
			{
				return class19_0.AutoCompleteCustomSource;
			}
			set
			{
				class19_0.AutoCompleteCustomSource = value;
			}
		}

		public bool ShortcutsEnabled
		{
			get
			{
				return class19_0.ShortcutsEnabled;
			}
			set
			{
				class19_0.ShortcutsEnabled = value;
			}
		}

		[DefaultValue(System.Windows.Forms.CharacterCasing.Normal)]
		public CharacterCasing CharacterCasing
		{
			get
			{
				return class19_0.CharacterCasing;
			}
			set
			{
				class19_0.CharacterCasing = value;
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

		public event EventHandler AcceptsTabChanged
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

		public event ButClick ButtonClick
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				butClick_0 = (ButClick)Delegate.Combine(butClick_0, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				butClick_0 = (ButClick)Delegate.Remove(butClick_0, value);
			}
		}

		public event LUClear ClearClicked
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				luclear_0 = (LUClear)Delegate.Combine(luclear_0, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				luclear_0 = (LUClear)Delegate.Remove(luclear_0, value);
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

		public MetroTextBox()
		{
			SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, value: true);
			base.GotFocus += MetroTextBox_GotFocus;
			base.TabStop = false;
			method_1();
			method_3();
			method_2();
		}

		private void class19_0_AcceptsTabChanged(object sender, EventArgs e)
		{
			if (eventHandler_3 != null)
			{
				eventHandler_3(this, e);
			}
		}

		private void class19_0_SizeChanged(object sender, EventArgs e)
		{
			base.OnSizeChanged(e);
		}

		private void class19_0_CursorChanged(object sender, EventArgs e)
		{
			base.OnCursorChanged(e);
		}

		private void class19_0_ContextMenuStripChanged(object sender, EventArgs e)
		{
			base.OnContextMenuStripChanged(e);
		}

		private void class19_0_ContextMenuChanged(object sender, EventArgs e)
		{
			base.OnContextMenuChanged(e);
		}

		private void class19_0_ClientSizeChanged(object sender, EventArgs e)
		{
			base.OnClientSizeChanged(e);
		}

		private void class19_0_Click(object sender, EventArgs e)
		{
			base.OnClick(e);
		}

		private void class19_0_ChangeUICues(object sender, UICuesEventArgs e)
		{
			base.OnChangeUICues(e);
		}

		private void class19_0_CausesValidationChanged(object sender, EventArgs e)
		{
			base.OnCausesValidationChanged(e);
		}

		private void class19_0_KeyUp(object sender, KeyEventArgs e)
		{
			base.OnKeyUp(e);
		}

		private void class19_0_KeyPress(object sender, KeyPressEventArgs e)
		{
			base.OnKeyPress(e);
		}

		private void class19_0_KeyDown(object sender, KeyEventArgs e)
		{
			base.OnKeyDown(e);
		}

		private void class19_0_TextChanged(object sender, EventArgs e)
		{
			base.OnTextChanged(e);
			if (class19_0.Text != "" && !bool_9)
			{
				bool_9 = true;
				bool_8 = false;
				Invalidate();
			}
			if (class19_0.Text == "" && !bool_8)
			{
				bool_9 = false;
				bool_8 = true;
				Invalidate();
			}
		}

		public void Select(int start, int length)
		{
			class19_0.Select(start, length);
		}

		public void SelectAll()
		{
			class19_0.SelectAll();
		}

		public void Clear()
		{
			class19_0.Clear();
		}

		private void MetroTextBox_GotFocus(object sender, EventArgs e)
		{
			class19_0.Focus();
		}

		public void AppendText(string text)
		{
			class19_0.AppendText(text);
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			try
			{
				Color color = BackColor;
				class19_0.BackColor = color;
				if (!bool_0)
				{
					color = MetroPaint.BackColor.Form(Theme);
					class19_0.BackColor = color;
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
			if (!bool_1)
			{
				class19_0.ForeColor = MetroPaint.ForeColor.Button.Normal(Theme);
			}
			else
			{
				class19_0.ForeColor = ForeColor;
			}
			Color color = MetroPaint.BorderColor.ComboBox.Normal(Theme);
			if (bool_2)
			{
				color = MetroPaint.GetStyleColor(Style);
			}
			if (bool_7)
			{
				color = MetroColors.Red;
				if (Style == MetroColorStyle.Red)
				{
					color = MetroColors.Orange;
				}
			}
			using (Pen pen = new Pen(color))
			{
				e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, base.Width - 2, base.Height - 1));
			}
			method_0(e.Graphics);
		}

		private void method_0(Graphics graphics_0)
		{
			if (bool_4 && image_0 != null)
			{
				Point location = new Point(5, 5);
				if (bool_3)
				{
					location = new Point(base.ClientRectangle.Width - iconSize.Width - 1, 1);
				}
				graphics_0.DrawImage(image_0, new Rectangle(location, iconSize));
				method_3();
			}
			else
			{
				metroTextButton_0.Visible = bool_5;
				if (bool_5 && metroTextButton_0 != null)
				{
					method_3();
				}
			}
			OnCustomPaintForeground(new MetroPaintEventArgs(Color.Empty, class19_0.ForeColor, graphics_0));
		}

		public override void Refresh()
		{
			base.Refresh();
			method_3();
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			method_3();
		}

		private void method_1()
		{
			if (class19_0 != null)
			{
				return;
			}
			class19_0 = new Class19();
			class19_0.BorderStyle = BorderStyle.None;
			class19_0.Font = MetroFonts.TextBox(metroTextBoxSize_0, metroTextBoxWeight_0);
			class19_0.Location = new Point(3, 3);
			class19_0.Size = new Size(base.Width - 6, base.Height - 6);
			base.Size = new Size(class19_0.Width + 6, class19_0.Height + 6);
			class19_0.TabStop = true;
			base.Controls.Add(class19_0);
			if (metroTextButton_0 == null)
			{
				metroTextButton_0 = new MetroTextButton();
				metroTextButton_0.Theme = Theme;
				metroTextButton_0.Style = Style;
				metroTextButton_0.Location = new Point(3, 1);
				metroTextButton_0.Size = new Size(base.Height - 4, base.Height - 4);
				metroTextButton_0.TextChanged += metroTextButton_0_TextChanged;
				metroTextButton_0.MouseEnter += metroTextButton_0_MouseEnter;
				metroTextButton_0.MouseLeave += metroTextButton_0_MouseLeave;
				metroTextButton_0.Click += metroTextButton_0_Click;
				if (!base.Controls.Contains(metroTextButton_0))
				{
					base.Controls.Add(metroTextButton_0);
				}
				if (lnkClear == null)
				{
					method_4();
				}
			}
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
		}

		private void metroTextButton_0_Click(object sender, EventArgs e)
		{
			if (butClick_0 != null)
			{
				butClick_0(this, e);
			}
		}

		private void metroTextButton_0_MouseLeave(object sender, EventArgs e)
		{
			UseStyleColors = class19_0.Focused;
			Invalidate();
		}

		private void metroTextButton_0_MouseEnter(object sender, EventArgs e)
		{
			UseStyleColors = true;
			Invalidate();
		}

		private void metroTextButton_0_TextChanged(object sender, EventArgs e)
		{
			metroTextButton_0.Invalidate();
		}

		private void method_2()
		{
			class19_0.AcceptsTabChanged += class19_0_AcceptsTabChanged;
			class19_0.CausesValidationChanged += class19_0_CausesValidationChanged;
			class19_0.ChangeUICues += class19_0_ChangeUICues;
			class19_0.Click += class19_0_Click;
			class19_0.ClientSizeChanged += class19_0_ClientSizeChanged;
			class19_0.ContextMenuChanged += class19_0_ContextMenuChanged;
			class19_0.ContextMenuStripChanged += class19_0_ContextMenuStripChanged;
			class19_0.CursorChanged += class19_0_CursorChanged;
			class19_0.KeyDown += class19_0_KeyDown;
			class19_0.KeyPress += class19_0_KeyPress;
			class19_0.KeyUp += class19_0_KeyUp;
			class19_0.SizeChanged += class19_0_SizeChanged;
			class19_0.TextChanged += class19_0_TextChanged;
			class19_0.GotFocus += class19_0_GotFocus;
			class19_0.LostFocus += class19_0_LostFocus;
		}

		private void class19_0_LostFocus(object sender, EventArgs e)
		{
			UseStyleColors = false;
			Invalidate();
			InvokeLostFocus(this, e);
		}

		private void class19_0_GotFocus(object sender, EventArgs e)
		{
			bool_7 = false;
			UseStyleColors = true;
			Invalidate();
			InvokeGotFocus(this, e);
		}

		private void method_3()
		{
			if (metroTextButton_0 != null)
			{
				if (base.Height % 2 <= 0)
				{
					metroTextButton_0.Size = new Size(base.Height - 5, base.Height - 5);
					metroTextButton_0.Location = new Point(base.Width - metroTextButton_0.Width - 3, 2);
				}
				else
				{
					metroTextButton_0.Size = new Size(base.Height - 2, base.Height - 2);
					metroTextButton_0.Location = new Point(base.Width - (metroTextButton_0.Width + 1), 1);
				}
				metroTextButton_0.Visible = bool_5;
			}
			int num = 0;
			if (lnkClear != null)
			{
				lnkClear.Visible = false;
				if (bool_6 && Text != "" && !ReadOnly && base.Enabled)
				{
					num = 16;
					lnkClear.Location = new Point(base.Width - (ButtonWidth + 17), (base.Height - 14) / 2);
					lnkClear.Visible = true;
				}
			}
			if (class19_0 == null)
			{
				return;
			}
			class19_0.Font = MetroFonts.TextBox(metroTextBoxSize_0, metroTextBoxWeight_0);
			if (!bool_4)
			{
				class19_0.Location = new Point(3, 3);
				class19_0.Size = new Size(base.Width - (6 + ButtonWidth + num), base.Height - 6);
				return;
			}
			Point location = new Point(iconSize.Width + 10, 5);
			if (bool_3)
			{
				location = new Point(3, 3);
			}
			class19_0.Location = location;
			class19_0.Size = new Size(base.Width - (20 + ButtonWidth + num) - iconSize.Width, base.Height - 6);
		}

		private void method_4()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(Type.GetTypeFromHandle((RuntimeTypeHandle)typeof(MetroTextBox).TypeHandle));
			lnkClear = new MetroLink();
			SuspendLayout();
			lnkClear.FontSize = MetroLinkSize.Medium;
			lnkClear.FontWeight = MetroLinkWeight.Regular;
			lnkClear.Image = (Image)componentResourceManager.GetObject("lnkClear.Image");
			lnkClear.ImageSize = 10;
			lnkClear.Location = new Point(654, 96);
			lnkClear.Name = "lnkClear";
			lnkClear.NoFocusImage = (Image)componentResourceManager.GetObject("lnkClear.NoFocusImage");
			lnkClear.Size = new Size(12, 12);
			lnkClear.TabIndex = 2;
			lnkClear.UseSelectable = true;
			lnkClear.Click += lnkClear_Click;
			ResumeLayout(performLayout: false);
			base.Controls.Add(lnkClear);
		}

		private void lnkClear_Click(object sender, EventArgs e)
		{
			Focus();
			Clear();
			class19_0.Focus();
			if (luclear_0 != null)
			{
				luclear_0();
			}
		}

		internal static bool WP51tQC7qMwqun8ryav()
		{
			return oJc3FLCxWifZqivuDKe == null;
		}

		internal static void j6NU5ECLh2alxPNeL0s()
		{
		}
	}
}
