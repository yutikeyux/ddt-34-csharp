using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;

namespace Bussiness.WebLogin
{
	// Token: 0x02000021 RID: 33
	[DebuggerStepThrough]
	[GeneratedCode("System.ServiceModel", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	[MessageContract(IsWrapped = false)]
	public class ChenckValidateRequest
	{
		// Token: 0x0600026E RID: 622 RVA: 0x00033BD4 File Offset: 0x00031DD4
		public ChenckValidateRequest()
		{
		}

		// Token: 0x0600026F RID: 623 RVA: 0x00033BDE File Offset: 0x00031DDE
		public ChenckValidateRequest(ChenckValidateRequestBody Body)
		{
			this.Body = Body;
		}

		// Token: 0x040000F4 RID: 244
		[MessageBodyMember(Name = "ChenckValidate", Namespace = "dandantang", Order = 0)]
		public ChenckValidateRequestBody Body;
	}
}
