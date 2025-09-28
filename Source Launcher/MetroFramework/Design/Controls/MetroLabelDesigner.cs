using System.Collections;
using System.Windows.Forms.Design;

namespace MetroFramework.Design.Controls
{
	internal class MetroLabelDesigner : ControlDesigner
	{
		internal static MetroLabelDesigner gF655LsvVPyt50kNFCR;

		public override SelectionRules SelectionRules => base.SelectionRules;

		protected override void PreFilterProperties(IDictionary properties)
		{
			properties.Remove("ImeMode");
			properties.Remove("Padding");
			properties.Remove("FlatAppearance");
			properties.Remove("FlatStyle");
			properties.Remove("AutoEllipsis");
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

		internal static bool ChEEWDs9siQ7cIuTYM5()
		{
			return gF655LsvVPyt50kNFCR == null;
		}

		internal static void Y1U0y6swWfhvxkLwgdB()
		{
		}
	}
}
