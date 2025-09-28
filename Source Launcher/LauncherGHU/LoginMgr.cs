using System;
using System.Windows.Forms;

namespace LauncherGHU
{
	public class LoginMgr
	{
		public static bool IsLogin;

		public static string Username;

		public static string Password;

		public static string PasswordReal;

		public static int ServerID;

		public static int Resource;

		public static string string_1;

		public static bool UseOcxFlash;

		public static bool UseNewForm;

		public static int ModeFlash;

		internal static LoginMgr fy5EqGfPrOGFgTVW8TL;

		public static void Login(string username, string password, int mode)
		{
			IsLogin = true;
			Username = username;
			Password = password;
			ModeFlash = mode;
		}

		public static void Logout()
		{
			IsLogin = false;
			Username = null;
			Password = null;
			ServerID = 0;
		}

		public static LoginResultInfo DecompileText(string text)
		{
			try
			{
				string[] array = text.Split('|');
				if (array.Length != 0)
				{
					LoginResultInfo loginResultInfo = new LoginResultInfo();
					loginResultInfo.ResultCode = int.Parse(array[0].Trim());
					if (loginResultInfo.ResultCode != 1)
					{
						loginResultInfo.ErrorText = array[1];
						return loginResultInfo;
					}
					loginResultInfo.Username = array[1];
					loginResultInfo.Password = array[2];
					return loginResultInfo;
				}
				return null;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
				return null;
			}
		}

		static LoginMgr()
		{
			IsLogin = false;
			Username = "default";
			Password = "";
			PasswordReal = "";
			ServerID = 0;
			Resource = 0;
			string_1 = null;
			UseOcxFlash = true;
			UseNewForm = false;
			ModeFlash = 0;
		}

		internal static bool wLTuPCfI0m6DR3tv8cy()
		{
			return fy5EqGfPrOGFgTVW8TL == null;
		}
	}
}
