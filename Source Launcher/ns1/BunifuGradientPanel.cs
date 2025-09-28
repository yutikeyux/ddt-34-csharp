using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ns0;

namespace ns1
{
	public class BunifuGradientPanel : Panel
	{
		private Color color_0 = Color.White;

		private Color EqqrxlokJn = Color.White;

		private Color color_1 = Color.White;

		private Color color_2 = Color.White;

		private int int_0 = 10;

		private IContainer icontainer_0;

		private static BunifuGradientPanel RbkvftIUtGyCan9DWqrD;

		public Color GradientTopLeft
		{
			get
			{
				return color_0;
			}
			set
			{
				color_0 = value;
				method_0();
			}
		}

		public Color GradientTopRight
		{
			get
			{
				return EqqrxlokJn;
			}
			set
			{
				EqqrxlokJn = value;
				method_0();
			}
		}

		public Color GradientBottomLeft
		{
			get
			{
				return color_1;
			}
			set
			{
				color_1 = value;
				method_0();
			}
		}

		public Color GradientBottomRight
		{
			get
			{
				return color_2;
			}
			set
			{
				color_2 = value;
				method_0();
			}
		}

		public int Quality
		{
			get
			{
				return int_0;
			}
			set
			{
				int_0 = value;
				method_0();
			}
		}

		public BunifuGradientPanel()
		{
			method_1();
			method_0();
		}

		private void method_0()
		{
			Bitmap bitmap = new Bitmap(Quality, Quality);
			if (Quality == 100)
			{
				bitmap = new Bitmap(base.Width, base.Height);
			}
			for (int i = 0; i < bitmap.Width; i++)
			{
				Color colorScale = BunifuColorTransition.getColorScale(int.Parse(Math.Round((double)i / (double)bitmap.Width * 100.0, 0).ToString()), GradientTopLeft, GradientTopRight);
				for (int j = 0; j < bitmap.Height; j++)
				{
					Color colorScale2 = BunifuColorTransition.getColorScale(int.Parse(Math.Round((double)j / (double)bitmap.Height * 100.0, 0).ToString()), GradientBottomLeft, GradientBottomRight);
					bitmap.SetPixel(i, j, Class7.smethod_3(colorScale, colorScale2));
				}
			}
			if (BackgroundImageLayout != ImageLayout.Stretch)
			{
				BackgroundImageLayout = ImageLayout.Stretch;
			}
			BackgroundImage = bitmap;
		}

		private void BunifuGradientPanel_Resize(object sender, EventArgs e)
		{
			method_0();
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
			SuspendLayout();
			base.Resize += BunifuGradientPanel_Resize;
			ResumeLayout(performLayout: false);
		}

		internal static void gYuOaWIUTyYnyTgpQ5pJ()
		{
		}

		internal static bool tX9Md7IUDHaK5FioC2QA()
		{
			return RbkvftIUtGyCan9DWqrD == null;
		}
	}
}
