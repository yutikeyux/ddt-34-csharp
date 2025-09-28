namespace hoiuclib
{
	public class Global
	{
		public static Version MinimumVersionToAllowFullscreen;

		internal static Global B59TgQxi2KXNNKH56mL;

		public static bool IsFlashInstalled => cedrus__wrapper.FPCIsFlashInstalled() != 0;

		public static bool IsTransparentModeAvailable => cedrus__wrapper.FPCIsTransparentAvailable() != 0;

		public static Version InstalledFlashVersion
		{
			get
			{
				cedrus__wrapper.GetInstalledFlashVersionEx(out var version);
				return new Version(version.v3, version.v2, version.v1, version.v0);
			}
		}

		static Global()
		{
			MinimumVersionToAllowFullscreen = new Version(9, 0, 28, 0);
		}

		internal static bool zRQvgdxwqrQqKKbDfO9()
		{
			return B59TgQxi2KXNNKH56mL == null;
		}

		internal static void O0xWGExkYgcLKgoQ8BC()
		{
		}
	}
}
