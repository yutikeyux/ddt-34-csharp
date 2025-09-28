using System;
using System.Collections.Generic;
using System.Drawing;

namespace MetroFramework.Drawing.Html
{
	internal class CssLineBox
	{
		private List<CssBoxWord> list_0;

		private CssBox cssBox_0;

		private Dictionary<CssBox, RectangleF> dictionary_0;

		private List<CssBox> list_1;

		private static CssLineBox uOMB5ylCPBO2ouZiq0Z;

		public List<CssBox> RelatedBoxes => list_1;

		public List<CssBoxWord> Words => list_0;

		public CssBox OwnerBox => cssBox_0;

		public Dictionary<CssBox, RectangleF> Rectangles => dictionary_0;

		public CssLineBox(CssBox ownerBox)
		{
			dictionary_0 = new Dictionary<CssBox, RectangleF>();
			list_1 = new List<CssBox>();
			list_0 = new List<CssBoxWord>();
			cssBox_0 = ownerBox;
			cssBox_0.LineBoxes.Add(this);
		}

		public float GetMaxWordBottom()
		{
			float num = float.MinValue;
			foreach (CssBoxWord word in Words)
			{
				num = Math.Max(num, word.Bottom);
			}
			return num;
		}

		internal void ReportExistanceOf(CssBoxWord word)
		{
			if (!Words.Contains(word))
			{
				Words.Add(word);
			}
			if (!RelatedBoxes.Contains(word.OwnerBox))
			{
				RelatedBoxes.Add(word.OwnerBox);
			}
		}

		internal List<CssBoxWord> WordsOf(CssBox box)
		{
			List<CssBoxWord> list = new List<CssBoxWord>();
			foreach (CssBoxWord word in Words)
			{
				if (word.OwnerBox.Equals(box))
				{
					list.Add(word);
				}
			}
			return list;
		}

		internal void UpdateRectangle(CssBox box, float x, float y, float r, float b)
		{
			float num = box.ActualBorderLeftWidth + box.ActualPaddingLeft;
			float num2 = box.ActualBorderRightWidth + box.ActualPaddingRight;
			float num3 = box.ActualBorderTopWidth + box.ActualPaddingTop;
			float num4 = box.ActualBorderBottomWidth + box.ActualPaddingTop;
			if ((box.FirstHostingLineBox != null && box.FirstHostingLineBox.Equals(this)) || box.IsImage)
			{
				x -= num;
			}
			if ((box.LastHostingLineBox != null && box.LastHostingLineBox.Equals(this)) || box.IsImage)
			{
				r += num2;
			}
			if (!box.IsImage)
			{
				y -= num3;
				b += num4;
			}
			if (Rectangles.ContainsKey(box))
			{
				RectangleF rectangleF = Rectangles[box];
				Rectangles[box] = RectangleF.FromLTRB(Math.Min(rectangleF.X, x), Math.Min(rectangleF.Y, y), Math.Max(rectangleF.Right, r), Math.Max(rectangleF.Bottom, b));
			}
			else
			{
				Rectangles.Add(box, RectangleF.FromLTRB(x, y, r, b));
			}
			if (box.ParentBox != null && box.ParentBox.Display == "inline")
			{
				UpdateRectangle(box.ParentBox, x, y, r, b);
			}
		}

		internal void AssignRectanglesToBoxes()
		{
			foreach (CssBox key in Rectangles.Keys)
			{
				key.Rectangles.Add(this, Rectangles[key]);
			}
		}

		internal void DrawRectangles(Graphics g)
		{
			foreach (CssBox key in Rectangles.Keys)
			{
				if (!float.IsInfinity(Rectangles[key].Width))
				{
					g.FillRectangle(new SolidBrush(Color.FromArgb(50, Color.Black)), Rectangle.Round(Rectangles[key]));
					g.DrawRectangle(Pens.Red, Rectangle.Round(Rectangles[key]));
				}
			}
		}

		public float GetBaseLineHeight(CssBox b, Graphics g)
		{
			Font actualFont = b.ActualFont;
			FontFamily fontFamily = actualFont.FontFamily;
			FontStyle style = actualFont.Style;
			return actualFont.GetHeight(g) * (float)fontFamily.GetCellAscent(style) / (float)fontFamily.GetLineSpacing(style);
		}

		internal void SetBaseLine(Graphics g, CssBox b, float baseline)
		{
			List<CssBoxWord> list = WordsOf(b);
			if (!Rectangles.ContainsKey(b))
			{
				return;
			}
			RectangleF rectangleF = Rectangles[b];
			float num = 0f;
			if (list.Count > 0)
			{
				num = list[0].Top - rectangleF.Top;
			}
			else
			{
				CssBoxWord cssBoxWord = b.FirstWordOccourence(b, this);
				if (cssBoxWord != null)
				{
					num = cssBoxWord.Top - rectangleF.Top;
				}
			}
			float num2 = baseline - GetBaseLineHeight(b, g);
			if (b.ParentBox != null && b.ParentBox.Rectangles.ContainsKey(this) && rectangleF.Height < b.ParentBox.Rectangles[this].Height)
			{
				float y = num2 - num;
				RectangleF value = new RectangleF(rectangleF.X, y, rectangleF.Width, rectangleF.Height);
				Rectangles[b] = value;
				b.OffsetRectangle(this, num);
			}
			foreach (CssBoxWord item in list)
			{
				if (!item.IsImage)
				{
					item.Top = num2;
				}
			}
		}

		public override string ToString()
		{
			string[] array = (string[])(object)new string[Words.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Words[i].Text;
			}
			return string.Join(" ", array);
		}

		internal static void nimlb3l4U3aWVmpK1k8()
		{
		}

		internal static bool IENwTolYNPymU86YFvX()
		{
			return uOMB5ylCPBO2ouZiq0Z == null;
		}
	}
}
