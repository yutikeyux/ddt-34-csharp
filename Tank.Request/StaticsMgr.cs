using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using log4net;
using Tank.Request.CelebList;

namespace Tank.Request
{
	// Token: 0x02000076 RID: 118
	public static class StaticsMgr
	{
		// Token: 0x06000217 RID: 535 RVA: 0x0000F814 File Offset: 0x0000DA14
		public static void Setup()
		{
			StaticsMgr.CurrentPath = HttpContext.Current.Server.MapPath("~");
			StaticsMgr.CelebBuildDay = DateTime.Now.Day;
			StaticsMgr.pid = int.Parse(ConfigurationManager.AppSettings["PID"]);
			StaticsMgr.did = int.Parse(ConfigurationManager.AppSettings["DID"]);
			StaticsMgr.sid = int.Parse(ConfigurationManager.AppSettings["SID"]);
			StaticsMgr._path = ConfigurationManager.AppSettings["LogPath"];
			StaticsMgr._interval = (long)(int.Parse(ConfigurationManager.AppSettings["LogInterval"]) * 60 * 1000);
			StaticsMgr._timer = new Timer(new TimerCallback(StaticsMgr.OnTimer), null, 0L, StaticsMgr._interval);
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000F8F0 File Offset: 0x0000DAF0
		private static void OnTimer(object state)
		{
			try
			{
				object locker = StaticsMgr._locker;
				lock (locker)
				{
					bool flag2 = StaticsMgr._list.Count > 0;
					if (flag2)
					{
						string filename = string.Format("{0}\\payment-{1:D2}{2:D2}{3:D2}-{4:yyyyMMdd}.log", new object[]
						{
							StaticsMgr._path,
							StaticsMgr.pid,
							StaticsMgr.did,
							StaticsMgr.sid,
							DateTime.Now
						});
						using (FileStream fs = File.Open(filename, FileMode.Append))
						{
							using (StreamWriter writer = new StreamWriter(fs))
							{
								while (StaticsMgr._list.Count != 0)
								{
									writer.WriteLine(StaticsMgr._list[0]);
									StaticsMgr._list.RemoveAt(0);
								}
							}
						}
					}
					bool flag3 = StaticsMgr.RegCount > 0;
					if (flag3)
					{
						string filename2 = string.Format("{0}\\reg-{1:D2}{2:D2}{3:D2}-{4:yyyyMMdd}.log", new object[]
						{
							StaticsMgr._path,
							StaticsMgr.pid,
							StaticsMgr.did,
							StaticsMgr.sid,
							DateTime.Now
						});
						using (FileStream fs2 = File.Open(filename2, FileMode.Append))
						{
							using (StreamWriter writer2 = new StreamWriter(fs2))
							{
								string str = string.Format("{0},{1},{2},{3},{4}", new object[]
								{
									StaticsMgr.pid,
									StaticsMgr.did,
									StaticsMgr.sid,
									DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
									StaticsMgr.RegCount
								});
								writer2.WriteLine(str);
								StaticsMgr.RegCount = 0;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				StaticsMgr.log.Error("Save log error", ex);
			}
			bool flag4 = StaticsMgr.CelebBuildDay != DateTime.Now.Day && DateTime.Now.Hour > 2 && DateTime.Now.Hour < 6;
			if (flag4)
			{
				StaticsMgr.CelebBuildDay = DateTime.Now.Day;
				StringBuilder Build = new StringBuilder();
				try
				{
					Build.Append(CelebByGpList.Build());
					Build.Append(CelebByDayGPList.Build());
					Build.Append(CelebByWeekGPList.Build());
					Build.Append(CelebByOfferList.Build());
					Build.Append(CelebByDayOfferList.Build());
					Build.Append(CelebByWeekOfferList.Build());
					Build.Append(CelebByDayFightPowerList.Build());
					Build.Append(CelebByConsortiaRiches.Build());
					Build.Append(CelebByConsortiaDayRiches.Build());
					Build.Append(CelebByConsortiaWeekRiches.Build());
					Build.Append(CelebByConsortiaHonor.Build());
					Build.Append(CelebByConsortiaDayHonor.Build());
					Build.Append(CelebByConsortiaWeekHonor.Build());
					Build.Append(CelebByConsortiaLevel.Build());
					Build.Append(CelebByDayBestEquip.Build());
					StaticsMgr.log.Info("Complete auto update Celeb in " + DateTime.Now.ToString());
				}
				catch (Exception ex2)
				{
					Build.Append("CelebByList is Error!");
					StaticsMgr.log.Error(Build.ToString(), ex2);
				}
			}
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000FD28 File Offset: 0x0000DF28
		public static void Log(DateTime dt, string username, bool sex, int money, string payway, decimal needMoney)
		{
			string str = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", new object[]
			{
				StaticsMgr.pid,
				StaticsMgr.did,
				StaticsMgr.sid,
				dt.ToString("yyyy-MM-dd HH:mm:ss"),
				username,
				sex ? 1 : 0,
				money,
				payway,
				needMoney
			});
			object locker = StaticsMgr._locker;
			lock (locker)
			{
				StaticsMgr._list.Add(str);
			}
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000FDE8 File Offset: 0x0000DFE8
		public static void RegCountAdd()
		{
			object locker = StaticsMgr._locker;
			lock (locker)
			{
				StaticsMgr.RegCount++;
			}
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00002B56 File Offset: 0x00000D56
		public static void Stop()
		{
			StaticsMgr._timer.Dispose();
			StaticsMgr.OnTimer(null);
		}

		// Token: 0x04000079 RID: 121
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Token: 0x0400007A RID: 122
		private static Timer _timer;

		// Token: 0x0400007B RID: 123
		private static object _locker = new object();

		// Token: 0x0400007C RID: 124
		private static List<string> _list = new List<string>();

		// Token: 0x0400007D RID: 125
		private static int RegCount = 0;

		// Token: 0x0400007E RID: 126
		private static int pid;

		// Token: 0x0400007F RID: 127
		private static int did;

		// Token: 0x04000080 RID: 128
		private static int sid;

		// Token: 0x04000081 RID: 129
		private static string _path;

		// Token: 0x04000082 RID: 130
		private static long _interval;

		// Token: 0x04000083 RID: 131
		private static int CelebBuildDay;

		// Token: 0x04000084 RID: 132
		public static string CurrentPath;
	}
}
