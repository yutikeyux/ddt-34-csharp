using System;
using System.Collections.Generic;
using System.Linq;

namespace Bussiness
{
	// Token: 0x0200000E RID: 14
	public static class EnumerableExtensions
	{
		// Token: 0x0600007B RID: 123 RVA: 0x0000BE04 File Offset: 0x0000A004
		public static T Random<T>(this IEnumerable<T> enumerable)
		{
			ThreadSafeRandom r = new ThreadSafeRandom();
			IList<T> list = (enumerable as IList<T>) ?? enumerable.ToList<T>();
			return list.ElementAt(r.Next(0, list.Count<T>()));
		}
	}
}
