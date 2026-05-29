using SqlDataProvider.Data;

namespace Game.Server.Quests
{
    /// <summary>
    /// Quest CondictionType = 58
    /// Kart takma gorevi. Para2 = kac kez kart takilmali
    /// </summary>
    public class EquipCardQuestCondition : BaseCondition
    {
        public EquipCardQuestCondition(BaseQuest quest, QuestConditionInfo info, int value)
            : base(quest, info, value)
        {
        }

        public override void AddTrigger(GamePlayer player)
        {
            player.EquipCardEvent += Player_EquipCard;
        }

        public override bool IsCompleted(GamePlayer player)
        {
            return base.Value <= 0;
        }

        private void Player_EquipCard()
        {
            if (base.Value > 0)
            {
                base.Value--;
            }
        }

        public override void RemoveTrigger(GamePlayer player)
        {
            player.EquipCardEvent -= Player_EquipCard;
        }
    }
}
