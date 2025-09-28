using System.Collections;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using MetroFramework.Controls;

namespace MetroFramework.Design.Controls
{
	internal class MetroTileDesigner : ParentControlDesigner
	{
		internal static MetroTileDesigner iSIgtbNl1PjRPNrcO2g;

		public override SelectionRules SelectionRules => base.SelectionRules;

		public override bool CanParent(Control control)
		{
			if (!(control is MetroLabel))
			{
				return control is MetroProgressSpinner;
			}
			return true;
		}

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
			properties.Remove("RightToLeft");
			base.PreFilterProperties(properties);
		}

		internal static bool YNu3N1NpR8vwycaMXrx()
		{
			return iSIgtbNl1PjRPNrcO2g == null;
		}

		internal static void pHQLlaNDvumY33VwGJA()
		{
		}
	}
}
