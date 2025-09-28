using System;
using System.Collections.Generic;
using System.Drawing;

namespace MetroFramework.Drawing.Html
{
	internal static class CssLayoutEngine
	{
		private static CssBoxWord cssBoxWord_0;

		internal static object GD4Ui7HeppLgfaY5R1N;

		public static void CreateLineBoxes(Graphics g, CssBox blockBox)
		{
			blockBox.LineBoxes.Clear();
			float float_ = blockBox.ActualRight - blockBox.ActualPaddingRight - blockBox.ActualBorderRightWidth;
			float num = blockBox.Location.X + blockBox.ActualPaddingLeft - 0f + blockBox.ActualBorderLeftWidth;
			float num2 = blockBox.Location.Y + blockBox.ActualPaddingTop - 0f + blockBox.ActualBorderTopWidth;
			float float_2 = num + blockBox.ActualTextIndent;
			float float_3 = num2;
			float float_4 = num2;
			float float_5 = 0f;
			CssLineBox cssLineBox_ = new CssLineBox(blockBox);
			smethod_0(g, blockBox, blockBox, float_, float_5, num, ref cssLineBox_, ref float_2, ref float_3, ref float_4);
			foreach (CssLineBox lineBox in blockBox.LineBoxes)
			{
				smethod_1(blockBox, lineBox);
				lineBox.AssignRectanglesToBoxes();
				smethod_2(g, lineBox);
				if (blockBox.Direction == "rtl")
				{
					smethod_3(lineBox);
				}
			}
			blockBox.ActualBottom = float_4 + blockBox.ActualPaddingBottom + blockBox.ActualBorderBottomWidth;
		}

		private static void smethod_0(Graphics graphics_0, CssBox cssBox_0, CssBox cssBox_1, float float_0, float float_1, float float_2, ref CssLineBox cssLineBox_0, ref float float_3, ref float float_4, ref float float_5)
		{
			cssBox_1.FirstHostingLineBox = cssLineBox_0;
			foreach (CssBox box in cssBox_1.Boxes)
			{
				float num = box.ActualMarginLeft + box.ActualBorderLeftWidth + box.ActualPaddingLeft;
				float num2 = box.ActualMarginRight + box.ActualBorderRightWidth + box.ActualPaddingRight;
				_ = box.ActualBorderTopWidth;
				_ = box.ActualPaddingTop;
				_ = box.ActualBorderBottomWidth;
				_ = box.ActualPaddingTop;
				box.RectanglesReset();
				box.MeasureWordsSize(graphics_0);
				float_3 += num;
				if (box.Words.Count > 0)
				{
					foreach (CssBoxWord word in box.Words)
					{
						if ((box.WhiteSpace != "nowrap" && float_3 + word.Width + num2 > float_0) || word.IsLineBreak)
						{
							float_3 = float_2;
							float_4 = float_5 + float_1;
							cssLineBox_0 = new CssLineBox(cssBox_0);
							if (word.IsImage || word.Equals(box.FirstWord))
							{
								float_3 += num;
							}
						}
						cssLineBox_0.ReportExistanceOf(word);
						word.Left = float_3;
						word.Top = float_4;
						float_3 = word.Right;
						float_5 = Math.Max(float_5, word.Bottom);
						cssBoxWord_0 = word;
					}
				}
				else
				{
					smethod_0(graphics_0, cssBox_0, box, float_0, float_1, float_2, ref cssLineBox_0, ref float_3, ref float_4, ref float_5);
				}
				float_3 += num2;
			}
			cssBox_1.LastHostingLineBox = cssLineBox_0;
		}

		private static void smethod_1(CssBox cssBox_0, CssLineBox cssLineBox_0)
		{
			if (cssBox_0.Words.Count > 0)
			{
				float num = float.MaxValue;
				float num2 = float.MaxValue;
				float num3 = float.MinValue;
				float num4 = float.MinValue;
				List<CssBoxWord> list = cssLineBox_0.WordsOf(cssBox_0);
				if (list.Count <= 0)
				{
					return;
				}
				foreach (CssBoxWord item in list)
				{
					num = Math.Min(num, item.Left);
					num3 = Math.Max(num3, item.Right);
					num2 = Math.Min(num2, item.Top);
					num4 = Math.Max(num4, item.Bottom);
				}
				cssLineBox_0.UpdateRectangle(cssBox_0, num, num2, num3, num4);
				return;
			}
			foreach (CssBox box in cssBox_0.Boxes)
			{
				smethod_1(box, cssLineBox_0);
			}
		}

