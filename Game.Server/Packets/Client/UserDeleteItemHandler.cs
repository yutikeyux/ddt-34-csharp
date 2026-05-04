using Bussiness;
using Game.Base.Packets;

namespace Game.Server.Packets.Client
{
    [PacketHandler(42, "删除物品")]
    public class UserDeleteItemHandler : IPacketHandler
    {
        public int HandlePacket(GameClient client, GSPacketIn packet)
        {
            if (client.Player.PlayerCharacter.HasBagPassword && client.Player.PlayerCharacter.IsLocked)
            {
                _ = client.Out.SendMessage(eMessageType.GM_NOTICE, LanguageMgr.GetTranslation("Bag.Locked"));
                return 0;
            }
            _ = packet.ReadByte();
            _ = packet.ReadInt();
            return 0;
        }
    }
}
