using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using MetroFramework.Controls;
using MetroFramework.Interfaces;

namespace MetroFramework.Components
{
	[Designer("MetroFramework.Design.Components.MetroStyleManagerDesigner, MetroFramework.Design, Version=1.4.0.0, Culture=neutral, PublicKeyToken=5f91a84759bf584a")]
	public sealed class MetroStyleManager : Component, ICloneable, ISupportInitialize
	{
		private readonly IContainer icontainer_0;

		private MetroColorStyle metroColorStyle_0 = MetroColorStyle.Blue;

		private MetroThemeStyle metroThemeStyle_0 = MetroThemeStyle.Light;

		private ContainerControl containerControl_0;

		private bool bool_0;

		internal static MetroStyleManager JfmYjaXDi0mgNDX1LVG;

		[DefaultValue(MetroColorStyle.Blue)]
		[Category("Metro Appearance")]
		public MetroColorStyle Style
		{
			get
			{
				return metroColorStyle_0;
			}
			set
			{
				if (value == MetroColorStyle.Default)
				{
					value = MetroColorStyle.Blue;
				}
				metroColorStyle_0 = value;
				if (!bool_0)
				{
					Update();
				}
			}
		}

		[DefaultValue(MetroThemeStyle.Light)]
		[Category("Metro Appearance")]
		public MetroThemeStyle Theme
		{
			get
			{
				return metroThemeStyle_0;
			}
			set
			{
				if (value == MetroThemeStyle.Default)
				{
					value = MetroThemeStyle.Light;
				}
				metroThemeStyle_0 = value;
				if (!bool_0)
				{
					Update();
				}
			}
		}

		public ContainerControl Owner
		{
			get
			{
				return containerControl_0;
			}
			set
			{
				if (containerControl_0 != null)
				{
					containerControl_0.ControlAdded -= containerControl_0_ControlAdded;
				}
				containerControl_0 = value;
				if (value != null)
				{
					containerControl_0.ControlAdded += containerControl_0_ControlAdded;
					if (!bool_0)
					{
						method_0(value);
					}
				}
			}
		}

		public MetroStyleManager()
		{
		}

		public MetroStyleManager(IContainer parentContainer)
			: this()
		{
			if (parentContainer != null)
			{
				icontainer_0 = parentContainer;
				icontainer_0.Add(this);
			}
		}

		public object Clone()
		{
			MetroStyleManager metroStyleManager = new MetroStyleManager();
			metroStyleManager.metroThemeStyle_0 = Theme;
			metroStyleManager.metroColorStyle_0 = Style;
			return metroStyleManager;
		}

		public object Clone(ContainerControl owner)
		{
			MetroStyleManager metroStyleManager = Clone() as MetroStyleManager;
			if (owner is IMetroForm)
			{
				metroStyleManager.Owner = owner;
				((IMetroForm)owner).StyleManager = metroStyleManager;
				Type type = owner.GetType();
				FieldInfo field = type.GetField("components", BindingFlags.Instance | BindingFlags.NonPublic);
				if (field != null)
				{
					IContainer container = (IContainer)field.GetValue(owner);
					if (container == null)
					{
						return metroStyleManager;
					}
					{
						foreach (Component component in container.Components)
						{
							if (component is IMetroComponent)
							{
								xYwVxPybhQ((IMetroComponent)component);
							}
							if (component.GetType() == Type.GetTypeFromHandle((RuntimeTypeHandle)typeof(MetroContextMenu).TypeHandle))
							{
								method_1((MetroContextMenu)component);
							}
						}
						return metroStyleManager;
					}
				}
				return metroStyleManager;
			}
			return metroStyleManager;
		}

		void ISupportInitialize.BeginInit()
		{
			bool_0 = true;
		}

		void ISupportInitialize.EndInit()
		{
			bool_0 = false;
			Update();
		}

		private void containerControl_0_ControlAdded(object sender, ControlEventArgs e)
		{
			if (!bool_0)
			{
				method_0(e.Control);
			}
		}

		public void Update()
		{
			if (containerControl_0 != null)
			{
				method_0(containerControl_0);
			}
			if (icontainer_0 == null || icontainer_0.Components == null)
			{
				return;
			}
			foreach (object component in icontainer_0.Components)
			{
				if (component is IMetroComponent)
				{
					xYwVxPybhQ((IMetroComponent)component);
				}
				if (((object)component).GetType() == Type.GetTypeFromHandle((RuntimeTypeHandle)typeof(MetroContextMenu).TypeHandle))
				{
					method_1((MetroContextMenu)component);
				}
			}
		}

		private void method_0(Control control_0)
		{
			if (control_0 == null)
			{
				return;
			}
			IMetroControl metroControl = control_0 as IMetroControl;
			if (metroControl != null)
			{
				method_1(metroControl);
			}
			IMetroComponent metroComponent = control_0 as IMetroComponent;
			if (metroComponent != null)
			{
				xYwVxPybhQ(metroComponent);
			}
			TabControl tabControl = control_0 as TabControl;
			if (tabControl != null)
			{
				foreach (TabPage tabPage in ((TabControl)control_0).TabPages)
				{
					method_0(tabPage);
				}
			}
			if (control_0.Controls != null)
			{
				foreach (Control control in control_0.Controls)
				{
					method_0(control);
				}
			}
			if (control_0.ContextMenuStrip != null)
			{
				method_0(control_0.ContextMenuStrip);
			}
			control_0.Refresh();
		}

		private void method_1(IMetroControl imetroControl_0)
		{
			imetroControl_0.StyleManager = this;
		}

		private void xYwVxPybhQ(IMetroComponent imetroComponent_0)
		{
			imetroComponent_0.StyleManager = this;
		}

		internal static bool aAuY4RXhxZblwmpPirU()
		{
			return JfmYjaXDi0mgNDX1LVG == null;
		}

		internal static void UZ2MInXJZ2NO8tWU4Fx()
		{
		}
	}
}
