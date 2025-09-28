using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Server.GameObjects;
using Game.Server.Packets.Client;
using Game.Base.Packets;

namespace Game.Server.EliteGame.Handle
{
    public interface IEliteGameCommandHadler
    {
        bool CommandHandler(GamePlayer Player, GSPacketIn packet);
    }
}
