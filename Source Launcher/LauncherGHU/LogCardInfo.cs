using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;

namespace LauncherGHU
{
	public class LogCardInfo
	{
		public static List<LogCardInfo> ListLogCards;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private string string_0;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string string_1;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private string string_2;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private string string_3;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string string_4;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string string_5;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private string string_6;

		internal static LogCardInfo RNuIo0bF5h6GQTLgIsB;

		public string ID
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

		public string CardType
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

		public string Serial
		{
			[CompilerGenerated]
			get
			{
				return string_2;
			}
			[CompilerGenerated]
			set
			{
				string_2 = value;
			}
		}

		public string Passcard
		{
			[CompilerGenerated]
			get
			{
				return string_3;
			}
			[CompilerGenerated]
			set
			{
				string_3 = value;
			}
		}

		public string Money
		{
			[CompilerGenerated]
			get
			{
				return string_4;
			}
			[CompilerGenerated]
			set
			{
				string_4 = value;
			}
		}

		public string Status
		{
			[CompilerGenerated]
			get
			{
				return string_5;
			}
			[CompilerGenerated]
			set
			{
				string_5 = value;
			}
		}

		public string Time
		{
			[CompilerGenerated]
			get
			{
				return string_6;
			}
			[CompilerGenerated]
			set
			{
				string_6 = value;
			}
		}

		public static void SetupLogListCard()
		{
			try
			{
				ListLogCards = new List<LogCardInfo>();
				string url = $"{ApplicationConfig.UrlApi}getlistlogcard.php?username={LoginMgr.Username}&password={LoginMgr.Password}";
				string[] array = ControlMgr.GetWebForm(url).Split('|');
				if (array.Length == 0)
				{
					return;
				}
				string[] array2 = array;
				foreach (string text in array2)
				{
					string[] array3 = text.Split(',');
					if (array3.Length >= 7)
					{
						LogCardInfo logCardInfo = new LogCardInfo();
						logCardInfo.ID = array3[0].Replace("\r\n ", "");
						logCardInfo.CardType = array3[1].Replace("\r\n ", "");
						logCardInfo.Serial = array3[2].Replace("\r\n ", "");
						logCardInfo.Passcard = array3[3].Replace("\r\n ", "");
						logCardInfo.Time = array3[4].Replace("\r\n ", "");
						logCardInfo.Money = array3[5].Replace("\r\n ", "");
						logCardInfo.Status = array3[6].Replace("\r\n ", "");
						ListLogCards.Add(logCardInfo);
					}
				}
			}
			catch (Exception)
			{
			}
		}

		public static Image GetImageFromPicPath(string strUrl)
		{
			using WebResponse webResponse = WebRequest.Create(strUrl).GetResponse();
			using Stream stream = webResponse.GetResponseStream();
			MemoryStream memoryStream = new MemoryStream();
			stream.CopyTo(memoryStream, 8192);
			return Image.FromStream(memoryStream);
		}

		internal static bool nu5jf5b4l9Oex5i6mVL()
		{
			return RNuIo0bF5h6GQTLgIsB == null;
		}

		internal static void I5vnWmbTkD39jDYYBta()
		{
		}
	}
}
