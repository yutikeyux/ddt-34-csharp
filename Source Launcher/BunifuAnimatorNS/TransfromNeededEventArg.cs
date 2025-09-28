using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace BunifuAnimatorNS
{
	public class TransfromNeededEventArg : EventArgs
	{
		[CompilerGenerated]
		private Matrix matrix_0;

		[CompilerGenerated]
		private float float_0;

		[CompilerGenerated]
		private Rectangle rectangle_0;

		[CompilerGenerated]
		private Rectangle rectangle_1;

		[CompilerGenerated]
		private Animation animation_0;

		[CompilerGenerated]
		private Control control_0;

		[CompilerGenerated]
		private AnimateMode animateMode_0;

		[CompilerGenerated]
		private bool bool_0;

		private static TransfromNeededEventArg bEd0GqILdrpkdYwmtqt1;

		public Matrix Matrix
		{
			[CompilerGenerated]
			get
			{
				return matrix_0;
			}
			[CompilerGenerated]
			set
			{
				matrix_0 = value;
			}
		}

		public float CurrentTime
		{
			[CompilerGenerated]
			get
			{
				return float_0;
			}
			[CompilerGenerated]
			internal set
			{
				float_0 = value;
			}
		}

		public Rectangle ClientRectangle
		{
			[CompilerGenerated]
			get
			{
				return rectangle_0;
			}
			[CompilerGenerated]
			internal set
			{
				rectangle_0 = value;
			}
		}

		public Rectangle ClipRectangle
		{
			[CompilerGenerated]
			get
			{
				return rectangle_1;
			}
			[CompilerGenerated]
			internal set
			{
				rectangle_1 = value;
			}
		}

		public Animation Animation
		{
			[CompilerGenerated]
			get
			{
				return animation_0;
			}
			[CompilerGenerated]
			set
			{
				animation_0 = value;
			}
		}

		public Control Control
		{
			[CompilerGenerated]
			get
			{
				return control_0;
			}
			[CompilerGenerated]
			internal set
			{
				control_0 = value;
			}
		}

		public AnimateMode Mode
		{
			[CompilerGenerated]
			get
			{
				return animateMode_0;
			}
			[CompilerGenerated]
			internal set
			{
				animateMode_0 = value;
			}
		}

		public bool UseDefaultMatrix
		{
			[CompilerGenerated]
			get
			{
				return bool_0;
			}
			[CompilerGenerated]
			set
			{
				bool_0 = value;
			}
		}

		public TransfromNeededEventArg()
		{
			Matrix = new Matrix(1f, 0f, 0f, 1f, 0f, 0f);
		}

		internal static bool nguvMsILiGfMRvRxw0P3()
		{
			return bEd0GqILdrpkdYwmtqt1 == null;
		}
	}
}
