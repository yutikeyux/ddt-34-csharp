using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SqlDataProvider.Data;

namespace Bussiness.Managers
{
	// Token: 0x02000030 RID: 48
	public static class AchievementMgr
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000291 RID: 657 RVA: 0x000341BE File Offset: 0x000323BE
		public static Hashtable ItemRecordType
		{
			get
			{
				return AchievementMgr.m_ItemRecordTypeInfo;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000292 RID: 658 RVA: 0x000341C5 File Offset: 0x000323C5
		public static Dictionary<int, AchievementInfo> Achievement
		{
			get
			{
				return AchievementMgr.m_achievement;
			}
		}

		// Token: 0x06000293 RID: 659 RVA: 0x000341CC File Offset: 0x000323CC
		public static bool Init()
		{
			return AchievementMgr.Reload();
		}

		// Token: 0x06000294 RID: 660 RVA: 0x000341E4 File Offset: 0x000323E4
		public static bool Reload()
		{
			try
			{
				AchievementMgr.LoadItemRecordTypeInfoDB();
				Dictionary<int, AchievementInfo> tempAchievementInfo = AchievementMgr.LoadAchievementInfoDB();
				Dictionary<int, List<AchievementConditionInfo>> tempAchievementConditionInfo = AchievementMgr.LoadAchievementConditionInfoDB(tempAchievementInfo);
				Dictionary<int, List<AchievementRewardInfo>> tempAchievementRewardInfo = AchievementMgr.LoadAchievementRewardInfoDB(tempAchievementInfo);
				bool flag = tempAchievementInfo.Count > 0;
				if (flag)
				{
					Interlocked.Exchange<Dictionary<int, AchievementInfo>>(ref AchievementMgr.m_achievement, tempAchievementInfo);
					Interlocked.Exchange<Dictionary<int, List<AchievementConditionInfo>>>(ref AchievementMgr.m_achievementCondition, tempAchievementConditionInfo);
					Interlocked.Exchange<Dictionary<int, List<AchievementRewardInfo>>>(ref AchievementMgr.m_achievementReward, tempAchievementRewardInfo);
				}
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(string.Format("AchievementMgr {0}", ex));
			}
			return false;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x00034274 File Offset: 0x00032474
		public static void LoadItemRecordTypeInfoDB()
		{
			using (ProduceBussiness db = new ProduceBussiness())
			{
				AchievementConditionInfo[] array = db.GetALlAchievementCondition();
				AchievementConditionInfo[] array2 = array;
				foreach (AchievementConditionInfo info in array2)
				{
					bool flag = !AchievementMgr.m_ItemRecordTypeInfo.Contains(info.CondictionType);
					if (flag)
					{
						AchievementMgr.m_ItemRecordTypeInfo.Add(info.CondictionType, info.CondictionType);
					}
				}
			}
		}

		// Token: 0x06000296 RID: 662 RVA: 0x00034310 File Offset: 0x00032510
		public static Dictionary<int, AchievementInfo> LoadAchievementInfoDB()
		{
			Dictionary<int, AchievementInfo> list = new Dictionary<int, AchievementInfo>();
			Dictionary<int, AchievementInfo> result;
			using (ProduceBussiness db = new ProduceBussiness())
			{
				AchievementInfo[] array = db.GetALlAchievement();
				AchievementInfo[] array2 = array;
				foreach (AchievementInfo info in array2)
				{
					bool flag = !list.ContainsKey(info.ID);
					if (flag)
					{
						list.Add(info.ID, info);
					}
				}
				result = list;
			}
			return result;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x000343A0 File Offset: 0x000325A0
		public static Dictionary<int, List<AchievementConditionInfo>> LoadAchievementConditionInfoDB(Dictionary<int, AchievementInfo> achievementInfos)
		{
			Dictionary<int, List<AchievementConditionInfo>> list = new Dictionary<int, List<AchievementConditionInfo>>();
			Dictionary<int, List<AchievementConditionInfo>> result;
			using (ProduceBussiness db = new ProduceBussiness())
			{
				AchievementConditionInfo[] infos = db.GetALlAchievementCondition();
				using (Dictionary<int, AchievementInfo>.ValueCollection.Enumerator enumerator = achievementInfos.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						AchievementInfo achievementInfo = enumerator.Current;
						IEnumerable<AchievementConditionInfo> temp = from s in infos
						where s.AchievementID == achievementInfo.ID
						select s;
						list.Add(achievementInfo.ID, temp.ToList<AchievementConditionInfo>());
						bool flag = temp == null;
						if (!flag)
						{
							foreach (AchievementConditionInfo info in temp)
							{
								bool flag2 = !AchievementMgr.m_distinctCondition.Contains(info.CondictionType);
								if (flag2)
								{
									AchievementMgr.m_distinctCondition.Add(info.CondictionType, info.CondictionType);
								}
							}
						}
					}
				}
				result = list;
			}
			return result;
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00034518 File Offset: 0x00032718
		public static Dictionary<int, List<AchievementRewardInfo>> LoadAchievementRewardInfoDB(Dictionary<int, AchievementInfo> achievementInfos)
		{
			Dictionary<int, List<AchievementRewardInfo>> list = new Dictionary<int, List<AchievementRewardInfo>>();
			Dictionary<int, List<AchievementRewardInfo>> result;
			using (ProduceBussiness db = new ProduceBussiness())
			{
				AchievementRewardInfo[] infos = db.GetALlAchievementReward();
				using (Dictionary<int, AchievementInfo>.ValueCollection.Enumerator enumerator = achievementInfos.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						AchievementInfo achievementInfo = enumerator.Current;
						IEnumerable<AchievementRewardInfo> temp = from s in infos
						where s.AchievementID == achievementInfo.ID
						select s;
						list.Add(achievementInfo.ID, temp.ToList<AchievementRewardInfo>());
					}
				}
				result = list;
			}
			return result;
		}

		// Token: 0x06000299 RID: 665 RVA: 0x000345D8 File Offset: 0x000327D8
		public static AchievementInfo GetSingleAchievement(int id)
		{
			bool flag = AchievementMgr.m_achievement.ContainsKey(id);
			AchievementInfo result;
			if (flag)
			{
				result = AchievementMgr.m_achievement[id];
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0003460C File Offset: 0x0003280C
		public static List<AchievementConditionInfo> GetAchievementCondition(AchievementInfo info)
		{
			bool flag = AchievementMgr.m_achievementCondition.ContainsKey(info.ID);
			List<AchievementConditionInfo> result;
			if (flag)
			{
				result = AchievementMgr.m_achievementCondition[info.ID];
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600029B RID: 667 RVA: 0x00034648 File Offset: 0x00032848
		public static List<AchievementRewardInfo> GetAchievementReward(AchievementInfo info)
		{
			bool flag = AchievementMgr.m_achievementReward.ContainsKey(info.ID);
			List<AchievementRewardInfo> result;
			if (flag)
			{
				result = AchievementMgr.m_achievementReward[info.ID];
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x04000133 RID: 307
		private static Dictionary<int, AchievementInfo> m_achievement = new Dictionary<int, AchievementInfo>();

		// Token: 0x04000134 RID: 308
		private static Dictionary<int, List<AchievementConditionInfo>> m_achievementCondition = new Dictionary<int, List<AchievementConditionInfo>>();

		// Token: 0x04000135 RID: 309
		private static Dictionary<int, List<AchievementRewardInfo>> m_achievementReward = new Dictionary<int, List<AchievementRewardInfo>>();

		// Token: 0x04000136 RID: 310
		private static Dictionary<int, List<ItemRecordTypeInfo>> m_itemRecordType = new Dictionary<int, List<ItemRecordTypeInfo>>();

		// Token: 0x04000137 RID: 311
		private static Hashtable m_distinctCondition = new Hashtable();

		// Token: 0x04000138 RID: 312
		private static Hashtable m_ItemRecordTypeInfo = new Hashtable();
	}
}
