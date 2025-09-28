using System;
using System.Windows.Forms;

namespace ns1
{
	public class Drag : Form
	{
		private bool bool_0;

		private int int_0;

		private int int_1;

		private Control control_0;

		internal static Drag ESfMHqIgMN5bZ4ME4es3;

		public void Grab(Control a)
		{
			try
			{
				control_0 = a;
				bool_0 = true;
				int_0 = Control.MousePosition.X - control_0.Left;
				int_1 = Control.MousePosition.Y - control_0.Top;
			}
			catch (Exception)
			{
			}
		}

		public void Release()
		{
			bool_0 = false;
		}

		public void MoveObject(bool Horizontal = true, bool Vertical = true)
		{
			try
			{
				if (bool_0)
				{
					int num = Control.MousePosition.X;
					int num2 = Control.MousePosition.Y;
					if (Vertical)
					{
						control_0.Top = num2 - int_1;
					}
					if (Horizontal)
					{
						control_0.Left = num - int_0;
					}
				}
			}
			catch (Exception)
			{
			}
		}

		internal static void coFx5xIgnhaCGGblEWD3()
		{
		}

		internal static bool knjsCkIgo5ZWkIKKXUAJ()
		{
			return ESfMHqIgMN5bZ4ME4es3 == null;
		}
	}
}
