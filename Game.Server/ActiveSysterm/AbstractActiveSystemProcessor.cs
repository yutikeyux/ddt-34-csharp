using Game.Base.Packets;

namespace Game.Server.ActiveSystem
{
    public abstract class AbstractActiveSystemProcessor : IActiveSystemProcessor
    {
        public virtual void OnGameData(GamePlayer player, GSPacketIn packet)
        {
        }
    }
}
