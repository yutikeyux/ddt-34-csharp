using System.Drawing;

namespace MaterialSkin
{
	public class ColorScheme
	{
		public readonly Color PrimaryColor;

		public readonly Color DarkPrimaryColor;

		public readonly Color LightPrimaryColor;

		public readonly Color AccentColor;

		public readonly Color TextColor;

		public readonly Pen PrimaryPen;

		public readonly Pen DarkPrimaryPen;

		public readonly Pen LightPrimaryPen;

		public readonly Pen AccentPen;

		public readonly Pen TextPen;

		public readonly Brush PrimaryBrush;

		public readonly Brush DarkPrimaryBrush;

		public readonly Brush LightPrimaryBrush;

		public readonly Brush AccentBrush;

		public readonly Brush TextBrush;

		private static ColorScheme O0dIOHIMDGbssanvdlH3;

		public ColorScheme(Primary primary, Primary darkPrimary, Primary lightPrimary, Accent accent, TextShade textShade)
		{
			PrimaryColor = ((int)primary).ToColor();
			DarkPrimaryColor = ((int)darkPrimary).ToColor();
			LightPrimaryColor = ((int)lightPrimary).ToColor();
			AccentColor = ((int)accent).ToColor();
			TextColor = ((int)textShade).ToColor();
			PrimaryPen = new Pen(PrimaryColor);
			DarkPrimaryPen = new Pen(DarkPrimaryColor);
			LightPrimaryPen = new Pen(LightPrimaryColor);
			AccentPen = new Pen(AccentColor);
			TextPen = new Pen(TextColor);
			PrimaryBrush = new SolidBrush(PrimaryColor);
			DarkPrimaryBrush = new SolidBrush(DarkPrimaryColor);
			LightPrimaryBrush = new SolidBrush(LightPrimaryColor);
			AccentBrush = new SolidBrush(AccentColor);
			TextBrush = new SolidBrush(TextColor);
		}

		internal static bool USdpifIMhgSoS25aqOVJ()
		{
			return O0dIOHIMDGbssanvdlH3 == null;
		}
	}
}
