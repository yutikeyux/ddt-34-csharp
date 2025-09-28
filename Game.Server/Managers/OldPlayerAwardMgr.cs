using Bussiness;
using Bussiness.Managers;
using Game.Server.Buffer;
using Game.Server.Packets;
using log4net;
using SqlDataProvider.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Game.Server.Managers
{
    public class OldPlayerAwardMgr
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static List<OldPlayerAwardInfo> oldPlayerAwards;

        public static bool Init()
        {
            try
            {
                using (ProduceBussiness pb = new ProduceBussiness())
                {
                    oldPlayerAwards = pb.GetAllOldPlayerAward().ToList();
                    foreach(OldPlayerAwardInfo oldPlayerAward in oldPlayerAwards)
                    {
                        oldPlayerAward.itemInfo = ItemInfo.CreateFromTemplate(ItemMgr.FindItemTemplate(oldPlayerAward.RewardItemID), oldPlayerAward.RewardItemCount, 105);
                        oldPlayerAward.itemInfo.IsBinds = oldPlayerAward.IsBind;
                        oldPlayerAward.itemInfo.ValidDate = oldPlayerAward.RewardItemValid;
                        oldPlayerAward.itemInfo.StrengthenLevel = oldPlayerAward.StrengthenLevel;
                        oldPlayerAward.itemInfo.AttackCompose = oldPlayerAward.AttackCompose;
                        oldPlayerAward.itemInfo.LuckCompose = oldPlayerAward.LuckCompose;
                        oldPlayerAward.itemInfo.AgilityCompose = oldPlayerAward.AgilityCompose;
                        oldPlayerAward.itemInfo.DefendCompose = oldPlayerAward.DefendCompose;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled)
                {
                    log.Error("OldPlayerAwardMgr", ex);
                }
            }
            return false;
        }
    }
}
