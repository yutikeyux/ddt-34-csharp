using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
	public class MaterialLabel : Label, IMaterialControl
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private int int_0;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private MouseState mouseState_0;

		internal static MaterialLabel yNpD65IV8GGQGKMDuNcF;

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

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			ForeColor = SkinManager.GetPrimaryTextColor();
			Font = SkinManager.ROBOTO_REGULAR_11;
			base.BackColorChanged += delegate
			{
				ForeColor = SkinManager.GetPrimaryTextColor();
			};
		}

		[CompilerGenerated]
		private void MaterialLabel_BackColorChanged(object sender, EventArgs e)
		{
			ForeColor = SkinManager.GetPrimaryTextColor();
		}

		internal static bool TMcsIjIVKYNT6UTBbQ7T()
		{
			return yNpD65IV8GGQGKMDuNcF == null;
		}

		internal static void rt6qVjIVduvIlaqLU09k()
		{
		}
	}
}
