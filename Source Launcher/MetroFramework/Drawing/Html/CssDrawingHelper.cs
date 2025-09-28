using System.Drawing;
using System.Drawing.Drawing2D;

namespace MetroFramework.Drawing.Html
{
	internal static class CssDrawingHelper
	{
		internal enum Border
		{
			Top,
			Right,
			Bottom,
			Left
		}

		private static object MWXWABH1vawm9SpKGyg;

		private static PointF smethod_0(PointF pointF_0, object object_0)
		{
			return pointF_0;
		}

		private static RectangleF smethod_1(RectangleF rectangleF_0, object object_0)
		{
			return Rectangle.Round(rectangleF_0);
		}

		public static GraphicsPath GetBorderPath(Border border, CssBox b, RectangleF r, bool isLineStart, bool isLineEnd)
		{
			PointF[] array = new PointF[4];
			float num = 0f;
			GraphicsPath graphicsPath = null;
			switch (border)
			{
			case Border.Top:
			{
				num = b.ActualBorderTopWidth;
				ref PointF reference13 = ref array[0];
				reference13 = smethod_0(new PointF(r.Left + b.ActualCornerNW, r.Top), b);
				ref PointF reference14 = ref array[1];
				reference14 = smethod_0(new PointF(r.Right - b.ActualCornerNE, r.Top), b);
				ref PointF reference15 = ref array[2];
				reference15 = smethod_0(new PointF(r.Right - b.ActualCornerNE, r.Top + num), b);
				ref PointF reference16 = ref array[3];
				reference16 = smethod_0(new PointF(r.Left + b.ActualCornerNW, r.Top + num), b);
				if (isLineEnd && b.ActualCornerNE == 0f)
				{
					array[2].X -= b.ActualBorderRightWidth;
				}
				if (isLineStart && b.ActualCornerNW == 0f)
				{
					array[3].X += b.ActualBorderLeftWidth;
				}
				if (b.ActualCornerNW > 0f)
				{
					graphicsPath = smethod_2(b, r, 1);
				}
				break;
			}
			case Border.Right:
			{
				num = b.ActualBorderRightWidth;
				ref PointF reference5 = ref array[0];
				reference5 = smethod_0(new PointF(r.Right - num, r.Top + b.ActualCornerNE), b);
				ref PointF reference6 = ref array[1];
				reference6 = smethod_0(new PointF(r.Right, r.Top + b.ActualCornerNE), b);
				ref PointF reference7 = ref array[2];
				reference7 = smethod_0(new PointF(r.Right, r.Bottom - b.ActualCornerSE), b);
				ref PointF reference8 = ref array[3];
				reference8 = smethod_0(new PointF(r.Right - num, r.Bottom - b.ActualCornerSE), b);
				if (b.ActualCornerNE == 0f)
				{
					array[0].Y += b.ActualBorderTopWidth;
				}
				if (b.ActualCornerSE == 0f)
				{
					array[3].Y -= b.ActualBorderBottomWidth;
				}
				if (b.ActualCornerNE > 0f)
				{
					graphicsPath = smethod_2(b, r, 2);
				}
				break;
			}
			case Border.Bottom:
			{
				num = b.ActualBorderBottomWidth;
				ref PointF reference9 = ref array[0];
				reference9 = smethod_0(new PointF(r.Left + b.ActualCornerSW, r.Bottom - num), b);
				ref PointF reference10 = ref array[1];
				reference10 = smethod_0(new PointF(r.Right - b.ActualCornerSE, r.Bottom - num), b);
				ref PointF reference11 = ref array[2];
				reference11 = smethod_0(new PointF(r.Right - b.ActualCornerSE, r.Bottom), b);
				ref PointF reference12 = ref array[3];
				reference12 = smethod_0(new PointF(r.Left + b.ActualCornerSW, r.Bottom), b);
				if (isLineStart && b.ActualCornerSW == 0f)
				{
					array[0].X += b.ActualBorderLeftWidth;
				}
				if (isLineEnd && b.ActualCornerSE == 0f)
				{
					array[1].X -= b.ActualBorderRightWidth;
				}
				if (b.ActualCornerSE > 0f)
				{
					graphicsPath = smethod_2(b, r, 3);
				}
				break;
			}
			case Border.Left:
			{
				num = b.ActualBorderLeftWidth;
				ref PointF reference = ref array[0];
				reference = smethod_0(new PointF(r.Left, r.Top + b.ActualCornerNW), b);
				ref PointF reference2 = ref array[1];
				reference2 = smethod_0(new PointF(r.Left + num, r.Top + b.ActualCornerNW), b);
				ref PointF reference3 = ref array[2];
				reference3 = smethod_0(new PointF(r.Left + num, r.Bottom - b.ActualCornerSW), b);
				ref PointF reference4 = ref array[3];
				reference4 = smethod_0(new PointF(r.Left, r.Bottom - b.ActualCornerSW), b);
				if (b.ActualCornerNW == 0f)
				{
					array[1].Y += b.ActualBorderTopWidth;
				}
				if (b.ActualCornerSW == 0f)
				{
					array[2].Y -= b.ActualBorderBottomWidth;
				}
				if (b.ActualCornerSW > 0f)
				{
					graphicsPath = smethod_2(b, r, 4);
				}
				break;
			}
			}
			GraphicsPath graphicsPath2 = new GraphicsPath(array, (byte[])(object)new byte[4] { 1, 1, 1, 1 });
			if (graphicsPath != null)
			{
				graphicsPath2.AddPath(graphicsPath, connect: true);
			}
			return graphicsPath2;
		}

