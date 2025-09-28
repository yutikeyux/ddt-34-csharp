using Game.Base.Packets;

namespace Game.Server.ActiveSystem
{
    public class ActiveSystemProcessor
    {
        private static object _syncStop = new object();
        private IActiveSystemProcessor _processor;
        public ActiveSystemProcessor(IActiveSystemProcessor processor)
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
