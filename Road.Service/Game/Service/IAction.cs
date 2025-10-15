using System;
using System.Collections;

namespace Game.Service
{
	// Token: 0x02000005 RID: 5
	internal interface IAction
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000E RID: 14
		string Name { get; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000F RID: 15
		string Syntax { get; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000010 RID: 16
		string Description { get; }

		// Token: 0x06000011 RID: 17
		void OnAction(Hashtable parameters);
	}
}