		private static GraphicsPath smethod_2(CssBox cssBox_0, RectangleF rectangleF_0, int int_0)
		{
			GraphicsPath graphicsPath = new GraphicsPath();
			RectangleF rectangleF_ = RectangleF.Empty;
			RectangleF rectangleF_2 = RectangleF.Empty;
			float startAngle = 0f;
			float startAngle2 = 0f;
			switch (int_0)
			{
			case 1:
				rectangleF_ = new RectangleF(rectangleF_0.Left, rectangleF_0.Top, cssBox_0.ActualCornerNW, cssBox_0.ActualCornerNW);
				rectangleF_2 = RectangleF.FromLTRB(rectangleF_.Left + cssBox_0.ActualBorderLeftWidth, rectangleF_.Top + cssBox_0.ActualBorderTopWidth, rectangleF_.Right, rectangleF_.Bottom);
				startAngle = 180f;
				startAngle2 = 270f;
				break;
			case 2:
				rectangleF_ = new RectangleF(rectangleF_0.Right - cssBox_0.ActualCornerNE, rectangleF_0.Top, cssBox_0.ActualCornerNE, cssBox_0.ActualCornerNE);
				rectangleF_2 = RectangleF.FromLTRB(rectangleF_.Left, rectangleF_.Top + cssBox_0.ActualBorderTopWidth, rectangleF_.Right - cssBox_0.ActualBorderRightWidth, rectangleF_.Bottom);
				rectangleF_.X -= rectangleF_.Width;
				rectangleF_2.X -= rectangleF_2.Width;
				startAngle = -90f;
				startAngle2 = 0f;
				break;
			case 3:
				rectangleF_ = RectangleF.FromLTRB(rectangleF_0.Right - cssBox_0.ActualCornerSE, rectangleF_0.Bottom - cssBox_0.ActualCornerSE, rectangleF_0.Right, rectangleF_0.Bottom);
				rectangleF_2 = new RectangleF(rectangleF_.Left, rectangleF_.Top, rectangleF_.Width - cssBox_0.ActualBorderRightWidth, rectangleF_.Height - cssBox_0.ActualBorderBottomWidth);
				rectangleF_.X -= rectangleF_.Width;
				rectangleF_.Y -= rectangleF_.Height;
				rectangleF_2.X -= rectangleF_2.Width;
				rectangleF_2.Y -= rectangleF_2.Height;
				startAngle = 0f;
				startAngle2 = 90f;
				break;
			case 4:
				rectangleF_ = new RectangleF(rectangleF_0.Left, rectangleF_0.Bottom - cssBox_0.ActualCornerSW, cssBox_0.ActualCornerSW, cssBox_0.ActualCornerSW);
				rectangleF_2 = RectangleF.FromLTRB(rectangleF_0.Left + cssBox_0.ActualBorderLeftWidth, rectangleF_.Top, rectangleF_.Right, rectangleF_.Bottom - cssBox_0.ActualBorderBottomWidth);
				startAngle = 90f;
				startAngle2 = 180f;
				rectangleF_.Y -= rectangleF_.Height;
				rectangleF_2.Y -= rectangleF_2.Height;
				break;
			}
			if (rectangleF_.Width <= 0f)
			{
				rectangleF_.Width = 1f;
			}
			if (rectangleF_.Height <= 0f)
			{
				rectangleF_.Height = 1f;
			}
			if (rectangleF_2.Width <= 0f)
			{
				rectangleF_2.Width = 1f;
			}
			if (rectangleF_2.Height <= 0f)
			{
				rectangleF_2.Height = 1f;
			}
			rectangleF_.Width *= 2f;
			rectangleF_.Height *= 2f;
			rectangleF_2.Width *= 2f;
			rectangleF_2.Height *= 2f;
			rectangleF_ = smethod_1(rectangleF_, cssBox_0);
			rectangleF_2 = smethod_1(rectangleF_2, cssBox_0);
			graphicsPath.AddArc(rectangleF_, startAngle, 90f);
			graphicsPath.AddArc(rectangleF_2, startAngle2, -90f);
			graphicsPath.CloseFigure();
			return graphicsPath;
		}

