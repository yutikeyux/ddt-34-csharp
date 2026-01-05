using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Road.Flash;
using zlib;

namespace Tank.Request
{
	// Token: 0x02000075 RID: 117
	public class StaticFunction
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000210 RID: 528 RVA: 0x0000F708 File Offset: 0x0000D908
		public static RSACryptoServiceProvider RsaCryptor
		{
			get
			{
				string rsa = ConfigurationManager.AppSettings["privateKey"];
				return CryptoHelper.GetRSACrypto(rsa);
			}
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000F734 File Offset: 0x0000D934
		public static byte[] Compress(string str)
		{
			byte[] src = Encoding.UTF8.GetBytes(str);
			return StaticFunction.Compress(src);
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000F758 File Offset: 0x0000D958
		public static byte[] Compress(byte[] src)
		{
			return StaticFunction.Compress(src, 0, src.Length);
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000F774 File Offset: 0x0000D974
		public static byte[] Compress(byte[] src, int offset, int length)
		{
			MemoryStream ms = new MemoryStream();
			Stream s = new ZOutputStream(ms, 9);
			s.Write(src, offset, length);
			s.Close();
			return ms.ToArray();
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000F7AC File Offset: 0x0000D9AC
		public static string Uncompress(string str)
		{
			byte[] src = Encoding.UTF8.GetBytes(str);
			return Encoding.UTF8.GetString(StaticFunction.Uncompress(src));
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000F7DC File Offset: 0x0000D9DC
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
