using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;
using SqlDataProvider.Data;

namespace Bussiness.Managers
{
	// Token: 0x0200002F RID: 47
	public static class AccumulActiveLoginMgr
	{
		// Token: 0x06000289 RID: 649 RVA: 0x00033DE0 File Offset: 0x00031FE0
		public static bool ReLoad()
		{
			try
			{
				AccumulAtiveLoginAwardInfo[] tempAccumulAtiveLoginAward = AccumulActiveLoginMgr.LoadAccumulAtiveLoginAwardDb();
				Dictionary<int, List<AccumulAtiveLoginAwardInfo>> tempAccumulAtiveLoginAwards = AccumulActiveLoginMgr.LoadAccumulAtiveLoginAwards(tempAccumulAtiveLoginAward);
				bool flag = tempAccumulAtiveLoginAward != null;
				if (flag)
				{
					Interlocked.Exchange<AccumulAtiveLoginAwardInfo[]>(ref AccumulActiveLoginMgr.m_AccumulAtiveLoginAward, tempAccumulAtiveLoginAward);
					Interlocked.Exchange<Dictionary<int, List<AccumulAtiveLoginAwardInfo>>>(ref AccumulActiveLoginMgr.m_AccumulAtiveLoginAwards, tempAccumulAtiveLoginAwards);
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = AccumulActiveLoginMgr.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					AccumulActiveLoginMgr.log.Error("ReLoad", e);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600028A RID: 650 RVA: 0x00033E64 File Offset: 0x00032064
		public static bool Init()
		{
			return AccumulActiveLoginMgr.ReLoad();
		}

		// Token: 0x0600028B RID: 651 RVA: 0x00033E7C File Offset: 0x0003207C
		public static AccumulAtiveLoginAwardInfo[] LoadAccumulAtiveLoginAwardDb()
		{
			AccumulAtiveLoginAwardInfo[] accumulAtiveLoginAwardInfos;
			using (ProduceBussiness db = new ProduceBussiness())
			{
				accumulAtiveLoginAwardInfos = db.GetAccumulAtiveLoginAwardInfos();
			}
			return accumulAtiveLoginAwardInfos;
		}

		// Token: 0x0600028C RID: 652 RVA: 0x00033EB8 File Offset: 0x000320B8
		public static Dictionary<int, List<AccumulAtiveLoginAwardInfo>> LoadAccumulAtiveLoginAwards(AccumulAtiveLoginAwardInfo[] AccumulAtiveLoginAwards)
		{
			Dictionary<int, List<AccumulAtiveLoginAwardInfo>> infos = new Dictionary<int, List<AccumulAtiveLoginAwardInfo>>();
			for (int i = 0; i < AccumulAtiveLoginAwards.Length; i++)
			{
				AccumulAtiveLoginAwardInfo info = AccumulAtiveLoginAwards[i];
				bool flag = !infos.Keys.Contains(info.Type);
				if (flag)
				{
					IEnumerable<AccumulAtiveLoginAwardInfo> temp = from s in AccumulAtiveLoginAwards
					where s.Type == info.Type
					select s;
					infos.Add(info.Type, temp.ToList<AccumulAtiveLoginAwardInfo>());
				}
			}
			return infos;
		}

		// Token: 0x0600028D RID: 653 RVA: 0x00033F44 File Offset: 0x00032144
		public static List<AccumulAtiveLoginAwardInfo> FindAccumulAtiveLoginAward(int Count)
		{
			bool flag = AccumulActiveLoginMgr.m_AccumulAtiveLoginAwards.ContainsKey(Count);
			List<AccumulAtiveLoginAwardInfo> result;
			if (flag)
			{
				result = AccumulActiveLoginMgr.m_AccumulAtiveLoginAwards[Count];
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600028E RID: 654 RVA: 0x00033F78 File Offset: 0x00032178
		public static List<ItemInfo> GetAllAccumulAtiveLoginAward(int Count)
		{
			List<AccumulAtiveLoginAwardInfo> items = AccumulActiveLoginMgr.FindAccumulAtiveLoginAward(Count);
			List<ItemInfo> infos = new List<ItemInfo>();
			bool flag = items != null;
			if (flag)
			{
				foreach (AccumulAtiveLoginAwardInfo info in items)
				{
					ItemInfo item = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(info.RewardItemID), info.RewardItemCount, 105);
					item.IsBinds = info.IsBind;
					item.ValidDate = info.RewardItemValid;
					item.StrengthenLevel = info.StrengthenLevel;
					item.AttackCompose = info.AttackCompose;
					item.DefendCompose = info.DefendCompose;
					item.AgilityCompose = info.AgilityCompose;
					item.LuckCompose = info.LuckCompose;
					infos.Add(item);
				}
			}
			return infos;
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0003407C File Offset: 0x0003227C
		public static List<ItemInfo> GetSelecedAccumulAtiveLoginAward(int ID)
		{
			List<ItemInfo> infos = new List<ItemInfo>();
			List<AccumulAtiveLoginAwardInfo> items = AccumulActiveLoginMgr.FindAccumulAtiveLoginAward(7);
			bool flag = items != null;
			if (flag)
			{
				foreach (AccumulAtiveLoginAwardInfo info in items)
				{
					bool flag2 = ID == info.ID;
					if (flag2)
					{
						ItemInfo item = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(info.RewardItemID), info.RewardItemCount, 105);
						item.IsBinds = info.IsBind;
						item.ValidDate = info.RewardItemValid;
						item.StrengthenLevel = info.StrengthenLevel;
						item.AttackCompose = info.AttackCompose;
						item.DefendCompose = info.DefendCompose;
						item.AgilityCompose = info.AgilityCompose;
						item.LuckCompose = info.LuckCompose;
						infos.Add(item);
						break;
					}
				}
			}
			return infos;
		}

		// Token: 0x0400012F RID: 303
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Token: 0x04000130 RID: 304
		private static AccumulAtiveLoginAwardInfo[] m_AccumulAtiveLoginAward;

		// Token: 0x04000131 RID: 305
		private static Dictionary<int, List<AccumulAtiveLoginAwardInfo>> m_AccumulAtiveLoginAwards = new Dictionary<int, List<AccumulAtiveLoginAwardInfo>>();

		// Token: 0x04000132 RID: 306
		private static Random random = new Random();
	}
}
