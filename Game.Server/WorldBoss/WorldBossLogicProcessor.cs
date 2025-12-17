using System;
using System.Reflection;
using Bussiness;
using Game.Base.Packets;
using Game.Server.GameObjects;
using Game.Server.Packets;
using Game.Server.WorldBoss.Handle;
using log4net;

namespace Game.Server.WorldBoss
{
    [WorldBoss((byte)ePackageType.WORLDBOSS_CMD, "礼堂逻辑")]
    public class WorldBossLogicProcessor : AbstractWorldBossProcessor
    {
        // 1. DEĞİŞTİ: Constructor ve _commandMgr alanı SİLİNDİ.
        // Artık 'new WorldBossHandleMgr()' çağrılmıyor.

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override void OnGameData(GamePlayer player, GSPacketIn packet)
        {
            var type = (WorldBossPackageType)packet.ReadByte();
            try
            {
                // 2. DEĞİŞTİ: Doğrudan static 'Instance' üzerinden çağrılıyor.
                var commandHandler = WorldBossHandleMgr.Instance.LoadCommandHandler((int)type);

                if (commandHandler != null)
                {
                    commandHandler.CommandHandler(player, packet);
                    if (GameProperties.DebugMode)
                    {
                        Console.WriteLine(commandHandler);
                    }
                }
                else
                {
                    // Artık çökmeyecek, sadece bu logu basacak
                    Console.WriteLine("______________ERROR______________");
                    Console.WriteLine("WorldBossLogicProcessor WorldBossPackageType.{0} not found!", type);
                    Console.WriteLine("_______________END_______________");
                }
            }
            catch (Exception e)
            {
                log.Error(string.Format("WorldBossLogicProcessor PackageType:{1}, OnGameData is Error: {0}", e,
                    type));
                log.Error($"IP: {player.Client.TcpEndpoint}");
            }
        }
    }
}