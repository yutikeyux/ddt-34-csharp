using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using ns0;

namespace ns1
{
	[ProvideProperty("BunifuFramework", typeof(Control))]
	[DebuggerStepThrough]
	[DefaultEvent("Click")]
	public class BunifuImageButton : PictureBox
	{
		private int int_0 = 10;

		private Image image_0;

		private Image image_1;

		private IContainer icontainer_0;

		private static BunifuImageButton aJhmaAIU0Wy9YkZQwVax;

		public int Zoom
		{
			get
			{
				return int_0;
			}
			set
			{
				int_0 = value;
			}
		}

		public Image ImageActive
		{
			get
			{
				return image_0;
			}
			set
			{
				image_0 = value;
			}
		}

		public BunifuImageButton()
		{
			method_0();
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			if (image_0 != null)
			{
				image_1 = base.Image;
				base.Image = image_0;
			}
			Class6.smethod_0(this, int_0);
			base.OnMouseEnter(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			if (image_0 != null)
			{
				base.Image = image_1;
			}
			Class6.smethod_1(this);
			base.OnMouseLeave(e);
		}

		protected override void OnClick(EventArgs e)
		{
			if (image_0 != null)
			{
				base.Image = image_1;
			}
			Class6.smethod_1(this);
			base.OnClick(e);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(BunifuImageButton));
			((ISupportInitialize)this).BeginInit();
			SuspendLayout();
			BackColor = Color.SeaGreen;
			base.Image = (Image)componentResourceManager.GetObject("$this.Image");
			base.Size = new Size(71, 71);
			base.SizeMode = PictureBoxSizeMode.Zoom;
			((ISupportInitialize)this).EndInit();
			ResumeLayout(performLayout: false);
		}

		internal static void AHIjHiIU5acxPbhuDkD3()
		{
		}

		internal static bool AceqLHIUuPibDfO8kekK()
		{
			return aJhmaAIU0Wy9YkZQwVax == null;
		}
	}
}
