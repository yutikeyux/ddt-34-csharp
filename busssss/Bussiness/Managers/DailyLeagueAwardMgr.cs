using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;
using SqlDataProvider.Data;

namespace Bussiness.Managers
{
	// Token: 0x02000033 RID: 51
	public class DailyLeagueAwardMgr
	{
		// Token: 0x060002C2 RID: 706 RVA: 0x000357CC File Offset: 0x000339CC
		public static bool ReLoad()
		{
			try
			{
				DailyLeagueAwardInfo[] tempDailyLeagueAward = DailyLeagueAwardMgr.LoadDailyLeagueAwardDb();
				Dictionary<int, List<DailyLeagueAwardInfo>> tempDailyLeagueAwards = DailyLeagueAwardMgr.LoadDailyLeagueAwards(tempDailyLeagueAward);
				bool flag = tempDailyLeagueAward.Length != 0;
				if (flag)
				{
					Interlocked.Exchange<Dictionary<int, List<DailyLeagueAwardInfo>>>(ref DailyLeagueAwardMgr.m_DailyLeagueAwards, tempDailyLeagueAwards);
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = DailyLeagueAwardMgr.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					DailyLeagueAwardMgr.log.Error("ReLoad DailyLeagueAward", e);
				}
				return false;
			}
			return true;
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x00035844 File Offset: 0x00033A44
		public static bool Init()
		{
			return DailyLeagueAwardMgr.ReLoad();
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0003585C File Offset: 0x00033A5C
		public static DailyLeagueAwardInfo[] LoadDailyLeagueAwardDb()
		{
			DailyLeagueAwardInfo[] result;
			using (ProduceBussiness pb = new ProduceBussiness())
			{
				DailyLeagueAwardInfo[] infos = pb.GetAllDailyLeagueAward();
				result = infos;
			}
			return result;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x00035898 File Offset: 0x00033A98
		public static Dictionary<int, List<DailyLeagueAwardInfo>> LoadDailyLeagueAwards(DailyLeagueAwardInfo[] DailyLeagueAward)
		{
			Dictionary<int, List<DailyLeagueAwardInfo>> infos = new Dictionary<int, List<DailyLeagueAwardInfo>>();
			for (int i = 0; i < DailyLeagueAward.Length; i++)
			{
				DailyLeagueAwardInfo info = DailyLeagueAward[i];
				bool flag = !infos.Keys.Contains(info.Class);
				if (flag)
				{
					IEnumerable<DailyLeagueAwardInfo> temp = from s in DailyLeagueAward
					where s.Class == info.Class
					select s;
					infos.Add(info.Class, temp.ToList<DailyLeagueAwardInfo>());
				}
			}
			return infos;
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x00035924 File Offset: 0x00033B24
		public static List<DailyLeagueAwardInfo> FindDailyLeagueAward(int Class)
		{
			bool flag = DailyLeagueAwardMgr.m_DailyLeagueAwards.ContainsKey(Class);
			List<DailyLeagueAwardInfo> result;
			if (flag)
			{
				List<DailyLeagueAwardInfo> items = DailyLeagueAwardMgr.m_DailyLeagueAwards[Class];
				result = items;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x04000144 RID: 324
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Token: 0x04000145 RID: 325
		private static Dictionary<int, List<DailyLeagueAwardInfo>> m_DailyLeagueAwards;

		// Token: 0x04000146 RID: 326
		private static Random random = new Random();
	}
}
