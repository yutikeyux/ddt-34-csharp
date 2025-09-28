using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MetroFramework.Components;
using MetroFramework.Drawing;
using MetroFramework.Interfaces;

namespace MetroFramework.Controls
{
	public class MetroListView : ListView, IMetroControl
	{
		private struct Struct6
		{
			public uint uint_0;

			public uint uint_1;

			public int int_0;

			public int int_1;

			public uint uint_2;

			public int int_2;

			public int int_3;
		}

		private enum Enum0
		{

		}

		private enum Enum1
		{

		}

		private enum Enum2
		{

		}

		private enum Enum3
		{

		}

		private struct Struct7
		{
			public uint uint_0;

			public int int_0;

			public int int_1;

			public uint uint_1;

			public uint uint_2;

			public IntPtr intptr_0;

			public int int_2;

			public int int_3;

			public IntPtr intptr_1;
		}

		public enum ScrollBarCommands
		{
			SB_LINEUP = 0,
			SB_LINELEFT = 0,
			SB_LINEDOWN = 1,
			SB_LINERIGHT = 1,
			SB_PAGEUP = 2,
			SB_PAGELEFT = 2,
			SB_PAGEDOWN = 3,
			SB_PAGERIGHT = 3,
			SB_THUMBPOSITION = 4,
			SB_THUMBTRACK = 5,
			SB_TOP = 6,
			SB_LEFT = 6,
			SB_BOTTOM = 7,
			SB_RIGHT = 7,
			SB_ENDSCROLL = 8
		}

		public delegate void ScrollPositionChangedDelegate(MetroListView listview, int pos);

		private ListViewColumnSorter listViewColumnSorter_0;

		private Font font_0 = new Font("Segoe UI", 11f, FontStyle.Regular, GraphicsUnit.Pixel);

		private float float_0 = 0.2f;

		private EventHandler<MetroPaintEventArgs> eventHandler_0;

		private EventHandler<MetroPaintEventArgs> eventHandler_1;

		private EventHandler<MetroPaintEventArgs> eventHandler_2;

		private MetroColorStyle metroColorStyle_0;

		private MetroThemeStyle cUepetxCw4;

		private MetroStyleManager metroStyleManager_0;

		private bool bool_0;

		private bool CdwpzseJu5;

		private bool bool_1;

		private ScrollPositionChangedDelegate scrollPositionChangedDelegate_0;

		private Action<MetroListView> action_0;

		private Action<MetroListView> action_1;

		private int int_0;

		private MetroScrollBar metroScrollBar_0 = new MetroScrollBar();

		private bool bool_2;

		internal static MetroListView w1QQ3RM5URW5KVaDkN0;

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

		[Category("Metro Appearance")]
		[DefaultValue(MetroThemeStyle.Default)]
		public MetroThemeStyle Theme
		{
			get
			{
				if (!base.DesignMode && cUepetxCw4 == MetroThemeStyle.Default)
				{
					if (StyleManager != null && cUepetxCw4 == MetroThemeStyle.Default)
					{
						return StyleManager.Theme;
					}
					if (StyleManager != null || cUepetxCw4 != 0)
					{
						return cUepetxCw4;
					}
					return MetroThemeStyle.Light;
				}
				return cUepetxCw4;
			}
			set
			{
				cUepetxCw4 = value;
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
				return CdwpzseJu5;
			}
			set
			{
				CdwpzseJu5 = value;
			}
		}

		[Category("Metro Appearance")]
		[DefaultValue(false)]
		public bool UseStyleColors
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

		[Category("Metro Behaviour")]
		[DefaultValue(false)]
		public bool AllowSorting
		{
			get
			{
				return bool_2;
			}
			set
			{
				bool_2 = value;
				if (!value)
				{
					listViewColumnSorter_0 = null;
					base.ListViewItemSorter = null;
				}
				else
				{
					listViewColumnSorter_0 = new ListViewColumnSorter();
					base.ListViewItemSorter = listViewColumnSorter_0;
				}
			}
		}

		[Description("Set the font of the button caption")]
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

