using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
	public class MaterialProgressBar : ProgressBar, IMaterialControl
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private int int_0;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private MouseState mouseState_0;

		internal static MaterialProgressBar VT6RV4InkdrPuQyf4HS9;

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

		public MaterialProgressBar()
		{
			SetStyle(ControlStyles.UserPaint, value: true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, value: true);
		}

		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			base.SetBoundsCore(x, y, width, 5, specified);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			int num = (int)((double)e.ClipRectangle.Width * ((double)base.Value / (double)base.Maximum));
			e.Graphics.FillRectangle(SkinManager.ColorScheme.PrimaryBrush, 0, 0, num, e.ClipRectangle.Height);
			e.Graphics.FillRectangle(SkinManager.GetDisabledOrHintBrush(), num, 0, e.ClipRectangle.Width, e.ClipRectangle.Height);
		}

		internal static bool ivJ5qBIn5PNIFmaqm8rV()
		{
			return VT6RV4InkdrPuQyf4HS9 == null;
		}
	}
}
