using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using MetroFramework.Controls;
using MetroFramework.Native;

namespace MetroFramework.Design.Controls
{
	internal class MetroTabControlDesigner : ParentControlDesigner
	{
		private readonly DesignerVerbCollection designerVerbCollection_0 = new DesignerVerbCollection();

		private IDesignerHost idesignerHost_0;

		private ISelectionService iselectionService_0;

		internal static MetroTabControlDesigner DK0RAxNffOJ3Yl3bBy9;

		public override SelectionRules SelectionRules
		{
			get
			{
				if (Control.Dock != DockStyle.Fill)
				{
					return base.SelectionRules;
				}
				return SelectionRules.Visible;
			}
		}

		public override DesignerVerbCollection Verbs
		{
			get
			{
				if (designerVerbCollection_0.Count == 2)
				{
					MetroTabControl metroTabControl = (MetroTabControl)Control;
					designerVerbCollection_0[1].Enabled = metroTabControl.TabCount != 0;
				}
				return designerVerbCollection_0;
			}
		}

		public IDesignerHost DesignerHost => idesignerHost_0 ?? (idesignerHost_0 = (IDesignerHost)GetService(Type.GetTypeFromHandle((RuntimeTypeHandle)typeof(IDesignerHost).TypeHandle)));

		public ISelectionService SelectionService => iselectionService_0 ?? (iselectionService_0 = (ISelectionService)GetService(Type.GetTypeFromHandle((RuntimeTypeHandle)typeof(ISelectionService).TypeHandle)));

		public MetroTabControlDesigner()
		{
			DesignerVerb designerVerb = new DesignerVerb("Add Tab", method_0);
			DesignerVerb designerVerb2 = new DesignerVerb("Remove Tab", method_1);
			designerVerbCollection_0.AddRange(new DesignerVerb[2] { designerVerb, designerVerb2 });
		}

		private void method_0(object sender, EventArgs e)
		{
			MetroTabControl metroTabControl = (MetroTabControl)Control;
			Control.ControlCollection oldValue = metroTabControl.Controls;
			RaiseComponentChanging(TypeDescriptor.GetProperties(metroTabControl)["TabPages"]);
			MetroTabPage metroTabPage = (MetroTabPage)DesignerHost.CreateComponent(Type.GetTypeFromHandle((RuntimeTypeHandle)typeof(MetroTabPage).TypeHandle));
			metroTabPage.Text = metroTabPage.Name;
			metroTabControl.TabPages.Add(metroTabPage);
			RaiseComponentChanged(TypeDescriptor.GetProperties(metroTabControl)["TabPages"], oldValue, metroTabControl.TabPages);
			metroTabControl.SelectedTab = metroTabPage;
			method_2();
		}

		private void method_1(object sender, EventArgs e)
		{
			MetroTabControl metroTabControl = (MetroTabControl)Control;
			Control.ControlCollection oldValue = metroTabControl.Controls;
			if (metroTabControl.SelectedIndex >= 0)
			{
				RaiseComponentChanging(TypeDescriptor.GetProperties(metroTabControl)["TabPages"]);
				DesignerHost.DestroyComponent(metroTabControl.TabPages[metroTabControl.SelectedIndex]);
				RaiseComponentChanged(TypeDescriptor.GetProperties(metroTabControl)["TabPages"], oldValue, metroTabControl.TabPages);
				SelectionService.SetSelectedComponents((ICollection)(object)new IComponent[1] { metroTabControl }, SelectionTypes.Auto);
				method_2();
			}
		}

		private void method_2()
		{
			MetroTabControl metroTabControl = (MetroTabControl)Control;
			if (metroTabControl.TabPages.Count == 0)
			{
				Verbs[1].Enabled = false;
			}
			else
			{
				Verbs[1].Enabled = true;
			}
		}

		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);
			int msg = m.Msg;
			if (msg == 132 && ((IntPtr)(nint)m.Result).ToInt32() == -1)
			{
				m.Result = (IntPtr)1L;
			}
		}

		protected override bool GetHitTest(Point point)
		{
			if (SelectionService.PrimarySelection == Control)
			{
				WinApi.TCHITTESTINFO tCHITTESTINFO = default(WinApi.TCHITTESTINFO);
				tCHITTESTINFO.pt = Control.PointToClient(point);
				tCHITTESTINFO.flags = 0u;
				WinApi.TCHITTESTINFO tCHITTESTINFO2 = tCHITTESTINFO;
				Message message = default(Message);
				message.HWnd = Control.Handle;
				message.Msg = 4883;
				Message m = message;
				IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(tCHITTESTINFO2));
				Marshal.StructureToPtr(tCHITTESTINFO2, intPtr, fDeleteOld: false);
				m.LParam = intPtr;
				base.WndProc(ref m);
				Marshal.FreeHGlobal(intPtr);
				if (((IntPtr)(nint)m.Result).ToInt32() != -1)
				{
					return tCHITTESTINFO2.flags != 1;
				}
			}
			return false;
		}

		protected override void PreFilterProperties(IDictionary properties)
		{
			properties.Remove("ImeMode");
			properties.Remove("Padding");
			properties.Remove("FlatAppearance");
			properties.Remove("FlatStyle");
			properties.Remove("AutoEllipsis");
			properties.Remove("UseCompatibleTextRendering");
			properties.Remove("Image");
			properties.Remove("ImageAlign");
			properties.Remove("ImageIndex");
			properties.Remove("ImageKey");
			properties.Remove("ImageList");
			properties.Remove("TextImageRelation");
			properties.Remove("BackgroundImage");
			properties.Remove("BackgroundImageLayout");
			properties.Remove("UseVisualStyleBackColor");
			properties.Remove("Font");
			properties.Remove("RightToLeft");
			base.PreFilterProperties(properties);
		}

		internal static bool aRZLTBNUag0TSbqNDkG()
		{
			return DK0RAxNffOJ3Yl3bBy9 == null;
		}
	}
}
