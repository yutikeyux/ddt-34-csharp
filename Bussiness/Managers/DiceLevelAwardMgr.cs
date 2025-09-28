using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
namespace Bussiness.Managers
{
	public class DiceLevelAwardMgr
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private static DiceLevelAwardInfo[] m_diceLevelAward;
		private static Dictionary<int, List<DiceLevelAwardInfo>> m_DiceLevelAwards;
		private static ThreadSafeRandom random = new ThreadSafeRandom();
		public static bool ReLoad()
		{
			try
			{
				DiceLevelAwardInfo[] tempDiceLevelAward = DiceLevelAwardMgr.LoadDiceLevelAwardDb();
				Dictionary<int, List<DiceLevelAwardInfo>> tempDiceLevelAwards = DiceLevelAwardMgr.LoadDiceLevelAwards(tempDiceLevelAward);
				if (tempDiceLevelAward != null)
				{
					Interlocked.Exchange<DiceLevelAwardInfo[]>(ref DiceLevelAwardMgr.m_diceLevelAward, tempDiceLevelAward);
					Interlocked.Exchange<Dictionary<int, List<DiceLevelAwardInfo>>>(ref DiceLevelAwardMgr.m_DiceLevelAwards, tempDiceLevelAwards);
				}
			}
			catch (Exception e)
			{
				if (DiceLevelAwardMgr.log.IsErrorEnabled)
				{
					DiceLevelAwardMgr.log.Error("ReLoad", e);
				}
				return false;
			}
			return true;
		}
		public static bool Init()
		{
			return DiceLevelAwardMgr.ReLoad();
		}
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
		public static Dictionary<int, List<DiceLevelAwardInfo>> LoadDiceLevelAwards(DiceLevelAwardInfo[] DiceLevelAwards)
		{
			Dictionary<int, List<DiceLevelAwardInfo>> infos = new Dictionary<int, List<DiceLevelAwardInfo>>();
			for (int i = 0; i < DiceLevelAwards.Length; i++)
			{
				DiceLevelAwardInfo info = DiceLevelAwards[i];
				if (!infos.Keys.Contains(info.DiceLevel))
				{
					IEnumerable<DiceLevelAwardInfo> temp = 
						from s in DiceLevelAwards
						where s.DiceLevel == info.DiceLevel
						select s;
					infos.Add(info.DiceLevel, temp.ToList<DiceLevelAwardInfo>());
				}
			}
			return infos;
		}
		public static List<DiceLevelAwardInfo> FindDiceLevelAward(int DataId)
		{
			if (DiceLevelAwardMgr.m_DiceLevelAwards.ContainsKey(DataId))
			{
				return DiceLevelAwardMgr.m_DiceLevelAwards[DataId];
			}
			return null;
		}
		public static List<DiceLevelAwardInfo> GetAllDiceLevelAwardAward(int DataId)
		{
			return DiceLevelAwardMgr.FindDiceLevelAward(DataId);
		}
	}
}
