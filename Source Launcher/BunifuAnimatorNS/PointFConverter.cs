using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;

namespace BunifuAnimatorNS
{
	[DebuggerStepThrough]
	public class PointFConverter : ExpandableObjectConverter
	{
		internal static PointFConverter duR5BOIsroV9UqyM7HE5;

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
			{
				return true;
			}
			return base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (!(value is string))
			{
				return base.ConvertFrom(context, culture, value);
			}
			try
			{
				string[] array = ((string)value).Split(',');
				float num = 0f;
				float num2 = 0f;
				if (array.Length > 1)
				{
					num = float.Parse(array[0].Trim().Trim('{', 'X', 'x', '='));
					num2 = float.Parse(array[1].Trim().Trim('}', 'Y', 'y', '='));
				}
				else if (array.Length == 1)
				{
					num = float.Parse(array[0].Trim());
					num2 = 0f;
				}
				else
				{
					num = 0f;
					num2 = 0f;
				}
				return new PointF(num, num2);
			}
			catch
			{
				throw new ArgumentException("Cannot convert [" + value.ToString() + "] to pointF");
			}
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string) && value.GetType() == typeof(PointF))
			{
				PointF pointF = (PointF)value;
				return $"{{X={pointF.X}, Y={pointF.Y}}}";
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}

		internal static void kdK4ZoINPBocfAoQSi6n()
		{
		}

		internal static bool GFRGKKIsQYCdkLgCXuIo()
		{
			return duR5BOIsroV9UqyM7HE5 == null;
		}
	}
}
