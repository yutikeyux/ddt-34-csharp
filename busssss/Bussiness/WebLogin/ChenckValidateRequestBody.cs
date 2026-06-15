using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Bussiness.WebLogin
{
	// Token: 0x02000022 RID: 34
	[DebuggerStepThrough]
	[GeneratedCode("System.ServiceModel", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	[DataContract(Namespace = "dandantang")]
	public class ChenckValidateRequestBody
	{
		// Token: 0x06000270 RID: 624 RVA: 0x00033BEF File Offset: 0x00031DEF
		public ChenckValidateRequestBody()
		{
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00033BF9 File Offset: 0x00031DF9
		public ChenckValidateRequestBody(string applicationname, string username, string password)
		{
			this.applicationname = applicationname;
			this.username = username;
			this.password = password;
		}

		// Token: 0x040000F5 RID: 245
		[DataMember(EmitDefaultValue = false, Order = 0)]
		public string applicationname;

		// Token: 0x040000F6 RID: 246
		[DataMember(EmitDefaultValue = false, Order = 1)]
		public string username;

		// Token: 0x040000F7 RID: 247
		[DataMember(EmitDefaultValue = false, Order = 2)]
		public string password;
	}
}
