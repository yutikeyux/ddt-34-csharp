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
	[ToolboxBitmap(typeof(Label))]
	[Designer("MetroFramework.Design.Controls.MetroLabelDesigner, MetroFramework.Design, Version=1.4.0.0, Culture=neutral, PublicKeyToken=5f91a84759bf584a")]
	public class MetroLabel : Label, IMetroControl
	{
		private class Class18 : TextBox
		{
			internal static object pGrCoCMHpCUpUF9Hmvt;

			public Class18()
			{
				SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, value: true);
			}

			internal static void S80U4mM6oT1eKKseYuB()
			{
			}

			internal static bool bfEemvMlx0TVJJRYpet()
			{
				return pGrCoCMHpCUpUF9Hmvt == null;
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

		private Class18 class18_0;

		private MetroLabelSize metroLabelSize_0 = MetroLabelSize.Medium;

		private MetroLabelWeight XvepPvByqq;

		private MetroLabelMode metroLabelMode_0;

		private bool bool_3;

		private bool bool_4 = true;

		private static MetroLabel tW21mjMGBJaj0NsOiIt;

		[Category("Metro Appearance")]
		[DefaultValue(MetroColorStyle.Default)]
		public MetroColorStyle Style
		{
			get
			{
				if (base.DesignMode || metroColorStyle_0 != 0)
				{
					return metroColorStyle_0;
				}
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

		[Category("Metro Appearance")]
		[DefaultValue(MetroLabelSize.Medium)]
		public MetroLabelSize FontSize
		{
			get
			{
				return metroLabelSize_0;
			}
			set
			{
				metroLabelSize_0 = value;
				Refresh();
			}
		}

		[DefaultValue(MetroLabelWeight.Light)]
		[Category("Metro Appearance")]
		public MetroLabelWeight FontWeight
		{
			get
			{
				return XvepPvByqq;
			}
			set
			{
				XvepPvByqq = value;
				Refresh();
			}
		}

		[DefaultValue(MetroLabelMode.Default)]
		[Category("Metro Appearance")]
		public MetroLabelMode LabelMode
		{
			get
			{
				return metroLabelMode_0;
			}
			set
			{
				metroLabelMode_0 = value;
			}
		}

		[DefaultValue(false)]
		[Category("Metro Behaviour")]
		public bool WrapToLine
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

		public MetroLabel()
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, value: true);
			class18_0 = new Class18();
			class18_0.Visible = false;
			base.Controls.Add(class18_0);
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			try
			{
				Color color = BackColor;
				if (!bool_0)
				{
					color = MetroPaint.BackColor.Form(Theme);
					if (base.Parent is MetroTile)
					{
						color = MetroPaint.GetStyleColor(Style);
					}
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
			Color foreColor = (bool_1 ? ForeColor : (base.Enabled ? ((base.Parent == null) ? ((!bool_2) ? MetroPaint.ForeColor.Label.Normal(Theme) : MetroPaint.GetStyleColor(Style)) : ((base.Parent is MetroTile) ? MetroPaint.ForeColor.Tile.Normal(Theme) : ((!bool_2) ? MetroPaint.ForeColor.Label.Normal(Theme) : MetroPaint.GetStyleColor(Style)))) : ((base.Parent == null) ? MetroPaint.ForeColor.Label.Disabled(Theme) : ((!(base.Parent is MetroTile)) ? MetroPaint.ForeColor.Label.Normal(Theme) : MetroPaint.ForeColor.Tile.Disabled(Theme)))));
			if (LabelMode == MetroLabelMode.Selectable)
			{
				method_0();
				method_4();
				if (!class18_0.Visible)
				{
					TextRenderer.DrawText(e.Graphics, Text, MetroFonts.Label(metroLabelSize_0, XvepPvByqq), base.ClientRectangle, foreColor, MetroPaint.GetTextFormatFlags(TextAlign));
				}
			}
			else
			{
				method_3();
				TextRenderer.DrawText(e.Graphics, Text, MetroFonts.Label(metroLabelSize_0, XvepPvByqq), base.ClientRectangle, foreColor, MetroPaint.GetTextFormatFlags(TextAlign, bool_3));
				OnCustomPaintForeground(new MetroPaintEventArgs(Color.Empty, foreColor, e.Graphics));
			}
		}

		public override void Refresh()
		{
			if (LabelMode == MetroLabelMode.Selectable)
			{
				method_4();
			}
			base.Refresh();
		}

		public override Size GetPreferredSize(Size proposedSize)
		{
			base.GetPreferredSize(proposedSize);
			using Graphics dc = CreateGraphics();
			proposedSize = new Size(int.MaxValue, int.MaxValue);
			return TextRenderer.MeasureText(dc, Text, MetroFonts.Label(metroLabelSize_0, XvepPvByqq), proposedSize, MetroPaint.GetTextFormatFlags(TextAlign));
		}

		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			Invalidate();
		}

		protected override void OnResize(EventArgs e)
		{
			if (LabelMode == MetroLabelMode.Selectable)
			{
				method_5();
			}
			base.OnResize(e);
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			if (LabelMode == MetroLabelMode.Selectable)
			{
				method_6();
			}
		}

		private void method_0()
		{
			if ((class18_0.Visible && !bool_4) || !bool_4)
			{
				return;
			}
			bool_4 = false;
			if (!base.DesignMode)
			{
				Form form = FindForm();
				if (form != null)
				{
					form.ResizeBegin += method_2;
					form.ResizeEnd += method_1;
				}
			}
			class18_0.BackColor = Color.Transparent;
			class18_0.Visible = true;
			class18_0.BorderStyle = System.Windows.Forms.BorderStyle.None;
			class18_0.Font = MetroFonts.Label(metroLabelSize_0, XvepPvByqq);
			class18_0.Location = new Point(1, 0);
			class18_0.Text = Text;
			class18_0.ReadOnly = true;
			class18_0.Size = GetPreferredSize(System.Drawing.Size.Empty);
			class18_0.Multiline = true;
			class18_0.DoubleClick += class18_0_DoubleClick;
			class18_0.Click += class18_0_Click;
			base.Controls.Add(class18_0);
		}

		private void method_1(object sender, EventArgs e)
		{
			if (LabelMode == MetroLabelMode.Selectable)
			{
				method_6();
			}
		}

		private void method_2(object sender, EventArgs e)
		{
			if (LabelMode == MetroLabelMode.Selectable)
			{
				method_5();
			}
		}

		private void method_3()
		{
			if (class18_0.Visible)
			{
				class18_0.DoubleClick -= class18_0_DoubleClick;
				class18_0.Click -= class18_0_Click;
				class18_0.Visible = false;
			}
		}

		private void method_4()
		{
			if (!class18_0.Visible)
			{
				return;
			}
			SuspendLayout();
			class18_0.SuspendLayout();
			if (bool_0)
			{
				class18_0.BackColor = BackColor;
			}
			else
			{
				class18_0.BackColor = MetroPaint.BackColor.Form(Theme);
			}
			if (!base.Enabled)
			{
				if (base.Parent != null)
				{
					if (!(base.Parent is MetroTile))
					{
						if (!bool_2)
						{
							class18_0.ForeColor = MetroPaint.ForeColor.Label.Disabled(Theme);
						}
						else
						{
							class18_0.ForeColor = MetroPaint.GetStyleColor(Style);
						}
					}
					else
					{
						class18_0.ForeColor = MetroPaint.ForeColor.Tile.Disabled(Theme);
					}
				}
				else if (bool_2)
				{
					class18_0.ForeColor = MetroPaint.GetStyleColor(Style);
				}
				else
				{
					class18_0.ForeColor = MetroPaint.ForeColor.Label.Disabled(Theme);
				}
			}
			else if (base.Parent != null)
			{
				if (base.Parent is MetroTile)
				{
					class18_0.ForeColor = MetroPaint.ForeColor.Tile.Normal(Theme);
				}
				else if (!bool_2)
				{
					class18_0.ForeColor = MetroPaint.ForeColor.Label.Normal(Theme);
				}
				else
				{
					class18_0.ForeColor = MetroPaint.GetStyleColor(Style);
				}
			}
			else if (bool_2)
			{
				class18_0.ForeColor = MetroPaint.GetStyleColor(Style);
			}
			else
			{
				class18_0.ForeColor = MetroPaint.ForeColor.Label.Normal(Theme);
			}
			class18_0.Font = MetroFonts.Label(metroLabelSize_0, XvepPvByqq);
			class18_0.Text = Text;
			class18_0.BorderStyle = System.Windows.Forms.BorderStyle.None;
			base.Size = GetPreferredSize(System.Drawing.Size.Empty);
			class18_0.ResumeLayout();
			ResumeLayout();
		}

		private void method_5()
		{
			class18_0.Visible = false;
		}

		private void method_6()
		{
			class18_0.Visible = true;
		}

		[SecuritySafeCritical]
		private void class18_0_Click(object sender, EventArgs e)
		{
			WinCaret.HideCaret(class18_0.Handle);
		}

		[SecuritySafeCritical]
		private void class18_0_DoubleClick(object sender, EventArgs e)
		{
			class18_0.SelectAll();
			WinCaret.HideCaret(class18_0.Handle);
		}

		internal static bool Xe33RAMmiebCIp3uvei()
		{
			return tW21mjMGBJaj0NsOiIt == null;
		}
	}
}
