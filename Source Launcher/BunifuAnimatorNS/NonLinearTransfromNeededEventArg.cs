using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace BunifuAnimatorNS
{
	public class NonLinearTransfromNeededEventArg : EventArgs
	{
		[CompilerGenerated]
		private float float_0;

		[CompilerGenerated]
		private Rectangle rectangle_0;

		[CompilerGenerated]
		private byte[] byte_0;

		[CompilerGenerated]
		private int int_0;

		[CompilerGenerated]
		private Rectangle rectangle_1;

		[CompilerGenerated]
		private byte[] byte_1;

		[CompilerGenerated]
		private int int_1;

		[CompilerGenerated]
		private Animation animation_0;

		[CompilerGenerated]
		private Control control_0;

		[CompilerGenerated]
		private AnimateMode animateMode_0;

		[CompilerGenerated]
		private bool bool_0;

		internal static NonLinearTransfromNeededEventArg rCA7jvIL0Gwm8GHq6qi8;

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

		public byte[] Pixels
		{
			[CompilerGenerated]
			get
			{
				return byte_0;
			}
			[CompilerGenerated]
			internal set
			{
				byte_0 = value;
			}
		}

		public int Stride
		{
			[CompilerGenerated]
			get
			{
				return int_0;
			}
			[CompilerGenerated]
			internal set
			{
				int_0 = value;
			}
		}

		public Rectangle SourceClientRectangle
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

		public byte[] SourcePixels
		{
			[CompilerGenerated]
			get
			{
				return byte_1;
			}
			[CompilerGenerated]
			internal set
			{
				byte_1 = value;
			}
		}

		public int SourceStride
		{
			[CompilerGenerated]
			get
			{
				return int_1;
			}
			[CompilerGenerated]
			set
			{
				int_1 = value;
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

		public bool UseDefaultTransform
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

		internal static void R0qFcGIL58iUTKWx0ruv()
		{
		}

		internal static bool k6NDNBILubZnoPyjnhaJ()
		{
			return rCA7jvIL0Gwm8GHq6qi8 == null;
		}
	}
}
