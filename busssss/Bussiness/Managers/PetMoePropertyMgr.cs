using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;
using SqlDataProvider.Data;

namespace Bussiness.Managers
{
	// Token: 0x0200003D RID: 61
	public class PetMoePropertyMgr
	{
		// Token: 0x06000329 RID: 809 RVA: 0x00039078 File Offset: 0x00037278
		public static bool ReLoad()
		{
			try
			{
				PetMoePropertyInfo[] tempPetMoeProperty = PetMoePropertyMgr.LoadPetMoePropertyDb();
				Dictionary<int, PetMoePropertyInfo> tempPetMoePropertys = PetMoePropertyMgr.LoadPetMoePropertys(tempPetMoeProperty);
				bool flag = tempPetMoeProperty.Length != 0;
				if (flag)
				{
					Interlocked.Exchange<Dictionary<int, PetMoePropertyInfo>>(ref PetMoePropertyMgr.m_PetMoePropertys, tempPetMoePropertys);
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = PetMoePropertyMgr.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					PetMoePropertyMgr.log.Error("ReLoad PetMoeProperty", e);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600032A RID: 810 RVA: 0x000390F0 File Offset: 0x000372F0
		public static bool Init()
		{
			return PetMoePropertyMgr.ReLoad();
		}

		// Token: 0x0600032B RID: 811 RVA: 0x00039108 File Offset: 0x00037308
		public static PetMoePropertyInfo[] LoadPetMoePropertyDb()
		{
			PetMoePropertyInfo[] result;
			using (ProduceBussiness pb = new ProduceBussiness())
			{
				PetMoePropertyInfo[] infos = pb.GetAllPetMoeProperty();
				result = infos;
			}
			return result;
		}

		// Token: 0x0600032C RID: 812 RVA: 0x00039144 File Offset: 0x00037344
		public static Dictionary<int, PetMoePropertyInfo> LoadPetMoePropertys(PetMoePropertyInfo[] PetMoeProperty)
		{
			Dictionary<int, PetMoePropertyInfo> infos = new Dictionary<int, PetMoePropertyInfo>();
			foreach (PetMoePropertyInfo info in PetMoeProperty)
			{
				bool flag = !infos.Keys.Contains(info.Level);
				if (flag)
				{
					infos.Add(info.Level, info);
				}
			}
			return infos;
		}

		// Token: 0x0600032D RID: 813 RVA: 0x000391A0 File Offset: 0x000373A0
		public static PetMoePropertyInfo FindPetMoeProperty(int Level)
		{
			PetMoePropertyMgr.m_clientLocker.AcquireWriterLock(-1);
			try
			{
				bool flag = PetMoePropertyMgr.m_PetMoePropertys.ContainsKey(Level);
				if (flag)
				{
					return PetMoePropertyMgr.m_PetMoePropertys[Level];
				}
			}
			finally
			{
				PetMoePropertyMgr.m_clientLocker.ReleaseWriterLock();
			}
			return null;
		}

		// Token: 0x0600032E RID: 814 RVA: 0x00039204 File Offset: 0x00037404
		public static int FindMaxLevel()
		{
			return PetMoePropertyMgr.m_PetMoePropertys.Count;
		}

		// Token: 0x0600032F RID: 815 RVA: 0x00039220 File Offset: 0x00037420
		public static PetMoePropertyInfo FindPetMoePropertyByGp(int exp)
		{
			PetMoePropertyInfo maxInfo = PetMoePropertyMgr.FindPetMoeProperty(PetMoePropertyMgr.FindMaxLevel());
			bool flag = maxInfo != null && exp >= maxInfo.Exp;
			PetMoePropertyInfo result;
			if (flag)
			{
				result = maxInfo;
			}
			else
			{
				for (int i = 1; i <= PetMoePropertyMgr.m_PetMoePropertys.Count; i++)
				{
					bool flag2 = PetMoePropertyMgr.m_PetMoePropertys.ContainsKey(i) && exp < PetMoePropertyMgr.m_PetMoePropertys[i].Exp;
					if (flag2)
					{
						return (i == 1) ? null : PetMoePropertyMgr.m_PetMoePropertys[i - 1];
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06000330 RID: 816 RVA: 0x000392B8 File Offset: 0x000374B8
		public static PetMoePropertyInfo FindPetMoeExpInfo(int level)
		{
			bool flag = PetMoePropertyMgr.m_PetMoePropertys.ContainsKey(level);
			PetMoePropertyInfo result;
			if (flag)
			{
				result = PetMoePropertyMgr.m_PetMoePropertys[level];
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000331 RID: 817 RVA: 0x000392E8 File Offset: 0x000374E8
		public static int getNeedExp(int Exp, int level)
		{
			PetMoePropertyInfo temp = PetMoePropertyMgr.FindPetMoeExpInfo(level + 1);
			bool flag = temp == null;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = temp.Exp - Exp;
			}
			return result;
		}

		// Token: 0x04000169 RID: 361
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Token: 0x0400016A RID: 362
		private static Dictionary<int, PetMoePropertyInfo> m_PetMoePropertys = new Dictionary<int, PetMoePropertyInfo>();

		// Token: 0x0400016B RID: 363
		private static Random random = new Random();

		// Token: 0x0400016C RID: 364
		private static ReaderWriterLock m_clientLocker = new ReaderWriterLock();
	}
}
