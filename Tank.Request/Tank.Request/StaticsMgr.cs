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
	// Token: 0x02000072 RID: 114
	public static class StaticsMgr
	{
		// Token: 0x06000211 RID: 529 RVA: 0x0000FDE0 File Offset: 0x0000DFE0
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

		// Token: 0x06000212 RID: 530 RVA: 0x0000FEBC File Offset: 0x0000E0BC
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
				StringBuilder build = new StringBuilder();
				try
				{
					build.Append(CelebByGpList.Build());
					build.Append(CelebByDayGPList.Build());
					build.Append(CelebByWeekGPList.Build());
					build.Append(CelebByOfferList.Build());
					build.Append(CelebByDayOfferList.Build());
					build.Append(CelebByWeekOfferList.Build());
					build.Append(CelebByDayFightPowerList.Build());
					build.Append(CelebByConsortiaRiches.Build());
					build.Append(CelebByConsortiaDayRiches.Build());
					build.Append(CelebByConsortiaWeekRiches.Build());
					build.Append(CelebByConsortiaHonor.Build());
					build.Append(CelebByConsortiaDayHonor.Build());
					build.Append(CelebByConsortiaWeekHonor.Build());
					build.Append(CelebByConsortiaLevel.Build());
					build.Append(CelebByDayBestEquip.Build());
					StaticsMgr.log.Info("Complete auto update Celeb in " + DateTime.Now.ToString());
				}
				catch (Exception ex2)
				{
					build.Append("CelebByList is Error!");
					StaticsMgr.log.Error(build.ToString(), ex2);
				}
			}
		}

		// Token: 0x06000213 RID: 531 RVA: 0x000102F4 File Offset: 0x0000E4F4
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

		// Token: 0x06000214 RID: 532 RVA: 0x000103B4 File Offset: 0x0000E5B4
		public static void RegCountAdd()
		{
			object locker = StaticsMgr._locker;
			lock (locker)
			{
				StaticsMgr.RegCount++;
			}
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00010400 File Offset: 0x0000E600
		public static void Stop()
		{
			StaticsMgr._timer.Dispose();
			StaticsMgr.OnTimer(null);
		}

		// Token: 0x04000073 RID: 115
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Token: 0x04000074 RID: 116
		private static Timer _timer;

		// Token: 0x04000075 RID: 117
		private static object _locker = new object();

		// Token: 0x04000076 RID: 118
		private static List<string> _list = new List<string>();

		// Token: 0x04000077 RID: 119
		private static int RegCount = 0;

		// Token: 0x04000078 RID: 120
		private static int pid;

		// Token: 0x04000079 RID: 121
		private static int did;

		// Token: 0x0400007A RID: 122
		private static int sid;

		// Token: 0x0400007B RID: 123
		private static string _path;

		// Token: 0x0400007C RID: 124
		private static long _interval;

		// Token: 0x0400007D RID: 125
		private static int CelebBuildDay;

		// Token: 0x0400007E RID: 126
		public static string CurrentPath;
	}
}
