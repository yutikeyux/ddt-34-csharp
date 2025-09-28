using System.Collections.Generic;
using System.Globalization;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace Bunifu.Framework.Lib
{
	public static class HardwareFingerprint
	{
		private static string string_0;

		private static string string_1;

		//private static HardwareFingerprint HVyg0iIgyg0ImmXLlocY;

		static HardwareFingerprint()
		{
			string_0 = "";
			string_1 = string.Empty;
		}

		public static string Value()
		{
			if (string.IsNullOrEmpty(string_1))
			{
				string_1 = smethod_5("CPU: " + smethod_8() + " BIOS: " + smethod_9() + " BASE: " + smethod_7() + " VIDEO: " + smethod_3());
			}
			return string_1;
		}

		private static string smethod_0(string string_2, string string_3, string string_4)
		{
			string text = "";
			foreach (ManagementBaseObject instance in new ManagementClass(string_2).GetInstances())
			{
				if (!(instance[string_4].ToString() != "True") && !(text != ""))
				{
					try
					{
						text = instance[string_3].ToString();
						return text;
					}
					catch
					{
					}
				}
			}
			return text;
		}

		private static string smethod_1(string string_2, string string_3)
		{
			string text = "";
			foreach (ManagementBaseObject instance in new ManagementClass(string_2).GetInstances())
			{
				if (!(text != ""))
				{
					try
					{
						text = instance[string_3].ToString();
						return text;
					}
					catch
					{
					}
				}
			}
			return text;
		}

		private static string smethod_2()
		{
			return smethod_1("Win32_DiskDrive", "Model") + smethod_1("Win32_DiskDrive", "Manufacturer") + smethod_1("Win32_DiskDrive", "Signature") + smethod_1("Win32_DiskDrive", "TotalHeads");
		}

		private static string smethod_3()
		{
			return smethod_1("Win32_VideoController", "DriverVersion") + smethod_1("Win32_VideoController", "Name");
		}

		private static string smethod_4()
		{
			return smethod_0("Win32_NetworkAdapterConfiguration", "MACAddress", "IPEnabled");
		}

		private static string smethod_5(string string_2)
		{
			MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
			byte[] bytes = Encoding.ASCII.GetBytes(string_2);
			return smethod_6(mD5CryptoServiceProvider.ComputeHash(bytes));
		}

		private static string smethod_6(IList<byte> ilist_0)
		{
			string text = string.Empty;
			for (int i = 0; i < ilist_0.Count; i++)
			{
				byte num = ilist_0[i];
				int num2 = num & 0xF;
				int num3 = (num >> 4) & 0xF;
				text = ((num3 <= 9) ? (text + num3.ToString(CultureInfo.InvariantCulture)) : (text + ((char)(num3 - 10 + 65)).ToString(CultureInfo.InvariantCulture)));
				text = ((num2 <= 9) ? (text + num2.ToString(CultureInfo.InvariantCulture)) : (text + ((char)(num2 - 10 + 65)).ToString(CultureInfo.InvariantCulture)));
				if (i + 1 != ilist_0.Count && (i + 1) % 2 == 0)
				{
					text += "-";
				}
			}
			return text;
		}

		private static string smethod_7()
		{
			return smethod_1("Win32_BaseBoard", "Model") + smethod_1("Win32_BaseBoard", "Manufacturer") + smethod_1("Win32_BaseBoard", "Name") + smethod_1("Win32_BaseBoard", "SerialNumber");
		}

		private static string smethod_8()
		{
			string text = smethod_1("Win32_Processor", "UniqueId");
			if (text != "")
			{
				return text;
			}
			text = smethod_1("Win32_Processor", "ProcessorId");
			if (text != "")
			{
				return text;
			}
			text = smethod_1("Win32_Processor", "Name");
			if (text == "")
			{
				text = smethod_1("Win32_Processor", "Manufacturer");
			}
			return text + smethod_1("Win32_Processor", "MaxClockSpeed");
		}

		private static string smethod_9()
		{
			return smethod_1("Win32_BIOS", "Manufacturer") + smethod_1("Win32_BIOS", "SMBIOSBIOSVersion") + smethod_1("Win32_BIOS", "IdentificationCode") + smethod_1("Win32_BIOS", "SerialNumber") + smethod_1("Win32_BIOS", "ReleaseDate") + smethod_1("Win32_BIOS", "Version");
		}

		internal static void JRo1SjIgc2F9LVwuKcIZ()
		{
		}

		//internal static bool YFtOm7IgRFprkQscW4Yw()
		//{
		//	return HVyg0iIgyg0ImmXLlocY == null;
		//}
	}
}
