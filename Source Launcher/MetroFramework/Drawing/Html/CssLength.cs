using System;
using System.Globalization;

namespace MetroFramework.Drawing.Html
{
	public class CssLength
	{
		public enum CssUnit
		{
			None,
			Ems,
			Pixels,
			Ex,
			Inches,
			Centimeters,
			Milimeters,
			Points,
			Picas
		}

		private float float_0;

		private bool bool_0;

		private CssUnit cssUnit_0;

		private string string_0;

		private bool bool_1;

		private bool bool_2;

		internal static CssLength l4hMJflsg6s7ftVaRTQ;

		public float Number => float_0;

		public bool HasError => bool_2;

		public bool IsPercentage => bool_1;

		public bool IsRelative => bool_0;

		public CssUnit Unit => cssUnit_0;

		public string Length => string_0;

		public CssLength(string length)
		{
			string_0 = length;
			float_0 = 0f;
			cssUnit_0 = CssUnit.None;
			bool_1 = false;
			if (string.IsNullOrEmpty(length) || length == "0")
			{
				return;
			}
			if (((string)(object)length).EndsWith("%"))
			{
				float_0 = CssValue.ParseNumber(length, 1f);
				bool_1 = true;
				return;
			}
			if (((string)(object)length).Length < 3)
			{
				float.TryParse(length, out float_0);
				bool_2 = true;
				return;
			}
			string text = ((string)(object)length).Substring(((string)(object)length).Length - 2, 2);
			string s = ((string)(object)length).Substring(0, ((string)(object)length).Length - 2);
			switch (text)
			{
			case "em":
				cssUnit_0 = CssUnit.Ems;
				bool_0 = true;
				goto IL_01b5;
			case "ex":
				cssUnit_0 = CssUnit.Ex;
				bool_0 = true;
				goto IL_01b5;
			case "px":
				cssUnit_0 = CssUnit.Pixels;
				bool_0 = true;
				goto IL_01b5;
			case "mm":
				cssUnit_0 = CssUnit.Milimeters;
				goto IL_01b5;
			case "cm":
				cssUnit_0 = CssUnit.Centimeters;
				goto IL_01b5;
			case "in":
				cssUnit_0 = CssUnit.Inches;
				goto IL_01b5;
			case "pt":
				cssUnit_0 = CssUnit.Points;
				goto IL_01b5;
			case "pc":
				cssUnit_0 = CssUnit.Picas;
				goto IL_01b5;
			default:
				{
					bool_2 = true;
					break;
				}
				IL_01b5:
				if (!float.TryParse(s, NumberStyles.Number, NumberFormatInfo.InvariantInfo, out float_0))
				{
					bool_2 = true;
				}
				break;
			}
		}

		public CssLength ConvertEmToPoints(float emSize)
		{
			if (!HasError)
			{
				if (Unit != CssUnit.Ems)
				{
					throw new InvalidOperationException("Length is not in ems");
				}
				return new CssLength(string.Format("{0}pt", ((float)Convert.ToSingle(Number * emSize)).ToString("0.0", NumberFormatInfo.InvariantInfo)));
			}
			throw new InvalidOperationException("Invalid length");
		}

		public CssLength ConvertEmToPixels(float pixelFactor)
		{
			if (!HasError)
			{
				if (Unit != CssUnit.Ems)
				{
					throw new InvalidOperationException("Length is not in ems");
				}
				return new CssLength(string.Format("{0}px", ((float)Convert.ToSingle(Number * pixelFactor)).ToString("0.0", NumberFormatInfo.InvariantInfo)));
			}
			throw new InvalidOperationException("Invalid length");
		}

		public override string ToString()
		{
			if (!HasError)
			{
				if (!IsPercentage)
				{
					string text = string.Empty;
					switch (Unit)
					{
					case CssUnit.Ems:
						text = "em";
						break;
					case CssUnit.Pixels:
						text = "px";
						break;
					case CssUnit.Ex:
						text = "ex";
						break;
					case CssUnit.Inches:
						text = "in";
						break;
					case CssUnit.Centimeters:
						text = "cm";
						break;
					case CssUnit.Milimeters:
						text = "mm";
						break;
					case CssUnit.Points:
						text = "pt";
						break;
					case CssUnit.Picas:
						text = "pc";
						break;
					}
					return string.Format(NumberFormatInfo.InvariantInfo, "{0}{1}", (float)Number, text);
				}
				return string.Format(NumberFormatInfo.InvariantInfo, "{0}%", (float)Number);
			}
			return string.Empty;
		}

		internal static bool V40VCWlNf7hhPd0W5Sc()
		{
			return l4hMJflsg6s7ftVaRTQ == null;
		}
	}
}
