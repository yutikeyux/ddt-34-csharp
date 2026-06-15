using System;
using System.Collections.Generic;
using System.Reflection;
using DAL;
using log4net;

namespace Bussiness
{
	// Token: 0x0200000D RID: 13
	public class CountBussiness
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600006C RID: 108 RVA: 0x0000B971 File Offset: 0x00009B71
		public static int AppID
		{
			get
			{
				return CountBussiness._appID;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600006D RID: 109 RVA: 0x0000B978 File Offset: 0x00009B78
		public static string ConnectionString
		{
			get
			{
				return CountBussiness._connectionString;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600006E RID: 110 RVA: 0x0000B97F File Offset: 0x00009B7F
		public static bool CountRecord
		{
			get
			{
				return CountBussiness._conutRecord;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600006F RID: 111 RVA: 0x0000B986 File Offset: 0x00009B86
		public static int ServerID
		{
			get
			{
				return CountBussiness._serverID;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000070 RID: 112 RVA: 0x0000B98D File Offset: 0x00009B8D
		public static int SubID
		{
			get
			{
				return CountBussiness._subID;
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x0000B994 File Offset: 0x00009B94
		public static void InsertContentCount(Dictionary<string, string> clientInfos)
		{
			try
			{
				bool countRecord = CountBussiness.CountRecord;
				if (countRecord)
				{
					SqlHelper.BeginExecuteNonQuery(CountBussiness.ConnectionString, "Modify_Count_Content", new object[]
					{
						clientInfos["Application_Id"],
						clientInfos["Cpu"],
						clientInfos["OperSystem"],
						clientInfos["IP"],
						clientInfos["IPAddress"],
						clientInfos["NETCLR"],
						clientInfos["Browser"],
						clientInfos["ActiveX"],
						clientInfos["Cookies"],
						clientInfos["CSS"],
						clientInfos["Language"],
						clientInfos["Computer"],
						clientInfos["Platform"],
						clientInfos["Win16"],
						clientInfos["Win32"],
						clientInfos["Referry"],
						clientInfos["Redirect"],
						clientInfos["TimeSpan"],
						clientInfos["ScreenWidth"] + clientInfos["ScreenHeight"],
						clientInfos["Color"],
						clientInfos["Flash"],
						"Insert"
					});
				}
			}
			catch (Exception exception)
			{
				CountBussiness.log.Error("Insert Log Error!!!!", exception);
			}
		}

		// Token: 0x06000072 RID: 114 RVA: 0x0000BB48 File Offset: 0x00009D48
		public static void InsertGameInfo(DateTime begin, int mapID, int money, int gold, string users)
		{
			CountBussiness.InsertGameInfo(CountBussiness.AppID, CountBussiness.SubID, CountBussiness.ServerID, begin, DateTime.Now, users.Split(new char[]
			{
				','
			}).Length, mapID, money, gold, users);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000BB8C File Offset: 0x00009D8C
		public static void InsertGameInfo(int appid, int subid, int serverid, DateTime begin, DateTime end, int usercount, int mapID, int money, int gold, string users)
		{
			try
			{
				bool countRecord = CountBussiness.CountRecord;
				if (countRecord)
				{
					SqlHelper.BeginExecuteNonQuery(CountBussiness.ConnectionString, "SP_Insert_Count_FightInfo", new object[]
					{
						appid,
						subid,
						serverid,
						begin,
						end,
						usercount,
						mapID,
						money,
						gold,
						users
					});
				}
			}
			catch (Exception exception)
			{
				CountBussiness.log.Error("Insert Log Error!", exception);
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0000BC44 File Offset: 0x00009E44
		public static void InsertServerInfo(int usercount, int gamecount)
		{
			CountBussiness.InsertServerInfo(CountBussiness.AppID, CountBussiness.SubID, CountBussiness.ServerID, usercount, gamecount, DateTime.Now);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x0000BC64 File Offset: 0x00009E64
		public static void InsertServerInfo(int appid, int subid, int serverid, int usercount, int gamecount, DateTime time)
		{
			try
			{
				bool countRecord = CountBussiness.CountRecord;
				if (countRecord)
				{
					SqlHelper.BeginExecuteNonQuery(CountBussiness.ConnectionString, "SP_Insert_Count_Server", new object[]
					{
						appid,
						subid,
						serverid,
						usercount,
						gamecount,
						time
					});
				}
			}
			catch (Exception exception)
			{
				CountBussiness.log.Error("Insert Log Error!!", exception);
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x0000BCF8 File Offset: 0x00009EF8
		public static void InsertSystemPayCount(int consumerid, int money, int gold, int consumertype, int subconsumertype)
		{
			CountBussiness.InsertSystemPayCount(CountBussiness.AppID, CountBussiness.SubID, consumerid, money, gold, consumertype, subconsumertype, DateTime.Now);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x0000BD18 File Offset: 0x00009F18
		public static void InsertSystemPayCount(int appid, int subid, int consumerid, int money, int gold, int consumertype, int subconsumertype, DateTime datime)
		{
			try
			{
				bool countRecord = CountBussiness.CountRecord;
				if (countRecord)
				{
					SqlHelper.BeginExecuteNonQuery(CountBussiness.ConnectionString, "SP_Insert_Count_SystemPay", new object[]
					{
						appid,
						subid,
						consumerid,
						money,
						gold,
						consumertype,
						subconsumertype,
						datime
					});
				}
			}
			catch (Exception exception)
			{
				CountBussiness.log.Error("InsertSystemPayCount Log Error!!!", exception);
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x0000BDC0 File Offset: 0x00009FC0
		public static void SetConfig(string connectionString, int appID, int subID, int serverID, bool countRecord)
		{
			CountBussiness._connectionString = connectionString;
			CountBussiness._appID = appID;
			CountBussiness._subID = subID;
			CountBussiness._serverID = serverID;
			CountBussiness._conutRecord = countRecord;
		}

		// Token: 0x04000070 RID: 112
		private static int _appID;

		// Token: 0x04000071 RID: 113
		private static string _connectionString;

		// Token: 0x04000072 RID: 114
		private static bool _conutRecord;

		// Token: 0x04000073 RID: 115
		private static int _serverID;

		// Token: 0x04000074 RID: 116
		private static int _subID;

		// Token: 0x04000075 RID: 117
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
	}
}
