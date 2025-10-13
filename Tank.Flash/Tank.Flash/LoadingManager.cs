using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;

namespace Tank.Flash
{
	// Token: 0x0200000E RID: 14
	public class LoadingManager
	{
		// Token: 0x0600002F RID: 47 RVA: 0x00002924 File Offset: 0x00000B24
		public static void Setup()
		{
			LoadingManager.m_timeout = int.Parse(ConfigurationManager.AppSettings["LoginSessionTimeOut"]);
			LoadingManager.m_timer = new Timer(new TimerCallback(LoadingManager.CheckTimerCallback), null, 0, 60000);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x0000295C File Offset: 0x00000B5C
		protected static bool CheckTimeOut(DateTime dt)
		{
			return (DateTime.Now - dt).TotalMinutes > (double)LoadingManager.m_timeout;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002984 File Offset: 0x00000B84
		private static void CheckTimerCallback(object state)
		{
			lock (LoadingManager.sys_obj)
			{
				List<string> list = new List<string>();
				foreach (LoadingManager.PlayerData playerData in LoadingManager.m_players.Values)
				{
					if (LoadingManager.CheckTimeOut(playerData.Date))
					{
						list.Add(playerData.Name);
					}
				}
				foreach (string key in list)
				{
					LoadingManager.m_players.Remove(key);
				}
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002A64 File Offset: 0x00000C64
		public static void Add(string name, string pass)
		{
			LoadingManager.Add(name, pass, false);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002A70 File Offset: 0x00000C70
		public static void Add(string name, string pass, bool isAdmin)
		{
			lock (LoadingManager.sys_obj)
			{
				if (LoadingManager.m_players.ContainsKey(name))
				{
					LoadingManager.m_players[name].Name = name;
					LoadingManager.m_players[name].Pass = pass;
					LoadingManager.m_players[name].Date = DateTime.Now;
					LoadingManager.m_players[name].Count = 0;
					LoadingManager.m_players[name].IsAdmin = isAdmin;
				}
				else
				{
					LoadingManager.PlayerData playerData = new LoadingManager.PlayerData();
					playerData.Name = name;
					playerData.Pass = pass;
					playerData.Date = DateTime.Now;
					playerData.IsAdmin = isAdmin;
					LoadingManager.m_players.Add(name, playerData);
				}
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002B44 File Offset: 0x00000D44
		public static bool Login(string name, string pass)
		{
			bool result;
			lock (LoadingManager.sys_obj)
			{
				if (LoadingManager.m_players.ContainsKey(name) && LoadingManager.m_players[name].Pass == pass)
				{
					LoadingManager.PlayerData playerData = LoadingManager.m_players[name];
					if (playerData.Pass == pass && !LoadingManager.CheckTimeOut(playerData.Date))
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

		// Token: 0x06000035 RID: 53 RVA: 0x00002BD4 File Offset: 0x00000DD4
		public static bool Update(string name, string pass)
		{
			lock (LoadingManager.sys_obj)
			{
				if (LoadingManager.m_players.ContainsKey(name))
				{
					LoadingManager.m_players[name].Pass = pass;
					LoadingManager.m_players[name].Count++;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002C4C File Offset: 0x00000E4C
		public static bool UpdateKey(string name, string key)
		{
			lock (LoadingManager.sys_obj)
			{
				if (LoadingManager.m_players.ContainsKey(name))
				{
					LoadingManager.m_players[name].Key = key;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002CAC File Offset: 0x00000EAC
		public static bool Remove(string name)
		{
			bool result;
			lock (LoadingManager.sys_obj)
			{
				result = LoadingManager.m_players.Remove(name);
			}
			return result;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002CF4 File Offset: 0x00000EF4
		public static bool CheckUser(string name)
		{
			lock (LoadingManager.sys_obj)
			{
				if (LoadingManager.m_players.ContainsKey(name))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002D44 File Offset: 0x00000F44
		public static bool GetByUserIsFirst(string name)
		{
			lock (LoadingManager.sys_obj)
			{
				if (LoadingManager.m_players.ContainsKey(name))
				{
					return LoadingManager.m_players[name].Count == 0;
				}
			}
			return false;
		}

		// Token: 0x0400000D RID: 13
		public static int LoadingCount = 0;

		// Token: 0x0400000E RID: 14
		private static Dictionary<string, LoadingManager.PlayerData> m_players = new Dictionary<string, LoadingManager.PlayerData>();

		// Token: 0x0400000F RID: 15
		private static object sys_obj = new object();

		// Token: 0x04000010 RID: 16
		private static Timer m_timer;

		// Token: 0x04000011 RID: 17
		private static int m_timeout = 30;

		// Token: 0x0200000F RID: 15
		private class PlayerData
		{
			// Token: 0x04000012 RID: 18
			public string Name;

			// Token: 0x04000013 RID: 19
			public string Pass;

			// Token: 0x04000014 RID: 20
			public string Key;

			// Token: 0x04000015 RID: 21
			public DateTime Date;

			// Token: 0x04000016 RID: 22
			public int Count;

			// Token: 0x04000017 RID: 23
			public bool IsAdmin;
		}
	}
}
