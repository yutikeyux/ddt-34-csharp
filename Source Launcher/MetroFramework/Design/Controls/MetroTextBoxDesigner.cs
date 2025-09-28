using System.Collections;
using System.ComponentModel;
using System.Windows.Forms.Design;

namespace MetroFramework.Design.Controls
{
	internal class MetroTextBoxDesigner : ControlDesigner
	{
		internal static MetroTextBoxDesigner McHNaPNCN8Kykhgd3jP;

		public override SelectionRules SelectionRules
		{
			get
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["Multiline"];
				if (propertyDescriptor == null)
				{
					return base.SelectionRules;
				}
				if (!(bool)propertyDescriptor.GetValue(base.Component))
				{
					return SelectionRules.Moveable | SelectionRules.Visible | SelectionRules.LeftSizeable | SelectionRules.RightSizeable;
				}
				return SelectionRules.AllSizeable | SelectionRules.Moveable | SelectionRules.Visible;
			}
		}

		protected override void PreFilterProperties(IDictionary properties)
		{
			properties.Remove("BackgroundImage");
			properties.Remove("ImeMode");
			properties.Remove("Padding");
			properties.Remove("BackgroundImageLayout");
			properties.Remove("Font");
			base.PreFilterProperties(properties);
		}

		internal static bool WpHeWbNYsl4uay6IS4J()
		{
			return McHNaPNCN8Kykhgd3jP == null;
		}

		internal static void On64KnNHnQsxK78Fluq()
		{
		}
	}
}
