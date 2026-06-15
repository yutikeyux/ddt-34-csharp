using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using log4net;
using SqlDataProvider.Data;

namespace Bussiness.Managers
{
	// Token: 0x02000042 RID: 66
	public class SubActiveMgr
	{
		// Token: 0x0600036B RID: 875 RVA: 0x0003AFA4 File Offset: 0x000391A4
		public static SubActiveConditionInfo GetSubActiveInfo(ItemInfo item)
		{
			SubActiveMgr.m_clientLocker.AcquireWriterLock(-1);
			try
			{
				foreach (List<SubActiveInfo> SubActives in SubActiveMgr.m_subActiveInfo.Values)
				{
					foreach (SubActiveInfo active in SubActives)
					{
						bool flag = active.IsValid();
						if (flag)
						{
							foreach (SubActiveConditionInfo condition in SubActiveMgr.m_subActiveConditionInfo.Values)
							{
								bool flag2 = active.OnActive(condition.ActiveID, condition.SubID);
								if (flag2)
								{
									bool flag3 = item.GoldValidDate() && condition.OnAvaible(item.Template.CategoryID, item.TemplateID) && condition.OnGold() && condition.OnStrengThen() == item.StrengthenLevel;
									if (flag3)
									{
										return condition;
									}
									bool flag4 = !condition.OnGold() && condition.OnStrengThen() == 1 && item.StrengthenLevel == 0 && condition.OnAvaible(item.Template.CategoryID, item.TemplateID);
									if (flag4)
									{
										return condition;
									}
									bool flag5 = !condition.OnGold() && condition.OnStrengThen() == item.StrengthenLevel && condition.OnAvaible(item.Template.CategoryID, item.TemplateID);
									if (flag5)
									{
										return condition;
									}
								}
							}
						}
					}
				}
			}
			finally
			{
				SubActiveMgr.m_clientLocker.ReleaseWriterLock();
			}
			return null;
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0003B1EC File Offset: 0x000393EC
		public static bool Init()
		{
			return SubActiveMgr.ReLoad();
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0003B204 File Offset: 0x00039404
		public static Dictionary<int, SubActiveConditionInfo> LoadSubActiveConditionDb(Dictionary<int, List<SubActiveInfo>> conditions)
		{
			Dictionary<int, SubActiveConditionInfo> dictionary = new Dictionary<int, SubActiveConditionInfo>();
			using (ActiveBussiness db = new ActiveBussiness())
			{
				foreach (int ActiveID in conditions.Keys)
				{
					SubActiveConditionInfo[] allSubActiveConditionInfo = db.GetAllSubActiveCondition(ActiveID);
					foreach (SubActiveConditionInfo info in allSubActiveConditionInfo)
					{
						bool flag = ActiveID == info.ActiveID && !dictionary.ContainsKey(info.ID);
						if (flag)
						{
							dictionary.Add(info.ID, info);
						}
					}
				}
			}
			return dictionary;
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0003B2E4 File Offset: 0x000394E4
		public static Dictionary<int, List<SubActiveInfo>> LoadSubActiveDb()
		{
			Dictionary<int, List<SubActiveInfo>> dictionary = new Dictionary<int, List<SubActiveInfo>>();
			using (ActiveBussiness db = new ActiveBussiness())
			{
				SubActiveInfo[] allSubActiveInfo = db.GetAllSubActive();
				foreach (SubActiveInfo info in allSubActiveInfo)
				{
					List<SubActiveInfo> list = new List<SubActiveInfo>();
					bool flag = !dictionary.ContainsKey(info.ActiveID);
					if (flag)
					{
						list.Add(info);
						dictionary.Add(info.ActiveID, list);
					}
					else
					{
						dictionary[info.ActiveID].Add(info);
					}
				}
			}
			return dictionary;
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0003B39C File Offset: 0x0003959C
		public static bool ReLoad()
		{
			try
			{
				Dictionary<int, List<SubActiveInfo>> subActives = SubActiveMgr.LoadSubActiveDb();
				Dictionary<int, SubActiveConditionInfo> subActiveCondition = SubActiveMgr.LoadSubActiveConditionDb(subActives);
				bool flag = subActives.Count > 0;
				if (flag)
				{
					Interlocked.Exchange<Dictionary<int, List<SubActiveInfo>>>(ref SubActiveMgr.m_subActiveInfo, subActives);
					Interlocked.Exchange<Dictionary<int, SubActiveConditionInfo>>(ref SubActiveMgr.m_subActiveConditionInfo, subActiveCondition);
				}
				return true;
			}
			catch (Exception exception)
			{
				SubActiveMgr.log.Error("SubActiveMgr", exception);
			}
			return false;
		}

		// Token: 0x04000180 RID: 384
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Token: 0x04000181 RID: 385
		public static Dictionary<int, SubActiveConditionInfo> m_subActiveConditionInfo = new Dictionary<int, SubActiveConditionInfo>();

		// Token: 0x04000182 RID: 386
		public static Dictionary<int, List<SubActiveInfo>> m_subActiveInfo = new Dictionary<int, List<SubActiveInfo>>();

		// Token: 0x04000183 RID: 387
		private static ReaderWriterLock m_clientLocker = new ReaderWriterLock();
	}
}
