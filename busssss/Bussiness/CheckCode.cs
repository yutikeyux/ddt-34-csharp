using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Bussiness
{
	// Token: 0x0200000A RID: 10
	public class CheckCode
	{
		// Token: 0x06000021 RID: 33 RVA: 0x00003918 File Offset: 0x00001B18
		public static byte[] CreateImage(string randomcode)
		{
			int maxValue = 30;
			Bitmap image = new Bitmap(randomcode.Length * 30, 32);
			Graphics graphics = Graphics.FromImage(image);
			graphics.SmoothingMode = SmoothingMode.HighQuality;
			byte[] result;
			try
			{
				graphics.Clear(Color.Transparent);
				int index = CheckCode.rand.Next(2);
				Brush brush = new SolidBrush(CheckCode.c[index]);
				for (int i = 0; i < 1; i++)
				{
					int num11 = CheckCode.rand.Next(image.Width / 2);
					int num12 = CheckCode.rand.Next(image.Width * 3 / 4, image.Width);
					int num13 = CheckCode.rand.Next(image.Height);
					int num14 = CheckCode.rand.Next(image.Height);
					graphics.DrawBezier(new Pen(CheckCode.c[index], 2f), (float)num11, (float)num13, (float)((num11 + num12) / 4), 0f, (float)((num11 + num12) * 3 / 4), (float)image.Height, (float)num12, (float)num14);
				}
				char[] chArray = randomcode.ToCharArray();
				StringFormat format = new StringFormat(StringFormatFlags.NoClip)
				{
					Alignment = StringAlignment.Center,
					LineAlignment = StringAlignment.Center
				};
				for (int j = 0; j < chArray.Length; j++)
				{
					int num15 = CheckCode.rand.Next(5);
					Font font = new Font(CheckCode.font[num15], 22f, FontStyle.Bold);
					Point point = new Point(16, 16);
					float angle = (float)ThreadSafeRandom.NextStatic(-maxValue, maxValue);
					graphics.TranslateTransform((float)point.X, (float)point.Y);
					graphics.RotateTransform(angle);
					graphics.DrawString(chArray[j].ToString(), font, brush, 1f, 1f, format);
					graphics.RotateTransform(0f - angle);
					graphics.TranslateTransform(2f, 0f - (float)point.Y);
				}
				MemoryStream stream = new MemoryStream();
				image.Save(stream, ImageFormat.Png);
				result = stream.ToArray();
			}
			finally
			{
				graphics.Dispose();
				image.Dispose();
			}
			return result;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00003B68 File Offset: 0x00001D68
		public static string GenerateCheckCode()
		{
			return CheckCode.GenerateRandomString(4, CheckCode.RandomStringMode.Digital);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00003B84 File Offset: 0x00001D84
		private static string GenerateRandomString(int length, CheckCode.RandomStringMode mode)
		{
			string str = string.Empty;
			bool flag = length != 0;
			if (flag)
			{
				switch (mode)
				{
				case CheckCode.RandomStringMode.LowerLetter:
					for (int i = 0; i < length; i++)
					{
						str += CheckCode.lowerLetters[CheckCode.rand.Next(0, CheckCode.lowerLetters.Length)].ToString();
					}
					return str;
				case CheckCode.RandomStringMode.UpperLetter:
					for (int j = 0; j < length; j++)
					{
						str += CheckCode.upperLetters[CheckCode.rand.Next(0, CheckCode.upperLetters.Length)].ToString();
					}
					return str;
				case CheckCode.RandomStringMode.Letter:
					for (int k = 0; k < length; k++)
					{
						str += CheckCode.letters[CheckCode.rand.Next(0, CheckCode.letters.Length)].ToString();
					}
					return str;
				case CheckCode.RandomStringMode.Digital:
					for (int l = 0; l < length; l++)
					{
						str += CheckCode.digitals[CheckCode.rand.Next(0, CheckCode.digitals.Length)].ToString();
					}
					return str;
				default:
					for (int m = 0; m < length; m++)
					{
						str += CheckCode.mix[CheckCode.rand.Next(0, CheckCode.mix.Length)].ToString();
					}
					break;
				}
			}
			return str;
		}

		// Token: 0x04000068 RID: 104
		private static Color[] c = new Color[]
		{
			Color.Gray,
			Color.DimGray
		};

		// Token: 0x04000069 RID: 105
		private static char[] digitals = new char[]
		{
			'1',
			'2',
			'3',
			'4',
			'5',
			'6',
			'7',
			'8',
			'9',
			'0'
		};

		// Token: 0x0400006A RID: 106
		private static string[] font = new string[]
		{
			"Verdana",
			"Terminal",
			"Comic Sans MS",
			"Arial",
			"Tekton Pro"
		};

		// Token: 0x0400006B RID: 107
		private static char[] letters = new char[]
		{
			'a',
			'b',
			'c',
			'd',
			'e',
			'f',
			'g',
			'h',
			'i',
			'j',
			'k',
			'l',
			'm',
			'n',
			'p',
			'q',
			'r',
			's',
			't',
			'u',
			'v',
			'w',
			'x',
			'y',
			'z',
			'A',
			'B',
			'C',
			'D',
			'E',
			'F',
			'G',
			'H',
			'I',
			'J',
			'K',
			'L',
			'M',
			'N',
			'P',
			'Q',
			'R',
			'S',
			'T',
			'U',
			'V',
			'W',
			'X',
			'Y',
			'Z'
		};

		// Token: 0x0400006C RID: 108
		private static char[] lowerLetters = new char[]
		{
			'a',
			'b',
			'c',
			'd',
			'e',
			'f',
			'h',
			'k',
			'm',
			'n',
			'p',
			'q',
			'r',
			's',
			't',
			'u',
			'v',
			'w',
			'x',
			'y',
			'z'
		};

		// Token: 0x0400006D RID: 109
		private static char[] mix = new char[]
		{
			'2',
			'3',
			'4',
			'5',
			'6',
			'7',
			'8',
			'9',
			'a',
			'b',
			'c',
			'd',
			'e',
			'f',
			'h',
			'k',
			'm',
			'n',
			'p',
			'q',
			'r',
			's',
			't',
			'u',
			'v',
			'w',
			'x',
			'y',
			'z',
			'A',
			'B',
			'C',
			'D',
			'E',
			'F',
			'G',
			'H',
			'K',
			'M',
			'N',
			'P',
			'Q',
			'R',
			'S',
			'T',
			'U',
			'V',
			'W',
			'X',
			'Y',
			'Z'
		};

		// Token: 0x0400006E RID: 110
		public static ThreadSafeRandom rand = new ThreadSafeRandom();

		// Token: 0x0400006F RID: 111
		private static char[] upperLetters = new char[]
		{
			'A',
			'B',
			'C',
			'D',
			'E',
			'F',
			'G',
			'H',
			'K',
			'M',
			'N',
			'P',
			'Q',
			'R',
			'S',
			'T',
			'U',
			'V',
			'W',
			'X',
			'Y',
			'Z'
		};

		// Token: 0x02000051 RID: 81
		private enum RandomStringMode
		{
			// Token: 0x040001A1 RID: 417
			LowerLetter,
			// Token: 0x040001A2 RID: 418
			UpperLetter,
			// Token: 0x040001A3 RID: 419
			Letter,
			// Token: 0x040001A4 RID: 420
			Digital,
			// Token: 0x040001A5 RID: 421
			Mix
		}
	}
}
