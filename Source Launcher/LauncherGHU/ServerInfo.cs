using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace LauncherGHU
{
	public class ServerInfo
	{
		private int int_0;

		private string string_0;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int int_1;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int int_2;

		public int ServerID
		{
			get
			{
				return int_0;
			}
			set
			{
				int_0 = value;
			}
		}

		public string ServerName
		{
			get
			{
				return string_0;
			}
			set
			{
				string_0 = value;
			}
		}

		public int XButton
		{
			[CompilerGenerated]
			get
			{
				return int_1;
			}
			[CompilerGenerated]
			set
			{
				int_1 = value;
			}
		}

		public int YButton
		{
			[CompilerGenerated]
			get
			{
				return int_2;
			}
			[CompilerGenerated]
			set
			{
				int_2 = value;
			}
		}
	}
}
