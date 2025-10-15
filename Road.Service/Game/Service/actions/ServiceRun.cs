using System;
using System.Collections;
using System.ServiceProcess;

namespace Game.Service.actions
{
	// Token: 0x0200000B RID: 11
	public class ServiceRun : IAction
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x0000E83C File Offset: 0x0000CA3C
		public string Name
		{
			get
			{
				return "--SERVICERUN";
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x0000E854 File Offset: 0x0000CA54
		public string Syntax
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x0000E868 File Offset: 0x0000CA68
		public string Description
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0000E87B File Offset: 0x0000CA7B
		public void OnAction(Hashtable parameters)
		{
			ServiceBase.Run(new GameServerService());
		}
	}
}
