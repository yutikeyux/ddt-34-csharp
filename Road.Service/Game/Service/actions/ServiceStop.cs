using System;
using System.Collections;
using System.ServiceProcess;

namespace Game.Service.actions
{
	// Token: 0x0200000D RID: 13
	public class ServiceStop : IAction
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000AD RID: 173 RVA: 0x0000E9E0 File Offset: 0x0000CBE0
		public string Name
		{
			get
			{
				return "--servicestop";
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000AE RID: 174 RVA: 0x0000E9F8 File Offset: 0x0000CBF8
		public string Syntax
		{
			get
			{
				return "--servicestop";
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000AF RID: 175 RVA: 0x0000EA10 File Offset: 0x0000CC10
		public string Description
		{
			get
			{
				return "Stops the DOL system service";
			}
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x0000EA28 File Offset: 0x0000CC28
		public void OnAction(Hashtable parameters)
		{
			ServiceController dolservice = GameServerService.GetDOLService();
			bool flag = dolservice == null;
			bool flag2 = flag;
			if (flag2)
			{
				Console.WriteLine("You have to install the service first!");
			}
			else
			{
				bool flag3 = dolservice.Status == ServiceControllerStatus.StartPending;
				bool flag4 = flag3;
				if (flag4)
				{
					Console.WriteLine("Server is still starting, please check the logfile for progress information!");
				}
				else
				{
					bool flag5 = dolservice.Status != ServiceControllerStatus.Running;
					bool flag6 = flag5;
					if (flag6)
					{
						Console.WriteLine("The DOL service is not running");
					}
					else
					{
						try
						{
							Console.WriteLine("Stopping the DOL service...");
							dolservice.Stop();
							dolservice.WaitForStatus(ServiceControllerStatus.Stopped);
							Console.WriteLine("Finished!");
						}
						catch (InvalidOperationException ex)
						{
							Console.WriteLine("Could not stop the DOL service!");
							Console.WriteLine(ex.Message);
						}
					}
				}
			}
		}
	}
}
