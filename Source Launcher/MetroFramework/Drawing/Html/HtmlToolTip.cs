using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace MetroFramework.Drawing.Html
{
	public class HtmlToolTip : ToolTip
	{
		private InitialContainer initialContainer_0;

		internal static HtmlToolTip UBQCIW6Dgmyf7iHimUk;

		public HtmlToolTip()
		{
			base.OwnerDraw = true;
			base.Popup += HtmlToolTip_Popup;
			base.Draw += HtmlToolTip_Draw;
		}

		private void HtmlToolTip_Popup(object sender, PopupEventArgs e)
		{
			string toolTip = GetToolTip(e.AssociatedControl);
			string text = string.Format(NumberFormatInfo.InvariantInfo, "font: {0}pt {1}", (float)e.AssociatedControl.Font.Size, e.AssociatedControl.Font.FontFamily.Name);
			initialContainer_0 = new InitialContainer(string.Concat((string[])(object)new string[5]
			{
				"<table class=htmltooltipbackground cellspacing=5 cellpadding=0 style=\"",
				(string)(object)text,
				"\"><tr><td style=border:0px>",
				(string)(object)toolTip,
				"</td></tr></table>"
			}));
			initialContainer_0.SetBounds(new Rectangle(0, 0, 10, 10));
			initialContainer_0.AvoidGeometryAntialias = true;
			using (Graphics g = e.AssociatedControl.CreateGraphics())
			{
				initialContainer_0.MeasureBounds(g);
			}
			e.ToolTipSize = Size.Round(initialContainer_0.MaximumSize);
		}

		private void HtmlToolTip_Draw(object sender, DrawToolTipEventArgs e)
		{
			e.Graphics.Clear(Color.White);
			if (initialContainer_0 != null)
			{
				initialContainer_0.Paint(e.Graphics);
			}
		}

		internal static bool kkh4ig6hB4nXXuB0gSP()
		{
			return UBQCIW6Dgmyf7iHimUk == null;
		}
	}
}
