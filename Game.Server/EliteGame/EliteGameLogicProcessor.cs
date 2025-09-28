using System;
using Game.Base.Packets;
using Game.Server.EliteGame.Handle;
using System.Reflection;
using log4net;

namespace Game.Server.EliteGame
{
    [EliteGameProcessorAtribute(255, "礼堂逻辑")]
    public class EliteGameLogicProcessor : AbstractEliteGameProcessor
    {
        public EliteGameLogicProcessor()
        {
            _commandMgr = new EliteGameHandleMgr();
        }
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private EliteGameHandleMgr _commandMgr;
        public override void OnGameData(GamePlayer player, GSPacketIn packet)
        {
            EliteGamePackageType type = (EliteGamePackageType)packet.ReadByte();
            try
            {
                IEliteGameCommandHadler commandHandler = _commandMgr.LoadCommandHandler((int)type);
                if (commandHandler != null)
                {
                    commandHandler.CommandHandler(player, packet);
                    //Console.WriteLine("EliteGamePackageType: {0}!", type);
                }
                else
                {
                    Console.WriteLine("______________ERROR______________");
                    Console.WriteLine("EliteGamePackageType: {0} not found!", type);
                    Console.WriteLine("_______________END_______________");
                }
            }
            catch (Exception e)
            {
                log.Error(string.Format("EliteGamePackageType: {1}, OnGameData is Error: {0}", e, type));
            }
        }
    }
}
