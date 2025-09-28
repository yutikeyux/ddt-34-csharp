using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Server.GameObjects;
using Game.Base.Packets;
using Game.Server.Packets;

namespace Game.Server.ActiveSystem
{
    public interface IActiveSystemProcessor
    {
        void OnGameData(GamePlayer player, GSPacketIn packet);
    }
}
