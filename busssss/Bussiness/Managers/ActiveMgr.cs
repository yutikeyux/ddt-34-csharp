using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;
using SqlDataProvider.Data;

namespace Bussiness.Managers
{
	// Token: 0x02000031 RID: 49
	public class ActiveMgr
	{
		// Token: 0x0600029D RID: 669 RVA: 0x000346C4 File Offset: 0x000328C4
		public static bool Init()
		{
			return ActiveMgr.ReLoad();
		}

		// Token: 0x0600029E RID: 670 RVA: 0x000346DC File Offset: 0x000328DC
		public static bool ReLoad()
		{
			try
			{
				Dictionary<int, ActiveInfo> tempActiveInfo = ActiveMgr.LoadActiveInfoDb();
				Dictionary<int, List<ActiveConvertItemInfo>> tempActiveCondiction = ActiveMgr.LoadActiveCondictionDb(tempActiveInfo);
				Dictionary<int, List<ActiveAwardInfo>> tempActiveGoods = ActiveMgr.LoadActiveGoodDb(tempActiveInfo);
				bool flag = tempActiveInfo.Count > 0;
				if (flag)
				{
					Interlocked.Exchange<Dictionary<int, ActiveInfo>>(ref ActiveMgr.m_activeinfo, tempActiveInfo);
					Interlocked.Exchange<Dictionary<int, List<ActiveConvertItemInfo>>>(ref ActiveMgr.m_activeConvertItem, tempActiveCondiction);
					Interlocked.Exchange<Dictionary<int, List<ActiveAwardInfo>>>(ref ActiveMgr.m_activeAwards, tempActiveGoods);
				}
				ActivitySystemItemInfo[] tempActivitySystemItem = ActiveMgr.LoadActivitySystemItemDb();
				Dictionary<int, List<ActivitySystemItemInfo>> tempActivitySystemItems = ActiveMgr.LoadActivitySystemItems(tempActivitySystemItem);
				bool flag2 = tempActivitySystemItem.Length != 0;
				if (flag2)
				{
					Interlocked.Exchange<Dictionary<int, List<ActivitySystemItemInfo>>>(ref ActiveMgr.m_ActivitySystemItems, tempActivitySystemItems);
				}
				return true;
			}
			catch (Exception e)
			{
				ActiveMgr.log.Error("ActiveMgr", e);
			}
			return false;
		}

