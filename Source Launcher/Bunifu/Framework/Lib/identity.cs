using System;
using System.Management;

namespace Bunifu.Framework.Lib
{
	public static class identity
	{
		private static object g9PhSIIq7Tr1OllKV5KJ;

		public static string getMotherBoardID()
		{
			string result = "";
			try
			{
				foreach (ManagementObject item in new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard").Get())
				{
					result = item["SerialNumber"].ToString();
				}
				return result;
			}
			catch (Exception)
			{
				return result;
			}
		}

		private static string smethod_0()
		{
			string text = smethod_1("Win32_Processor", "UniqueId");
			if (text != "")
			{
				return text;
			}
			text = smethod_1("Win32_Processor", "ProcessorId");
			if (!(text != ""))
			{
				text = smethod_1("Win32_Processor", "Name");
				if (text == "")
				{
					text = smethod_1("Win32_Processor", "Manufacturer");
				}
				return text + smethod_1("Win32_Processor", "MaxClockSpeed");
			}
			return text;
		}

		private static string smethod_1(string string_0, string string_1)
		{
			string text = "";
			foreach (ManagementBaseObject instance in new ManagementClass(string_0).GetInstances())
			{
				if (!(text != ""))
				{
					try
					{
						text = instance[string_1].ToString();
						return text;
					}
					catch
					{
					}
				}
			}
			return text;
		}

		internal static bool f2ANDSIqbgA7YTm96T1R()
		{
			return g9PhSIIq7Tr1OllKV5KJ == null;
		}
	}
}
