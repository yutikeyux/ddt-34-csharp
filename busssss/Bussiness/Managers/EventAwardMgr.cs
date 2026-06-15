using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;
using SqlDataProvider.Data;

namespace Bussiness.Managers
{
	// Token: 0x02000036 RID: 54
	public class EventAwardMgr
	{
		// Token: 0x060002D9 RID: 729 RVA: 0x00035E1B File Offset: 0x0003401B
		public static void CreateEventAward(eEventType DateId)
		{
		}

		// Token: 0x060002DA RID: 730 RVA: 0x00035E20 File Offset: 0x00034020
		public static EventAwardInfo CreateSearchGoodsAward(eEventType DataId)
		{
			List<EventAwardInfo> list = new List<EventAwardInfo>();
			List<EventAwardInfo> list2 = EventAwardMgr.FindEventAward(DataId);
			int count = 1;
			int maxRound = ThreadSafeRandom.NextStatic((from s in list2
			select s.Random).Max());
			List<EventAwardInfo> source = (from s in list2
			where s.Random >= maxRound
			select s).ToList<EventAwardInfo>();
			int num2 = source.Count<EventAwardInfo>();
			bool flag = num2 > 0;
			if (flag)
			{
				count = ((count > num2) ? num2 : count);
				int[] randomUnrepeatArray = EventAwardMgr.GetRandomUnrepeatArray(0, num2 - 1, count);
				int[] array = randomUnrepeatArray;
				foreach (int num3 in array)
				{
					EventAwardInfo item = source[num3];
					list.Add(item);
				}
			}
			foreach (EventAwardInfo info2 in list)
			{
				bool flag2 = ItemMgr.FindItemTemplate(info2.TemplateID) != null;
				if (flag2)
				{
					return info2;
				}
			}
			return null;
		}

