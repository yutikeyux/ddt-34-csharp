using SqlDataProvider.Data;

namespace Game.Server.Quests
{
    public class SendGiftForFriendCondition : BaseCondition
    {
        public SendGiftForFriendCondition(BaseQuest quest, QuestConditionInfo info, int value)
			: base(quest, info, value)
        {
        }

        public override void AddTrigger(GamePlayer player)
        {
            //player.SendGiftForFriend += player_SendGiftForFriend;

        }

        public override bool IsCompleted(GamePlayer player)
        {
			return base.Value <= 0;
        }

        private void player_SendGiftForFriend()
        {
			if (base.Value > 0)
			{
				base.Value--;
			}
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            //player.SendGiftForFriend -= player_SendGiftForFriend;
        }
    }
}
