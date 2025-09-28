using System;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace MetroFramework.Drawing
{
	public class MetroPaintEventArgs : EventArgs
	{
		[CompilerGenerated]
		private Color color_0;

		[CompilerGenerated]
		private Color color_1;

		[CompilerGenerated]
		private Graphics graphics_0;

		private static MetroPaintEventArgs j3amhISmRnmuCllbakU;

		public Color BackColor
		{
			[CompilerGenerated]
			get
			{
				return color_0;
			}
			[CompilerGenerated]
			private set
			{
				color_0 = value;
			}
		}

		public Color ForeColor
		{
			[CompilerGenerated]
			get
			{
				return color_1;
			}
			[CompilerGenerated]
			private set
			{
				color_1 = value;
			}
		}

		public Graphics Graphics
		{
			[CompilerGenerated]
			get
			{
				return graphics_0;
			}
			[CompilerGenerated]
			private set
			{
				graphics_0 = value;
			}
		}

		public MetroPaintEventArgs(Color backColor, Color foreColor, Graphics g)
		{
			BackColor = backColor;
			ForeColor = foreColor;
			Graphics = g;
		}

		internal static bool nCG0AESgt0cdNKLkx9J()
		{
			return j3amhISmRnmuCllbakU == null;
		}

		internal static void LLuhEbSLhrlWdJB4bI9()
		{
		}
	}
}
