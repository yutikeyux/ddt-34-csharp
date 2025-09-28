using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MetroFramework.Drawing;
using MetroFramework.Interfaces;

namespace MetroFramework.Components
{
	[ProvideProperty("ApplyMetroTheme", typeof(Control))]
	public sealed class MetroStyleExtender : Component, IExtenderProvider, IMetroComponent
	{
		private MetroThemeStyle metroThemeStyle_0;

		private MetroStyleManager metroStyleManager_0;

		private readonly List<Control> list_0 = new List<Control>();

		internal static MetroStyleExtender cN11DfXlwARfm8MdQeW;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public MetroColorStyle Style
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
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
				method_0();
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
				method_0();
			}
		}

		public MetroStyleExtender()
		{
		}

		public MetroStyleExtender(IContainer parent)
			: this()
		{
			parent?.Add(this);
		}

		private void method_0()
		{
			Color backColor = MetroPaint.BackColor.Form(Theme);
			Color foreColor = MetroPaint.ForeColor.Label.Normal(Theme);
			foreach (Control item in list_0)
			{
				if (item != null)
				{
					try
					{
						item.BackColor = backColor;
					}
					catch
					{
					}
					try
					{
						item.ForeColor = foreColor;
					}
					catch
					{
					}
				}
			}
		}

		bool IExtenderProvider.CanExtend(object target)
		{
			if (!(target is Control))
			{
				return false;
			}
			if (target is IMetroControl)
			{
				return false;
			}
			return !(target is IMetroForm);
		}

		[Category("Metro Appearance")]
		[Description("Apply Metro Theme BackColor and ForeColor.")]
		[DefaultValue(false)]
		public bool GetApplyMetroTheme(Control control)
		{
			if (control == null)
			{
				return false;
			}
			return list_0.Contains(control);
		}

		public void SetApplyMetroTheme(Control control, bool value)
		{
			if (control == null)
			{
				return;
			}
			if (!list_0.Contains(control))
			{
				if (value)
				{
					list_0.Add(control);
				}
			}
			else if (!value)
			{
				list_0.Remove(control);
			}
		}

		internal static bool QJSZ7GXpmFFbKwDZGiN()
		{
			return cN11DfXlwARfm8MdQeW == null;
		}

		internal static void UyRY7fXtVKJE7gNnx6r()
		{
		}
	}
}
