using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace BunifuAnimatorNS
{
	public class AnimationCompletedEventArg : EventArgs
	{
		[CompilerGenerated]
		private Animation vJlUkqpdAe;

		[CompilerGenerated]
		private Control control_0;

		[CompilerGenerated]
		private AnimateMode animateMode_0;

		private static AnimationCompletedEventArg bwQdMbILKaSsCh7aBnTN;

		public Animation Animation
		{
			[CompilerGenerated]
			get
			{
				return vJlUkqpdAe;
			}
			[CompilerGenerated]
			set
			{
				vJlUkqpdAe = value;
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

		internal static void NMuKj5ILZIfQRdiFQOE2()
		{
		}

		internal static bool OmSoX5ILv4GJj39nl9W6()
		{
			return bwQdMbILKaSsCh7aBnTN == null;
		}
	}
}
