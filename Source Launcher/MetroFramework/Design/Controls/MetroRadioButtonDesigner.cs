using System.Collections;
using System.Windows.Forms.Design;

namespace MetroFramework.Design.Controls
{
	internal class MetroRadioButtonDesigner : ControlDesigner
	{
		internal static MetroRadioButtonDesigner U3fNQ0sV84qMGBBQVnW;

		protected override void PreFilterProperties(IDictionary properties)
		{
			properties.Remove("ImeMode");
			properties.Remove("Padding");
			properties.Remove("FlatAppearance");
			properties.Remove("FlatStyle");
			properties.Remove("UseCompatibleTextRendering");
			properties.Remove("Image");
			properties.Remove("ImageAlign");
			properties.Remove("ImageIndex");
			properties.Remove("ImageKey");
			properties.Remove("ImageList");
			properties.Remove("TextImageRelation");
			properties.Remove("UseVisualStyleBackColor");
			properties.Remove("Font");
			properties.Remove("RightToLeft");
			base.PreFilterProperties(properties);
		}

		internal static bool scd3CEsnksxiEwCMghA()
		{
			return U3fNQ0sV84qMGBBQVnW == null;
		}

		internal static void zDWdhgs45NvE9fAbBSD()
		{
		}
	}
}
