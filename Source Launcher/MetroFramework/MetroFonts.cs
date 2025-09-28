using System;
using System.Drawing;

namespace MetroFramework
{
	public static class MetroFonts
	{
		internal interface Interface0
		{
			Font ResolveFont(string familyName, float emSize, FontStyle fontStyle, GraphicsUnit unit);
		}

		private class Class21 : Interface0
		{
			internal static object TMxov98UF5uCjq55j90;

			public Font ResolveFont(string familyName, float emSize, FontStyle fontStyle, GraphicsUnit unit)
			{
				return new Font(familyName, emSize, fontStyle, unit);
			}

			internal static bool wmUoBA8Gks1t0uQYdy9()
			{
				return TMxov98UF5uCjq55j90 == null;
			}

			internal static void MwX7aW8g0ESaAvLfvpY()
			{
			}
		}

		private static Interface0 interface0_0;

		private static object Jw3bGgJjkyFok2a7fZa;

		public static Font Title => DefaultLight(24f);

		public static Font Subtitle => Default(14f);

		public static Font TileCount => Default(44f);

		static MetroFonts()
		{
			try
			{
				Type type = Type.GetType("MetroFramework.Fonts.FontResolver, MetroFramework.Fonts, Version=1.4.0.0, Culture=neutral, PublicKeyToken=5f91a84759bf584a");
				if (type != null)
				{
					interface0_0 = (Interface0)Activator.CreateInstance(type);
					if (interface0_0 != null)
					{
						return;
					}
				}
			}
			catch (Exception)
			{
			}
			interface0_0 = new Class21();
		}

		public static Font DefaultLight(float size)
		{
			return interface0_0.ResolveFont("Segoe UI Light", size, FontStyle.Regular, GraphicsUnit.Pixel);
		}

		public static Font Default(float size)
		{
			return interface0_0.ResolveFont("Segoe UI", size, FontStyle.Regular, GraphicsUnit.Pixel);
		}

		public static Font DefaultBold(float size)
		{
			return interface0_0.ResolveFont("Segoe UI", size, FontStyle.Bold, GraphicsUnit.Pixel);
		}

		public static Font DefaultItalic(float size)
		{
			return interface0_0.ResolveFont("Segoe UI", size, FontStyle.Italic, GraphicsUnit.Pixel);
		}

		public static Font Tile(MetroTileTextSize labelSize, MetroTileTextWeight labelWeight)
		{
			switch (labelSize)
			{
			case MetroTileTextSize.Medium:
				switch (labelWeight)
				{
				case MetroTileTextWeight.Regular:
					return Default(14f);
				case MetroTileTextWeight.Bold:
					return DefaultBold(14f);
				case MetroTileTextWeight.Light:
					return DefaultLight(14f);
				}
				break;
			case MetroTileTextSize.Tall:
				switch (labelWeight)
				{
				case MetroTileTextWeight.Light:
					return DefaultLight(18f);
				case MetroTileTextWeight.Regular:
					return Default(18f);
				case MetroTileTextWeight.Bold:
					return DefaultBold(18f);
				}
				break;
			case MetroTileTextSize.Small:
				switch (labelWeight)
				{
				case MetroTileTextWeight.Light:
					return DefaultLight(12f);
				case MetroTileTextWeight.Regular:
					return Default(12f);
				case MetroTileTextWeight.Bold:
					return DefaultBold(12f);
				}
				break;
			}
			return DefaultLight(14f);
		}

		public static Font Link(MetroLinkSize linkSize, MetroLinkWeight linkWeight)
		{
			switch (linkSize)
			{
			case MetroLinkSize.Small:
				switch (linkWeight)
				{
				case MetroLinkWeight.Regular:
					return Default(12f);
				case MetroLinkWeight.Bold:
					return DefaultBold(12f);
				case MetroLinkWeight.Light:
					return DefaultLight(12f);
				}
				break;
			case MetroLinkSize.Medium:
				switch (linkWeight)
				{
				case MetroLinkWeight.Regular:
					return Default(14f);
				case MetroLinkWeight.Bold:
					return DefaultBold(14f);
				case MetroLinkWeight.Light:
					return DefaultLight(14f);
				}
				break;
			case MetroLinkSize.Tall:
				switch (linkWeight)
				{
				case MetroLinkWeight.Light:
					return DefaultLight(18f);
				case MetroLinkWeight.Regular:
					return Default(18f);
				case MetroLinkWeight.Bold:
					return DefaultBold(18f);
				}
				break;
			}
			return Default(12f);
		}

