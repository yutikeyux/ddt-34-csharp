using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace Bussiness.WebLogin
{
	// Token: 0x02000027 RID: 39
	[DebuggerStepThrough]
	[GeneratedCode("System.ServiceModel", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	[MessageContract(IsWrapped = false)]
	public class Get_UserSexResponse
	{
		// Token: 0x0600027A RID: 634 RVA: 0x00033C8B File Offset: 0x00031E8B
		public Get_UserSexResponse()
		{
		}

		// Token: 0x0600027B RID: 635 RVA: 0x00033C95 File Offset: 0x00031E95
		public Get_UserSexResponse(Get_UserSexResponseBody Body)
		{
			this.Body = Body;
		}

		// Token: 0x040000FD RID: 253
		[MessageBodyMember(Name = "Get_UserSexResponse", Namespace = "dandantang", Order = 0)]
		public Get_UserSexResponseBody Body;
	}
}
