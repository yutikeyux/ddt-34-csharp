using System;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using Game.Server;

namespace Game.Service
{
	// Token: 0x02000003 RID: 3
	public class GameServerService : ServiceBase
	{
		// Token: 0x06000005 RID: 5 RVA: 0x000020C4 File Offset: 0x000002C4
		public GameServerService()
		{
			base.ServiceName = "ROAD";
			base.AutoLog = false;
			base.CanHandlePowerEvent = false;
			base.CanPauseAndContinue = false;
			base.CanShutdown = true;
			base.CanStop = true;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002104 File Offset: 0x00000304
		private static bool StartServer()
		{
			Directory.SetCurrentDirectory(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName);
			new FileInfo("./config/serverconfig.xml");
			GameServer.CreateInstance(new GameServerConfig());
			return GameServer.Instance.Start();
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002150 File Offset: 0x00000350
		private static void StopServer()
		{
			GameServer.Instance.Stop();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002160 File Offset: 0x00000360
		protected override void OnStart(string[] args)
		{
			bool flag = !GameServerService.StartServer();
			bool flag2 = flag;
			if (flag2)
			{
				throw new ApplicationException("Failed to start server!");
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002188 File Offset: 0x00000388
		protected override void OnStop()
		{
			GameServerService.StopServer();
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002188 File Offset: 0x00000388
		protected override void OnShutdown()
		{
			GameServerService.StopServer();
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002194 File Offset: 0x00000394
		public static ServiceController GetDOLService()
		{
			ServiceController[] services = ServiceController.GetServices();
			ServiceController[] array = services;
			foreach (ServiceController serviceController in array)
			{
				bool flag = serviceController.ServiceName.ToLower().Equals("ROAD");
				bool flag2 = flag;
				if (flag2)
				{
					return serviceController;
				}
			}
			return null;
		}
	}
}
