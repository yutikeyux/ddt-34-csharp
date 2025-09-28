using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms;
using MetroFramework.Components;
using MetroFramework.Drawing;
using MetroFramework.Interfaces;
using MetroFramework.Native;

namespace MetroFramework.Controls
{
	[ToolboxBitmap(typeof(TabControl))]
	[Designer("MetroFramework.Design.Controls.MetroTabControlDesigner, MetroFramework.Design, Version=1.4.0.0, Culture=neutral, PublicKeyToken=5f91a84759bf584a")]
	public class MetroTabControl : TabControl, IMetroControl
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

		private List<string> list_0 = new List<string>();

		private List<string> list_1 = new List<string>();

		private List<HiddenTabs> list_2 = new List<HiddenTabs>();

		private SubClass subClass_0;

		private bool bool_3;

		private MetroTabControlSize metroTabControlSize_0 = MetroTabControlSize.Medium;

		private MetroTabControlWeight metroTabControlWeight_0;

		private ContentAlignment contentAlignment_0 = ContentAlignment.MiddleLeft;

		private bool bool_4;

		internal static MetroTabControl mnSCxfnoXR28oZUGcU4;

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

		[DefaultValue(MetroTabControlSize.Medium)]
		[Category("Metro Appearance")]
		public MetroTabControlSize FontSize
		{
			get
			{
				return metroTabControlSize_0;
			}
			set
			{
				metroTabControlSize_0 = value;
			}
		}

		[DefaultValue(MetroTabControlWeight.Light)]
		[Category("Metro Appearance")]
		public MetroTabControlWeight FontWeight
		{
			get
			{
				return metroTabControlWeight_0;
			}
			set
			{
				metroTabControlWeight_0 = value;
			}
		}

		[DefaultValue(ContentAlignment.MiddleLeft)]
		[Category("Metro Appearance")]
		public ContentAlignment TextAlign
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

		[Editor("MetroFramework.Design.MetroTabPageCollectionEditor, MetroFramework.Design, Version=1.4.0.0, Culture=neutral, PublicKeyToken=5f91a84759bf584a", typeof(UITypeEditor))]
		public new TabPageCollection TabPages => base.TabPages;

		[DefaultValue(false)]
		[Category("Metro Appearance")]
		public new bool IsMirrored
		{
			get
			{
				return bool_4;
			}
			set
			{
				if (bool_4 != value)
				{
					bool_4 = value;
					UpdateStyles();
				}
			}
		}

		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				if (bool_4)
				{
					createParams.ExStyle = createParams.ExStyle | 0x400000 | 0x100000;
				}
				return createParams;
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

