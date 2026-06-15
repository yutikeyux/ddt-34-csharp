using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;
using SqlDataProvider.Data;

namespace Bussiness.Managers
{
	// Token: 0x0200003F RID: 63
	public class QuestMgr
	{
		// Token: 0x0600033A RID: 826 RVA: 0x000395B0 File Offset: 0x000377B0
		public static List<AchievementConditionInfo> GetAchievementCondiction(AchievementInfo info)
		{
			bool flag = !QuestMgr._achievementCondition.ContainsKey(info.ID);
			List<AchievementConditionInfo> result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = QuestMgr._achievementCondition[info.ID];
			}
			return result;
		}

		// Token: 0x0600033B RID: 827 RVA: 0x000395F4 File Offset: 0x000377F4
		public static List<AchievementGoodsInfo> GetAchievementGoods(AchievementInfo info)
		{
			bool flag = QuestMgr._achievementGoods.ContainsKey(info.ID);
			List<AchievementGoodsInfo> result;
			if (flag)
			{
				result = QuestMgr._achievementGoods[info.ID];
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600033C RID: 828 RVA: 0x00039634 File Offset: 0x00037834
		public static List<AchievementInfo> GetAllAchievements()
		{
			return QuestMgr._achievement.Values.ToList<AchievementInfo>();
		}

		// Token: 0x0600033D RID: 829 RVA: 0x00039658 File Offset: 0x00037858
		public static int[] GetAllBuriedQuest()
		{
			List<int> list = new List<int>();
			foreach (QuestInfo questInfo in QuestMgr.m_questinfo.Values)
			{
				bool flag = questInfo.QuestID == 10;
				if (flag)
				{
					list.Add(questInfo.ID);
				}
			}
			return list.ToArray();
		}

		// Token: 0x0600033E RID: 830 RVA: 0x000396DC File Offset: 0x000378DC
		public static List<QuestConditionInfo> GetQuestCondiction(QuestInfo info)
		{
			bool flag = QuestMgr.m_questcondiction.ContainsKey(info.ID);
			List<QuestConditionInfo> result;
			if (flag)
			{
				result = QuestMgr.m_questcondiction[info.ID];
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0003971C File Offset: 0x0003791C
		public static List<QuestAwardInfo> GetQuestGoods(QuestInfo info)
		{
			bool flag = QuestMgr.m_questgoods.ContainsKey(info.ID);
			List<QuestAwardInfo> result;
			if (flag)
			{
				result = QuestMgr.m_questgoods[info.ID];
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0003975C File Offset: 0x0003795C
		public static AchievementInfo GetSingleAchievement(int id)
		{
			bool flag = QuestMgr._achievement.ContainsKey(id);
			AchievementInfo result;
			if (flag)
			{
				result = QuestMgr._achievement[id];
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00039794 File Offset: 0x00037994
		public static QuestInfo GetSingleQuest(int id)
		{
			bool flag = !QuestMgr.m_questinfo.ContainsKey(id);
			QuestInfo result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = QuestMgr.m_questinfo[id];
			}
			return result;
		}

		// Token: 0x06000342 RID: 834 RVA: 0x000397D0 File Offset: 0x000379D0
		public static bool Init()
		{
			return QuestMgr.ReLoad();
		}

		// Token: 0x06000343 RID: 835 RVA: 0x000397E8 File Offset: 0x000379E8
		public static Dictionary<int, List<AchievementConditionInfo>> LoadAchievementCondictionDb(Dictionary<int, AchievementInfo> achs)
		{
			Dictionary<int, List<AchievementConditionInfo>> dictionary = new Dictionary<int, List<AchievementConditionInfo>>();
			Dictionary<int, List<AchievementConditionInfo>> result;
			using (ProduceBussiness produceBussiness = new ProduceBussiness())
			{
				AchievementConditionInfo[] allAchievementCondition = produceBussiness.GetALlAchievementCondition();
				using (Dictionary<int, AchievementInfo>.ValueCollection.Enumerator enumerator = achs.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						AchievementInfo ach = enumerator.Current;
						IEnumerable<AchievementConditionInfo> source = from s in allAchievementCondition
						where s.AchievementID == ach.ID
						select s;
						dictionary.Add(ach.ID, source.ToList<AchievementConditionInfo>());
					}
				}
				result = dictionary;
			}
			return result;
		}

		// Token: 0x06000344 RID: 836 RVA: 0x000398B0 File Offset: 0x00037AB0
		public static Dictionary<int, List<AchievementGoodsInfo>> LoadAchievementGoodDb(Dictionary<int, AchievementInfo> achs)
		{
			Dictionary<int, List<AchievementGoodsInfo>> dictionary = new Dictionary<int, List<AchievementGoodsInfo>>();
			Dictionary<int, List<AchievementGoodsInfo>> result;
			using (ProduceBussiness produceBussiness = new ProduceBussiness())
			{
				AchievementGoodsInfo[] allAchievementGoods = produceBussiness.GetAllAchievementGoods();
				using (Dictionary<int, AchievementInfo>.ValueCollection.Enumerator enumerator = achs.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						AchievementInfo ach = enumerator.Current;
						IEnumerable<AchievementGoodsInfo> source = from s in allAchievementGoods
						where s.AchievementID == ach.ID
						select s;
						dictionary.Add(ach.ID, source.ToList<AchievementGoodsInfo>());
					}
				}
				result = dictionary;
			}
			return result;
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00039978 File Offset: 0x00037B78
		public static Dictionary<int, AchievementInfo> LoadAchievementInfoDb()
		{
			Dictionary<int, AchievementInfo> dictionary = new Dictionary<int, AchievementInfo>();
			Dictionary<int, AchievementInfo> result;
			using (ProduceBussiness produceBussiness = new ProduceBussiness())
			{
				AchievementInfo[] allAchievement = produceBussiness.GetAllAchievement();
				foreach (AchievementInfo achievementInfo in allAchievement)
				{
					bool flag = !dictionary.ContainsKey(achievementInfo.ID);
					if (flag)
					{
						dictionary.Add(achievementInfo.ID, achievementInfo);
					}
				}
				result = dictionary;
			}
			return result;
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00039A0C File Offset: 0x00037C0C
		public static Dictionary<int, List<QuestConditionInfo>> LoadQuestCondictionDb(Dictionary<int, QuestInfo> quests)
		{
			Dictionary<int, List<QuestConditionInfo>> dictionary = new Dictionary<int, List<QuestConditionInfo>>();
			Dictionary<int, List<QuestConditionInfo>> result;
			using (ProduceBussiness produceBussiness = new ProduceBussiness())
			{
				QuestConditionInfo[] allQuestCondiction = produceBussiness.GetAllQuestCondiction();
				using (Dictionary<int, QuestInfo>.ValueCollection.Enumerator enumerator = quests.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						QuestInfo quest = enumerator.Current;
						IEnumerable<QuestConditionInfo> source = from s in allQuestCondiction
						where s.QuestID == quest.ID
						select s;
						dictionary.Add(quest.ID, source.ToList<QuestConditionInfo>());
					}
				}
				result = dictionary;
			}
			return result;
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00039AD4 File Offset: 0x00037CD4
		public static Dictionary<int, List<QuestAwardInfo>> LoadQuestGoodDb(Dictionary<int, QuestInfo> quests)
		{
			Dictionary<int, List<QuestAwardInfo>> dictionary = new Dictionary<int, List<QuestAwardInfo>>();
			Dictionary<int, List<QuestAwardInfo>> result;
			using (ProduceBussiness produceBussiness = new ProduceBussiness())
			{
				QuestAwardInfo[] allQuestGoods = produceBussiness.GetAllQuestGoods();
				using (Dictionary<int, QuestInfo>.ValueCollection.Enumerator enumerator = quests.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						QuestInfo quest = enumerator.Current;
						IEnumerable<QuestAwardInfo> source = from s in allQuestGoods
						where s.QuestID == quest.ID
						select s;
						dictionary.Add(quest.ID, source.ToList<QuestAwardInfo>());
					}
				}
				result = dictionary;
			}
			return result;
		}

		// Token: 0x06000348 RID: 840 RVA: 0x00039B9C File Offset: 0x00037D9C
		public static Dictionary<int, QuestInfo> LoadQuestInfoDb()
		{
			Dictionary<int, QuestInfo> dictionary = new Dictionary<int, QuestInfo>();
			Dictionary<int, QuestInfo> result;
			using (ProduceBussiness produceBussiness = new ProduceBussiness())
			{
				QuestInfo[] allQuest = produceBussiness.GetALlQuest();
				foreach (QuestInfo questInfo in allQuest)
				{
					bool flag = !dictionary.ContainsKey(questInfo.ID);
					if (flag)
					{
						dictionary.Add(questInfo.ID, questInfo);
					}
				}
				result = dictionary;
			}
			return result;
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00039C30 File Offset: 0x00037E30
		public static bool ReLoad()
		{
			try
			{
				Dictionary<int, QuestInfo> quests = QuestMgr.LoadQuestInfoDb();
				Dictionary<int, List<QuestConditionInfo>> questscon = QuestMgr.LoadQuestCondictionDb(quests);
				Dictionary<int, List<QuestAwardInfo>> questsgood = QuestMgr.LoadQuestGoodDb(quests);
				Dictionary<int, AchievementInfo> achs = QuestMgr.LoadAchievementInfoDb();
				Dictionary<int, List<AchievementConditionInfo>> achscon = QuestMgr.LoadAchievementCondictionDb(achs);
				Dictionary<int, List<AchievementGoodsInfo>> achsgood = QuestMgr.LoadAchievementGoodDb(achs);
				bool flag = quests.Count > 0;
				if (flag)
				{
					Interlocked.Exchange<Dictionary<int, QuestInfo>>(ref QuestMgr.m_questinfo, quests);
					Interlocked.Exchange<Dictionary<int, List<QuestConditionInfo>>>(ref QuestMgr.m_questcondiction, questscon);
					Interlocked.Exchange<Dictionary<int, List<QuestAwardInfo>>>(ref QuestMgr.m_questgoods, questsgood);
				}
				bool flag2 = achs.Count > 0;
				if (flag2)
				{
					Interlocked.Exchange<Dictionary<int, List<AchievementConditionInfo>>>(ref QuestMgr._achievementCondition, achscon);
					Interlocked.Exchange<Dictionary<int, AchievementInfo>>(ref QuestMgr._achievement, achs);
					Interlocked.Exchange<Dictionary<int, List<AchievementGoodsInfo>>>(ref QuestMgr._achievementGoods, achsgood);
				}
				return true;
			}
			catch (Exception ex)
			{
				QuestMgr.log.Error("QuestMgr", ex);
			}
			return false;
		}

		// Token: 0x04000171 RID: 369
		private static Dictionary<int, AchievementInfo> _achievement = new Dictionary<int, AchievementInfo>();

		// Token: 0x04000172 RID: 370
		private static Dictionary<int, List<AchievementConditionInfo>> _achievementCondition = new Dictionary<int, List<AchievementConditionInfo>>();

		// Token: 0x04000173 RID: 371
		private static Dictionary<int, List<AchievementGoodsInfo>> _achievementGoods = new Dictionary<int, List<AchievementGoodsInfo>>();

		// Token: 0x04000174 RID: 372
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Token: 0x04000175 RID: 373
		private static Dictionary<int, List<QuestConditionInfo>> m_questcondiction = new Dictionary<int, List<QuestConditionInfo>>();

		// Token: 0x04000176 RID: 374
		private static Dictionary<int, List<QuestAwardInfo>> m_questgoods = new Dictionary<int, List<QuestAwardInfo>>();

		// Token: 0x04000177 RID: 375
		private static Dictionary<int, QuestInfo> m_questinfo = new Dictionary<int, QuestInfo>();
	}
}
