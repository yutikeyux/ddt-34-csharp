using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace MetroFramework.Drawing
{
	internal sealed class MetroBackBuffer
	{
		private Bitmap bitmap_0;

		internal static MetroBackBuffer aExJLSSfhEmHh1ZfQeY;

		public MetroBackBuffer(Size bufferSize)
		{
			bitmap_0 = new Bitmap(bufferSize.Width, bufferSize.Height, PixelFormat.Format32bppArgb);
		}

		public Graphics CreateGraphics()
		{
			Graphics graphics = Graphics.FromImage(bitmap_0);
			graphics.CompositingMode = CompositingMode.SourceOver;
			graphics.CompositingQuality = CompositingQuality.HighQuality;
			graphics.InterpolationMode = InterpolationMode.High;
			graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
			return graphics;
		}

		public void Draw(Graphics g)
		{
			g.DrawImageUnscaled(bitmap_0, Point.Empty);
		}

		internal static bool Jq0mrjSUijGd9IVaXiE()
		{
			return aExJLSSfhEmHh1ZfQeY == null;
		}
	}
}
