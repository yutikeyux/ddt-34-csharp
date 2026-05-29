using SqlDataProvider.Data;

namespace Game.Server.Quests
{
    /// <summary>
    /// Quest CondictionType = 61
    /// Kaplica/Hot Spring'de vakit gecirme gorevi. Para2 = hedef dakika
    /// </summary>
    public class HotSpringTimeQuestCondition : BaseCondition
    {
        public HotSpringTimeQuestCondition(BaseQuest quest, QuestConditionInfo info, int value)
            : base(quest, info, value)
        {
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.HotSpingExpAdd += Player_HotSpringExpAdd;
        }

        public override bool IsCompleted(GamePlayer player)
        {
            return base.Value <= 0;
        }

        private void Player_HotSpringExpAdd(int minutes, int exp)
        {
            if (base.Value > 0)
            {
                base.Value -= minutes;
                if (base.Value < 0) base.Value = 0;
            }
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.HotSpingExpAdd -= Player_HotSpringExpAdd;
        }
    }
}
