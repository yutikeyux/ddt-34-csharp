using System;
using System.Collections.Generic;
using System.Drawing;

namespace MetroFramework.Drawing.Html
{
	internal class CssTable
	{
		public class SpacingBox : CssBox
		{
			public readonly CssBox ExtendedBox;

			private int int_0;

			private int int_1;

			private static SpacingBox HQWcTAppp9csdg38mnp;

			public int StartRow => int_0;

			public int EndRow => int_1;

			public SpacingBox(CssBox tableBox, ref CssBox extendedBox, int startRow)
				: base(tableBox, new HtmlTag("<none colspan=" + extendedBox.GetAttribute("colspan", "1") + ">"))
			{
				ExtendedBox = extendedBox;
				base.Display = "none";
				int_0 = startRow;
				int_1 = startRow + int.Parse(extendedBox.GetAttribute("rowspan", "1")) - 1;
			}

			internal static void dTJnwXptEFPo8lv2ixI()
			{
			}

			internal static bool zrOoTPp6fhGdspVtrbg()
			{
				return HQWcTAppp9csdg38mnp == null;
			}
		}

		private CssBox cssBox_0;

		private int int_0;

		private int int_1;

		private List<CssBox> list_0;

		private CssBox cssBox_1;

		private List<CssBox> list_1;

		private CssBox cssBox_2;

		private CssBox cssBox_3;

		private List<CssBox> list_2;

		private float[] float_0;

		private bool bool_0;

		private float[] float_1;

		internal static CssTable u7GlFUlADAkeH0HOQio;

		public bool WidthSpecified => bool_0;

		public List<CssBox> AllRows => list_2;

		public CssBox Caption => cssBox_1;

		public int ColumnCount => int_1;

		public float[] ColumnMinWidths
		{
			get
			{
				if (float_1 == null)
				{
					float_1 = (float[])(object)new float[ColumnWidths.Length];
					foreach (CssBox allRow in AllRows)
					{
						foreach (CssBox box in allRow.Boxes)
						{
							int num = method_4(box);
							int num2 = method_2(allRow, box);
							int num3 = num2 + num - 1;
							float num4 = method_1(allRow, box, num2, num) + (float)(num - 1) * HorizontalSpacing;
							float_1[num3] = Math.Max(float_1[num3], box.GetMinimumWidth() - num4);
						}
					}
				}
				return float_1;
			}
		}

		public List<CssBox> Columns => list_1;

		public float[] ColumnWidths => float_0;

		public List<CssBox> BodyRows => list_0;

		public CssBox FooterBox => cssBox_3;

		public CssBox HeaderBox => cssBox_2;

		public float HorizontalSpacing
		{
			get
			{
				if (!(TableBox.BorderCollapse == "collapse"))
				{
					return TableBox.ActualBorderSpacingHorizontal;
				}
				return -1f;
			}
		}

		public float VerticalSpacing
		{
			get
			{
				if (!(TableBox.BorderCollapse == "collapse"))
				{
					return TableBox.ActualBorderSpacingVertical;
				}
				return -1f;
			}
		}

		public int RowCount => int_0;

		public CssBox TableBox => cssBox_0;

		private CssTable()
		{
			list_0 = new List<CssBox>();
			list_1 = new List<CssBox>();
			list_2 = new List<CssBox>();
		}

		public CssTable(CssBox tableBox, Graphics g)
			: this()
		{
			if (!(tableBox.Display == "table") && !(tableBox.Display == "inline-table"))
			{
				throw new ArgumentException("Box is not a table", "tableBox");
			}
			cssBox_0 = tableBox;
			method_7(tableBox, g);
			method_0(g);
		}

