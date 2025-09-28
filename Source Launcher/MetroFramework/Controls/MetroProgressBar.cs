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
	[Designer("MetroFramework.Design.Controls.MetroProgressBarDesigner, MetroFramework.Design, Version=1.4.0.0, Culture=neutral, PublicKeyToken=5f91a84759bf584a")]
	[ToolboxBitmap(typeof(ProgressBar))]
	public class MetroProgressBar : ProgressBar, IMetroControl
	{
		private EventHandler<MetroPaintEventArgs> eventHandler_0;

		private EventHandler<MetroPaintEventArgs> eventHandler_1;

		private EventHandler<MetroPaintEventArgs> eventHandler_2;

		private MetroColorStyle metroColorStyle_0;

		private MetroThemeStyle metroThemeStyle_0;

		private MetroStyleManager metroStyleManager_0;

		private bool bool_0;

		private bool bool_1;

		private bool bool_2 = true;

		private MetroProgressBarSize metroProgressBarSize_0 = MetroProgressBarSize.Medium;

		private MetroProgressBarWeight metroProgressBarWeight_0;

		private ContentAlignment contentAlignment_0 = ContentAlignment.MiddleRight;

		private bool bool_3 = true;

		private ProgressBarStyle progressBarStyle_0 = System.Windows.Forms.ProgressBarStyle.Continuous;

		private int int_0;

		private Timer timer_0;

		private static MetroProgressBar RDgZohV7UfmoMmY05Dv;

		[Category("Metro Appearance")]
		[DefaultValue(MetroColorStyle.Default)]
		public new MetroColorStyle Style
		{
			get
			{
				if (!base.DesignMode && metroColorStyle_0 == MetroColorStyle.Default)
				{
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
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Category("Metro Appearance")]
		[Browsable(false)]
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

		[DefaultValue(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
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
		[DefaultValue(MetroProgressBarSize.Medium)]
		public MetroProgressBarSize FontSize
		{
			get
			{
				return metroProgressBarSize_0;
			}
			set
			{
				metroProgressBarSize_0 = value;
			}
		}

		[Category("Metro Appearance")]
		[DefaultValue(MetroProgressBarWeight.Light)]
		public MetroProgressBarWeight FontWeight
		{
			get
			{
				return metroProgressBarWeight_0;
			}
			set
			{
				metroProgressBarWeight_0 = value;
			}
		}

		[Category("Metro Appearance")]
		[DefaultValue(ContentAlignment.MiddleRight)]
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

		[DefaultValue(true)]
		[Category("Metro Appearance")]
		public bool HideProgressText
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

		[DefaultValue(System.Windows.Forms.ProgressBarStyle.Continuous)]
		[Category("Metro Appearance")]
		public ProgressBarStyle ProgressBarStyle
		{
			get
			{
				return progressBarStyle_0;
			}
			set
			{
				progressBarStyle_0 = value;
			}
		}

		public new int Value
		{
			get
			{
				return base.Value;
			}
			set
			{
				if (value <= base.Maximum)
				{
					base.Value = value;
					Invalidate();
				}
			}
		}

		[Browsable(false)]
		public double ProgressTotalPercent => (1.0 - (double)(base.Maximum - Value) / (double)base.Maximum) * 100.0;

		[Browsable(false)]
		public double ProgressTotalValue => 1.0 - (double)(base.Maximum - Value) / (double)base.Maximum;

		[Browsable(false)]
		public string ProgressPercentText => $"{(double)Math.Round(ProgressTotalPercent)}%";

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

		[SpecialName]
		private double method_0()
		{
			return (double)Value / (double)base.Maximum * (double)base.ClientRectangle.Width;
		}

		[SpecialName]
		private int method_1()
		{
			return base.ClientRectangle.Width / 3;
		}

		public MetroProgressBar()
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, value: true);
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			try
			{
				Color color = BackColor;
				if (!bool_0)
				{
					color = (base.Enabled ? MetroPaint.BackColor.ProgressBar.Bar.Normal(Theme) : MetroPaint.BackColor.ProgressBar.Bar.Disabled(Theme));
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
			if (progressBarStyle_0 == System.Windows.Forms.ProgressBarStyle.Continuous)
			{
				if (!base.DesignMode)
				{
					method_7();
				}
				method_2(e.Graphics);
			}
			else if (progressBarStyle_0 != 0)
			{
				if (progressBarStyle_0 == System.Windows.Forms.ProgressBarStyle.Marquee)
				{
					if (!base.DesignMode && base.Enabled)
					{
						method_6();
					}
					if (!base.Enabled)
					{
						method_7();
					}
					if (Value == base.Maximum)
					{
						method_7();
						method_2(e.Graphics);
					}
					else
					{
						method_3(e.Graphics);
					}
				}
			}
			else
			{
				if (!base.DesignMode)
				{
					method_7();
				}
				method_2(e.Graphics);
			}
			method_4(e.Graphics);
			using (Pen pen = new Pen(MetroPaint.BorderColor.ProgressBar.Normal(Theme)))
			{
				Rectangle rect = new Rectangle(0, 0, base.Width - 1, base.Height - 1);
				e.Graphics.DrawRectangle(pen, rect);
			}
			OnCustomPaintForeground(new MetroPaintEventArgs(Color.Empty, Color.Empty, e.Graphics));
		}

		private void method_2(Graphics graphics_0)
		{
			graphics_0.FillRectangle(MetroPaint.GetStyleBrush(Style), 0, 0, (int)method_0(), base.ClientRectangle.Height);
		}

		private void method_3(Graphics graphics_0)
		{
			graphics_0.FillRectangle(MetroPaint.GetStyleBrush(Style), int_0, 0, method_1(), base.ClientRectangle.Height);
		}

		private void method_4(Graphics graphics_0)
		{
			if (!HideProgressText)
			{
				TextRenderer.DrawText(foreColor: base.Enabled ? MetroPaint.ForeColor.ProgressBar.Normal(Theme) : MetroPaint.ForeColor.ProgressBar.Disabled(Theme), dc: graphics_0, text: ProgressPercentText, font: MetroFonts.ProgressBar(metroProgressBarSize_0, metroProgressBarWeight_0), bounds: base.ClientRectangle, flags: MetroPaint.GetTextFormatFlags(TextAlign));
			}
		}

		public override Size GetPreferredSize(Size proposedSize)
		{
			base.GetPreferredSize(proposedSize);
			using Graphics dc = CreateGraphics();
			proposedSize = new Size(int.MaxValue, int.MaxValue);
			return TextRenderer.MeasureText(dc, ProgressPercentText, MetroFonts.ProgressBar(metroProgressBarSize_0, metroProgressBarWeight_0), proposedSize, MetroPaint.GetTextFormatFlags(TextAlign));
		}

		[SpecialName]
		private bool method_5()
		{
			if (timer_0 == null)
			{
				return false;
			}
			return timer_0.Enabled;
		}

		private void method_6()
		{
			if (!method_5())
			{
				if (timer_0 == null)
				{
					timer_0 = new Timer
					{
						Interval = 10
					};
					timer_0.Tick += timer_0_Tick;
				}
				int_0 = -method_1();
				timer_0.Stop();
				timer_0.Start();
				timer_0.Enabled = true;
				Invalidate();
			}
		}

		private void method_7()
		{
			if (timer_0 != null)
			{
				timer_0.Stop();
				Invalidate();
			}
		}

		private void timer_0_Tick(object sender, EventArgs e)
		{
			int_0++;
			if (int_0 > base.ClientRectangle.Width)
			{
				int_0 = -method_1();
			}
			Invalidate();
		}

		internal static bool mmKihVVb8JpaQeRUEhH()
		{
			return RDgZohV7UfmoMmY05Dv == null;
		}

		internal static void Ll06jdVUkFrnIUES4bg()
		{
		}
	}
}
