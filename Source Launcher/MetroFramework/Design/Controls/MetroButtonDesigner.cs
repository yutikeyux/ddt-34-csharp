using System.Collections;
using System.Windows.Forms.Design;

namespace MetroFramework.Design.Controls
{
	internal class MetroButtonDesigner : ControlDesigner
	{
		private static MetroButtonDesigner tUrBufsDSw8HkKaaGWl;

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

		internal static bool XLCMysshQU3ktsDjaZC()
		{
			return tUrBufsDSw8HkKaaGWl == null;
		}

		internal static void ON1bhysKr80kpUpjOe1()
		{
		}
	}
}
