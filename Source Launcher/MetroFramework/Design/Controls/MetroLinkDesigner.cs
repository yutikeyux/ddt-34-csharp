using System.Collections;
using System.Windows.Forms.Design;

namespace MetroFramework.Design.Controls
{
	internal class MetroLinkDesigner : ControlDesigner
	{
		internal static MetroLinkDesigner qfvm21s0DR7m8PWxWiN;

		public override SelectionRules SelectionRules => base.SelectionRules;

		protected override void PreFilterProperties(IDictionary properties)
		{
			properties.Remove("ImeMode");
			properties.Remove("Padding");
			properties.Remove("FlatAppearance");
			properties.Remove("FlatStyle");
			properties.Remove("AutoEllipsis");
			properties.Remove("UseCompatibleTextRendering");
			properties.Remove("ImageIndex");
			properties.Remove("ImageKey");
			properties.Remove("ImageList");
			properties.Remove("TextImageRelation");
			properties.Remove("UseVisualStyleBackColor");
			properties.Remove("Font");
			properties.Remove("RightToLeft");
			base.PreFilterProperties(properties);
		}

		internal static bool FMYAfmsu0pZV9lIDa3g()
		{
			return qfvm21s0DR7m8PWxWiN == null;
		}

		internal static void FOnqPGsE1Bn4VbZrGDO()
		{
		}
	}
}