		// Token: 0x0600029F RID: 671 RVA: 0x00034790 File Offset: 0x00032990
		public static void UpdateCurrentServerActive()
		{
			using (PlayerBussiness pb = new PlayerBussiness())
			{
				pb.DeleteAllActive();
				foreach (ActiveInfo info in ActiveMgr.m_activeinfo.Values)
				{
					pb.AddActive(info);
					bool flag = ActiveMgr.m_activeAwards.ContainsKey(info.ActiveID);
					if (flag)
					{
						List<ActiveAwardInfo> awards = ActiveMgr.m_activeAwards[info.ActiveID];
						foreach (ActiveAwardInfo award in awards)
						{
							pb.AddActiveAward(award);
						}
					}
				}
			}
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x00034884 File Offset: 0x00032A84
		public static ActiveConvertItemInfo GetActiveConvertItem(int id, int templateID, int index)
		{
			bool flag = ActiveMgr.m_activeConvertItem.ContainsKey(id);
			if (flag)
			{
				List<ActiveConvertItemInfo> lists = ActiveMgr.m_activeConvertItem[id];
				foreach (ActiveConvertItemInfo info in lists)
				{
					bool flag2 = info.TemplateID == templateID && info.ItemType == ActiveMgr.GetNeedGoodsAward(index);
					if (flag2)
					{
						return info;
					}
				}
			}
			return null;
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0003491C File Offset: 0x00032B1C
		public static List<ActiveConvertItemInfo> GetActiveConvertItemAward(int id, int index)
		{
			List<ActiveConvertItemInfo> listAward = new List<ActiveConvertItemInfo>();
			bool flag = ActiveMgr.m_activeConvertItem.ContainsKey(id);
			if (flag)
			{
				List<ActiveConvertItemInfo> lists = ActiveMgr.m_activeConvertItem[id];
				foreach (ActiveConvertItemInfo info in lists)
				{
					bool flag2 = info.ItemType == ActiveMgr.GetGoodsAward(index);
					if (flag2)
					{
						listAward.Add(info);
					}
				}
			}
			return listAward;
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x000349B4 File Offset: 0x00032BB4
		public static List<ActiveConvertItemInfo> FindActiveConvertItem(int id)
		{
			bool flag = ActiveMgr.m_activeConvertItem.ContainsKey(id);
			List<ActiveConvertItemInfo> result;
			if (flag)
			{
				result = ActiveMgr.m_activeConvertItem[id];
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x000349E8 File Offset: 0x00032BE8
		public static int GetGoodsAward(int index)
		{
			int result;
			switch (index)
			{
			case 1:
				result = 3;
				break;
			case 2:
				result = 5;
				break;
			case 3:
				result = 7;
				break;
			default:
				result = 1;
				break;
			}
			return result;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00034A24 File Offset: 0x00032C24
		public static int GetNeedGoodsAward(int index)
		{
			int result;
			switch (index)
			{
			case 1:
				result = 2;
				break;
			case 2:
				result = 4;
				break;
			case 3:
				result = 6;
				break;
			default:
				result = 0;
				break;
			}
			return result;
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x00034A60 File Offset: 0x00032C60
		public static ActivitySystemItemInfo[] LoadActivitySystemItemDb()
		{
			ActivitySystemItemInfo[] result;
			using (ProduceBussiness pb = new ProduceBussiness())
			{
				ActivitySystemItemInfo[] infos = pb.GetAllActivitySystemItem();
				result = infos;
			}
			return result;
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x00034A9C File Offset: 0x00032C9C
		public static Dictionary<int, List<ActivitySystemItemInfo>> LoadActivitySystemItems(ActivitySystemItemInfo[] ActivitySystemItem)
		{
			Dictionary<int, List<ActivitySystemItemInfo>> infos = new Dictionary<int, List<ActivitySystemItemInfo>>();
			for (int i = 0; i < ActivitySystemItem.Length; i++)
			{
				ActivitySystemItemInfo info = ActivitySystemItem[i];
				bool flag = !infos.Keys.Contains(info.ActivityType);
				if (flag)
				{
					IEnumerable<ActivitySystemItemInfo> temp = from s in ActivitySystemItem
					where s.ActivityType == info.ActivityType
					select s;
					infos.Add(info.ActivityType, temp.ToList<ActivitySystemItemInfo>());
				}
			}
			return infos;
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x00034B28 File Offset: 0x00032D28
		public static List<ActivitySystemItemInfo> FindActivitySystemItem(int ActivityType)
		{
			bool flag = ActiveMgr.m_ActivitySystemItems.ContainsKey(ActivityType);
			List<ActivitySystemItemInfo> result;
			if (flag)
			{
				List<ActivitySystemItemInfo> items = new List<ActivitySystemItemInfo>();
				foreach (ActivitySystemItemInfo sysItem in ActiveMgr.m_ActivitySystemItems[ActivityType])
				{
					items.Add(sysItem);
				}
				result = items;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x00034BA8 File Offset: 0x00032DA8
		public static List<ActivitySystemItemInfo> GetActivitySystemItemByLayer(int layer)
		{
			List<ActivitySystemItemInfo> lists = new List<ActivitySystemItemInfo>();
			List<ActivitySystemItemInfo> infos = ActiveMgr.FindActivitySystemItem(8);
			foreach (ActivitySystemItemInfo info in infos)
			{
				bool flag = info.Quality == layer;
				if (flag)
				{
					lists.Add(info);
				}
			}
			return lists;
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x00034C24 File Offset: 0x00032E24
		public static List<ActivitySystemItemInfo> GetGrowthPackage(int layer)
		{
			List<ActivitySystemItemInfo> lists = new List<ActivitySystemItemInfo>();
			List<ActivitySystemItemInfo> infos = ActiveMgr.FindActivitySystemItem(20);
			foreach (ActivitySystemItemInfo info in infos)
			{
				bool flag = info.Quality == layer;
				if (flag)
				{
					lists.Add(info);
				}
			}
			return lists;
		}

		// Token: 0x060002AA RID: 682 RVA: 0x00034CA0 File Offset: 0x00032EA0
		public static List<ActivitySystemItemInfo> FindChickActivePakage(int quality)
		{
			List<ActivitySystemItemInfo> lists = new List<ActivitySystemItemInfo>();
			List<ActivitySystemItemInfo> infos = ActiveMgr.FindActivitySystemItem(40);
			foreach (ActivitySystemItemInfo info in infos)
			{
				bool flag = info.Quality == quality;
				if (flag)
				{
					lists.Add(info);
				}
			}
			return lists;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x00034D1C File Offset: 0x00032F1C
		public static List<ActivitySystemItemInfo> FindSignBuffPackage(int quality)
		{
			List<ActivitySystemItemInfo> lists = new List<ActivitySystemItemInfo>();
			List<ActivitySystemItemInfo> infos = ActiveMgr.FindActivitySystemItem(112);
			foreach (ActivitySystemItemInfo info in infos)
			{
				bool flag = info.Quality == quality;
				if (flag)
				{
					lists.Add(info);
				}
			}
			return lists;
		}

		// Token: 0x060002AC RID: 684 RVA: 0x00034D98 File Offset: 0x00032F98
		public static List<ActivitySystemItemInfo> FindLoginDevicePackage(int quality)
		{
			List<ActivitySystemItemInfo> lists = new List<ActivitySystemItemInfo>();
			List<ActivitySystemItemInfo> infos = ActiveMgr.FindActivitySystemItem(110);
			foreach (ActivitySystemItemInfo info in infos)
			{
				bool flag = info.Quality == quality;
				if (flag)
				{
					lists.Add(info);
				}
			}
			return lists;
		}

		// Token: 0x060002AD RID: 685 RVA: 0x00034E14 File Offset: 0x00033014
		public static List<ActivitySystemItemInfo> FindMinesRandomExchange(int total)
		{
			List<ActivitySystemItemInfo> lists = new List<ActivitySystemItemInfo>();
			List<ActivitySystemItemInfo> infos = ActiveMgr.FindActivitySystemItem(156);
			for (int i = 0; i < total; i++)
			{
				int place = ThreadSafeRandom.NextStatic(infos.Count);
				bool flag = place < infos.Count;
				if (flag)
				{
					lists.Add(infos[place]);
					infos.RemoveAt(place);
				}
			}
			return lists;
		}

		// Token: 0x060002AE RID: 686 RVA: 0x00034E84 File Offset: 0x00033084
		public static List<ActivitySystemItemInfo> FindLotteryTicketPackage(int quality)
		{
			List<ActivitySystemItemInfo> lists = new List<ActivitySystemItemInfo>();
			List<ActivitySystemItemInfo> infos = ActiveMgr.FindActivitySystemItem(114);
			foreach (ActivitySystemItemInfo info in infos)
			{
				bool flag = info.Quality == quality;
				if (flag)
				{
					lists.Add(info);
				}
			}
			return lists;
		}

		// Token: 0x060002AF RID: 687 RVA: 0x00034F00 File Offset: 0x00033100
		public static Dictionary<int, ActiveInfo> LoadActiveInfoDb()
		{
			Dictionary<int, ActiveInfo> list = new Dictionary<int, ActiveInfo>();
			using (ActiveBussiness ab = new ActiveBussiness())
			{
				ActiveInfo[] infos = ab.GetAllActives();
				foreach (ActiveInfo info in infos)
				{
					bool flag = info.ActiveID < 0;
					if (!flag)
					{
						bool flag2 = !list.ContainsKey(info.ActiveID);
						if (flag2)
						{
							list.Add(info.ActiveID, info);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x00034FA0 File Offset: 0x000331A0
		public static Dictionary<int, List<ActiveConvertItemInfo>> LoadActiveCondictionDb(Dictionary<int, ActiveInfo> Actives)
		{
			Dictionary<int, List<ActiveConvertItemInfo>> list = new Dictionary<int, List<ActiveConvertItemInfo>>();
			using (ActiveBussiness ab = new ActiveBussiness())
			{
				ActiveConvertItemInfo[] infos = ab.GetAllActiveConvertItem();
				using (Dictionary<int, ActiveInfo>.ValueCollection.Enumerator enumerator = Actives.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActiveInfo active = enumerator.Current;
						IEnumerable<ActiveConvertItemInfo> temp = from s in infos
						where s.ActiveID == active.ActiveID
						select s;
						list.Add(active.ActiveID, temp.ToList<ActiveConvertItemInfo>());
					}
				}
			}
			return list;
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x00035064 File Offset: 0x00033264
		public static Dictionary<int, List<ActiveAwardInfo>> LoadActiveGoodDb(Dictionary<int, ActiveInfo> Actives)
		{
			Dictionary<int, List<ActiveAwardInfo>> list = new Dictionary<int, List<ActiveAwardInfo>>();
			using (ActiveBussiness ab = new ActiveBussiness())
			{
				ActiveAwardInfo[] infos = ab.GetAllActiveAwardInfo();
				using (Dictionary<int, ActiveInfo>.ValueCollection.Enumerator enumerator = Actives.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActiveInfo Active = enumerator.Current;
						IEnumerable<ActiveAwardInfo> temp = from s in infos
						where s.ActiveID == Active.ActiveID
						select s;
						list.Add(Active.ActiveID, temp.ToList<ActiveAwardInfo>());
					}
				}
			}
			return list;
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x00035128 File Offset: 0x00033328
		public static ActiveInfo GetSingleActive(int id)
		{
			bool flag = ActiveMgr.m_activeinfo.Count == 0;
			if (flag)
			{
				ActiveMgr.Init();
			}
			bool flag2 = ActiveMgr.m_activeinfo.ContainsKey(id);
			ActiveInfo result;
			if (flag2)
			{
				result = ActiveMgr.m_activeinfo[id];
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x00035170 File Offset: 0x00033370
		public static List<ActiveAwardInfo> GetActiveAward(int id)
		{
			bool flag = ActiveMgr.m_activeinfo.Count == 0;
			if (flag)
			{
				ActiveMgr.Init();
			}
			bool flag2 = ActiveMgr.m_activeAwards.ContainsKey(id);
			List<ActiveAwardInfo> result;
			if (flag2)
			{
				result = ActiveMgr.m_activeAwards[id];
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x04000139 RID: 313
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Token: 0x0400013A RID: 314
		private static Dictionary<int, ActiveInfo> m_activeinfo = new Dictionary<int, ActiveInfo>();

		// Token: 0x0400013B RID: 315
		private static Dictionary<int, List<ActiveConvertItemInfo>> m_activeConvertItem = new Dictionary<int, List<ActiveConvertItemInfo>>();

		// Token: 0x0400013C RID: 316
		private static Dictionary<int, List<ActiveAwardInfo>> m_activeAwards = new Dictionary<int, List<ActiveAwardInfo>>();

		// Token: 0x0400013D RID: 317
		private static Dictionary<int, List<ActivitySystemItemInfo>> m_ActivitySystemItems = new Dictionary<int, List<ActivitySystemItemInfo>>();
	}
}
