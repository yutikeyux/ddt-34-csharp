using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace MetroFramework.Drawing.Html
{
	[CLSCompliant(false)]
	public static class CssValue
	{
		internal static object V0lgSSpTds54mtIQCfr;

		public static float ParseNumber(string number, float hundredPercent)
		{
			if (string.IsNullOrEmpty(number))
			{
				return 0f;
			}
			string s = number;
			bool flag = ((string)(object)number).EndsWith("%");
			float result = 0f;
			if (flag)
			{
				s = ((string)(object)number).Substring(0, ((string)(object)number).Length - 1);
			}
			if (float.TryParse(s, NumberStyles.Number, NumberFormatInfo.InvariantInfo, out result))
			{
				if (flag)
				{
					result = result / 100f * hundredPercent;
				}
				return result;
			}
			return 0f;
		}

		public static float ParseLength(string length, float hundredPercent, CssBox box)
		{
			return ParseLength(length, hundredPercent, box, box.GetEmHeight(), returnPoints: false);
		}

		public static float ParseLength(string length, float hundredPercent, CssBox box, float emFactor, bool returnPoints)
		{
			if (!string.IsNullOrEmpty(length) && !(length == "0"))
			{
				if (((string)(object)length).EndsWith("%"))
				{
					return ParseNumber(length, hundredPercent);
				}
				if (((string)(object)length).Length < 3)
				{
					return 0f;
				}
				string text = ((string)(object)length).Substring(((string)(object)length).Length - 2, 2);
				float num = 1f;
				string number = ((string)(object)length).Substring(0, ((string)(object)length).Length - 2);
				switch (text)
				{
				default:
					num = 0f;
					break;
				case "em":
					num = emFactor;
					break;
				case "px":
					num = 1f;
					break;
				case "mm":
					num = 3f;
					break;
				case "cm":
					num = 37f;
					break;
				case "in":
					num = 96f;
					break;
				case "pt":
					num = 1.33333337f;
					if (returnPoints)
					{
						return ParseNumber(number, hundredPercent);
					}
					break;
				case "pc":
					num = 16f;
					break;
				}
				return num * ParseNumber(number, hundredPercent);
			}
			return 0f;
		}

		public static Color GetActualColor(string colorValue)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			Color empty = Color.Empty;
			if (!string.IsNullOrEmpty(colorValue))
			{
				colorValue = ((string)(object)((string)(object)colorValue).ToLower()).Trim();
				if (((string)(object)colorValue).StartsWith("#"))
				{
					string text = ((string)(object)colorValue).Substring(1);
					if (((string)(object)text).Length == 6)
					{
						num = int.Parse(((string)(object)text).Substring(0, 2), NumberStyles.HexNumber);
						num2 = int.Parse(((string)(object)text).Substring(2, 2), NumberStyles.HexNumber);
						num3 = int.Parse(((string)(object)text).Substring(4, 2), NumberStyles.HexNumber);
					}
					else
					{
						if (((string)(object)text).Length != 3)
						{
							return empty;
						}
						num = int.Parse((string)(object)new string(((string)(object)((string)(object)text).Substring(0, 1))[0], 2), NumberStyles.HexNumber);
						num2 = int.Parse((string)(object)new string(((string)(object)((string)(object)text).Substring(1, 1))[0], 2), NumberStyles.HexNumber);
						num3 = int.Parse((string)(object)new string(((string)(object)((string)(object)text).Substring(2, 1))[0], 2), NumberStyles.HexNumber);
					}
				}
				else if (((string)(object)colorValue).StartsWith("rgb(") && ((string)(object)colorValue).EndsWith(")"))
				{
					string text2 = ((string)(object)colorValue).Substring(4, ((string)(object)colorValue).Length - 5);
					string[] array = ((string)(object)text2).Split((char[])(object)new char[1] { ',' });
					if (array.Length != 3)
					{
						return empty;
					}
					num = Convert.ToInt32(ParseNumber(((string)(object)array[0]).Trim(), 255f));
					num2 = Convert.ToInt32(ParseNumber(((string)(object)array[1]).Trim(), 255f));
					num3 = Convert.ToInt32(ParseNumber(((string)(object)array[2]).Trim(), 255f));
				}
				else
				{
					string text3 = string.Empty;
					switch (colorValue)
					{
					case "maroon":
						text3 = "#800000";
						break;
					case "red":
						text3 = "#ff0000";
						break;
					case "orange":
						text3 = "#ffA500";
						break;
					case "olive":
						text3 = "#808000";
						break;
					case "purple":
						text3 = "#800080";
						break;
					case "fuchsia":
						text3 = "#ff00ff";
						break;
					case "white":
						text3 = "#ffffff";
						break;
					case "lime":
						text3 = "#00ff00";
						break;
					case "green":
						text3 = "#008000";
						break;
					case "navy":
						text3 = "#000080";
						break;
					case "blue":
						text3 = "#0000ff";
						break;
					case "aqua":
						text3 = "#00ffff";
						break;
					case "teal":
						text3 = "#008080";
						break;
					case "black":
						text3 = "#000000";
						break;
					case "silver":
						text3 = "#c0c0c0";
						break;
					case "gray":
						text3 = "#808080";
						break;
					case "yellow":
						text3 = "#FFFF00";
						break;
					}
					if (string.IsNullOrEmpty(text3))
					{
						return empty;
					}
					Color actualColor = GetActualColor(text3);
					num = actualColor.R;
					num2 = actualColor.G;
					num3 = actualColor.B;
				}
				return Color.FromArgb(num, num2, num3);
			}
			return empty;
		}

		public static float GetActualBorderWidth(string borderValue, CssBox b)
		{
			if (string.IsNullOrEmpty(borderValue))
			{
				return GetActualBorderWidth("medium", b);
			}
			return borderValue switch
			{
				"thin" => 1f, 
				"medium" => 2f, 
				"thick" => 4f, 
				_ => Math.Abs(ParseLength(borderValue, 1f, b)), 
			};
		}

		public static string[] SplitValues(string value)
		{
			return SplitValues(value, ' ');
		}

		public static string[] SplitValues(string value, char separator)
		{
			if (string.IsNullOrEmpty(value))
			{
				return (string[])(object)new string[0];
			}
			string[] array = ((string)(object)value).Split((char[])(object)new char[1] { (char)separator });
			List<string> list = new List<string>();
			for (int i = 0; i < array.Length; i++)
			{
				string text = ((string)(object)array[i]).Trim();
				if (!string.IsNullOrEmpty(text))
				{
					list.Add(text);
				}
			}
			return list.ToArray();
		}

		private static Type smethod_0(string string_0, ref string string_1)
		{
			int num = ((string)(object)string_0).LastIndexOf('.');
			if (num >= 0)
			{
				string name = ((string)(object)string_0).Substring(0, num);
				string_1 = ((string)(object)string_0).Substring(num + 1);
				string_1 = ((string)(object)((string)(object)string_1).Replace("(", string.Empty)).Replace(")", string.Empty);
				foreach (Assembly reference in HtmlRenderer.References)
				{
					Type type = reference.GetType(name, throwOnError: false, ignoreCase: true);
					if (type != null)
					{
						return type;
					}
				}
				return null;
			}
			return null;
		}

		private static object smethod_1(string string_0)
		{
			if (((string)(object)string_0).StartsWith("method:", StringComparison.CurrentCultureIgnoreCase))
			{
				string string_ = string.Empty;
				Type type = smethod_0(((string)(object)string_0).Substring(7), ref string_);
				if (type == null)
				{
					return null;
				}
				MethodInfo method = type.GetMethod(string_);
				if (method.IsStatic && method.GetParameters().Length <= 0)
				{
					return method;
				}
				return null;
			}
			if (!((string)(object)string_0).StartsWith("property:", StringComparison.CurrentCultureIgnoreCase))
			{
				if (!Uri.IsWellFormedUriString(string_0, UriKind.RelativeOrAbsolute))
				{
					return new FileInfo(string_0);
				}
				return new Uri(string_0);
			}
			string string_2 = string.Empty;
			return smethod_0(((string)(object)string_0).Substring(9), ref string_2)?.GetProperty(string_2);
		}

		public static Image GetImage(string path)
		{
			object obj = smethod_1(path);
			FileInfo fileInfo = obj as FileInfo;
			PropertyInfo propertyInfo = obj as PropertyInfo;
			MethodInfo methodInfo = obj as MethodInfo;
			try
			{
				if (fileInfo != null)
				{
					if (!fileInfo.Exists)
					{
						return null;
					}
					return Image.FromFile(fileInfo.FullName);
				}
				if (propertyInfo == null)
				{
					if (methodInfo == null)
					{
						return null;
					}
					if (!methodInfo.ReturnType.IsSubclassOf(Type.GetTypeFromHandle((RuntimeTypeHandle)typeof(Image).TypeHandle)))
					{
						return null;
					}
					return methodInfo.Invoke(null, null) as Image;
				}
				if (propertyInfo.PropertyType.IsSubclassOf(Type.GetTypeFromHandle((RuntimeTypeHandle)typeof(Image).TypeHandle)) || propertyInfo.PropertyType.Equals(Type.GetTypeFromHandle((RuntimeTypeHandle)typeof(Image).TypeHandle)))
				{
					return propertyInfo.GetValue(null, null) as Image;
				}
				return null;
			}
			catch
			{
				return new Bitmap(50, 50);
			}
		}

		public static string GetStyleSheet(string path)
		{
			object obj = smethod_1(path);
			FileInfo fileInfo = obj as FileInfo;
			PropertyInfo propertyInfo = obj as PropertyInfo;
			MethodInfo methodInfo = obj as MethodInfo;
			try
			{
				if (fileInfo != null)
				{
					if (fileInfo.Exists)
					{
						StreamReader streamReader = new StreamReader(fileInfo.FullName);
						string result = streamReader.ReadToEnd();
						streamReader.Dispose();
						return result;
					}
					return null;
				}
				if (propertyInfo == null)
				{
					if (methodInfo != null)
					{
						if (!methodInfo.ReturnType.Equals(Type.GetTypeFromHandle((RuntimeTypeHandle)typeof(string).TypeHandle)))
						{
							return null;
						}
						return (string)(object)(methodInfo.Invoke(null, null) as string);
					}
					return string.Empty;
				}
				if (propertyInfo.PropertyType.Equals(Type.GetTypeFromHandle((RuntimeTypeHandle)typeof(string).TypeHandle)))
				{
					return (string)(object)(propertyInfo.GetValue(null, null) as string);
				}
				return null;
			}
			catch
			{
				return string.Empty;
			}
		}

		public static void GoLink(string href)
		{
			object obj = smethod_1(href);
			FileInfo fileInfo = obj as FileInfo;
			MethodInfo methodInfo = obj as MethodInfo;
			Uri uri = obj as Uri;
			try
			{
				if (fileInfo == null && !(uri != null))
				{
					methodInfo?.Invoke(null, null);
					return;
				}
				ProcessStartInfo processStartInfo = new ProcessStartInfo(href);
				processStartInfo.UseShellExecute = true;
				Process.Start(processStartInfo);
			}
			catch
			{
				throw;
			}
		}

		internal static bool Qb3ZvApJ33V9YUTpZ2x()
		{
			return V0lgSSpTds54mtIQCfr == null;
		}
	}
}
