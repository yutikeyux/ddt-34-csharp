using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;
using SqlDataProvider.Data;

namespace Bussiness.Managers
{
	// Token: 0x0200003C RID: 60
	public class NewTitleMgr
	{
		// Token: 0x06000321 RID: 801 RVA: 0x00038E0C File Offset: 0x0003700C
		public static bool ReLoad()
		{
			try
			{
				NewTitleInfo[] tempNewTitle = NewTitleMgr.LoadNewTitleDb();
				Dictionary<int, NewTitleInfo> tempNewTitles = NewTitleMgr.LoadNewTitles(tempNewTitle);
				bool flag = tempNewTitle.Length != 0;
				if (flag)
				{
					Interlocked.Exchange<Dictionary<int, NewTitleInfo>>(ref NewTitleMgr.m_NewTitles, tempNewTitles);
				}
			}
			catch (Exception e)
			{
				bool isErrorEnabled = NewTitleMgr.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					NewTitleMgr.log.Error("ReLoad NewTitle", e);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00038E84 File Offset: 0x00037084
		public static bool Init()
		{
			return NewTitleMgr.ReLoad();
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00038E9C File Offset: 0x0003709C
		public static NewTitleInfo[] LoadNewTitleDb()
		{
			NewTitleInfo[] allNewTitle;
			using (ProduceBussiness pb = new ProduceBussiness())
			{
				allNewTitle = pb.GetAllNewTitle();
			}
			return allNewTitle;
		}

		// Token: 0x06000324 RID: 804 RVA: 0x00038ED8 File Offset: 0x000370D8
		public static Dictionary<int, NewTitleInfo> LoadNewTitles(NewTitleInfo[] NewTitle)
		{
			Dictionary<int, NewTitleInfo> infos = new Dictionary<int, NewTitleInfo>();
			foreach (NewTitleInfo info in NewTitle)
			{
				bool flag = !infos.Keys.Contains(info.ID);
				if (flag)
				{
					infos.Add(info.ID, info);
				}
			}
			return infos;
		}

		// Token: 0x06000325 RID: 805 RVA: 0x00038F34 File Offset: 0x00037134
		public static NewTitleInfo FindNewTitle(int ID)
		{
			NewTitleMgr.m_clientLocker.AcquireWriterLock(-1);
			try
			{
				bool flag = NewTitleMgr.m_NewTitles.ContainsKey(ID);
				if (flag)
				{
					return NewTitleMgr.m_NewTitles[ID];
				}
			}
			finally
			{
				NewTitleMgr.m_clientLocker.ReleaseWriterLock();
			}
			return null;
		}

		// Token: 0x06000326 RID: 806 RVA: 0x00038F94 File Offset: 0x00037194
		public static NewTitleInfo FindNewTitleByName(string Name)
		{
			NewTitleMgr.m_clientLocker.AcquireWriterLock(-1);
			try
			{
				foreach (NewTitleInfo info in NewTitleMgr.m_NewTitles.Values)
				{
					bool flag = info.Name.ToLower() == Name.ToLower();
					if (flag)
					{
						return info;
					}
				}
			}
			finally
			{
				NewTitleMgr.m_clientLocker.ReleaseWriterLock();
			}
			return null;
		}

		// Token: 0x04000165 RID: 357
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Token: 0x04000166 RID: 358
		private static Dictionary<int, NewTitleInfo> m_NewTitles = new Dictionary<int, NewTitleInfo>();

		// Token: 0x04000167 RID: 359
		private static Random random = new Random();

		// Token: 0x04000168 RID: 360
		private static ReaderWriterLock m_clientLocker = new ReaderWriterLock();
	}
}
