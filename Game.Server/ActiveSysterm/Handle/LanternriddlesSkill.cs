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
    [ActiveSystemHandleAttbute((byte)ActiveSystemPackageType.LANTERNRIDDLES_SKILL)]
    public class LanternriddlesSkill : IActiveSystemCommandHadler
    {
        public bool CommandHandler(GamePlayer Player, GSPacketIn packet)
        {
            GSPacketIn pkg = new GSPacketIn((byte)ePackageType.ACTIVITY_SYSTEM, Player.PlayerCharacter.ID);

            packet.ReadInt();//_loc_4.writeInt(param1);QuestionID
            packet.ReadInt();//_loc_4.writeInt(param2);QuestionIndex
            int option = packet.ReadInt();//_loc_4.writeInt(param3);type 1:double 0:hit
            packet.ReadBoolean();//_loc_5.writeBoolean(false);
            //LanternriddlesInfo Lanternriddles = ActiveSystemMgr.GetLanternriddles(Player.PlayerCharacter.ID);
            //if (Lanternriddles == null)
            //{
            //    Player.SendMessage(LanguageMgr.GetTranslation("ActiveSystemHandler.Msg9"));
            //    return false;
            //}
            //bool resultSkill = false;
            //int needMoney = 0;
            //if (option == 0)
            //{
            //    if (Lanternriddles.HitFreeCount > 0)
            //    {
            //        Lanternriddles.HitFreeCount--;
            //        Lanternriddles.IsHint = true;
            //        resultSkill = true;
            //    }
            //    else
            //    {
            //        needMoney = Lanternriddles.HitPrice;
            //        if (Player.ActiveMoneyEnable(needMoney))
            //        {
            //            Lanternriddles.IsHint = true;
            //            resultSkill = true;
            //        }
            //    }
            //}
            //else
            //{
            //    if (Lanternriddles.DoubleFreeCount > 0)
            //    {
            //        Lanternriddles.DoubleFreeCount--;
            //        Lanternriddles.IsDouble = true;
            //        resultSkill = true;
            //    }
            //    else
            //    {
            //        needMoney = Lanternriddles.DoublePrice;
            //        if (Player.ActiveMoneyEnable(needMoney))
            //        {
            //            Lanternriddles.IsDouble = true;
            //            resultSkill = true;
            //        }
            //    }

            //}
            //if (resultSkill)
            //{
            //    pkg.WriteByte((byte)LanternriddlesPackageType.LANTERNRIDDLES_SKILL);
            //    pkg.WriteBoolean(resultSkill);
            //    Player.Out.SendTCP(pkg);
            //    Player.SendMessage(LanguageMgr.GetTranslation("ActiveSystemHandler.Msg2"));
            //}
            return true;
			
        }		
    }
}
