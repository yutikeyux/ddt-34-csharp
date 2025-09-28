using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Security;
using System.Windows.Forms;
using MetroFramework.Components;
using MetroFramework.Drawing;
using MetroFramework.Interfaces;
using MetroFramework.Native;

namespace MetroFramework.Forms
{
	public class MetroForm : Form, IDisposable, IMetroForm
	{
		private enum Enum4
		{

		}

		private class Class20 : Button, IMetroControl
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

			internal static object lMx6JPDyDL6LbREYoYG;

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
			[DefaultValue(false)]
			[Category("Metro Behaviour")]
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

			protected virtual void OnCustomPaintBackground(MetroPaintEventArgs metroPaintEventArgs_0)
			{
				if (GetStyle(ControlStyles.UserPaint) && eventHandler_0 != null)
				{
					eventHandler_0(this, metroPaintEventArgs_0);
				}
			}

			protected virtual void OnCustomPaint(MetroPaintEventArgs metroPaintEventArgs_0)
			{
				if (GetStyle(ControlStyles.UserPaint) && eventHandler_1 != null)
				{
					eventHandler_1(this, metroPaintEventArgs_0);
				}
			}

			protected virtual void OnCustomPaintForeground(MetroPaintEventArgs metroPaintEventArgs_0)
			{
				if (GetStyle(ControlStyles.UserPaint) && eventHandler_2 != null)
				{
					eventHandler_2(this, metroPaintEventArgs_0);
				}
			}

			public Class20()
			{
				SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, value: true);
			}

