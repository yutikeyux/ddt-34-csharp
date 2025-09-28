using System;

namespace MaterialSkin.Animations
{
	internal static class AnimationEaseInOut
	{
		public static double PI;

		public static double PI_HALF;

		internal static object EnWenSICDwEI8f6HwJvo;

		public static double CalculateProgress(double progress)
		{
			return smethod_0(progress);
		}

		private static double smethod_0(double double_0)
		{
			return double_0 - Math.Sin(double_0 * 2.0 * PI) / (2.0 * PI);
		}

		static AnimationEaseInOut()
		{
			PI = Math.PI;
			PI_HALF = Math.PI / 2.0;
		}

		internal static bool N8RQPbIChCtpMRJ9JgZ4()
		{
			return EnWenSICDwEI8f6HwJvo == null;
		}

		internal static void JbtyK4IC8bm18YN3M7jo()
		{
		}
	}
}
