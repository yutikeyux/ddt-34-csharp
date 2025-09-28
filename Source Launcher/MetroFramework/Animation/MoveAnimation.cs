using System.Drawing;
using System.Windows.Forms;

namespace MetroFramework.Animation
{
	public sealed class MoveAnimation : AnimationBase
	{
		internal static MoveAnimation tcHGaRXei2l7xyH5oKR;

		public void Start(Control control, Point targetPoint, TransitionType transitionType, int duration)
		{
			Start(control, transitionType, duration, delegate
			{
				int x = method_2(control.Location.X, targetPoint.X);
				int y = method_2(control.Location.Y, targetPoint.Y);
				control.Location = new Point(x, y);
			}, () => control.Location.Equals(targetPoint));
		}

		private int method_2(int int_0, int int_1)
		{
			float t = (float)counter - (float)startTime;
			float b = int_0;
			float c = (float)int_1 - (float)int_0;
			float d = (float)targetTime - (float)startTime;
			return MakeTransition(t, b, d, c);
		}

		internal static bool HQmav4XMejBYZ68Saey()
		{
			return tcHGaRXei2l7xyH5oKR == null;
		}

		internal static void BLcSu6XV9LZmWSfIi87()
		{
		}
	}
}
