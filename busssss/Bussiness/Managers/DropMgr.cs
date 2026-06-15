using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Bussiness.Protocol;
using log4net;
using SqlDataProvider.Data;

namespace Bussiness.Managers
{
	// Token: 0x02000035 RID: 53
	public class DropMgr
	{
		// Token: 0x060002D1 RID: 721 RVA: 0x00035B64 File Offset: 0x00033D64
		public static int FindCondiction(eDropType type, string para1, string para2)
		{
			string str = "," + para1 + ",";
			string str2 = "," + para2 + ",";
			foreach (DropCondiction condiction in DropMgr.m_dropcondiction)
			{
				bool flag = condiction.CondictionType == (int)type && condiction.Para1.IndexOf(str) != -1 && condiction.Para2.IndexOf(str2) != -1;
				if (flag)
				{
					return condiction.DropId;
				}
			}
			return 0;
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x00035C1C File Offset: 0x00033E1C
		public static List<DropItem> FindDropItem(int dropId)
		{
			bool flag = DropMgr.m_dropitem.ContainsKey(dropId);
			List<DropItem> result;
			if (flag)
			{
				result = DropMgr.m_dropitem[dropId];
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x00035C50 File Offset: 0x00033E50
		public static bool Init()
		{
			return DropMgr.ReLoad();
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x00035C68 File Offset: 0x00033E68
		public static List<DropCondiction> LoadDropConditionDb()
		{
			List<DropCondiction> result;
			using (ProduceBussiness bussiness = new ProduceBussiness())
			{
				DropCondiction[] allDropCondictions = bussiness.GetAllDropCondictions();
				result = ((allDropCondictions != null) ? allDropCondictions.ToList<DropCondiction>() : null);
			}
			return result;
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x00035CB0 File Offset: 0x00033EB0
		public static Dictionary<int, List<DropItem>> LoadDropItemDb()
		{
			Dictionary<int, List<DropItem>> dictionary = new Dictionary<int, List<DropItem>>();
			Dictionary<int, List<DropItem>> result;
			using (ProduceBussiness bussiness = new ProduceBussiness())
			{
				DropItem[] allDropItems = bussiness.GetAllDropItems();
				using (List<DropCondiction>.Enumerator enumerator = DropMgr.m_dropcondiction.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						DropCondiction info = enumerator.Current;
						IEnumerable<DropItem> source = from s in allDropItems
						where s.DropId == info.DropId
						select s;
						dictionary.Add(info.DropId, source.ToList<DropItem>());
					}
				}
				result = dictionary;
			}
			return result;
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x00035D70 File Offset: 0x00033F70
		public static bool ReLoad()
		{
			try
			{
				List<DropCondiction> list = DropMgr.LoadDropConditionDb();
				Interlocked.Exchange<List<DropCondiction>>(ref DropMgr.m_dropcondiction, list);
				Dictionary<int, List<DropItem>> dictionary = DropMgr.LoadDropItemDb();
				Interlocked.Exchange<Dictionary<int, List<DropItem>>>(ref DropMgr.m_dropitem, dictionary);
				return true;
			}
			catch (Exception exception)
			{
				DropMgr.log.Error("DropMgr", exception);
			}
			return false;
		}

		// Token: 0x0400014B RID: 331
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Token: 0x0400014C RID: 332
		private static List<DropCondiction> m_dropcondiction = new List<DropCondiction>();

		// Token: 0x0400014D RID: 333
		private static Dictionary<int, List<DropItem>> m_dropitem = new Dictionary<int, List<DropItem>>();

		// Token: 0x0400014E RID: 334
		private static string[] m_DropTypes = Enum.GetNames(typeof(eDropType));
	}
}
