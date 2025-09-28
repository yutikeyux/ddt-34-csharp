using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
	public class MaterialListView : ListView, IMaterialControl
	{
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class LogFont
		{
			public int lfHeight = 0;

			public int lfWidth = 0;

			public int lfEscapement = 0;

			public int lfOrientation = 0;

			public int lfWeight = 0;

			public byte lfItalic = 0;

			public byte lfUnderline = 0;

			public byte lfStrikeOut = 0;

			public byte lfCharSet = 0;

			public byte lfOutPrecision = 0;

			public byte lfClipPrecision = 0;

			public byte lfQuality = 0;

			public byte lfPitchAndFamily = 0;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string lfFaceName = string.Empty;

			private static LogFont zlnV7FICQfo3PMFp60O5;

			internal static void R5912IIYIvQ69cr9C1Rw()
			{
			}

			internal static bool zjoPx7ICzHD604JWUoV2()
			{
				return zlnV7FICQfo3PMFp60O5 == null;
			}
		}

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private int int_0;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private MouseState mouseState_0;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private Point point_0;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private ListViewItem listViewItem_0;

		internal static MaterialListView PgwghTIViLplL8P8phXF;

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

		[Browsable(false)]
		public Point MouseLocation
		{
			[CompilerGenerated]
			get
			{
				return point_0;
			}
			[CompilerGenerated]
			set
			{
				point_0 = value;
			}
		}

		[Browsable(false)]
		private ListViewItem ListViewItem_0
		{
			[CompilerGenerated]
			get
			{
				return listViewItem_0;
			}
			[CompilerGenerated]
			set
			{
				listViewItem_0 = value;
			}
		}

		public MaterialListView()
		{
			base.GridLines = false;
			base.FullRowSelect = true;
			base.HeaderStyle = ColumnHeaderStyle.Nonclickable;
			base.View = View.Details;
			base.OwnerDraw = true;
			base.ResizeRedraw = true;
			base.BorderStyle = BorderStyle.None;
			SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, value: true);
			MouseLocation = new Point(-1, -1);
			MouseState = MouseState.OUT;
			base.MouseEnter += delegate
			{
				MouseState = MouseState.HOVER;
			};
			base.MouseLeave += delegate
			{
				MouseState = MouseState.OUT;
				MouseLocation = new Point(-1, -1);
				ListViewItem_0 = null;
				Invalidate();
			};
			base.MouseDown += delegate
			{
				MouseState = MouseState.DOWN;
			};
			base.MouseUp += delegate
			{
				MouseState = MouseState.HOVER;
			};
			base.MouseMove += delegate(object sender, MouseEventArgs e)
			{
				MouseLocation = e.Location;
				ListViewItem itemAt = GetItemAt(MouseLocation.X, MouseLocation.Y);
				if (ListViewItem_0 != itemAt)
				{
					ListViewItem_0 = itemAt;
					Invalidate();
				}
			};
		}

		protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
		{
			e.Graphics.FillRectangle(new SolidBrush(SkinManager.GetApplicationBackgroundColor()), new Rectangle(e.Bounds.X, e.Bounds.Y, base.Width, e.Bounds.Height));
			e.Graphics.DrawString(e.Header.Text, SkinManager.ROBOTO_MEDIUM_10, SkinManager.GetSecondaryTextBrush(), new Rectangle(e.Bounds.X + 12, e.Bounds.Y + 12, e.Bounds.Width - 24, e.Bounds.Height - 24), method_0());
		}

		protected override void OnDrawItem(DrawListViewItemEventArgs e)
		{
			Bitmap bitmap = new Bitmap(e.Item.Bounds.Width, e.Item.Bounds.Height);
			Graphics graphics = Graphics.FromImage(bitmap);
			graphics.FillRectangle(new SolidBrush(SkinManager.GetApplicationBackgroundColor()), new Rectangle(new Point(e.Bounds.X, 0), e.Bounds.Size));
			if (!e.State.HasFlag(ListViewItemStates.Selected))
			{
				if (e.Bounds.Contains(MouseLocation) && MouseState == MouseState.HOVER)
				{
					graphics.FillRectangle(SkinManager.GetFlatButtonHoverBackgroundBrush(), new Rectangle(new Point(e.Bounds.X, 0), e.Bounds.Size));
				}
			}
			else
			{
				graphics.FillRectangle(SkinManager.GetFlatButtonPressedBackgroundBrush(), new Rectangle(new Point(e.Bounds.X, 0), e.Bounds.Size));
			}
			graphics.DrawLine(new Pen(SkinManager.GetDividersColor()), e.Bounds.Left, 0, e.Bounds.Right, 0);
			foreach (ListViewItem.ListViewSubItem subItem in e.Item.SubItems)
			{
				graphics.DrawString(subItem.Text, SkinManager.ROBOTO_MEDIUM_10, SkinManager.GetPrimaryTextBrush(), new Rectangle(subItem.Bounds.X + 12, 12, subItem.Bounds.Width - 24, subItem.Bounds.Height - 24), method_0());
			}
			e.Graphics.DrawImage((Image)bitmap.Clone(), new Point(0, e.Item.Bounds.Location.Y));
			graphics.Dispose();
			bitmap.Dispose();
		}

		private StringFormat method_0()
		{
			return new StringFormat
			{
				FormatFlags = StringFormatFlags.LineLimit,
				Trimming = StringTrimming.EllipsisCharacter,
				Alignment = StringAlignment.Near,
				LineAlignment = StringAlignment.Center
			};
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			Font font = new Font(SkinManager.ROBOTO_MEDIUM_12.FontFamily, 24f);
			LogFont logFont = new LogFont();
			font.ToLogFont(logFont);
			try
			{
				Font = Font.FromLogFont(logFont);
			}
			catch (ArgumentException)
			{
				Font = new Font(FontFamily.GenericSansSerif, 24f);
			}
		}

		[CompilerGenerated]
		private void MaterialListView_MouseEnter(object sender, EventArgs e)
		{
			MouseState = MouseState.HOVER;
		}

		[CompilerGenerated]
		private void MaterialListView_MouseLeave(object sender, EventArgs e)
		{
			MouseState = MouseState.OUT;
			MouseLocation = new Point(-1, -1);
			ListViewItem_0 = null;
			Invalidate();
		}

		[CompilerGenerated]
		private void MaterialListView_MouseDown(object sender, MouseEventArgs e)
		{
			MouseState = MouseState.DOWN;
		}

		[CompilerGenerated]
		private void MaterialListView_MouseUp(object sender, MouseEventArgs e)
		{
			MouseState = MouseState.HOVER;
		}

		[CompilerGenerated]
		private void MaterialListView_MouseMove(object sender, MouseEventArgs e)
		{
			MouseLocation = e.Location;
			ListViewItem itemAt = GetItemAt(MouseLocation.X, MouseLocation.Y);
			if (ListViewItem_0 != itemAt)
			{
				ListViewItem_0 = itemAt;
				Invalidate();
			}
		}

		internal static bool KPckk8IVwBUxnoCBINfR()
		{
			return PgwghTIViLplL8P8phXF == null;
		}

		internal static void lyPY7XIVkq9iYhph1afp()
		{
		}
	}
}
