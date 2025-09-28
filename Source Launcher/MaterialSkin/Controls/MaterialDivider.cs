using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
	public sealed class MaterialDivider : Control, IMaterialControl
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int int_0;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private MouseState mouseState_0;

		internal static MaterialDivider EASsjaIV1TdKdcdwML1D;

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

		public MaterialDivider()
		{
			SetStyle(ControlStyles.SupportsTransparentBackColor, value: true);
			base.Height = 1;
			BackColor = SkinManager.GetDividersColor();
		}

		internal static bool JotdWJIVxFCubKfBJ9bp()
		{
			return EASsjaIV1TdKdcdwML1D == null;
		}

		internal static void OwhQKVIVB5VRXQfYRl2v()
		{
		}
	}
}
