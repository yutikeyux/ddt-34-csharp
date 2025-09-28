using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using MetroFramework.Components;
using MetroFramework.Drawing;
using MetroFramework.Interfaces;

namespace MetroFramework.Controls
{
	[ToolboxBitmap(typeof(ProgressBar))]
	[Designer("MetroFramework.Design.Controls.MetroProgressSpinnerDesigner, MetroFramework.Design, Version=1.4.0.0, Culture=neutral, PublicKeyToken=5f91a84759bf584a")]
	public class MetroProgressSpinner : Control, IMetroControl
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

		private Timer timer_0;

		private int int_0;

		private float float_0 = 270f;

		private int int_1;

		private int int_2 = 100;

		private bool bool_3 = true;

		private float float_1;

		private bool bool_4;

		private bool bool_5;

		private static MetroProgressSpinner vRHX6FVoaFNbBe003YD;

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

		[DefaultValue(false)]
		[Browsable(false)]
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

		[Category("Metro Behaviour")]
		[DefaultValue(true)]
		public bool Spinning
		{
			get
			{
				return timer_0.Enabled;
			}
			set
			{
				timer_0.Enabled = value;
			}
		}

		[DefaultValue(0)]
		[Category("Metro Appearance")]
		public int Value
		{
			get
			{
				return int_0;
			}
			set
			{
				if (value != -1 && (value < int_1 || value > int_2))
				{
					throw new ArgumentOutOfRangeException("Progress value must be -1 or between Minimum and Maximum.", (Exception)null);
				}
				int_0 = value;
				Refresh();
			}
		}

		[Category("Metro Appearance")]
		[DefaultValue(0)]
		public int Minimum
		{
			get
			{
				return int_1;
			}
			set
			{
				if (value >= 0)
				{
					if (value >= int_2)
					{
						throw new ArgumentOutOfRangeException("Minimum value must be < Maximum.", (Exception)null);
					}
					int_1 = value;
					if (int_0 != -1 && int_0 < int_1)
					{
						int_0 = int_1;
					}
					Refresh();
					return;
				}
				throw new ArgumentOutOfRangeException("Minimum value must be >= 0.", (Exception)null);
			}
		}

		[DefaultValue(0)]
		[Category("Metro Appearance")]
		public int Maximum
		{
			get
			{
				return int_2;
			}
			set
			{
				if (value <= int_1)
				{
					throw new ArgumentOutOfRangeException("Maximum value must be > Minimum.", (Exception)null);
				}
				int_2 = value;
				if (int_0 > int_2)
				{
					int_0 = int_2;
				}
				Refresh();
			}
		}

		[Category("Metro Appearance")]
		[DefaultValue(true)]
		public bool EnsureVisible
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

		[Category("Metro Behaviour")]
		[DefaultValue(1f)]
		public float Speed
		{
			get
			{
				return float_1;
			}
			set
			{
				if (value <= 0f || value > 10f)
				{
					throw new ArgumentOutOfRangeException("Speed value must be > 0 and <= 10.", (Exception)null);
				}
				float_1 = value;
			}
		}

		[Category("Metro Behaviour")]
		[DefaultValue(false)]
		public bool Backwards
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

		[Category("Metro Appearance")]
		[DefaultValue(false)]
		public bool CustomBackground
		{
			get
			{
				return bool_5;
			}
			set
			{
				bool_5 = value;
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

		public MetroProgressSpinner()
		{
			timer_0 = new Timer();
			timer_0.Interval = 20;
			timer_0.Tick += timer_0_Tick;
			timer_0.Enabled = true;
			base.Width = 16;
			base.Height = 16;
			float_1 = 1f;
			DoubleBuffered = true;
		}

		public void Reset()
		{
			int_0 = int_1;
			float_0 = 270f;
			Refresh();
		}

		private void timer_0_Tick(object sender, EventArgs e)
		{
			if (!base.DesignMode)
			{
				float_0 += 6f * float_1 * (float)((!bool_4) ? 1 : (-1));
				Refresh();
			}
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			try
			{
				Color color = BackColor;
				if (!bool_0)
				{
					color = ((base.Parent is MetroTile) ? MetroPaint.GetStyleColor(Style) : MetroPaint.BackColor.Form(Theme));
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
			Color color = (bool_5 ? MetroPaint.GetStyleColor(Style) : ((base.Parent is MetroTile) ? MetroPaint.ForeColor.Tile.Normal(Theme) : MetroPaint.GetStyleColor(Style)));
			using (Pen pen = new Pen(color, (float)base.Width / 5f))
			{
				int num = (int)Math.Ceiling((float)base.Width / 10f);
				e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
				if (int_0 != -1)
				{
					float num2 = (float)(int_0 - int_1) / (float)(int_2 - int_1);
					float num3 = ((!bool_3) ? (360f * num2) : (30f + 300f * num2));
					if (bool_4)
					{
						num3 = 0f - num3;
					}
					e.Graphics.DrawArc(pen, num, num, base.Width - 2 * num - 1, base.Height - 2 * num - 1, float_0, num3);
				}
				else
				{
					for (int i = 0; i <= 180; i += 15)
					{
						int num4 = 290 - i * 290 / 180;
						if (num4 > 255)
						{
							num4 = 255;
						}
						if (num4 < 0)
						{
							num4 = 0;
						}
						Color color2 = Color.FromArgb(num4, pen.Color);
						using Pen pen2 = new Pen(color2, pen.Width);
						float startAngle = float_0 + (float)((i - (bool_3 ? 30 : 0)) * (bool_4 ? 1 : (-1)));
						float sweepAngle = 15 * (bool_4 ? 1 : (-1));
						e.Graphics.DrawArc(pen2, num, num, base.Width - 2 * num - 1, base.Height - 2 * num - 1, startAngle, sweepAngle);
					}
				}
			}
			OnCustomPaintForeground(new MetroPaintEventArgs(Color.Empty, color, e.Graphics));
		}

		internal static bool FFVDbpVVDUZJLgwklD3()
		{
			return vRHX6FVoaFNbBe003YD == null;
		}

		internal static void YQKQWvVFOjakIJLh2YX()
		{
		}
	}
}
