using Game.Base.Packets;

namespace Game.Server.EliteGame
{
    public interface IEliteGameProcessor
    {
        void OnGameData(GamePlayer player, GSPacketIn packet);
    }
}
