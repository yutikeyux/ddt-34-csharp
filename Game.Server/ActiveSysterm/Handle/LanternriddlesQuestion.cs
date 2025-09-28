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
using Game.Server.Managers;

namespace Game.Server.ActiveSystem.Handle
{
    [ActiveSystemHandleAttbute((byte)ActiveSystemPackageType.LANTERNRIDDLES_QUESTION)]
    public class LanternriddlesQuestion : IActiveSystemCommandHadler
    {
        public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
        {
            //LanternriddlesInfo Lanternriddles = ActiveSystemMgr.EnterLanternriddles(Player.PlayerCharacter.ID);
            //Player.Actives.SendLightriddleQuestion(Lanternriddles);
            //if (!Player.Actives.LightriddleStart && Lanternriddles.CanNextQuest)
            //{
            //    Player.Actives.BeginLightriddleTimer();
            //}
            return true;
			
        }		
    }
}
