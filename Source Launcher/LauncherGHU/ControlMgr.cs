using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Deployment.Application;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ns0;

namespace LauncherGHU
{
	public class ControlMgr
	{
		public static Dictionary<int, ServerInfo> ServerList;

		public static string[] definedPrograms;

		private static Thread thread_0;

		private static ControlMgr Gg4c2obmyclr0nw3jOp;

		public static string CurrentVersion => (!ApplicationDeployment.IsNetworkDeployed) ? Assembly.GetExecutingAssembly().GetName().Version.ToString() : ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();

		public static void stopCheats(object param1 = null)
		{
			try
			{
				Process[] processes = Process.GetProcesses();
				Process[] array = processes;
				foreach (Process process in array)
				{
					if (!process.ProcessName.StartsWith("cheat"))
					{
						if (!process.ProcessName.StartsWith("Cheat"))
						{
							if (!process.ProcessName.StartsWith("pack"))
							{
								if (!process.ProcessName.StartsWith("Pack"))
								{
									if (!process.ProcessName.StartsWith("ida"))
									{
										if (process.ProcessName.StartsWith("IDA"))
										{
											Process[] processesByName = Process.GetProcessesByName("Neuz");
											Process[] array2 = processesByName;
											foreach (Process process2 in array2)
											{
												process2.Kill();
											}
											process.Kill();
											MessageBox.Show("Phát hiện PC sử dụng phần mềm cheat :" + process.ProcessName + " \n Hệ thống tự động tắt phiên đăng nhập!");
											Application.Exit();
											break;
										}
										continue;
									}
									Process[] processesByName2 = Process.GetProcessesByName("Neuz");
									Process[] array3 = processesByName2;
									foreach (Process process3 in array3)
									{
										process3.Kill();
									}
									process.Kill();
									MessageBox.Show("Phát hiện PC sử dụng phần mềm cheat :" + process.ProcessName + " \n Hệ thống tự động tắt phiên đăng nhập!");
									Application.Exit();
									break;
								}
								Process[] processesByName3 = Process.GetProcessesByName("Neuz");
								Process[] array4 = processesByName3;
								foreach (Process process4 in array4)
								{
									process4.Kill();
								}
								process.Kill();
								MessageBox.Show("Phát hiện PC sử dụng phần mềm cheat :" + process.ProcessName + " \n Hệ thống tự động tắt phiên đăng nhập!");
								Application.Exit();
								break;
							}
							Process[] processesByName4 = Process.GetProcessesByName("Neuz");
							Process[] array5 = processesByName4;
							foreach (Process process5 in array5)
							{
								process5.Kill();
							}
							process.Kill();
							MessageBox.Show("Phát hiện PC sử dụng phần mềm cheat :" + process.ProcessName + " \n Hệ thống tự động tắt phiên đăng nhập!");
							Application.Exit();
							break;
						}
						Process[] processesByName5 = Process.GetProcessesByName("Neuz");
						Process[] array6 = processesByName5;
						foreach (Process process6 in array6)
						{
							process6.Kill();
						}
						process.Kill();
						MessageBox.Show("Phát hiện PC sử dụng phần mềm cheat :" + process.ProcessName + " \n Hệ thống tự động tắt phiên đăng nhập!");
						Application.Exit();
						break;
					}
					Process[] processesByName6 = Process.GetProcessesByName("Neuz");
					Process[] array7 = processesByName6;
					foreach (Process process7 in array7)
					{
						process7.Kill();
					}
					process.Kill();
					MessageBox.Show("Phát hiện PC sử dụng phần mềm cheat :" + process.ProcessName + " \n Hệ thống tự động tắt phiên đăng nhập!");
					Application.Exit();
					break;
				}
			}
			catch (Exception)
			{
			}
		}

