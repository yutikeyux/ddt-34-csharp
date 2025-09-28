using System;
using System.Windows.Forms;
using Bunifu.Framework.Lib;
using ns0;

namespace Bunifu.Framework
{
	public static class License
	{
		public static string myidentity;

		private static object NVA151IOx3L95Gjf1Atu;

		static License()
		{
			myidentity = "";
			if (IsInDesignMode())
			{
				Check(firstTime: true);
			}
		}

		public static void Check(Control sender)
		{
		}

		public static void Check(bool firstTime)
		{
			if (!IsInDesignMode())
			{
				return;
			}
			string text = ValidateLicense();
			if (text.ToLower().Trim() != "true")
			{
				DialogResult dialogResult = default(DialogResult);
				if (dialogResult == DialogResult.Abort)
				{
					Class3.smethod_0("taskkill /im devenv.exe /f");
				}
				if (dialogResult == DialogResult.Ignore && firstTime)
				{
					Timer timer = new Timer();
					timer.Interval = 5000;
					timer.Tick += smethod_0;
					timer.Start();
				}
			}
		}

		private static void smethod_0(object object_1, EventArgs object_0)
		{
			//timer_0.Enabled = false;
			Check(firstTime: false);
			//timer_0.Enabled = true;
		}

		public static string ValidateLicense()
		{
			if (myidentity.Trim().Length == 0)
			{
				myidentity = HardwareFingerprint.Value().ToString();
			}
			try
			{
				string text = Class5.smethod_1(Environment.GetEnvironmentVariable("BunifuFramework.product", EnvironmentVariableTarget.Machine).Trim(), myidentity);
				if (!(Environment.GetEnvironmentVariable("BunifuFramework.product", EnvironmentVariableTarget.Machine).Trim() == "Blocked"))
				{
					if (text.Trim().Length <= 0)
					{
						return "Bunifu Framework not installed";
					}
					string text2 = Class5.smethod_1(Environment.GetEnvironmentVariable("BunifuFramework.token", EnvironmentVariableTarget.Machine).Trim(), myidentity);
					string s = Class5.smethod_1(Environment.GetEnvironmentVariable("BunifuFramework.date", EnvironmentVariableTarget.Machine).Trim(), myidentity);
					string s2 = Class5.smethod_1(Environment.GetEnvironmentVariable("BunifuFramework.days", EnvironmentVariableTarget.Machine).Trim(), myidentity);
					string s3 = Class5.smethod_1(Environment.GetEnvironmentVariable("BunifuFramework.expiry", EnvironmentVariableTarget.Machine).Trim(), myidentity);
					Class5.smethod_1(Environment.GetEnvironmentVariable("BunifuFramework.email", EnvironmentVariableTarget.Machine).Trim(), myidentity);
					Class5.smethod_1(Environment.GetEnvironmentVariable("BunifuFramework.key", EnvironmentVariableTarget.Machine).Trim(), myidentity);
					string text3 = DateTime.Now.Ticks.ToString();
					try
					{
						text3 = Class5.smethod_1(Environment.GetEnvironmentVariable("BunifuFramework.lastseen", EnvironmentVariableTarget.User).Trim(), myidentity);
					}
					catch
					{
					}
					if (text3.ToLower() == "blocked")
					{
						return "Bunifu Framework Blocked";
					}
					if (!(text2.ToLower() == "blocked"))
					{
						if (!(myidentity != text2))
						{
							DateTime dateTime = new DateTime(long.Parse(s));
							DateTime dateTime2 = new DateTime(long.Parse(text3));
							DateTime dateTime3 = new DateTime(long.Parse(s3));
							if (!(DateTime.Now < dateTime))
							{
								if (DateTime.Now < dateTime2)
								{
									return "-Please Correct the system date to proceed [LS]";
								}
								Environment.SetEnvironmentVariable("BunifuFramework.stat.lastseen", Class5.smethod_0(DateTime.Today.Ticks.ToString(), myidentity), EnvironmentVariableTarget.User);
								if (!(DateTime.Now > dateTime3))
								{
									TimeSpan timeSpan = new TimeSpan(dateTime3.Ticks - DateTime.Now.Ticks);
									if (int.Parse(s2) - timeSpan.Days <= 0)
									{
										return "License Expired.";
									}
									return true.ToString();
								}
								return "Design Time License expired :(";
							}
							return "-Please Correct the system date to proceed [DI]";
						}
						return "-Bunifu License Rejected, Contact Bunifu for support";
					}
					return "Bunifu Framework Blocked";
				}
				return "Bunifu framework installation blocked";
			}
			catch (Exception)
			{
				return "[M] Bunifu Framework not installed ";
			}
		}