		public MetroTabControl()
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, value: true);
			base.Padding = new Point(6, 8);
			base.Selecting += MetroTabControl_Selecting;
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
			for (int i = 0; i < TabPages.Count; i++)
			{
				if (i != base.SelectedIndex)
				{
					method_2(i, e.Graphics);
				}
			}
			if (base.SelectedIndex > -1)
			{
				method_0(base.SelectedIndex, e.Graphics);
				method_2(base.SelectedIndex, e.Graphics);
				method_1(base.SelectedIndex, e.Graphics);
				OnCustomPaintForeground(new MetroPaintEventArgs(Color.Empty, Color.Empty, e.Graphics));
			}
		}

		private void method_0(int int_0, Graphics graphics_0)
		{
			using Brush brush = new SolidBrush(MetroPaint.BorderColor.TabControl.Normal(Theme));
			Rectangle rect = new Rectangle(DisplayRectangle.X, method_4(int_0).Bottom + 2 - 3, DisplayRectangle.Width, 3);
			graphics_0.FillRectangle(brush, rect);
		}

		private void method_1(int int_0, Graphics graphics_0)
		{
			using Brush brush = new SolidBrush(MetroPaint.GetStyleColor(Style));
			Rectangle rectangle = method_4(int_0);
			Rectangle rect = new Rectangle(rectangle.X + ((int_0 == 0) ? 2 : 0), method_4(int_0).Bottom + 2 - 3, rectangle.Width + ((int_0 != 0) ? 2 : 0), 3);
			graphics_0.FillRectangle(brush, rect);
		}

		private Size MeasureText(string text)
		{
			using Graphics dc = CreateGraphics();
			return TextRenderer.MeasureText(proposedSize: new Size(int.MaxValue, int.MaxValue), dc: dc, text: text, font: MetroFonts.TabControl(metroTabControlSize_0, metroTabControlWeight_0), flags: MetroPaint.GetTextFormatFlags(TextAlign) | TextFormatFlags.NoPadding);
		}

		private void method_2(int int_0, Graphics graphics_0)
		{
			Color color = BackColor;
			if (!bool_0)
			{
				color = MetroPaint.BackColor.Form(Theme);
			}
			TabPage tabPage = TabPages[int_0];
			Rectangle rectangle = method_4(int_0);
			Color foreColor = ((!base.Enabled || list_0.Contains(tabPage.Name)) ? MetroPaint.ForeColor.Label.Disabled(Theme) : ((!bool_1) ? (bool_2 ? MetroPaint.GetStyleColor(Style) : MetroPaint.ForeColor.TabControl.Normal(Theme)) : Control.DefaultForeColor));
			if (int_0 == 0)
			{
				rectangle.X = DisplayRectangle.X;
			}
			Rectangle rect = rectangle;
			rectangle.Width += 20;
			using (Brush brush = new SolidBrush(color))
			{
				graphics_0.FillRectangle(brush, rect);
			}
			TextRenderer.DrawText(graphics_0, tabPage.Text, MetroFonts.TabControl(metroTabControlSize_0, metroTabControlWeight_0), rectangle, foreColor, color, MetroPaint.GetTextFormatFlags(TextAlign));
		}

		[SecuritySafeCritical]
		private void method_3(Graphics graphics_0)
		{
			Color color = ((base.Parent != null) ? base.Parent.BackColor : MetroPaint.BackColor.Form(Theme));
			Rectangle rect = default(Rectangle);
			WinApi.GetClientRect(subClass_0.Handle, ref rect);
			graphics_0.CompositingQuality = CompositingQuality.HighQuality;
			graphics_0.SmoothingMode = SmoothingMode.AntiAlias;
			graphics_0.Clear(color);
			using Brush brush = new SolidBrush(MetroPaint.BorderColor.TabControl.Normal(Theme));
			GraphicsPath graphicsPath = new GraphicsPath(FillMode.Winding);
			PointF[] points = new PointF[3]
			{
				new PointF(6f, 6f),
				new PointF(16f, 0f),
				new PointF(16f, 12f)
			};
			graphicsPath.AddLines(points);
			graphics_0.FillPath(brush, graphicsPath);
			graphicsPath.Reset();
			PointF[] points2 = new PointF[3]
			{
				new PointF(rect.Width - 15, 0f),
				new PointF(rect.Width - 5, 6f),
				new PointF(rect.Width - 15, 12f)
			};
			graphicsPath.AddLines(points2);
			graphics_0.FillPath(brush, graphicsPath);
			graphicsPath.Dispose();
		}

		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			Invalidate();
		}

		protected override void OnParentBackColorChanged(EventArgs e)
		{
			base.OnParentBackColorChanged(e);
			Invalidate();
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			Invalidate();
		}

		[SecuritySafeCritical]
		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);
			if (!base.DesignMode)
			{
				WinApi.ShowScrollBar(base.Handle, 3, 0);
			}
		}

		private Rectangle method_4(int int_0)
		{
			if (int_0 < 0)
			{
				return default(Rectangle);
			}
			return GetTabRect(int_0);
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			if (base.SelectedIndex != -1 && !TabPages[base.SelectedIndex].Focused)
			{
				bool flag = false;
				foreach (Control control in TabPages[base.SelectedIndex].Controls)
				{
					if (control.Focused)
					{
						flag = true;
						return;
					}
				}
				if (!flag)
				{
					TabPages[base.SelectedIndex].Select();
					TabPages[base.SelectedIndex].Focus();
				}
			}
			base.OnMouseWheel(e);
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			OnFontChanged(EventArgs.Empty);
			method_5();
		}

		protected override void OnControlAdded(ControlEventArgs e)
		{
			base.OnControlAdded(e);
			method_5();
			method_6();
		}

		protected override void OnControlRemoved(ControlEventArgs e)
		{
			base.OnControlRemoved(e);
			method_5();
			method_6();
		}

		protected override void OnSelectedIndexChanged(EventArgs e)
		{
			base.OnSelectedIndexChanged(e);
			method_6();
			Invalidate();
		}

		[DllImport("user32.dll")]
		private static extern IntPtr SendMessage(IntPtr intptr_0, int int_0, IntPtr intptr_1, IntPtr intptr_2);

		[SecuritySafeCritical]
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			IntPtr intptr_ = MetroFonts.TabControl(metroTabControlSize_0, metroTabControlWeight_0).ToHfont();
			SendMessage(base.Handle, 48, intptr_, (IntPtr)(-1));
			SendMessage(base.Handle, 29, IntPtr.Zero, IntPtr.Zero);
			UpdateStyles();
		}

		private void MetroTabControl_Selecting(object sender, TabControlCancelEventArgs e)
		{
			if (list_0.Count > 0 && list_0.Contains(e.TabPage.Name))
			{
				e.Cancel = true;
			}
		}

		[SecuritySafeCritical]
		private void method_5()
		{
			if (base.DesignMode)
			{
				return;
			}
			bool flag = false;
			IntPtr intPtr = WinApi.GetWindow(base.Handle, 5);
			while (intPtr != IntPtr.Zero)
			{
				char[] array = (char[])(object)new char[33];
				int className = WinApi.GetClassName(intPtr, array, 32);
				string text = (string)(object)new string(array, 0, className);
				if (!(text == "msctls_updown32"))
				{
					intPtr = WinApi.GetWindow(intPtr, 2);
					continue;
				}
				flag = true;
				if (!bool_3)
				{
					subClass_0 = new SubClass(intPtr, _SubClass: true);
					subClass_0.SubClassedWndProc += subClass_0_SubClassedWndProc;
					bool_3 = true;
				}
				break;
			}
			if (!flag && bool_3)
			{
				bool_3 = false;
			}
		}

		[SecuritySafeCritical]
		private void method_6()
		{
			if (bool_3 && !base.DesignMode && WinApi.IsWindowVisible(subClass_0.Handle))
			{
				Rectangle rect = default(Rectangle);
				WinApi.GetClientRect(subClass_0.Handle, ref rect);
				WinApi.InvalidateRect(subClass_0.Handle, ref rect, bErase: true);
			}
		}

		[SecuritySafeCritical]
		private int subClass_0_SubClassedWndProc(ref Message message_0)
		{
			int msg = message_0.Msg;
			if (msg == 15)
			{
				IntPtr windowDC = WinApi.GetWindowDC(subClass_0.Handle);
				Graphics graphics = Graphics.FromHdc(windowDC);
				method_3(graphics);
				graphics.Dispose();
				WinApi.ReleaseDC(subClass_0.Handle, windowDC);
				message_0.Result = IntPtr.Zero;
				Rectangle rect = default(Rectangle);
				WinApi.GetClientRect(subClass_0.Handle, ref rect);
				WinApi.ValidateRect(subClass_0.Handle, ref rect);
				return 1;
			}
			return 0;
		}

		public void HideTab(MetroTabPage tabpage)
		{
			if (TabPages.Contains(tabpage))
			{
				int id = TabPages.IndexOf(tabpage);
				list_2.Add(new HiddenTabs(id, tabpage.Name));
				TabPages.Remove(tabpage);
			}
		}

		public void ShowTab(MetroTabPage tabpage)
		{
			HiddenTabs hiddenTabs = list_2.Find((HiddenTabs bk) => bk.tabpage == tabpage.Name);
			if (hiddenTabs != null)
			{
				TabPages.Insert(hiddenTabs.index, tabpage);
				list_2.Remove(hiddenTabs);
			}
		}

		public void DisableTab(MetroTabPage tabpage)
		{
			if (list_0.Contains(tabpage.Name) || (base.SelectedTab == tabpage && base.TabCount == 1))
			{
				return;
			}
			if (base.SelectedTab == tabpage)
			{
				if (base.SelectedIndex == base.TabCount - 1)
				{
					base.SelectedIndex = 0;
				}
				else
				{
					base.SelectedIndex++;
				}
			}
			int int_ = TabPages.IndexOf(tabpage);
			list_0.Add(tabpage.Name);
			Graphics graphics_ = CreateGraphics();
			method_2(int_, graphics_);
			method_0(base.SelectedIndex, graphics_);
			method_1(base.SelectedIndex, graphics_);
		}

		public void EnableTab(MetroTabPage tabpage)
		{
			if (list_0.Contains(tabpage.Name))
			{
				list_0.Remove(tabpage.Name);
				int int_ = TabPages.IndexOf(tabpage);
				Graphics graphics_ = CreateGraphics();
				method_2(int_, graphics_);
				method_0(base.SelectedIndex, graphics_);
				method_1(base.SelectedIndex, graphics_);
			}
		}

		public bool IsTabEnable(MetroTabPage tabpage)
		{
			return list_0.Contains(tabpage.Name);
		}

		public bool IsTabHidden(MetroTabPage tabpage)
		{
			HiddenTabs hiddenTabs = list_2.Find((HiddenTabs bk) => bk.tabpage == tabpage.Name);
			return hiddenTabs != null;
		}

		internal static bool DdwHm3nV2gNitj23uwE()
		{
			return mnSCxfnoXR28oZUGcU4 == null;
		}

		internal static void EAv2KOnYc1qNYPflSxJ()
		{
		}
	}
}
