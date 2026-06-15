using System;
using System.Collections.Generic;
using System.Linq;

namespace Bussiness.Helpers
{
	// Token: 0x0200004A RID: 74
	public static class DictionaryHelper
	{
		// Token: 0x060003AE RID: 942 RVA: 0x0003CFD1 File Offset: 0x0003B1D1
		public static IEnumerable<TValue> RandomValues<TKey, TValue>(this IDictionary<TKey, TValue> dict)
		{
			Random rand = new Random();
			List<TValue> values = dict.Values.ToList<TValue>();
			int size = dict.Count;
			for (;;)
			{
				yield return values[rand.Next(size)];
			}
#pragma warning disable CS0162 // Unreachable code detected
            yield break;
#pragma warning restore CS0162 // Unreachable code detected
        }
	}
}
