using System;
using System.Collections.Generic;
using System.Linq;
using Game.Base.Packets;
using Game.Logic;
using Game.Server.Packets;

namespace Game.Server.EliteGame.Handle
{
    [EliteGameHandleAttbute((byte)EliteGamePackageType.ELITE_MATCH_TYPE)]
    public class EliteMatchType : IEliteGameCommandHadler
    {
        public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.ELITEGAME);
            pkg.WriteByte((byte)EliteGamePackageType.ELITE_MATCH_TYPE);
            pkg.WriteInt(ExerciseMgr.EliteStatus);
            Player.Out.SendTCP(pkg);
            return true;
        }
    }
}