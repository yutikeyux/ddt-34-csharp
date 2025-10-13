using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Road.Flash;
using zlib;

namespace Tank.Request
{
	// Token: 0x02000071 RID: 113
	public class StaticFunction
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600020A RID: 522 RVA: 0x0000FCD4 File Offset: 0x0000DED4
		public static RSACryptoServiceProvider RsaCryptor
		{
			get
			{
				string rsa = ConfigurationSettings.AppSettings["privateKey"];
				return CryptoHelper.GetRSACrypto(rsa);
			}
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000FD00 File Offset: 0x0000DF00
		public static byte[] Compress(string str)
		{
			byte[] src = Encoding.UTF8.GetBytes(str);
			return StaticFunction.Compress(src);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000FD24 File Offset: 0x0000DF24
		public static byte[] Compress(byte[] src)
		{
			return StaticFunction.Compress(src, 0, src.Length);
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000FD40 File Offset: 0x0000DF40
		public static byte[] Compress(byte[] src, int offset, int length)
		{
			MemoryStream ms = new MemoryStream();
			Stream s = new ZOutputStream(ms, 9);
			s.Write(src, offset, length);
			s.Close();
			return ms.ToArray();
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000FD78 File Offset: 0x0000DF78
		public static string Uncompress(string str)
		{
			byte[] src = Encoding.UTF8.GetBytes(str);
			return Encoding.UTF8.GetString(StaticFunction.Uncompress(src));
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000FDA8 File Offset: 0x0000DFA8
		public static byte[] Uncompress(byte[] src)
		{
			MemoryStream md = new MemoryStream();
			Stream d = new ZOutputStream(md);
			d.Write(src, 0, src.Length);
			d.Close();
			return md.ToArray();
		}
	}
}
