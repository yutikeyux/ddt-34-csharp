using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;

namespace MetroFramework.Drawing.Html
{
	[CLSCompliant(false)]
	public class InitialContainer : CssBox
	{
		private Dictionary<string, Dictionary<string, CssBlock>> dictionary_2;

		private string string_69;

		private bool bool_1;

		private SizeF sizeF_1;

		private PointF pointF_1;

		private Dictionary<CssBox, RectangleF> dictionary_3;

		private bool bool_2;

		internal static InitialContainer qqyODP69DAu7Ub0N3ip;

		internal Dictionary<CssBox, RectangleF> LinkRegions => dictionary_3;

		public Dictionary<string, Dictionary<string, CssBlock>> MediaBlocks => dictionary_2;

		public string DocumentSource => string_69;

		public bool AvoidGeometryAntialias
		{
			get
			{
				return bool_1;
			}
			set
			{
				bool_1 = value;
			}
		}

		public bool AvoidTextAntialias
		{
			get
			{
				return bool_2;
			}
			set
			{
				bool_2 = value;
			}
		}

		public SizeF MaximumSize
		{
			get
			{
				return sizeF_1;
			}
			set
			{
				sizeF_1 = value;
			}
		}

		public PointF ScrollOffset
		{
			get
			{
				return pointF_1;
			}
			set
			{
				pointF_1 = value;
			}
		}

		public InitialContainer()
		{
			_initialContainer = this;
			dictionary_2 = new Dictionary<string, Dictionary<string, CssBlock>>();
			dictionary_3 = new Dictionary<CssBox, RectangleF>();
			MediaBlocks.Add("all", new Dictionary<string, CssBlock>());
			base.Display = "block";
			FeedStyleSheet("\r\n\r\n        \r\n        html, address,\r\n        blockquote,\r\n        body, dd, div,\r\n        dl, dt, fieldset, form,\r\n        frame, frameset,\r\n        h1, h2, h3, h4,\r\n        h5, h6, noframes,\r\n        ol, p, ul, center,\r\n        dir, hr, menu, pre   { display: block }\r\n        li              { display: list-item }\r\n        head            { display: none }\r\n        table           { display: table }\r\n        tr              { display: table-row }\r\n        thead           { display: table-header-group }\r\n        tbody           { display: table-row-group }\r\n        tfoot           { display: table-footer-group }\r\n        col             { display: table-column }\r\n        colgroup        { display: table-column-group }\r\n        td, th          { display: table-cell }\r\n        caption         { display: table-caption }\r\n        th              { font-weight: bolder; text-align: center }\r\n        caption         { text-align: center }\r\n        body            { margin: 8px }\r\n        h1              { font-size: 2em; margin: .67em 0 }\r\n        h2              { font-size: 1.5em; margin: .75em 0 }\r\n        h3              { font-size: 1.17em; margin: .83em 0 }\r\n        h4, p,\r\n        blockquote, ul,\r\n        fieldset, form,\r\n        ol, dl, dir,\r\n        menu            { margin: 1.12em 0 }\r\n        h5              { font-size: .83em; margin: 1.5em 0 }\r\n        h6              { font-size: .75em; margin: 1.67em 0 }\r\n        h1, h2, h3, h4,\r\n        h5, h6, b,\r\n        strong          { font-weight: bolder; }\r\n        blockquote      { margin-left: 40px; margin-right: 40px }\r\n        i, cite, em,\r\n        var, address    { font-style: italic }\r\n        pre, tt, code,\r\n        kbd, samp       { font-family: monospace }\r\n        pre             { white-space: pre }\r\n        button, textarea,\r\n        input, select   { display: inline-block }\r\n        big             { font-size: 1.17em }\r\n        small, sub, sup { font-size: .83em }\r\n        sub             { vertical-align: sub }\r\n        sup             { vertical-align: super }\r\n        table           { border-spacing: 2px; }\r\n        thead, tbody,\r\n        tfoot           { vertical-align: middle }\r\n        td, th          { vertical-align: inherit }\r\n        s, strike, del  { text-decoration: line-through }\r\n        hr              { border: 1px inset }\r\n        ol, ul, dir,\r\n        menu, dd        { margin-left: 40px }\r\n        ol              { list-style-type: decimal }\r\n        ol ul, ul ol,\r\n        ul ul, ol ol    { margin-top: 0; margin-bottom: 0 }\r\n        u, ins          { text-decoration: underline }\r\n        br:before       { content: \"\\A\" }\r\n        :before, :after { white-space: pre-line }\r\n        center          { text-align: center }\r\n        :link, :visited { text-decoration: underline }\r\n        :focus          { outline: thin dotted invert }\r\n\r\n        /* Begin bidirectionality settings (do not change) */\r\n        BDO[DIR=\"ltr\"]  { direction: ltr; unicode-bidi: bidi-override }\r\n        BDO[DIR=\"rtl\"]  { direction: rtl; unicode-bidi: bidi-override }\r\n\r\n        *[DIR=\"ltr\"]    { direction: ltr; unicode-bidi: embed }\r\n        *[DIR=\"rtl\"]    { direction: rtl; unicode-bidi: embed }\r\n\r\n        @media print {\r\n          h1            { page-break-before: always }\r\n          h1, h2, h3,\r\n          h4, h5, h6    { page-break-after: avoid }\r\n          ul, ol, dl    { page-break-before: avoid }\r\n        }\r\n\r\n        /* Not in the specification but necessary */\r\n        a               { color:blue; text-decoration:underline }\r\n        table           { border-color:#dfdfdf; border-style:outset; }\r\n        td, th          { border-color:#dfdfdf; border-style:inset; }\r\n        style, title,\r\n        script, link,\r\n        meta, area,\r\n        base, param     { display:none }\r\n        hr              { border-color: #ccc }  \r\n        pre             { font-size:10pt }\r\n        \r\n        /*This is the background of the HtmlToolTip*/\r\n        .htmltooltipbackground {\r\n              border:solid 1px #767676;\r\n              corner-radius:3px;\r\n              background-color:#white;\r\n              background-gradient:#E4E5F0;\r\n        }\r\n\r\n        ");
		}