		public static float WhiteSpace(Graphics g, CssBox b)
		{
			string text = " .";
			float num = 0f;
			float result = 5f;
			StringFormat stringFormat = new StringFormat();
			stringFormat.SetMeasurableCharacterRanges(new CharacterRange[1]
			{
				new CharacterRange(0, 1)
			});
			Region[] array = g.MeasureCharacterRanges(text, b.ActualFont, new RectangleF(0f, 0f, float.MaxValue, float.MaxValue), stringFormat);
			if (array != null && array.Length != 0)
			{
				num = array[0].GetBounds(g).Width;
				if (!string.IsNullOrEmpty(b.WordSpacing) && !(b.WordSpacing == "normal"))
				{
					num += CssValue.ParseLength(b.WordSpacing, 0f, b);
				}
				return num;
			}
			return result;
		}

		private static void smethod_2(Graphics graphics_0, CssLineBox cssLineBox_0)
		{
			switch (cssLineBox_0.OwnerBox.TextAlign)
			{
			case "justify":
				smethod_5(graphics_0, cssLineBox_0);
				break;
			case "center":
				smethod_6(graphics_0, cssLineBox_0);
				break;
			case "right":
				smethod_7(graphics_0, cssLineBox_0);
				break;
			default:
				smethod_8(graphics_0, cssLineBox_0);
				break;
			}
			smethod_4(graphics_0, cssLineBox_0);
		}

		private static void smethod_3(CssLineBox cssLineBox_0)
		{
			float clientLeft = cssLineBox_0.OwnerBox.ClientLeft;
			float clientRight = cssLineBox_0.OwnerBox.ClientRight;
			foreach (CssBoxWord word in cssLineBox_0.Words)
			{
				float num = word.Left - clientLeft;
				float num2 = clientRight - num;
				word.Left = num2 - word.Width;
			}
		}

		public static float GetAscent(Font f)
		{
			return f.Size * (float)f.FontFamily.GetCellAscent(f.Style) / (float)f.FontFamily.GetEmHeight(f.Style);
		}

		public static float GetDescent(Font f)
		{
			return f.Size * (float)f.FontFamily.GetCellDescent(f.Style) / (float)f.FontFamily.GetEmHeight(f.Style);
		}

		public static float GetLineSpacing(Font f)
		{
			return f.Size * (float)f.FontFamily.GetLineSpacing(f.Style) / (float)f.FontFamily.GetEmHeight(f.Style);
		}

		private static void smethod_4(Graphics graphics_0, CssLineBox cssLineBox_0)
		{
			_ = cssLineBox_0.OwnerBox.Display == "table-cell";
			float num = cssLineBox_0.GetMaxWordBottom() - GetDescent(cssLineBox_0.OwnerBox.ActualFont) - 2f;
			List<CssBox> list = new List<CssBox>(cssLineBox_0.Rectangles.Keys);
			foreach (CssBox item in list)
			{
				GetAscent(item.ActualFont);
				GetDescent(item.ActualFont);
				switch (item.VerticalAlign)
				{
				case "sub":
					cssLineBox_0.SetBaseLine(graphics_0, item, num + cssLineBox_0.Rectangles[item].Height * 0.2f);
					break;
				case "super":
					cssLineBox_0.SetBaseLine(graphics_0, item, num - cssLineBox_0.Rectangles[item].Height * 0.2f);
					break;
				default:
					cssLineBox_0.SetBaseLine(graphics_0, item, num);
					break;
				case "text-top":
				case "text-bottom":
				case "top":
				case "bottom":
				case "middle":
					break;
				}
			}
		}

		public static void ApplyCellVerticalAlignment(Graphics g, CssBox cell)
		{
			if (cell.VerticalAlign == "top" || cell.VerticalAlign == "baseline")
			{
				return;
			}
			_ = cell.ClientTop;
			float clientBottom = cell.ClientBottom;
			float maximumBottom = cell.GetMaximumBottom(cell, 0f);
			float amount = 0f;
			if (cell.VerticalAlign == "bottom")
			{
				amount = clientBottom - maximumBottom;
			}
			else if (cell.VerticalAlign == "middle")
			{
				amount = (clientBottom - maximumBottom) / 2f;
			}
			foreach (CssBox box in cell.Boxes)
			{
				box.OffsetTop(amount);
			}
		}

