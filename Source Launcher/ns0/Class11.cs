using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using BunifuAnimatorNS;

namespace ns0
{
	[DebuggerStepThrough]
	internal static class Class11
	{
		private static Random random_0;

		private static object zrL8BpIN1oJp2lLZt73f;

		static Class11()
		{
			random_0 = new Random();
		}

		public static void smethod_0(TransfromNeededEventArg transfromNeededEventArg_0, Animation animation_0)
		{
			Rectangle clientRectangle = transfromNeededEventArg_0.ClientRectangle;
			PointF pointF = new PointF(clientRectangle.Left + clientRectangle.Width / 2, clientRectangle.Top + clientRectangle.Height / 2);
			transfromNeededEventArg_0.Matrix.Translate(pointF.X, pointF.Y);
			float num = 1f - animation_0.ScaleCoeff.X * transfromNeededEventArg_0.CurrentTime;
			float num2 = 1f - animation_0.ScaleCoeff.X * transfromNeededEventArg_0.CurrentTime;
			if (Math.Abs(num) <= 0.001f)
			{
				num = 0.001f;
			}
			if (Math.Abs(num2) <= 0.001f)
			{
				num2 = 0.001f;
			}
			transfromNeededEventArg_0.Matrix.Scale(num, num2);
			transfromNeededEventArg_0.Matrix.Translate(0f - pointF.X, 0f - pointF.Y);
		}

		public static void smethod_1(TransfromNeededEventArg transfromNeededEventArg_0, Animation animation_0)
		{
			float currentTime = transfromNeededEventArg_0.CurrentTime;
			transfromNeededEventArg_0.Matrix.Translate((float)(-transfromNeededEventArg_0.ClientRectangle.Width) * currentTime * animation_0.SlideCoeff.X, (float)(-transfromNeededEventArg_0.ClientRectangle.Height) * currentTime * animation_0.SlideCoeff.Y);
		}

