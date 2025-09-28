using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
	public class MaterialTabControl : TabControl, IMaterialControl
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private int int_0;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private MouseState mouseState_0;

		private static MaterialTabControl m6s7JcInlACWfTj4x4V1;

		[Browsable(false)]
		public int Depth
		{
			[CompilerGenerated]
			get
			{
				return int_0;
			}
			[CompilerGenerated]
			set
			{
				int_0 = value;
			}
		}

		[Browsable(false)]
		public MaterialSkinManager SkinManager => MaterialSkinManager.Instance;

		[Browsable(false)]
		public MouseState MouseState
		{
			[CompilerGenerated]
			get
			{
				return mouseState_0;
			}
			[CompilerGenerated]
			set
			{
				mouseState_0 = value;
			}
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg != 4904 || base.DesignMode)
			{
				base.WndProc(ref m);
			}
			else
			{
				m.Result = (IntPtr)1;
			}
		}

		internal static bool o8qEMNInpPkxg6wlEjsi()
		{
			return m6s7JcInlACWfTj4x4V1 == null;
		}

		internal static void MAc3neIntNrQljCJsqCN()
		{
		}
	}
}
