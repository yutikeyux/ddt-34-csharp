using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MetroFramework.Drawing.Html
{
	public class HtmlTag
	{
		private string string_0;

		private bool bool_0;

		private Dictionary<string, string> dictionary_0;

		internal static HtmlTag ub38TZ6qX0XamRhBaFW;

		public Dictionary<string, string> Attributes => dictionary_0;

		public string TagName => string_0;

		public bool IsClosing => bool_0;

		public bool IsSingle
		{
			get
			{
				if (((string)(object)TagName).StartsWith("!"))
				{
					return true;
				}
				return new List<string>((IEnumerable<string>)(object)new string[13]
				{
					"area", "base", "basefont", "br", "col", "frame", "hr", "img", "input", "isindex",
					"link", "meta", "param"
				}).Contains(TagName);
			}
		}

		private HtmlTag()
		{
			dictionary_0 = new Dictionary<string, string>();
		}

		public HtmlTag(string tag)
			: this()
		{
			tag = ((string)(object)tag).Substring(1, ((string)(object)tag).Length - 2);
			int num = ((string)(object)tag).IndexOf(" ");
			if (num < 0)
			{
				string_0 = tag;
			}
			else
			{
				string_0 = ((string)(object)tag).Substring(0, num);
			}
			if (((string)(object)string_0).StartsWith("/"))
			{
				bool_0 = true;
				string_0 = ((string)(object)string_0).Substring(1);
			}
			string_0 = ((string)(object)string_0).ToLower();
			MatchCollection matchCollection = Parser.Match("[^\\s]*\\s*=\\s*(\"[^\"]*\"|[^\\s]*)", tag);
			foreach (Match item in matchCollection)
			{
				string[] array = ((string)(object)item.Value).Split((char[])(object)new char[1] { '=' });
				if (array.Length == 1)
				{
					if (!Attributes.ContainsKey(array[0]))
					{
						Attributes.Add(((string)(object)array[0]).ToLower(), string.Empty);
					}
				}
				else if (array.Length == 2)
				{
					string key = ((string)(object)array[0]).Trim();
					string text = ((string)(object)array[1]).Trim();
					if (((string)(object)text).StartsWith("\"") && ((string)(object)text).EndsWith("\"") && ((string)(object)text).Length > 2)
					{
						text = ((string)(object)text).Substring(1, ((string)(object)text).Length - 2);
					}
					if (!Attributes.ContainsKey(key))
					{
						Attributes.Add(key, text);
					}
				}
			}
		}

		internal void TranslateAttributes(CssBox box)
		{
			string text = ((string)(object)TagName).ToUpper();
			foreach (string key in Attributes.Keys)
			{
				string text2 = Attributes[key];
				switch (key)
				{
				case "align":
					switch (text2)
					{
					default:
						box.VerticalAlign = text2;
						break;
					case "left":
					case "center":
					case "right":
					case "justify":
						box.TextAlign = text2;
						break;
					}
					break;
				case "background":
					box.BackgroundImage = text2;
					break;
				case "bgcolor":
					box.BackgroundColor = text2;
					break;
				case "border":
					box.BorderWidth = method_0(text2);
					if (text == "TABLE")
					{
						method_1(box, text2);
					}
					else
					{
						box.BorderStyle = "solid";
					}
					break;
				case "bordercolor":
					box.BorderColor = text2;
					break;
				case "cellspacing":
					box.BorderSpacing = method_0(text2);
					break;
				case "cellpadding":
					method_2(box, text2);
					break;
				case "color":
					box.Color = text2;
					break;
				case "dir":
					box.Direction = text2;
					break;
				case "face":
					box.FontFamily = text2;
					break;
				case "height":
					box.Height = method_0(text2);
					break;
				case "hspace":
				{
					string text8 = (box.MarginRight = (box.MarginLeft = method_0(text2)));
					break;
				}
				case "nowrap":
					box.WhiteSpace = "nowrap";
					break;
				case "size":
					if (text == "HR")
					{
						box.Height = method_0(text2);
					}
					break;
				case "valign":
					box.VerticalAlign = text2;
					break;
				case "vspace":
				{
					string text5 = (box.MarginTop = (box.MarginBottom = method_0(text2)));
					break;
				}
				case "width":
					box.Width = method_0(text2);
					break;
				}
			}
		}

		private string method_0(string string_1)
		{
			CssLength cssLength = new CssLength(string_1);
			if (!cssLength.HasError)
			{
				return string_1;
			}
			return string_1 + "px";
		}

		private void method_1(CssBox cssBox_0, string string_1)
		{
			foreach (CssBox box in cssBox_0.Boxes)
			{
				foreach (CssBox box2 in box.Boxes)
				{
					box2.BorderWidth = method_0(string_1);
				}
			}
		}

		private void method_2(CssBox cssBox_0, string string_1)
		{
			foreach (CssBox box in cssBox_0.Boxes)
			{
				foreach (CssBox box2 in box.Boxes)
				{
					box2.Padding = method_0(string_1);
				}
			}
		}

		public bool HasAttribute(string attribute)
		{
			return Attributes.ContainsKey(attribute);
		}

		public override string ToString()
		{
			return string.Format("<{1}{0}>", TagName, (!IsClosing) ? string.Empty : "/");
		}

		internal static void bdmgwk6NgsmS7bWyHly()
		{
		}

		internal static bool Nm5aaO6LwfVS1dNsvdo()
		{
			return ub38TZ6qX0XamRhBaFW == null;
		}
	}
}
