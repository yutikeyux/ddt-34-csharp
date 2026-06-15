using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;
using SqlDataProvider.Data;

namespace Bussiness.Managers
{
	// Token: 0x02000038 RID: 56
	public class FightSpiritTemplateMgr
	{
		// Token: 0x060002F0 RID: 752 RVA: 0x00036B20 File Offset: 0x00034D20
		public static bool ReLoad()
		{
			try
			{
				FightSpiritTemplateInfo[] tempFightSpiritTemplate = FightSpiritTemplateMgr.LoadFightSpiritTemplateDb();
				Dictionary<int, List<FightSpiritTemplateInfo>> tempFightSpiritTemplates = FightSpiritTemplateMgr.LoadFightSpiritTemplates(tempFightSpiritTemplate);
				bool flag = tempFightSpiritTemplate.Length != 0;
				if (flag)
				{
					Interlocked.Exchange<Dictionary<int, List<FightSpiritTemplateInfo>>>(ref FightSpiritTemplateMgr.m_fightSpiritTemplates, tempFightSpiritTemplates);
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = FightSpiritTemplateMgr.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					FightSpiritTemplateMgr.log.Error("ReLoad FightSpiritTemplate", e);
				}
				return false;
			}
			return true;
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x00036B98 File Offset: 0x00034D98
		public static bool Init()
		{
			return FightSpiritTemplateMgr.ReLoad();
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x00036BB0 File Offset: 0x00034DB0
		public static FightSpiritTemplateInfo[] LoadFightSpiritTemplateDb()
		{
			FightSpiritTemplateInfo[] result;
			using (ProduceBussiness pb = new ProduceBussiness())
			{
				FightSpiritTemplateInfo[] infos = pb.GetAllFightSpiritTemplate();
				result = infos;
			}
			return result;
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x00036BEC File Offset: 0x00034DEC
		public static Dictionary<int, List<FightSpiritTemplateInfo>> LoadFightSpiritTemplates(FightSpiritTemplateInfo[] fightSpiritTemplates)
		{
			Dictionary<int, List<FightSpiritTemplateInfo>> infos = new Dictionary<int, List<FightSpiritTemplateInfo>>();
			for (int i = 0; i < fightSpiritTemplates.Length; i++)
			{
				FightSpiritTemplateInfo info = fightSpiritTemplates[i];
				bool flag = !infos.Keys.Contains(info.FightSpiritID);
				if (flag)
				{
					IEnumerable<FightSpiritTemplateInfo> temp = from s in fightSpiritTemplates
					where s.FightSpiritID == info.FightSpiritID
					select s;
					infos.Add(info.FightSpiritID, temp.ToList<FightSpiritTemplateInfo>());
				}
			}
			return infos;
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x00036C78 File Offset: 0x00034E78
		public static List<FightSpiritTemplateInfo> FindFightSpiritTemplates(int id)
		{
			FightSpiritTemplateMgr.m_clientLocker.AcquireWriterLock(-1);
			try
			{
				bool flag = FightSpiritTemplateMgr.m_fightSpiritTemplates.ContainsKey(id);
				if (flag)
				{
					return FightSpiritTemplateMgr.m_fightSpiritTemplates[id];
				}
			}
			finally
			{
				FightSpiritTemplateMgr.m_clientLocker.ReleaseWriterLock();
			}
			return new List<FightSpiritTemplateInfo>();
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x00036CE0 File Offset: 0x00034EE0
		public static FightSpiritTemplateInfo FindFightSpiritTemplateInfo(int FigSpiritId, int lv)
		{
			List<FightSpiritTemplateInfo> infos = FightSpiritTemplateMgr.FindFightSpiritTemplates(FigSpiritId);
			foreach (FightSpiritTemplateInfo fs in infos)
			{
				bool flag = fs.Level == lv;
				if (flag)
				{
					return fs;
				}
			}
			return null;
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x00036D50 File Offset: 0x00034F50
		public static int GOLDEN_LEVEL(int lv)
		{
			try
			{
				string[] addamages = GameProperties.FightSpiritLevelAddDamage.Split(new char[]
				{
					'|'
				});
				foreach (string add in addamages)
				{
					bool flag = add.Split(new char[]
					{
						','
					})[0] == lv.ToString();
					if (flag)
					{
						return int.Parse(add.Split(new char[]
						{
							','
						})[1]);
					}
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = FightSpiritTemplateMgr.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					FightSpiritTemplateMgr.log.Error("FightSpiritTemplate.GOLDEN_LEVEL: ", e);
				}
			}
			return 0;
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x00036E10 File Offset: 0x00035010
		public static int[] Exps()
		{
			int[] exps = new int[]
			{
				0,
				600,
				5220,
				15840,
				39020,
				89580
			};
			return exps.ToArray<int>();
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00036E3C File Offset: 0x0003503C
		public static int getProp(int figSpiritId, int lv, int place)
		{
			FightSpiritTemplateInfo temp = FightSpiritTemplateMgr.FindFightSpiritTemplateInfo(figSpiritId, lv);
			bool flag = temp == null;
			if (flag)
			{
				List<FightSpiritTemplateInfo> infos = FightSpiritTemplateMgr.FindFightSpiritTemplates(figSpiritId);
				bool flag2 = infos.Count > 0;
				if (!flag2)
				{
					FightSpiritTemplateMgr.log.ErrorFormat("FigSpiritId: {0} not found! Return 0", figSpiritId);
					return 0;
				}
				temp = infos[infos.Count - 1];
				FightSpiritTemplateMgr.log.ErrorFormat("FigSpiritId: {0}, level: {1} not found! Return Max level in database is {2}", figSpiritId, lv, temp.Level);
			}
			switch (place)
			{
			case 2:
				return temp.Attack;
			case 3:
				return temp.Lucky;
			case 4:
				break;
			case 5:
				return temp.Agility;
			default:
				if (place == 11)
				{
					return temp.Defence;
				}
				if (place == 13)
				{
					return temp.Blood;
				}
				break;
			}
			return 0;
		}

		// Token: 0x04000156 RID: 342
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Token: 0x04000157 RID: 343
		private static Dictionary<int, List<FightSpiritTemplateInfo>> m_fightSpiritTemplates = new Dictionary<int, List<FightSpiritTemplateInfo>>();

		// Token: 0x04000158 RID: 344
		private static ReaderWriterLock m_clientLocker = new ReaderWriterLock();
	}
}
