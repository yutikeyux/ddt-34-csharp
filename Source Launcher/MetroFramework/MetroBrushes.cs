using System.Collections.Generic;
using System.Drawing;

namespace MetroFramework
{
	public sealed class MetroBrushes
	{
		private static Dictionary<string, SolidBrush> dictionary_0;

		internal static MetroBrushes dmD4prTH8c23nOxVHjj;

		public static SolidBrush Black => smethod_0("Black", MetroColors.Black);

		public static SolidBrush White => smethod_0("White", MetroColors.White);

		public static SolidBrush Silver => smethod_0("Silver", MetroColors.Silver);

		public static SolidBrush Blue => smethod_0("Blue", MetroColors.Blue);

		public static SolidBrush Green => smethod_0("Green", MetroColors.Green);

		public static SolidBrush Lime => smethod_0("Lime", MetroColors.Lime);

		public static SolidBrush Teal => smethod_0("Teal", MetroColors.Teal);

		public static SolidBrush Orange => smethod_0("Orange", MetroColors.Orange);

		public static SolidBrush Brown => smethod_0("Brown", MetroColors.Brown);

		public static SolidBrush Pink => smethod_0("Pink", MetroColors.Pink);

		public static SolidBrush Magenta => smethod_0("Magenta", MetroColors.Magenta);

		public static SolidBrush Purple => smethod_0("Purple", MetroColors.Purple);

		public static SolidBrush Red => smethod_0("Red", MetroColors.Red);

		public static SolidBrush Yellow => smethod_0("Yellow", MetroColors.Yellow);

		private static SolidBrush smethod_0(object object_0, Color color_0)
		{
			lock (dictionary_0)
			{
				if (!dictionary_0.ContainsKey((string)object_0))
				{
					dictionary_0.Add((string)object_0, new SolidBrush(color_0));
				}
				return dictionary_0[(string)object_0].Clone() as SolidBrush;
			}
		}

		static MetroBrushes()
		{
			dictionary_0 = new Dictionary<string, SolidBrush>();
		}

		internal static bool PSZmnKTlmu2dyrqU0hN()
		{
			return dmD4prTH8c23nOxVHjj == null;
		}

		internal static void WFoJxNTiSMdYk7yZvKl()
		{
		}
	}
}
