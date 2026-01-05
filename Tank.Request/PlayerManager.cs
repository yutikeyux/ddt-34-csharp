using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Threading;
using log4net;

namespace Tank.Request
{
	// Token: 0x02000066 RID: 102
	public class PlayerManager
	{
		// Token: 0x060001D0 RID: 464 RVA: 0x00002A3E File Offset: 0x00000C3E
		public static void Setup()
		{
			PlayerManager.m_timeout = int.Parse(ConfigurationManager.AppSettings["LoginSessionTimeOut"]);
			PlayerManager.m_timer = new Timer(new TimerCallback(PlayerManager.CheckTimerCallback), null, 0, 60000);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000DE68 File Offset: 0x0000C068
		protected static bool CheckTimeOut(DateTime dt)
		{
			return (DateTime.Now - dt).TotalMinutes > (double)PlayerManager.m_timeout;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000DE98 File Offset: 0x0000C098
		private static void CheckTimerCallback(object state)
		{
			object obj = PlayerManager.sys_obj;
			lock (obj)
			{
				List<string> list = new List<string>();
				foreach (PlayerManager.PlayerData p in PlayerManager.m_players.Values)
				{
					bool flag2 = PlayerManager.CheckTimeOut(p.Date);
					if (flag2)
					{
						list.Add(p.Name);
					}
				}
				foreach (string name in list)
				{
					PlayerManager.m_players.Remove(name);
				}
			}
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000DF8C File Offset: 0x0000C18C
		public static void Add(string name, string pass)
		{
			object obj = PlayerManager.sys_obj;
			lock (obj)
			{
				bool flag2 = PlayerManager.m_players.ContainsKey(name);
				if (flag2)
				{
					PlayerManager.m_players[name].Name = name;
					PlayerManager.m_players[name].Pass = pass;
					PlayerManager.m_players[name].Date = DateTime.Now;
					PlayerManager.m_players[name].Count = 0;
				}
				else
				{
					PlayerManager.PlayerData data = new PlayerManager.PlayerData();
					data.Name = name;
					data.Pass = pass;
					data.Date = DateTime.Now;
					PlayerManager.m_players.Add(name, data);
				}
			}
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000E054 File Offset: 0x0000C254
		public static bool Login(string name, string pass)
		{
			object obj = PlayerManager.sys_obj;
			bool result;
			lock (obj)
			{
				bool flag2 = PlayerManager.m_players.ContainsKey(name);
				if (flag2)
				{
					PlayerManager.PlayerData p = PlayerManager.m_players[name];
					bool flag3 = p.Pass == pass && !PlayerManager.CheckTimeOut(p.Date);
					if (flag3)
					{
						result = true;
					}
					else
					{
						result = false;
					}
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000E0E4 File Offset: 0x0000C2E4
		public static bool Update(string name, string pass)
		{
			object obj = PlayerManager.sys_obj;
			lock (obj)
			{
				bool flag2 = PlayerManager.m_players.ContainsKey(name);
				if (flag2)
				{
					PlayerManager.m_players[name].Pass = pass;
					PlayerManager.m_players[name].Count++;
					return true;
				}
			}
			return false;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000E164 File Offset: 0x0000C364
		public static bool Remove(string name)
		{
			object obj = PlayerManager.sys_obj;
			bool result;
			lock (obj)
			{
				result = PlayerManager.m_players.Remove(name);
			}
			return result;
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000E1B0 File Offset: 0x0000C3B0
		public static bool GetByUserIsFirst(string name)
		{
			object obj = PlayerManager.sys_obj;
			lock (obj)
			{
				bool flag2 = PlayerManager.m_players.ContainsKey(name);
				if (flag2)
				{
					return PlayerManager.m_players[name].Count == 0;
				}
			}
			return false;
		}

		// Token: 0x04000068 RID: 104
		private static Dictionary<string, PlayerManager.PlayerData> m_players = new Dictionary<string, PlayerManager.PlayerData>();

		// Token: 0x04000069 RID: 105
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// Token: 0x0400006A RID: 106
		private static object sys_obj = new object();

		// Token: 0x0400006B RID: 107
		private static Timer m_timer;

		// Token: 0x0400006C RID: 108
		private static int m_timeout = 30;

		// Token: 0x02000067 RID: 103
		private class PlayerData
		{
			// Token: 0x0400006D RID: 109
			public string Name;

			// Token: 0x0400006E RID: 110
			public string Pass;

			// Token: 0x0400006F RID: 111
			public DateTime Date;

			// Token: 0x04000070 RID: 112
			public int Count;
		}
	}
}
