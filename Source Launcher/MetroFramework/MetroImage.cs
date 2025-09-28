using System;
using System.Drawing;

namespace MetroFramework
{
	internal class MetroImage
	{
		internal static MetroImage T6pKIH8qMfxMk8hJxIJ;

		public static Image ResizeImage(Image imgToResize, Rectangle maxOffset)
		{
			int width = imgToResize.Width;
			int height = imgToResize.Height;
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			num2 = (float)maxOffset.Width / (float)width;
			num3 = (float)maxOffset.Height / (float)height;
			num = ((num3 < num2) ? num3 : num2);
			int thumbWidth = (int)((float)width * num);
			int thumbHeight = (int)((float)height * num);
			return imgToResize.GetThumbnailImage(thumbWidth, thumbHeight, null, IntPtr.Zero);
		}

		internal static bool eMOdUa8LpS2TSi9GB8q()
		{
			return T6pKIH8qMfxMk8hJxIJ == null;
		}

		internal static void sXKxXW8aicbCZMDlj43()
		{
		}
	}
}
