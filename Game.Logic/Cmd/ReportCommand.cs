using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bussiness.Managers;
using Game.Base.Packets;
using Game.Logic;
using Game.Logic.Phy.Object;
using SqlDataProvider.Data;

namespace Game.Logic.Cmd
{
    [GameCommand((int)eTankCmdType.REPORT_COMMAND, "战胜关卡中Boss翻牌")]
    public class ReportCommand : ICommandHandler
    {
        public void HandleCommand(BaseGame game, Player player, GSPacketIn packet)
        {
            string reportCommand = packet.ReadString();
            if (reportCommand == "!report")
            {
                player.PlayerDetail.SendMessage("Test report của Lâm Gay");
            }
            if (game is PVPGame)
            {
                PVPGame pvp = game as PVPGame;
                
                //GSPacketIn pkg = new GSPacketIn((byte)ePackageTypeLogic.GAME_CMD, player.Id);
                //pkg.WriteInt((int)eTankCmdType.REPORT_COMMAND);
                //game.SendToAll(pkg);
            }
        }
    }
}