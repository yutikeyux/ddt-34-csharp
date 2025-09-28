using System;
using Game.Base.Packets;
using Game.Server.WonderFul;

namespace Game.Server.Packets.Client
{
	// Token: 0x02000717 RID: 1815
	[PacketHandler(373, "Coleta de Recompensas Wonder")]
	public class WonderfulActivityGetRewardHandler : IPacketHandler
	{
		// Token: 0x0600405D RID: 16477 RVA: 0x001C9E10 File Offset: 0x001C8010
		public int HandlePacket(GameClient client, GSPacketIn packet)
		{
			WonderFulActivityManager.SendWonderFulReward(client.Player, packet);
			return 0;
		}
	}
}
