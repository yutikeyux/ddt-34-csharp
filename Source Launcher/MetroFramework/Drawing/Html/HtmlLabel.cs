using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MetroFramework.Drawing.Html
{
	[CLSCompliant(false)]
	public class HtmlLabel : HtmlPanel
	{
		internal static HtmlLabel bKkwpj6IyhgrdVyFHyL;

		[Description("Automatically sets the size of the label by measuring the content")]
		[DefaultValue(true)]
		[Browsable(true)]
		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;
				if (value)
				{
					MeasureBounds();
				}
			}
		}

		public HtmlLabel()
		{
			SetStyle(ControlStyles.Opaque, value: false);
			AutoScroll = false;
		}

		protected override void CreateFragment()
		{
			string text = Text;
			string text2 = $"font: {(float)Font.Size}pt {Font.FontFamily.Name}";
			htmlContainer = new InitialContainer(string.Concat((string[])(object)new string[5]
			{
				"<table border=0 cellspacing=5 cellpadding=0 style=\"",
				(string)(object)text2,
				"\"><tr><td>",
				(string)(object)text,
				"</td></tr></table>"
			}));
		}

		public override void MeasureBounds()
		{
			base.MeasureBounds();
			if (htmlContainer != null && AutoSize)
			{
				base.Size = System.Drawing.Size.Round(htmlContainer.MaximumSize);
			}
		}

		internal static void BqHmRA67vDeRGjG045D()
		{
		}

		internal static bool qeGhf661AGITg1lZP28()
		{
			return bKkwpj6IyhgrdVyFHyL == null;
		}
	}
}
