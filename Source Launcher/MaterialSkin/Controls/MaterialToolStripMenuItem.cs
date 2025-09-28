using System.Drawing;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
	public class MaterialToolStripMenuItem : ToolStripMenuItem
	{
		private static MaterialToolStripMenuItem rAVEkZIo0TJM3g7ls6Pf;

		public MaterialToolStripMenuItem()
		{
			base.AutoSize = false;
			Size = new Size(120, 30);
		}

		protected override ToolStripDropDown CreateDefaultDropDown()
		{
			ToolStripDropDown toolStripDropDown = base.CreateDefaultDropDown();
			if (base.DesignMode)
			{
				return toolStripDropDown;
			}
			MaterialContextMenuStrip materialContextMenuStrip = new MaterialContextMenuStrip();
			materialContextMenuStrip.Items.AddRange(toolStripDropDown.Items);
			return materialContextMenuStrip;
		}

		internal static void buCEUkIo5gqhvE2wbkF9()
		{
		}

		internal static bool JEOjM8Iou5mmKIOvYftu()
		{
			return rAVEkZIo0TJM3g7ls6Pf == null;
		}
	}
}
