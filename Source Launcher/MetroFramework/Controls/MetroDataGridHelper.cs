using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MetroFramework.Controls
{
	public class MetroDataGridHelper
	{
		private MetroScrollBar metroScrollBar_0;

		private DataGridView dataGridView_0;

		private int int_0;

		private bool bool_0;

		private HScrollBar hscrollBar_0;

		private VScrollBar vscrollBar_0;

		internal static MetroDataGridHelper jLPHXZeyij7ukU5IwKH;

		public MetroDataGridHelper(MetroScrollBar scrollbar, DataGridView grid)
		{
			new MetroDataGridHelper(scrollbar, grid, vertical: true);
		}

		public MetroDataGridHelper(MetroScrollBar scrollbar, DataGridView grid, bool vertical)
		{
			metroScrollBar_0 = scrollbar;
			metroScrollBar_0.UseBarColor = true;
			dataGridView_0 = grid;
			bool_0 = !vertical;
			foreach (object control in dataGridView_0.Controls)
			{
				if (((object)control).GetType() == Type.GetTypeFromHandle((RuntimeTypeHandle)typeof(VScrollBar).TypeHandle))
				{
					vscrollBar_0 = (VScrollBar)control;
				}
				if (((object)control).GetType() == Type.GetTypeFromHandle((RuntimeTypeHandle)typeof(HScrollBar).TypeHandle))
				{
					hscrollBar_0 = (HScrollBar)control;
				}
			}
			dataGridView_0.RowsAdded += dataGridView_0_RowsAdded;
			dataGridView_0.UserDeletedRow += dataGridView_0_UserDeletedRow;
			dataGridView_0.Scroll += dataGridView_0_Scroll;
			dataGridView_0.Resize += dataGridView_0_Resize;
			metroScrollBar_0.Scroll += metroScrollBar_0_Scroll;
			metroScrollBar_0.ScrollbarSize = 17;
			UpdateScrollbar();
		}

		private void dataGridView_0_Scroll(object sender, ScrollEventArgs e)
		{
			UpdateScrollbar();
		}

		private void dataGridView_0_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
		{
			UpdateScrollbar();
		}

		private void dataGridView_0_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
		{
			UpdateScrollbar();
		}

		private void metroScrollBar_0_Scroll(object sender, ScrollEventArgs e)
		{
			if (int_0 > 0)
			{
				return;
			}
			if (bool_0)
			{
				try
				{
					hscrollBar_0.Value = metroScrollBar_0.Value;
					dataGridView_0.HorizontalScrollingOffset = metroScrollBar_0.Value;
				}
				catch
				{
				}
			}
			else if (metroScrollBar_0.Value >= 0 && metroScrollBar_0.Value < dataGridView_0.Rows.Count)
			{
				dataGridView_0.FirstDisplayedScrollingRowIndex = ((metroScrollBar_0.Value + ((metroScrollBar_0.Value != 1) ? 1 : (-1)) >= dataGridView_0.Rows.Count) ? (dataGridView_0.Rows.Count - 1) : (metroScrollBar_0.Value + ((metroScrollBar_0.Value != 1) ? 1 : (-1))));
			}
			else
			{
				dataGridView_0.FirstDisplayedScrollingRowIndex = metroScrollBar_0.Value - 1;
			}
			dataGridView_0.Invalidate();
		}

		private void method_0()
		{
			int_0++;
		}

		private void method_1()
		{
			if (int_0 > 0)
			{
				int_0--;
			}
		}

		public void UpdateScrollbar()
		{
			if (dataGridView_0 == null)
			{
				return;
			}
			try
			{
				method_0();
				if (bool_0)
				{
					method_3();
					metroScrollBar_0.Maximum = hscrollBar_0.Maximum;
					metroScrollBar_0.Minimum = hscrollBar_0.Minimum;
					metroScrollBar_0.SmallChange = hscrollBar_0.SmallChange;
					metroScrollBar_0.LargeChange = hscrollBar_0.LargeChange;
					metroScrollBar_0.Location = new Point(0, dataGridView_0.Height - metroScrollBar_0.ScrollbarSize);
					metroScrollBar_0.Width = dataGridView_0.Width - (vscrollBar_0.Visible ? metroScrollBar_0.ScrollbarSize : 0);
					metroScrollBar_0.BringToFront();
					metroScrollBar_0.Visible = hscrollBar_0.Visible;
					metroScrollBar_0.Value = ((hscrollBar_0.Value == 0) ? 1 : hscrollBar_0.Value);
					return;
				}
				int num = method_2();
				metroScrollBar_0.Maximum = dataGridView_0.RowCount;
				metroScrollBar_0.Minimum = 1;
				metroScrollBar_0.SmallChange = 1;
				metroScrollBar_0.LargeChange = Math.Max(1, num - 1);
				metroScrollBar_0.Value = dataGridView_0.FirstDisplayedScrollingRowIndex;
				if (dataGridView_0.RowCount > 0 && dataGridView_0.Rows[dataGridView_0.RowCount - 1].Cells[0].Displayed)
				{
					metroScrollBar_0.Value = dataGridView_0.RowCount;
				}
				metroScrollBar_0.Location = new Point(dataGridView_0.Width - metroScrollBar_0.ScrollbarSize, 0);
				metroScrollBar_0.Height = dataGridView_0.Height - (hscrollBar_0.Visible ? metroScrollBar_0.ScrollbarSize : 0);
				metroScrollBar_0.BringToFront();
				metroScrollBar_0.Visible = vscrollBar_0.Visible;
			}
			finally
			{
				method_1();
			}
		}

		private int method_2()
		{
			return dataGridView_0.DisplayedRowCount(includePartialRow: true);
		}

		private int method_3()
		{
			return dataGridView_0.DisplayedColumnCount(includePartialColumns: true);
		}

		public bool VisibleVerticalScroll()
		{
			bool result = false;
			if (dataGridView_0.DisplayedRowCount(includePartialRow: true) < dataGridView_0.RowCount + (dataGridView_0.RowHeadersVisible ? 1 : 0))
			{
				result = true;
			}
			return result;
		}

		public bool VisibleHorizontalScroll()
		{
			bool result = false;
			if (dataGridView_0.DisplayedColumnCount(includePartialColumns: true) < dataGridView_0.ColumnCount + (dataGridView_0.ColumnHeadersVisible ? 1 : 0))
			{
				result = true;
			}
			return result;
		}

		private void dataGridView_0_Resize(object sender, EventArgs e)
		{
			UpdateScrollbar();
		}

		private void method_4(object sender, ListChangedEventArgs e)
		{
			UpdateScrollbar();
		}

		internal static void BpjKAJec1I3nrh0Tetx()
		{
		}

		internal static bool amtKj3eRjshRDmwmmTR()
		{
			return jLPHXZeyij7ukU5IwKH == null;
		}
	}
}
