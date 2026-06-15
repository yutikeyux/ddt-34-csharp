using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using Bussiness.CenterService;
using log4net;
using SqlDataProvider.Data;

namespace Bussiness.Interface
{
	// Token: 0x02000046 RID: 70
	public abstract class BaseInterface
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600038F RID: 911 RVA: 0x0003C4C1 File Offset: 0x0003A6C1
		public static string GetInterName
		{
			get
			{
				return ConfigurationManager.AppSettings["InterName"].ToLower();
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000390 RID: 912 RVA: 0x0003C4D7 File Offset: 0x0003A6D7
		public static string GetLoginKey
		{
			get
			{
				return ConfigurationManager.AppSettings["LoginKey"];
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000391 RID: 913 RVA: 0x0003C4E8 File Offset: 0x0003A6E8
		public static string GetChargeKey
		{
			get
			{
				return ConfigurationManager.AppSettings["ChargeKey"];
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000392 RID: 914 RVA: 0x0003C4F9 File Offset: 0x0003A6F9
		public static string LoginUrl
		{
			get
			{
				return ConfigurationManager.AppSettings["LoginUrl"];
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000393 RID: 915 RVA: 0x0003C50A File Offset: 0x0003A70A
		public virtual int ActiveGold
		{
			get
			{
				return int.Parse(ConfigurationManager.AppSettings["DefaultGold"]);
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000394 RID: 916 RVA: 0x0003C520 File Offset: 0x0003A720
		public virtual int ActiveMoney
		{
			get
			{
				return int.Parse(ConfigurationManager.AppSettings["DefaultMoney"]);
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000395 RID: 917 RVA: 0x0003C536 File Offset: 0x0003A736
		public virtual int ActiveGiftToken
		{
			get
			{
				return int.Parse(ConfigurationManager.AppSettings["DefaultGiftToken"]);
			}
		}

		// Token: 0x06000396 RID: 918 RVA: 0x0003C54C File Offset: 0x0003A74C
		public static string Encrypt(string toEncrypt, string key)
		{
			bool useHashing = true;
			byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);
			bool flag = useHashing;
			byte[] keyArray;
			if (flag)
			{
				MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
				keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
			}
			else
			{
				keyArray = Encoding.UTF8.GetBytes(key);
			}
			ICryptoTransform cTransform = new TripleDESCryptoServiceProvider
			{
				Key = keyArray,
				Mode = CipherMode.ECB,
				Padding = PaddingMode.PKCS7
			}.CreateEncryptor();
			byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
			return Convert.ToBase64String(resultArray, 0, resultArray.Length);
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0003C5DF File Offset: 0x0003A7DF
		public PlayerInfo LoginGame(string text, string text2, ref bool isFirst)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0003C5E8 File Offset: 0x0003A7E8
		public static string Decrypt(string toDecrypt, string key)
		{
			bool useHashing = true;
			byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
			bool flag = useHashing;
			byte[] keyArray;
			if (flag)
			{
				MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
				keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
			}
			else
			{
				keyArray = Encoding.UTF8.GetBytes(key);
			}
			ICryptoTransform cTransform = new TripleDESCryptoServiceProvider
			{
				Key = keyArray,
				Mode = CipherMode.ECB,
				Padding = PaddingMode.PKCS7
			}.CreateDecryptor();
			byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
			return Encoding.UTF8.GetString(resultArray);
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0003C678 File Offset: 0x0003A878
		public static string GetNameBySite(string user, string site)
		{
			bool flag = !string.IsNullOrEmpty(site) && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["LoginKey_" + site]);
			if (flag)
			{
				user = site + "_" + user;
			}
			return user;
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0003C6C8 File Offset: 0x0003A8C8
		public static DateTime ConvertIntDateTime(double d)
		{
			return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddSeconds(d);
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0003C6FC File Offset: 0x0003A8FC
		public static int ConvertDateTimeInt(DateTime time)
		{
			DateTime localTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
			return (int)(time - localTime).TotalSeconds;
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0003C738 File Offset: 0x0003A938
		[Obsolete]
		public static string md5(string str)
		{
			return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "md5").ToLower();
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0003C75C File Offset: 0x0003A95C
		public static string RequestContent(string Url)
		{
			return BaseInterface.RequestContent(Url, 2560);
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0003C77C File Offset: 0x0003A97C
		public static string RequestContent(string Url, int byteLength)
		{
			byte[] numArray = new byte[byteLength];
			HttpWebRequest obj = (HttpWebRequest)WebRequest.Create(Url);
			obj.ContentType = "text/plain";
			Stream responseStream = obj.GetResponse().GetResponseStream();
			int count = responseStream.Read(numArray, 0, numArray.Length);
			string @string = Encoding.UTF8.GetString(numArray, 0, count);
			responseStream.Close();
			return @string;
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0003C7E0 File Offset: 0x0003A9E0
		public static string RequestContent(string Url, string param, string code)
		{
			Encoding encoding = Encoding.GetEncoding(code);
			byte[] bytes = encoding.GetBytes(param);
			encoding.GetString(bytes);
			byte[] numArray = new byte[2560];
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
			httpWebRequest.ServicePoint.Expect100Continue = false;
			httpWebRequest.Method = "POST";
			httpWebRequest.ContentType = "application/x-www-form-urlencoded";
			httpWebRequest.ContentLength = (long)bytes.Length;
			using (Stream requestStream = httpWebRequest.GetRequestStream())
			{
				requestStream.Write(bytes, 0, bytes.Length);
			}
			string @string;
			using (WebResponse response = httpWebRequest.GetResponse())
			{
				int count = response.GetResponseStream().Read(numArray, 0, numArray.Length);
				@string = Encoding.UTF8.GetString(numArray, 0, count);
			}
			return @string;
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0003C8CC File Offset: 0x0003AACC
		public static BaseInterface CreateInterface()
		{
			string getInterName = BaseInterface.GetInterName;
			if (!true)
			{
			}
			BaseInterface result;
			if (!(getInterName == "qunying"))
			{
				if (!(getInterName == "sevenroad"))
				{
					if (!(getInterName == "duowan"))
					{
						result = null;
					}
					else
					{
						result = new DWInterface();
					}
				}
				else
				{
					result = new SRInterface();
				}
			}
			else
			{
				result = new QYInterface();
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0003C934 File Offset: 0x0003AB34
		public virtual PlayerInfo CreateLogin(string name, string password, int zoneId, ref string message, ref int isFirst, string IP, ref bool isError, bool firstValidate, ref bool isActive, string site, string nickname)
		{
			try
			{
				using (PlayerBussiness db = new PlayerBussiness())
				{
					bool isExist = true;
					DateTime forbidDate = DateTime.Now;
					PlayerInfo info = db.LoginGame(name, ref isFirst, ref isExist, ref isError, firstValidate, ref forbidDate, nickname);
					bool flag = info == null;
					if (flag)
					{
						bool flag2 = !db.ActivePlayer(ref info, name, password, true, this.ActiveGold, this.ActiveMoney, IP, site);
						if (flag2)
						{
							info = null;
							message = LanguageMgr.GetTranslation("BaseInterface.LoginAndUpdate.Fail", Array.Empty<object>());
						}
						else
						{
							isActive = true;
							using (CenterServiceClient client = new CenterServiceClient())
							{
								client.ActivePlayer(true);
							}
						}
					}
					else
					{
						bool flag3 = isExist;
						if (!flag3)
						{
							message = LanguageMgr.GetTranslation("ManageBussiness.Forbid1", new object[]
							{
								forbidDate.Year,
								forbidDate.Month,
								forbidDate.Day,
								forbidDate.Hour,
								forbidDate.Minute
							});
							return null;
						}
						using (CenterServiceClient client2 = new CenterServiceClient())
						{
							client2.CreatePlayer(info.ID, name, password, isFirst == 0);
						}
					}
					return info;
				}
			}
			catch (Exception ex)
			{
				BaseInterface.log.Error("LoginAndUpdate", ex);
			}
			return null;
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0003CB0C File Offset: 0x0003AD0C
		public virtual PlayerInfo LoginGame(string name, string pass, int zoneId, ref bool isFirst)
		{
			try
			{
				using (CenterServiceClient centerServiceClient = new CenterServiceClient())
				{
					int userID = 0;
					bool flag = centerServiceClient.ValidateLoginAndGetID(name, pass, zoneId, ref userID, ref isFirst);
					if (flag)
					{
						return new PlayerInfo
						{
							ID = userID,
							UserName = name
						};
					}
				}
			}
			catch (Exception ex)
			{
				BaseInterface.log.Error("LoginGame", ex);
			}
			return null;
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0003CB98 File Offset: 0x0003AD98
		[Obsolete]
		public virtual string[] UnEncryptLogin(string content, ref int result, string site)
		{
			try
			{
				string str = string.Empty;
				bool flag = !string.IsNullOrEmpty(site);
				if (flag)
				{
					str = ConfigurationManager.AppSettings["LoginKey_" + site];
				}
				bool flag2 = string.IsNullOrEmpty(str);
				if (flag2)
				{
					str = BaseInterface.GetLoginKey;
				}
				bool flag3 = !string.IsNullOrEmpty(str);
				if (flag3)
				{
					string[] strArray = content.Split(new char[]
					{
						'|'
					});
					bool flag4 = strArray.Length > 3;
					if (flag4)
					{
						bool flag5 = BaseInterface.md5(strArray[0] + strArray[1] + strArray[2] + str) == strArray[3].ToLower();
						if (flag5)
						{
							return strArray;
						}
						result = 5;
					}
					else
					{
						result = 2;
					}
				}
				else
				{
					result = 4;
				}
			}
			catch (Exception ex)
			{
				BaseInterface.log.Error("UnEncryptLogin", ex);
			}
			return new string[0];
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0003CC90 File Offset: 0x0003AE90
		[Obsolete]
		public virtual string[] UnEncryptCharge(string content, ref int result, string site)
		{
			try
			{
				string str = string.Empty;
				bool flag = !string.IsNullOrEmpty(site);
				if (flag)
				{
					str = ConfigurationManager.AppSettings["ChargeKey_" + site];
				}
				bool flag2 = string.IsNullOrEmpty(str);
				if (flag2)
				{
					str = BaseInterface.GetChargeKey;
				}
				bool flag3 = !string.IsNullOrEmpty(str);
				if (flag3)
				{
					string[] strArray = content.Split(new char[]
					{
						'|'
					});
					string str2 = BaseInterface.md5(string.Concat(new string[]
					{
						strArray[0],
						strArray[1],
						strArray[2],
						strArray[3],
						strArray[4],
						str
					}));
					bool flag4 = strArray.Length > 5;
					if (flag4)
					{
						bool flag5 = str2 == strArray[5].ToLower();
						if (flag5)
						{
							return strArray;
						}
						result = 7;
					}
					else
					{
						result = 8;
					}
				}
				else
				{
					result = 6;
				}
			}
			catch (Exception ex)
			{
				BaseInterface.log.Error("UnEncryptCharge", ex);
			}
			return new string[0];
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0003CDB0 File Offset: 0x0003AFB0
		[Obsolete]
		public virtual string[] UnEncryptSentReward(string content, ref int result, string key)
		{
			try
			{
				string[] strArray = content.Split(new char[]
				{
					'#'
				});
				bool flag = strArray.Length == 8;
				if (flag)
				{
					string appSetting = ConfigurationManager.AppSettings["SentRewardTimeSpan"];
					int num = int.Parse(string.IsNullOrEmpty(appSetting) ? "1" : appSetting);
					TimeSpan timeSpan = string.IsNullOrEmpty(strArray[6]) ? new TimeSpan(1, 1, 1) : (DateTime.Now - BaseInterface.ConvertIntDateTime(double.Parse(strArray[6])));
					bool flag2 = timeSpan.Days == 0 && timeSpan.Hours == 0 && timeSpan.Minutes < num;
					if (flag2)
					{
						bool flag3 = string.IsNullOrEmpty(key);
						if (flag3)
						{
							return strArray;
						}
						bool flag4 = BaseInterface.md5(string.Concat(new string[]
						{
							strArray[2],
							strArray[3],
							strArray[4],
							strArray[5],
							strArray[6],
							key
						})) == strArray[7].ToLower();
						if (flag4)
						{
							return strArray;
						}
						result = 5;
					}
					else
					{
						result = 7;
					}
				}
				else
				{
					result = 6;
				}
			}
			catch (Exception ex)
			{
				BaseInterface.log.Error("UnEncryptSentReward", ex);
			}
			return new string[0];
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0003CF10 File Offset: 0x0003B110
		public virtual bool GetUserSex(string name)
		{
			return true;
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0003CF24 File Offset: 0x0003B124
		public static bool CheckRnd(string str)
		{
			return !string.IsNullOrEmpty(str);
		}

		// Token: 0x0400018F RID: 399
		protected static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
