using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace ns0
{
	[DebuggerStepThrough]
	internal static class Class6
	{
		private static int int_0;

		private static int int_1;

		private static int int_2;

		private static int int_3;

		private static bool bool_0;

		private static object ddSKL7IgJ6R7mgDj2VxW;

		static Class6()
		{
			bool_0 = true;
		}

		public static void smethod_0(Control control_0, int int_4)
		{
			if (bool_0)
			{
				int_0 = control_0.Height;
				int_1 = control_0.Width;
				int num = Convert.ToInt32(Math.Round((double)int_4 * 0.01 * (double)int_0) * 0.5);
				int num2 = Convert.ToInt32(Math.Round((double)int_4 * 0.01 * (double)int_1) * 0.5);
				int height = int_0 + num * 2;
				int width = int_1 + num2 * 2;
				int_2 = num;
				int_3 = num2;
				control_0.Width = width;
				control_0.Height = height;
				control_0.Top -= int_2;
				control_0.Left -= int_3;
				bool_0 = false;
			}
		}

		public static void smethod_1(Control control_0)
		{
			if (!bool_0)
			{
				control_0.SuspendLayout();
				control_0.Width = int_1;
				control_0.Left += int_3;
				control_0.Height = int_0;
				control_0.Top += int_2;
				control_0.ResumeLayout();
				bool_0 = true;
			}
		}

		public static void smethod_2(Control control_0)
		{
			smethod_1(control_0);
		}

		public static void smethod_3(Control control_0)
		{
		}

		internal static void hLWGu8Igvl61n9bVDiOn()
		{
		}

		internal static bool WP8CqFIg8NhoUCdNTyhL()
		{
			return ddSKL7IgJ6R7mgDj2VxW == null;
		}
	}
}