		public static GraphicsPath GetRoundRect(RectangleF rect, float nwRadius, float neRadius, float seRadius, float swRadius)
		{
			GraphicsPath graphicsPath = new GraphicsPath();
			nwRadius *= 2f;
			neRadius *= 2f;
			seRadius *= 2f;
			swRadius *= 2f;
			graphicsPath.AddLine(rect.X + nwRadius, rect.Y, rect.Right - neRadius, rect.Y);
			if (neRadius > 0f)
			{
				graphicsPath.AddArc(RectangleF.FromLTRB(rect.Right - neRadius, rect.Top, rect.Right, rect.Top + neRadius), -90f, 90f);
			}
			graphicsPath.AddLine(rect.Right, rect.Top + neRadius, rect.Right, rect.Bottom - seRadius);
			if (seRadius > 0f)
			{
				graphicsPath.AddArc(RectangleF.FromLTRB(rect.Right - seRadius, rect.Bottom - seRadius, rect.Right, rect.Bottom), 0f, 90f);
			}
			graphicsPath.AddLine(rect.Right - seRadius, rect.Bottom, rect.Left + swRadius, rect.Bottom);
			if (swRadius > 0f)
			{
				graphicsPath.AddArc(RectangleF.FromLTRB(rect.Left, rect.Bottom - swRadius, rect.Left + swRadius, rect.Bottom), 90f, 90f);
			}
			graphicsPath.AddLine(rect.Left, rect.Bottom - swRadius, rect.Left, rect.Top + nwRadius);
			if (nwRadius > 0f)
			{
				graphicsPath.AddArc(RectangleF.FromLTRB(rect.Left, rect.Top, rect.Left + nwRadius, rect.Top + nwRadius), 180f, 90f);
			}
			graphicsPath.CloseFigure();
			return graphicsPath;
		}

		public static Color Darken(Color c)
		{
			return Color.FromArgb((int)c.R / 2, (int)c.G / 2, (int)c.B / 2);
		}

		internal static bool WscOnmHxH36dReLTFua()
		{
			return MWXWABH1vawm9SpKGyg == null;
		}
	}
}
