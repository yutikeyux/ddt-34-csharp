using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;
using SqlDataProvider.Data;

namespace Bussiness.Managers
{
	// Token: 0x02000034 RID: 52
	public class DiceLevelAwardMgr
	{
		// Token: 0x060002C9 RID: 713 RVA: 0x00035980 File Offset: 0x00033B80
		public static bool ReLoad()
		{
			try
			{
				DiceLevelAwardInfo[] tempDiceLevelAward = DiceLevelAwardMgr.LoadDiceLevelAwardDb();
				Dictionary<int, List<DiceLevelAwardInfo>> tempDiceLevelAwards = DiceLevelAwardMgr.LoadDiceLevelAwards(tempDiceLevelAward);
				bool flag = tempDiceLevelAward != null;
				if (flag)
				{
					Interlocked.Exchange<DiceLevelAwardInfo[]>(ref DiceLevelAwardMgr.m_diceLevelAward, tempDiceLevelAward);
					Interlocked.Exchange<Dictionary<int, List<DiceLevelAwardInfo>>>(ref DiceLevelAwardMgr.m_DiceLevelAwards, tempDiceLevelAwards);
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = DiceLevelAwardMgr.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					DiceLevelAwardMgr.log.Error("ReLoad", e);
				}
				return false;
			}
			return true;
		}

		// Token: 0x060002CA RID: 714 RVA: 0x00035A04 File Offset: 0x00033C04
		public static bool Init()
		{
			return DiceLevelAwardMgr.ReLoad();
		}

		// Token: 0x060002CB RID: 715 RVA: 0x00035A1C File Offset: 0x00033C1C
		public static DiceLevelAwardInfo[] LoadDiceLevelAwardDb()
		{
			DiceLevelAwardInfo[] result;
			using (ProduceBussiness db = new ProduceBussiness())
			{
				DiceLevelAwardInfo[] infos = db.GetDiceLevelAwardInfos();
				result = infos;
			}
			return result;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x00035A60 File Offset: 0x00033C60
		public static Dictionary<int, List<DiceLevelAwardInfo>> LoadDiceLevelAwards(DiceLevelAwardInfo[] DiceLevelAwards)
		{
			Dictionary<int, List<DiceLevelAwardInfo>> infos = new Dictionary<int, List<DiceLevelAwardInfo>>();
			for (int i = 0; i < DiceLevelAwards.Length; i++)
			{
				DiceLevelAwardInfo info = DiceLevelAwards[i];
				bool flag = !infos.Keys.Contains(info.DiceLevel);
				if (flag)
				{
					IEnumerable<DiceLevelAwardInfo> temp = from s in DiceLevelAwards
					where s.DiceLevel == info.DiceLevel
					select s;
					infos.Add(info.DiceLevel, temp.ToList<DiceLevelAwardInfo>());
				}
			}
			return infos;
		}

		// Token: 0x060002CD RID: 717 RVA: 0x00035AEC File Offset: 0x00033CEC
		public static List<DiceLevelAwardInfo> FindDiceLevelAward(int DataId)
		{
			bool flag = DiceLevelAwardMgr.m_DiceLevelAwards.ContainsKey(DataId);
			List<DiceLevelAwardInfo> result;
			if (flag)
			{
				result = DiceLevelAwardMgr.m_DiceLevelAwards[DataId];
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060002CE RID: 718 RVA: 0x00035B20 File Offset: 0x00033D20
		public static List<DiceLevelAwardInfo> GetAllDiceLevelAwardAward(int DataId)
		{
			return DiceLevelAwardMgr.FindDiceLevelAward(DataId);
		}

		// Token: 0x04000147 RID: 327
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Token: 0x04000148 RID: 328
		private static DiceLevelAwardInfo[] m_diceLevelAward;

		// Token: 0x04000149 RID: 329
		private static Dictionary<int, List<DiceLevelAwardInfo>> m_DiceLevelAwards;

		// Token: 0x0400014A RID: 330
		private static ThreadSafeRandom random = new ThreadSafeRandom();
	}
}
