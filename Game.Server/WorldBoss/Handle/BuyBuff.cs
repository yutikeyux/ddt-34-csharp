using Bussiness;
using Game.Base.Packets;
using Game.Server.Buffer;
using Game.Server.GameObjects;
using Game.Server.Packets;
using Game.Server.Rooms;

namespace Game.Server.WorldBoss.Handle
{
    [WorldBossHandle((byte)WorldBossGamePackageType.BUFF_BUY)]
    public class BuyBuff : IWorldBossCommandHandler
    {
        public int CommandHandler(GamePlayer Player, GSPacketIn packet)
        {
            int damevalue = RoomMgr.WorldBossRoom.addInjureValue;

            AbstractBuffer buffer = BufferList.CreatePayBuffer((int)BuffType.WorldBossAttrack_MoneyBuff, damevalue, 1);
            if (buffer != null)
            {
                buffer.Start(Player);
                Player.SendMessage("Buff başarıyla uygulandı!");
            }

            return 0;
        }
    }
}