		private void method_0(Graphics graphics_0)
		{
			method_11();
			float num = float.NaN;
			foreach (CssBox box in TableBox.Boxes)
			{
				box.RemoveAnonymousSpaces();
				switch (box.Display)
				{
				case "table-caption":
					cssBox_1 = box;
					break;
				case "table-column":
				{
					for (int k = 0; k < method_14(box); k++)
					{
						Columns.Add(method_15(box));
					}
					break;
				}
				case "table-column-group":
					if (box.Boxes.Count == 0)
					{
						int num2 = method_14(box);
						for (int i = 0; i < num2; i++)
						{
							Columns.Add(method_15(box));
						}
						break;
					}
					foreach (CssBox box2 in box.Boxes)
					{
						int num3 = method_14(box2);
						for (int j = 0; j < num3; j++)
						{
							Columns.Add(method_15(box2));
						}
					}
					break;
				case "table-footer-group":
					if (FooterBox != null)
					{
						BodyRows.Add(box);
					}
					else
					{
						cssBox_3 = box;
					}
					break;
				case "table-header-group":
					if (HeaderBox != null)
					{
						BodyRows.Add(box);
					}
					else
					{
						cssBox_2 = box;
					}
					break;
				case "table-row":
					BodyRows.Add(box);
					break;
				case "table-row-group":
					foreach (CssBox box3 in box.Boxes)
					{
						_ = box3;
						if (box.Display == "table-row")
						{
							BodyRows.Add(box);
						}
					}
					break;
				}
			}
			if (HeaderBox != null)
			{
				list_2.AddRange(HeaderBox.Boxes);
			}
			list_2.AddRange(BodyRows);
			if (FooterBox != null)
			{
				list_2.AddRange(FooterBox.Boxes);
			}
			if (!TableBox.TableFixed)
			{
				int num4 = 0;
				int num5 = 0;
				List<CssBox> bodyRows = BodyRows;
				foreach (CssBox item in bodyRows)
				{
					item.RemoveAnonymousSpaces();
					num5 = 0;
					for (int l = 0; l < item.Boxes.Count; l++)
					{
						CssBox extendedBox = item.Boxes[l];
						int num6 = method_5(extendedBox);
						int num7 = method_2(item, extendedBox);
						for (int m = num4 + 1; m < num4 + num6; m++)
						{
							int num8 = 0;
							for (int n = 0; n <= bodyRows[m].Boxes.Count; n++)
							{
								if (num8 != num7)
								{
									num8++;
									num7 -= method_4(bodyRows[m].Boxes[n]) - 1;
									continue;
								}
								bodyRows[m].Boxes.Insert(num8, new SpacingBox(TableBox, ref extendedBox, num4));
								break;
							}
						}
						num5++;
					}
					num4++;
				}
				TableBox.TableFixed = true;
			}
			int_0 = BodyRows.Count + ((HeaderBox != null) ? HeaderBox.Boxes.Count : 0) + ((FooterBox != null) ? FooterBox.Boxes.Count : 0);
			if (Columns.Count > 0)
			{
				int_1 = Columns.Count;
			}
			else
			{
				foreach (CssBox allRow in AllRows)
				{
					int_1 = Math.Max(int_1, allRow.Boxes.Count);
				}
			}
			float_0 = (float[])(object)new float[int_1];
			for (int num9 = 0; num9 < float_0.Length; num9++)
			{
				float_0[num9] = float.NaN;
			}
			num = method_12();
			if (Columns.Count > 0)
			{
				for (int num10 = 0; num10 < Columns.Count; num10++)
				{
					CssLength cssLength = new CssLength(Columns[num10].Width);
					if (cssLength.Number > 0f)
					{
						if (cssLength.IsPercentage)
						{
							ColumnWidths[num10] = CssValue.ParseNumber(Columns[num10].Width, num);
						}
						else if (cssLength.Unit == CssLength.CssUnit.Pixels || cssLength.Unit == CssLength.CssUnit.None)
						{
							ColumnWidths[num10] = cssLength.Number;
						}
					}
				}
			}
			else
			{
				foreach (CssBox allRow2 in AllRows)
				{
					for (int num11 = 0; num11 < int_1; num11++)
					{
						if (!float.IsNaN(ColumnWidths[num11]) || num11 >= allRow2.Boxes.Count || !(allRow2.Boxes[num11].Display == "table-cell"))
						{
							continue;
						}
						CssLength cssLength2 = new CssLength(allRow2.Boxes[num11].Width);
						if (!(cssLength2.Number > 0f))
						{
							continue;
						}
						int num12 = method_4(allRow2.Boxes[num11]);
						float num13 = 0f;
						if (!cssLength2.IsPercentage)
						{
							if (cssLength2.Unit == CssLength.CssUnit.Pixels || cssLength2.Unit == CssLength.CssUnit.None)
							{
								num13 = cssLength2.Number;
							}
						}
						else
						{
							num13 = CssValue.ParseNumber(allRow2.Boxes[num11].Width, num);
						}
						num13 /= Convert.ToSingle(num12);
						for (int num14 = num11; num14 < num11 + num12; num14++)
						{
							ColumnWidths[num14] = num13;
						}
					}
				}
			}
			if (!WidthSpecified)
			{
				float[] array = (float[])(object)new float[ColumnWidths.Length];
				foreach (CssBox allRow3 in AllRows)
				{
					for (int num15 = 0; num15 < allRow3.Boxes.Count; num15++)
					{
						int num16 = method_2(allRow3, allRow3.Boxes[num15]);
						if (float.IsNaN(ColumnWidths[num16]) && num15 < allRow3.Boxes.Count && method_4(allRow3.Boxes[num15]) == 1)
						{
							array[num16] = Math.Max(array[num16], allRow3.Boxes[num15].GetFullWidth(graphics_0));
						}
					}
				}
				for (int num17 = 0; num17 < ColumnWidths.Length; num17++)
				{
					if (float.IsNaN(ColumnWidths[num17]))
					{
						ColumnWidths[num17] = array[num17];
					}
				}
			}
			else
			{
				int num18 = 0;
				float num19 = 0f;
				for (int num20 = 0; num20 < ColumnWidths.Length; num20++)
				{
					if (!float.IsNaN(ColumnWidths[num20]))
					{
						num19 += ColumnWidths[num20];
					}
					else
					{
						num18++;
					}
				}
				float num21 = (num - num19) / Convert.ToSingle(num18);
				for (int num22 = 0; num22 < ColumnWidths.Length; num22++)
				{
					if (float.IsNaN(ColumnWidths[num22]))
					{
						ColumnWidths[num22] = num21;
					}
				}
			}
			int num23 = 0;
			float num24 = 1f;
			while (!(method_13() <= method_11()) && method_9())
			{
				for (; !method_10(num23); num23++)
				{
				}
				((float[])(object)ColumnWidths)[num23] -= (float)num24;
				num23++;
				if (num23 >= ColumnWidths.Length)
				{
					num23 = 0;
				}
			}
			foreach (CssBox allRow4 in AllRows)
			{
				foreach (CssBox box4 in allRow4.Boxes)
				{
					int num25 = method_4(box4);
					int num26 = method_2(allRow4, box4);
					int num27 = num26 + num25 - 1;
					if (ColumnWidths[num26] < ColumnMinWidths[num26])
					{
						float num28 = ColumnMinWidths[num26] - ColumnWidths[num26];
						ColumnWidths[num27] = ColumnMinWidths[num27];
						if (num26 < ColumnWidths.Length - 1)
						{
							((float[])(object)ColumnWidths)[num26 + 1] -= (float)num28;
						}
					}
				}
			}
			TableBox.Padding = "0";
			float num29 = TableBox.ClientLeft + HorizontalSpacing;
			float num30 = TableBox.ClientTop + VerticalSpacing;
			float num31 = num29;
			float y = num30;
			float num32 = num29;
			float num33 = 0f;
			int num34 = 0;
			foreach (CssBox allRow5 in AllRows)
			{
				if (allRow5 is CssAnonymousSpaceBlockBox || allRow5 is CssAnonymousSpaceBox)
				{
					continue;
				}
				num31 = num29;
				num23 = 0;
				foreach (CssBox box5 in allRow5.Boxes)
				{
					if (num23 >= ColumnWidths.Length)
					{
						break;
					}
					int num35 = method_5(box5);
					float width = method_3(method_2(allRow5, box5), box5);
					box5.Location = new PointF(num31, y);
					box5.Size = new SizeF(width, 0f);
					box5.MeasureBounds(graphics_0);
					SpacingBox spacingBox = box5 as SpacingBox;
					if (spacingBox == null)
					{
						if (num35 == 1)
						{
							num33 = Math.Max(num33, box5.ActualBottom);
						}
					}
					else if (spacingBox.EndRow == num34)
					{
						num33 = Math.Max(num33, spacingBox.ExtendedBox.ActualBottom);
					}
					num32 = Math.Max(num32, box5.ActualRight);
					num23++;
					num31 = box5.ActualRight + HorizontalSpacing;
				}
				foreach (CssBox box6 in allRow5.Boxes)
				{
					SpacingBox spacingBox2 = box6 as SpacingBox;
					if (spacingBox2 == null && method_5(box6) == 1)
					{
						box6.ActualBottom = num33;
						CssLayoutEngine.ApplyCellVerticalAlignment(graphics_0, box6);
					}
					else if (spacingBox2 != null && spacingBox2.EndRow == num34)
					{
						spacingBox2.ExtendedBox.ActualBottom = num33;
						CssLayoutEngine.ApplyCellVerticalAlignment(graphics_0, spacingBox2.ExtendedBox);
					}
				}
				y = num33 + VerticalSpacing;
				num34++;
			}
			TableBox.ActualRight = num32 + HorizontalSpacing + TableBox.ActualBorderRightWidth;
			TableBox.ActualBottom = num33 + VerticalSpacing + TableBox.ActualBorderBottomWidth;
		}

