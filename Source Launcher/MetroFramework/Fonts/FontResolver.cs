using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace MetroFramework.Fonts
{
	public class FontResolver : MetroFonts.Interface0
	{
		private readonly PrivateFontCollection privateFontCollection_0 = new PrivateFontCollection();

		private static FontResolver parGwk9OwllZIyYcUuX;

		public Font ResolveFont(string familyName, float emSize, FontStyle fontStyle, GraphicsUnit unit)
		{
			Font font = new Font(familyName, emSize, fontStyle, unit);
			if (font.Name == familyName || !smethod_0(ref familyName, ref fontStyle))
			{
				return font;
			}
			font.Dispose();
			FontFamily family = method_0(familyName);
			return new Font(family, emSize, fontStyle, unit);
		}

		private static bool smethod_0(ref string string_0, ref FontStyle fontStyle_0)
		{
			if (!(string_0 == "Segoe UI Light"))
			{
				if (!(string_0 == "Segoe UI"))
				{
					return false;
				}
				if (fontStyle_0 == FontStyle.Bold)
				{
					string_0 = "Open Sans Bold";
					return true;
				}
				string_0 = "Open Sans";
				return true;
			}
			string_0 = "Open Sans Light";
			if (fontStyle_0 != FontStyle.Bold)
			{
				fontStyle_0 = FontStyle.Regular;
			}
			return true;
		}

		private FontFamily method_0(string string_0)
		{
			lock (privateFontCollection_0)
			{
				FontFamily[] families = privateFontCollection_0.Families;
				int num = 0;
				FontFamily fontFamily;
				while (true)
				{
					if (num < families.Length)
					{
						fontFamily = families[num];
						if (fontFamily.Name == string_0)
						{
							break;
						}
						num++;
						continue;
					}
					string name = GetType().Namespace + ".Resources." + ((string)(object)string_0).Replace(' ', '_') + ".ttf";
					Stream stream = null;
					IntPtr intPtr = IntPtr.Zero;
					try
					{
						stream = GetType().Assembly.GetManifestResourceStream(name);
						int num2 = (int)stream.Length;
						intPtr = Marshal.AllocCoTaskMem(num2);
						byte[] array = (byte[])(object)new byte[num2];
						stream.Read(array, 0, num2);
						Marshal.Copy(array, 0, intPtr, num2);
						privateFontCollection_0.AddMemoryFont(intPtr, num2);
						return privateFontCollection_0.Families[privateFontCollection_0.Families.Length - 1];
					}
					finally
					{
						stream?.Dispose();
						if (intPtr != IntPtr.Zero)
						{
							Marshal.FreeCoTaskMem(intPtr);
						}
					}
				}
				return fontFamily;
			}
		}

		internal static bool egeTfw9foK4F4OU69XA()
		{
			return parGwk9OwllZIyYcUuX == null;
		}

		internal static void H5oYQS9mnpw1Ngmgkd5()
		{
		}
	}
}
