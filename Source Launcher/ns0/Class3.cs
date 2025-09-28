using System.Diagnostics;

namespace ns0
{
	[DebuggerStepThrough]
	internal static class Class3
	{
		private static Process process_0;

		private static object o0MqcIIgCI25xhDG5oqa;

		internal static void smethod_0(string string_0)
		{
			process_0 = new Process();
			process_0.StartInfo.FileName = "CMD.exe";
			process_0.StartInfo.Arguments = "/C " + string_0;
			process_0.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			process_0.Start();
			process_0.WaitForExit();
		}

		internal static bool YQaJFkIgYOGaw7Kw8v3G()
		{
			return o0MqcIIgCI25xhDG5oqa == null;
		}
	}
}
