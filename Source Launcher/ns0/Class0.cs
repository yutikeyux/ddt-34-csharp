using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading;

namespace ns0
{
	internal class Class0
	{
		private readonly string string_0;

		private readonly string string_1;

		private bool uSvuaEgrnf;

		private readonly SemaphoreSlim semaphoreSlim_0 = new SemaphoreSlim(0);

		public int int_0;

		private static Class0 FrGAeZIB28GR6R5XHhJe;

		public Class0(string string_2, string string_3)
		{
			if (!string.IsNullOrEmpty(string_2))
			{
				if (string.IsNullOrEmpty(string_3))
				{
					throw new ArgumentNullException("fullPathWhereToSave");
				}
				string_0 = string_2;
				string_1 = string_3;
				return;
			}
			throw new ArgumentNullException("url");
		}

		public bool method_0(int int_1)
		{
			try
			{
				Directory.CreateDirectory(Path.GetDirectoryName(string_1));
				if (File.Exists(string_1))
				{
					File.Delete(string_1);
				}
				using WebClient webClient = new WebClient();
				Uri address = new Uri(string_0);
				webClient.DownloadProgressChanged += method_1;
				webClient.DownloadFileCompleted += method_2;
				Console.WriteLine("Downloading file:");
				webClient.DownloadFileAsync(address, string_1);
				semaphoreSlim_0.Wait(int_1);
				return uSvuaEgrnf && File.Exists(string_1);
			}
			catch (Exception value)
			{
				Console.WriteLine("Was not able to download file!");
				Console.Write(value);
				return false;
			}
			finally
			{
				semaphoreSlim_0.Dispose();
			}
		}

		private void method_1(object sender, DownloadProgressChangedEventArgs e)
		{
			Console.Write("\r     -->    {0}%.", e.ProgressPercentage);
			int_0 = e.ProgressPercentage;
		}

		private void method_2(object sender, AsyncCompletedEventArgs e)
		{
			int_0 = 0;
			uSvuaEgrnf = !e.Cancelled;
			if (!uSvuaEgrnf)
			{
				Console.Write(e.Error.ToString());
			}
			Console.WriteLine(Environment.NewLine + "Download finished!");
			semaphoreSlim_0.Release();
		}

		public static bool smethod_0(string string_2, string string_3, int int_1)
		{
			return new Class0(string_2, string_3).method_0(int_1);
		}

		internal static void R0OKdeIBALeeusjkX8vQ()
		{
		}

		internal static bool Gs79U4IByyYA7UsyRorq()
		{
			return FrGAeZIB28GR6R5XHhJe == null;
		}
	}
}
