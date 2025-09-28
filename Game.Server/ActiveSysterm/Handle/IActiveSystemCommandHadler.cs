using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Server.GameObjects;
using Game.Server.Packets.Client;
using Game.Base.Packets;

namespace Game.Server.ActiveSystem.Handle
{
    public interface IActiveSystemCommandHadler
    {
        bool CommandHandler(GamePlayer Player, GSPacketIn packet);
    }
}
