using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;
using SqlDataProvider.Data;

namespace Bussiness.Managers
{
	// Token: 0x0200003E RID: 62
	public class QQTipsMgr
	{
		// Token: 0x06000334 RID: 820 RVA: 0x00039354 File Offset: 0x00037554
		public static QQtipsMessagesInfo GetQQtipsMessages()
		{
			bool flag = QQTipsMgr._qqtips == null;
			if (flag)
			{
				QQTipsMgr.Init();
			}
			QQTipsMgr.m_lock.AcquireReaderLock(10000);
			QQtipsMessagesInfo result;
			try
			{
				bool flag2 = QQTipsMgr.qqtipSelectIndex >= QQTipsMgr._qqtips.Count;
				if (flag2)
				{
					QQTipsMgr.qqtipSelectIndex = 1;
				}
				else
				{
					QQTipsMgr.qqtipSelectIndex++;
				}
				result = QQTipsMgr._qqtips[QQTipsMgr.qqtipSelectIndex];
			}
			finally
			{
				QQTipsMgr.m_lock.ReleaseReaderLock();
			}
			return result;
		}

		// Token: 0x06000335 RID: 821 RVA: 0x000393E8 File Offset: 0x000375E8
		public static bool Init()
		{
			bool result;
			try
			{
				QQTipsMgr.m_lock = new ReaderWriterLock();
				QQTipsMgr._qqtips = new Dictionary<int, QQtipsMessagesInfo>();
				result = QQTipsMgr.LoadItem(QQTipsMgr._qqtips);
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = QQTipsMgr.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					QQTipsMgr.log.Error("Init", exception);
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06000336 RID: 822 RVA: 0x00039450 File Offset: 0x00037650
		public static bool LoadItem(Dictionary<int, QQtipsMessagesInfo> infos)
		{
			using (ProduceBussiness bussiness = new ProduceBussiness())
			{
				QQtipsMessagesInfo[] allQQtipsMessagesLoad = bussiness.GetAllQQtipsMessagesLoad();
				QQtipsMessagesInfo[] array = allQQtipsMessagesLoad;
				foreach (QQtipsMessagesInfo info in array)
				{
					bool flag = !infos.Keys.Contains(info.ID);
					if (flag)
					{
						infos.Add(info.ID, info);
					}
				}
			}
			return true;
		}

		// Token: 0x06000337 RID: 823 RVA: 0x000394E0 File Offset: 0x000376E0
		public static bool ReLoad()
		{
			try
			{
				Dictionary<int, QQtipsMessagesInfo> infos = new Dictionary<int, QQtipsMessagesInfo>();
				bool flag = QQTipsMgr.LoadItem(infos);
				if (flag)
				{
					QQTipsMgr.m_lock.AcquireWriterLock(-1);
					try
					{
						QQTipsMgr._qqtips = infos;
						return true;
					}
					catch
					{
					}
					finally
					{
						QQTipsMgr.m_lock.ReleaseWriterLock();
					}
				}
			}
			catch (Exception exception)
			{
				bool isErrorEnabled = QQTipsMgr.log.IsErrorEnabled;
				if (isErrorEnabled)
				{
					QQTipsMgr.log.Error("ReLoad", exception);
				}
			}
			return false;
		}

		// Token: 0x0400016D RID: 365
		private static Dictionary<int, QQtipsMessagesInfo> _qqtips;

		// Token: 0x0400016E RID: 366
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Token: 0x0400016F RID: 367
		private static ReaderWriterLock m_lock;

		// Token: 0x04000170 RID: 368
		private static int qqtipSelectIndex = 0;
	}
}
