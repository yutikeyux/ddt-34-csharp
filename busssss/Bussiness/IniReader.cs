using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Bussiness
{
	// Token: 0x02000010 RID: 16
	public class IniReader
	{
		// Token: 0x0600008B RID: 139 RVA: 0x0000C2FA File Offset: 0x0000A4FA
		public IniReader(string _FilePath)
		{
			this.FilePath = _FilePath;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x0000C30C File Offset: 0x0000A50C
		public string GetIniString(string Section, string Key)
		{
			StringBuilder retVal = new StringBuilder(2550);
			IniReader.GetPrivateProfileString(Section, Key, "", retVal, 2550, this.FilePath);
			return retVal.ToString();
		}

		// Token: 0x0600008D RID: 141
		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

		// Token: 0x040000EA RID: 234
		private string FilePath;
	}
}
