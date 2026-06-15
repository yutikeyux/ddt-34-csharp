using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Bussiness;
using log4net;
using SqlDataProvider.Data;

namespace Game.Server.Managers
{
	// Token: 0x02000002 RID: 2
	public class ConsortiaLevelMgr
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static bool ReLoad()
		{
			try
			{
				Dictionary<int, ConsortiaLevelInfo> tempConsortiaLevel = new Dictionary<int, ConsortiaLevelInfo>();
				bool flag = ConsortiaLevelMgr.Load(tempConsortiaLevel);
				if (flag)
				{
					ConsortiaLevelMgr.m_lock.AcquireWriterLock(-1);
					try
					{
						ConsortiaLevelMgr._consortiaLevel = tempConsortiaLevel;
						return true;
					}
					catch
					{
					}
					finally
					{
						ConsortiaLevelMgr.m_lock.ReleaseWriterLock();
					}
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = ConsortiaLevelMgr.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					ConsortiaLevelMgr.log.Error("ConsortiaLevelMgr", e);
				}
			}
			return false;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020F8 File Offset: 0x000002F8
		public static bool Init()
		{
			bool result;
			try
			{
				ConsortiaLevelMgr.m_lock = new ReaderWriterLock();
				ConsortiaLevelMgr._consortiaLevel = new Dictionary<int, ConsortiaLevelInfo>();
				ConsortiaLevelMgr.rand = new ThreadSafeRandom();
				result = ConsortiaLevelMgr.Load(ConsortiaLevelMgr._consortiaLevel);
			}
			catch (Exception e)
			{
				bool isErrorEnabled = ConsortiaLevelMgr.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					ConsortiaLevelMgr.log.Error("ConsortiaLevelMgr", e);
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000216C File Offset: 0x0000036C
		private static bool Load(Dictionary<int, ConsortiaLevelInfo> consortiaLevel)
		{
			using (ConsortiaBussiness db = new ConsortiaBussiness())
			{
				ConsortiaLevelInfo[] array = db.GetAllConsortiaLevel();
				ConsortiaLevelInfo[] array2 = array;
				foreach (ConsortiaLevelInfo info in array2)
				{
					bool flag = !consortiaLevel.ContainsKey(info.Level);
					if (flag)
					{
						consortiaLevel.Add(info.Level, info);
					}
				}
			}
			return true;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000021F4 File Offset: 0x000003F4
		public static ConsortiaLevelInfo FindConsortiaLevelInfo(int level)
		{
			ConsortiaLevelMgr.m_lock.AcquireReaderLock(-1);
			try
			{
				bool flag = ConsortiaLevelMgr._consortiaLevel.ContainsKey(level);
				if (flag)
				{
					return ConsortiaLevelMgr._consortiaLevel[level];
				}
			}
			catch
			{
			}
			finally
			{
				ConsortiaLevelMgr.m_lock.ReleaseReaderLock();
			}
			return null;
		}

		// Token: 0x04000001 RID: 1
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Token: 0x04000002 RID: 2
		private static Dictionary<int, ConsortiaLevelInfo> _consortiaLevel;

		// Token: 0x04000003 RID: 3
		private static ReaderWriterLock m_lock;

		// Token: 0x04000004 RID: 4
		private static ThreadSafeRandom rand;
	}
}
