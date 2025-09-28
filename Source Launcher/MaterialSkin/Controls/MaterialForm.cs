using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
	public class MaterialForm : Form, IMaterialControl
	{
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
		public class MONITORINFOEX
		{
			public int cbSize = Marshal.SizeOf(typeof(MONITORINFOEX));

			public RECT rcMonitor = default(RECT);

			public RECT rcWork = default(RECT);

			public int dwFlags = 0;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
			public char[] szDevice = new char[32];

			internal static MONITORINFOEX n4sYUdICkXwsy6OdUQxX;

			internal static bool dHSaD4IC50Iku0Y6DSwn()
			{
				return n4sYUdICkXwsy6OdUQxX == null;
			}
		}

		public struct RECT
		{
			public int left;

			public int top;

			public int right;

			public int bottom;

			internal static object mwbkPuICWkhucBHOBSXm;

			public int Width()
			{
				return right - left;
			}

			public int Height()
			{
				return bottom - top;
			}

			internal static bool Ke9jGlIC3cMBCJZJ7Pn0()
			{
				return mwbkPuICWkhucBHOBSXm == null;
			}
		}

		private enum Enum6
		{

		}

		private enum Enum7
		{

		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private int int_0;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private MouseState mouseState_0;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private bool bool_0;

		public const int WM_NCLBUTTONDOWN = 161;

		public const int HT_CAPTION = 2;

		public const int WM_MOUSEMOVE = 512;

		public const int WM_LBUTTONDOWN = 513;

		public const int WM_LBUTTONUP = 514;

		public const int WM_LBUTTONDBLCLK = 515;

		public const int WM_RBUTTONDOWN = 516;

		private Enum6 enum6_0;

		private Enum7 enum7_0 = (Enum7)6;

		private readonly Dictionary<int, int> dictionary_0 = new Dictionary<int, int>
		{
			{ 12, 3 },
			{ 13, 4 },
			{ 14, 5 },
			{ 10, 1 },
			{ 11, 2 },
			{ 15, 6 },
			{ 16, 7 },
			{ 17, 8 }
		};

		private readonly Cursor[] cursor_0 = new Cursor[5]
		{
			Cursors.SizeNESW,
			Cursors.SizeWE,
			Cursors.SizeNWSE,
			Cursors.SizeWE,
			Cursors.SizeNS
		};

		private Rectangle rectangle_0;

		private Rectangle rectangle_1;

		private Rectangle rectangle_2;

		private Rectangle rectangle_3;

		private Rectangle rectangle_4;

		private bool bool_1;

		private Size size_0;

		private Point point_0;

		private bool bool_2;

		private static MaterialForm m7CiRxIVCZ3mVE45qXit;

		[Browsable(false)]
		public int Depth
		{
			[CompilerGenerated]
			get
			{
				return int_0;
			}
			[CompilerGenerated]
			set
			{
				int_0 = value;
			}
		}

		[Browsable(false)]
		public MaterialSkinManager SkinManager => MaterialSkinManager.Instance;

		[Browsable(false)]
		public MouseState MouseState
		{
			[CompilerGenerated]
			get
			{
				return mouseState_0;
			}
			[CompilerGenerated]
			set
			{
				mouseState_0 = value;
			}
		}

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

		public bool Sizable
		{
			[CompilerGenerated]
			get
			{
				return bool_0;
			}
			[CompilerGenerated]
			set
			{
				bool_0 = value;
			}
		}

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.Style = createParams.Style | 0x20000 | 0x80000;
				return createParams;
			}
		}

		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

		[DllImport("user32.dll")]
		public static extern bool ReleaseCapture();

		[DllImport("user32.dll")]
		public static extern int TrackPopupMenuEx(IntPtr hmenu, uint fuFlags, int x, int y, IntPtr hwnd, IntPtr lptpm);

		[DllImport("user32.dll")]
		public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

		[DllImport("user32.dll")]
		public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool GetMonitorInfo(HandleRef hmonitor, [In][Out] MONITORINFOEX info);

		public MaterialForm()
		{
			FormBorderStyle = FormBorderStyle.None;
			Sizable = true;
			DoubleBuffered = true;
			SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, value: true);
			Application.AddMessageFilter(new MouseMessageFilter());
			MouseMessageFilter.MouseMove += OnGlobalMouseMove;
		}

		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);
			if (base.DesignMode || base.IsDisposed)
			{
				return;
			}
			if (m.Msg != 515)
			{
				if (m.Msg == 512 && bool_1 && (rectangle_4.Contains(PointToClient(Cursor.Position)) || rectangle_3.Contains(PointToClient(Cursor.Position))) && !rectangle_0.Contains(PointToClient(Cursor.Position)) && !rectangle_1.Contains(PointToClient(Cursor.Position)) && !rectangle_2.Contains(PointToClient(Cursor.Position)))
				{
					if (bool_2)
					{
						bool_1 = false;
						bool_2 = false;
						Point point = PointToClient(Cursor.Position);
						if (point.X >= base.Width / 2)
						{
							base.Location = ((base.Width - point.X < size_0.Width / 2) ? new Point(Cursor.Position.X - size_0.Width + base.Width - point.X, Cursor.Position.Y - point.Y) : new Point(Cursor.Position.X - size_0.Width / 2, Cursor.Position.Y - point.Y));
						}
						else
						{
							base.Location = ((point.X >= size_0.Width / 2) ? new Point(Cursor.Position.X - size_0.Width / 2, Cursor.Position.Y - point.Y) : new Point(Cursor.Position.X - point.X, Cursor.Position.Y - point.Y));
						}
						base.Size = size_0;
						ReleaseCapture();
						SendMessage(base.Handle, 161, 2, 0);
					}
				}
				else if (m.Msg == 513 && (rectangle_4.Contains(PointToClient(Cursor.Position)) || rectangle_3.Contains(PointToClient(Cursor.Position))) && !rectangle_0.Contains(PointToClient(Cursor.Position)) && !rectangle_1.Contains(PointToClient(Cursor.Position)) && !rectangle_2.Contains(PointToClient(Cursor.Position)))
				{
					if (bool_1)
					{
						bool_2 = true;
						return;
					}
					ReleaseCapture();
					SendMessage(base.Handle, 161, 2, 0);
				}
				else if (m.Msg != 516)
				{
					if (m.Msg != 161)
					{
						if (m.Msg == 514)
						{
							bool_2 = false;
						}
					}
					else if (Sizable)
					{
						byte b = 0;
						if (dictionary_0.ContainsKey((int)m.WParam))
						{
							b = (byte)dictionary_0[(int)m.WParam];
						}
						if (b != 0)
						{
							SendMessage(base.Handle, 274, 0xF000 | b, (int)m.LParam);
						}
					}
				}
				else
				{
					Point pt = PointToClient(Cursor.Position);
					if (rectangle_4.Contains(pt) && !rectangle_0.Contains(pt) && !rectangle_1.Contains(pt) && !rectangle_2.Contains(pt))
					{
						int wParam = TrackPopupMenuEx(GetSystemMenu(base.Handle, bRevert: false), 256u, Cursor.Position.X, Cursor.Position.Y, base.Handle, IntPtr.Zero);
						SendMessage(base.Handle, 274, wParam, 0);
					}
				}
			}
			else
			{
				method_1(!bool_1);
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (!base.DesignMode)
			{
				method_0(e);
				if (e.Button == MouseButtons.Left && !bool_1)
				{
					method_2(enum6_0);
				}
				base.OnMouseDown(e);
			}
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			if (!base.DesignMode)
			{
				enum7_0 = (Enum7)6;
				Invalidate();
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (base.DesignMode)
			{
				return;
			}
			if (Sizable)
			{
				bool flag = GetChildAtPoint(e.Location) != null;
				if (e.Location.X < 7 && e.Location.Y > base.Height - 7 && !flag && !bool_1)
				{
					enum6_0 = (Enum6)0;
					Cursor = Cursors.SizeNESW;
				}
				else if (e.Location.X < 7 && !flag && !bool_1)
				{
					enum6_0 = (Enum6)1;
					Cursor = Cursors.SizeWE;
				}
				else if (e.Location.X <= base.Width - 7 || e.Location.Y <= base.Height - 7 || flag || bool_1)
				{
					if (e.Location.X > base.Width - 7 && !flag && !bool_1)
					{
						enum6_0 = (Enum6)2;
						Cursor = Cursors.SizeWE;
					}
					else if (e.Location.Y > base.Height - 7 && !flag && !bool_1)
					{
						enum6_0 = (Enum6)4;
						Cursor = Cursors.SizeNS;
					}
					else
					{
						enum6_0 = (Enum6)5;
						if (cursor_0.Contains(Cursor))
						{
							Cursor = Cursors.Default;
						}
					}
				}
				else
				{
					enum6_0 = (Enum6)3;
					Cursor = Cursors.SizeNWSE;
				}
			}
			method_0(e);
		}

		protected void OnGlobalMouseMove(object sender, MouseEventArgs e)
		{
			if (!base.IsDisposed)
			{
				Point point = PointToClient(e.Location);
				MouseEventArgs e2 = new MouseEventArgs(MouseButtons.None, 0, point.X, point.Y, 0);
				OnMouseMove(e2);
			}
		}

		private void method_0(MouseEventArgs mouseEventArgs_0, bool bool_3 = false)
		{
			if (base.DesignMode)
			{
				return;
			}
			Enum7 @enum = enum7_0;
			bool flag = base.MinimizeBox && base.ControlBox;
			bool flag2 = base.MaximizeBox && base.ControlBox;
			if (mouseEventArgs_0.Button != MouseButtons.Left || bool_3)
			{
				if (!flag || flag2 || !rectangle_1.Contains(mouseEventArgs_0.Location))
				{
					if (!(flag && flag2) || !rectangle_0.Contains(mouseEventArgs_0.Location))
					{
						if (base.MaximizeBox && base.ControlBox && rectangle_1.Contains(mouseEventArgs_0.Location))
						{
							enum7_0 = (Enum7)1;
							if (@enum == (Enum7)4 && bool_3)
							{
								method_1(!bool_1);
							}
						}
						else if (base.ControlBox && rectangle_2.Contains(mouseEventArgs_0.Location))
						{
							enum7_0 = (Enum7)0;
							if (@enum == (Enum7)3 && bool_3)
							{
								Close();
							}
						}
						else
						{
							enum7_0 = (Enum7)6;
						}
					}
					else
					{
						enum7_0 = (Enum7)2;
						if (@enum == (Enum7)5 && bool_3)
						{
							base.WindowState = FormWindowState.Minimized;
						}
					}
				}
				else
				{
					enum7_0 = (Enum7)2;
					if (@enum == (Enum7)5 && bool_3)
					{
						base.WindowState = FormWindowState.Minimized;
					}
				}
			}
			else if (!flag || flag2 || !rectangle_1.Contains(mouseEventArgs_0.Location))
			{
				if (flag && flag2 && rectangle_0.Contains(mouseEventArgs_0.Location))
				{
					enum7_0 = (Enum7)5;
				}
				else if (!flag2 || !rectangle_1.Contains(mouseEventArgs_0.Location))
				{
					if (!base.ControlBox || !rectangle_2.Contains(mouseEventArgs_0.Location))
					{
						enum7_0 = (Enum7)6;
					}
					else
					{
						enum7_0 = (Enum7)3;
					}
				}
				else
				{
					enum7_0 = (Enum7)4;
				}
			}
			else
			{
				enum7_0 = (Enum7)5;
			}
			if (@enum != enum7_0)
			{
				Invalidate();
			}
		}

		private void method_1(bool bool_3)
		{
			if (base.MaximizeBox && base.ControlBox)
			{
				bool_1 = bool_3;
				if (bool_3)
				{
					IntPtr handle = MonitorFromWindow(base.Handle, 2u);
					MONITORINFOEX mONITORINFOEX = new MONITORINFOEX();
					GetMonitorInfo(new HandleRef(null, handle), mONITORINFOEX);
					size_0 = base.Size;
					point_0 = base.Location;
					base.Size = new Size(mONITORINFOEX.rcWork.Width(), mONITORINFOEX.rcWork.Height());
					base.Location = new Point(mONITORINFOEX.rcWork.left, mONITORINFOEX.rcWork.top);
				}
				else
				{
					base.Size = size_0;
					base.Location = point_0;
				}
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (!base.DesignMode)
			{
				method_0(e, bool_3: true);
				base.OnMouseUp(e);
				ReleaseCapture();
			}
		}

		private void method_2(Enum6 enum6_1)
		{
			if (!base.DesignMode)
			{
				int num = -1;
				switch (enum6_1)
				{
				case (Enum6)0:
					num = 16;
					break;
				case (Enum6)1:
					num = 10;
					break;
				case (Enum6)2:
					num = 11;
					break;
				case (Enum6)3:
					num = 17;
					break;
				case (Enum6)4:
					num = 15;
					break;
				}
				ReleaseCapture();
				if (num != -1)
				{
					SendMessage(base.Handle, 161, num, 0);
				}
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			rectangle_0 = new Rectangle(base.Width - SkinManager.FORM_PADDING / 2 - 72, 0, 24, 24);
			rectangle_1 = new Rectangle(base.Width - SkinManager.FORM_PADDING / 2 - 48, 0, 24, 24);
			rectangle_2 = new Rectangle(base.Width - SkinManager.FORM_PADDING / 2 - 24, 0, 24, 24);
			rectangle_4 = new Rectangle(0, 0, base.Width, 24);
			rectangle_3 = new Rectangle(0, 24, base.Width, 40);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics graphics = e.Graphics;
			graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
			graphics.Clear(SkinManager.GetApplicationBackgroundColor());
			graphics.FillRectangle(SkinManager.ColorScheme.DarkPrimaryBrush, rectangle_4);
			graphics.FillRectangle(SkinManager.ColorScheme.PrimaryBrush, rectangle_3);
			using (Pen pen = new Pen(SkinManager.GetDividersColor(), 1f))
			{
				graphics.DrawLine(pen, new Point(0, rectangle_3.Bottom), new Point(0, base.Height - 2));
				graphics.DrawLine(pen, new Point(base.Width - 1, rectangle_3.Bottom), new Point(base.Width - 1, base.Height - 2));
				graphics.DrawLine(pen, new Point(0, base.Height - 1), new Point(base.Width - 1, base.Height - 1));
			}
			bool flag = base.MinimizeBox && base.ControlBox;
			bool flag2 = base.MaximizeBox && base.ControlBox;
			Brush flatButtonHoverBackgroundBrush = SkinManager.GetFlatButtonHoverBackgroundBrush();
			Brush flatButtonPressedBackgroundBrush = SkinManager.GetFlatButtonPressedBackgroundBrush();
			if (enum7_0 == (Enum7)2 && flag)
			{
				graphics.FillRectangle(flatButtonHoverBackgroundBrush, (!flag2) ? rectangle_1 : rectangle_0);
			}
			if (enum7_0 == (Enum7)5 && flag)
			{
				graphics.FillRectangle(flatButtonPressedBackgroundBrush, (!flag2) ? rectangle_1 : rectangle_0);
			}
			if (enum7_0 == (Enum7)1 && flag2)
			{
				graphics.FillRectangle(flatButtonHoverBackgroundBrush, rectangle_1);
			}
			if (enum7_0 == (Enum7)4 && flag2)
			{
				graphics.FillRectangle(flatButtonPressedBackgroundBrush, rectangle_1);
			}
			if (enum7_0 == (Enum7)0 && base.ControlBox)
			{
				graphics.FillRectangle(flatButtonHoverBackgroundBrush, rectangle_2);
			}
			if (enum7_0 == (Enum7)3 && base.ControlBox)
			{
				graphics.FillRectangle(flatButtonPressedBackgroundBrush, rectangle_2);
			}
			using (Pen pen2 = new Pen(SkinManager.ACTION_BAR_TEXT_SECONDARY, 2f))
			{
				if (flag)
				{
					int num = ((!flag2) ? rectangle_1.X : rectangle_0.X);
					int num2 = ((!flag2) ? rectangle_1.Y : rectangle_0.Y);
					graphics.DrawLine(pen2, num + (int)((double)rectangle_0.Width * 0.33), num2 + (int)((double)rectangle_0.Height * 0.66), num + (int)((double)rectangle_0.Width * 0.66), num2 + (int)((double)rectangle_0.Height * 0.66));
				}
				if (flag2)
				{
					graphics.DrawRectangle(pen2, rectangle_1.X + (int)((double)rectangle_1.Width * 0.33), rectangle_1.Y + (int)((double)rectangle_1.Height * 0.36), (int)((double)rectangle_1.Width * 0.39), (int)((double)rectangle_1.Height * 0.31));
				}
				if (base.ControlBox)
				{
					graphics.DrawLine(pen2, rectangle_2.X + (int)((double)rectangle_2.Width * 0.33), rectangle_2.Y + (int)((double)rectangle_2.Height * 0.33), rectangle_2.X + (int)((double)rectangle_2.Width * 0.66), rectangle_2.Y + (int)((double)rectangle_2.Height * 0.66));
					graphics.DrawLine(pen2, rectangle_2.X + (int)((double)rectangle_2.Width * 0.66), rectangle_2.Y + (int)((double)rectangle_2.Height * 0.33), rectangle_2.X + (int)((double)rectangle_2.Width * 0.33), rectangle_2.Y + (int)((double)rectangle_2.Height * 0.66));
				}
			}
			graphics.DrawString(Text, SkinManager.ROBOTO_MEDIUM_12, SkinManager.ColorScheme.TextBrush, new Rectangle(SkinManager.FORM_PADDING, 24, base.Width, 40), new StringFormat
			{
				LineAlignment = StringAlignment.Center
			});
		}

		internal static bool rKJlJcIVYDPRVXtPaVxa()
		{
			return m7CiRxIVCZ3mVE45qXit == null;
		}

		internal static void P4ga5cIVHwbL1DbW5Vh9()
		{
		}
	}
}
