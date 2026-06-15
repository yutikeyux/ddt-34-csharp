using System;
using System.Collections.Generic;

namespace Bussiness
{
	// Token: 0x0200001E RID: 30
	public class ThreadSafeRandom
	{
		// Token: 0x0600025E RID: 606 RVA: 0x00033650 File Offset: 0x00031850
		public int Next()
		{
			Random obj = this.random;
			int result;
			lock (obj)
			{
				result = this.random.Next();
			}
			return result;
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0003369C File Offset: 0x0003189C
		public int Next(int maxValue)
		{
			Random obj = this.random;
			int result;
			lock (obj)
			{
				result = this.random.Next(maxValue);
			}
			return result;
		}

		// Token: 0x06000260 RID: 608 RVA: 0x000336E8 File Offset: 0x000318E8
		public int Next(int minValue, int maxValue)
		{
			Random obj = this.random;
			int result;
			lock (obj)
			{
				result = this.random.Next(minValue, maxValue);
			}
			return result;
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00033738 File Offset: 0x00031938
		public static int NextStatic()
		{
			Random obj = ThreadSafeRandom.randomStatic;
			int result;
			lock (obj)
			{
				result = ThreadSafeRandom.randomStatic.Next();
			}
			return result;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00033784 File Offset: 0x00031984
		public static int NextStatic(int maxValue)
		{
			Random obj = ThreadSafeRandom.randomStatic;
			int result;
			lock (obj)
			{
				result = ThreadSafeRandom.randomStatic.Next(maxValue);
			}
			return result;
		}

		// Token: 0x06000263 RID: 611 RVA: 0x000337D0 File Offset: 0x000319D0
		public static void NextStatic(byte[] keys)
		{
			Random obj = ThreadSafeRandom.randomStatic;
			lock (obj)
			{
				ThreadSafeRandom.randomStatic.NextBytes(keys);
			}
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0003381C File Offset: 0x00031A1C
		public static int NextStatic(int minValue, int maxValue)
		{
			Random obj = ThreadSafeRandom.randomStatic;
			int result;
			lock (obj)
			{
				result = ThreadSafeRandom.randomStatic.Next(minValue, maxValue);
			}
			return result;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00033868 File Offset: 0x00031A68
		public void Shuffer<T>(T[] array)
		{
			for (int i = array.Length; i > 1; i--)
			{
				int index = this.random.Next(i);
				T local = array[index];
				array[index] = array[i - 1];
				array[i - 1] = local;
			}
		}

		// Token: 0x06000266 RID: 614 RVA: 0x000338BC File Offset: 0x00031ABC
		public void ShufferList<T>(List<T> array)
		{
			for (int i = array.Count; i > 1; i--)
			{
				int num2 = this.random.Next(i);
				T local = array[num2];
				array[num2] = array[i - 1];
				array[i - 1] = local;
			}
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00033914 File Offset: 0x00031B14
		public static void ShufferStatic<T>(T[] array)
		{
			for (int i = array.Length; i > 1; i--)
			{
				int index = ThreadSafeRandom.randomStatic.Next(i);
				T local = array[index];
				array[index] = array[i - 1];
				array[i - 1] = local;
			}
		}

		// Token: 0x040000F2 RID: 242
		private Random random = new Random();

		// Token: 0x040000F3 RID: 243
		private static Random randomStatic = new Random();
	}
}