		public static bool IsInDesignMode()
		{
			if (Application.ExecutablePath.IndexOf("devenv.exe", StringComparison.OrdinalIgnoreCase) <= -1)
			{
				return false;
			}
			return true;
		}

		public static void Authenticate(string email, string token)
		{
			if (Class5.smethod_1(token, "dMyKVp19z6") == email)
			{
				Class1.bool_0 = true;
			}
			else
			{
				Class1.bool_0 = false;
			}
		}

		public static void ViewLicenseAgreement()
		{
			MessageBox.Show("IMPORTANT – PLEASE READ THIS END USER LICENSE AGREEMENT (THE “AGREEMENT”) CAREFULLY\r\n\r\nBEFORE ATTEMPTING TO DOWNLOAD OR USE ANY SOFTWARE, DOCUMENTATION, OR OTHER\r\n\r\nMATERIALS MADE AVAILABLE THROUGH THIS WEB SITE (devtools.bunifu.co.ke).\r\n\r\nTHIS AGREEMENT CONSTITUTES A LEGALLY BINDING AGREEMENT BETWEEN YOU OR THE COMPANY\r\n\r\nWHICH YOU REPRESENT AND ARE AUTHORIZED TO BIND (the “Licensee” or “You”), AND BUNIFU\r\n\r\nTECHNOLOGIES LTD. AD (“Bunifu Technologies Ltd.” or “Licensor”).\r\n\r\nPLEASE CHECK THE “I HAVE READ AND AGREE TO THE LICENSE AGREEMENT” BOX AT THE BOTTOM\r\n\r\nOF THIS AGREEMENT IF YOU AGREE TO BE BOUND BY THE TERMS AND CONDITIONS OF THIS\r\n\r\nAGREEMENT. BY CHECKING THE “I HAVE READ AND AGREE TO THE LICENSE AGREEMENT” BOX\r\n\r\nAND/OR BY PURCHASING, DOWNLOADING, INSTALLING OR OTHERWISE USING THE SOFTWARE\r\n\r\nMADE AVAILABLE BY BUNIFU THROUGH THIS WEB SITE, YOU ACKNOWLEDGE:\r\n\r\n(1) THAT YOU HAVE READ THIS AGREEMENT,\r\n\r\n(2) THAT YOU UNDERSTAND IT,\r\n\r\n(3) THAT YOU AGREE TO BE BOUND BY ITS TERMS AND CONDITIONS, AND\r\n\r\n(4) TO THE EXTENT YOU ARE ENTERING INTO THIS AGREEMENT ON BEHALF OF A COMPANY, YOU\r\n\r\nHAVE THE POWER AND AUTHORITY TO BIND THAT COMPANY.\r\n\r\nContent Management System and/or .NET component vendors are not allowed to use the Software\r\n\r\n(as defined below) without the express permission of Bunifu Technologies Ltd. If you or the company\r\n\r\nyou represent is a Content Management System or .NET component vendor, you may not purchase a\r\n\r\nlicense for or use the Software unless you contact Bunifu Technologies Ltd. directly and obtain\r\n\r\npermission.\r\n\r\nThis License does not grant You a license or any rights to the “2007 Microsoft Office System User\r\n\r\nInterface” and You must contact Microsoft directly to obtain such a license. Any and all rights in the\r\n\r\nSoftware not expressly granted to You as part of the License hereunder are reserved in all respects by\r\n\r\nBunifu.");
		}

		internal static void TLhcHbIOBwVROWKBfrLC()
		{
		}

		internal static bool FCLiOFIO7OnKJ91HowLP()
		{
			return NVA151IOx3L95Gjf1Atu == null;
		}
	}
}
