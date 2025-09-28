using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Server.Packets;
using Game.Server.GameObjects;
using Game.Server.Packets.Client;
using Game.Base.Packets;
using SqlDataProvider.Data;
using Bussiness;

namespace Game.Server.ActiveSystem.Handle
{
    [ActiveSystemHandleAttbute((byte)ActiveSystemPackageType.LANTERNRIDDLES_RANKINFO)]
    public class LanternriddlesRankInfo : IActiveSystemCommandHadler
    {
        public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
        {
            //GameServer.Instance.LoginServer.SendLightriddleRank(Player.PlayerCharacter.NickName, Player.PlayerCharacter.ID);
            return true;
			
        }		
    }
}
