using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MetroFramework.Drawing;
using MetroFramework.Interfaces;

namespace MetroFramework.Components
{
	[ToolboxBitmap(typeof(ToolTip))]
	public class MetroToolTip : ToolTip, IMetroComponent
	{
		private MetroColorStyle metroColorStyle_0 = MetroColorStyle.Blue;

		private MetroThemeStyle metroThemeStyle_0 = MetroThemeStyle.Light;

		private MetroStyleManager lxBfGrwvud;

		internal static MetroToolTip niJ7j2XiYTlQPZVk6KA;

		[Category("Metro Appearance")]
		public MetroColorStyle Style
		{
			get
			{
				if (StyleManager != null)
				{
					return StyleManager.Style;
				}
				return metroColorStyle_0;
			}
			set
			{
				metroColorStyle_0 = value;
			}
		}

		[Category("Metro Appearance")]
		public MetroThemeStyle Theme
		{
			get
			{
				if (StyleManager != null)
				{
					return StyleManager.Theme;
				}
				return metroThemeStyle_0;
			}
			set
			{
				metroThemeStyle_0 = value;
			}
		}

		[Browsable(false)]
		public MetroStyleManager StyleManager
		{
			get
			{
				return lxBfGrwvud;
			}
			set
			{
				lxBfGrwvud = value;
			}
		}

		[Browsable(false)]
		[DefaultValue(true)]
		public new bool ShowAlways
		{
			get
			{
				return base.ShowAlways;
			}
			set
			{
				base.ShowAlways = true;
			}
		}

		[Browsable(false)]
		[DefaultValue(true)]
		public new bool OwnerDraw
		{
			get
			{
				return base.OwnerDraw;
			}
			set
			{
				base.OwnerDraw = true;
			}
		}

		[Browsable(false)]
		public new bool IsBalloon
		{
			get
			{
				return base.IsBalloon;
			}
			set
			{
				base.IsBalloon = false;
			}
		}

		[Browsable(false)]
		public new Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
			}
		}

		[Browsable(false)]
		public new Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
			}
		}

		[Browsable(false)]
		public new string ToolTipTitle
		{
			get
			{
				return base.ToolTipTitle;
			}
			set
			{
				base.ToolTipTitle = "";
			}
		}

		[Browsable(false)]
		public new ToolTipIcon ToolTipIcon
		{
			get
			{
				return base.ToolTipIcon;
			}
			set
			{
				base.ToolTipIcon = System.Windows.Forms.ToolTipIcon.None;
			}
		}

		public MetroToolTip()
		{
			OwnerDraw = true;
			ShowAlways = true;
			base.Draw += MetroToolTip_Draw;
			base.Popup += MetroToolTip_Popup;
		}

		public new void SetToolTip(Control control, string caption)
		{
			base.SetToolTip(control, caption);
			if (!(control is IMetroControl))
			{
				return;
			}
			foreach (Control control2 in control.Controls)
			{
				SetToolTip(control2, caption);
			}
		}

		private void MetroToolTip_Popup(object sender, PopupEventArgs e)
		{
			if (e.AssociatedWindow is IMetroForm)
			{
				Style = ((IMetroForm)e.AssociatedWindow).Style;
				Theme = ((IMetroForm)e.AssociatedWindow).Theme;
				StyleManager = ((IMetroForm)e.AssociatedWindow).StyleManager;
			}
			else if (e.AssociatedControl is IMetroControl)
			{
				Style = ((IMetroControl)e.AssociatedControl).Style;
				Theme = ((IMetroControl)e.AssociatedControl).Theme;
				StyleManager = ((IMetroControl)e.AssociatedControl).StyleManager;
			}
			e.ToolTipSize = new Size(e.ToolTipSize.Width + 24, e.ToolTipSize.Height + 9);
		}

		private void MetroToolTip_Draw(object sender, DrawToolTipEventArgs e)
		{
			MetroThemeStyle theme = ((Theme != MetroThemeStyle.Light) ? MetroThemeStyle.Light : MetroThemeStyle.Dark);
			Color color = MetroPaint.BackColor.Form(theme);
			Color color2 = MetroPaint.BorderColor.Button.Normal(theme);
			Color color3 = MetroPaint.ForeColor.Label.Normal(theme);
			using (SolidBrush brush = new SolidBrush(color))
			{
				e.Graphics.FillRectangle(brush, e.Bounds);
			}
			using (Pen pen = new Pen(color2))
			{
				e.Graphics.DrawRectangle(pen, new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1));
			}
			Font font = MetroFonts.Default(13f);
			TextRenderer.DrawText(e.Graphics, e.ToolTipText, font, e.Bounds, color3, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
		}

		internal static bool d4umnKXww8GieiKm8Ma()
		{
			return niJ7j2XiYTlQPZVk6KA == null;
		}

		internal static void oKK8dBX5Tx2V7Nn1oUx()
		{
		}
	}
}
