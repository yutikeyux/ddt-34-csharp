using System;
using System.Collections.Generic;
using System.Linq;

namespace Bussiness.Helpers
{
	// Token: 0x0200004B RID: 75
	public static class Functions
	{
		// Token: 0x060003AF RID: 943 RVA: 0x0003CFE1 File Offset: 0x0003B1E1
		public static IEnumerable<IEnumerable<T>> Split<T>(this T[] array, int size)
		{
			int i = 0;
			while ((float)i < (float)array.Length / (float)size)
			{
				yield return array.Skip(i * size).Take(size);
				int num = i;
				i = num + 1;
			}
			yield break;
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0003CFF8 File Offset: 0x0003B1F8
		public static IEnumerable<IEnumerable<T>> Split<T>(this T[] array, int size, int startIndex)
		{
			int i = 1;
			while ((float)i < (float)array.Length / (float)size)
			{
				yield return array.Skip(i * size).Take(size);
				int num = i;
				i = num + 1;
			}
			yield break;
		}
	}
}
