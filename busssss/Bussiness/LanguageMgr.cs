using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using log4net;

namespace Bussiness
{
	// Token: 0x02000012 RID: 18
	public class LanguageMgr
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000096 RID: 150 RVA: 0x0000CC90 File Offset: 0x0000AE90
		private static string LanguageFile
		{
			get
			{
				return ConfigurationManager.AppSettings["LanguagePath"];
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x0000CCB4 File Offset: 0x0000AEB4
		public static bool Setup(string path)
		{
			return LanguageMgr.Reload(path);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x0000CCCC File Offset: 0x0000AECC
		public static bool Reload(string path)
		{
			try
			{
				Hashtable temp = LanguageMgr.LoadLanguage(path);
				bool flag = temp.Count > 0;
				if (flag)
				{
					Interlocked.Exchange<Hashtable>(ref LanguageMgr.LangsSentences, temp);
					return true;
				}
			}
			catch (Exception ex)
			{
				LanguageMgr.log.Error("Load language file error:", ex);
			}
			return false;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x0000CD30 File Offset: 0x0000AF30
		private static Hashtable LoadLanguage(string path)
		{
			Hashtable list = new Hashtable();
			string filePath = path + LanguageMgr.LanguageFile;
			bool flag = !File.Exists(filePath);
			if (flag)
			{
				LanguageMgr.log.Error("Language file : " + filePath + " not found !");
			}
			else
			{
				string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);
				IList textList = new ArrayList(lines);
				foreach (object obj in textList)
				{
					string line = (string)obj;
					bool flag2 = line.StartsWith("#");
					if (!flag2)
					{
						bool flag3 = line.IndexOf(':') == -1;
						if (!flag3)
						{
							string[] splitted = new string[]
							{
								line.Substring(0, line.IndexOf(':')),
								line.Substring(line.IndexOf(':') + 1)
							};
							splitted[1] = splitted[1].Replace("\t", "");
							list[splitted[0]] = splitted[1];
						}
					}
				}
			}
			return list;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x0000CE70 File Offset: 0x0000B070
		public static string GetTranslation(string translateId, params object[] args)
		{
			bool flag = LanguageMgr.LangsSentences.ContainsKey(translateId);
			string result;
			if (flag)
			{
				string translated = (string)LanguageMgr.LangsSentences[translateId];
				try
				{
					translated = string.Format(translated, args);
				}
				catch (Exception ex)
				{
					LanguageMgr.log.Error(string.Concat(new string[]
					{
						"Parameters number error, ID: ",
						translateId,
						" (Arg count=",
						args.Length.ToString(),
						")"
					}), ex);
				}
				result = ((translated == null) ? translateId : translated);
			}
			else
			{
				result = translateId;
			}
			return result;
		}

		// Token: 0x040000EB RID: 235
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Token: 0x040000EC RID: 236
		private static Hashtable LangsSentences = new Hashtable();
	}
}
