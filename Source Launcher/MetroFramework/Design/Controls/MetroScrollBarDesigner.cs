using System.Collections;
using System.ComponentModel;
using System.Windows.Forms.Design;
using MetroFramework.Controls;

namespace MetroFramework.Design.Controls
{
	[Designer(typeof(ScrollableControlDesigner), typeof(ParentControlDesigner))]
	internal class MetroScrollBarDesigner : ControlDesigner
	{
		private static MetroScrollBarDesigner MhliloN7e9gh7No1T2N;

		public override SelectionRules SelectionRules
		{
			get
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(base.Component)["Orientation"];
				if (propertyDescriptor == null)
				{
					return base.SelectionRules;
				}
				MetroScrollOrientation metroScrollOrientation = (MetroScrollOrientation)propertyDescriptor.GetValue(base.Component);
				if (metroScrollOrientation == MetroScrollOrientation.Vertical)
				{
					return SelectionRules.Moveable | SelectionRules.Visible | SelectionRules.TopSizeable | SelectionRules.BottomSizeable;
				}
				return SelectionRules.Moveable | SelectionRules.Visible | SelectionRules.LeftSizeable | SelectionRules.RightSizeable;
			}
		}

		protected override void PreFilterProperties(IDictionary properties)
		{
			properties.Remove("Text");
			properties.Remove("BackgroundImage");
			properties.Remove("ForeColor");
			properties.Remove("ImeMode");
			properties.Remove("Padding");
			properties.Remove("BackgroundImageLayout");
			properties.Remove("BackColor");
			properties.Remove("Font");
			properties.Remove("RightToLeft");
			base.PreFilterProperties(properties);
		}

		internal static bool mXgl7yNbYMIvaxiiYUV()
		{
			return MhliloN7e9gh7No1T2N == null;
		}
	}
}