		private float method_1(CssBox cssBox_4, CssBox cssBox_5, int int_2, int int_3)
		{
			float num = 0f;
			for (int i = int_2; i < cssBox_4.Boxes.Count || i < int_2 + int_3 - 1; i++)
			{
				num += ColumnMinWidths[i];
			}
			return num;
		}

		private int method_2(CssBox cssBox_4, CssBox cssBox_5)
		{
			int num = 0;
			foreach (CssBox box in cssBox_4.Boxes)
			{
				if (!box.Equals(cssBox_5))
				{
					num += method_4(box);
					continue;
				}
				return num;
			}
			return num;
		}

		private float method_3(int int_2, CssBox cssBox_4)
		{
			float num = Convert.ToSingle(method_4(cssBox_4));
			float num2 = 0f;
			for (int i = int_2; (float)i < (float)int_2 + num; i++)
			{
				if (int_2 >= ColumnWidths.Length)
				{
					break;
				}
				if (ColumnWidths.Length <= i)
				{
					break;
				}
				num2 += ColumnWidths[i];
			}
			return num2 + (num - 1f) * HorizontalSpacing;
		}

		private int method_4(CssBox cssBox_4)
		{
			string attribute = cssBox_4.GetAttribute("colspan", "1");
			if (!int.TryParse(attribute, out var result))
			{
				return 1;
			}
			return result;
		}

