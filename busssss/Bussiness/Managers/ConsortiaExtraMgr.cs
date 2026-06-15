using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using log4net;
using SqlDataProvider.Data;

namespace Bussiness.Managers
{
	// Token: 0x02000032 RID: 50
	public class ConsortiaExtraMgr
	{
		// Token: 0x060002B6 RID: 694 RVA: 0x00035200 File Offset: 0x00033400
		public static bool ReLoad()
		{
			try
			{
				Dictionary<int, ConsortiaLevelInfo> tempConsortiaLevel = new Dictionary<int, ConsortiaLevelInfo>();
				Dictionary<int, ConsortiaBuffTempInfo> tempConsortiaBuffTemp = new Dictionary<int, ConsortiaBuffTempInfo>();
				Dictionary<int, ConsortiaBadgeConfigInfo> tempConsortiaBadgeConfigs = ConsortiaExtraMgr.LoadFromDatabase();
				bool flag = ConsortiaExtraMgr.Load(tempConsortiaLevel, tempConsortiaBuffTemp);
				if (flag)
				{
					ConsortiaExtraMgr.m_clientLocker.AcquireWriterLock(-1);
					try
					{
						ConsortiaExtraMgr._consortiaLevel = tempConsortiaLevel;
						ConsortiaExtraMgr._consortiaBuffTemp = tempConsortiaBuffTemp;
						bool flag2 = tempConsortiaBadgeConfigs.Values.Count > 0;
						if (flag2)
						{
							Interlocked.Exchange<Dictionary<int, ConsortiaBadgeConfigInfo>>(ref ConsortiaExtraMgr.m_consortiaBadgeConfigs, tempConsortiaBadgeConfigs);
						}
						return true;
					}
					catch
					{
					}
					finally
					{
						ConsortiaExtraMgr.m_clientLocker.ReleaseWriterLock();
					}
				}
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = ConsortiaExtraMgr.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					ConsortiaExtraMgr.log.Error("ConsortiaExtraMgr", ex);
				}
			}
			return false;
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x000352E0 File Offset: 0x000334E0
		public static bool Init()
		{
			bool result;
			try
			{
				result = ConsortiaExtraMgr.ReLoad();
			}
			catch (Exception ex)
			{
				bool isErrorEnabled = ConsortiaExtraMgr.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					ConsortiaExtraMgr.log.Error("ConsortiaExtraMgr", ex);
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x00035330 File Offset: 0x00033530
		private static Dictionary<int, ConsortiaBadgeConfigInfo> LoadFromDatabase()
		{
			Dictionary<int, ConsortiaBadgeConfigInfo> list = new Dictionary<int, ConsortiaBadgeConfigInfo>();
			using (ProduceBussiness db = new ProduceBussiness())
			{
				ConsortiaBadgeConfigInfo[] consortiaBadgeConfigInfos = db.GetAllConsortiaBadgeConfig();
				ConsortiaBadgeConfigInfo[] array = consortiaBadgeConfigInfos;
				foreach (ConsortiaBadgeConfigInfo info in array)
				{
					bool flag = !list.ContainsKey(info.BadgeID);
					if (flag)
					{
						list.Add(info.BadgeID, info);
					}
				}
			}
			return list;
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x000353C4 File Offset: 0x000335C4
		private static bool Load(Dictionary<int, ConsortiaLevelInfo> consortiaLevel, Dictionary<int, ConsortiaBuffTempInfo> consortiaBuffTemp)
		{
			using (ProduceBussiness db = new ProduceBussiness())
			{
				ConsortiaLevelInfo[] infos = db.GetAllConsortiaLevel();
				ConsortiaLevelInfo[] array = infos;
				foreach (ConsortiaLevelInfo info in array)
				{
					bool flag = !consortiaLevel.ContainsKey(info.Level);
					if (flag)
					{
						consortiaLevel.Add(info.Level, info);
					}
				}
				ConsortiaBuffTempInfo[] buffInfos = db.GetAllConsortiaBuffTemp();
				ConsortiaBuffTempInfo[] array2 = buffInfos;
				foreach (ConsortiaBuffTempInfo info2 in array2)
				{
					bool flag2 = !consortiaBuffTemp.ContainsKey(info2.id);
					if (flag2)
					{
						consortiaBuffTemp.Add(info2.id, info2);
					}
				}
			}
			return true;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x000354A4 File Offset: 0x000336A4
		public static ConsortiaBadgeConfigInfo FindConsortiaBadgeConfig(int level)
		{
			bool flag = ConsortiaExtraMgr.m_consortiaBadgeConfigs.ContainsKey(level);
			ConsortiaBadgeConfigInfo result;
			if (flag)
			{
				result = ConsortiaExtraMgr.m_consortiaBadgeConfigs[level];
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060002BB RID: 699 RVA: 0x000354D8 File Offset: 0x000336D8
		public static ConsortiaBossConfigInfo FindConsortiaBossInfo(int id)
		{
			ConsortiaExtraMgr.m_clientLocker.AcquireReaderLock(-1);
			try
			{
				bool flag = ConsortiaExtraMgr._consortiaBossConfig.ContainsKey(id);
				if (flag)
				{
					return ConsortiaExtraMgr._consortiaBossConfig[id];
				}
			}
			catch
			{
			}
			finally
			{
				ConsortiaExtraMgr.m_clientLocker.ReleaseReaderLock();
			}
			return null;
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0003554C File Offset: 0x0003374C
		public static ConsortiaLevelInfo FindConsortiaLevelInfo(int level)
		{
			ConsortiaExtraMgr.m_clientLocker.AcquireReaderLock(-1);
			try
			{
				bool flag = ConsortiaExtraMgr._consortiaLevel.ContainsKey(level);
				if (flag)
				{
					return ConsortiaExtraMgr._consortiaLevel[level];
				}
			}
			catch
			{
			}
			finally
			{
				ConsortiaExtraMgr.m_clientLocker.ReleaseReaderLock();
			}
			return null;
		}

		// Token: 0x060002BD RID: 701 RVA: 0x000355C0 File Offset: 0x000337C0
		public static ConsortiaBuffTempInfo FindConsortiaBuffInfo(int id)
		{
			ConsortiaExtraMgr.m_clientLocker.AcquireReaderLock(-1);
			try
			{
				bool flag = ConsortiaExtraMgr._consortiaBuffTemp.ContainsKey(id);
				if (flag)
				{
					return ConsortiaExtraMgr._consortiaBuffTemp[id];
				}
			}
			catch
			{
			}
			finally
			{
				ConsortiaExtraMgr.m_clientLocker.ReleaseReaderLock();
			}
			return null;
		}

		// Token: 0x060002BE RID: 702 RVA: 0x00035634 File Offset: 0x00033834
		public static List<ConsortiaBuffTempInfo> GetAllConsortiaBuff()
		{
			ConsortiaExtraMgr.m_clientLocker.AcquireReaderLock(-1);
			List<ConsortiaBuffTempInfo> list = new List<ConsortiaBuffTempInfo>();
			List<ConsortiaBuffTempInfo> result;
			try
			{
				foreach (ConsortiaBuffTempInfo buff in ConsortiaExtraMgr._consortiaBuffTemp.Values)
				{
					list.Add(buff);
				}
				result = list;
			}
			catch
			{
				result = list;
			}
			finally
			{
				ConsortiaExtraMgr.m_clientLocker.ReleaseReaderLock();
			}
			return result;
		}

		// Token: 0x060002BF RID: 703 RVA: 0x000356D8 File Offset: 0x000338D8
		public static List<ConsortiaBuffTempInfo> GetAllConsortiaBuff(int level, int type)
		{
			ConsortiaExtraMgr.m_clientLocker.AcquireReaderLock(-1);
			List<ConsortiaBuffTempInfo> list = new List<ConsortiaBuffTempInfo>();
			List<ConsortiaBuffTempInfo> result;
			try
			{
				foreach (ConsortiaBuffTempInfo buff in ConsortiaExtraMgr._consortiaBuffTemp.Values)
				{
					bool flag = buff.level == level && buff.type == type;
					if (flag)
					{
						list.Add(buff);
					}
				}
				result = list;
			}
			catch
			{
				result = list;
			}
			finally
			{
				ConsortiaExtraMgr.m_clientLocker.ReleaseReaderLock();
			}
			return result;
		}

		// Token: 0x0400013E RID: 318
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Token: 0x0400013F RID: 319
		private static Dictionary<int, ConsortiaLevelInfo> _consortiaLevel;

		// Token: 0x04000140 RID: 320
		private static Dictionary<int, ConsortiaBuffTempInfo> _consortiaBuffTemp;

        // Token: 0x04000141 RID: 321
#pragma warning disable IDE0044 // Add readonly modifier
        private static Dictionary<int, ConsortiaBossConfigInfo> _consortiaBossConfig;
#pragma warning restore IDE0044 // Add readonly modifier

        // Token: 0x04000142 RID: 322
        private static Dictionary<int, ConsortiaBadgeConfigInfo> m_consortiaBadgeConfigs = new Dictionary<int, ConsortiaBadgeConfigInfo>();

		// Token: 0x04000143 RID: 323
		private static ReaderWriterLock m_clientLocker = new ReaderWriterLock();

        public static Dictionary<int, ConsortiaBossConfigInfo> ConsortiaBossConfig { get => _consortiaBossConfig; set => _consortiaBossConfig = value; }
    }
}
