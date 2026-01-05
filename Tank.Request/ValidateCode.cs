using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Bussiness;

namespace Tank.Request
{
	// Token: 0x02000082 RID: 130
	public class ValidateCode : Page
	{
		// Token: 0x0600024B RID: 587 RVA: 0x00010B48 File Offset: 0x0000ED48
		protected void Page_Load(object sender, EventArgs e)
		{
			string code = CheckCode.GenerateCheckCode();
			byte[] bytes = CheckCode.CreateImage(code);
			base.Response.ClearContent();
			base.Response.ContentType = "image/Gif";
			base.Response.BinaryWrite(bytes);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x00002C85 File Offset: 0x00000E85
		protected void Button1_Click(object sender, EventArgs e)
		{
			this.CreateCheckCodeImage(this.GenerateCheckCode());
		}

		// Token: 0x0600024D RID: 589 RVA: 0x00010B90 File Offset: 0x0000ED90
		private string GenerateCheckCode()
		{
			string checkCode = string.Empty;
			Random random = new Random();
			for (int i = 0; i < 4; i++)
			{
				int number = random.Next();
				checkCode += ((char)(65 + (ushort)(number % 26))).ToString();
			}
			return checkCode;
		}

		// Token: 0x0600024E RID: 590 RVA: 0x00010BE8 File Offset: 0x0000EDE8
		private void CreateCheckCodeImage(string checkCode)
		{
			bool flag = checkCode == null || checkCode.Trim() == string.Empty;
			if (!flag)
			{
				Bitmap image = new Bitmap((int)Math.Ceiling((double)checkCode.Length * 40.5), 44);
				Graphics g = Graphics.FromImage(image);
				try
				{
					Random random = new Random();
					Color color = ValidateCode.colors[random.Next(ValidateCode.colors.Length)];
					g.Clear(Color.Transparent);
					for (int i = 0; i < 2; i++)
					{
						int x = random.Next(image.Width);
						int x2 = random.Next(image.Width);
						int y = random.Next(image.Height);
						int y2 = random.Next(image.Height);
						g.DrawArc(new Pen(color, 2f), -x, -y, image.Width * 2, image.Height, 45, 100);
					}
					Font font = new Font("Arial", 24f, FontStyle.Bold | FontStyle.Italic);
					LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), color, color, 1.2f, true);
					g.DrawString(checkCode, font, brush, 2f, 2f);
					int angle = 40;
					double sin = Math.Sin(3.141592653589793 * (double)angle / 180.0);
					double cos = Math.Cos(3.141592653589793 * (double)angle / 180.0);
					double tan = Math.Atan(3.141592653589793 * (double)angle / 180.0);
					bool flag2 = angle > 0;
					if (flag2)
					{
						int px = (int)(sin * 20.0);
						int py = (int)(-sin * (double)image.Width);
					}
					else
					{
						int py = (int)(-sin * 22.0);
					}
					TextureBrush MyBrush = new TextureBrush(image);
					MyBrush.RotateTransform(30f);
					image.Save("c:\\1.jpg", ImageFormat.Png);
					MemoryStream ms = new MemoryStream();
					image.Save(ms, ImageFormat.Png);
					base.Response.ClearContent();
					base.Response.ContentType = "image/Gif";
					base.Response.BinaryWrite(ms.ToArray());
				}
				finally
				{
					g.Dispose();
					image.Dispose();
				}
			}
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00010E6C File Offset: 0x0000F06C
		public static Bitmap KiRotate(Bitmap bmp, float angle, Color bkColor)
		{
			int w = bmp.Width + 2;
			int h = bmp.Height + 2;
			bool flag = bkColor == Color.Transparent;
			PixelFormat pf;
			if (flag)
			{
				pf = PixelFormat.Format32bppArgb;
			}
			else
			{
				pf = bmp.PixelFormat;
			}
			Bitmap tmp = new Bitmap(w, h, pf);
			Graphics g = Graphics.FromImage(tmp);
			g.Clear(bkColor);
			g.DrawImageUnscaled(bmp, 1, 1);
			g.Dispose();
			GraphicsPath path = new GraphicsPath();
			path.AddRectangle(new RectangleF(0f, 0f, (float)w, (float)h));
			Matrix mtrx = new Matrix();
			mtrx.Rotate(angle);
			RectangleF rct = path.GetBounds(mtrx);
			Bitmap dst = new Bitmap((int)rct.Width, (int)rct.Height, pf);
			g = Graphics.FromImage(dst);
			g.Clear(bkColor);
			g.TranslateTransform(-rct.X, -rct.Y);
			g.RotateTransform(angle);
			g.InterpolationMode = InterpolationMode.HighQualityBilinear;
			g.DrawImageUnscaled(tmp, 0, 0);
			g.Dispose();
			tmp.Dispose();
			return dst;
		}

		// Token: 0x04000091 RID: 145
		public static Color[] colors = new Color[]
		{
			Color.Blue,
			Color.DarkRed,
			Color.Green,
			Color.Gold
		};

		// Token: 0x04000092 RID: 146
		protected HtmlForm form1;

		// Token: 0x04000093 RID: 147
		protected Button Button1;
	}
}
