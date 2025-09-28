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
    [ActiveSystemHandleAttbute((byte)ActiveSystemPackageType.CHRISTMAS_BUY_TIMER)]
    public class ChristmasBuyTimer : IActiveSystemCommandHadler
    {
        public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.ACTIVITY_SYSTEM, Player.PlayerCharacter.ID);
            int needMoney = GameProperties.ChristmasBuyTimeMoney;
            if (Player.MoneyDirect(needMoney, false, false, true))
            {
                int min = GameProperties.ChristmasBuyMinute;
                Player.Actives.AddTime(min);
                Player.SendMessage(LanguageMgr.GetTranslation("ActiveSystemHandler.Msg2"));
            }
            return true;

        }
    }
}
