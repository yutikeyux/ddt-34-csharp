using Game.Base.Packets;

namespace Game.Server.EliteGame
{
    public class EliteGameProcessor
    {
        private static object _syncStop = new object();
        private IEliteGameProcessor _processor;
        public EliteGameProcessor(IEliteGameProcessor processor)
        {
            _processor = processor;
        }
        public void ProcessData(GamePlayer player, GSPacketIn data)
        {
            lock (_syncStop)
            {
                _processor.OnGameData(player, data);
            }
        }
    }
}
