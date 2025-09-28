using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace ns0
{
	[DebuggerStepThrough]
	internal class Class7
	{
		internal static Class7 zurxZUIg0VKD3qQTovRq;

		public static Image smethod_0(Image image_0, Color color_0, Color color_1)
		{
			Bitmap bitmap = new Bitmap(image_0);
			for (int i = 0; i < bitmap.Height; i++)
			{
				for (int j = 0; j < bitmap.Width; j++)
				{
					if (!smethod_4(bitmap.GetPixel(j, i)))
					{
						bitmap.SetPixel(j, i, color_1);
					}
				}
			}
			return bitmap;
		}

		public static Image smethod_1(Image image_0, Color color_0)
		{
			Bitmap bitmap = new Bitmap(image_0);
			for (int i = 0; i < bitmap.Height; i++)
			{
				for (int j = 0; j < bitmap.Width; j++)
				{
					if (!smethod_4(bitmap.GetPixel(j, i)))
					{
						bitmap.SetPixel(j, i, color_0);
					}
				}
			}
			return bitmap;
		}

		public static Image smethod_2(Image image_0)
		{
			Bitmap bitmap = new Bitmap(image_0);
			List<int[]> list = new List<int[]>();
			for (int i = 0; i < bitmap.Height - 1; i++)
			{
				for (int j = 0; j < bitmap.Width - 1; j++)
				{
					Color[] array = new Color[4];
					array[0] = bitmap.GetPixel(j, i);
					array[2] = bitmap.GetPixel(j, i + 1);
					array[1] = bitmap.GetPixel(j + 1, i);
					array[3] = bitmap.GetPixel(j + 1, i + 1);
					if (array[1] == array[2] && !smethod_4(array[1]) && smethod_4(array[0]))
					{
						list.Add(new int[2] { j, i });
					}
					if (array[0] == array[3] && !smethod_4(array[0]) && smethod_4(array[2]))
					{
						list.Add(new int[2]
						{
							j,
							i + 1
						});
					}
					if (array[0] == array[3] && !smethod_4(array[0]) && smethod_4(array[1]))
					{
						list.Add(new int[2]
						{
							j + 1,
							i
						});
					}
					if (array[1] == array[2] && !smethod_4(array[1]) && smethod_4(array[3]))
					{
						list.Add(new int[2]
						{
							j + 1,
							i + 1
						});
					}
				}
			}
			for (int k = 0; k < list.Count; k++)
			{
				bitmap.SetPixel(list[k][0], list[k][1], smethod_3(Color.Yellow, Color.FromArgb(211, 211, 211)));
			}
			return bitmap;
		}

		public static Color smethod_3(Color color_0, Color color_1)
		{
			return Color.FromArgb((color_0.R + color_1.R) / 2, (color_0.G + color_1.G) / 2, (color_0.B + color_1.B) / 2);
		}

		private static bool smethod_4(Color color_0)
		{
			if (color_0.R != 0 || color_0.G != 0)
			{
				return false;
			}
			return color_0.B == 0;
		}

		public static Color smethod_5(int int_0, Color color_0, Color color_1)
		{
			int red = int.Parse(Math.Round((double)(int)color_0.R + (double)((color_1.R - color_0.R) * int_0) * 0.01, 0).ToString());
			int green = int.Parse(Math.Round((double)(int)color_0.G + (double)((color_1.G - color_0.G) * int_0) * 0.01, 0).ToString());
			int blue = int.Parse(Math.Round((double)(int)color_0.B + (double)((color_1.B - color_0.B) * int_0) * 0.01, 0).ToString());
			return Color.FromArgb(255, red, green, blue);
		}

		internal static void tSAmn2Ig5S3YhFj0SU9B()
		{
		}

		internal static bool Yeqj8YIguNKeMvIggEGf()
		{
			return zurxZUIg0VKD3qQTovRq == null;
		}
	}
}