			protected override void OnPaint(PaintEventArgs pevent)
			{
				MetroThemeStyle theme = Theme;
				Color color;
				if (base.Parent == null)
				{
					color = MetroPaint.BackColor.Form(theme);
				}
				else if (!(base.Parent is IMetroForm))
				{
					color = ((base.Parent is IMetroControl) ? MetroPaint.GetStyleColor(Style) : base.Parent.BackColor);
				}
				else
				{
					theme = ((IMetroForm)base.Parent).Theme;
					color = MetroPaint.BackColor.Form(theme);
				}
				Color foreColor;
				if (bool_3 && !bool_4 && base.Enabled)
				{
					foreColor = MetroPaint.ForeColor.Button.Normal(theme);
					color = MetroPaint.BackColor.Button.Normal(theme);
				}
				else if (bool_3 && bool_4 && base.Enabled)
				{
					foreColor = MetroPaint.ForeColor.Button.Press(theme);
					color = MetroPaint.GetStyleColor(Style);
				}
				else if (!base.Enabled)
				{
					foreColor = MetroPaint.ForeColor.Button.Disabled(theme);
					color = MetroPaint.BackColor.Button.Disabled(theme);
				}
				else
				{
					foreColor = MetroPaint.ForeColor.Button.Normal(theme);
				}
				pevent.Graphics.Clear(color);
				Font font = new Font("Webdings", 9.25f);
				TextRenderer.DrawText(pevent.Graphics, Text, font, base.ClientRectangle, foreColor, color, TextFormatFlags.EndEllipsis | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
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

			protected override void OnMouseUp(MouseEventArgs mevent)
			{
				bool_4 = false;
				Invalidate();
				base.OnMouseUp(mevent);
			}

			protected override void OnMouseLeave(EventArgs e)
			{
				bool_3 = false;
				Invalidate();
				base.OnMouseLeave(e);
			}

			internal static bool zbqK0kDRfOnJgTdKkPD()
			{
				return lMx6JPDyDL6LbREYoYG == null;
			}

			internal static void DRnVEbDjqcLZo3NapGO()
			{
			}
		}

		protected abstract class MetroShadowBase : Form
		{
			protected const int WS_EX_TRANSPARENT = 32;

			protected const int WS_EX_LAYERED = 524288;

			protected const int WS_EX_NOACTIVATE = 134217728;

			private readonly int int_0;

			private readonly int int_1;

			private bool bool_0;

			private long long_0;

			[CompilerGenerated]
			private Form form_0;

			internal static MetroShadowBase c2q1rfhbHfOR9QPxvrt;

			protected Form TargetForm
			{
				[CompilerGenerated]
				get
				{
					return form_0;
				}
				[CompilerGenerated]
				private set
				{
					form_0 = value;
				}
			}

			protected override CreateParams CreateParams
			{
				get
				{
					CreateParams createParams = base.CreateParams;
					createParams.ExStyle |= int_1;
					return createParams;
				}
			}

			protected MetroShadowBase(Form targetForm, int shadowSize, int wsExStyle)
			{
				TargetForm = targetForm;
				int_0 = shadowSize;
				int_1 = wsExStyle;
				TargetForm.Activated += method_1;
				TargetForm.ResizeBegin += method_4;
				TargetForm.ResizeEnd += method_8;
				TargetForm.VisibleChanged += method_2;
				TargetForm.SizeChanged += method_7;
				TargetForm.Move += method_5;
				TargetForm.Resize += method_6;
				if (TargetForm.Owner != null)
				{
					base.Owner = TargetForm.Owner;
				}
				TargetForm.Owner = this;
				base.MaximizeBox = false;
				base.MinimizeBox = false;
				base.ShowInTaskbar = false;
				base.ShowIcon = false;
				base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
				base.Bounds = method_0();
			}

			private Rectangle method_0()
			{
				Rectangle bounds = TargetForm.Bounds;
				bounds.Inflate(int_0, int_0);
				return bounds;
			}

			protected abstract void PaintShadow();

			protected abstract void ClearShadow();

			protected override void OnDeactivate(EventArgs e)
			{
				base.OnDeactivate(e);
				bool_0 = true;
			}

			private void method_1(object sender, EventArgs e)
			{
				if (base.Visible)
				{
					Update();
				}
				if (!bool_0)
				{
					BringToFront();
					return;
				}
				base.Visible = true;
				bool_0 = false;
			}

			private void method_2(object sender, EventArgs e)
			{
				base.Visible = TargetForm.Visible && TargetForm.WindowState != FormWindowState.Minimized;
				Update();
			}

			[SpecialName]
			private bool method_3()
			{
				return long_0 > 0L;
			}

			private void method_4(object sender, EventArgs e)
			{
				long_0 = DateTime.Now.Ticks;
			}

			private void method_5(object sender, EventArgs e)
			{
				if (TargetForm.Visible && TargetForm.WindowState == FormWindowState.Normal)
				{
					base.Bounds = method_0();
				}
				else
				{
					base.Visible = false;
				}
			}

			private void method_6(object sender, EventArgs e)
			{
				ClearShadow();
			}

			private void method_7(object sender, EventArgs e)
			{
				base.Bounds = method_0();
				if (!method_3())
				{
					method_9();
				}
			}

			private void method_8(object sender, EventArgs e)
			{
				long_0 = 0L;
				method_9();
			}

			private void method_9()
			{
				if (TargetForm.Visible && TargetForm.WindowState != FormWindowState.Minimized)
				{
					PaintShadow();
				}
			}

			internal static bool TysRbNhBnc0Ekp95YJy()
			{
				return c2q1rfhbHfOR9QPxvrt == null;
			}

			internal static void EZc0sDhUbgeIUcJkVhi()
			{
			}
		}

		protected class MetroAeroDropShadow : MetroShadowBase
		{
			private static MetroAeroDropShadow pis7vKhmuv49EDneV1Q;

			public MetroAeroDropShadow(Form targetForm)
				: base(targetForm, 0, 134217760)
			{
				base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			}

			protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
			{
				if (specified != BoundsSpecified.Size)
				{
					base.SetBoundsCore(x, y, width, height, specified);
				}
			}

			protected override void PaintShadow()
			{
				base.Visible = true;
			}

			protected override void ClearShadow()
			{
			}

			internal static void SRt4DRhLojWqLJrPGL5()
			{
			}

			internal static bool IYt2D1hgIljkhy1HMKx()
			{
				return pis7vKhmuv49EDneV1Q == null;
			}
		}

		protected class MetroFlatDropShadow : MetroShadowBase
		{
			private Point point_0 = new Point(-6, -6);

			internal static MetroFlatDropShadow PDIw1Qhs1OdqJRvErZR;

			public MetroFlatDropShadow(Form targetForm)
				: base(targetForm, 6, 134742048)
			{
			}

			protected override void OnLoad(EventArgs e)
			{
				base.OnLoad(e);
				PaintShadow();
			}

			protected override void OnPaint(PaintEventArgs e)
			{
				base.Visible = true;
				PaintShadow();
			}

			protected override void PaintShadow()
			{
				using Bitmap bitmap_ = method_11();
				method_10(bitmap_, byte.MaxValue);
			}

			protected override void ClearShadow()
			{
				Bitmap bitmap = new Bitmap(base.Width, base.Height, PixelFormat.Format32bppArgb);
				Graphics graphics = Graphics.FromImage(bitmap);
				graphics.Clear(Color.Transparent);
				graphics.Flush();
				graphics.Dispose();
				method_10(bitmap, byte.MaxValue);
				bitmap.Dispose();
			}

			[SecuritySafeCritical]
			private void method_10(Bitmap bitmap_0, byte byte_0)
			{
				if (bitmap_0.PixelFormat != PixelFormat.Format32bppArgb)
				{
					throw new ApplicationException("The bitmap must be 32ppp with alpha-channel.");
				}
				IntPtr dC = WinApi.GetDC(IntPtr.Zero);
				IntPtr intPtr = WinApi.CreateCompatibleDC(dC);
				IntPtr intPtr2 = IntPtr.Zero;
				IntPtr hObject = IntPtr.Zero;
				try
				{
					intPtr2 = bitmap_0.GetHbitmap(Color.FromArgb(0));
					hObject = WinApi.SelectObject(intPtr, intPtr2);
					WinApi.SIZE psize = new WinApi.SIZE(bitmap_0.Width, bitmap_0.Height);
					WinApi.POINT pprSrc = new WinApi.POINT(0, 0);
					WinApi.POINT pptDst = new WinApi.POINT(base.Left, base.Top);
					WinApi.BLENDFUNCTION pblend = default(WinApi.BLENDFUNCTION);
					pblend.BlendOp = 0;
					pblend.BlendFlags = 0;
					pblend.SourceConstantAlpha = byte_0;
					pblend.AlphaFormat = 1;
					WinApi.UpdateLayeredWindow(base.Handle, dC, ref pptDst, ref psize, intPtr, ref pprSrc, 0, ref pblend, 2);
				}
				finally
				{
					WinApi.ReleaseDC(IntPtr.Zero, dC);
					if (intPtr2 != IntPtr.Zero)
					{
						WinApi.SelectObject(intPtr, hObject);
						WinApi.DeleteObject(intPtr2);
					}
					WinApi.DeleteDC(intPtr);
				}
			}

			private Bitmap method_11()
			{
				return (Bitmap)method_12(Color.Black, new Rectangle(0, 0, base.ClientRectangle.Width, base.ClientRectangle.Height));
			}

			private Image method_12(Color color_0, Rectangle rectangle_0)
			{
				Rectangle rect = rectangle_0;
				Rectangle rect2 = new Rectangle(rectangle_0.X + (-point_0.X - 1), rectangle_0.Y + (-point_0.Y - 1), rectangle_0.Width - (-point_0.X * 2 - 1), rectangle_0.Height - (-point_0.Y * 2 - 1));
				Bitmap bitmap = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
				Graphics graphics = Graphics.FromImage(bitmap);
				graphics.SmoothingMode = SmoothingMode.AntiAlias;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				using (Brush brush = new SolidBrush(Color.FromArgb(30, Color.Black)))
				{
					graphics.FillRectangle(brush, rect);
				}
				using (Brush brush2 = new SolidBrush(Color.FromArgb(60, Color.Black)))
				{
					graphics.FillRectangle(brush2, rect2);
				}
				graphics.Flush();
				graphics.Dispose();
				return bitmap;
			}

			internal static void EtGrSRharTcruOxHawn()
			{
			}

			internal static bool MUdwL2hNCtripE6D49U()
			{
				return PDIw1Qhs1OdqJRvErZR == null;
			}
		}

		protected class MetroRealisticDropShadow : MetroShadowBase
		{
			private static MetroRealisticDropShadow exNoTghoFNbwgGqAJCk;

			public MetroRealisticDropShadow(Form targetForm)
				: base(targetForm, 15, 134742048)
			{
			}

			protected override void OnLoad(EventArgs e)
			{
				base.OnLoad(e);
				PaintShadow();
			}

			protected override void OnPaint(PaintEventArgs e)
			{
				base.Visible = true;
				PaintShadow();
			}

			protected override void PaintShadow()
			{
				using Bitmap bitmap_ = method_11();
				method_10(bitmap_, byte.MaxValue);
			}

			protected override void ClearShadow()
			{
				Bitmap bitmap = new Bitmap(base.Width, base.Height, PixelFormat.Format32bppArgb);
				Graphics graphics = Graphics.FromImage(bitmap);
				graphics.Clear(Color.Transparent);
				graphics.Flush();
				graphics.Dispose();
				method_10(bitmap, byte.MaxValue);
				bitmap.Dispose();
			}

			[SecuritySafeCritical]
			private void method_10(Bitmap bitmap_0, byte byte_0)
			{
				if (bitmap_0.PixelFormat != PixelFormat.Format32bppArgb)
				{
					throw new ApplicationException("The bitmap must be 32ppp with alpha-channel.");
				}
				IntPtr dC = WinApi.GetDC(IntPtr.Zero);
				IntPtr intPtr = WinApi.CreateCompatibleDC(dC);
				IntPtr intPtr2 = IntPtr.Zero;
				IntPtr hObject = IntPtr.Zero;
				try
				{
					intPtr2 = bitmap_0.GetHbitmap(Color.FromArgb(0));
					hObject = WinApi.SelectObject(intPtr, intPtr2);
					WinApi.SIZE psize = new WinApi.SIZE(bitmap_0.Width, bitmap_0.Height);
					WinApi.POINT pprSrc = new WinApi.POINT(0, 0);
					WinApi.POINT pptDst = new WinApi.POINT(base.Left, base.Top);
					WinApi.BLENDFUNCTION bLENDFUNCTION = default(WinApi.BLENDFUNCTION);
					bLENDFUNCTION.BlendOp = 0;
					bLENDFUNCTION.BlendFlags = 0;
					bLENDFUNCTION.SourceConstantAlpha = byte_0;
					bLENDFUNCTION.AlphaFormat = 1;
					WinApi.BLENDFUNCTION pblend = bLENDFUNCTION;
					WinApi.UpdateLayeredWindow(base.Handle, dC, ref pptDst, ref psize, intPtr, ref pprSrc, 0, ref pblend, 2);
				}
				finally
				{
					WinApi.ReleaseDC(IntPtr.Zero, dC);
					if (intPtr2 != IntPtr.Zero)
					{
						WinApi.SelectObject(intPtr, hObject);
						WinApi.DeleteObject(intPtr2);
					}
					WinApi.DeleteDC(intPtr);
				}
			}

			private Bitmap method_11()
			{
				return (Bitmap)method_12(0, 0, 40, 1, Color.Black, new Rectangle(1, 1, base.ClientRectangle.Width, base.ClientRectangle.Height));
			}

			private Image method_12(int int_2, int int_3, int int_4, int int_5, Color color_0, Rectangle rectangle_0)
			{
				Rectangle rectangle = rectangle_0;
				Rectangle rectangle2 = rectangle_0;
				rectangle2.Offset(int_2, int_3);
				rectangle2.Inflate(-int_4, -int_4);
				rectangle.Inflate(int_5, int_5);
				rectangle.Offset(int_2, int_3);
				Rectangle rectangle3 = rectangle;
				Bitmap bitmap = new Bitmap(rectangle3.Width, rectangle3.Height, PixelFormat.Format32bppArgb);
				Graphics graphics = Graphics.FromImage(bitmap);
				graphics.SmoothingMode = SmoothingMode.AntiAlias;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				int int_6 = 0;
				do
				{
					double num = (double)(rectangle.Height - rectangle2.Height) / (double)(int_4 * 2 + int_5 * 2);
					Color color_ = Color.FromArgb((int)(200.0 * (num * num)), color_0);
					Rectangle rectangle_ = rectangle2;
					rectangle_.Offset(-rectangle3.Left, -rectangle3.Top);
					method_13(graphics, rectangle_, int_6, Pens.Transparent, color_);
					rectangle2.Inflate(1, 1);
					int_6 = (int)((double)int_4 * (1.0 - num * num));
				}
				while (rectangle.Contains(rectangle2));
				graphics.Flush();
				graphics.Dispose();
				return bitmap;
			}

			private void method_13(Graphics graphics_0, Rectangle rectangle_0, int int_2, Pen pen_0, Color color_0)
			{
				int num = Convert.ToInt32(Math.Ceiling(pen_0.Width));
				rectangle_0 = Rectangle.Inflate(rectangle_0, -num, -num);
				GraphicsPath graphicsPath = new GraphicsPath();
				if (int_2 > 0)
				{
					graphicsPath.AddArc(rectangle_0.X, rectangle_0.Y, int_2, int_2, 180f, 90f);
					graphicsPath.AddArc(rectangle_0.X + rectangle_0.Width - int_2, rectangle_0.Y, int_2, int_2, 270f, 90f);
					graphicsPath.AddArc(rectangle_0.X + rectangle_0.Width - int_2, rectangle_0.Y + rectangle_0.Height - int_2, int_2, int_2, 0f, 90f);
					graphicsPath.AddArc(rectangle_0.X, rectangle_0.Y + rectangle_0.Height - int_2, int_2, int_2, 90f, 90f);
				}
				else
				{
					graphicsPath.AddRectangle(rectangle_0);
				}
				graphicsPath.CloseAllFigures();
				if (int_2 > 5)
				{
					using SolidBrush brush = new SolidBrush(color_0);
					graphics_0.FillPath(brush, graphicsPath);
				}
				if (pen_0 != Pens.Transparent)
				{
					using Pen pen = new Pen(pen_0.Color);
					pen.StartCap = LineCap.Round;
					pen.EndCap = LineCap.Round;
					graphics_0.DrawPath(pen, graphicsPath);
				}
			}

			internal static void VZKRrehC8XbZBpkQ4qa()
			{
			}

			internal static bool tfCO19hVgdOJse1yNNG()
			{
				return exNoTghoFNbwgGqAJCk == null;
			}
		}

		private MetroColorStyle metroColorStyle_0 = MetroColorStyle.Blue;

		private MetroThemeStyle metroThemeStyle_0 = MetroThemeStyle.Light;

		private MetroStyleManager metroStyleManager_0;

		private MetroFormTextAlign metroFormTextAlign_0;

		private MetroFormBorderStyle metroFormBorderStyle_0;

		private bool bool_0 = true;

		private bool bool_1 = true;

		private bool bool_2 = true;

		private MetroFormShadowType metroFormShadowType_0 = MetroFormShadowType.Flat;

		private Bitmap bitmap_0;

		private Image image_0;

		private Padding padding_0;

		private int int_0;

		private BackLocation backLocation_0;

		private bool bool_3;

		private Dictionary<Enum4, Class20> dictionary_0;

		private Form form_0;

		internal static MetroForm Rl1T1GDTJghogGYkLCv;

		[Category("Metro Appearance")]
		public MetroColorStyle Style
		{
			get
			{
				if (StyleManager != null)
				{
					return StyleManager.Style;
				}
				return metroColorStyle_0;
			}
			set
			{
				metroColorStyle_0 = value;
			}
		}

		[Category("Metro Appearance")]
		public MetroThemeStyle Theme
		{
			get
			{
				if (StyleManager == null)
				{
					return metroThemeStyle_0;
				}
				return StyleManager.Theme;
			}
			set
			{
				metroThemeStyle_0 = value;
			}
		}

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

		[Browsable(true)]
		[Category("Metro Appearance")]
		public MetroFormTextAlign TextAlign
		{
			get
			{
				return metroFormTextAlign_0;
			}
			set
			{
				metroFormTextAlign_0 = value;
			}
		}

		[Browsable(false)]
		public override Color BackColor => MetroPaint.BackColor.Form(Theme);

		[Browsable(true)]
		[DefaultValue(MetroFormBorderStyle.None)]
		[Category("Metro Appearance")]
		public MetroFormBorderStyle BorderStyle
		{
			get
			{
				return metroFormBorderStyle_0;
			}
			set
			{
				metroFormBorderStyle_0 = value;
			}
		}

		[Category("Metro Appearance")]
		public bool Movable
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

		public new Padding Padding
		{
			get
			{
				return base.Padding;
			}
			set
			{
				value.Top = Math.Max(value.Top, (!DisplayHeader) ? 30 : 60);
				base.Padding = value;
			}
		}

		protected override Padding DefaultPadding => new Padding(20, (!DisplayHeader) ? 20 : 60, 20, 20);

		[DefaultValue(true)]
		[Category("Metro Appearance")]
		public bool DisplayHeader
		{
			get
			{
				return bool_1;
			}
			set
			{
				if (value != bool_1)
				{
					Padding padding = base.Padding;
					padding.Top += ((!value) ? (-30) : 30);
					base.Padding = padding;
				}
				bool_1 = value;
			}
		}

		[Category("Metro Appearance")]
		public bool Resizable
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

		[Category("Metro Appearance")]
		[DefaultValue(MetroFormShadowType.Flat)]
		public MetroFormShadowType ShadowType
		{
			get
			{
				if (base.IsMdiChild)
				{
					return MetroFormShadowType.None;
				}
				return metroFormShadowType_0;
			}
			set
			{
				metroFormShadowType_0 = value;
			}
		}

		[Browsable(false)]
		public new FormBorderStyle FormBorderStyle
		{
			get
			{
				return base.FormBorderStyle;
			}
			set
			{
				base.FormBorderStyle = value;
			}
		}

		public new Form MdiParent
		{
			get
			{
				return base.MdiParent;
			}
			set
			{
				if (value != null)
				{
					method_8();
					metroFormShadowType_0 = MetroFormShadowType.None;
				}
				base.MdiParent = value;
			}
		}

		[DefaultValue(null)]
		[Category("Metro Appearance")]
		public Image BackImage
		{
			get
			{
				return image_0;
			}
			set
			{
				image_0 = value;
				if (value != null)
				{
					bitmap_0 = ApplyInvert(new Bitmap(value));
				}
				Refresh();
			}
		}

		[Category("Metro Appearance")]
		public Padding BackImagePadding
		{
			get
			{
				return padding_0;
			}
			set
			{
				padding_0 = value;
				Refresh();
			}
		}

		[Category("Metro Appearance")]
		public int BackMaxSize
		{
			get
			{
				return int_0;
			}
			set
			{
				int_0 = value;
				Refresh();
			}
		}

		[Category("Metro Appearance")]
		[DefaultValue(BackLocation.TopLeft)]
		public BackLocation BackLocation
		{
			get
			{
				return backLocation_0;
			}
			set
			{
				backLocation_0 = value;
				Refresh();
			}
		}

		[DefaultValue(true)]
		[Category("Metro Appearance")]
		public bool ApplyImageInvert
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

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.Style |= 131072;
				if (ShadowType == MetroFormShadowType.SystemShadow)
				{
					createParams.ClassStyle |= 131072;
				}
				return createParams;
			}
		}

		public MetroForm()
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, value: true);
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.Name = "MetroForm";
			base.StartPosition = FormStartPosition.CenterScreen;
			base.TransparencyKey = Color.Lavender;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				method_8();
			}
			base.Dispose(disposing);
		}

		public Bitmap ApplyInvert(Bitmap bitmapImage)
		{
			for (int i = 0; i < bitmapImage.Height; i++)
			{
				for (int j = 0; j < bitmapImage.Width; j++)
				{
					Color pixel = bitmapImage.GetPixel(j, i);
					_ = pixel.A;
					byte b = (byte)(255 - pixel.R);
					byte b2 = (byte)(255 - pixel.G);
					byte b3 = (byte)(255 - pixel.B);
					if (b <= 0)
					{
						b = 17;
					}
					if (b2 <= 0)
					{
						b2 = 17;
					}
					if (b3 <= 0)
					{
						b3 = 17;
					}
					bitmapImage.SetPixel(j, i, Color.FromArgb(b, b2, b3));
				}
			}
			return bitmapImage;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Color color = MetroPaint.BackColor.Form(Theme);
			Color foreColor = MetroPaint.ForeColor.Title(Theme);
			e.Graphics.Clear(color);
			using (SolidBrush brush = MetroPaint.GetStyleBrush(Style))
			{
				Rectangle rect = new Rectangle(0, 0, base.Width, 5);
				e.Graphics.FillRectangle(brush, rect);
			}
			if (BorderStyle != 0)
			{
				Color color2 = MetroPaint.BorderColor.Form(Theme);
				using Pen pen = new Pen(color2);
				e.Graphics.DrawLines(pen, new Point[4]
				{
					new Point(0, 5),
					new Point(0, base.Height - 1),
					new Point(base.Width - 1, base.Height - 1),
					new Point(base.Width - 1, 5)
				});
			}
			if (image_0 != null && int_0 != 0)
			{
				Image image = MetroImage.ResizeImage(image_0, new Rectangle(0, 0, int_0, int_0));
				if (bool_3)
				{
					image = MetroImage.ResizeImage((Theme == MetroThemeStyle.Dark) ? bitmap_0 : image_0, new Rectangle(0, 0, int_0, int_0));
				}
				switch (backLocation_0)
				{
				case BackLocation.TopLeft:
					e.Graphics.DrawImage(image, padding_0.Left, padding_0.Top);
					break;
				case BackLocation.TopRight:
					e.Graphics.DrawImage(image, base.ClientRectangle.Right - (padding_0.Right + image.Width), padding_0.Top);
					break;
				case BackLocation.BottomLeft:
					e.Graphics.DrawImage(image, padding_0.Left, base.ClientRectangle.Bottom - (image.Height + padding_0.Bottom));
					break;
				case BackLocation.BottomRight:
					e.Graphics.DrawImage(image, base.ClientRectangle.Right - (padding_0.Right + image.Width), base.ClientRectangle.Bottom - (image.Height + padding_0.Bottom));
					break;
				}
			}
			if (bool_1)
			{
				Rectangle bounds = new Rectangle(20, 20, base.ClientRectangle.Width - 40, 40);
				TextFormatFlags flags = TextFormatFlags.EndEllipsis | method_0();
				TextRenderer.DrawText(e.Graphics, Text, MetroFonts.Title, bounds, foreColor, flags);
			}
			if (Resizable && (base.SizeGripStyle == System.Windows.Forms.SizeGripStyle.Auto || base.SizeGripStyle == System.Windows.Forms.SizeGripStyle.Show))
			{
				using SolidBrush brush2 = new SolidBrush(MetroPaint.ForeColor.Button.Disabled(Theme));
				Size size = new Size(2, 2);
				e.Graphics.FillRectangles(brush2, new Rectangle[6]
				{
					new Rectangle(new Point(base.ClientRectangle.Width - 6, base.ClientRectangle.Height - 6), size),
					new Rectangle(new Point(base.ClientRectangle.Width - 10, base.ClientRectangle.Height - 10), size),
					new Rectangle(new Point(base.ClientRectangle.Width - 10, base.ClientRectangle.Height - 6), size),
					new Rectangle(new Point(base.ClientRectangle.Width - 6, base.ClientRectangle.Height - 10), size),
					new Rectangle(new Point(base.ClientRectangle.Width - 14, base.ClientRectangle.Height - 6), size),
					new Rectangle(new Point(base.ClientRectangle.Width - 6, base.ClientRectangle.Height - 14), size)
				});
			}
		}

		private TextFormatFlags method_0()
		{
			return TextAlign switch
			{
				MetroFormTextAlign.Left => TextFormatFlags.Default, 
				MetroFormTextAlign.Center => TextFormatFlags.HorizontalCenter, 
				MetroFormTextAlign.Right => TextFormatFlags.Right, 
				_ => throw new InvalidOperationException(), 
			};
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			if (!(this is MetroTaskWindow))
			{
				MetroTaskWindow.ForceClose();
			}
			base.OnClosing(e);
		}

		protected override void OnClosed(EventArgs e)
		{
			if (base.Owner != null)
			{
				base.Owner = null;
			}
			method_8();
			base.OnClosed(e);
		}

		[SecuritySafeCritical]
		public bool FocusMe()
		{
			return WinApi.SetForegroundWindow(base.Handle);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if (base.DesignMode)
			{
				return;
			}
			switch (base.StartPosition)
			{
			case FormStartPosition.CenterParent:
				CenterToParent();
				break;
			case FormStartPosition.CenterScreen:
				if (!base.IsMdiChild)
				{
					CenterToScreen();
				}
				else
				{
					CenterToParent();
				}
				break;
			}
			RemoveCloseButton();
			if (base.ControlBox)
			{
				method_4((Enum4)2);
				if (base.MaximizeBox)
				{
					method_4((Enum4)1);
				}
				if (base.MinimizeBox)
				{
					method_4((Enum4)0);
				}
				method_6();
			}
			method_7();
		}

		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);
			if (metroFormShadowType_0 == MetroFormShadowType.AeroShadow && smethod_0() && smethod_1())
			{
				int attrValue = 2;
				DwmApi.DwmSetWindowAttribute(base.Handle, 2, ref attrValue, 4);
				DwmApi.MARGINS mARGINS = default(DwmApi.MARGINS);
				mARGINS.cyBottomHeight = 1;
				mARGINS.cxLeftWidth = 0;
				mARGINS.cxRightWidth = 0;
				mARGINS.cyTopHeight = 0;
				DwmApi.MARGINS marInset = mARGINS;
				DwmApi.DwmExtendFrameIntoClientArea(base.Handle, ref marInset);
			}
		}

		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			Invalidate();
		}

		protected override void OnResizeEnd(EventArgs e)
		{
			base.OnResizeEnd(e);
			method_6();
		}

		protected override void WndProc(ref Message m)
		{
			if (base.DesignMode)
			{
				base.WndProc(ref m);
				return;
			}
			switch (m.Msg)
			{
			case 132:
			{
				WinApi.HitTest hitTest = method_2(m.HWnd, m.WParam, m.LParam);
				if (hitTest != WinApi.HitTest.HTCLIENT)
				{
					m.Result = (IntPtr)(long)hitTest;
					break;
				}
				goto default;
			}
			case 163:
			case 515:
				if (!base.MaximizeBox)
				{
					break;
				}
				goto default;
			case 274:
				switch (((IntPtr)(nint)m.WParam).ToInt32() & 0xFFF0)
				{
				case 61456:
					if (!Movable)
					{
						return;
					}
					break;
				}
				goto default;
			default:
				base.WndProc(ref m);
				switch (m.Msg)
				{
				case 36:
					method_1(m.HWnd, m.LParam);
					break;
				case 5:
				{
					if (dictionary_0 == null)
					{
						break;
					}
					dictionary_0.TryGetValue((Enum4)1, out var value);
					if (value == null)
					{
						break;
					}
					if (base.WindowState == FormWindowState.Normal)
					{
						if (form_0 != null)
						{
							form_0.Visible = true;
						}
						value.Text = "1";
					}
					if (base.WindowState == FormWindowState.Maximized)
					{
						value.Text = "2";
					}
					break;
				}
				}
				break;
			}
		}

		[SecuritySafeCritical]
		private unsafe void method_1(IntPtr intptr_0, IntPtr intptr_1)
		{
			WinApi.MINMAXINFO* ptr = (WinApi.MINMAXINFO*)(void*)intptr_1;
			Screen screen = Screen.FromHandle(intptr_0);
			ptr->ptMaxSize.x = screen.WorkingArea.Width;
			ptr->ptMaxSize.y = screen.WorkingArea.Height;
			ptr->ptMaxPosition.x = Math.Abs(screen.WorkingArea.Left - screen.Bounds.Left);
			ptr->ptMaxPosition.y = Math.Abs(screen.WorkingArea.Top - screen.Bounds.Top);
		}

		private WinApi.HitTest method_2(IntPtr intptr_0, IntPtr intptr_1, IntPtr intptr_2)
		{
			Point pt = new Point((short)(int)intptr_2, (short)((int)intptr_2 >> 16));
			int num = Math.Max(Padding.Right, Padding.Bottom);
			if (Resizable && RectangleToScreen(new Rectangle(base.ClientRectangle.Width - num, base.ClientRectangle.Height - num, num, num)).Contains(pt))
			{
				return WinApi.HitTest.HTBOTTOMRIGHT;
			}
			if (!RectangleToScreen(new Rectangle(5, 5, base.ClientRectangle.Width - 10, 50)).Contains(pt))
			{
				return WinApi.HitTest.HTCLIENT;
			}
			return WinApi.HitTest.HTCAPTION;
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Button == System.Windows.Forms.MouseButtons.Left && Movable && base.WindowState != FormWindowState.Maximized && base.Width - 5 > e.Location.X && e.Location.X > 5 && e.Location.Y > 5)
			{
				method_3();
			}
		}

		[SecuritySafeCritical]
		private void method_3()
		{
			WinApi.ReleaseCapture();
			WinApi.SendMessage(base.Handle, 161, 2, 0);
		}

		[SecuritySafeCritical]
		private static bool smethod_0()
		{
			if (Environment.OSVersion.Version.Major > 5)
			{
				DwmApi.DwmIsCompositionEnabled(out var pfEnabled);
				return pfEnabled;
			}
			return false;
		}

		private static bool smethod_1()
		{
			if (Environment.OSVersion.Version.Major <= 5)
			{
				return false;
			}
			return SystemInformation.IsDropShadowEnabled;
		}

		private void method_4(Enum4 enum4_0)
		{
			if (dictionary_0 == null)
			{
				dictionary_0 = new Dictionary<Enum4, Class20>();
			}
			if (dictionary_0.ContainsKey(enum4_0))
			{
				return;
			}
			Class20 @class = new Class20();
			switch (enum4_0)
			{
			case (Enum4)2:
				@class.Text = "r";
				break;
			case (Enum4)0:
				@class.Text = "0";
				break;
			case (Enum4)1:
				if (base.WindowState == FormWindowState.Normal)
				{
					@class.Text = "1";
				}
				else
				{
					@class.Text = "2";
				}
				break;
			}
			@class.Style = Style;
			@class.Theme = Theme;
			@class.Tag = enum4_0;
			@class.Size = new Size(25, 20);
			@class.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			@class.TabStop = false;
			@class.Click += method_5;
			base.Controls.Add(@class);
			dictionary_0.Add(enum4_0, @class);
		}

		private void method_5(object sender, EventArgs e)
		{
			Class20 @class = sender as Class20;
			if (@class == null)
			{
				return;
			}
			switch ((Enum4)@class.Tag)
			{
			case (Enum4)2:
				Close();
				break;
			case (Enum4)1:
				if (base.WindowState != 0)
				{
					base.WindowState = FormWindowState.Normal;
					@class.Text = "1";
				}
				else
				{
					base.WindowState = FormWindowState.Maximized;
					@class.Text = "2";
				}
				break;
			case (Enum4)0:
				base.WindowState = FormWindowState.Minimized;
				break;
			}
		}

		private void method_6()
		{
			if (!base.ControlBox)
			{
				return;
			}
			Dictionary<int, Enum4> dictionary = new Dictionary<int, Enum4>(3);
			dictionary.Add(0, (Enum4)2);
			dictionary.Add(1, (Enum4)1);
			dictionary.Add(2, (Enum4)0);
			Dictionary<int, Enum4> dictionary2 = dictionary;
			Point location = new Point(base.ClientRectangle.Width - 5 - 25, 5);
			int num = location.X - 25;
			Class20 @class = null;
			if (dictionary_0.Count == 1)
			{
				foreach (KeyValuePair<Enum4, Class20> item in dictionary_0)
				{
					item.Value.Location = location;
				}
			}
			else
			{
				foreach (KeyValuePair<int, Enum4> item2 in dictionary2)
				{
					bool flag = dictionary_0.ContainsKey(item2.Value);
					if (@class != null || !flag)
					{
						if (@class != null && flag)
						{
							dictionary_0[item2.Value].Location = new Point(num, 5);
							num -= 25;
						}
					}
					else
					{
						@class = dictionary_0[item2.Value];
						@class.Location = location;
					}
				}
			}
			Refresh();
		}

		private void method_7()
		{
			switch (ShadowType)
			{
			case MetroFormShadowType.Flat:
				form_0 = new MetroFlatDropShadow(this);
				break;
			case MetroFormShadowType.DropShadow:
				form_0 = new MetroRealisticDropShadow(this);
				break;
			case MetroFormShadowType.None:
				break;
			}
		}

		private void method_8()
		{
			if (form_0 != null && !form_0.IsDisposed)
			{
				form_0.Visible = false;
				base.Owner = form_0.Owner;
				form_0.Owner = null;
				form_0.Dispose();
				form_0 = null;
			}
		}

		[SecuritySafeCritical]
		public void RemoveCloseButton()
		{
			IntPtr systemMenu = WinApi.GetSystemMenu(base.Handle, bRevert: false);
			if (!(systemMenu == IntPtr.Zero))
			{
				int menuItemCount = WinApi.GetMenuItemCount(systemMenu);
				if (menuItemCount > 0)
				{
					WinApi.RemoveMenu(systemMenu, (uint)(menuItemCount - 1), 5120u);
					WinApi.RemoveMenu(systemMenu, (uint)(menuItemCount - 2), 5120u);
					WinApi.DrawMenuBar(base.Handle);
				}
			}
		}

		private Rectangle MeasureText(Graphics g, Rectangle clientRectangle, Font font, string text, TextFormatFlags flags)
		{
			Size proposedSize = new Size(int.MaxValue, int.MinValue);
			Size size = TextRenderer.MeasureText(g, text, font, proposedSize, flags);
			return new Rectangle(clientRectangle.X, clientRectangle.Y, size.Width, size.Height);
		}

		internal static bool bggjqpDJ1isqMBevo0F()
		{
			return Rl1T1GDTJghogGYkLCv == null;
		}

		internal static void pxLmI2D9VguwjfELvf5()
		{
		}
	}
}
