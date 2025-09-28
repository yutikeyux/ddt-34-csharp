using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Bunifu.Framework.Lib
{
	[DebuggerStepThrough]
	public static class Elipse
	{
		//private static Elipse rj9D42IgdLNirA4pcZYu;

		[DllImport("Gdi32.dll")]
		private static extern IntPtr CreateRoundRectRgn(int int_0, int int_1, int int_2, int int_3, int int_4, int int_5);

		public static void Apply(Form Form, int _Elipse)
		{
			try
			{
				Form.FormBorderStyle = FormBorderStyle.None;
				Form.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Form.Width, Form.Height, _Elipse, _Elipse));
			}
			catch (Exception)
			{
			}
		}

		public static void Apply(Control ctrl, int Elipse)
		{
			try
			{
				ctrl.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, ctrl.Width, ctrl.Height, Elipse, Elipse));
			}
			catch (Exception)
			{
			}
		}

		//internal static bool GiAmH8IgiejXK1UUcwri()
		//{
		//	return rj9D42IgdLNirA4pcZYu == null;
		//}
	}
}
