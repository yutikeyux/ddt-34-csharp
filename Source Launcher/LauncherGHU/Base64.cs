using System;
using System.Text;

namespace LauncherGHU
{
	public static class Base64
	{
		internal static object SP6WPN7A19dOmFRHsJR;

		public static string EncodeBase64(this Encoding encoding, string text)
		{
			if (text != null)
			{
				byte[] bytes = encoding.GetBytes(text);
				return Convert.ToBase64String(bytes);
			}
			return null;
		}

		public static string DecodeBase64(this Encoding encoding, string encodedText)
		{
			if (encodedText != null)
			{
				byte[] bytes = Convert.FromBase64String(encodedText);
				return encoding.GetString(bytes);
			}
			return null;
		}

		internal static bool NX6Shu7cuar1xpHqTxL()
		{
			return SP6WPN7A19dOmFRHsJR == null;
		}
	}
}
