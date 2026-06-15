using System;
using Bussiness.WebLogin;

namespace Bussiness.Interface
{
	// Token: 0x02000049 RID: 73
	public class SRInterface : BaseInterface
	{
		// Token: 0x060003AC RID: 940 RVA: 0x0003CF74 File Offset: 0x0003B174
		public override bool GetUserSex(string name)
		{
			bool result;
			try
			{
				result = new PassPortSoapClient().Get_UserSex(string.Empty, name).Value;
			}
			catch (Exception exception)
			{
				BaseInterface.log.Error("获取性别失败", exception);
				result = true;
			}
			return result;
		}
	}
}
