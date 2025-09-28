using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Win32;

namespace AutoUpdaterDotNET
{
	public static class AutoUpdater
	{
		public delegate void CheckForUpdateEventHandler(UpdateInfoEventArgs args);

		internal static string ChangeLogURL;

		internal static string string_0;

		internal static string RegistryLocation;

		internal static string AppTitle;

		internal static Version CurrentVersion;

		internal static Version InstalledVersion;

		internal static bool IsWinFormsApplication;

		public static string string_1;

		public static bool OpenDownloadPage;

		public static CultureInfo CurrentCulture;

		public static bool LetUserSelectRemindLater;

		public static int RemindLaterAt;

		public static RemindLaterFormat RemindLaterTimeSpan;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static CheckForUpdateEventHandler checkForUpdateEventHandler_0;

		//internal static AutoUpdater cYiBkb7fQJGZJ51G1AG;

		public static event CheckForUpdateEventHandler CheckForUpdateEvent
		{
			[CompilerGenerated]
			add
			{
				CheckForUpdateEventHandler checkForUpdateEventHandler = checkForUpdateEventHandler_0;
				CheckForUpdateEventHandler checkForUpdateEventHandler2;
				do
				{
					checkForUpdateEventHandler2 = checkForUpdateEventHandler;
					CheckForUpdateEventHandler value2 = (CheckForUpdateEventHandler)Delegate.Combine(checkForUpdateEventHandler2, value);
					checkForUpdateEventHandler = Interlocked.CompareExchange(ref checkForUpdateEventHandler_0, value2, checkForUpdateEventHandler2);
				}
				while ((object)checkForUpdateEventHandler != checkForUpdateEventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				CheckForUpdateEventHandler checkForUpdateEventHandler = checkForUpdateEventHandler_0;
				CheckForUpdateEventHandler checkForUpdateEventHandler2;
				do
				{
					checkForUpdateEventHandler2 = checkForUpdateEventHandler;
					CheckForUpdateEventHandler value2 = (CheckForUpdateEventHandler)Delegate.Remove(checkForUpdateEventHandler2, value);
					checkForUpdateEventHandler = Interlocked.CompareExchange(ref checkForUpdateEventHandler_0, value2, checkForUpdateEventHandler2);
				}
				while ((object)checkForUpdateEventHandler != checkForUpdateEventHandler2);
			}
		}

		public static void Start()
		{
			Start(string_1);
		}

		public static void Start(string appCast)
		{
			string_1 = appCast;
			IsWinFormsApplication = Application.MessageLoop;
			BackgroundWorker backgroundWorker = new BackgroundWorker();
			backgroundWorker.DoWork += smethod_0;
			backgroundWorker.RunWorkerAsync();
		}

		private static void smethod_0(object object_0, object object_1)
		{
			Assembly entryAssembly = Assembly.GetEntryAssembly();
			AssemblyCompanyAttribute assemblyCompanyAttribute = (AssemblyCompanyAttribute)smethod_3(entryAssembly, typeof(AssemblyCompanyAttribute));
			AssemblyTitleAttribute assemblyTitleAttribute = (AssemblyTitleAttribute)smethod_3(entryAssembly, typeof(AssemblyTitleAttribute));
			AppTitle = ((assemblyTitleAttribute == null) ? entryAssembly.GetName().Name : assemblyTitleAttribute.Title);
			string text = ((assemblyCompanyAttribute == null) ? "" : assemblyCompanyAttribute.Company);
			RegistryLocation = (string.IsNullOrEmpty(text) ? $"Software\\{AppTitle}\\AutoUpdater" : $"Software\\{text}\\{AppTitle}\\AutoUpdater");
			RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(RegistryLocation);
			if (registryKey != null)
			{
				object value = registryKey.GetValue("remindlater");
				if (value != null)
				{
					DateTime t = Convert.ToDateTime(value.ToString(), CultureInfo.CreateSpecificCulture("en-US"));
					int num = DateTime.Compare(DateTime.Now, t);
					if (num < 0)
					{
						return;
					}
				}
			}
			InstalledVersion = entryAssembly.GetName().Version;
			WebRequest webRequest = WebRequest.Create(string_1);
			webRequest.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
			WebResponse response;
			try
			{
				response = webRequest.GetResponse();
			}
			catch (Exception)
			{
				if (checkForUpdateEventHandler_0 != null)
				{
					checkForUpdateEventHandler_0(null);
				}
				return;
			}
			Stream responseStream = response.GetResponseStream();
			XmlDocument xmlDocument = new XmlDocument();
			if (responseStream != null)
			{
				xmlDocument.Load(responseStream);
				XmlNodeList xmlNodeList = xmlDocument.SelectNodes("item");
				if (xmlNodeList != null)
				{
					foreach (XmlNode item in xmlNodeList)
					{
						XmlNode xmlNode2 = item.SelectSingleNode("version");
						if (xmlNode2 == null)
						{
							continue;
						}
						string innerText = xmlNode2.InnerText;
						CurrentVersion = new Version(innerText);
						if (CurrentVersion == null)
						{
							return;
						}
						XmlNode xmlNode_ = item.SelectSingleNode("changelog");
						ChangeLogURL = smethod_1(response.ResponseUri, xmlNode_);
						XmlNode xmlNode_2 = item.SelectSingleNode("url");
						string_0 = smethod_1(response.ResponseUri, xmlNode_2);
						if (IntPtr.Size.Equals(8))
						{
							XmlNode xmlNode_3 = item.SelectSingleNode("url64");
							string value2 = smethod_1(response.ResponseUri, xmlNode_3);
							if (!string.IsNullOrEmpty(value2))
							{
								string_0 = value2;
							}
						}
					}
				}
				if (registryKey != null)
				{
					object value3 = registryKey.GetValue("skip");
					object value4 = registryKey.GetValue("version");
					if (value3 != null && value4 != null)
					{
						string text2 = value3.ToString();
						Version version = new Version(value4.ToString());
						if (text2.Equals("1") && CurrentVersion <= version && CurrentVersion > version)
						{
							RegistryKey registryKey2 = Registry.CurrentUser.CreateSubKey(RegistryLocation);
							if (registryKey2 != null)
							{
								registryKey2.SetValue("version", CurrentVersion.ToString());
								registryKey2.SetValue("skip", 0);
							}
						}
					}
					registryKey.Close();
				}
				UpdateInfoEventArgs args = new UpdateInfoEventArgs
				{
					String_0 = string_0,
					ChangelogURL = ChangeLogURL,
					CurrentVersion = CurrentVersion,
					InstalledVersion = InstalledVersion,
					IsUpdateAvailable = false
				};
				if (CurrentVersion > InstalledVersion)
				{
					DownloadUpdate();
				}
				if (checkForUpdateEventHandler_0 != null)
				{
					checkForUpdateEventHandler_0(args);
				}
			}
			else if (checkForUpdateEventHandler_0 != null)
			{
				checkForUpdateEventHandler_0(null);
			}
		}

		private static string smethod_1(Uri uri_0, XmlNode xmlNode_0)
		{
			string text = ((xmlNode_0 == null) ? "" : xmlNode_0.InnerText);
			if (!string.IsNullOrEmpty(text) && Uri.IsWellFormedUriString(text, UriKind.Relative))
			{
				Uri uri = new Uri(uri_0, text);
				if (uri.IsAbsoluteUri)
				{
					text = uri.AbsoluteUri;
				}
			}
			return text;
		}

		private static void smethod_2()
		{
		}

		private static Attribute smethod_3(Assembly assembly_0, Type type_0)
		{
			object[] customAttributes = assembly_0.GetCustomAttributes(type_0, inherit: false);
			if (customAttributes.Length == 0)
			{
				return null;
			}
			return (Attribute)customAttributes[0];
		}

		public static void DownloadUpdate()
		{
			DownloadUpdateDialog downloadUpdateDialog = new DownloadUpdateDialog(string_0);
			try
			{
				downloadUpdateDialog.ShowDialog();
			}
			catch (TargetInvocationException)
			{
			}
		}

		static AutoUpdater()
		{
			LetUserSelectRemindLater = true;
			RemindLaterAt = 2;
			RemindLaterTimeSpan = RemindLaterFormat.Days;
		}

		//internal static bool S0BsY47Umes4H1uAZUY()
		//{
		//	return cYiBkb7fQJGZJ51G1AG == null;
		//}
	}
}
