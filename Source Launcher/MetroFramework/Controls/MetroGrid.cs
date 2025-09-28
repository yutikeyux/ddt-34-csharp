using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using MetroFramework.Components;
using MetroFramework.Drawing;
using MetroFramework.Interfaces;

namespace MetroFramework.Controls
{
	public class MetroGrid : DataGridView, IMetroControl
	{
		private EventHandler<MetroPaintEventArgs> eventHandler_0;

		private EventHandler<MetroPaintEventArgs> eventHandler_1;

		private EventHandler<MetroPaintEventArgs> eventHandler_2;

		private MetroColorStyle metroColorStyle_0;

		private MetroThemeStyle metroThemeStyle_0;

		private MetroStyleManager metroStyleManager_0;

		private bool bool_0;

		private bool bool_1;

		private bool bool_2;

		private float float_0 = 0.2f;

		private MetroDataGridHelper metroDataGridHelper_0;

		private MetroDataGridHelper metroDataGridHelper_1;

		private IContainer icontainer_0;

		private MetroScrollBar _horizontal;

		private MetroScrollBar _vertical;

		internal static MetroGrid i1eLK2eh8euYowitoOJ;

		[Category("Metro Appearance")]
		[DefaultValue(MetroColorStyle.Default)]
		public MetroColorStyle Style
		{
			get
			{
				if (!base.DesignMode && metroColorStyle_0 == MetroColorStyle.Default)
				{
					if (StyleManager != null && metroColorStyle_0 == MetroColorStyle.Default)
					{
						return StyleManager.Style;
					}
					if (StyleManager == null && metroColorStyle_0 == MetroColorStyle.Default)
					{
						return MetroColorStyle.Blue;
					}
					return metroColorStyle_0;
				}
				return metroColorStyle_0;
			}
			set
			{
				metroColorStyle_0 = value;
				method_0();
			}
		}

		[DefaultValue(MetroThemeStyle.Default)]
		[Category("Metro Appearance")]
		public MetroThemeStyle Theme
		{
			get
			{
				if (base.DesignMode || metroThemeStyle_0 != 0)
				{
					return metroThemeStyle_0;
				}
				if (StyleManager != null && metroThemeStyle_0 == MetroThemeStyle.Default)
				{
					return StyleManager.Theme;
				}
				if (StyleManager == null && metroThemeStyle_0 == MetroThemeStyle.Default)
				{
					return MetroThemeStyle.Light;
				}
				return metroThemeStyle_0;
			}
			set
			{
				metroThemeStyle_0 = value;
				method_0();
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public MetroStyleManager StyleManager
		{
			get
			{
				return metroStyleManager_0;
			}
			set
			{
				metroStyleManager_0 = value;
				method_0();
			}
		}

		[Category("Metro Appearance")]
		[DefaultValue(false)]
		public bool UseCustomBackColor
		{
			get
			{
				return bool_0;
			}
			set
			{
				bool_0 = value;
			}
		}

		[DefaultValue(false)]
		[Category("Metro Appearance")]
		public bool UseCustomForeColor
		{
			get
			{
				return bool_1;
			}
			set
			{
				bool_1 = value;
			}
		}

		[Category("Metro Appearance")]
		[DefaultValue(false)]
		public bool UseStyleColors
		{
			get
			{
				return bool_2;
			}
			set
			{
				bool_2 = value;
			}
		}

		[DefaultValue(true)]
		[Category("Metro Behaviour")]
		[Browsable(false)]
		public bool UseSelectable
		{
			get
			{
				return GetStyle(ControlStyles.Selectable);
			}
			set
			{
				SetStyle(ControlStyles.Selectable, value);
			}
		}

		[DefaultValue(0.2f)]
		public float HighLightPercentage
		{
			get
			{
				return float_0;
			}
			set
			{
				float_0 = value;
			}
		}

		[Category("Metro Appearance")]
		public event EventHandler<MetroPaintEventArgs> CustomPaintBackground
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				eventHandler_0 = (EventHandler<MetroPaintEventArgs>)Delegate.Combine(eventHandler_0, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				eventHandler_0 = (EventHandler<MetroPaintEventArgs>)Delegate.Remove(eventHandler_0, value);
			}
		}

		[Category("Metro Appearance")]
		public event EventHandler<MetroPaintEventArgs> CustomPaint
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				eventHandler_1 = (EventHandler<MetroPaintEventArgs>)Delegate.Combine(eventHandler_1, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				eventHandler_1 = (EventHandler<MetroPaintEventArgs>)Delegate.Remove(eventHandler_1, value);
			}
		}

		[Category("Metro Appearance")]
		public event EventHandler<MetroPaintEventArgs> CustomPaintForeground
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			add
			{
				eventHandler_2 = (EventHandler<MetroPaintEventArgs>)Delegate.Combine(eventHandler_2, value);
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			remove
			{
				eventHandler_2 = (EventHandler<MetroPaintEventArgs>)Delegate.Remove(eventHandler_2, value);
			}
		}

		protected virtual void OnCustomPaintBackground(MetroPaintEventArgs e)
		{
			if (GetStyle(ControlStyles.UserPaint) && eventHandler_0 != null)
			{
				eventHandler_0(this, e);
			}
		}

		protected virtual void OnCustomPaint(MetroPaintEventArgs e)
		{
			if (GetStyle(ControlStyles.UserPaint) && eventHandler_1 != null)
			{
				eventHandler_1(this, e);
			}
		}

