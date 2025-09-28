using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsControlLibrary1
{
	public class BunifuCustomTextbox : TextBox
	{
		private Color color_0 = Color.SeaGreen;

		private IContainer icontainer_0;

		internal static BunifuCustomTextbox Cr8ZORIN6Y3F75YcPJN6;

		public Color BorderColor
		{
			get
			{
				return color_0;
			}
			set
			{
				color_0 = value;
				Refresh();
			}
		}

		public BunifuCustomTextbox()
		{
			method_0();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Pen pen = new Pen(BorderColor, 1f);
			Rectangle rect = new Rectangle(e.ClipRectangle.X, e.ClipRectangle.Y, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1);
			e.Graphics.DrawRectangle(pen, rect);
			Rectangle bounds = new Rectangle(e.ClipRectangle.X + 1, e.ClipRectangle.Y + 1, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1);
			TextRenderer.DrawText(e.Graphics, Text, Font, bounds, ForeColor, BackColor, TextFormatFlags.Default);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && icontainer_0 != null)
			{
				icontainer_0.Dispose();
			}
			base.Dispose(disposing);
		}

		private void method_0()
		{
			icontainer_0 = new Container();
		}

		internal static void n6p3JkINDP4jWDja09Ew()
		{
		}

		internal static bool BZNCTIINSLsBBCM2mJYs()
		{
			return Cr8ZORIN6Y3F75YcPJN6 == null;
		}
	}
}
