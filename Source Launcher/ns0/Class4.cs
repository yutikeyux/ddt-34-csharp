using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace ns0
{
	[DebuggerStepThrough]
	internal class Class4
	{
		public string string_0 = "";

		public string string_1 = "";

		public Exception exception_0 = new Exception();

		private static Class4 E0bDkxIgHlV2fI82D6ER;

		public Class4(string string_2)
		{
			string_0 = string_2;
		}

		public bool method_0(string string_2)
		{
			try
			{
				HttpWebRequest obj = (HttpWebRequest)WebRequest.Create(string_0);
				obj.Method = "POST";
				byte[] bytes = Encoding.ASCII.GetBytes(string_2);
				obj.ContentType = "application/x-www-form-urlencoded";
				obj.ContentLength = bytes.Length;
				Stream requestStream = obj.GetRequestStream();
				requestStream.Write(bytes, 0, bytes.Length);
				requestStream.Close();
				HttpWebResponse obj2 = (HttpWebResponse)obj.GetResponse();
				obj2.GetResponseStream();
				string text = (string_1 = new StreamReader(obj2.GetResponseStream()).ReadToEnd());
				return true;
			}
			catch (Exception ex)
			{
				string_1 = "";
				exception_0 = ex;
				return false;
			}
		}

		internal static void NXWXUlIg6aCX5UwaoL4c()
		{
		}

		internal static bool FgkaUlIglXc3qbR3Gvyp()
		{
			return E0bDkxIgHlV2fI82D6ER == null;
		}
	}
}
