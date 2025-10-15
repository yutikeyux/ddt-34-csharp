using System;
using System.Collections;
using System.ServiceProcess;

namespace Game.Service.actions
{
	// Token: 0x0200000C RID: 12
	public class ServiceStart : IAction
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x0000E88C File Offset: 0x0000CA8C
		public string Name
		{
			get
			{
				return "--servicestart";
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x0000E8A4 File Offset: 0x0000CAA4
		public string Syntax
		{
			get
			{
				return "--servicestart";
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000AA RID: 170 RVA: 0x0000E8BC File Offset: 0x0000CABC
		public string Description
		{
			get
			{
				return "Starts the DOL system service";
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x0000E8D4 File Offset: 0x0000CAD4
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
					bool flag5 = dolservice.Status != ServiceControllerStatus.Stopped;
					bool flag6 = flag5;
					if (flag6)
					{
						Console.WriteLine("The DOL service is not stopped");
					}
					else
					{
						try
						{
							Console.WriteLine("Starting the DOL service...");
							dolservice.Start();
							dolservice.WaitForStatus(ServiceControllerStatus.StartPending, TimeSpan.FromSeconds(10.0));
							Console.WriteLine("Starting can take some time, please check the logfile for progress information!");
							Console.WriteLine("Finished!");
						}
						catch (InvalidOperationException ex)
						{
							Console.WriteLine("Could not start the DOL service!");
							Console.WriteLine(ex.Message);
						}
						catch (System.ServiceProcess.TimeoutException)
						{
							Console.WriteLine("Error starting the service, please check the logfile for further info!");
						}
					}
				}
			}
		}
	}
}
