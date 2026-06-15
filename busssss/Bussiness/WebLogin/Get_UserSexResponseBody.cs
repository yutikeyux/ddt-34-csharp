using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Bussiness.WebLogin
{
	// Token: 0x02000028 RID: 40
	[DebuggerStepThrough]
	[GeneratedCode("System.ServiceModel", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	[DataContract(Namespace = "dandantang")]
	public class Get_UserSexResponseBody
	{
		// Token: 0x0600027C RID: 636 RVA: 0x00033CA6 File Offset: 0x00031EA6
		public Get_UserSexResponseBody()
		{
		}

		// Token: 0x0600027D RID: 637 RVA: 0x00033CB0 File Offset: 0x00031EB0
		public Get_UserSexResponseBody(bool? Get_UserSexResult)
		{
			this.Get_UserSexResult = Get_UserSexResult;
		}

		// Token: 0x040000FE RID: 254
		[DataMember(Order = 0)]
		public bool? Get_UserSexResult;
	}
}
