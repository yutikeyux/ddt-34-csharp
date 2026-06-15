using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Bussiness.WebLogin
{
	// Token: 0x02000024 RID: 36
	[DebuggerStepThrough]
	[GeneratedCode("System.ServiceModel", "4.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	[DataContract(Namespace = "dandantang")]
	public class ChenckValidateResponseBody
	{
		// Token: 0x06000274 RID: 628 RVA: 0x00033C33 File Offset: 0x00031E33
		public ChenckValidateResponseBody()
		{
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00033C3D File Offset: 0x00031E3D
		public ChenckValidateResponseBody(string ChenckValidateResult)
		{
			this.ChenckValidateResult = ChenckValidateResult;
		}

		// Token: 0x040000F9 RID: 249
		[DataMember(EmitDefaultValue = false, Order = 0)]
		public string ChenckValidateResult;
	}
}
