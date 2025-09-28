using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

namespace MetroFramework.Controls
{
	[ToolboxItem(false)]
	[Editor("MetroFramework.Design.MetroTabPageCollectionEditor, MetroFramework.Design, Version=1.4.0.0, Culture=neutral, PublicKeyToken=5f91a84759bf584a", typeof(UITypeEditor))]
	public class MetroTabPageCollection : TabControl.TabPageCollection
	{
		private static MetroTabPageCollection iI3UXgnqZGPxa0ftVZ3;

		public MetroTabPageCollection(MetroTabControl owner)
			: base(owner)
		{
		}

		internal static void fZNi6KnNaYXB76c4HGD()
		{
		}

		internal static bool swCD4jnLDlO7V3ix2pq()
		{
			return iI3UXgnqZGPxa0ftVZ3 == null;
		}
	}
}
