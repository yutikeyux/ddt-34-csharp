using System.Drawing;
using System.Windows.Forms;

namespace ns1
{
	public class BunifuCustomLabel : Label
	{
		private static BunifuCustomLabel v9DviJIU7DjA1x9EwjN2;

		public BunifuCustomLabel()
		{
			SetStyle(ControlStyles.UserPaint, value: true);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (base.Enabled)
			{
				base.OnPaint(e);
				return;
			}
			SolidBrush brush = new SolidBrush(ForeColor);
			e.Graphics.DrawString(Text, Font, brush, 0f, 0f);
		}

		internal static void mmCtbMIUOR3TsrWYvcn7()
		{
		}

		internal static bool Yd0vsyIUbVUV8eOuhysa()
		{
			return v9DviJIU7DjA1x9EwjN2 == null;
		}
	}
}