		protected virtual void OnCustomPaintForeground(MetroPaintEventArgs e)
		{
			if (GetStyle(ControlStyles.UserPaint) && eventHandler_2 != null)
			{
				eventHandler_2(this, e);
			}
		}

		public MetroGrid()
		{
			method_1();
			method_0();
			base.Controls.Add(_vertical);
			base.Controls.Add(_horizontal);
			base.Controls.SetChildIndex(_vertical, 0);
			base.Controls.SetChildIndex(_horizontal, 1);
			_horizontal.Visible = false;
			_vertical.Visible = false;
			metroDataGridHelper_0 = new MetroDataGridHelper(_vertical, this);
			metroDataGridHelper_1 = new MetroDataGridHelper(_horizontal, this, vertical: false);
			DoubleBuffered = true;
		}

		protected override void OnColumnStateChanged(DataGridViewColumnStateChangedEventArgs e)
		{
			base.OnColumnStateChanged(e);
			if (e.StateChanged == DataGridViewElementStates.Visible)
			{
				metroDataGridHelper_0.UpdateScrollbar();
				metroDataGridHelper_1.UpdateScrollbar();
			}
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			if (base.RowCount > 1)
			{
				if (e.Delta > 0 && base.FirstDisplayedScrollingRowIndex > 0)
				{
					base.FirstDisplayedScrollingRowIndex--;
				}
				else if (e.Delta < 0)
				{
					base.FirstDisplayedScrollingRowIndex++;
				}
			}
		}

		private void method_0()
		{
			base.BorderStyle = System.Windows.Forms.BorderStyle.None;
			base.CellBorderStyle = DataGridViewCellBorderStyle.None;
			base.EnableHeadersVisualStyles = false;
			base.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			BackColor = MetroPaint.BackColor.Form(Theme);
			base.BackgroundColor = MetroPaint.BackColor.Form(Theme);
			base.GridColor = MetroPaint.BackColor.Form(Theme);
			ForeColor = MetroPaint.ForeColor.Button.Disabled(Theme);
			Font = new Font("Segoe UI", 11f, FontStyle.Regular, GraphicsUnit.Pixel);
			base.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			base.AllowUserToResizeRows = false;
			base.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
			base.ColumnHeadersDefaultCellStyle.BackColor = MetroPaint.GetStyleColor(Style);
			base.ColumnHeadersDefaultCellStyle.ForeColor = MetroPaint.ForeColor.Button.Press(Theme);
			base.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
			base.RowHeadersDefaultCellStyle.BackColor = MetroPaint.GetStyleColor(Style);
			base.RowHeadersDefaultCellStyle.ForeColor = MetroPaint.ForeColor.Button.Press(Theme);
			base.DefaultCellStyle.BackColor = MetroPaint.BackColor.Form(Theme);
			base.DefaultCellStyle.SelectionBackColor = ControlPaint.Light(MetroPaint.GetStyleColor(Style), float_0);
			base.DefaultCellStyle.SelectionForeColor = Color.FromArgb(17, 17, 17);
			base.DefaultCellStyle.SelectionBackColor = ControlPaint.Light(MetroPaint.GetStyleColor(Style), float_0);
			base.DefaultCellStyle.SelectionForeColor = Color.FromArgb(17, 17, 17);
			base.RowHeadersDefaultCellStyle.SelectionBackColor = ControlPaint.Light(MetroPaint.GetStyleColor(Style), float_0);
			base.RowHeadersDefaultCellStyle.SelectionForeColor = Color.FromArgb(17, 17, 17);
			base.ColumnHeadersDefaultCellStyle.SelectionBackColor = ControlPaint.Light(MetroPaint.GetStyleColor(Style), float_0);
			base.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.FromArgb(17, 17, 17);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && icontainer_0 != null)
			{
				icontainer_0.Dispose();
			}
			base.Dispose(disposing);
		}

		private void method_1()
		{
			_horizontal = new MetroScrollBar();
			_vertical = new MetroScrollBar();
			((ISupportInitialize)this).BeginInit();
			SuspendLayout();
			_horizontal.LargeChange = 10;
			_horizontal.Location = new Point(0, 0);
			_horizontal.Maximum = 100;
			_horizontal.Minimum = 0;
			_horizontal.MouseWheelBarPartitions = 10;
			_horizontal.Name = "_horizontal";
			_horizontal.Orientation = MetroScrollOrientation.Horizontal;
			_horizontal.ScrollbarSize = 50;
			_horizontal.Size = new Size(200, 50);
			_horizontal.TabIndex = 0;
			_horizontal.UseSelectable = true;
			_vertical.LargeChange = 10;
			_vertical.Location = new Point(0, 0);
			_vertical.Maximum = 100;
			_vertical.Minimum = 0;
			_vertical.MouseWheelBarPartitions = 10;
			_vertical.Name = "_vertical";
			_vertical.Orientation = MetroScrollOrientation.Vertical;
			_vertical.ScrollbarSize = 50;
			_vertical.Size = new Size(50, 200);
			_vertical.TabIndex = 0;
			_vertical.UseSelectable = true;
			((ISupportInitialize)this).EndInit();
			ResumeLayout(performLayout: false);
		}

		internal static bool Yv32h4eTXiaWNeDrbVq()
		{
			return i1eLK2eh8euYowitoOJ == null;
		}

		internal static void EwdT9ke9IRUbjFq2HIU()
		{
		}
	}
}
