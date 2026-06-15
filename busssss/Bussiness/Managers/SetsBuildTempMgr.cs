using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;
using SqlDataProvider.Data;

namespace Bussiness.Managers
{
	// Token: 0x02000040 RID: 64
	public class SetsBuildTempMgr
	{
		// Token: 0x0600034C RID: 844 RVA: 0x00039D78 File Offset: 0x00037F78
		public static bool Init()
		{
			return SetsBuildTempMgr.ReLoad();
		}

		// Token: 0x0600034D RID: 845 RVA: 0x00039D90 File Offset: 0x00037F90
		public static bool ReLoad()
		{
			try
			{
				Dictionary<int, SetsBuildTempInfo> tempSetsBuildTemps = SetsBuildTempMgr.LoadFromDatabase();
				bool flag = tempSetsBuildTemps.Values.Count > 0;
				if (flag)
				{
					Interlocked.Exchange<Dictionary<int, SetsBuildTempInfo>>(ref SetsBuildTempMgr.m_setsBuildTemps, tempSetsBuildTemps);
					return true;
				}
			}
			catch (Exception ex)
			{
				SetsBuildTempMgr.log.Error("SetsBuildTempMgr init error:", ex);
			}
			return false;
		}

		// Token: 0x0600034E RID: 846 RVA: 0x00039DF8 File Offset: 0x00037FF8
		private static Dictionary<int, SetsBuildTempInfo> LoadFromDatabase()
		{
			Dictionary<int, SetsBuildTempInfo> list = new Dictionary<int, SetsBuildTempInfo>();
			using (ProduceBussiness db = new ProduceBussiness())
			{
				SetsBuildTempInfo[] setsBuildTempInfos = db.GetAllSetsBuildTemp();
				foreach (SetsBuildTempInfo info in setsBuildTempInfos)
				{
					bool flag = !list.ContainsKey(info.Level);
					if (flag)
					{
						list.Add(info.Level, info);
					}
				}
			}
			return list;
		}

		// Token: 0x0600034F RID: 847 RVA: 0x00039E84 File Offset: 0x00038084
		public static List<SetsBuildTempInfo> GetAllSetsBuildTemp()
		{
			bool flag = SetsBuildTempMgr.m_setsBuildTemps.Count == 0;
			if (flag)
			{
				SetsBuildTempMgr.Init();
			}
			return SetsBuildTempMgr.m_setsBuildTemps.Values.ToList<SetsBuildTempInfo>();
		}

		// Token: 0x06000350 RID: 848 RVA: 0x00039EBC File Offset: 0x000380BC
		public static SetsBuildTempInfo FindSetsBuildTemp(int id)
		{
			bool flag = SetsBuildTempMgr.m_setsBuildTemps.Count == 0;
			if (flag)
			{
				SetsBuildTempMgr.Init();
			}
			bool flag2 = SetsBuildTempMgr.m_setsBuildTemps.ContainsKey(id);
			SetsBuildTempInfo result;
			if (flag2)
			{
				result = SetsBuildTempMgr.m_setsBuildTemps[id];
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000351 RID: 849 RVA: 0x00039F04 File Offset: 0x00038104
		public static int SetsBuildMax()
		{
			int maxLv = SetsBuildTempMgr.m_setsBuildTemps.Count;
			return SetsBuildTempMgr.m_setsBuildTemps[maxLv].Exp;
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00039F34 File Offset: 0x00038134
		public static SetsBuildTempInfo FindSetsBuildExp(int exp)
		{
			List<SetsBuildTempInfo> infos = SetsBuildTempMgr.GetAllSetsBuildTemp();
			SetsBuildTempInfo info = null;
			for (int i = 0; i < infos.Count; i++)
			{
				info = infos[i];
				bool flag = exp <= info.Exp;
				if (flag)
				{
					return infos[(i - 1 < 0) ? 0 : (i - 1)];
				}
			}
			return info;
		}

		// Token: 0x06000353 RID: 851 RVA: 0x00039F9C File Offset: 0x0003819C
		public static SetsBuildTempInfo FindNextSetsBuildExp(int exp)
		{
			int max = SetsBuildTempMgr.SetsBuildMax();
			List<SetsBuildTempInfo> infos = SetsBuildTempMgr.GetAllSetsBuildTemp();
			SetsBuildTempInfo info = null;
			for (int i = 0; i < infos.Count; i++)
			{
				info = infos[(i + 1 > max) ? max : (i + 1)];
				bool flag = exp < info.Exp;
				if (flag)
				{
					return info;
				}
			}
			return info;
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0003A000 File Offset: 0x00038200
		public static void GetSetsBuildProp(int exp, ref int def, ref int blood, ref int luck, ref int agi, ref int dam)
		{
			List<SetsBuildTempInfo> infos = SetsBuildTempMgr.GetAllSetsBuildTemp();
			for (int i = 0; i < infos.Count; i++)
			{
				SetsBuildTempInfo info = infos[i];
				bool flag = info != null && exp >= info.Exp;
				if (flag)
				{
					def += info.DefenceGrow;
					blood += info.BloodGrow;
					luck += info.LuckGrow;
					agi += info.AgilityGrow;
					dam += info.DamageGrow;
				}
			}
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0003A08C File Offset: 0x0003828C
		public static void GetSetsBuildProp(int exp, ref int guard)
		{
			List<SetsBuildTempInfo> infos = SetsBuildTempMgr.GetAllSetsBuildTemp();
			for (int i = 0; i < infos.Count; i++)
			{
				SetsBuildTempInfo info = infos[i];
				bool flag = info != null && exp >= info.Exp;
				if (flag)
				{
					guard += info.GuardGrow;
				}
			}
		}

		// Token: 0x04000178 RID: 376
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Token: 0x04000179 RID: 377
		private static Dictionary<int, SetsBuildTempInfo> m_setsBuildTemps = new Dictionary<int, SetsBuildTempInfo>();

		// Token: 0x0400017A RID: 378
		private static ThreadSafeRandom random = new ThreadSafeRandom();
	}
}
