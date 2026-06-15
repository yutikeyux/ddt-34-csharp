using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Bussiness.WebLogin
{
	// Token: 0x02000026 RID: 38
	[DebuggerStepThrough]
	[GeneratedCode("System.ServiceModel", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	[DataContract(Namespace = "dandantang")]
	public class Get_UserSexRequestBody
	{
		// Token: 0x06000278 RID: 632 RVA: 0x00033C69 File Offset: 0x00031E69
		public Get_UserSexRequestBody()
		{
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00033C73 File Offset: 0x00031E73
		public Get_UserSexRequestBody(string applicationname, string username)
		{
			this.applicationname = applicationname;
			this.username = username;
		}

		// Token: 0x040000FB RID: 251
		[DataMember(EmitDefaultValue = false, Order = 0)]
		public string applicationname;

		// Token: 0x040000FC RID: 252
		[DataMember(EmitDefaultValue = false, Order = 1)]
		public string username;
	}
}
