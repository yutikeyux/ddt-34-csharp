using System.Drawing;

namespace MetroFramework.Drawing.Html
{
	public class CssRectangle
	{
		private float float_0;

		private float float_1;

		private float float_2;

		private float float_3;

		private static CssRectangle v93YdG4tPoTH6UI31kV;

		public float Left
		{
			get
			{
				return float_0;
			}
			set
			{
				float_0 = value;
			}
		}

		public float Top
		{
			get
			{
				return float_1;
			}
			set
			{
				float_1 = value;
			}
		}

		public float Width
		{
			get
			{
				return float_2;
			}
			set
			{
				float_2 = value;
			}
		}

		public float Height
		{
			get
			{
				return float_3;
			}
			set
			{
				float_3 = value;
			}
		}

		public float Right
		{
			get
			{
				return Bounds.Right;
			}
			set
			{
				Width = value - Left;
			}
		}

		public float Bottom
		{
			get
			{
				return Bounds.Bottom;
			}
			set
			{
				Height = value - Top;
			}
		}

		public RectangleF Bounds
		{
			get
			{
				return new RectangleF(Left, Top, Width, Height);
			}
			set
			{
				Left = value.Left;
				Top = value.Top;
				Width = value.Width;
				Height = value.Height;
			}
		}

		public PointF Location
		{
			get
			{
				return new PointF(Left, Top);
			}
			set
			{
				Left = value.X;
				Top = value.Y;
			}
		}

		public SizeF Size
		{
			get
			{
				return new SizeF(Width, Height);
			}
			set
			{
				Width = value.Width;
				Height = value.Height;
			}
		}

		internal static bool fAhWKd4DqO8g05NXxob()
		{
			return v93YdG4tPoTH6UI31kV == null;
		}

		internal static void z6gMfj4TdP58yxDgTHP()
		{
		}
	}
}
