using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ns0
{
	[DebuggerStepThrough]
	internal static class Class5
	{
		private static object YFkRX7IgtUZlsCLbwyrj;

		public static string smethod_0(string string_0, string string_1, string string_2 = "tu89geji340t89u2")
		{
			byte[] bytes = Encoding.UTF8.GetBytes(string_2);
			byte[] bytes2 = Encoding.UTF8.GetBytes(string_0);
			byte[] bytes3 = new PasswordDeriveBytes(string_1, null).GetBytes(32);
			ICryptoTransform transform = new RijndaelManaged
			{
				Mode = CipherMode.CBC
			}.CreateEncryptor(bytes3, bytes);
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
			cryptoStream.Write(bytes2, 0, bytes2.Length);
			cryptoStream.FlushFinalBlock();
			byte[] inArray = memoryStream.ToArray();
			memoryStream.Close();
			cryptoStream.Close();
			return Convert.ToBase64String(inArray);
		}

		public static string smethod_1(string string_0, string string_1, string string_2 = "tu89geji340t89u2")
		{
			try
			{
				byte[] bytes = Encoding.ASCII.GetBytes(string_2);
				byte[] array = Convert.FromBase64String(string_0);
				byte[] bytes2 = new PasswordDeriveBytes(string_1, null).GetBytes(32);
				ICryptoTransform transform = new RijndaelManaged
				{
					Mode = CipherMode.CBC
				}.CreateDecryptor(bytes2, bytes);
				MemoryStream memoryStream = new MemoryStream(array);
				CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read);
				byte[] array2 = new byte[array.Length];
				int count = cryptoStream.Read(array2, 0, array2.Length);
				memoryStream.Close();
				cryptoStream.Close();
				return Encoding.UTF8.GetString(array2, 0, count);
			}
			catch (Exception)
			{
				return null;
			}
		}

		internal static bool p41VpcIgDM3qN0tdKSZg()
		{
			return YFkRX7IgtUZlsCLbwyrj == null;
		}
	}
}
