using System.Collections.Generic;
using System.Drawing;

namespace MetroFramework
{
	public sealed class MetroPens
	{
		private static Dictionary<string, Pen> dictionary_0;

		private static MetroPens KgGD0O8efSD2YRUUcVI;

		public static Pen Black => smethod_0("Black", MetroColors.Black);

		public static Pen White => smethod_0("White", MetroColors.White);

		public static Pen Silver => smethod_0("Silver", MetroColors.Silver);

		public static Pen Blue => smethod_0("Blue", MetroColors.Blue);

		public static Pen Green => smethod_0("Green", MetroColors.Green);

		public static Pen Lime => smethod_0("Lime", MetroColors.Lime);

		public static Pen Teal => smethod_0("Teal", MetroColors.Teal);

		public static Pen Orange => smethod_0("Orange", MetroColors.Orange);

		public static Pen Brown => smethod_0("Brown", MetroColors.Brown);

		public static Pen Pink => smethod_0("Pink", MetroColors.Pink);

		public static Pen Magenta => smethod_0("Magenta", MetroColors.Magenta);

		public static Pen Purple => smethod_0("Purple", MetroColors.Purple);

		public static Pen Red => smethod_0("Red", MetroColors.Red);

		public static Pen Yellow => smethod_0("Yellow", MetroColors.Yellow);

		private static Pen smethod_0(object object_0, Color color_0)
		{
			lock (dictionary_0)
			{
				if (!dictionary_0.ContainsKey((string)object_0))
				{
					dictionary_0.Add((string)object_0, new Pen(color_0, 1f));
				}
				return dictionary_0[(string)object_0].Clone() as Pen;
			}
		}

		static MetroPens()
		{
			dictionary_0 = new Dictionary<string, Pen>();
		}

		internal static bool jZnywS8ML8v6lZFPQnJ()
		{
			return KgGD0O8efSD2YRUUcVI == null;
		}

		internal static void LkL93F8TNDkB7M3Oey9()
		{
		}
	}
}