		private static void smethod_5(object object_0, CssLineBox cssLineBox_0)
		{
			if (cssLineBox_0.Equals(cssLineBox_0.OwnerBox.LineBoxes[cssLineBox_0.OwnerBox.LineBoxes.Count - 1]))
			{
				return;
			}
			float num = ((!cssLineBox_0.Equals(cssLineBox_0.OwnerBox.LineBoxes[0])) ? 0f : cssLineBox_0.OwnerBox.ActualTextIndent);
			float num2 = 0f;
			float num3 = 0f;
			float num4 = cssLineBox_0.OwnerBox.ClientRectangle.Width - num;
			foreach (CssBoxWord word in cssLineBox_0.Words)
			{
				num2 += word.Width;
				num3 += 1f;
			}
			if (num3 <= 0f)
			{
				return;
			}
			float num5 = (num4 - num2) / num3;
			float left = cssLineBox_0.OwnerBox.ClientLeft + num;
			foreach (CssBoxWord word2 in cssLineBox_0.Words)
			{
				word2.Left = left;
				left = word2.Right + num5;
				if (word2 == cssLineBox_0.Words[cssLineBox_0.Words.Count - 1])
				{
					word2.Left = cssLineBox_0.OwnerBox.ClientRight - word2.Width;
				}
			}
		}

		private static void smethod_6(object object_0, CssLineBox cssLineBox_0)
		{
			if (cssLineBox_0.Words.Count == 0)
			{
				return;
			}
			CssBoxWord cssBoxWord = cssLineBox_0.Words[cssLineBox_0.Words.Count - 1];
			float num = cssLineBox_0.OwnerBox.ActualRight - cssLineBox_0.OwnerBox.ActualPaddingRight - cssLineBox_0.OwnerBox.ActualBorderRightWidth;
			float num2 = num - cssBoxWord.Right - cssBoxWord.LastMeasureOffset.X - cssBoxWord.OwnerBox.ActualBorderRightWidth - cssBoxWord.OwnerBox.ActualPaddingRight;
			num2 /= 2f;
			if (num2 <= 0f)
			{
				return;
			}
			foreach (CssBoxWord word in cssLineBox_0.Words)
			{
				word.Left += num2;
			}
			foreach (CssBox key in cssLineBox_0.Rectangles.Keys)
			{
				RectangleF rectangleF = key.Rectangles[cssLineBox_0];
				key.Rectangles[cssLineBox_0] = new RectangleF(rectangleF.X + num2, rectangleF.Y, rectangleF.Width, rectangleF.Height);
			}
		}

		private static void smethod_7(object object_0, CssLineBox cssLineBox_0)
		{
			if (cssLineBox_0.Words.Count == 0)
			{
				return;
			}
			CssBoxWord cssBoxWord = cssLineBox_0.Words[cssLineBox_0.Words.Count - 1];
			float num = cssLineBox_0.OwnerBox.ActualRight - cssLineBox_0.OwnerBox.ActualPaddingRight - cssLineBox_0.OwnerBox.ActualBorderRightWidth;
			float num2 = num - cssBoxWord.Right - cssBoxWord.LastMeasureOffset.X - cssBoxWord.OwnerBox.ActualBorderRightWidth - cssBoxWord.OwnerBox.ActualPaddingRight;
			if (num2 <= 0f)
			{
				return;
			}
			foreach (CssBoxWord word in cssLineBox_0.Words)
			{
				word.Left += num2;
			}
			foreach (CssBox key in cssLineBox_0.Rectangles.Keys)
			{
				RectangleF rectangleF = key.Rectangles[cssLineBox_0];
				key.Rectangles[cssLineBox_0] = new RectangleF(rectangleF.X + num2, rectangleF.Y, rectangleF.Width, rectangleF.Height);
			}
		}

		private static void smethod_8(object object_0, object object_1)
		{
		}

		internal static bool WCGQq0HMuu8x5LkTvKY()
		{
			return GD4Ui7HeppLgfaY5R1N == null;
		}

		internal static void d7bBThlL2ZHswJeZKVY()
		{
		}
	}
}
