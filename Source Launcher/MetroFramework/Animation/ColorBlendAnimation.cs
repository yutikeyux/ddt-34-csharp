using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace MetroFramework.Animation
{
	public sealed class ColorBlendAnimation : AnimationBase
	{
		private double double_0 = 1.0;

		private static ColorBlendAnimation tpBvtaNWnWZNRoBb47Z;

		public void Start(Control control, string property, Color targetColor, int duration)
		{
			if (duration == 0)
			{
				duration = 1;
			}
			Start(control, transitionType, 2 * duration, delegate
			{
				Color color_ = method_3(property, control);
				Color color2 = method_2(color_, targetColor, 0.1 * (double_0 / 2.0));
				PropertyInfo property2 = control.GetType().GetProperty(property);
				MethodInfo setMethod = property2.GetSetMethod(nonPublic: true);
				setMethod.Invoke(control, new object[1] { color2 });
			}, delegate
			{
				Color color = method_3(property, control);
				return (((byte)color.A).Equals(targetColor.A) && ((byte)color.R).Equals(targetColor.R) && ((byte)color.G).Equals(targetColor.G) && ((byte)color.B).Equals(targetColor.B)) ? true : false;
			});
		}

		private Color method_2(Color color_0, Color color_1, double double_1)
		{
			double_0 += 0.2;
			int alpha = (int)Math.Round((double)(int)color_0.A * (1.0 - double_1) + (double)(int)color_1.A * double_1);
			int red = (int)Math.Round((double)(int)color_0.R * (1.0 - double_1) + (double)(int)color_1.R * double_1);
			int green = (int)Math.Round((double)(int)color_0.G * (1.0 - double_1) + (double)(int)color_1.G * double_1);
			int blue = (int)Math.Round((double)(int)color_0.B * (1.0 - double_1) + (double)(int)color_1.B * double_1);
			return Color.FromArgb(alpha, red, green, blue);
		}

		private Color method_3(string string_0, Control control_1)
		{
			Type type = control_1.GetType();
			Binder binder = null;
			object[] args = null;
			object obj = type.InvokeMember(string_0, BindingFlags.GetProperty, binder, control_1, args);
			return (Color)obj;
		}

		internal static bool ErJ88sN3jInSQCHUxpI()
		{
			return tpBvtaNWnWZNRoBb47Z == null;
		}
	}
}
