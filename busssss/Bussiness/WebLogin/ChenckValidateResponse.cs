using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace Bussiness.WebLogin
{
	// Token: 0x02000023 RID: 35
	[DebuggerStepThrough]
	[GeneratedCode("System.ServiceModel", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	[MessageContract(IsWrapped = false)]
	public class ChenckValidateResponse
	{
		// Token: 0x06000272 RID: 626 RVA: 0x00033C18 File Offset: 0x00031E18
		public ChenckValidateResponse()
		{
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00033C22 File Offset: 0x00031E22
		public ChenckValidateResponse(ChenckValidateResponseBody Body)
		{
			this.Body = Body;
		}

		// Token: 0x040000F8 RID: 248
		[MessageBodyMember(Name = "ChenckValidateResponse", Namespace = "dandantang", Order = 0)]
		public ChenckValidateResponseBody Body;
	}
}