		// Token: 0x060002DB RID: 731 RVA: 0x00035F5C File Offset: 0x0003415C
		public static List<EventAwardInfo> FindEventAward(eEventType DataId)
		{
			bool flag = EventAwardMgr.m_EventAwards.ContainsKey((int)DataId);
			List<EventAwardInfo> result;
			if (flag)
			{
				result = EventAwardMgr.m_EventAwards[(int)DataId];
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060002DC RID: 732 RVA: 0x00035F90 File Offset: 0x00034190
		public static List<NewChickenBoxItemInfo> GetNewChickenBoxAward(eEventType DataId)
		{
			List<NewChickenBoxItemInfo> list = new List<NewChickenBoxItemInfo>();
			List<EventAwardInfo> list2 = new List<EventAwardInfo>();
			List<EventAwardInfo> source = EventAwardMgr.FindEventAward(DataId);
			int num = 1;
			int maxRound = ThreadSafeRandom.NextStatic((from s in source
			select s.Random).Max());
			List<EventAwardInfo> list3 = (from s in source
			where s.Random >= maxRound
			select s).ToList<EventAwardInfo>();
			int num2 = list3.Count<EventAwardInfo>();
			bool flag = num2 > 0;
			if (flag)
			{
				num = ((num > num2) ? num2 : num);
				int[] randomUnrepeatArray = EventAwardMgr.GetRandomUnrepeatArray(0, num2 - 1, num);
				int[] array = randomUnrepeatArray;
				foreach (int index in array)
				{
					EventAwardInfo item = list3[index];
					list2.Add(item);
				}
			}
			foreach (EventAwardInfo current in list2)
			{
				NewChickenBoxItemInfo newChickenBoxItemInfo = new NewChickenBoxItemInfo();
				newChickenBoxItemInfo.TemplateID = current.TemplateID;
				newChickenBoxItemInfo.IsBinds = current.IsBinds;
				newChickenBoxItemInfo.ValidDate = current.ValidDate;
				newChickenBoxItemInfo.Count = current.Count;
				newChickenBoxItemInfo.StrengthenLevel = current.StrengthenLevel;
				newChickenBoxItemInfo.AttackCompose = 0;
				newChickenBoxItemInfo.DefendCompose = 0;
				newChickenBoxItemInfo.AgilityCompose = 0;
				newChickenBoxItemInfo.LuckCompose = 0;
				NewChickenBoxItemInfo newChickenBoxItemInfo2 = newChickenBoxItemInfo;
				ItemTemplateInfo itemTemplateInfo = ItemMgr.FindItemTemplate(current.TemplateID);
				newChickenBoxItemInfo2.Quality = ((itemTemplateInfo != null) ? itemTemplateInfo.Quality : 2);
				newChickenBoxItemInfo.IsSelected = false;
				newChickenBoxItemInfo.IsSeeded = false;
				list.Add(newChickenBoxItemInfo);
			}
			return list;
		}

		// Token: 0x060002DD RID: 733 RVA: 0x00036174 File Offset: 0x00034374
		public static List<NewChickenBoxItemInfo> GetLuckyStartAward(eEventType DataId)
		{
			List<NewChickenBoxItemInfo> infos = new List<NewChickenBoxItemInfo>();
			List<EventAwardInfo> FiltInfos = new List<EventAwardInfo>();
			List<EventAwardInfo> unFiltInfos = EventAwardMgr.FindEventAward(DataId);
			int dropItemCount = 1;
			int maxRound = EventAwardMgr.rand.Next((from s in unFiltInfos
			select s.Random).Max());
			List<EventAwardInfo> RoundInfos = (from s in unFiltInfos
			where s.Random >= maxRound
			select s).ToList<EventAwardInfo>();
			int maxItems = RoundInfos.Count<EventAwardInfo>();
			bool flag = maxItems > 0;
			if (flag)
			{
				dropItemCount = ((dropItemCount > maxItems) ? maxItems : dropItemCount);
				int[] randomArray = EventAwardMgr.GetRandomUnrepeatArray(0, maxItems - 1, dropItemCount);
				foreach (int i in randomArray)
				{
					EventAwardInfo item = RoundInfos[i];
					FiltInfos.Add(item);
				}
			}
			foreach (EventAwardInfo info in FiltInfos)
			{
				NewChickenBoxItemInfo item2 = new NewChickenBoxItemInfo();
				item2.TemplateID = info.TemplateID;
				item2.IsBinds = info.IsBinds;
				item2.ValidDate = info.ValidDate;
				item2.Count = info.Count;
				item2.StrengthenLevel = info.StrengthenLevel;
				item2.AttackCompose = 0;
				item2.DefendCompose = 0;
				item2.AgilityCompose = 0;
				item2.LuckCompose = 0;
				ItemTemplateInfo tempInfo = ItemMgr.FindItemTemplate(info.TemplateID);
				item2.Quality = ((tempInfo == null) ? 2 : tempInfo.Quality);
				item2.IsSelected = true;
				item2.IsSeeded = true;
				item2.Random = info.Random;
				infos.Add(item2);
			}
			return infos;
		}

		// Token: 0x060002DE RID: 734 RVA: 0x00036368 File Offset: 0x00034568
		public static int[] GetRandomUnrepeatArray(int minValue, int maxValue, int count)
		{
			int[] numArray = new int[count];
			for (int i = 0; i < count; i++)
			{
				int num2 = EventAwardMgr.rand.Next(minValue, maxValue + 1);
				int num3 = 0;
				for (int j = 0; j < i; j++)
				{
					bool flag = numArray[j] == num2;
					if (flag)
					{
						num3++;
					}
				}
				bool flag2 = num3 == 0;
				if (flag2)
				{
					numArray[i] = num2;
				}
				else
				{
					i--;
				}
			}
			return numArray;
		}

		// Token: 0x060002DF RID: 735 RVA: 0x000363EC File Offset: 0x000345EC
		public static bool Init()
		{
			return EventAwardMgr.ReLoad();
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00036404 File Offset: 0x00034604
		public static EventAwardInfo[] LoadEventAwardDb()
		{
			EventAwardInfo[] eventAwardInfos;
			using (ProduceBussiness bussiness = new ProduceBussiness())
			{
				eventAwardInfos = bussiness.GetEventAwardInfos();
			}
			return eventAwardInfos;
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x00036440 File Offset: 0x00034640
		public static Dictionary<int, List<EventAwardInfo>> LoadEventAwards(EventAwardInfo[] EventAwards)
		{
			Dictionary<int, List<EventAwardInfo>> dictionary = new Dictionary<int, List<EventAwardInfo>>();
			for (int i = 0; i < EventAwards.Length; i++)
			{
				EventAwardInfo info = EventAwards[i];
				bool flag = !dictionary.Keys.Contains(info.ActivityType);
				if (flag)
				{
					IEnumerable<EventAwardInfo> source = from s in EventAwards
					where s.ActivityType == info.ActivityType
					select s;
					dictionary.Add(info.ActivityType, source.ToList<EventAwardInfo>());
				}
			}
			return dictionary;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x000364CC File Offset: 0x000346CC
		public static List<EventAwardInfo> GetDiceAward(eEventType DataId)
		{
			List<EventAwardInfo> FiltInfos = new List<EventAwardInfo>();
			List<EventAwardInfo> unFiltInfos = EventAwardMgr.FindEventAward(DataId);
			int dropItemCount = 1;
			int maxRound = ThreadSafeRandom.NextStatic((from s in unFiltInfos
			select s.Random).Max());
			List<EventAwardInfo> RoundInfos = (from s in unFiltInfos
			where s.Random >= maxRound
			select s).ToList<EventAwardInfo>();
			int maxItems = RoundInfos.Count<EventAwardInfo>();
			bool flag = maxItems > 0;
			if (flag)
			{
				dropItemCount = ((dropItemCount > maxItems) ? maxItems : dropItemCount);
				int[] randomArray = EventAwardMgr.GetRandomUnrepeatArray(0, maxItems - 1, dropItemCount);
				foreach (int j in randomArray)
				{
					EventAwardInfo item = RoundInfos[j];
					FiltInfos.Add(item);
				}
			}
			return FiltInfos;
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x000365AC File Offset: 0x000347AC
		public static bool ReLoad()
		{
			try
			{
				EventAwardInfo[] eventAwards = EventAwardMgr.LoadEventAwardDb();
				Dictionary<int, List<EventAwardInfo>> dictionary = EventAwardMgr.LoadEventAwards(eventAwards);
				bool flag = eventAwards != null;
				if (flag)
				{
					Interlocked.Exchange<EventAwardInfo[]>(ref EventAwardMgr.m_eventAward, eventAwards);
					Interlocked.Exchange<Dictionary<int, List<EventAwardInfo>>>(ref EventAwardMgr.m_EventAwards, dictionary);
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = EventAwardMgr.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					EventAwardMgr.log.Error("ReLoad", exception);
				}
				return false;
			}
			return true;
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x00036630 File Offset: 0x00034830
		public static List<ItemInfo> GetEventAwardByType(eEventType DataId)
		{
			List<ItemInfo> infos = new List<ItemInfo>();
			List<EventAwardInfo> FiltInfos = new List<EventAwardInfo>();
			List<EventAwardInfo> unFiltInfos = EventAwardMgr.FindEventAward(DataId);
			bool flag = unFiltInfos.Count == 0;
			List<ItemInfo> result;
			if (flag)
			{
				result = null;
			}
			else
			{
				FiltInfos = (from s in unFiltInfos
				where s.IsSelect
				select s).ToList<EventAwardInfo>();
				int dropItemCount = 1;
				int maxRound = EventAwardMgr.rand.Next((from s in unFiltInfos
				select s.Random).Max());
				List<EventAwardInfo> RoundInfos = (from s in unFiltInfos
				where s.Random >= maxRound && !s.IsSelect
				select s).ToList<EventAwardInfo>();
				int maxItems = RoundInfos.Count<EventAwardInfo>();
				bool flag2 = maxItems > 0;
				if (flag2)
				{
					dropItemCount = ((dropItemCount > maxItems) ? maxItems : dropItemCount);
					int[] randomArray = EventAwardMgr.GetRandomUnrepeatArray(0, maxItems - 1, dropItemCount);
					foreach (int i in randomArray)
					{
						EventAwardInfo item = RoundInfos[i];
						FiltInfos.Add(item);
					}
				}
				foreach (EventAwardInfo info in FiltInfos)
				{
					ItemTemplateInfo tempInfo = ItemMgr.FindItemTemplate(info.TemplateID);
					ItemInfo item2 = ItemInfo.CreateFromTemplate(tempInfo, info.Count, 105);
					item2.TemplateID = info.TemplateID;
					item2.IsBinds = info.IsBinds;
					item2.ValidDate = info.ValidDate;
					item2.Count = info.Count;
					item2.StrengthenLevel = info.StrengthenLevel;
					item2.AttackCompose = info.AttackCompose;
					item2.DefendCompose = info.DefendCompose;
					item2.AgilityCompose = info.AgilityCompose;
					item2.LuckCompose = info.LuckCompose;
					infos.Add(item2);
				}
				result = infos;
			}
			return result;
		}

		// Token: 0x0400014F RID: 335
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Token: 0x04000150 RID: 336
		private static EventAwardInfo[] m_eventAward;

		// Token: 0x04000151 RID: 337
		private static Dictionary<int, List<EventAwardInfo>> m_EventAwards;

		// Token: 0x04000152 RID: 338
		private static ThreadSafeRandom rand = new ThreadSafeRandom();
	}
}
