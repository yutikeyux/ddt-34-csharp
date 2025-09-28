using System.Collections;
using System.Windows.Forms.Design;

namespace MetroFramework.Design.Controls
{
	internal class MetroProgressSpinnerDesigner : ControlDesigner
	{
		private static MetroProgressSpinnerDesigner Nw2LCrszLkhlSlItgNd;

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
			properties.Remove("BackgroundImage");
			properties.Remove("BackgroundImageLayout");
			properties.Remove("UseVisualStyleBackColor");
			properties.Remove("Font");
			properties.Remove("ForeColor");
			properties.Remove("RightToLeft");
			properties.Remove("Text");
			base.PreFilterProperties(properties);
		}

		internal static bool Jia4l1NPcxsQQSlSNif()
		{
			return Nw2LCrszLkhlSlItgNd == null;
		}

		internal static void zu8CCdNxJvuMpvCl7TT()
		{
		}
	}
}
