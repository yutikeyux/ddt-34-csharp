using System.Drawing;

namespace MetroFramework.Drawing.Html
{
	internal class CssBoxWord : CssRectangle
	{
		private string string_0;

		private PointF pointF_0;

		private CssBox cssBox_0;

		private Image fcpnWkupvd;

		private static CssBoxWord CPmYCX4JaDnN5bSic79;

		public float FullWidth => base.Width;

		public Image Image
		{
			get
			{
				return fcpnWkupvd;
			}
			set
			{
				fcpnWkupvd = value;
				if (value != null)
				{
					CssLength cssLength = new CssLength(OwnerBox.Width);
					CssLength cssLength2 = new CssLength(OwnerBox.Height);
					if (cssLength.Number > 0f && cssLength.Unit == CssLength.CssUnit.Pixels)
					{
						base.Width = cssLength.Number;
					}
					else
					{
						base.Width = value.Width;
					}
					if (cssLength2.Number > 0f && cssLength2.Unit == CssLength.CssUnit.Pixels)
					{
						base.Height = cssLength2.Number;
					}
					else
					{
						base.Height = value.Height;
					}
					base.Height += OwnerBox.ActualBorderBottomWidth + OwnerBox.ActualBorderTopWidth + OwnerBox.ActualPaddingTop + OwnerBox.ActualPaddingBottom;
				}
			}
		}

		public bool IsImage => Image != null;

		public bool IsSpaces => string.IsNullOrEmpty(((string)(object)Text).Trim());

		public bool IsLineBreak => Text == "\n";

		public bool IsTab => Text == "\t";

		public CssBox OwnerBox => cssBox_0;

		public string Text => string_0;

		internal PointF LastMeasureOffset
		{
			get
			{
				return pointF_0;
			}
			set
			{
				pointF_0 = value;
			}
		}

		internal CssBoxWord(CssBox owner)
		{
			cssBox_0 = owner;
			string_0 = string.Empty;
		}

		public CssBoxWord(CssBox owner, Image image)
			: this(owner)
		{
			Image = image;
		}

		internal void ReplaceLineBreaksAndTabs()
		{
			string_0 = ((string)(object)string_0).Replace('\n', ' ');
			string_0 = ((string)(object)string_0).Replace('\t', ' ');
		}

		internal void AppendChar(char c)
		{
			string_0 += (char)c;
		}

		public override string ToString()
		{
			return string.Format("{0} ({1} char{2})", ((string)(object)((string)(object)Text).Replace(' ', '-')).Replace("\n", "\\n"), (int)((string)(object)Text).Length, (((string)(object)Text).Length != 1) ? "s" : string.Empty);
		}

		internal static void WOroFe4v7ZTndMhrKiw()
		{
		}

		internal static bool uwi0kh48A8GjXS5wr1I()
		{
			return CPmYCX4JaDnN5bSic79 == null;
		}
	}
}
