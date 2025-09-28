using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MetroFramework.Drawing.Html
{
	[CLSCompliant(false)]
	public class CssBlock
	{
		private string string_0;

		private Dictionary<PropertyInfo, string> dictionary_0;

		private Dictionary<string, string> dictionary_1;

		internal static CssBlock iqdoTf4CVLugM3enyXo;

		public Dictionary<string, string> Properties => dictionary_1;

		public Dictionary<PropertyInfo, string> PropertyValues => dictionary_0;

		public string BlockSource => string_0;

		private CssBlock()
		{
			dictionary_0 = new Dictionary<PropertyInfo, string>();
			dictionary_1 = new Dictionary<string, string>();
		}

		public CssBlock(string blockSource)
			: this()
		{
			string_0 = blockSource;
			MatchCollection matchCollection = Parser.Match(";?[^;\\s]*:[^\\{\\}:;]*(\\}|;)?", blockSource);
			foreach (Match item in matchCollection)
			{
				string[] array = ((string)(object)item.Value).Split((char[])(object)new char[1] { ':' });
				if (array.Length == 2)
				{
					string key = ((string)(object)array[0]).Trim();
					string text = ((string)(object)array[1]).Trim();
					if (((string)(object)text).EndsWith(";"))
					{
						text = ((string)(object)((string)(object)text).Substring(0, ((string)(object)text).Length - 1)).Trim();
					}
					Properties.Add(key, text);
					if (CssBox._properties.ContainsKey(key))
					{
						PropertyValues.Add(CssBox._properties[key], text);
					}
				}
			}
		}

		internal void UpdatePropertyValues()
		{
			PropertyValues.Clear();
			foreach (string key in Properties.Keys)
			{
				if (CssBox._properties.ContainsKey(key))
				{
					PropertyValues.Add(CssBox._properties[key], Properties[key]);
				}
			}
		}

		public void AssignTo(CssBox b)
		{
			foreach (PropertyInfo key in PropertyValues.Keys)
			{
				string text = PropertyValues[key];
				if (text == "inherit" && b.ParentBox != null)
				{
					text = Convert.ToString(key.GetValue(b.ParentBox, null));
				}
				key.SetValue(b, text, null);
			}
		}

		internal static bool skspKJ4YHFFRVGeQj8g()
		{
			return iqdoTf4CVLugM3enyXo == null;
		}

		internal static void xLW5BF4HxBXIwPVl06v()
		{
		}
	}
}
