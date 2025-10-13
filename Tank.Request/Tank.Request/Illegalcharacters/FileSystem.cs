using System;
using System.Collections;
using System.IO;
using System.Text;

namespace Tank.Request.Illegalcharacters
{
	// Token: 0x02000082 RID: 130
	public class FileSystem
	{
		// Token: 0x06000258 RID: 600 RVA: 0x00011E70 File Offset: 0x00010070
		public FileSystem(string Path, string Directory, string Type)
		{
			this.initContent(Path);
			this.initFileWatcher(Directory, Type);
		}

		// Token: 0x06000259 RID: 601 RVA: 0x00011ED0 File Offset: 0x000100D0
		private void initContent(string Path)
		{
			bool flag = File.Exists(Path);
			if (flag)
			{
				this.filePath = Path;
				StreamReader sr = new StreamReader(Path, Encoding.GetEncoding("GB2312"));
				string str = "";
				bool flag2 = this.contentList.Count > 0;
				if (flag2)
				{
					this.contentList.Clear();
				}
				while (str != null)
				{
					str = sr.ReadLine();
					bool flag3 = !string.IsNullOrEmpty(str);
					if (flag3)
					{
						this.contentList.Add(str);
					}
				}
				bool flag4 = str == null;
				if (flag4)
				{
					sr.Close();
				}
			}
		}

		// Token: 0x0600025A RID: 602 RVA: 0x00011F74 File Offset: 0x00010174
		private void initFileWatcher(string directory, string type)
		{
			bool flag = Directory.Exists(directory);
			if (flag)
			{
				this.fileDirectory = directory;
				this.fileType = type;
				this.fileWatcher.Path = directory;
				this.fileWatcher.Filter = type;
				this.fileWatcher.NotifyFilter = (NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.LastAccess);
				this.fileWatcher.EnableRaisingEvents = true;
				this.fileWatcher.Changed += this.OnChanged;
				this.fileWatcher.Renamed += FileSystem.OnRenamed;
			}
		}

		// Token: 0x0600025B RID: 603 RVA: 0x00012004 File Offset: 0x00010204
		public bool checkIllegalChar(string strRegName)
		{
			bool flag = false;
			bool flag2 = !string.IsNullOrEmpty(strRegName);
			if (flag2)
			{
				flag = this.checkChar(strRegName);
			}
			return flag;
		}

		// Token: 0x0600025C RID: 604 RVA: 0x00012030 File Offset: 0x00010230
		private bool checkChar(string strRegName)
		{
			bool flag = false;
			foreach (object obj in this.contentList)
			{
				string strLine = (string)obj;
				bool flag2 = !strLine.StartsWith("GM");
				if (flag2)
				{
					foreach (char charl in strLine)
					{
						bool flag3 = strRegName.Contains(charl.ToString()) && charl.ToString() != " ";
						if (flag3)
						{
							flag = true;
							break;
						}
					}
					bool flag4 = flag;
					if (flag4)
					{
						break;
					}
				}
				else
				{
					string[] keyword = strLine.Split(new char[]
					{
						'|'
					});
					foreach (string key in keyword)
					{
						bool flag5 = strRegName.Contains(key);
						if (flag5)
						{
							flag = true;
							break;
						}
					}
					bool flag6 = flag;
					if (flag6)
					{
						break;
					}
				}
			}
			return flag;
		}

		// Token: 0x0600025D RID: 605 RVA: 0x00012164 File Offset: 0x00010364
		private void OnChanged(object source, FileSystemEventArgs e)
		{
			this.UpdataContent();
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0001216E File Offset: 0x0001036E
		private void UpdataContent()
		{
			this.initContent(this.filePath);
		}

		// Token: 0x0600025F RID: 607 RVA: 0x000090F0 File Offset: 0x000072F0
		private static void OnRenamed(object source, RenamedEventArgs e)
		{
		}

		// Token: 0x04000091 RID: 145
		public ArrayList contentList = new ArrayList();

		// Token: 0x04000092 RID: 146
		private FileSystemWatcher fileWatcher = new FileSystemWatcher();

		// Token: 0x04000093 RID: 147
		private string filePath = string.Empty;

		// Token: 0x04000094 RID: 148
		private string fileDirectory = string.Empty;

		// Token: 0x04000095 RID: 149
		private string fileType = string.Empty;
	}
}