		public static Font ComboBox(MetroComboBoxSize linkSize, MetroComboBoxWeight linkWeight)
		{
			switch (linkSize)
			{
			case MetroComboBoxSize.Small:
				switch (linkWeight)
				{
				case MetroComboBoxWeight.Light:
					return DefaultLight(12f);
				case MetroComboBoxWeight.Regular:
					return Default(12f);
				case MetroComboBoxWeight.Bold:
					return DefaultBold(12f);
				}
				break;
			case MetroComboBoxSize.Medium:
				switch (linkWeight)
				{
				case MetroComboBoxWeight.Light:
					return DefaultLight(14f);
				case MetroComboBoxWeight.Regular:
					return Default(14f);
				case MetroComboBoxWeight.Bold:
					return DefaultBold(14f);
				}
				break;
			case MetroComboBoxSize.Tall:
				switch (linkWeight)
				{
				case MetroComboBoxWeight.Light:
					return DefaultLight(18f);
				case MetroComboBoxWeight.Regular:
					return Default(18f);
				case MetroComboBoxWeight.Bold:
					return DefaultBold(18f);
				}
				break;
			}
			return Default(12f);
		}

		public static Font DateTime(MetroDateTimeSize linkSize, MetroDateTimeWeight linkWeight)
		{
			switch (linkSize)
			{
			case MetroDateTimeSize.Medium:
				switch (linkWeight)
				{
				case MetroDateTimeWeight.Light:
					return DefaultLight(14f);
				case MetroDateTimeWeight.Regular:
					return Default(14f);
				case MetroDateTimeWeight.Bold:
					return DefaultBold(14f);
				}
				break;
			case MetroDateTimeSize.Tall:
				switch (linkWeight)
				{
				case MetroDateTimeWeight.Light:
					return DefaultLight(18f);
				case MetroDateTimeWeight.Regular:
					return Default(18f);
				case MetroDateTimeWeight.Bold:
					return DefaultBold(18f);
				}
				break;
			case MetroDateTimeSize.Small:
				switch (linkWeight)
				{
				case MetroDateTimeWeight.Regular:
					return Default(12f);
				case MetroDateTimeWeight.Bold:
					return DefaultBold(12f);
				case MetroDateTimeWeight.Light:
					return DefaultLight(12f);
				}
				break;
			}
			return Default(12f);
		}

		public static Font Label(MetroLabelSize labelSize, MetroLabelWeight labelWeight)
		{
			switch (labelSize)
			{
			case MetroLabelSize.Small:
				switch (labelWeight)
				{
				case MetroLabelWeight.Light:
					return DefaultLight(12f);
				case MetroLabelWeight.Regular:
					return Default(12f);
				case MetroLabelWeight.Bold:
					return DefaultBold(12f);
				}
				break;
			case MetroLabelSize.Medium:
				switch (labelWeight)
				{
				case MetroLabelWeight.Regular:
					return Default(14f);
				case MetroLabelWeight.Bold:
					return DefaultBold(14f);
				case MetroLabelWeight.Light:
					return DefaultLight(14f);
				}
				break;
			case MetroLabelSize.Tall:
				switch (labelWeight)
				{
				case MetroLabelWeight.Regular:
					return Default(18f);
				case MetroLabelWeight.Bold:
					return DefaultBold(18f);
				case MetroLabelWeight.Light:
					return DefaultLight(18f);
				}
				break;
			}
			return DefaultLight(14f);
		}

		public static Font TextBox(MetroTextBoxSize linkSize, MetroTextBoxWeight linkWeight)
		{
			switch (linkSize)
			{
			case MetroTextBoxSize.Small:
				switch (linkWeight)
				{
				case MetroTextBoxWeight.Regular:
					return Default(12f);
				case MetroTextBoxWeight.Bold:
					return DefaultBold(12f);
				case MetroTextBoxWeight.Light:
					return DefaultLight(12f);
				}
				break;
			case MetroTextBoxSize.Medium:
				switch (linkWeight)
				{
				case MetroTextBoxWeight.Regular:
					return Default(14f);
				case MetroTextBoxWeight.Bold:
					return DefaultBold(14f);
				case MetroTextBoxWeight.Light:
					return DefaultLight(14f);
				}
				break;
			case MetroTextBoxSize.Tall:
				switch (linkWeight)
				{
				case MetroTextBoxWeight.Regular:
					return Default(18f);
				case MetroTextBoxWeight.Bold:
					return DefaultBold(18f);
				case MetroTextBoxWeight.Light:
					return DefaultLight(18f);
				}
				break;
			}
			return Default(12f);
		}

		public static Font ProgressBar(MetroProgressBarSize labelSize, MetroProgressBarWeight labelWeight)
		{
			switch (labelSize)
			{
			case MetroProgressBarSize.Medium:
				switch (labelWeight)
				{
				case MetroProgressBarWeight.Light:
					return DefaultLight(14f);
				case MetroProgressBarWeight.Regular:
					return Default(14f);
				case MetroProgressBarWeight.Bold:
					return DefaultBold(14f);
				}
				break;
			case MetroProgressBarSize.Tall:
				switch (labelWeight)
				{
				case MetroProgressBarWeight.Light:
					return DefaultLight(18f);
				case MetroProgressBarWeight.Regular:
					return Default(18f);
				case MetroProgressBarWeight.Bold:
					return DefaultBold(18f);
				}
				break;
			case MetroProgressBarSize.Small:
				switch (labelWeight)
				{
				case MetroProgressBarWeight.Regular:
					return Default(12f);
				case MetroProgressBarWeight.Bold:
					return DefaultBold(12f);
				case MetroProgressBarWeight.Light:
					return DefaultLight(12f);
				}
				break;
			}
			return DefaultLight(14f);
		}

