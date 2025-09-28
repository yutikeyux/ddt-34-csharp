using System.Collections.Generic;

namespace MetroFramework.Drawing.Html
{
	internal class CssBoxWordSplitter
	{
		private CssBox cssBox_0;

		private string string_0;

		private List<CssBoxWord> list_0;

		private CssBoxWord cssBoxWord_0;

		internal static CssBoxWordSplitter Y8HGCh4u9yXaJxUba8c;

		public List<CssBoxWord> Words => list_0;

		public string Text => string_0;

		public CssBox Box => cssBox_0;

		public static bool CollapsesWhiteSpaces(CssBox b)
		{
			if (!(b.WhiteSpace == "normal") && !(b.WhiteSpace == "nowrap"))
			{
				return b.WhiteSpace == "pre-line";
			}
			return true;
		}

		public static bool EliminatesLineBreaks(CssBox b)
		{
			if (!(b.WhiteSpace == "normal"))
			{
				return b.WhiteSpace == "nowrap";
			}
			return true;
		}

		private CssBoxWordSplitter()
		{
			list_0 = new List<CssBoxWord>();
			cssBoxWord_0 = null;
		}

		public CssBoxWordSplitter(CssBox box, string text)
			: this()
		{
			cssBox_0 = box;
			string_0 = ((string)(object)text).Replace("\r", string.Empty);
		}

		public void SplitWords()
		{
			if (string.IsNullOrEmpty(Text))
			{
				return;
			}
			cssBoxWord_0 = new CssBoxWord(Box);
			bool flag = method_1(((string)(object)Text)[0]);
			for (int i = 0; i < ((string)(object)Text).Length; i++)
			{
				if (!method_1(((string)(object)Text)[i]))
				{
					if (flag)
					{
						method_0();
					}
					cssBoxWord_0.AppendChar(((string)(object)Text)[i]);
					flag = false;
					continue;
				}
				if (!flag)
				{
					method_0();
				}
				if (method_2(((string)(object)Text)[i]))
				{
					cssBoxWord_0.AppendChar('\n');
					method_0();
				}
				else if (!method_3(((string)(object)Text)[i]))
				{
					cssBoxWord_0.AppendChar(' ');
				}
				else
				{
					cssBoxWord_0.AppendChar('\t');
					method_0();
				}
				flag = true;
			}
			method_0();
		}

		private void method_0()
		{
			if (((string)(object)cssBoxWord_0.Text).Length > 0)
			{
				Words.Add(cssBoxWord_0);
			}
			cssBoxWord_0 = new CssBoxWord(Box);
		}

		private bool method_1(char char_0)
		{
			if (char_0 != ' ' && char_0 != '\t')
			{
				return char_0 == '\n';
			}
			return true;
		}

		private bool method_2(char char_0)
		{
			if (char_0 != '\n')
			{
				return char_0 == '\a';
			}
			return true;
		}

		private bool method_3(char char_0)
		{
			return char_0 == '\t';
		}

		internal static bool MZTWCa4kBRPuI10NWBK()
		{
			return Y8HGCh4u9yXaJxUba8c == null;
		}

		internal static void oRePsn43IHu7VUsd29u()
		{
		}
	}
}
