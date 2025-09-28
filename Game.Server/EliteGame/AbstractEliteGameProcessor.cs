using Game.Base.Packets;

namespace Game.Server.EliteGame
{
    public abstract class AbstractEliteGameProcessor : IEliteGameProcessor
    {
        public virtual void OnGameData(GamePlayer player, GSPacketIn packet)
        {
        }
    }
}
