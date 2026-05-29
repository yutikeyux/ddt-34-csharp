using System.Collections.Generic;
using System.Linq;
using Game.Server.GameObjects;
using Game.Server.Managers;
using SqlDataProvider.Data;

namespace Game.Server.GmActivity
{
    /// <summary>
    /// Seviye Atlama Etkinligi (activityType = 35)
    /// Oyuncu belirli seviyeye ulastiginda odul verir.
    /// DB'de activityType=35 olarak kaydedilir.
    /// </summary>
    public class LevelUpActivity : BaseUserGmActivity
    {
        private GamePlayer Player;

        public LevelUpActivity(GamePlayer player, GmActivityInfo gmActivityInfo, List<UserGmActivityCondition> userConditions, List<UserGmActivityReward> userRewards)
            : base(player, gmActivityInfo, userConditions, userRewards)
        {
            Player = player;
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.LevelUp += Player_LevelUp;
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.LevelUp -= Player_LevelUp;
        }

        private void Player_LevelUp(GamePlayer player)
        {
            SetValue(player.PlayerCharacter.Grade);
        }

        public override void SetValue(int value)
        {
            List<GmGiftInfo> list = GmActivityMgr.FindGmGifts(ActivityInfo.activityId);
            foreach (GmGiftInfo gmGift in list)
            {
                UserGmActivityCondition userCondition = UserConditions.Find((UserGmActivityCondition t) => t.GiftBagID == gmGift.giftbagId);
                if (userCondition == null)
                {
                    userCondition = new UserGmActivityCondition
                    {
                        UserID = Player.PlayerId,
                        ActivityID = ActivityInfo.activityId,
                        GiftBagID = gmGift.giftbagId,
                        StatusID = 0,
                        StatusValue = 0
                    };
                    UserConditions.Add(userCondition);
                }
                if (value > userCondition.StatusValue)
                {
                    userCondition.StatusValue = value;
                }
                GmActiveConditionInfo gmActiveCondition = GmActivityMgr.FindGmActiveCondition(gmGift.giftbagId).Find((GmActiveConditionInfo x) => x.conditionIndex == 0);
                if (gmActiveCondition != null && value >= gmActiveCondition.conditionValue)
                {
                    userCondition.StatusID = 1;
                }
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
                return "Gerekli seviyeye ulasmadiniz!";
            }
            if (userReward != null && userReward.Times > 0)
            {
                return "Bu odulu zaten aldiniz.";
            }
            return result;
        }
    }
}
