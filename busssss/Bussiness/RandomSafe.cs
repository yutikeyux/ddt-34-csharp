using System;
using System.Security.Cryptography;

namespace Bussiness
{
	// Token: 0x0200001A RID: 26
	public class RandomSafe : Random
	{
		// Token: 0x06000247 RID: 583 RVA: 0x000321F0 File Offset: 0x000303F0
		public override int Next(int max)
		{
			return this.Next(0, max);
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0003220C File Offset: 0x0003040C
		public override int Next(int min, int max)
		{
			int num = base.Next(1, 50);
			int num2 = max - 1;
			for (int index = 0; index < num; index++)
			{
				num2 = base.Next(min, max);
			}
			return num2;
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0003224C File Offset: 0x0003044C
		public int NextSmallValue(int min, int max)
		{
			int num = Math.Abs(this.Next(min, max) - max);
			bool flag = num > max;
			if (flag)
			{
				num = max;
			}
			else
			{
				bool flag2 = num < min;
				if (flag2)
				{
					num = min;
				}
			}
			return num;
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0003228C File Offset: 0x0003048C
		private static int smethod_0()
		{
			byte[] data = new byte[4];
			new RNGCryptoServiceProvider().GetBytes(data);
			return BitConverter.ToInt32(data, 0);
		}
	}
}
