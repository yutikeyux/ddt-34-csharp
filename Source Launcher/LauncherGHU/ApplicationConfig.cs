namespace LauncherGHU
{
	public class ApplicationConfig
	{
		public static readonly string KeyCodeLauncher;

		public static readonly string UrlHome;

		public static readonly string UrlLostPassword;

		public static readonly string UrlApi;

		public static readonly string UrlLauncherDownload;

		public static readonly string UrlRecharge;

		public static readonly string UrlRegister;

		public static readonly string UrlOauthFacebook;

		public static readonly string UrlOauthGoogle;

		public static readonly string UrlOauthYahoo;

		public static readonly string FanpageLink;

		public static readonly string ServerTitle;

		public static readonly string ServerName;

		public static readonly string ServerNameSpace;

		public static readonly string ServerDomainName;

		public static readonly string ResxFileName;

		public static readonly string LoadingSwf;
		static ApplicationConfig()
		{
			KeyCodeLauncher = "cedrusprovip";
			UrlHome = "http://185.88.175.192/";
			UrlLostPassword = UrlHome + "?p=quen-mat-khau";
			UrlApi = UrlHome + "API/";
			UrlLauncherDownload = "https://admdownload.adobe.com/bin/live/flashplayer32ax_xa_install.exe";
			UrlRecharge = UrlHome + "?p=nap-the";
			UrlRegister = UrlHome + "?p=dang-ky";
			UrlOauthFacebook = "http://facebook.com/huynhduc2210/";
			UrlOauthGoogle = "http://14.225.212.144:9500/";
			UrlOauthYahoo = "http://14.225.212.144:9500/";
			FanpageLink = "https://www.facebook.com/sieugun/";
			ServerNameSpace = "Gà Hướng Dương";
			ServerDomainName = "185.88.175.192";
			ServerTitle = "Gà Hướng Dương";
			ServerName = "Gà Hướng Dương";
			ResxFileName = "Bembemgunny";
			LoadingSwf = "http://flash1.185.88.175.192/Loading.swf";
		}
	}
}