		public InitialContainer(string documentSource)
			: this()
		{
			string_69 = documentSource;
			method_15();
			method_17(this);
			method_18(this);
		}

		public void FeedStyleSheet(string stylesheet)
		{
			if (string.IsNullOrEmpty(stylesheet))
			{
				return;
			}
			stylesheet = ((string)(object)stylesheet).ToLower();
			MatchCollection matchCollection = Parser.Match("/\\*[^*/]*\\*/", stylesheet);
			while (matchCollection.Count > 0)
			{
				stylesheet = ((string)(object)stylesheet).Remove(matchCollection[0].Index, matchCollection[0].Length);
				matchCollection = Parser.Match("/\\*[^*/]*\\*/", stylesheet);
			}
			MatchCollection matchCollection2 = Parser.Match("@.*\\{\\s*(\\s*[^\\{\\}]*\\{[^\\{\\}]*\\}\\s*)*\\s*\\}", stylesheet);
			while (matchCollection2.Count > 0)
			{
				Match match = matchCollection2[0];
				string value = match.Value;
				stylesheet = ((string)(object)stylesheet).Remove(match.Index, match.Length);
				if (((string)(object)value).StartsWith("@media"))
				{
					MatchCollection matchCollection3 = Parser.Match("@media[^\\{\\}]*\\{", value);
					if (matchCollection3.Count == 1)
					{
						string value2 = matchCollection3[0].Value;
						if (((string)(object)value2).StartsWith("@media") && ((string)(object)value2).EndsWith("{"))
						{
							string[] array = ((string)(object)((string)(object)value2).Substring(6, ((string)(object)value2).Length - 7)).Split((char[])(object)new char[1] { ' ' });
							for (int i = 0; i < array.Length; i++)
							{
								if (string.IsNullOrEmpty(((string)(object)array[i]).Trim()))
								{
									continue;
								}
								MatchCollection matchCollection4 = Parser.Match("[^\\{\\}]*\\{[^\\{\\}]*\\}", value);
								foreach (Match item in matchCollection4)
								{
									method_14(((string)(object)array[i]).Trim(), item.Value);
								}
							}
						}
					}
				}
				matchCollection2 = Parser.Match("@.*\\{\\s*(\\s*[^\\{\\}]*\\{[^\\{\\}]*\\}\\s*)*\\s*\\}", stylesheet);
			}
			MatchCollection matchCollection5 = Parser.Match("[^\\{\\}]*\\{[^\\{\\}]*\\}", stylesheet);
			foreach (Match item2 in matchCollection5)
			{
				method_14("all", item2.Value);
			}
		}

