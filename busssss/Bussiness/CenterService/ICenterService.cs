using System;
using System.CodeDom.Compiler;
using System.ServiceModel;

namespace Bussiness.CenterService
{
	// Token: 0x0200004D RID: 77
	[GeneratedCode("System.ServiceModel", "4.0.0.0")]
	[ServiceContract(ConfigurationName = "CenterService.ICenterService")]
	public interface ICenterService
	{
		// Token: 0x060003C6 RID: 966
		[OperationContract(Action = "http://tempuri.org/ICenterService/GetServerList", ReplyAction = "http://tempuri.org/ICenterService/GetServerListResponse")]
		ServerData[] GetServerList();

		// Token: 0x060003C7 RID: 967
		[OperationContract(Action = "http://tempuri.org/ICenterService/ChargeMoney", ReplyAction = "http://tempuri.org/ICenterService/ChargeMoneyResponse")]
		bool ChargeMoney(int userID, string chargeID);

		// Token: 0x060003C8 RID: 968
		[OperationContract(Action = "http://tempuri.org/ICenterService/SystemNotice", ReplyAction = "http://tempuri.org/ICenterService/SystemNoticeResponse")]
		bool SystemNotice(string msg);

		// Token: 0x060003C9 RID: 969
		[OperationContract(Action = "http://tempuri.org/ICenterService/KitoffUser", ReplyAction = "http://tempuri.org/ICenterService/KitoffUserResponse")]
		bool KitoffUser(int playerID, string msg);

		// Token: 0x060003CA RID: 970
		[OperationContract(Action = "http://tempuri.org/ICenterService/ReLoadServerList", ReplyAction = "http://tempuri.org/ICenterService/ReLoadServerListResponse")]
		bool ReLoadServerList();

		// Token: 0x060003CB RID: 971
		[OperationContract(Action = "http://tempuri.org/ICenterService/MailNotice", ReplyAction = "http://tempuri.org/ICenterService/MailNoticeResponse")]
		bool MailNotice(int playerID);

		// Token: 0x060003CC RID: 972
		[OperationContract(Action = "http://tempuri.org/ICenterService/ActivePlayer", ReplyAction = "http://tempuri.org/ICenterService/ActivePlayerResponse")]
		bool ActivePlayer(bool isActive);

		// Token: 0x060003CD RID: 973
		[OperationContract(Action = "http://tempuri.org/ICenterService/CreatePlayer", ReplyAction = "http://tempuri.org/ICenterService/CreatePlayerResponse")]
		bool CreatePlayer(int id, string name, string password, bool isFirst);

		// Token: 0x060003CE RID: 974
		[OperationContract(Action = "http://tempuri.org/ICenterService/ValidateLoginAndGetID", ReplyAction = "http://tempuri.org/ICenterService/ValidateLoginAndGetIDResponse")]
		bool ValidateLoginAndGetID(string name, string password, int zoneId, ref int userID, ref bool isFirst);

		// Token: 0x060003CF RID: 975
		[OperationContract(Action = "http://tempuri.org/ICenterService/AASUpdateState", ReplyAction = "http://tempuri.org/ICenterService/AASUpdateStateResponse")]
		bool AASUpdateState(bool state);

		// Token: 0x060003D0 RID: 976
		[OperationContract(Action = "http://tempuri.org/ICenterService/AASGetState", ReplyAction = "http://tempuri.org/ICenterService/AASGetStateResponse")]
		int AASGetState();

		// Token: 0x060003D1 RID: 977
		[OperationContract(Action = "http://tempuri.org/ICenterService/ExperienceRateUpdate", ReplyAction = "http://tempuri.org/ICenterService/ExperienceRateUpdateResponse")]
		int ExperienceRateUpdate(int serverId);

		// Token: 0x060003D2 RID: 978
		[OperationContract(Action = "http://tempuri.org/ICenterService/NoticeServerUpdate", ReplyAction = "http://tempuri.org/ICenterService/NoticeServerUpdateResponse")]
		int NoticeServerUpdate(int serverId, int type);

		// Token: 0x060003D3 RID: 979
		[OperationContract(Action = "http://tempuri.org/ICenterService/UpdateConfigState", ReplyAction = "http://tempuri.org/ICenterService/UpdateConfigStateResponse")]
		bool UpdateConfigState(int type, bool state);

		// Token: 0x060003D4 RID: 980
		[OperationContract(Action = "http://tempuri.org/ICenterService/GetConfigState", ReplyAction = "http://tempuri.org/ICenterService/GetConfigStateResponse")]
		int GetConfigState(int type);

		// Token: 0x060003D5 RID: 981
		[OperationContract(Action = "http://tempuri.org/ICenterService/Reload", ReplyAction = "http://tempuri.org/ICenterService/ReloadResponse")]
		bool Reload(string type);
	}
}
