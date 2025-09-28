using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace AutoUpdaterDotNET
{
	public class UpdateInfoEventArgs : EventArgs
	{
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool bool_0;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string string_0;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private string string_1;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Version version_0;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private Version version_1;

		private static UpdateInfoEventArgs XINFl87oEE4i7sxANUJ;

		public bool IsUpdateAvailable
		{
			[CompilerGenerated]
			get
			{
				return bool_0;
			}
			[CompilerGenerated]
			set
			{
				bool_0 = value;
			}
		}

		public string String_0
		{
			[CompilerGenerated]
			get
			{
				return string_0;
			}
			[CompilerGenerated]
			set
			{
				string_0 = value;
			}
		}

		public string ChangelogURL
		{
			[CompilerGenerated]
			get
			{
				return string_1;
			}
			[CompilerGenerated]
			set
			{
				string_1 = value;
			}
		}

		public Version CurrentVersion
		{
			[CompilerGenerated]
			get
			{
				return version_0;
			}
			[CompilerGenerated]
			set
			{
				version_0 = value;
			}
		}

		public Version InstalledVersion
		{
			[CompilerGenerated]
			get
			{
				return version_1;
			}
			[CompilerGenerated]
			set
			{
				version_1 = value;
			}
		}

		internal static bool nSnw027VmAgqc1hJpUc()
		{
			return XINFl87oEE4i7sxANUJ == null;
		}

		internal static void YS2Hqu7CCMiYWJaGxHX()
		{
		}
	}
}
