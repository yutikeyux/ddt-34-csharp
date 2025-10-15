using System;
using System.Collections;
using System.Configuration.Install;
using System.Reflection;

namespace Game.Service.actions
{
	// Token: 0x0200000E RID: 14
	public class ServiceUninstall : IAction
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x0000EAFC File Offset: 0x0000CCFC
		public string Name
		{
			get
			{
				return "--serviceuninstall";
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x0000EB14 File Offset: 0x0000CD14
		public string Syntax
		{
			get
			{
				return "--serviceuninstall";
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x0000EB2C File Offset: 0x0000CD2C
		public string Description
		{
			get
			{
				return "Uninstalls the DOL system service";
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x0000EB44 File Offset: 0x0000CD44
		public void OnAction(Hashtable parameters)
		{
			AssemblyInstaller assemblyInstaller = new AssemblyInstaller(Assembly.GetExecutingAssembly(), new string[]
			{
				"/LogToConsole=false"
			});
			Hashtable savedState = new Hashtable();
			bool flag = GameServerService.GetDOLService() == null;
			bool flag2 = flag;
			if (flag2)
			{
				Console.WriteLine("No service named \"DOL\" found!");
			}
			else
			{
				Console.WriteLine("Uninstalling DOL system service...");
				try
				{
					assemblyInstaller.Uninstall(savedState);
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error uninstalling system service");
					Console.WriteLine(ex.Message);
					return;
				}
				Console.WriteLine("Finished!");
			}
		}
	}
}