		public static string smethod_0(string input)
		{
			using MD5 mD = MD5.Create();
			byte[] bytes = Encoding.ASCII.GetBytes(input);
			byte[] array = mD.ComputeHash(bytes);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("X2"));
			}
			return stringBuilder.ToString().ToLower();
		}

		public static void CreateFileIfNotFound(string name, string path, string extension)
		{
			string path2 = path + "\\" + name + "." + extension;
			if (!File.Exists(path2))
			{
				Stream stream = Assembly.GetCallingAssembly().GetManifestResourceStream(name + "_" + extension) ?? Assembly.GetCallingAssembly().GetManifestResourceStream("LauncherGHU.cedrus." + name + "." + extension);
				Stream stream2 = File.OpenWrite(path2);
				File.SetAttributes(path2, File.GetAttributes(path2) | FileAttributes.Hidden);
				byte[] buffer = new byte[stream.Length];
				stream.Read(buffer, 0, (int)stream.Length);
				stream2.Write(buffer, 0, (int)stream.Length);
				stream2.Close();
			}
		}

		public static void CreateShortcut(string targetPath, string shortcutFile, string description, string arguments, string hotKey, string workingDirectory, string iconLocation)
		{
			if (string.IsNullOrEmpty(targetPath))
			{
				throw new ArgumentNullException("targetPath");
			}
			if (!string.IsNullOrEmpty(shortcutFile))
			{
				WshShell wshShell = (WshShell)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")));
				IWshShortcut wshShortcut = (IWshShortcut)(dynamic)wshShell.CreateShortcut(shortcutFile);
				wshShortcut.TargetPath = targetPath;
				wshShortcut.Description = description;
				if (!string.IsNullOrEmpty(arguments))
				{
					wshShortcut.Arguments = arguments;
				}
				if (!string.IsNullOrEmpty(hotKey))
				{
					wshShortcut.Hotkey = hotKey;
				}
				if (!string.IsNullOrEmpty(workingDirectory))
				{
					wshShortcut.WorkingDirectory = workingDirectory;
				}
				if (!string.IsNullOrEmpty(iconLocation))
				{
					wshShortcut.IconLocation = iconLocation;
				}
				wshShortcut.Save();
				return;
			}
			throw new ArgumentNullException("shortcutFile");
		}

		public static string xoabug(string A)
		{
			A.Trim();
			A.Replace("'", "");
			A.Replace(".", "");
			A.Replace("/", "");
			A.Replace("\\", "");
			A.Replace(" ", "");
			A.Replace("=", "");
			A.Replace(">", "");
			A.Replace("<", "");
			A.Replace("!", "");
			return A;
		}

		public static void CreateShortcut(string shortcutName, string shortcutPath, string targetFileLocation)
		{
			string pathLink = Path.Combine(shortcutPath, shortcutName + ".lnk");
			WshShell wshShell = (WshShell)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")));
			IWshShortcut wshShortcut = (IWshShortcut)(dynamic)wshShell.CreateShortcut(pathLink);
			_ = Assembly.GetExecutingAssembly().Location;
			wshShortcut.Description = ApplicationConfig.ServerNameSpace;
			wshShortcut.Hotkey = "Ctrl+M";
			wshShortcut.TargetPath = targetFileLocation;
			wshShortcut.Save();
		}

		public static string[] getFlashConfigs()
		{
			try
			{
				string address = ApplicationConfig.UrlApi + "/launchernew.php";
				string text = Encoding.UTF8.GetString(new WebClient().UploadValues(address, new NameValueCollection
				{
					["user"] = LoginMgr.Username,
					["key"] = LoginMgr.Password,
					["sid"] = LoginMgr.ServerID.ToString()
				})).Replace("\r\n   ", string.Empty);
				return text.Split('|');
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static void UpLogLauncher(string content)
		{
			try
			{
				string address = ApplicationConfig.UrlApi + "uploglauncher.php";
				Encoding.UTF8.GetString(new WebClient
				{
					Encoding = Encoding.UTF8
				}.UploadValues(address, new NameValueCollection
				{
					["user"] = LoginMgr.Username,
					["content"] = content
				})).Replace("\r\n   ", string.Empty);
			}
			catch (Exception)
			{
			}
		}

		public static void OpenWebsite(string url)
		{
			Process process = new Process();
			try
			{
				process.StartInfo.UseShellExecute = true;
				process.StartInfo.FileName = url;
				process.Start();
			}
			catch (Exception)
			{
			}
		}

		public static string GetCoin()
		{
			try
			{
				string address = string.Format(ApplicationConfig.UrlApi + "/getcoin.php");
				return Encoding.UTF8.GetString(new WebClient
				{
					Encoding = Encoding.UTF8
				}.UploadValues(address, new NameValueCollection
				{
					["user"] = LoginMgr.Username,
					["key"] = LoginMgr.Password,
					["sid"] = "1001"
				})).Replace("\r\n   ", string.Empty);
			}
			catch (Exception ex)
			{
				UpLogLauncher(ex.Message);
				return "";
			}
		}

		public static void CreateToolTip(Control target, string value)
		{
			CreateToolTip(target, value, 0, 0);
		}

		public static void CreateToolTip(Control target, string value, int delayInitial, int delayReshow)
		{
			ToolTip toolTip = new ToolTip();
			toolTip.AutoPopDelay = 5000;
			toolTip.InitialDelay = delayInitial;
			toolTip.ReshowDelay = delayReshow;
			toolTip.ShowAlways = true;
			toolTip.SetToolTip(target, value);
		}

		public static string GetWebForm(string url)
		{
			Uri uri = new Uri(url);
			try
			{
				using WebClient webClient = new WebClient();
				webClient.Encoding = Encoding.UTF8;
				string text = webClient.DownloadString(uri);
				webClient.Dispose();
				return text.Replace("\r\n   ", "");
			}
			catch (Exception)
			{
				UpLogLauncher("Can't Get Request From Link: " + uri);
				return "";
			}
		}

		public static string getUrlContent(string url)
		{
			Uri uri = new Uri(url);
			try
			{
				using WebClient webClient = new WebClient();
				webClient.Encoding = Encoding.UTF8;
				webClient.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.BypassCache);
				string text = webClient.DownloadString(uri);
				webClient.Dispose();
				return text.Replace("\r\n   ", "");
			}
			catch (Exception)
			{
				UpLogLauncher("Can't Get Request From Link: " + uri);
				return "";
			}
		}

		public static string GetWebForm(string url, string param)
		{
			Uri uri = new Uri(url + "?" + param);
			try
			{
				using WebClient webClient = new WebClient();
				webClient.Encoding = Encoding.UTF8;
				string text = webClient.DownloadString(uri);
				webClient.Dispose();
				return text.Replace("\r\n   ", "");
			}
			catch (Exception)
			{
				UpLogLauncher("Can't Get Request From Link: " + uri);
				return "";
			}
		}

		public static string RequestWebForm(string url)
		{
			return RequestWebForm(url, "");
		}

		public static string RequestWebForm(string url, string param)
		{
			try
			{
				Uri address = new Uri(url);
				using WebClient webClient = new WebClient();
				webClient.Encoding = Encoding.UTF8;
				webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
				string text = webClient.UploadString(address, param);
				webClient.Dispose();
				return text.Replace("\r\n   ", "");
			}
			catch (Exception)
			{
				UpLogLauncher("Can't Send Request to Link: " + url);
				return "";
			}
		}

		public static byte[] encryptData(string data)
		{
			MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
			UTF8Encoding uTF8Encoding = new UTF8Encoding();
			return mD5CryptoServiceProvider.ComputeHash(uTF8Encoding.GetBytes(data));
		}

		public static string md5(string data)
		{
			return BitConverter.ToString(encryptData(data)).Replace("-", "").ToLower();
		}

		public static void CloseAndBackForm(Form currentForm, Form backForm)
		{
			currentForm.Close();
			thread_0 = new Thread(smethod_2);
			thread_0.SetApartmentState(ApartmentState.STA);
			thread_0.Start(backForm);
		}

		public static void EmptyFolder(DirectoryInfo directoryInfo)
		{
			try
			{
				foreach (FileInfo item in directoryInfo.EnumerateFiles())
				{
					FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(directoryInfo?.ToString() + "\\" + item.Name);
					if ((versionInfo.FileVersion != CurrentVersion && versionInfo.CompanyName == "GunHoiUc") || (versionInfo.FileVersion != CurrentVersion && versionInfo.CompanyName == "Gunny Launcher") || (versionInfo.FileVersion != CurrentVersion && versionInfo.ProductName == "Gunny Launcher") || (versionInfo.FileVersion != CurrentVersion && versionInfo.CompanyName == "GunHoiUc Launcher") || (versionInfo.FileVersion != CurrentVersion && versionInfo.ProductName == "GunHoiUc Launcher"))
					{
						item.Delete();
					}
				}
			}
			catch
			{
			}
		}

		public static bool CheckServerIsOpen()
		{
			string url = ApplicationConfig.UrlApi + "/checkopensv.php";
			string webForm = GetWebForm(url);
			bool result;
			if (!(result = Convert.ToBoolean(webForm)))
			{
				MessageBox.Show("Máy chủ đang bảo trì !", "Thông báo");
			}
			return result;
		}

		private static void smethod_2(object object_0)
		{
			Form form = object_0 as Form;
			if (!form.InvokeRequired)
			{
				Application.Run(object_0 as Form);
				return;
			}
			form.Invoke((MethodInvoker)delegate
			{
				smethod_2(object_0);
			});
		}

		static ControlMgr()
		{
			ServerList = new Dictionary<int, ServerInfo>();
			definedPrograms = new string[2] { "cheatengine.exe", "otherhacktool.exe" };
		}

		internal static bool Lc7Xk6bg5l5JRwVp2UN()
		{
			return Gg4c2obmyclr0nw3jOp == null;
		}
	}
}
