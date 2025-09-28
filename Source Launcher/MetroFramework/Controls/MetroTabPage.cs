using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Security;
using System.Windows.Forms;
using MetroFramework.Components;
using MetroFramework.Drawing;
using MetroFramework.Interfaces;
using MetroFramework.Native;

namespace MetroFramework.Controls
{
	[Designer("MetroFramework.Design.Controls.MetroTabPageDesigner, MetroFramework.Design, Version=1.4.0.0, Culture=neutral, PublicKeyToken=5f91a84759bf584a")]
	[ToolboxItem(false)]
	public class MetroTabPage : TabPage, IMetroControl
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

		private MetroScrollBar metroScrollBar_0 = new MetroScrollBar(MetroScrollOrientation.Vertical);

		private MetroScrollBar metroScrollBar_1 = new MetroScrollBar(MetroScrollOrientation.Horizontal);

		private bool bool_3;

		private bool bool_4;

		internal static MetroTabPage QBcvjqnvLDRxB9UdS6m;

		[DefaultValue(MetroColorStyle.Default)]
		[Category("Metro Appearance")]
		public MetroColorStyle Style
		{
			get
			{
				if (!base.DesignMode && metroColorStyle_0 == MetroColorStyle.Default)
				{
					if (StyleManager == null || metroColorStyle_0 != 0)
					{
						if (StyleManager != null || metroColorStyle_0 != 0)
						{
							return metroColorStyle_0;
						}
						return MetroColorStyle.Blue;
					}
					return StyleManager.Style;
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
					if (StyleManager == null || metroThemeStyle_0 != 0)
					{
						if (StyleManager != null || metroThemeStyle_0 != 0)
						{
							return metroThemeStyle_0;
						}
						return MetroThemeStyle.Light;
					}
					return StyleManager.Theme;
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

		[DefaultValue(false)]
		[Category("Metro Appearance")]
		public bool HorizontalScrollbar
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

		[Category("Metro Appearance")]
		public int HorizontalScrollbarSize
		{
			get
			{
				return metroScrollBar_1.ScrollbarSize;
			}
			set
			{
				metroScrollBar_1.ScrollbarSize = value;
			}
		}

		[Category("Metro Appearance")]
		public bool HorizontalScrollbarBarColor
		{
			get
			{
				return metroScrollBar_1.UseBarColor;
			}
			set
			{
				metroScrollBar_1.UseBarColor = value;
			}
		}

		[Category("Metro Appearance")]
		public bool HorizontalScrollbarHighlightOnWheel
		{
			get
			{
				return metroScrollBar_1.HighlightOnWheel;
			}
			set
			{
				metroScrollBar_1.HighlightOnWheel = value;
			}
		}

		[Category("Metro Appearance")]
		[DefaultValue(false)]
		public bool VerticalScrollbar
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

		[Category("Metro Appearance")]
		public int VerticalScrollbarSize
		{
			get
			{
				return metroScrollBar_0.ScrollbarSize;
			}
			set
			{
				metroScrollBar_0.ScrollbarSize = value;
			}
		}

		[Category("Metro Appearance")]
		public bool VerticalScrollbarBarColor
		{
			get
			{
				return metroScrollBar_0.UseBarColor;
			}
			set
			{
				metroScrollBar_0.UseBarColor = value;
			}
		}

		[Category("Metro Appearance")]
		public bool VerticalScrollbarHighlightOnWheel
		{
			get
			{
				return metroScrollBar_0.HighlightOnWheel;
			}
			set
			{
				metroScrollBar_0.HighlightOnWheel = value;
			}
		}

		[Category("Metro Appearance")]
		public new bool AutoScroll
		{
			get
			{
				return base.AutoScroll;
			}
			set
			{
				if (value)
				{
					bool_3 = true;
					bool_4 = true;
				}
				base.AutoScroll = value;
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

		public MetroTabPage()
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, value: true);
			base.Controls.Add(metroScrollBar_0);
			base.Controls.Add(metroScrollBar_1);
			metroScrollBar_0.UseBarColor = true;
			metroScrollBar_1.UseBarColor = true;
			metroScrollBar_0.Scroll += metroScrollBar_0_Scroll;
			metroScrollBar_1.Scroll += metroScrollBar_1_Scroll;
		}

		private void metroScrollBar_1_Scroll(object sender, ScrollEventArgs e)
		{
			base.AutoScrollPosition = new Point(e.NewValue, metroScrollBar_0.Value);
			method_0();
		}

		private void metroScrollBar_0_Scroll(object sender, ScrollEventArgs e)
		{
			base.AutoScrollPosition = new Point(metroScrollBar_1.Value, e.NewValue);
			method_0();
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
			base.OnPaint(e);
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
			if (base.DesignMode)
			{
				metroScrollBar_1.Visible = false;
				metroScrollBar_0.Visible = false;
				return;
			}
			method_0();
			if (HorizontalScrollbar)
			{
				metroScrollBar_1.Visible = base.HorizontalScroll.Visible;
			}
			if (base.HorizontalScroll.Visible)
			{
				metroScrollBar_1.Minimum = base.HorizontalScroll.Minimum;
				metroScrollBar_1.Maximum = base.HorizontalScroll.Maximum;
				metroScrollBar_1.SmallChange = base.HorizontalScroll.SmallChange;
				metroScrollBar_1.LargeChange = base.HorizontalScroll.LargeChange;
			}
			if (VerticalScrollbar)
			{
				metroScrollBar_0.Visible = base.VerticalScroll.Visible;
			}
			if (base.VerticalScroll.Visible)
			{
				metroScrollBar_0.Minimum = base.VerticalScroll.Minimum;
				metroScrollBar_0.Maximum = base.VerticalScroll.Maximum;
				metroScrollBar_0.SmallChange = base.VerticalScroll.SmallChange;
				metroScrollBar_0.LargeChange = base.VerticalScroll.LargeChange;
			}
			OnCustomPaintForeground(new MetroPaintEventArgs(Color.Empty, Color.Empty, e.Graphics));
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			metroScrollBar_0.Value = base.VerticalScroll.Value;
			metroScrollBar_1.Value = base.HorizontalScroll.Value;
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

		private void method_0()
		{
			if (base.DesignMode)
			{
				return;
			}
			if (AutoScroll)
			{
				metroScrollBar_0.Location = new Point(base.ClientRectangle.Width - metroScrollBar_0.Width, base.ClientRectangle.Y);
				metroScrollBar_0.Height = base.ClientRectangle.Height;
				if (!VerticalScrollbar)
				{
					metroScrollBar_0.Visible = false;
				}
				metroScrollBar_1.Location = new Point(base.ClientRectangle.X, base.ClientRectangle.Height - metroScrollBar_1.Height);
				metroScrollBar_1.Width = base.ClientRectangle.Width;
				if (!HorizontalScrollbar)
				{
					metroScrollBar_1.Visible = false;
				}
			}
			else
			{
				metroScrollBar_0.Visible = false;
				metroScrollBar_1.Visible = false;
			}
		}

		internal static bool m79mTIn9EhnqpePmMMj()
		{
			return QBcvjqnvLDRxB9UdS6m == null;
		}

		internal static void EUBy7AnWsSND6CNm03Q()
		{
		}
	}
}