		public static Font TabControl(MetroTabControlSize labelSize, MetroTabControlWeight labelWeight)
		{
			switch (labelSize)
			{
			case MetroTabControlSize.Medium:
				switch (labelWeight)
				{
				case MetroTabControlWeight.Regular:
					return Default(14f);
				case MetroTabControlWeight.Bold:
					return DefaultBold(14f);
				case MetroTabControlWeight.Light:
					return DefaultLight(14f);
				}
				break;
			case MetroTabControlSize.Tall:
				switch (labelWeight)
				{
				case MetroTabControlWeight.Light:
					return DefaultLight(18f);
				case MetroTabControlWeight.Regular:
					return Default(18f);
				case MetroTabControlWeight.Bold:
					return DefaultBold(18f);
				}
				break;
			case MetroTabControlSize.Small:
				switch (labelWeight)
				{
				case MetroTabControlWeight.Light:
					return DefaultLight(12f);
				case MetroTabControlWeight.Regular:
					return Default(12f);
				case MetroTabControlWeight.Bold:
					return DefaultBold(12f);
				}
				break;
			}
			return DefaultLight(14f);
		}

		public static Font CheckBox(MetroCheckBoxSize linkSize, MetroCheckBoxWeight linkWeight)
		{
			switch (linkSize)
			{
			case MetroCheckBoxSize.Medium:
				switch (linkWeight)
				{
				case MetroCheckBoxWeight.Regular:
					return Default(14f);
				case MetroCheckBoxWeight.Bold:
					return DefaultBold(14f);
				case MetroCheckBoxWeight.Light:
					return DefaultLight(14f);
				}
				break;
			case MetroCheckBoxSize.Tall:
				switch (linkWeight)
				{
				case MetroCheckBoxWeight.Light:
					return DefaultLight(18f);
				case MetroCheckBoxWeight.Regular:
					return Default(18f);
				case MetroCheckBoxWeight.Bold:
					return DefaultBold(18f);
				}
				break;
			case MetroCheckBoxSize.Small:
				switch (linkWeight)
				{
				case MetroCheckBoxWeight.Light:
					return DefaultLight(12f);
				case MetroCheckBoxWeight.Regular:
					return Default(12f);
				case MetroCheckBoxWeight.Bold:
					return DefaultBold(12f);
				}
				break;
			}
			return Default(12f);
		}

		public static Font WaterMark(MetroLabelSize labelSize, MetroWaterMarkWeight labelWeight)
		{
			switch (labelSize)
			{
			case MetroLabelSize.Small:
				switch (labelWeight)
				{
				case MetroWaterMarkWeight.Light:
					return DefaultLight(12f);
				case MetroWaterMarkWeight.Regular:
					return Default(12f);
				case MetroWaterMarkWeight.Bold:
					return DefaultBold(12f);
				case MetroWaterMarkWeight.Italic:
					return DefaultItalic(12f);
				}
				break;
			case MetroLabelSize.Medium:
				switch (labelWeight)
				{
				case MetroWaterMarkWeight.Light:
					return DefaultLight(14f);
				case MetroWaterMarkWeight.Regular:
					return Default(14f);
				case MetroWaterMarkWeight.Bold:
					return DefaultBold(14f);
				case MetroWaterMarkWeight.Italic:
					return DefaultItalic(14f);
				}
				break;
			case MetroLabelSize.Tall:
				switch (labelWeight)
				{
				case MetroWaterMarkWeight.Light:
					return DefaultLight(18f);
				case MetroWaterMarkWeight.Regular:
					return Default(18f);
				case MetroWaterMarkWeight.Bold:
					return DefaultBold(18f);
				case MetroWaterMarkWeight.Italic:
					return DefaultItalic(18f);
				}
				break;
			}
			return DefaultLight(14f);
		}

		public static Font Button(MetroButtonSize linkSize, MetroButtonWeight linkWeight)
		{
			switch (linkSize)
			{
			case MetroButtonSize.Small:
				switch (linkWeight)
				{
				case MetroButtonWeight.Regular:
					return Default(11f);
				case MetroButtonWeight.Bold:
					return DefaultBold(11f);
				case MetroButtonWeight.Light:
					return DefaultLight(11f);
				}
				break;
			case MetroButtonSize.Medium:
				switch (linkWeight)
				{
				case MetroButtonWeight.Regular:
					return Default(13f);
				case MetroButtonWeight.Bold:
					return DefaultBold(13f);
				case MetroButtonWeight.Light:
					return DefaultLight(13f);
				}
				break;
			case MetroButtonSize.Tall:
				switch (linkWeight)
				{
				case MetroButtonWeight.Light:
					return DefaultLight(16f);
				case MetroButtonWeight.Regular:
					return Default(16f);
				case MetroButtonWeight.Bold:
					return DefaultBold(16f);
				}
				break;
			}
			return Default(11f);
		}

		internal static bool dum3R8JrryGpHXb24LS()
		{
			return Jw3bGgJjkyFok2a7fZa == null;
		}
	}
}
