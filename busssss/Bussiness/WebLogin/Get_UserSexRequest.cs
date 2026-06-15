using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace Bussiness.WebLogin
{
	// Token: 0x02000025 RID: 37
	[DebuggerStepThrough]
	[GeneratedCode("System.ServiceModel", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	[MessageContract(IsWrapped = false)]
	public class Get_UserSexRequest
	{
		// Token: 0x06000276 RID: 630 RVA: 0x00033C4E File Offset: 0x00031E4E
		public Get_UserSexRequest()
		{
		}

		// Token: 0x06000277 RID: 631 RVA: 0x00033C58 File Offset: 0x00031E58
		public Get_UserSexRequest(Get_UserSexRequestBody Body)
		{
			this.Body = Body;
		}

		// Token: 0x040000FA RID: 250
		[MessageBodyMember(Name = "Get_UserSex", Namespace = "dandantang", Order = 0)]
		public Get_UserSexRequestBody Body;
	}
}
