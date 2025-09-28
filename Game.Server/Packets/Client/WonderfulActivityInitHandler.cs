using System;
using Game.Base.Packets;
using Game.Server.WonderFul;

namespace Game.Server.Packets.Client
{
	// Token: 0x02000719 RID: 1817
	[PacketHandler(405, "Evento")]
	public class WonderfulActivityInitHandler : IPacketHandler
	{
		// Token: 0x06004061 RID: 16481 RVA: 0x001C9E9C File Offset: 0x001C809C
		public int HandlePacket(GameClient client, GSPacketIn packet)
		{
			WonderFulActivityManager.WonderFulActivityInit(client.Player, packet.ReadInt());
			return 0;
		}
	}
}
