using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using MetroFramework.Components;
using MetroFramework.Interfaces;

namespace MetroFramework.Design.Components
{
	internal class MetroStyleManagerDesigner : ComponentDesigner
	{
		private DesignerVerbCollection designerVerbCollection_0;

		private IDesignerHost idesignerHost_0;

		private IComponentChangeService icomponentChangeService_0;

		internal static MetroStyleManagerDesigner nh0JSZsfClklAQeASyB;

		public override DesignerVerbCollection Verbs
		{
			get
			{
				if (designerVerbCollection_0 == null)
				{
					designerVerbCollection_0 = new DesignerVerbCollection();
					designerVerbCollection_0.Add(new DesignerVerb("Reset Styles to Default", method_0));
					return designerVerbCollection_0;
				}
				return designerVerbCollection_0;
			}
		}

		public IDesignerHost DesignerHost
		{
			get
			{
				if (idesignerHost_0 != null)
				{
					return idesignerHost_0;
				}
				idesignerHost_0 = (IDesignerHost)GetService(Type.GetTypeFromHandle((RuntimeTypeHandle)typeof(IDesignerHost).TypeHandle));
				return idesignerHost_0;
			}
		}

		public IComponentChangeService ComponentChangeService
		{
			get
			{
				if (icomponentChangeService_0 == null)
				{
					icomponentChangeService_0 = (IComponentChangeService)GetService(Type.GetTypeFromHandle((RuntimeTypeHandle)typeof(IComponentChangeService).TypeHandle));
					return icomponentChangeService_0;
				}
				return icomponentChangeService_0;
			}
		}

		private void method_0(object sender, EventArgs e)
		{
			MetroStyleManager metroStyleManager = base.Component as MetroStyleManager;
			if (metroStyleManager != null && metroStyleManager.Owner == null)
			{
				MessageBox.Show("StyleManager needs the Owner property assigned to before it can reset styles.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else
			{
				method_1(metroStyleManager, metroStyleManager.Owner);
			}
		}

		private void method_1(MetroStyleManager metroStyleManager_0, Control control_0)
		{
			IMetroForm metroForm = control_0 as IMetroForm;
			if (metroForm != null && !object.ReferenceEquals(metroStyleManager_0, metroForm.StyleManager))
			{
				return;
			}
			if (control_0 is IMetroControl)
			{
				method_2(control_0, "Style", MetroColorStyle.Default);
				method_2(control_0, "Theme", MetroThemeStyle.Default);
			}
			else if (control_0 is IMetroComponent)
			{
				method_2(control_0, "Style", MetroColorStyle.Default);
				method_2(control_0, "Theme", MetroThemeStyle.Default);
			}
			if (control_0.ContextMenuStrip != null)
			{
				method_1(metroStyleManager_0, control_0.ContextMenuStrip);
			}
			TabControl tabControl = control_0 as TabControl;
			if (tabControl != null)
			{
				foreach (TabPage tabPage in tabControl.TabPages)
				{
					method_1(metroStyleManager_0, tabPage);
				}
			}
			if (control_0.Controls == null)
			{
				return;
			}
			foreach (Control control in control_0.Controls)
			{
				method_1(metroStyleManager_0, control);
			}
		}

		private void method_2(Control control_0, string string_0, object object_0)
		{
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(control_0)[string_0];
			if (propertyDescriptor != null)
			{
				object value = propertyDescriptor.GetValue(control_0);
				if (!((object)object_0).Equals(value))
				{
					ComponentChangeService.OnComponentChanging(control_0, propertyDescriptor);
					propertyDescriptor.SetValue(control_0, object_0);
					ComponentChangeService.OnComponentChanged(control_0, propertyDescriptor, value, object_0);
				}
			}
		}

		internal static bool dYSrKQsUw226QnPOmrv()
		{
			return nh0JSZsfClklAQeASyB == null;
		}

		internal static void tlVabDsNl92gqyT50ZN()
		{
		}
	}
}
