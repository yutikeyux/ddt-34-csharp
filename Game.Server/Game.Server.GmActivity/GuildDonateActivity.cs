using System.Collections.Generic;
using System.Linq;
using Game.Server.GameObjects;
using Game.Server.Managers;
using SqlDataProvider.Data;

namespace Game.Server.GmActivity
{
    /// <summary>
    /// Lonca Bagis Etkinligi (activityType = 34)
    /// Oyuncu loncaya belirli miktarda bagis yaptiginda odul verir.
    /// DB'de activityType=34 olarak kaydedilir.
    /// </summary>
    public class GuildDonateActivity : BaseUserGmActivity
    {
        private GamePlayer Player;

        public GuildDonateActivity(GamePlayer player, GmActivityInfo gmActivityInfo, List<UserGmActivityCondition> userConditions, List<UserGmActivityReward> userRewards)
            : base(player, gmActivityInfo, userConditions, userRewards)
        {
            Player = player;
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.Riches += Player_Riches;
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.Riches -= Player_Riches;
        }

        private void Player_Riches(int value, int type)
        {
            SetValue(value);
        }

        public override void SetValue(int value)
        {
            List<GmGiftInfo> list = GmActivityMgr.FindGmGifts(ActivityInfo.activityId);
            foreach (GmGiftInfo item in list)
            {
                GmActiveConditionInfo gmCondition = GmActivityMgr.FindGmActiveCondition(item.giftbagId).First();
                UserGmActivityCondition userCondition = UserConditions.Find((UserGmActivityCondition t) => t.GiftBagID == gmCondition.giftbagId);
                if (userCondition == null)
                {
                    userCondition = new UserGmActivityCondition
                    {
                        UserID = Player.PlayerId,
                        ActivityID = ActivityInfo.activityId,
                        GiftBagID = gmCondition.giftbagId,
                        StatusID = 0,
                        StatusValue = 0
                    };
                    UserConditions.Add(userCondition);
                }
                userCondition.StatusValue += value;
                userCondition.StatusID = gmCondition.conditionValue;
            }
        }

        public override string CanGetRewardMsg(string giftId, ref int times)
        {
            string result = "ok";
            List<GmActiveConditionInfo> list = GmActivityMgr.FindGmActiveCondition(giftId);
            UserGmActivityCondition userCondition = UserConditions.Find((UserGmActivityCondition a) => a.GiftBagID == giftId);
            UserGmActivityReward userReward = UserRewards.Find((UserGmActivityReward a) => a.GiftBagID == giftId);

            if (userCondition == null || list == null || list.Count == 0)
            {
                return "Kosullar gecersiz!";
            }
            if (userCondition.StatusValue < list[0].conditionValue)
            {
                return "Yeterli bagis yapmadiniz!";
            }
            if (userReward != null && userReward.Times > 0)
            {
                return "Bu odulu zaten aldiniz.";
            }
            return result;
        }
    }
}