		public event ScrollPositionChangedDelegate ScrollPositionChanged
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				scrollPositionChangedDelegate_0 = (ScrollPositionChangedDelegate)Delegate.Combine(scrollPositionChangedDelegate_0, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				scrollPositionChangedDelegate_0 = (ScrollPositionChangedDelegate)Delegate.Remove(scrollPositionChangedDelegate_0, value);
			}
		}

		public event Action<MetroListView> ItemAdded
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				action_0 = (Action<MetroListView>)Delegate.Combine(action_0, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				action_0 = (Action<MetroListView>)Delegate.Remove(action_0, value);
			}
		}

		public event Action<MetroListView> ItemsRemoved
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				action_1 = (Action<MetroListView>)Delegate.Combine(action_1, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				action_1 = (Action<MetroListView>)Delegate.Remove(action_1, value);
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

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetScrollInfo(IntPtr intptr_0, int int_1, ref Struct6 struct6_0);

		private void method_0()
		{
			int_0++;
		}

		private void method_1()
		{
			if (int_0 > 0)
			{
				int_0--;
			}
		}

		private void method_2(object object_0, int int_1)
		{
			if (int_0 <= 0)
			{
				SetScrollPosition(metroScrollBar_0.Value);
			}
		}

		public void GetScrollPosition(out int min, out int max, out int pos, out int smallchange, out int largechange)
		{
			Struct6 struct6_ = default(Struct6);
			struct6_.uint_0 = (uint)Marshal.SizeOf(Type.GetTypeFromHandle((RuntimeTypeHandle)typeof(Struct6).TypeHandle));
			struct6_.uint_1 = 23u;
			if (GetScrollInfo(base.Handle, 1, ref struct6_))
			{
				min = struct6_.int_0;
				max = struct6_.int_1;
				pos = struct6_.int_2 + 1;
				smallchange = 1;
				largechange = (int)struct6_.uint_2;
			}
			else
			{
				min = 0;
				max = 0;
				pos = 0;
				smallchange = 0;
				largechange = 0;
			}
		}

		public void UpdateScrollbar()
		{
			if (metroScrollBar_0 != null)
			{
				GetScrollPosition(out var min, out var max, out var pos, out var smallchange, out var largechange);
				method_0();
				metroScrollBar_0.Value = pos;
				metroScrollBar_0.Maximum = max - largechange + 1;
				metroScrollBar_0.Minimum = min;
				metroScrollBar_0.SmallChange = smallchange;
				metroScrollBar_0.LargeChange = largechange;
				metroScrollBar_0.Visible = metroScrollBar_0.Maximum != 101;
				method_1();
			}
		}

		public void SetScrollPosition(int pos)
		{
			pos = Math.Min(base.Items.Count - 1, pos);
			if (pos < 0 || pos >= base.Items.Count)
			{
				return;
			}
			SuspendLayout();
			EnsureVisible(pos);
			for (int i = 0; i < 10; i++)
			{
				if (base.TopItem != null && base.TopItem.Index != pos)
				{
					base.TopItem = base.Items[pos];
				}
			}
			ResumeLayout();
		}

		protected void OnItemAdded()
		{
			if (int_0 <= 0)
			{
				UpdateScrollbar();
				if (action_0 != null)
				{
					action_0(this);
				}
			}
		}

		protected void OnItemsRemoved()
		{
			if (int_0 <= 0)
			{
				UpdateScrollbar();
				if (action_1 != null)
				{
					action_1(this);
				}
			}
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			if (metroScrollBar_0 != null)
			{
				metroScrollBar_0.Value -= 3 * Math.Sign(e.Delta);
			}
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 277L)
			{
				GetScrollPosition(out var _, out var _, out var pos, out var _, out var _);
				if (scrollPositionChangedDelegate_0 != null)
				{
					scrollPositionChangedDelegate_0(this, pos);
				}
				if (metroScrollBar_0 != null)
				{
					metroScrollBar_0.Value = pos;
				}
			}
			else if (m.Msg == 131L)
			{
				int windowLong = GetWindowLong(base.Handle, -16);
				if ((windowLong & 0x200000) == 2097152)
				{
					SetWindowLong(base.Handle, -16, windowLong & -2097153);
				}
			}
			else if (m.Msg != 4103L && m.Msg != 4173L)
			{
				if (m.Msg == 4104L || m.Msg == 4105L)
				{
					OnItemsRemoved();
				}
			}
			else
			{
				OnItemAdded();
			}
			base.WndProc(ref m);
		}

		public static int GetWindowLong(IntPtr hWnd, int nIndex)
		{
			if (IntPtr.Size == 4)
			{
				return (int)GetWindowLong_1(hWnd, nIndex);
			}
			return (int)(long)GetWindowLongPtr(hWnd, nIndex);
		}

		public static int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong)
		{
			if (IntPtr.Size == 4)
			{
				return (int)SetWindowLong_1(hWnd, nIndex, dwNewLong);
			}
			return (int)(long)SetWindowLongPtr(hWnd, nIndex, dwNewLong);
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "GetWindowLong")]
		public static extern IntPtr GetWindowLong_1(IntPtr hWnd, int nIndex);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

		[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SetWindowLong")]
		public static extern IntPtr SetWindowLong_1(IntPtr hWnd, int nIndex, int dwNewLong);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, int dwNewLong);

		public MetroListView()
		{
			Font = new Font("Segoe UI", 12f);
			base.HideSelection = true;
			base.OwnerDraw = true;
			base.DrawColumnHeader += MetroListView_DrawColumnHeader;
			base.DrawItem += MetroListView_DrawItem;
			base.DrawSubItem += MetroListView_DrawSubItem;
			base.Resize += MetroListView_Resize;
			base.ColumnClick += MetroListView_ColumnClick;
			base.SelectedIndexChanged += MetroListView_SelectedIndexChanged;
			base.FullRowSelect = true;
			base.Controls.Add(metroScrollBar_0);
			metroScrollBar_0.Visible = false;
			metroScrollBar_0.Width = 15;
			metroScrollBar_0.Dock = DockStyle.Right;
			metroScrollBar_0.ValueChanged += method_2;
		}

		private void MetroListView_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateScrollbar();
		}

		private void MetroListView_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			if (listViewColumnSorter_0 == null)
			{
				return;
			}
			if (e.Column == listViewColumnSorter_0.SortColumn)
			{
				if (listViewColumnSorter_0.Order == SortOrder.Ascending)
				{
					listViewColumnSorter_0.Order = SortOrder.Descending;
				}
				else
				{
					listViewColumnSorter_0.Order = SortOrder.Ascending;
				}
			}
			else
			{
				listViewColumnSorter_0.SortColumn = e.Column;
				listViewColumnSorter_0.Order = SortOrder.Ascending;
			}
			Sort();
		}

		private void MetroListView_Resize(object sender, EventArgs e)
		{
			_ = base.Columns.Count;
		}

		private void MetroListView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
		{
			Color color = MetroPaint.ForeColor.Button.Disabled(Theme);
			if (base.View == System.Windows.Forms.View.Details)
			{
				if (e.Item.Selected)
				{
					e.Graphics.FillRectangle(new SolidBrush(ControlPaint.Light(MetroPaint.GetStyleColor(Style), float_0)), e.Bounds);
					color = Color.White;
				}
				TextFormatFlags textFormatFlags = TextFormatFlags.Default;
				int num = 0;
				int num2 = 0;
				if (base.CheckBoxes && e.ColumnIndex == 0)
				{
					num = 12;
					num2 = 14;
					int num3 = e.Bounds.Height / 2 - 6;
					using (Pen pen = new Pen(color))
					{
						Rectangle rect = new Rectangle(e.Bounds.X + 2, e.Bounds.Y + num3, 12, 12);
						e.Graphics.DrawRectangle(pen, rect);
					}
					if (e.Item.Checked)
					{
						Color color2 = MetroPaint.GetStyleColor(Style);
						if (e.Item.Selected)
						{
							color2 = Color.White;
						}
						using SolidBrush brush = new SolidBrush(color2);
						num3 = e.Bounds.Height / 2 - 4;
						Rectangle rect2 = new Rectangle(e.Bounds.X + 4, e.Bounds.Y + num3, 9, 9);
						e.Graphics.FillRectangle(brush, rect2);
					}
				}
				if (base.SmallImageList != null)
				{
					int num4 = 0;
					Image image = null;
					if (e.Item.ImageIndex > -1)
					{
						image = base.SmallImageList.Images[e.Item.ImageIndex];
					}
					if (e.Item.ImageKey != "")
					{
						image = base.SmallImageList.Images[e.Item.ImageKey];
					}
					if (image != null)
					{
						num2 += ((num2 > 0) ? 4 : 2);
						num4 = (e.Item.Bounds.Height - image.Height) / 2;
						e.Graphics.DrawImage(image, new Rectangle(e.Item.Bounds.Left + num2, e.Item.Bounds.Top + num4, image.Width, image.Height));
						num2 += base.SmallImageList.ImageSize.Width;
						num += base.SmallImageList.ImageSize.Width;
					}
				}
				int num5 = e.Item.Bounds.Width;
				if (base.View == System.Windows.Forms.View.Details)
				{
					num5 = base.Columns[0].Width;
				}
				using StringFormat stringFormat = new StringFormat();
				switch (e.Header.TextAlign)
				{
				case HorizontalAlignment.Right:
					stringFormat.Alignment = StringAlignment.Far;
					break;
				case HorizontalAlignment.Center:
					stringFormat.Alignment = StringAlignment.Center;
					break;
				}
				if (e.ColumnIndex > 0 && double.TryParse(e.SubItem.Text, NumberStyles.Currency, NumberFormatInfo.CurrentInfo, out var _))
				{
					stringFormat.Alignment = StringAlignment.Far;
				}
				Rectangle bounds = new Rectangle(e.Bounds.X + num2, e.Bounds.Y, num5 - num, e.Item.Bounds.Height);
				TextRenderer.DrawText(e.Graphics, e.SubItem.Text, font_0, bounds, color, textFormatFlags | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.WordEllipsis);
			}
			else
			{
				e.DrawDefault = true;
			}
		}

		private void MetroListView_DrawItem(object sender, DrawListViewItemEventArgs e)
		{
			Color color = MetroPaint.ForeColor.Button.Disabled(Theme);
			if (!((base.View == System.Windows.Forms.View.Details) | (base.View == System.Windows.Forms.View.List) | (base.View == System.Windows.Forms.View.SmallIcon)))
			{
				if (base.View == System.Windows.Forms.View.Tile)
				{
					int num = 0;
					if (base.LargeImageList != null)
					{
						int num2 = 0;
						num = base.LargeImageList.ImageSize.Width + 2;
						Image image = null;
						if (e.Item.ImageIndex > -1)
						{
							image = base.LargeImageList.Images[e.Item.ImageIndex];
						}
						if (e.Item.ImageKey != "")
						{
							image = base.LargeImageList.Images[e.Item.ImageKey];
						}
						if (image != null)
						{
							num2 = (e.Item.Bounds.Height - image.Height) / 2;
							e.Graphics.DrawImage(image, new Rectangle(e.Item.Bounds.Left + num, e.Item.Bounds.Top + num2, image.Width, image.Height));
						}
					}
					if (e.Item.Selected)
					{
						Rectangle rect = new Rectangle(e.Item.Bounds.X + num, e.Item.Bounds.Y, e.Item.Bounds.Width, e.Item.Bounds.Height);
						e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(250, 194, 87)), rect);
					}
					int num3 = 0;
					foreach (ListViewItem.ListViewSubItem subItem in e.Item.SubItems)
					{
						if (num3 > 0 && !e.Item.Selected)
						{
							color = Color.Silver;
						}
						_ = e.Item.Bounds.Y;
						_ = (e.Item.Bounds.Height - e.Item.SubItems.Count * 15) / 2;
						Rectangle bounds = new Rectangle(e.Item.Bounds.X + num, e.Item.Bounds.Y + num3, e.Item.Bounds.Width, e.Item.Bounds.Height);
						TextRenderer.DrawText(e.Graphics, subItem.Text, new Font("Segoe UI", 9f), bounds, color, TextFormatFlags.SingleLine | TextFormatFlags.WordEllipsis);
						num3 += 15;
					}
					return;
				}
				if (base.CheckBoxes)
				{
					int num4 = e.Bounds.Height / 2 - 6;
					using (Pen pen = new Pen(Color.Black))
					{
						Rectangle rect2 = new Rectangle(e.Bounds.X + 6, e.Bounds.Y + num4, 12, 12);
						e.Graphics.DrawRectangle(pen, rect2);
					}
					if (e.Item.Checked)
					{
						Color color2 = MetroPaint.GetStyleColor(Style);
						if (e.Item.Selected)
						{
							color2 = Color.White;
						}
						using SolidBrush brush = new SolidBrush(color2);
						num4 = e.Bounds.Height / 2 - 4;
						Rectangle rect3 = new Rectangle(e.Bounds.X + 8, e.Bounds.Y + num4, 9, 9);
						e.Graphics.FillRectangle(brush, rect3);
					}
					Rectangle rectangle = new Rectangle(e.Bounds.X + 23, e.Bounds.Y + 1, e.Bounds.Width, e.Bounds.Height);
					e.Graphics.DrawString(e.Item.Text, font_0, new SolidBrush(color), rectangle);
				}
				Font = font_0;
				e.DrawDefault = true;
				return;
			}
			Color color3 = MetroPaint.GetStyleColor(Style);
			if (e.Item.Selected)
			{
				e.Graphics.FillRectangle(new SolidBrush(ControlPaint.Light(MetroPaint.GetStyleColor(Style), float_0)), e.Bounds);
				color = Color.White;
				color3 = Color.White;
			}
			TextFormatFlags textFormatFlags = TextFormatFlags.Default;
			int num5 = 0;
			int num6 = 0;
			if (base.CheckBoxes)
			{
				num5 = 12;
				num6 = 14;
				int num7 = e.Bounds.Height / 2 - 6;
				using (Pen pen2 = new Pen(color))
				{
					Rectangle rect4 = new Rectangle(e.Bounds.X + 2, e.Bounds.Y + num7, 12, 12);
					e.Graphics.DrawRectangle(pen2, rect4);
				}
				if (e.Item.Checked)
				{
					using SolidBrush brush2 = new SolidBrush(color3);
					num7 = e.Bounds.Height / 2 - 4;
					Rectangle rect5 = new Rectangle(e.Bounds.X + 4, e.Bounds.Y + num7, 9, 9);
					e.Graphics.FillRectangle(brush2, rect5);
				}
			}
			if (base.SmallImageList != null)
			{
				int num8 = 0;
				Image image2 = null;
				if (e.Item.ImageIndex > -1)
				{
					image2 = base.SmallImageList.Images[e.Item.ImageIndex];
				}
				if (e.Item.ImageKey != "")
				{
					image2 = base.SmallImageList.Images[e.Item.ImageKey];
				}
				if (image2 != null)
				{
					num6 += ((num6 > 0) ? 4 : 2);
					num8 = (e.Item.Bounds.Height - image2.Height) / 2;
					e.Graphics.DrawImage(image2, new Rectangle(e.Item.Bounds.Left + num6, e.Item.Bounds.Top + num8, image2.Width, image2.Height));
					num6 += base.SmallImageList.ImageSize.Width;
					num5 += base.SmallImageList.ImageSize.Width;
				}
			}
			if (base.View != System.Windows.Forms.View.Details)
			{
				int num9 = e.Item.Bounds.Width;
				if (base.View == System.Windows.Forms.View.Details)
				{
					num9 = base.Columns[0].Width;
				}
				Rectangle bounds2 = new Rectangle(e.Bounds.X + num6, e.Bounds.Y, num9 - num5, e.Item.Bounds.Height);
				TextRenderer.DrawText(e.Graphics, e.Item.Text, font_0, bounds2, color, textFormatFlags | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.WordEllipsis);
			}
		}

		private void MetroListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
		{
			Color color = MetroPaint.ForeColor.Button.Press(Theme);
			e.Graphics.FillRectangle(new SolidBrush(MetroPaint.GetStyleColor(Style)), e.Bounds);
			using StringFormat stringFormat = new StringFormat();
			stringFormat.Alignment = StringAlignment.Center;
			e.Graphics.DrawString(e.Header.Text, font_0, new SolidBrush(color), e.Bounds, stringFormat);
		}

		internal static bool kp8qx7ME8GeQMxTrxS7()
		{
			return w1QQ3RM5URW5KVaDkN0 == null;
		}

		internal static void AT6wukoP74RGVP84gJJ()
		{
		}
	}
}
