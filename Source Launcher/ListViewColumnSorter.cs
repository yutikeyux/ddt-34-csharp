using System;
using System.Collections;
using System.Windows.Forms;

public class ListViewColumnSorter : IComparer
{
	public enum SortModifiers
	{
		SortByImage,
		SortByCheckbox,
		SortByText
	}

	public int ColumnToSort;

	public SortOrder OrderOfSort;

	private CaseInsensitiveComparer caseInsensitiveComparer_0;

	private SortModifiers sortModifiers_0 = SortModifiers.SortByText;

	internal static ListViewColumnSorter k9ddmRot8PNCifUyIvc;

	public SortModifiers _SortModifier
	{
		get
		{
			return sortModifiers_0;
		}
		set
		{
			sortModifiers_0 = value;
		}
	}

	public int SortColumn
	{
		get
		{
			return ColumnToSort;
		}
		set
		{
			ColumnToSort = value;
		}
	}

	public SortOrder Order
	{
		get
		{
			return OrderOfSort;
		}
		set
		{
			OrderOfSort = value;
		}
	}

	public ListViewColumnSorter()
	{
		ColumnToSort = 0;
		caseInsensitiveComparer_0 = new CaseInsensitiveComparer();
	}

	public int Compare(object x, object y)
	{
		int num = 0;
		ListViewItem listViewItem = (ListViewItem)x;
		ListViewItem listViewItem2 = (ListViewItem)y;
		num = ((!DateTime.TryParse(listViewItem.SubItems[ColumnToSort].Text, out var result) || !DateTime.TryParse(listViewItem2.SubItems[ColumnToSort].Text, out var result2)) ? caseInsensitiveComparer_0.Compare(listViewItem.SubItems[ColumnToSort].Text, listViewItem2.SubItems[ColumnToSort].Text) : caseInsensitiveComparer_0.Compare(result, result2));
		if (OrderOfSort == SortOrder.Ascending)
		{
			return num;
		}
		if (OrderOfSort == SortOrder.Descending)
		{
			return -num;
		}
		return 0;
	}

	internal static bool tUsc2AoDThphHZVY9vY()
	{
		return k9ddmRot8PNCifUyIvc == null;
	}

	internal static void immdrOoTiLSUhsJD6Kv()
	{
	}
}