		private void method_14(string string_70, string string_71)
		{
			if (string.IsNullOrEmpty(string_70))
			{
				string_70 = "all";
			}
			int num = ((string)(object)string_71).IndexOf("{");
			string blockSource = ((string)(object)((string)(object)((string)(object)string_71).Substring(num)).Replace("{", string.Empty)).Replace("}", string.Empty);
			if (num < 0)
			{
				return;
			}
			string[] array = ((string)(object)((string)(object)string_71).Substring(0, num)).Split((char[])(object)new char[1] { ',' });
			for (int i = 0; i < array.Length; i++)
			{
				string text = ((string)(object)array[i]).Trim();
				if (string.IsNullOrEmpty(text))
				{
					continue;
				}
				CssBlock cssBlock = new CssBlock(blockSource);
				if (!MediaBlocks.ContainsKey(string_70))
				{
					MediaBlocks.Add(string_70, new Dictionary<string, CssBlock>());
				}
				if (MediaBlocks[string_70].ContainsKey(text))
				{
					CssBlock cssBlock2 = MediaBlocks[string_70][text];
					foreach (string key in cssBlock.Properties.Keys)
					{
						if (!cssBlock2.Properties.ContainsKey(key))
						{
							cssBlock2.Properties.Add(key, cssBlock.Properties[key]);
						}
						else
						{
							cssBlock2.Properties[key] = cssBlock.Properties[key];
						}
					}
					cssBlock2.UpdatePropertyValues();
				}
				else
				{
					MediaBlocks[string_70].Add(text, cssBlock);
				}
			}
		}

		private void method_15()
		{
			MatchCollection matchCollection = Parser.Match("<[^<>]*>", DocumentSource);
			CssBox cssBox = this;
			int num = -1;
			foreach (Match item in matchCollection)
			{
				string text = ((item.Index <= 0) ? string.Empty : ((string)(object)DocumentSource).Substring(num + 1, item.Index - num - 1));
				if (string.IsNullOrEmpty(((string)(object)text).Trim()))
				{
					if (text != null && ((string)(object)text).Length > 0)
					{
						CssAnonymousSpaceBox cssAnonymousSpaceBox = new CssAnonymousSpaceBox(cssBox);
						cssAnonymousSpaceBox.Text = text;
					}
				}
				else
				{
					CssAnonymousBox cssAnonymousBox = new CssAnonymousBox(cssBox);
					cssAnonymousBox.Text = text;
				}
				HtmlTag htmlTag = new HtmlTag(item.Value);
				if (htmlTag.IsClosing)
				{
					cssBox = method_16(htmlTag.TagName, cssBox);
				}
				else if (!htmlTag.IsSingle)
				{
					cssBox = new CssBox(cssBox, htmlTag);
				}
				else
				{
					new CssBox(cssBox, htmlTag);
				}
				num = item.Index + item.Length - 1;
			}
			string text2 = ((string)(object)DocumentSource).Substring((num > 0) ? (num + 1) : 0, ((string)(object)DocumentSource).Length - num - 1 + ((num == 0) ? 1 : 0));
			if (!string.IsNullOrEmpty(text2))
			{
				CssAnonymousBox cssAnonymousBox2 = new CssAnonymousBox(cssBox);
				cssAnonymousBox2.Text = text2;
			}
		}