		public static void smethod_2(NonLinearTransfromNeededEventArg nonLinearTransfromNeededEventArg_0, Animation animation_0)
		{
			if (animation_0.BlindCoeff == PointF.Empty)
			{
				return;
			}
			byte[] pixels = nonLinearTransfromNeededEventArg_0.Pixels;
			int width = nonLinearTransfromNeededEventArg_0.ClientRectangle.Width;
			int height = nonLinearTransfromNeededEventArg_0.ClientRectangle.Height;
			int stride = nonLinearTransfromNeededEventArg_0.Stride;
			float x = animation_0.BlindCoeff.X;
			float y = animation_0.BlindCoeff.Y;
			int num = (int)(((float)width * x + (float)height * y) * (1f - nonLinearTransfromNeededEventArg_0.CurrentTime));
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					int num2 = j * stride + i * 4;
					if ((float)i * x + (float)j * y - (float)num >= 0f)
					{
						pixels[num2 + 3] = 0;
					}
				}
			}
		}

		public static void smethod_3(NonLinearTransfromNeededEventArg nonLinearTransfromNeededEventArg_0, Animation animation_0, ref Point[] point_0, ref byte[] byte_0)
		{
			if (animation_0.MosaicCoeff == PointF.Empty || animation_0.MosaicSize == 0)
			{
				return;
			}
			byte[] pixels = nonLinearTransfromNeededEventArg_0.Pixels;
			int width = nonLinearTransfromNeededEventArg_0.ClientRectangle.Width;
			int height = nonLinearTransfromNeededEventArg_0.ClientRectangle.Height;
			int stride = nonLinearTransfromNeededEventArg_0.Stride;
			float currentTime = nonLinearTransfromNeededEventArg_0.CurrentTime;
			int num = pixels.Length;
			float num2 = 1f - nonLinearTransfromNeededEventArg_0.CurrentTime;
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			if (num2 > 1f)
			{
				num2 = 1f;
			}
			float x = animation_0.MosaicCoeff.X;
			float y = animation_0.MosaicCoeff.Y;
			if (point_0 == null)
			{
				point_0 = new Point[pixels.Length];
				for (int i = 0; i < pixels.Length; i++)
				{
					point_0[i] = new Point((int)((double)x * (random_0.NextDouble() - 0.5)), (int)((double)y * (random_0.NextDouble() - 0.5)));
				}
			}
			if (byte_0 == null)
			{
				byte_0 = (byte[])pixels.Clone();
			}
			for (int j = 0; j < num; j += 4)
			{
				pixels[j] = byte.MaxValue;
				pixels[j + 1] = byte.MaxValue;
				pixels[j + 2] = byte.MaxValue;
				pixels[j + 3] = 0;
			}
			int mosaicSize = animation_0.MosaicSize;
			float x2 = animation_0.MosaicShift.X;
			float y2 = animation_0.MosaicShift.Y;
			for (int k = 0; k < height; k++)
			{
				for (int l = 0; l < width; l++)
				{
					int num3 = k / mosaicSize;
					int num4 = l / mosaicSize;
					int num5 = k * stride + l * 4;
					int num6 = num3 * stride + num4 * 4;
					int num7 = l + (int)(currentTime * ((float)point_0[num6].X + (float)num4 * x2));
					int num8 = k + (int)(currentTime * ((float)point_0[num6].Y + (float)num3 * y2));
					if (num7 >= 0 && num7 < width && num8 >= 0 && num8 < height)
					{
						int num9 = num8 * stride + num7 * 4;
						pixels[num9] = byte_0[num5];
						pixels[num9 + 1] = byte_0[num5 + 1];
						pixels[num9 + 2] = byte_0[num5 + 2];
						pixels[num9 + 3] = (byte)((float)(int)byte_0[num5 + 3] * num2);
					}
				}
			}
		}

		public static void smethod_4(NonLinearTransfromNeededEventArg nonLinearTransfromNeededEventArg_0, Animation animation_0)
		{
			if (animation_0.LeafCoeff == 0f)
			{
				return;
			}
			byte[] pixels = nonLinearTransfromNeededEventArg_0.Pixels;
			int width = nonLinearTransfromNeededEventArg_0.ClientRectangle.Width;
			int height = nonLinearTransfromNeededEventArg_0.ClientRectangle.Height;
			int stride = nonLinearTransfromNeededEventArg_0.Stride;
			int num = (int)((float)(width + height) * (1f - nonLinearTransfromNeededEventArg_0.CurrentTime * nonLinearTransfromNeededEventArg_0.CurrentTime));
			int num2 = pixels.Length;
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					int num3 = j * stride + i * 4;
					if (i + j >= num)
					{
						int num4 = num - j;
						int num5 = num - i;
						int num6 = num - i - j;
						if (num6 < -20)
						{
							num6 = -20;
						}
						int num7 = num5 * stride + num4 * 4;
						if (num4 >= 0 && num5 >= 0 && num7 >= 0 && num7 < num2 && pixels[num3 + 3] > 0)
						{
							pixels[num7] = (byte)Math.Min(255, num6 + 250 + (int)pixels[num3] / 10);
							pixels[num7 + 1] = (byte)Math.Min(255, num6 + 250 + (int)pixels[num3 + 1] / 10);
							pixels[num7 + 2] = (byte)Math.Min(255, num6 + 250 + (int)pixels[num3 + 2] / 10);
							pixels[num7 + 3] = 230;
						}
						pixels[num3 + 3] = 0;
					}
				}
			}
		}

		public static void smethod_5(NonLinearTransfromNeededEventArg nonLinearTransfromNeededEventArg_0, Animation animation_0)
		{
			if (animation_0.TransparencyCoeff != 0f)
			{
				float num = 1f - animation_0.TransparencyCoeff * nonLinearTransfromNeededEventArg_0.CurrentTime;
				if (num < 0f)
				{
					num = 0f;
				}
				if (num > 1f)
				{
					num = 1f;
				}
				byte[] pixels = nonLinearTransfromNeededEventArg_0.Pixels;
				for (int i = 0; i < pixels.Length; i += 4)
				{
					pixels[i + 3] = (byte)((float)(int)pixels[i + 3] * num);
				}
			}
		}

		public static void smethod_6(Bitmap bitmap_0, Bitmap bitmap_1)
		{
			Rectangle rect = new Rectangle(0, 0, bitmap_0.Width, bitmap_0.Height);
			BitmapData bitmapData = bitmap_0.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
			IntPtr scan = bitmapData.Scan0;
			BitmapData bitmapData2 = bitmap_1.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			IntPtr scan2 = bitmapData2.Scan0;
			int num = bitmap_0.Width * bitmap_0.Height * 4;
			byte[] array = new byte[num];
			byte[] array2 = new byte[num];
			Marshal.Copy(scan, array, 0, num);
			Marshal.Copy(scan2, array2, 0, num);
			for (int i = 0; i < num; i += 4)
			{
				if (array[i] == array2[i] && array[i + 1] == array2[i + 1] && array[i + 2] == array2[i + 2])
				{
					array[i] = byte.MaxValue;
					array[i + 1] = byte.MaxValue;
					array[i + 2] = byte.MaxValue;
					array[i + 3] = 0;
				}
			}
			Marshal.Copy(array, 0, scan, num);
			bitmap_0.UnlockBits(bitmapData);
			bitmap_1.UnlockBits(bitmapData2);
		}

		public static void smethod_7(TransfromNeededEventArg transfromNeededEventArg_0, Animation animation_0)
		{
			Rectangle clientRectangle = transfromNeededEventArg_0.ClientRectangle;
			PointF pointF = new PointF(clientRectangle.Left + clientRectangle.Width / 2, clientRectangle.Top + clientRectangle.Height / 2);
			transfromNeededEventArg_0.Matrix.Translate(pointF.X, pointF.Y);
			if (transfromNeededEventArg_0.CurrentTime > animation_0.RotateLimit)
			{
				transfromNeededEventArg_0.Matrix.Rotate(360f * (transfromNeededEventArg_0.CurrentTime - animation_0.RotateLimit) * animation_0.RotateCoeff);
			}
			transfromNeededEventArg_0.Matrix.Translate(0f - pointF.X, 0f - pointF.Y);
		}

		public static void smethod_8(NonLinearTransfromNeededEventArg nonLinearTransfromNeededEventArg_0)
		{
			byte[] sourcePixels = nonLinearTransfromNeededEventArg_0.SourcePixels;
			byte[] pixels = nonLinearTransfromNeededEventArg_0.Pixels;
			int stride = nonLinearTransfromNeededEventArg_0.Stride;
			int num = 1;
			int num2 = nonLinearTransfromNeededEventArg_0.SourceClientRectangle.Bottom + 1;
			int height = nonLinearTransfromNeededEventArg_0.ClientRectangle.Height;
			int left = nonLinearTransfromNeededEventArg_0.SourceClientRectangle.Left;
			int right = nonLinearTransfromNeededEventArg_0.SourceClientRectangle.Right;
			int num3 = height - num2;
			for (int i = left; i < right; i++)
			{
				for (int j = num2; j < height; j++)
				{
					int num4 = num2 - 1 - num - (j - num2);
					if (num4 < 0)
					{
						break;
					}
					int num5 = i;
					int num6 = num4 * stride + num5 * 4;
					int num7 = j * stride + i * 4;
					pixels[num7] = sourcePixels[num6];
					pixels[num7 + 1] = sourcePixels[num6 + 1];
					pixels[num7 + 2] = sourcePixels[num6 + 2];
					pixels[num7 + 3] = (byte)((1f - 1f * (float)(j - num2) / (float)num3) * 90f);
				}
			}
		}

		public static void smethod_9(NonLinearTransfromNeededEventArg nonLinearTransfromNeededEventArg_0, int int_1)
		{
			byte[] pixels = nonLinearTransfromNeededEventArg_0.Pixels;
			byte[] sourcePixels = nonLinearTransfromNeededEventArg_0.SourcePixels;
			int stride = nonLinearTransfromNeededEventArg_0.Stride;
			int height = nonLinearTransfromNeededEventArg_0.ClientRectangle.Height;
			int width = nonLinearTransfromNeededEventArg_0.ClientRectangle.Width;
			int num = sourcePixels.Length - 4;
			for (int i = int_1; i < width - int_1; i++)
			{
				for (int j = int_1; j < height - int_1; j++)
				{
					int num2 = j * stride + i * 4;
					int num3 = 0;
					int num4 = 0;
					int num5 = 0;
					int num6 = 0;
					int num7 = 0;
					for (int k = i - int_1; k < i + int_1; k++)
					{
						for (int l = j - int_1; l < j + int_1; l++)
						{
							int num8 = l * stride + k * 4;
							if (num8 >= 0 && num8 < num && sourcePixels[num8 + 3] > 0)
							{
								num5 += sourcePixels[num8];
								num4 += sourcePixels[num8 + 1];
								num3 += sourcePixels[num8 + 2];
								num6 += sourcePixels[num8 + 3];
								num7++;
							}
						}
					}
					if (num2 < num && num7 > 5)
					{
						pixels[num2] = (byte)(num5 / num7);
						pixels[num2 + 1] = (byte)(num4 / num7);
						pixels[num2 + 2] = (byte)(num3 / num7);
						pixels[num2 + 3] = (byte)(num6 / num7);
					}
				}
			}
		}

		internal static bool AiNieXINxvHE0SfoH3M6()
		{
			return zrL8BpIN1oJp2lLZt73f == null;
		}
	}
}
