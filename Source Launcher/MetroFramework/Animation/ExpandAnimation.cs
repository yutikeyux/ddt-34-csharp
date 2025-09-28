using System.Drawing;
using System.Windows.Forms;

namespace MetroFramework.Animation
{
	public sealed class ExpandAnimation : AnimationBase
	{
		internal static ExpandAnimation lf5VoFXNF4NJfVIRM1I;

		public void Start(Control control, Size targetSize, TransitionType transitionType, int duration)
		{
			Start(control, transitionType, duration, delegate
			{
				int width = method_2(control.Width, targetSize.Width);
				int height = method_2(control.Height, targetSize.Height);
				control.Size = new Size(width, height);
			}, () => control.Size.Equals(targetSize));
		}

		private int method_2(int int_0, int int_1)
		{
			float t = (float)counter - (float)startTime;
			float b = int_0;
			float c = (float)int_1 - (float)int_0;
			float d = (float)targetTime - (float)startTime;
			return MakeTransition(t, b, d, c);
		}

		internal static bool bk8H8cXX0R0HUbGVmek()
		{
			return lf5VoFXNF4NJfVIRM1I == null;
		}
	}
}
