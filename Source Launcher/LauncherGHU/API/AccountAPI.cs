using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace LauncherGHU.API
{
	public class AccountAPI
	{
		internal static AccountAPI oGbZmnmEtxQ9ocQQpVu;

		public static string ChangePass(string url, string currentPassword, string newPass)
		{
			try
			{
				return Encoding.UTF8.GetString(new WebClient
				{
					Encoding = Encoding.UTF8
				}.UploadValues(url, new NameValueCollection
				{
					["txtUser"] = LoginMgr.Username,
					["txtPassword"] = currentPassword,
					["txtNewPassword"] = newPass,
				})).Replace("\r\n   ", string.Empty);
			}
			catch (Exception ex)
			{
				ControlMgr.UpLogLauncher(ex.Message);
				return "";
			}
		}

		public static string ChangePassTwo(string url, string newPass, string OTP)
		{
			return "";
		}

		public static string[] Login(string UserName, string Password)
		{
			try
			{
				string text = Encoding.UTF8.GetString(new WebClient
				{
					Encoding = Encoding.UTF8
				}.UploadValues(ApplicationConfig.UrlApi + "checkfirstlogin.php", new NameValueCollection
				{
					["user"] = UserName,
					["pass"] = Password
				})).Replace("\r\n   ", string.Empty);
				return text.Split('|');
			}
			catch (Exception ex)
			{
				ControlMgr.UpLogLauncher(ex.Message);
				return null;
			}
		}

		public static string ChangePhoneNumber(string url, string phone, string OTP)
		{
			return "";
		}

		public static string Register(string url, string UserName, string Password/*, string phone*//*, string userref, string sexText*/)
		{
			try
			{
				return Encoding.UTF8.GetString(new WebClient
				{
					Encoding = Encoding.UTF8
				}.UploadValues(url, new NameValueCollection
				{
					["txtUser"] = UserName,
					["txtPassword"] = Password,
					//["txtPhone"] = phone,
					//["txtRef"] = userref,
					//["txtSex"] = sexText
				})).Replace("\r\n   ", string.Empty);
			}
			catch (Exception ex)
			{
				ControlMgr.UpLogLauncher(ex.Message);
				return "";
			}
		}

		public static string ForgotPass(string url, string username, string password, string OTP)
		{
			try
			{
				return Encoding.UTF8.GetString(new WebClient
				{
					Encoding = Encoding.UTF8
				}.UploadValues(url, new NameValueCollection
				{
					["txtUser"] = username,
					["txtPassword"] = password,
					["txtOtp"] = OTP
				})).Replace("\r\n   ", string.Empty);
			}
			catch (Exception ex)
			{
				ControlMgr.UpLogLauncher(ex.Message);
				return "";
			}
		}

		internal static bool lypRhUmWbu7fyhVLBXf()
		{
			return oGbZmnmEtxQ9ocQQpVu == null;
		}

		internal static void udX3YTmRTA5dHq5y48X()
		{
		}
	}
}
