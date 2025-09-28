using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace LauncherGHU.API
{
	public class RechargeAPI
	{
		private static RechargeAPI o5qhsxmAxim5XAlJ2jv;

		public static string Recharge(string url, string UserName, string Password, int typeCard, string serial, string passcard, int moneycard)
		{
			try
			{
				return Encoding.UTF8.GetString(new WebClient
				{
					Encoding = Encoding.UTF8
				}.UploadValues(url, new NameValueCollection
				{
					["UsernameTxt"] = UserName,
					["PasswordTxt"] = Password,
					["txtType"] = typeCard.ToString(),
					["txtSerial"] = serial,
					["txtPasscard"] = passcard.ToString(),
					["menhgia_the"] = moneycard.ToString()
				})).Replace("\r\n   ", string.Empty);
			}
			catch (Exception ex)
			{
				ControlMgr.UpLogLauncher(ex.Message);
				return "";
			}
		}

		public static string SetupCharater(string url, int svid, string username)
		{
			try
			{
				string text = Encoding.UTF8.GetString(new WebClient
				{
					Encoding = Encoding.UTF8
				}.UploadValues(url, new NameValueCollection
				{
					["sid"] = svid.ToString(),
					["username"] = username
				})).Replace("\r\n   ", string.Empty);
				//return text.Split('+')[1];
				return text;
			}
			catch (Exception ex)
			{
				ControlMgr.UpLogLauncher(ex.Message);
				return null;
			}
		}

		public static string ChangeMoney(string url, string UserName, string Password, string coinChane, string serverid, string useridchange, string passs2)
		{
			try
			{
				return Encoding.UTF8.GetString(new WebClient
				{
					Encoding = Encoding.UTF8
				}.UploadValues(url, new NameValueCollection
				{
					["UsernameTxt"] = UserName,
					["PasswordTxt"] = Password,
					["txtCoin"] = coinChane,
					["txtServer"] = serverid,
					["txtcharacter"] = useridchange,
					["txtPassword2"] = passs2
				})).Replace("\r\n   ", string.Empty);
			}
			catch (Exception ex)
			{
				ControlMgr.UpLogLauncher(ex.Message);
				return "";
			}
		}

		internal static bool NWtsP5mc0JLmIBe1Moc()
		{
			return o5qhsxmAxim5XAlJ2jv == null;
		}

		internal static void ysFBWamz2UHHEbmeDs2()
		{
		}
	}
}