		private CssBox method_16(string string_70, CssBox cssBox_2)
		{
			if (cssBox_2 != null)
			{
				if (cssBox_2.HtmlTag == null || !((string)(object)cssBox_2.HtmlTag.TagName).Equals(string_70, StringComparison.CurrentCultureIgnoreCase))
				{
					return method_16(string_70, cssBox_2.ParentBox);
				}
				if (cssBox_2.ParentBox == null)
				{
					return base.InitialContainer;
				}
				return cssBox_2.ParentBox;
			}
			return base.InitialContainer;
		}

		private void method_17(CssBox cssBox_2)
		{
			bool flag = false;
			foreach (CssBox box in cssBox_2.Boxes)
			{
				box.InheritStyle();
				if (box.HtmlTag != null)
				{
					if (MediaBlocks["all"].ContainsKey(box.HtmlTag.TagName))
					{
						MediaBlocks["all"][box.HtmlTag.TagName].AssignTo(box);
					}
					if (box.HtmlTag.HasAttribute("class") && MediaBlocks["all"].ContainsKey("." + box.HtmlTag.Attributes["class"]))
					{
						MediaBlocks["all"]["." + box.HtmlTag.Attributes["class"]].AssignTo(box);
					}
					box.HtmlTag.TranslateAttributes(box);
					if (box.HtmlTag.HasAttribute("style"))
					{
						CssBlock cssBlock = new CssBlock(box.HtmlTag.Attributes["style"]);
						cssBlock.AssignTo(box);
					}
					if (((string)(object)box.HtmlTag.TagName).Equals("style", StringComparison.CurrentCultureIgnoreCase) && box.Boxes.Count == 1)
					{
						FeedStyleSheet(box.Boxes[0].Text);
					}
					if (((string)(object)box.HtmlTag.TagName).Equals("link", StringComparison.CurrentCultureIgnoreCase) && ((string)(object)box.GetAttribute("rel", string.Empty)).Equals("stylesheet", StringComparison.CurrentCultureIgnoreCase))
					{
						FeedStyleSheet(CssValue.GetStyleSheet(box.GetAttribute("href", string.Empty)));
					}
				}
				method_17(box);
			}
			if (!flag)
			{
				return;
			}
			foreach (CssBox box2 in cssBox_2.Boxes)
			{
				box2.Display = "block";
			}
		}

		private void method_18(CssBox cssBox_2)
		{
			if (!cssBox_2.ContainsInlinesOnly())
			{
				List<List<CssBox>> list = method_19(cssBox_2);
				foreach (List<CssBox> item in list)
				{
					if (item.Count == 0)
					{
						continue;
					}
					if (item.Count == 1 && item[0] is CssAnonymousSpaceBox)
					{
						CssAnonymousSpaceBlockBox parentBox = new CssAnonymousSpaceBlockBox(cssBox_2, item[0]);
						item[0].ParentBox = parentBox;
						continue;
					}
					CssAnonymousBlockBox parentBox2 = new CssAnonymousBlockBox(cssBox_2, item[0]);
					foreach (CssBox item2 in item)
					{
						item2.ParentBox = parentBox2;
					}
				}
			}
			foreach (CssBox box in cssBox_2.Boxes)
			{
				method_18(box);
			}
		}

		private List<List<CssBox>> method_19(CssBox cssBox_2)
		{
			List<List<CssBox>> list = new List<List<CssBox>>();
			List<CssBox> list2 = null;
			for (int i = 0; i < cssBox_2.Boxes.Count; i++)
			{
				CssBox cssBox = cssBox_2.Boxes[i];
				if (cssBox.Display == "inline")
				{
					if (list2 == null)
					{
						list2 = new List<CssBox>();
						list.Add(list2);
					}
					list2.Add(cssBox);
				}
				else
				{
					list2 = null;
				}
			}
			if (list.Count > 0 && list[list.Count - 1].Count == 0)
			{
				list.RemoveAt(list.Count - 1);
			}
			return list;
		}

		public override void MeasureBounds(Graphics g)
		{
			LinkRegions.Clear();
			base.MeasureBounds(g);
		}

		internal static void ODyMFX6ipSY3NoXG6MR()
		{
		}

		internal static bool gCVkZ06ZDLHush3CS0L()
		{
			return qqyODP69DAu7Ub0N3ip == null;
		}
	}
}
