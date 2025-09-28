using System;

namespace MetroFramework.Drawing.Html
{
	[CLSCompliant(false)]
	public class CssAnonymousBlockBox : CssBox
	{
		internal static CssAnonymousBlockBox L0Lj8M4Ob6XNnausZGC;

		public CssAnonymousBlockBox(CssBox parent)
			: base(parent)
		{
			base.Display = "block";
		}

		public CssAnonymousBlockBox(CssBox parent, CssBox insertBefore)
			: this(parent)
		{
			int num = parent.Boxes.IndexOf(insertBefore);
			if (num < 0)
			{
				throw new Exception("insertBefore box doesn't exist on parent");
			}
			parent.Boxes.Remove(this);
			parent.Boxes.Insert(num, this);
		}

		internal static void F994cv4GGMAqgkRggXv()
		{
		}

		internal static bool xyu30U4f0VvJ8pCkQK6()
		{
			return L0Lj8M4Ob6XNnausZGC == null;
		}
	}
}
