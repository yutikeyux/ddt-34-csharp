using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;
using SqlDataProvider.Data;

namespace Bussiness.Managers
{
	// Token: 0x02000037 RID: 55
	public class EventLiveMgr
	{
		// Token: 0x060002E7 RID: 743 RVA: 0x00036880 File Offset: 0x00034A80
		public static bool Init()
		{
			return EventLiveMgr.ReLoad();
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x00036898 File Offset: 0x00034A98
		public static bool ReLoad()
		{
			try
			{
				Dictionary<int, EventLiveInfo> tempEventLiveInfo = EventLiveMgr.LoadEventLiveInfoDb();
				Dictionary<int, List<EventLiveGoods>> tempEventGoods = EventLiveMgr.LoadEventGoods(tempEventLiveInfo);
				bool flag = tempEventLiveInfo.Count > 0;
				if (flag)
				{
					Interlocked.Exchange<Dictionary<int, EventLiveInfo>>(ref EventLiveMgr.m_EventLiveInfo, tempEventLiveInfo);
					Interlocked.Exchange<Dictionary<int, List<EventLiveGoods>>>(ref EventLiveMgr.m_EventLiveGoods, tempEventGoods);
				}
				return true;
			}
			catch (Exception e)
			{
				EventLiveMgr.log.Error("EventLiveMgr", e);
			}
			return false;
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0003690C File Offset: 0x00034B0C
		public static Dictionary<int, EventLiveInfo> LoadEventLiveInfoDb()
		{
			Dictionary<int, EventLiveInfo> list = new Dictionary<int, EventLiveInfo>();
			Dictionary<int, EventLiveInfo> result;
			using (ProduceBussiness db = new ProduceBussiness())
			{
				EventLiveInfo[] array = db.GetAllEventLive();
				EventLiveInfo[] array2 = array;
				foreach (EventLiveInfo info in array2)
				{
					bool flag = !list.ContainsKey(info.EventID);
					if (flag)
					{
						list.Add(info.EventID, info);
					}
				}
				result = list;
			}
			return result;
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0003699C File Offset: 0x00034B9C
		public static Dictionary<int, List<EventLiveGoods>> LoadEventGoods(Dictionary<int, EventLiveInfo> events)
		{
			Dictionary<int, List<EventLiveGoods>> list = new Dictionary<int, List<EventLiveGoods>>();
			Dictionary<int, List<EventLiveGoods>> result;
			using (ProduceBussiness db = new ProduceBussiness())
			{
				EventLiveGoods[] infos = db.GetAllEventLiveGoods();
				using (Dictionary<int, EventLiveInfo>.ValueCollection.Enumerator enumerator = events.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						EventLiveInfo eventLive = enumerator.Current;
						IEnumerable<EventLiveGoods> temp = from s in infos
						where s.EventID == eventLive.EventID
						select s;
						list.Add(eventLive.EventID, temp.ToList<EventLiveGoods>());
					}
				}
				result = list;
			}
			return result;
		}

		// Token: 0x060002EB RID: 747 RVA: 0x00036A5C File Offset: 0x00034C5C
		public static EventLiveInfo GetSingleEvent(int id)
		{
			bool flag = EventLiveMgr.m_EventLiveInfo.ContainsKey(id);
			EventLiveInfo result;
			if (flag)
			{
				result = EventLiveMgr.m_EventLiveInfo[id];
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060002EC RID: 748 RVA: 0x00036A90 File Offset: 0x00034C90
		public static List<EventLiveGoods> GetEventGoods(EventLiveInfo info)
		{
			bool flag = EventLiveMgr.m_EventLiveGoods.ContainsKey(info.EventID);
			List<EventLiveGoods> result;
			if (flag)
			{
				result = EventLiveMgr.m_EventLiveGoods[info.EventID];
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060002ED RID: 749 RVA: 0x00036ACC File Offset: 0x00034CCC
		public static List<EventLiveInfo> GetAllEventInfo()
		{
			return EventLiveMgr.m_EventLiveInfo.Values.ToList<EventLiveInfo>();
		}

		// Token: 0x04000153 RID: 339
		private static Dictionary<int, EventLiveInfo> m_EventLiveInfo = new Dictionary<int, EventLiveInfo>();

		// Token: 0x04000154 RID: 340
		private static Dictionary<int, List<EventLiveGoods>> m_EventLiveGoods = new Dictionary<int, List<EventLiveGoods>>();

		// Token: 0x04000155 RID: 341
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
