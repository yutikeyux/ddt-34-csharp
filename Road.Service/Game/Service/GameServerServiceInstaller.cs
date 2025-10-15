using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace Game.Service
{
	// Token: 0x02000004 RID: 4
	[RunInstaller(true)]
	public class GameServerServiceInstaller : Installer
	{
		// Token: 0x0600000C RID: 12 RVA: 0x000021F0 File Offset: 0x000003F0
		public GameServerServiceInstaller()
		{
			this.m_gameServerServiceProcessInstaller = new ServiceProcessInstaller();
			this.m_gameServerServiceProcessInstaller.Account = ServiceAccount.LocalSystem;
			this.m_gameServerServiceInstaller = new ServiceInstaller();
			this.m_gameServerServiceInstaller.StartType = ServiceStartMode.Manual;
			this.m_gameServerServiceInstaller.ServiceName = "ROAD";
			base.Installers.Add(this.m_gameServerServiceProcessInstaller);
			base.Installers.Add(this.m_gameServerServiceInstaller);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000226C File Offset: 0x0000046C
		public override void Install(IDictionary stateSaver)
		{
			StringDictionary parameters = base.Context.Parameters;
			parameters["assemblyPath"] = parameters["assemblyPath"] + " --SERVICERUN " + base.Context.Parameters["commandline"];
			base.Install(stateSaver);
		}

		// Token: 0x04000003 RID: 3
		private ServiceInstaller m_gameServerServiceInstaller;

		// Token: 0x04000004 RID: 4
		private ServiceProcessInstaller m_gameServerServiceProcessInstaller;
	}
}
