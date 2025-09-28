using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ns1
{
	public class BunifuColorTransition : Component
	{
		private int int_0;

		private Color color_0 = Color.White;

		private Color color_1 = Color.White;

		private Color xyqhWuUpjH = Color.White;

		[CompilerGenerated]
		private EventHandler eventHandler_0;

		private IContainer icontainer_0;

		internal static BunifuColorTransition rVqq82IOQImcEoxswiOn;

		public int ProgessValue
		{
			get
			{
				return int_0;
			}
			set
			{
				int_0 = value;
				method_0();
			}
		}

		public Color Value => xyqhWuUpjH;

		public Color Color1
		{
			get
			{
				return color_0;
			}
			set
			{
				color_0 = value;
				method_0();
			}
		}

		public Color Color2
		{
			get
			{
				return color_1;
			}
			set
			{
				color_1 = value;
				method_0();
			}
		}

		public event EventHandler OnValueChange
		{
			[CompilerGenerated]
			add
			{
				EventHandler eventHandler = eventHandler_0;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref eventHandler_0, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler eventHandler = eventHandler_0;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange(ref eventHandler_0, value2, eventHandler2);
				}
				while ((object)eventHandler != eventHandler2);
			}
		}

		public BunifuColorTransition()
		{
			method_1();
		}

		public BunifuColorTransition(IContainer container)
		{
			container.Add(this);
			method_1();
		}

		private void method_0()
		{
			Color colorScale = getColorScale(ProgessValue, Color1, Color2);
			if (colorScale != Value)
			{
				xyqhWuUpjH = colorScale;
				if (eventHandler_0 != null)
				{
					eventHandler_0(this, new EventArgs());
				}
			}
		}

		public static Color getColorScale(int Passentage, Color startColor, Color endColor)
		{
			try
			{
				int red = int.Parse(Math.Round((double)(int)startColor.R + (double)((endColor.R - startColor.R) * Passentage) * 0.01, 0).ToString());
				int green = int.Parse(Math.Round((double)(int)startColor.G + (double)((endColor.G - startColor.G) * Passentage) * 0.01, 0).ToString());
				int blue = int.Parse(Math.Round((double)(int)startColor.B + (double)((endColor.B - startColor.B) * Passentage) * 0.01, 0).ToString());
				return Color.FromArgb(255, red, green, blue);
			}
			catch (Exception)
			{
				return startColor;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && icontainer_0 != null)
			{
				icontainer_0.Dispose();
			}
			base.Dispose(disposing);
		}

		private void method_1()
		{
			icontainer_0 = new Container();
		}

		internal static void bhJ4AQIfIqm5d6FkBYGQ()
		{
		}

		internal static bool RdCcDTIOzsIVSuX24XxY()
		{
			return rVqq82IOQImcEoxswiOn == null;
		}
	}
}
