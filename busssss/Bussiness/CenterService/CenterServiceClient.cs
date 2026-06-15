using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Bussiness.CenterService
{
	// Token: 0x0200004C RID: 76
	[DebuggerStepThrough]
	[GeneratedCode("System.ServiceModel", "4.0.0.0")]
	public class CenterServiceClient : ClientBase<ICenterService>, ICenterService
	{
		// Token: 0x060003B1 RID: 945 RVA: 0x0003D016 File Offset: 0x0003B216
		public CenterServiceClient()
		{
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0003D020 File Offset: 0x0003B220
		public CenterServiceClient(string endpointConfigurationName) : base(endpointConfigurationName)
		{
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0003D02B File Offset: 0x0003B22B
		public CenterServiceClient(string endpointConfigurationName, string remoteAddress) : base(endpointConfigurationName, remoteAddress)
		{
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0003D037 File Offset: 0x0003B237
		public CenterServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress) : base(endpointConfigurationName, remoteAddress)
		{
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0003D043 File Offset: 0x0003B243
		public CenterServiceClient(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress)
		{
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0003D050 File Offset: 0x0003B250
		public ServerData[] GetServerList()
		{
			return base.Channel.GetServerList();
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0003D070 File Offset: 0x0003B270
		public bool ChargeMoney(int userID, string chargeID)
		{
			return base.Channel.ChargeMoney(userID, chargeID);
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0003D090 File Offset: 0x0003B290
		public bool SystemNotice(string msg)
		{
			return base.Channel.SystemNotice(msg);
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0003D0B0 File Offset: 0x0003B2B0
		public bool KitoffUser(int playerID, string msg)
		{
			return base.Channel.KitoffUser(playerID, msg);
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0003D0D0 File Offset: 0x0003B2D0
		public bool ReLoadServerList()
		{
			return base.Channel.ReLoadServerList();
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0003D0F0 File Offset: 0x0003B2F0
		public bool MailNotice(int playerID)
		{
			return base.Channel.MailNotice(playerID);
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0003D110 File Offset: 0x0003B310
		public bool ActivePlayer(bool isActive)
		{
			return base.Channel.ActivePlayer(isActive);
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0003D130 File Offset: 0x0003B330
		public bool CreatePlayer(int id, string name, string password, bool isFirst)
		{
			return base.Channel.CreatePlayer(id, name, password, isFirst);
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0003D154 File Offset: 0x0003B354
		public bool ValidateLoginAndGetID(string name, string password, int zoneId, ref int userID, ref bool isFirst)
		{
			return base.Channel.ValidateLoginAndGetID(name, password, zoneId, ref userID, ref isFirst);
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0003D178 File Offset: 0x0003B378
		public bool AASUpdateState(bool state)
		{
			return base.Channel.AASUpdateState(state);
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0003D198 File Offset: 0x0003B398
		public int AASGetState()
		{
			return base.Channel.AASGetState();
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0003D1B8 File Offset: 0x0003B3B8
		public int ExperienceRateUpdate(int serverId)
		{
			return base.Channel.ExperienceRateUpdate(serverId);
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0003D1D8 File Offset: 0x0003B3D8
		public int NoticeServerUpdate(int serverId, int type)
		{
			return base.Channel.NoticeServerUpdate(serverId, type);
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0003D1F8 File Offset: 0x0003B3F8
		public bool UpdateConfigState(int type, bool state)
		{
			return base.Channel.UpdateConfigState(type, state);
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0003D218 File Offset: 0x0003B418
		public int GetConfigState(int type)
		{
			return base.Channel.GetConfigState(type);
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0003D238 File Offset: 0x0003B438
		public bool Reload(string type)
		{
			return base.Channel.Reload(type);
		}
	}
}