		private int method_5(CssBox cssBox_4)
		{
			string attribute = cssBox_4.GetAttribute("rowspan", "1");
			if (int.TryParse(attribute, out var result))
			{
				return result;
			}
			return 1;
		}

		private void method_6(CssBox cssBox_4, Graphics graphics_0)
		{
			if (cssBox_4 == null)
			{
				return;
			}
			foreach (CssBox box in cssBox_4.Boxes)
			{
				box.MeasureBounds(graphics_0);
				method_6(box, graphics_0);
			}
		}

		private void method_7(CssBox cssBox_4, Graphics graphics_0)
		{
			if (cssBox_4 == null)
			{
				return;
			}
			foreach (CssBox box in cssBox_4.Boxes)
			{
				box.MeasureWordsSize(graphics_0);
				method_7(box, graphics_0);
			}
		}

		private int method_8()
		{
			int num = 0;
			for (int i = 0; i < ColumnWidths.Length; i++)
			{
				if (method_10(i))
				{
					num++;
				}
			}
			return num;
		}

		private bool method_9()
		{
			int num = 0;
			while (true)
			{
				if (num < ColumnWidths.Length)
				{
					if (method_10(num))
					{
						break;
					}
					num++;
					continue;
				}
				return false;
			}
			return true;
		}

		private bool method_10(int int_2)
		{
			if (ColumnWidths.Length < int_2 && ColumnMinWidths.Length < int_2)
			{
				return ColumnWidths[int_2] > ColumnMinWidths[int_2];
			}
			return false;
		}

		private float method_11()
		{
			CssLength cssLength = new CssLength(TableBox.Width);
			if (cssLength.Number > 0f)
			{
				bool_0 = true;
				if (!cssLength.IsPercentage)
				{
					return cssLength.Number;
				}
				return CssValue.ParseNumber(cssLength.Length, TableBox.ParentBox.AvailableWidth);
			}
			return TableBox.ParentBox.AvailableWidth;
		}

		private float method_12()
		{
			return method_11() - HorizontalSpacing * (float)(ColumnCount + 1) - TableBox.ActualBorderLeftWidth - TableBox.ActualBorderRightWidth;
		}

		private float method_13()
		{
			float num = 0f;
			for (int i = 0; i < ColumnWidths.Length; i++)
			{
				if (!float.IsNaN(ColumnWidths[i]))
				{
					num += ColumnWidths[i];
					continue;
				}
				throw new Exception("CssTable Algorithm error: There's a NaN in column widths");
			}
			num += HorizontalSpacing * (float)(ColumnWidths.Length + 1);
			return num + (TableBox.ActualBorderLeftWidth + TableBox.ActualBorderRightWidth);
		}

		private int method_14(CssBox cssBox_4)
		{
			float value = CssValue.ParseNumber(cssBox_4.GetAttribute("span"), 1f);
			return Math.Max(1, Convert.ToInt32(value));
		}

		private CssBox method_15(CssBox cssBox_4)
		{
			return cssBox_4;
		}

		internal static void emRpNJlrUyi5GRaZGp8()
		{
		}

		internal static bool HRORIjlcvGiwyMqQUK8()
		{
			return u7GlFUlADAkeH0HOQio == null;
		}
	}
}
