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
	public class MetroContextMenu : ContextMenuStrip, IMetroControl
	{
		private class Class16 : ToolStripProfessionalRenderer
		{
			private MetroThemeStyle metroThemeStyle_0;

			private static object sXC6BxefIsgWxsvSFZc;

			public Class16(MetroThemeStyle metroThemeStyle_1, MetroColorStyle metroColorStyle_0)
				: base(new Class17(metroThemeStyle_1, metroColorStyle_0))
			{
				metroThemeStyle_0 = metroThemeStyle_1;
			}

			protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
			{
				e.TextColor = MetroPaint.ForeColor.Button.Normal(metroThemeStyle_0);
				base.OnRenderItemText(e);
			}

			internal static void xhJ7FWemBbkSStTUxf5()
			{
			}

			internal static bool Lx5d4geUK3Rl4QEOJdi()
			{
				return sXC6BxefIsgWxsvSFZc == null;
			}
		}

		private class Class17 : ProfessionalColorTable
		{
			private MetroThemeStyle metroThemeStyle_0 = MetroThemeStyle.Light;

			private MetroColorStyle metroColorStyle_0 = MetroColorStyle.Blue;

			private static object abhyvReg5SiAmPstQWi;

			public override Color MenuItemSelected => MetroPaint.GetStyleColor(metroColorStyle_0);

			public override Color MenuBorder => MetroPaint.BackColor.Form(metroThemeStyle_0);

			public override Color ToolStripBorder => MetroPaint.GetStyleColor(metroColorStyle_0);

			public override Color MenuItemBorder => MetroPaint.GetStyleColor(metroColorStyle_0);

			public override Color ToolStripDropDownBackground => MetroPaint.BackColor.Form(metroThemeStyle_0);

			public override Color ImageMarginGradientBegin => MetroPaint.BackColor.Form(metroThemeStyle_0);

			public override Color ImageMarginGradientMiddle => MetroPaint.BackColor.Form(metroThemeStyle_0);

			public override Color ImageMarginGradientEnd => MetroPaint.BackColor.Form(metroThemeStyle_0);

			public Class17(MetroThemeStyle metroThemeStyle_1, MetroColorStyle metroColorStyle_1)
			{
				metroThemeStyle_0 = metroThemeStyle_1;
				metroColorStyle_0 = metroColorStyle_1;
			}

			internal static bool ixk2Gbeqrynuf2Oa6Bk()
			{
				return abhyvReg5SiAmPstQWi == null;
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

		internal static MetroContextMenu UIMyDeeIfaxig6Or9cC;

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
				method_0();
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

		public MetroContextMenu(IContainer Container)
		{
			Container?.Add(this);
		}

		private void method_0()
		{
			base.BackColor = MetroPaint.BackColor.Form(Theme);
			base.ForeColor = MetroPaint.ForeColor.Button.Normal(Theme);
			base.Renderer = new Class16(Theme, Style);
		}

		internal static bool H0axaae1ba28Rd0GEwy()
		{
			return UIMyDeeIfaxig6Or9cC == null;
		}

		internal static void tZcPFpeBc7d0Thd34kZ()
		{
		}
	}
}
