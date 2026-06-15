using System;
using System.CodeDom.Compiler;
using System.ServiceModel;

namespace Bussiness.WebLogin
{
	// Token: 0x02000029 RID: 41
	[GeneratedCode("System.ServiceModel", "4.0.0.0")]
	[ServiceContract(Namespace = "dandantang", ConfigurationName = "WebLogin.PassPortSoap")]
	public interface PassPortSoap
	{
		// Token: 0x0600027E RID: 638
		[OperationContract(Action = "dandantang/ChenckValidate", ReplyAction = "*")]
		ChenckValidateResponse ChenckValidate(ChenckValidateRequest request);

		// Token: 0x0600027F RID: 639
		[OperationContract(Action = "dandantang/Get_UserSex", ReplyAction = "*")]
		Get_UserSexResponse Get_UserSex(Get_UserSexRequest request);
	}
}
